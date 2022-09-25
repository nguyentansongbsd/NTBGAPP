using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace PhuLongCRM.Controls
{
    public class BsdListView : ListView
    {
        public BsdListView()
        {
            Init();
        }

        public BsdListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            Init();
        }

        public void Init()
        {
            this.IsPullToRefreshEnabled = true;
            this.HasUnevenRows = true;
            this.SelectionMode = ListViewSelectionMode.None;
            this.SeparatorVisibility = SeparatorVisibility.None;
            this.BackgroundColor = Color.FromHex("#eeeeee");
            this.SetBinding(RefreshCommandProperty, new Binding("RefreshCommand"));
            this.SetBinding(IsRefreshingProperty, new Binding("IsRefreshing"));
            this.SetBinding(ItemsSourceProperty, new Binding("Data"));

            InfiniteScrollBehavior behavior = new InfiniteScrollBehavior();
            behavior.SetBinding(InfiniteScrollBehavior.IsLoadingMoreProperty, new Binding("IsLoadingMore"));
            this.Behaviors.Add(behavior);

            if (Device.RuntimePlatform == Device.Android)
            {
                SetUpFooterLayout();
            }
        }

        public void SetUpFooterLayout()
        {
            StackLayout stack = new StackLayout();
            stack.SetBinding(StackLayout.IsVisibleProperty, "IsLoadingMore");
            stack.Padding = new Thickness(0, 0, 0, 64);

            DataTrigger trigger = new DataTrigger(typeof(StackLayout))
            {
                Binding = new Binding()
                {
                    Path = "IsVisible",
                    Source = stack
                },
                Value = false
            };

            trigger.Setters.Add(new Setter()
            {
                Property = StackLayout.HeightRequestProperty,
                Value = 0
            });

            stack.Triggers.Add(trigger);

            ActivityIndicator activityIndicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 30,
                IsRunning = true,
                Margin = new Thickness(0, 5, 0, 0),
                Color = Color.FromHex("#333333")
            };

            stack.Children.Add(activityIndicator);

            this.Footer = stack;
        }
    }
}
