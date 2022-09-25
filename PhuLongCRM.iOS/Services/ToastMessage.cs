using System;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using PhuLongCRM.IServices;
using PhuLongCRM.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace PhuLongCRM.iOS.Services
{
    public class ToastMessage : IToastMessage
    {
        const int LONG_DELAY = 3000;
        //1300 fix task 482 thành 1500
        const int SHORT_DELAY = 1500;

        private UIView mView;
        private UILabel label;
        public CancellationTokenSource tokenSource;


        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }
        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        async void ShowAlert(string message, int time)
        {
            Init();

            if (mView.Alpha == 1)
            {
                tokenSource.Cancel();
                tokenSource = new CancellationTokenSource();
                label.Text = message;
            }
            else
            {
                tokenSource = new CancellationTokenSource();
                label.Text = message;
                mView.Alpha = 1;
            }

            try
            {
                await Task.Delay(time, tokenSource.Token);
                UIView.Animate(0.5, () =>
                {
                    mView.Alpha = 0;
                });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (tokenSource != null)
                {
                    tokenSource.Dispose();
                    tokenSource = null;
                }
            }
        }

        private void Init()
        {
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            if (mView == null)
            {
                mView = new UIView();
                mView.TranslatesAutoresizingMaskIntoConstraints = false;
                mView.Alpha = 0;
                mView.BackgroundColor = UIColor.White;
                mView.Layer.CornerRadius = 10;
                mView.Layer.ShadowOffset = new CGSize(4, 4);
                mView.Layer.ShadowColor = UIColor.Gray.CGColor;
                mView.Layer.ShadowRadius = 10f;
                mView.Layer.ShadowOpacity = 0.8f;
                mView.Layer.BorderColor = UIColor.Gray.CGColor;
                mView.Layer.BorderWidth = 0.5f;
                window.AddSubview(mView);

                label = new UILabel();
                label.TranslatesAutoresizingMaskIntoConstraints = false;
                label.Font = UIFont.SystemFontOfSize(16);
                label.TextColor = UIColor.Black;
                label.Lines = 0;

                mView.AddSubview(label);


                label.LeadingAnchor.ConstraintEqualTo(mView.LeadingAnchor, 15).Active = true;
                label.TrailingAnchor.ConstraintEqualTo(mView.TrailingAnchor, -16).Active = true;
                label.BottomAnchor.ConstraintEqualTo(mView.BottomAnchor, -15).Active = true;
                label.TopAnchor.ConstraintEqualTo(mView.TopAnchor, 15).Active = true;


                mView.LeadingAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.LeadingAnchor, 25).Active = true;
                mView.TrailingAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.TrailingAnchor, -25).Active = true;
                mView.BottomAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.BottomAnchor, -60).Active = true;


                mView.HeightAnchor.ConstraintGreaterThanOrEqualTo(20).Active = true;
            }
            else
            {
                window.AddSubview(mView);
            }
        }
    }
}
