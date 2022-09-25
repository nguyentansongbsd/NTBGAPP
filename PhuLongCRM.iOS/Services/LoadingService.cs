using System;
using UIKit;
using PhuLongCRM.IServices;
using PhuLongCRM.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(LoadingService))]
namespace PhuLongCRM.iOS.Services
{
    public class LoadingService : ILoadingService
    {
        UIView spinnerView;

        public void Init()
        {
            UIWindow window = UIApplication.SharedApplication.KeyWindow;

            var frame = window.Frame;

            spinnerView = new UIView(frame);
            spinnerView.Alpha = 1;
            spinnerView.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0.3f);

            var ai = new UIActivityIndicatorView();
            ai.Color = UIColor.White;
            ai.StartAnimating();
            ai.Center = spinnerView.Center;
            spinnerView.AddSubview(ai);
        }

        public void Show()
        {
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            if (spinnerView == null)
            {
                Init();
            }
            //var frame = window.Frame;
            //if (spinnerView.Frame.Width != frame.Width)
            //{
            //    spinnerView.Frame = frame;
            //}
            UIApplication.SharedApplication.KeyWindow.AddSubview(spinnerView);
        }

        public void Hide()
        {
            if (spinnerView == null) return;
            UIView.Animate(0.2, () =>
            {
                spinnerView.RemoveFromSuperview();
            });
        }

        public void Initilize()
        {

        }
    }
}
