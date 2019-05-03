using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayerMusica
{
    public class Musicas
    {
        List<Musicas> _lista = new List<Musicas>();

        public string Caminho { get; set; }
        public string Nome { get; set; }
        public bool IsTocando { get; set; }
        public double ID { get; set; }

        public int Duracao { get; set; }
        public int Valor { get; set; }

        public List<Musicas> GetMusicas { get { return _lista; } }
        public List<Musicas> SetMusicas { set { _lista = value; } }

        public void Add()
        {
            _lista.Add(new Musicas { Caminho = this.Caminho, Nome = this.Nome, IsTocando = this.IsTocando, ID = this.ID });
        }
    }
}
