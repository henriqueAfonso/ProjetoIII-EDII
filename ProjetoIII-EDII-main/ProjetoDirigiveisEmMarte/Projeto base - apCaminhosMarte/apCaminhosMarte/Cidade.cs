﻿//Henrique Afonso MArtins - 19178
//Guilherme Brentan de Oliveira - 19175
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Cidade
    {
        private int codCidade;
        private string nomeCidade;
        private int coordX, coordY;
        public Cidade(int codCidade, string nomeCidade, int coordX, int coordY)
        {
            CodCidade = codCidade;
            NomeCidade = nomeCidade;
            CoordX = coordX;
            CoordY = coordY;
        }
        public Cidade(Cidade cidade)
        {
            CodCidade  = cidade.CodCidade;
            NomeCidade = cidade.NomeCidade;
            CoordX     = cidade.CoordX;
            CoordY     = cidade.CoordY;
        }
        public int CodCidade 
        { 
            get => codCidade; 
            set => codCidade = value; 
        }
        public string NomeCidade 
        { 
            get => nomeCidade; 
            set => nomeCidade = value; 
        }
        public int CoordX 
        { 
            get => coordX; 
            set => coordX = value; 
        }
        public int CoordY 
        { 
            get => coordY; 
            set => coordY = value; 
        }
    }
}
