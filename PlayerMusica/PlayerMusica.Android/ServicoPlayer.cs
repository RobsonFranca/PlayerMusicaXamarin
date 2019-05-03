using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using System.IO;
using Android.Util;
using Xamarin.Forms;

namespace PlayerMusica.Droid
{
    [Service]
    class ServicoPlayer : Service
    {
        MediaPlayer player;
        int posicao;
        bool sozinho;

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var caminho = intent.GetStringExtra("musica");
            var id = intent.GetStringExtra("id");

            ColocarMusica(caminho);

            var message = new Musicas
            {
                Duracao = player.Duration,
                Nome = caminho.Split('/')[caminho.Split('/').Length - 1],
                ID = double.Parse(id)
            };
            MessagingCenter.Send(message, "Info");

            EsperarComandos();
            
            return StartCommandResult.Sticky;
        }

        void ColocarMusica(string caminho)
        {
            try
            {
                sozinho = false;
                player.Pause();
                player.Release();
            }
            catch { }
            player = new MediaPlayer();
            player.SetAudioStreamType(Android.Media.Stream.Music);
            player.SetDataSource(caminho);
            player.Prepare();
            player.Start();
            player.SetWakeMode(this, WakeLockFlags.Partial);
            posicao = 0;
            MessagingCenter.Unsubscribe<string>(this, "Play");
            MessagingCenter.Unsubscribe<string>(this, "Pular");
        }

        private void EsperarComandos()
        {
            Task.Run(() =>
            {
                MessagingCenter.Subscribe<string>(this, "Play", message =>
                {
                    if (player.IsPlaying)
                    {
                        sozinho = false;
                        player.Stop();
                    }
                    else
                    {
                        player.Prepare();
                        player.SeekTo(posicao);
                        player.Start();
                        MandarPosicao();
                    }
                });
                //MessagingCenter.Subscribe<string>(this, "Pular", message =>
                //{
                //    int tempo;
                //    if(int.TryParse(message, out tempo))
                //    {
                //        sozinho = false;
                //        player.Stop();
                //        player.Prepare();
                //        player.SeekTo(posicao);
                //        player.Start();
                //        MandarPosicao();
                //    }
                //});
                MandarPosicao();
            });
        }

        public void MandarPosicao()
        {
            Task.Run(() =>
            {
                sozinho = true;
                while (player.IsPlaying)
                {
                    posicao = player.CurrentPosition;
                    var message = new Musicas { Valor = posicao };
                    MessagingCenter.Send(message, "Barra");
                }
                if (sozinho)
                {
                    var message2 = new Musicas { Valor = posicao };
                    MessagingCenter.Send(message2, "Proxima");
                }
            });
        }
    }
}