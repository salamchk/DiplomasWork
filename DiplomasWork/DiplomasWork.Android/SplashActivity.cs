using Android.Animation;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Android.Animation.Animator;

namespace DiplomasWork.Droid
{
    [Activity(Label = "SplashActivity",
        Theme = "@style/SplashTheme",
        Icon = "@mipmap/icon",
        MainLauncher = true,
        NoHistory = true,
        ConfigurationChanges =
    ConfigChanges.ScreenSize | ConfigChanges.Orientation |
    ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
    ConfigChanges.SmallestScreenSize,
        ScreenOrientation = ScreenOrientation.Landscape)]
    public class SplashActivity : Activity, IAnimatorListener
    {
        public void OnAnimationCancel(Animator animation)
        {
           
        }

        public void OnAnimationEnd(Animator animation)
        {
            StartActivity(typeof(MainActivity));
            OverridePendingTransition(0, 0);
        }

        public void OnAnimationRepeat(Animator animation)
        {
           
        }

        public void OnAnimationStart(Animator animation)
        {
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashLayout);

            var animation = FindViewById<Com.Airbnb.Lottie.LottieAnimationView>(Resource.Id.animation_view);
            animation.AddAnimatorListener(this);
        }
    }
}
