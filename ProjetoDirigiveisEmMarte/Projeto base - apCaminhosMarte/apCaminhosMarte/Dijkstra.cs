using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Dijkstra
    {
        private Grafo grafoCidades;

        private int[] valorViagem;
        private int[] anteriores;
        private bool[] abertos;

        private int origem;

        //se for 1, usa distancia
        //se for 2, usa tempo
        //se for 3, usa custo
        private int criterio;

        ///<sumary>
        ///Construtor da classe Dijkstra
        ///</sumary>
        ///<param name="criterio">Um valor inteiro entre 1 e 3. 
        ///Caso seja 1, o critério utilizado será a distância;
        ///Caso seja 2, o critério utilizado será o tempo;
        ///Caso seja 3, o critério utilizado será o custo;
        ///</param>
        public Dijkstra(Grafo grafoCidades, int criterio, int origem)
        {
            if (criterio < 1 || criterio > 3)
                throw new Exception("Critério inválido, leia a documentação");

            if(origem<0||origem>grafoCidades.QuantasCidades)
                throw new Exception("Origem inválida");

            this.grafoCidades = grafoCidades;
            this.criterio = criterio;
            this.origem = origem;

            this.valorViagem = new int[grafoCidades.QuantasCidades];
            this.anteriores = new int[grafoCidades.QuantasCidades];
            this.abertos = new bool[grafoCidades.QuantasCidades];

            for (int i = 0; i<this.grafoCidades.QuantasCidades; i++)
            {
                valorViagem[i] = int.MaxValue / 2;
                anteriores[i] = -1;
                abertos[i] = true;
            }
            valorViagem[origem] = 0;//o valor da viagem (de acordo com o critério escolhido) da origem até ela mesma é zero
        }

        private bool existeAberto()
        {
            for (int i = 0; i < abertos.Length; i++)
                if (abertos[i] == true)
                    return true;

            return false;
        }

        private int menorValorViagem()
        {
            int i;
            //procura o primeiro vertice aberto 
            for (i = 0; i < valorViagem.Length; i++)
                if (abertos[i] == true)
                    break;

            //caso não ache nenhum, retorna -1
            if (i > valorViagem.Length)
                return -1;

            //define o vertice achado como o menor (provisoriamente)
            int menor = i;
            for (i = menor + 1; i < valorViagem.Length; i++)
                if (abertos[i] && (valorViagem[menor] > valorViagem[i]))
                    menor = i;

            return menor;
        }

        public void Resolva()
        {
            int menor;

            while(existeAberto())
            {
                menor = menorValorViagem();
                abertos[menor] = false;
                relaxa(menor);
            }

        }

        private int valorCriterio(int x, int y)
        {
            switch (this.criterio)
            {
                case 1: return grafoCidades.MatrizAdjacente[x, y].Distancia;
                case 2: return grafoCidades.MatrizAdjacente[x, y].Tempo;
                case 3: return grafoCidades.MatrizAdjacente[x, y].Custo;
            } 
            //nuca retornará o valor abaixo, precisa desse código para funcionar
            return -1;
        }

        void relaxa(int atual)
        {
            for(int i = 0; i<valorViagem.Length; i++)
            {
                if(grafoCidades.MatrizAdjacente[atual, i] != null)
                {
                    int valor = valorCriterio(atual, i);
                    //se o valor atual for maior que o novo valor encontrado, substitui
                    if(valorViagem[i]>valor)
                    {
                        valorViagem[i] = valor;
                        anteriores[i] = atual;
                    }
                }
            }
        }
    }
}
