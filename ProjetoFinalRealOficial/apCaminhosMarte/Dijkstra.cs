﻿using System;
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
            anteriores[origem] = origem;
            valorViagem[origem] = 0;//o valor da viagem (de acordo com o critério escolhido) da origem até ela mesma é zero
        }

        private bool existeAberto()
        {
            for (int i = 0; i < abertos.Length; i++)
                if (abertos[i] == true)
                    return true;

            return false;
        }


        //retorna o indice do vertice cujo valor de viagem é menor dentre todos os outros
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
                //caso o vertice i esteja aberto e o valor de viagem dele seja menor que 
                if (abertos[i] && (valorViagem[menor] > valorViagem[i]))
                    menor = i;

            return menor;
        }


        //busca os melhores caminhos a partir de um vértice para todos os outros
        public void Resolva()
        {
            int menor;

            //faz isso qté que todos os vertices estejam fechados, ou seja, todos os caminhos
            //para todos os vértices foram encontrados
            while(existeAberto())
            {
                //atribui o valor do vertice cujo valor da viagem é o menor dentre todos os possiveis
                menor = menorValorViagem();

                //fecha esse vertice, ja que o menor caminho até ele ja foi encontrado
                abertos[menor] = false;

                //relaxa as arestas a partir do vertice encontrado
                relaxa(menor);
            }

        }

        //retorna o melhor caminho até o vertice de destino
        public List<Caminho> melhorCaminho(int codDestino)
        {

            int atual = codDestino;
            List<Caminho> caminho = new List<Caminho>();
            

            while(atual != this.origem)
            {
                caminho.Add( new Caminho(anteriores[atual], atual, grafoCidades.MatrizAdjacente[anteriores[atual], atual].Distancia, grafoCidades.MatrizAdjacente[anteriores[atual], atual].Tempo, grafoCidades.MatrizAdjacente[anteriores[atual], atual].Custo));
                
                atual = anteriores[atual];
            }

            //usa a função List.Reverse, já que encontramos o caminho a partir do destino, e não da origem
            caminho.Reverse();
            return caminho;

        }


        //retorna o valor da viagem com base no critério escolhido
        private int valorCriterio(int x, int y)
        {
            if(x>=0)//caso o anterior exista
                switch (this.criterio)
                {
                    case 1: return grafoCidades.MatrizAdjacente[x, y].Distancia;
                    case 2: return grafoCidades.MatrizAdjacente[x, y].Tempo;
                    case 3: return grafoCidades.MatrizAdjacente[x, y].Custo;
                } 
            //caso o anterior não exista, ou seja, ser -1
            return 0;//retorna 0
        }

        private void relaxa(int atual)
        {
            //relaxa as arestas a partir de um vertice no grafo, ou seja, altera os valores no vetor valorViagem
            //a partir dos novos valores encontrados
            for(int i = 0; i<valorViagem.Length; i++)
            {
                //verifica todos os vertices e caso encontre uma aresta entre o vertice atual e qualquer outro
                if(grafoCidades.MatrizAdjacente[atual, i] != null)
                {
                    //define o valor como o valor da aresta entre o vertice atual e o vertice encontrado 
                    //somado ao valor da viagem da origem ao vertice anterior
                    int valor = valorCriterio(atual, i) + valorViagem[anteriores[atual]];

                    //se o valor atual for maior que o novo valor encontrado, substitui o valor atual pelo novo valor encontrado
                    if(abertos[i]&&valorViagem[i]>valor)
                    {
                        valorViagem[i] = valor;
                        anteriores[i] = atual;
                    }
                }
            }
        }
    }
}
