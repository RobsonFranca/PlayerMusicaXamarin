using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android;
using Android.Util;
using System.IO;
using Android.Support.V4.App;
using Android.Media;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V4.Content;

[assembly:UsesPermission(Manifest.Permission.ReadExternalStorage)]
namespace PlayerMusica.Droid
{
    [Activity(Label = "PlayerMusica", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReadExternalStorage))
                {
                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage }, 12);
                    return;
                }
            }

            EsperarInicialiizacao();

            LoadApplication(new App());

            EsperarMuiscaSelecionada();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case 12:
                    {
                        // If request is cancelled, the result arrays are empty.
                        if (grantResults.Length > 0
                            && grantResults[0] == Permission.Granted)
                        {

                            // permission was granted, yay! Do the
                            // contacts-related task you need to do.

                        }
                        else
                        {

                            // permission denied, boo! Disable the
                            // functionality that depends on this permission.
                        }
                        return;
                    }

                    // other 'case' lines to check for other
                    // permissions this app might request
            }
        }

        void EsperarInicialiizacao()
        {
            MessagingCenter.Subscribe<string>(this, "Iniciando", message => 
            {
                var intent = new Intent(this, typeof(IniciarService));
                StartService(intent);
            });
        }

        void EsperarMuiscaSelecionada()
        {
            MessagingCenter.Subscribe<Musicas>(this, "ColocarMusica", message => 
            {
                //var intent = new Intent(this, typeof(SegundoPlano));
                var intent = new Intent(this, typeof(ServicoPlayer));
                intent.PutExtra("musica", message.Caminho);
                intent.PutExtra("id", message.ID.ToString());
                StartService(intent);
                //SendBroadcast(intent);
            });
        }
    }
}