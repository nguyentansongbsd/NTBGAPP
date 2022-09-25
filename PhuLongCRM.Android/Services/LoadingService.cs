using System;
using Android.App;
using Android.Graphics.Drawables;
using andoirdGraphics = Android.Graphics;
using Android.Views;
using Android.Widget;
using PhuLongCRM.IServices;
using PhuLongCRM.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(LoadingService))]
namespace PhuLongCRM.Droid.Services
{
    public class LoadingService : ILoadingService
    {
        public static View view;
        public Dialog _dialog;
        private bool init = false;

        public void Initilize()
        {
            Activity activity = Xamarin.Essentials.Platform.CurrentActivity;

            view = activity.LayoutInflater.Inflate(Resource.Layout.LoadingLayout, null);
            LinearLayout.LayoutParams linearLayoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent,
                    LinearLayout.LayoutParams.MatchParent);
            activity.AddContentView(view, linearLayoutParams);
        }

        public void Init()
        {
            if (init == false)
            {
                try
                {
                    var mainActivity = Xamarin.Essentials.Platform.CurrentActivity;

                    AlertDialog.Builder builder = new AlertDialog.Builder(mainActivity);
                    builder.SetView(mainActivity.LayoutInflater.Inflate(Resource.Layout.LoadingLayout, null));
                    builder.SetCancelable(false);
                    _dialog = builder.Create();
                    _dialog.Window.SetBackgroundDrawable(new ColorDrawable(andoirdGraphics.Color.Transparent));

                    init = true;
                }
                catch
                {

                }
            }
        }

        public void Show()
        {
            try
            {
                view.Visibility = ViewStates.Visible;
            }
            catch
            {

            }
        }
        public void Hide()
        {
            try
            {
                view.Visibility = ViewStates.Gone;
            }
            catch
            {

            }
        }
    }
}
