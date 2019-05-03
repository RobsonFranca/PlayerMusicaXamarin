using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PlayerMusica
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        Musicas musica;

        public TabbedPage1 ()
        {
            InitializeComponent();

            musica = new Musicas();

            CarregarMusicas();

            EsperarLista();

            //_barra.ValueChanged += (s, e) =>
            //{
            //    //barraSendoMovida = true;
            //    var message = e.NewValue.ToString();
            //    MessagingCenter.Send(message, "Pular");
            //};

            lista_musicas.ItemTapped += (s, e) =>
            {
                var message = (e.Item as Musicas);
                MessagingCenter.Send(message, "ColocarMusica");
            };

            _playPause.Clicked += (s, e) =>
            {
                var message = "play";
                MessagingCenter.Send(message, "Play");
            };

            _prox.Clicked += (s, e) =>
            {
                Proximo();
            };

            _ante.Clicked += (s, e) =>
            {
                var musica = this.musica.GetMusicas.Single(m => m.ID == this.musica.ID);
                int indice = this.musica.GetMusicas.IndexOf(musica);
                Musicas message;
                try
                {
                    message = this.musica.GetMusicas[indice - 1];
                }
                catch (Exception ex)
                {
                    message = this.musica.GetMusicas[this.musica.GetMusicas.Count - 1];
                    Console.WriteLine("Erro Lista: " + ex.ToString());
                }
                lista_musicas.SelectedItem = message;
                MessagingCenter.Send(message, "ColocarMusica");
            };

            EsperarInformacoesMuisca();

            AtualizarBarra();

            EsperarProxima();
        }

        void Proximo()
        {
            var musica = this.musica.GetMusicas.Single(m => m.ID == this.musica.ID);
            int indice = this.musica.GetMusicas.IndexOf(musica);
            Musicas message;
            try
            {
                message = this.musica.GetMusicas[indice + 1];
            }
            catch (Exception ex)
            {
                message = this.musica.GetMusicas[0];
                Console.WriteLine("Erro Lista: " + ex.ToString());
            }
            lista_musicas.SelectedItem = message;
            MessagingCenter.Send(message, "ColocarMusica");
        }

        void CarregarMusicas()
        {
            var message = "iniciar";
            MessagingCenter.Send(message, "Iniciando");
        }

        void EsperarLista()
        {
            MessagingCenter.Subscribe<Musicas>(this, "FinalizarLista", message =>
            {
                Device.BeginInvokeOnMainThread(() => 
                {
                    musica.SetMusicas = message.GetMusicas;
                    musica.GetMusicas.Reverse();
                    lista_musicas.ItemsSource = musica.GetMusicas;

                    Console.WriteLine(musica.GetMusicas.Count.ToString());
                });
            });
        }

        void EsperarInformacoesMuisca()
        {
            MessagingCenter.Subscribe<Musicas>(this, "Info", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    musica.Duracao = message.Duracao;
                    musica.Nome = message.Nome;
                    musica.ID = message.ID;

                    _barra.Maximum = musica.Duracao;

                    int segundos = ((musica.Duracao / 1000));
                    int minutos = (segundos / 60);

                    _total.Text = string.Format("{0:00}:{1:00}",minutos,segundos % 60);
                    _barra.Value = 0;
                    _tamanho.Text = musica.Nome;
                    _playPause.Text = "Pause";
                });
            });
        }

        bool barraSendoMovida = false;

        void AtualizarBarra()
        {
            MessagingCenter.Subscribe<Musicas>(this, "Barra", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //if (!barraSendoMovida)
                    _barra.Value = message.Valor;

                    int segundos = (((musica.Duracao - message.Valor) / 1000));
                    int minutos = (segundos / 60);

                    _resta.Text = string.Format("{0:00}:{1:00}", minutos, segundos%60);
                    //if (segundos == 0 && minutos == 0) Proximo();
                });
            });
        }

        void EsperarProxima()
        {
            MessagingCenter.Subscribe<Musicas>(this, "Proxima", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Proximo();
                });
            });
        }
    }
}