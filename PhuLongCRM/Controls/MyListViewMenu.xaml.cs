using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    /// <summary>
    ///     My list view menu control   
    ///         The control to show a menu from bottom
    ///         Content of control is a listview, and can define any template in it
    ///         The control was created by Anh Phong
    /// </summary>
    public partial class MyListViewMenu : ContentView
    {
        public MyListViewMenu()
        {
            InitializeComponent();

            this.IsVisible = false;

            //// Alway hide menu at the bottom.
            this.main_content.TranslateTo(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) * 0.25, 0, Easing.CubicInOut);

            //// Set location for menu.
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            this.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);

            this.title_label.BindingContext = this;
            this.header_line.BindingContext = this;
            this.menu_listview.BindingContext = this;

            this.menu_listview.ItemTapped += (sender, e) => 
            { 
                this.sendItemTapped(e); 
            };
        }

        /// <summary>
        ///     Click anywhere outside the menu event, if clicked, menu will hide.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Handle_Tapped(object sender, System.EventArgs e)
        {
            this.unFocus();
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(MyListViewMenu), "Menu", BindingMode.TwoWay);
        public static readonly BindableProperty HasTitleProperty = BindableProperty.Create(nameof(HasTitle), typeof(bool), typeof(MyListViewMenu), true, BindingMode.TwoWay);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(MyListViewMenu), null, BindingMode.TwoWay);
        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(IEnumerable), typeof(MyListViewMenu), null, BindingMode.TwoWay);
        public static readonly BindableProperty isTapableProperty = BindableProperty.Create(nameof(isTapable), typeof(bool), typeof(MyListViewMenu), true, BindingMode.TwoWay);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(MyListViewMenu), null, BindingMode.TwoWay);

        /// <summary>
        ///     Title is a bindale variable to binding title of menu.
        ///     Bindable property of this is TitleProperty.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        ///     HasTitle is a bindale variable to binding status of title of menu (show or hide).
        ///     Bindable property of this is HasTitleProperty.
        /// </summary>
        /// <value><c>true</c> if has title; otherwise, <c>false</c>.</value>
        public bool HasTitle
        {
            get { return (bool)GetValue(HasTitleProperty); }
            set { SetValue(HasTitleProperty, value); }
        }

        /// <summary>
        ///     ItemTemplate is a bindale variable to binding Template of Listview.
        ///     Bindable property of this is ItemTemplateProperty.
        /// </summary>
        /// <value>The item template.</value>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        ///     ItemSource is a bindale variable to binding ItemSource for listview
        ///     Bindable property of this is ItemSourceProperty.
        /// </summary>
        /// <value>The item source.</value>
        public IEnumerable ItemSource
        {
            get { return (IEnumerable)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        /// <summary>
        ///     isTapable is a bindale variable to binding status tap of listview item of menu (enable or unenable)
        ///     Bindable property of this is isTapableProperty.
        /// </summary>
        /// <value><c>true</c> if is tapable; otherwise, <c>false</c>.</value>
        public bool isTapable
        {
            get { return (bool)GetValue(isTapableProperty); }
            set { SetValue(isTapableProperty, value); }
        }


        /// <summary>
        ///     Gets or sets the selected item of listview.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); this.menu_listview.SelectedItem = value == null ? null : this.menu_listview.SelectedItem; }
        }

        /// <summary>
        ///     Event tap item in listview
        /// </summary>
        public event EventHandler<ItemTappedEventArgs> ItemTapped;
        private void sendItemTapped(ItemTappedEventArgs e)
        {
            EventHandler<ItemTappedEventArgs> eventHandler = this.ItemTapped;
            eventHandler?.Invoke((object)this, e);
        }

        /// <summary>
        ///     Call this function to show menu with animation
        /// </summary>
        public async void focus()
        {
            this.IsVisible = true;
            await this.main_content.TranslateTo(0, 0, 500, Easing.CubicInOut);
        }

        /// <summary>
        ///     Call this function to hide menu with animation
        /// </summary>
        public async void unFocus()
        {
            await this.main_content.TranslateTo(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) * 0.25, 300, Easing.CubicInOut);
            this.IsVisible = false;
        }
    }
}
