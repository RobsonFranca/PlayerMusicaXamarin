using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace PlayerMusica.Droid
{
    [BroadcastReceiver(Enabled = true)]
    class SegundoPlano : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var message = intent.GetStringExtra("musica2");


            var intent2 = new Intent(context, typeof(ServicoPlayer));
            intent2.PutExtra("musica", message);
            
            context.StartForegroundService(intent2);
        }
    }
}