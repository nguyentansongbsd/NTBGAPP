﻿using System;
using System.ComponentModel;
using System.Drawing;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;
using Foundation;
using PhuLongCRM.Controls;
using PhuLongCRM.iOS.Renderers;

[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]
namespace PhuLongCRM.iOS.Renderers
{
    public class ExtendedFrameRenderer : FrameRenderer, ITabStop
    {
        ShadowView _shadowView;
		UIView ITabStop.TabStop => this;

		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
				SetupLayer();
			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
				OverrideUserInterfaceStyle = UIKit.UIUserInterfaceStyle.Light;
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
			    e.PropertyName == Xamarin.Forms.Frame.BorderColorProperty.PropertyName ||
				e.PropertyName == Xamarin.Forms.Frame.HasShadowProperty.PropertyName ||
				e.PropertyName == Xamarin.Forms.Frame.CornerRadiusProperty.PropertyName ||
				e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
				SetupLayer();
		}

		public virtual void SetupLayer()
		{
			float cornerRadius = Element.CornerRadius;

			if (cornerRadius == -1f)
				cornerRadius = 5f; // default corner radius

			Layer.CornerRadius = cornerRadius;
			Layer.MasksToBounds = Layer.CornerRadius > 0;

			if (Element.BackgroundColor == Xamarin.Forms.Color.Default)
				Layer.BackgroundColor = UIColor.White.CGColor;
			else
				Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();

			if (Element.BorderColor == Xamarin.Forms.Color.Default)
				Layer.BorderColor = UIColor.Clear.CGColor;
			else
			{
				Layer.BorderColor = Element.BorderColor.ToCGColor();
				Layer.BorderWidth = 1;
			}

			//if (Element.HasShadow)
			//{
			//	if (_shadowView == null)
			//	{
			//		_shadowView = new ShadowView(Layer);
			//		SetNeedsLayout();
			//	}
			//	_shadowView.UpdateBackgroundColor();
			//	_shadowView.Layer.CornerRadius = Layer.CornerRadius;
			//	_shadowView.Layer.BorderColor = Layer.BorderColor;
			//	_shadowView.Hidden = !Element.IsVisible;
			//}
			//else
			//{
			//	if (_shadowView != null)
			//	{
			//		_shadowView.RemoveFromSuperview();
			//		_shadowView.Dispose();
			//		_shadowView = null;
			//	}
			//}
            if (_shadowView == null)
            {
                _shadowView = new ShadowView(Layer);
                SetNeedsLayout();
            }
            _shadowView.UpdateBackgroundColor();
            _shadowView.Layer.CornerRadius = Layer.CornerRadius;
            _shadowView.Layer.BorderColor = Layer.BorderColor;
            _shadowView.Hidden = !Element.IsVisible;




            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
			Layer.ShouldRasterize = true;
		}

		public override void LayoutSubviews()
		{
			if (_shadowView != null)
			{
				if (_shadowView.Superview == null)
					Superview.InsertSubviewBelow(_shadowView, this);

				_shadowView?.SetNeedsLayout();
			}
			base.LayoutSubviews();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (_shadowView != null)
				{
					_shadowView.RemoveFromSuperview();
					_shadowView.Dispose();
					_shadowView = null;
				}
			}
		}


		[Preserve(Conditional = true)]
		class ShadowView : UIView
		{
			CALayer _shadowee;
			CGRect _previousBounds;
			CGRect _previousFrame;

			[Preserve(Conditional = true)]
			public ShadowView(CALayer shadowee)
			{
				_shadowee = shadowee;
				//Layer.ShadowRadius = 5;
				Layer.ShadowColor = UIColor.Gray.CGColor;
				Layer.ShadowOpacity = 0.5f; 
				Layer.ShadowOffset = new CGSize(0.5f,2f);
                UserInteractionEnabled = false;
			}

			public void UpdateBackgroundColor()
			{
				//Putting a transparent background under any shadowee having a background with alpha < 1
				//Giving the Shadow a background of the same color when shadowee background == 1.
				//The latter will result in a 'darker' shadow as you would expect from something that
				//isn't transparent. This also mimics the look as it was before with non-transparent Frames.
				if (_shadowee.BackgroundColor.Alpha < 1)
					BackgroundColor = UIColor.Clear;
				else
					BackgroundColor = new UIColor(_shadowee.BackgroundColor);
			}

			public override void LayoutSubviews()
			{
				if (_shadowee.Bounds != _previousBounds || _shadowee.Frame != _previousFrame)
				{
					base.LayoutSubviews();
					SetBounds();
				}
			}

			void SetBounds()
			{
				Layer.Frame = _shadowee.Frame;
				Layer.Bounds = _shadowee.Bounds;
				_previousBounds = _shadowee.Bounds;
				_previousFrame = _shadowee.Frame;
			}
		}
    }
}