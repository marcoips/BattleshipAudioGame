using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1.Model
{
    public class Partida
    {
        public int id { get; private set; }
        public DateTime inicio_Partida { get; private set; }
        public List<Navio> Navios_Jogador { get; private set;}
        public List<Navio> Navios_CPU { get; private set; }
        public bool Estado { get; private set; }
        public DateTime fim_Partida { get; private set; }


        public Partida(int id, DateTime inicio, List<Navio> navios_jogador, List<Navio> navios_cpu, bool estado, DateTime fim)
        {
            this.id = id;
            this.inicio_Partida = inicio;
            this.Navios_Jogador = navios_jogador;
            this.Navios_CPU = navios_cpu;
            this.Estado = estado;
            this.fim_Partida = fim;
        }

    }
}
