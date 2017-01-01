using Android.App;
using Android.Content.PM;
using Android.OS;
using Java.Interop;
using Xamarinos.AdMob.Forms;
using Xamarinos.AdMob.Forms.Android;

namespace HangoverApp.Droid
{
    [Activity(Label = "HangoverApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //protected override void OnCreate(Bundle bundle)
        //{
        //    //Register AdBanner Control Renderer
        //    AdBannerRenderer.Init();

        //    //Initialize Interstitial Manager with a Specific AdMob Key
        //    CrossAdmobManager.Init("ca-app-pub-3564256941949898/3666384762");
        //    TabLayoutResource = Resource.Layout.Tabbar;
        //    ToolbarResource = Resource.Layout.Toolbar;

        //    base.OnCreate(bundle);

        //    global::Xamarin.Forms.Forms.Init(this, bundle);
        //    LoadApplication(new App());

        //    var myBanner = new AdBanner();

        //    //Set Your AdMob Key
        //    myBanner.AdID = "ca-app-pub-3564256941949898/3666384762";

        //}

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

