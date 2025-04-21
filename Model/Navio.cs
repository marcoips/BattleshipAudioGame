using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipAudioGame.Model
{
    public class Navio
    {
        public string nome_navio { get; private set; }
        public int tamanho_navio { get; private set; }
        public bool afundado { get; private set; }
        public List<string> localizacao { get; set; }

        public Navio(string name, int tamanho, bool afundado, List<string> localizacao)
        {
            this.nome_navio = name;
            this.tamanho_navio = tamanho;
            this.afundado = afundado;
            this.localizacao = localizacao;
        }
    }
}
