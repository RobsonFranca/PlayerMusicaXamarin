using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace PlayerMusica.Droid
{
    [Service]
    class IniciarService : Service
    {
        Musicas m = new Musicas();
        double id;

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            id = 0;
            Task.Run(() => 
            {
                PegarMusicas();

                m.SetMusicas = m.GetMusicas.OrderBy(x => Directory.GetCreationTime(x.Caminho)).ToList();
                MessagingCenter.Send(m, "FinalizarLista");
            });
            return StartCommandResult.Sticky;
        }

        void PegarMusicas()
        {
            string celular = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string cartao = Directory.GetParent(Directory.GetParent(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath).FullName).FullName;

            try
            {
                Mostrar(Path.GetFullPath(celular));
            }
            catch (Exception e) { Log.Info("ERRO", e.Message); }
            try
            {
                Mostrar(Path.GetFullPath(cartao));
            }
            catch (Exception e) { Log.Info("ERRO", e.Message); }
        }

        void Mostrar(string pasta)
        {
            try
            {
                string[] lista = Directory.GetDirectories(pasta);
                string[] arq = Directory.GetFiles(pasta);
                if (arq.Length > 0)
                {
                    foreach (string s in arq)
                    {
                        if (s.Contains(".mp3"))
                        {
                            m.Caminho = s;
                            m.Nome = s.Split('/')[s.Split('/').Length - 1];
                            m.IsTocando = false;
                            m.ID = id;
                            id += 1;
                            m.Add();
                        }
                    }
                }
                if (lista.Length > 0)
                {
                    foreach (string s in lista)
                    {
                        Mostrar(s);
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Info("TESTE", "pasta nao pode ser acessada!");
            }
        }
    }
}