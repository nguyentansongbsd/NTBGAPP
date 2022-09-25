using System;
using System.Collections.Generic;
using PhuLongCRM.Helper;
using PhuLongCRM.Models;
using PhuLongCRM.Resources;
using PhuLongCRM.ViewModels;
using Xamarin.Forms;

namespace PhuLongCRM.Views
{
    public partial class LichLamViecTheoTuan : ContentPage
    {
        LichLamViecViewModel viewModel;
        public Action<bool> OnComplete;
        public static bool? NeedToRefresh = null;
        public LichLamViecTheoTuan()
        {
            InitializeComponent();

            this.BindingContext = viewModel = new LichLamViecViewModel();
            NeedToRefresh = false;
            loadData();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                viewModel.lstEvents?.Clear();
                await viewModel.loadAllActivities();
                viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);
                ActivityPopup.Refresh();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
        }

        public async void loadData()
        {
            viewModel.EntityName = "tasks";
            viewModel.entity = "task";
            VisualStateManager.GoToState(radBorderTask, "Active");
            VisualStateManager.GoToState(radBorderMeeting, "InActive");
            VisualStateManager.GoToState(radBorderPhoneCall, "InActive");
            VisualStateManager.GoToState(lblTask, "Active");
            VisualStateManager.GoToState(lblMeeting, "InActive");
            VisualStateManager.GoToState(lblPhoneCall, "InActive");

            await viewModel.loadAllActivities();
            Handle_DateSelected(null,EventArgs.Empty);
            datePicker.ReSetTime();
            if (viewModel.lstEvents != null && viewModel.lstEvents.Count > 0)
                OnComplete?.Invoke(true);
            else
                OnComplete?.Invoke(false);
        }

        private async void Task_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radBorderTask, "Active");
            VisualStateManager.GoToState(radBorderMeeting, "InActive");
            VisualStateManager.GoToState(radBorderPhoneCall, "InActive");
            VisualStateManager.GoToState(lblTask, "Active");
            VisualStateManager.GoToState(lblMeeting, "InActive");
            VisualStateManager.GoToState(lblPhoneCall, "InActive");

            if (viewModel.entity != "task")
            {
                viewModel.EntityName = "tasks";
                viewModel.entity = "task";
                viewModel.lstEvents.Clear();
                await viewModel.loadAllActivities();
                viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);
            }

            LoadingHelper.Hide();
        }

        private async void Meeting_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radBorderTask, "InActive");
            VisualStateManager.GoToState(radBorderMeeting, "Active");
            VisualStateManager.GoToState(radBorderPhoneCall, "InActive");
            VisualStateManager.GoToState(lblTask, "InActive");
            VisualStateManager.GoToState(lblMeeting, "Active");
            VisualStateManager.GoToState(lblPhoneCall, "InActive");

            if (viewModel.entity != "appointment")
            {
                viewModel.EntityName = "appointments";
                viewModel.entity = "appointment";
                viewModel.lstEvents.Clear();
                await viewModel.loadAllActivities();
                viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);
            }

            LoadingHelper.Hide();
        }

        private async void PhoneCall_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radBorderTask, "InActive");
            VisualStateManager.GoToState(radBorderMeeting, "InActive");
            VisualStateManager.GoToState(radBorderPhoneCall, "Active");
            VisualStateManager.GoToState(lblTask, "InActive");
            VisualStateManager.GoToState(lblMeeting, "InActive");
            VisualStateManager.GoToState(lblPhoneCall, "Active");

            if (viewModel.entity != "phonecall")
            {
                viewModel.EntityName = "phonecalls";
                viewModel.entity = "phonecall";
                viewModel.lstEvents.Clear();
                await viewModel.loadAllActivities();
                viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);
            }

            LoadingHelper.Hide();
        }

        private void Handle_DateSelected(System.Object sender, System.EventArgs e)
        {
            if (sender == null)
                viewModel.UpdateSelectedEventsForWeekView(DateTime.Now);
            else
                viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);
        }

        private async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { Language.tao_cong_viec, Language.tao_cuoc_hop, Language.tao_cuoc_goi };
            string asw = await DisplayActionSheet(Language.tuy_chon, Language.huy, null, options);
            if (asw == Language.tao_cong_viec)
            {
                await Navigation.PushAsync(new TaskForm());
            }
            else if (asw == Language.tao_cuoc_goi)
            {
                await Navigation.PushAsync(new PhoneCallForm());
            }
            else if (asw == Language.tao_cuoc_hop)
            {
                await Navigation.PushAsync(new MeetingForm());
            }
            LoadingHelper.Hide();
        }

        async void Event_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var val = e.Item as CalendarEvent;
            if (val != null && val.Activity != null)
            {
                ActivityPopup.ShowActivityPopup(val.Activity.activityid, val.Activity.activitytypecode);
            }
        }

        private void ActivityPopup_HidePopupActivity(object sender, EventArgs e)
        {
            OnAppearing();
        }
    }
}
