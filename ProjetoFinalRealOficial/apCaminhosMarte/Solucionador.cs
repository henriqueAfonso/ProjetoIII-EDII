//Henrique Afonso MArtins - 19178
//Guilherme Brentan de Oliveira - 19175
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Solucionador
    {
        //variavel que armazena o caminho atual
        private List<Caminho> caminho;

        //variaveis que armazenam o melhor caminho e sua distancia total
        private List<Caminho> melhorCaminho;
        private int menorDistanciaAtual = 0;
        private int menorTempoAtaul     = 0;
        private int menorCustoAtaul     = 0;

        //lista com todos os caminhos possiveis
        private List<List<Caminho>> caminhos;

        private Grafo grafo; //grafo que contem a matriz adjacente com os caminhos disponiveis

        private bool[] visitou;//vetor que guarda quais cidades ja foram visitadas


        public List<List<Caminho>> Caminhos { get => caminhos; }
        public List<Caminho> MelhorCaminho { get => melhorCaminho; }

        public Solucionador(Grafo grafo)
        {
            this.grafo = grafo;
            caminho = new List<Caminho>();//inicia uma lista vazia com os "movimentos" do caminho atual
            melhorCaminho = new List<Caminho>();//inicia uma lista com os melhores movimentos, de acordo com o criterio de distancia percorrida
            caminhos = new List<List<Caminho>>();//inicia a lista coms os caminhos encontrados
            visitou = new bool[grafo.QuantasCidades];//inicia um vetor com o numero de cidades respectivas
        }

        public void SolucionaDjikstra(int origem, int destino, int criterio)
        {
            ZerarVariaveis();
            Dijkstra solucionador = new Dijkstra(grafo, criterio, origem);
            solucionador.Resolva();
            List<Caminho> melhor = solucionador.melhorCaminho(destino);
            this.caminhos.Add(melhor);
            this.melhorCaminho = melhor;

        }


        public void SolucionaRecursivo(int origem, int destino, string criterio)
        {
            //método utilizado para chamar o método ZerarVariaveis apenas uma vez, pois com a recursão ocorreria um erro de StackOverflow
            ZerarVariaveis();
            SolucionaRecursivoMesmo(origem, destino, criterio);
        }
        private void SolucionaRecursivoMesmo(int origem, int destino, string criterio)
        {
            //usa o 'for' para verificar para quais cidades se pode andar
            //sendo 'i' o codigo da cidade no grafo
            
            for (int i = 0; i < grafo.QuantasCidades; i++)
            {
                if (grafo.MatrizAdjacente[origem, i] != null && visitou[i] == false)//se pode andar para a cidade 'i' e ela não foi visitada
                {
                    //se a cidade i é o destino
                    if (i == destino)
                    {
                        caminho.Add(grafo.MatrizAdjacente[origem, destino]);//adiciona o movimento a lista
                        caminhos.Add(new List<Caminho>(caminho));//adiciona um novo caminho à lista 'caminhos'

                        //marca a origem do movimento como não visitado, para que outros caminhos possam passar pela origem
                        visitou[origem] = false;
                        caminho.Remove(grafo.MatrizAdjacente[origem, i]); //remove o ultimo movimento (que foi o final) e continua a busca de novos caminhos
                    }
                    //se a cidade i não for o destino
                    else
                    {
                        //caso nao de para andar pro destino, anda para outra cidade e reinicia a busca

                        visitou[origem] = true;//marca que ja visitou a cidade atual
                        caminho.Add(grafo.MatrizAdjacente[origem, i]);//adiciona o movimento que sera feito a lista caminho
                        SolucionaRecursivoMesmo(i, destino, criterio);//refaz a busca a partir da nova cidade

                        //após refazer a busca e encontrar todos os caminhos possiveis a partir da nova cidade
                        caminho.Remove(grafo.MatrizAdjacente[origem, i]);//remove o ultimo movimento feito
                        visitou[origem] = false;//marca a origem do movimento como não visitado, para que outros caminhos possam passar pela origem
                    }
                }
            }

            OrdenarPorCriterio(criterio);
        }

        public Queue<List<Caminho>> SolucionaBacktracking(int codOrigem, int codDestino, string criterio)
        {
            ZerarVariaveis();
            Queue<List<Caminho>> queue = new Queue<List<Caminho>>(); //inicializo uma pilha 

            caminho.Add(new Caminho(codOrigem, codOrigem, 0, 0, 0));//adiciono o primeiro caminho "possível" no vetor de caminho
            queue.Enqueue(caminho); //e coloco ele empilhado dentro da pilha

            while (queue.Count != 0) //enquanto não acabar a pilha, quer dizer que posso ter solução ainda
            {
                caminho = queue.Dequeue(); //desempilho a pilha
                Caminho ult = caminho[caminho.Count - 1]; //pego o ultimo caminho realizado

                if (ult.CodDestino == codDestino) //se for inde eu quero 
                    Caminhos.Add(caminho); //adidicono o caminho solução no vetor de solução

                for (int i = 0; i < grafo.MatrizAdjacente.GetLength(0); i++) //percorro a matriz adjacente 
                {
                    if (grafo.MatrizAdjacente[ult.CodDestino, i] != null &&          //vejo se pra onde estou indo não é null 
                       !Visitado(grafo.MatrizAdjacente[ult.CodDestino, i], caminho)) //ou se eu ja passei por aquela cidade
                    {
                        List<Caminho> novoCaminho = new List<Caminho>(caminho); //inicializo um novo vetor de caminho
                        novoCaminho.Add(grafo.MatrizAdjacente[ult.CodDestino, i]); //adiciono o caminho atual
                        queue.Enqueue(novoCaminho); //empilho o ultimo caminho
                    }
                }
            }

            OrdenarPorCriterio(criterio);

            return queue; //com isso, ele achará todos os caminhos, pois a partir de um momento irá parar de passar no if do for
                          //fazendo isso, ele só ira desempilhar no começo do while, até acabar a pilha
        }

        private bool Visitado(Caminho caminho, List<Caminho> listaCaminho) //recebe um caminho e um vetor de caminho
        {
            foreach (Caminho cam in listaCaminho) //percorro o vetor de caminho
                if ((cam.CodOrigem == caminho.CodOrigem) && (cam.CodDestino == caminho.CodDestino)) //se ja fiz aquele caminho nesse vetor
                    return true; //retorno true
            
            return false; //senão retorno false
        }

        private void OrdenarPorCriterio(string criterio) //recebo o criterio
        {
            if(criterio == "distancia") //caso seja distancia
            {
                int distanciaAtual; //declaro uma variavel auxiliar

                foreach(List<Caminho> caminhos in Caminhos)//percorro cado lista de caminho na lista de todos os caminhos
                {
                    distanciaAtual = 0; //zero a distancia atual
                    foreach(Caminho cam in caminhos) //pego o caminho especifico
                    {
                        distanciaAtual += cam.Distancia; //somo a distancia daquele caminho ao total
                    }

                    if (menorDistanciaAtual == 0) //se menorDistancia o == 0, quer dizer que é a primeira vez
                    {
                        menorDistanciaAtual = distanciaAtual; //coloco então a distancia atual
                        melhorCaminho = caminhos;//marco o melhor caminho
                    }
                    else if (menorDistanciaAtual > distanciaAtual) //caso a menorDistanciaAtual seja > que a distanciaAtual
                    {
                        menorDistanciaAtual = distanciaAtual;//troco o valor da distanciaAtual
                        melhorCaminho = caminhos;//marco o melhor caminho
                    }
                }
            }

            if(criterio == "tempo")//mesma logica aplicada ao tempo, ó mudando o nome das variaveis
            {
                int tempoAtual;

                foreach (List<Caminho> caminhos in Caminhos)
                {
                    tempoAtual = 0;
                    foreach (Caminho cam in caminhos)
                    {
                        tempoAtual += cam.Tempo;
                    }

                    if (menorTempoAtaul == 0)
                    {
                        menorTempoAtaul = tempoAtual;
                        melhorCaminho = caminhos;
                    }
                    else if (menorTempoAtaul > tempoAtual)
                    {
                        menorTempoAtaul = tempoAtual;
                        melhorCaminho = caminhos;
                    }
                }
            }

            if(criterio == "custo")//mesma logica aplicada ao custo, só mudando o nome das variaveis
            {
                int custoAtual;

                foreach (List<Caminho> caminhos in Caminhos)
                {
                    custoAtual = 0;
                    foreach (Caminho cam in caminhos)
                    {
                        custoAtual += cam.Custo;
                    }

                    if (menorCustoAtaul == 0)
                    {
                        menorCustoAtaul = custoAtual;
                        melhorCaminho = caminhos;
                    }
                    else if (menorCustoAtaul > custoAtual)
                    {
                        menorCustoAtaul = custoAtual;
                        melhorCaminho = caminhos;
                    }
                }
            }
        }

        private void ZerarVariaveis()
        {
            caminho = new List<Caminho>();//inicia uma lista vazia com os "movimentos" do caminho atual
            melhorCaminho = new List<Caminho>();//inicia uma lista com os melhores movimentos, de acordo com o criterio de distancia percorrida
            caminhos = new List<List<Caminho>>();//inicia a lista coms os caminhos encontrados
            visitou = new bool[grafo.QuantasCidades];//inicia um vetor com o numero de cidades respectivas
        }
    }
}
