using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Plugin.Connectivity;
using Plugin.SecureStorage;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarinos.AdMob.Forms;
using Xamarinos.AdMob.Forms.Android;

namespace HangoverApp.Droid
{
    [Activity(Label = "HangoverApp", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //Register AdBanner Control Renderer
            AdBannerRenderer.Init();

            //Initialize Interstitial Manager with a Specific AdMob Key
            CrossAdmobManager.Init("ca-app-pub-3564256941949898/8465093569");

            if (CrossConnectivity.Current.IsConnected)
            {
                List<string> helloMessages = new List<string>()
                {
                    "Had fun last night? :D",
                    "Have you taken some painkillers? :D",
                    "Hungry? :D",
                    "I feel you.. :D",
                };
                // add items to the list
                Random r = new Random();
                int index = r.Next(helloMessages.Count);

                var myHandler = new Handler();
                myHandler.Post(() =>
                {
                    Toast.MakeText(this, helloMessages[index], ToastLength.Long).Show();
                });
                base.Window.RequestFeature(WindowFeatures.ActionBar);
                base.SetTheme(Resource.Style.MainTheme);

                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(Color.FromHex("#8BC34A").ToAndroid());
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                SecureStorageImplementation.StoragePassword = "lota";
                base.OnCreate(bundle);
                global::Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App());


                var myBanner = new AdBanner();

                //Set Your AdMob Key
                myBanner.AdID = "ca-app-pub-3564256941949898/8465093569";
            }
            else
            {
                var myHandler = new Handler();
                myHandler.Post(() =>
                {
                    Toast.MakeText(this,"You need an interent connection to use this app!", ToastLength.Long).Show();
                });
                base.Window.RequestFeature(WindowFeatures.ActionBar);
                base.SetTheme(Resource.Style.MainTheme);

                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(Color.FromHex("#8BC34A").ToAndroid());
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                SecureStorageImplementation.StoragePassword = "lota";
                base.OnCreate(bundle);
                global::Xamarin.Forms.Forms.Init(this, bundle);
                this.FinishAffinity();
            }
        }
    }
}

