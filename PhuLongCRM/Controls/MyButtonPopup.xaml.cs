using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace PhuLongCRM.Controls
{
    /// <summary>
    ///     MyButtonPopup control
    ///         The control is used to show and hide button any time, any where on a form
    ///         The control include: a image button, styles to style button, animation hide and show button, click button event
    ///         The control was created by Anh Phong
    /// </summary>
    public partial class MyButtonPopup : ContentView
    {
        public MyButtonPopup()
        {
            InitializeComponent();

            //// Define default Location of button (98% x 98%)
            ///  Defuen defaul size of button (80 x 80)
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(.98, .98, 80, 80));
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
            this.BackgroundColor = Color.Transparent;
            this.Padding = 10;

            //this.border.BorderColor = Color.FromRgba(0, 0, 0, 0.15);
            this.main_image.BindingContext = this;
            this.main_frame.BindingContext = this;

            this.main_image.Clicked += (sender, e) => { this.sendOnClicked(); };

            this.unFocus();
        }

        public static readonly BindableProperty SourceImageProperty = BindableProperty.Create(nameof(SourceImage), typeof(ImageSource), typeof(MyButtonPopup), ImageSource.FromFile("btn_send.png"), BindingMode.TwoWay);
        public static readonly BindableProperty BackgroundContentColorProperty = BindableProperty.Create(nameof(BackgroundContentColor), typeof(Color), typeof(MyButtonPopup), Color.White, BindingMode.TwoWay);
        public static readonly BindableProperty BorderContentColorProperty = BindableProperty.Create(nameof(BorderContentColor), typeof(Color), typeof(MyButtonPopup), Color.Gray, BindingMode.TwoWay);
        public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(MyButtonPopup), new Thickness(5), BindingMode.TwoWay);
        public static readonly BindableProperty isVisibleProperty = BindableProperty.Create(nameof(isVisible), typeof(bool), typeof(MyButtonPopup), false, BindingMode.TwoWay, propertyChanged : isVisiblePropertyChanged);
        public static readonly BindableProperty radiusCornerProperty = BindableProperty.Create(nameof(radiusCorner), typeof(double), typeof(MyButtonPopup), 30.0, BindingMode.TwoWay);
        public static readonly BindableProperty hideToLeftProperty = BindableProperty.Create(nameof(hideToLeft), typeof(bool), typeof(MyButtonPopup), false, BindingMode.TwoWay, propertyChanged : hideToLeftPropertyChanged);

        /// <summary>
        ///     SourceImage is a bindable variable to binding source of image of ImageButton. Default image of button is
        ///         image btn_send.png. BindableProperty of this is SourceImageProperty.
        /// </summary>
        /// <value>The source image.</value>
        public ImageSource SourceImage
        {
            get { return (ImageSource)GetValue(SourceImageProperty); }
            set { SetValue(SourceImageProperty, value); }
        }
        /// <summary>
        ///    BackgroundContentColor is a bindable variable to binding background color of Button. 
        ///         BindableProperty of this is BackgroundContentColorProperty.
        /// </summary>
        /// <value>The color of the background content.</value>
        public Color BackgroundContentColor
        {
            get { return (Color)GetValue(BackgroundContentColorProperty); }
            set { SetValue(BackgroundContentColorProperty, value); }
        }
        /// <summary>
        ///    BorderContentColor is a bindable variable to binding Border color of button. 
        ///         BindableProperty of this is BorderContentColorProperty.
        /// </summary>
        /// <value>The color of the border content.</value>
        public Color BorderContentColor
        {
            get { return (Color)GetValue(BorderContentColorProperty); }
            set { SetValue(BorderContentColorProperty, value); }
        }
        /// <summary>
        ///    ContentPadding is a bindable variable to binding padding of imagebutton to button. 
        ///         BindableProperty of this is ContentPaddingProperty.
        /// </summary>
        /// <value>The content padding.</value>
        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }
        /// <summary>
        ///     isVisible is a bindable variable to binding status of button (show or hide). 
        ///         BindableProperty of this is isVisibleProperty.
        /// 
        /// ==> WARNING :::: isVisible (with animation) and IsVisible (without animation) is different
        /// 
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public bool isVisible
        {
            get { return (bool)GetValue(isVisibleProperty); }
            set { SetValue(isVisibleProperty, value); }
        }
        /// <summary>
        ///     radiusCorner is a bindable variable to binding corner radius of button, it can define button is a circle or a square. 
        ///         BindableProperty of this is radiusCornerProperty.
        /// </summary>
        /// <value>The radius corner.</value>
        public double radiusCorner
        {
            get { return (double)GetValue(radiusCornerProperty); }
            set { SetValue(radiusCornerProperty, value); }
        }
        /// <summary>
        ///     hideToLeft is a bindable variable to binding status of hideSide of button.
        ///         Default status is hide to Right (mean hideToLeft is false) => button will hide to right side. 
        ///         If hideToLeft is modified to true, the button will hide to left side
        ///         BindableProperty of this is radiusCornerProperty.
        /// </summary>
        /// <value><c>true</c> if hide to left; otherwise, <c>false</c>.</value>
        public bool hideToLeft
        {
            get { return (bool)GetValue(hideToLeftProperty); }
            set { SetValue(hideToLeftProperty, value); this.unFocus(); }
        }

        /// <summary>
        ///     sizeRadius is used to define size of button
        /// </summary>
        private double _sizeRadius;
        public double sizeRadius
        {
            get => _sizeRadius;
            set { _sizeRadius = value;
                var position = AbsoluteLayout.GetLayoutBounds(this).Location;
                var size = new Size(value, value);
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle() { Size = size, Location = position });
                //this.unFocus();
             }
        }


        /// <summary>
        ///     location is used to define location of button
        /// </summary>
        private Point _location;
        public Point location
        {
            get => _location;
            set
            {
                _location = value;
                var size = AbsoluteLayout.GetLayoutBounds(this).Size;
                AbsoluteLayout.SetLayoutBounds(this, new Rectangle() { Size = size, Location = value});
                //this.unFocus();
            }
        }


        /// <summary>
        ///     Listen isVisible property changed to show or hide button
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void isVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (MyButtonPopup)bindable;
            if((bool)newValue)
            {
                control.focus();
            }
            else
            {
                control.unFocus();
            }
        }

        /// <summary>
        ///     Listen hideTolEft property changed to hide button to the side that has been defined
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void hideToLeftPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (MyButtonPopup)bindable;
            control.unFocus();
        }

        /// <summary>
        ///     Click event of button
        /// </summary>
        public event EventHandler OnClicked;
        private void sendOnClicked()
        {
            EventHandler eventHandler = this.OnClicked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }


        /// <summary>
        ///     show button with animation
        /// </summary>
        public void focus()
        {
            this.IsVisible = true;
            this.Content.TranslateTo(0, 0, 500, Easing.CubicInOut);
        }


        /// <summary>
        ///     hide button with animation
        /// </summary>
        public void unFocus()
        {
            var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            var distance = screenWidth * ((hideToLeft?0:1) - AbsoluteLayout.GetLayoutBounds(this).Location.X);
            this.Content.TranslateTo((hideToLeft ? -1 : 1) *AbsoluteLayout.GetLayoutBounds(this).Size.Width  + distance, 0, 500, Easing.CubicInOut);
            //this.IsVisible = false;
        }
    }
}
