using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FormsAnalyzerLib
{
    public class RelatorioObjetos
    {
        public string[] linhasArquivo { get; set; }
        int             _indice       { get; set; }

        public RelatorioObjetos(string[] pLinhas)
        {
            _indice = 0;
            linhasArquivo = pLinhas;
        }

        internal string linha
        { 
            get{
                    try 
	                {	        
		                return linhasArquivo[_indice];
	                }
	                catch (Exception)
	                {
		                return "";
	                }
               }
        }

        internal void setLinhaAtual(int pLinha)
        {
            _indice = pLinha;
        }

        internal string getValor(string valor)
        {
            string retorno = null;
            retorno = linha.Substring(linha.IndexOf(valor) + valor.Length).Trim();
            return retorno;
        }

        internal bool isLabel(string pLabel, int pIndice = -1)
        {
            var regex = new Regex(@" ([\*\-o\^]) " + pLabel.Trim() + "  ");
            if (pIndice != -1)
                return regex.IsMatch(getLinhaNoIndice(pIndice));
            return regex.IsMatch(linha);
        }

        internal int getIndice()
        {
            return _indice;
        }

        internal int getTotalLinhas()
        {
            return linhasArquivo.Length;
        }

        internal string getProximaLinha(int pIndiceAdd = 0)
        {
            try
            {
                return linhasArquivo[_indice + pIndiceAdd + 1];
            }
            catch (Exception)
            {
                return "";
            }
        }

        internal string getLinhaNoIndice(int pIndice)
        {
            try 
	        {
                return linhasArquivo[pIndice];
	        }
	        catch (Exception)
	        {
                return "";
	        }
        }

        internal int next()
        {
            _indice ++;
            return _indice;
        }

        internal int next(int pAddIndice)
        {
            _indice += pAddIndice;
            return _indice;
        }

        internal int previus()
        {
            _indice --;
            return _indice;
        }

        internal int previus(int pSubtraIndice)
        {
            _indice -= pSubtraIndice;
            return _indice;
        }

        internal bool isFinal()
        {
            return (_indice+1) >= linhasArquivo.Length;
        }


    }
}
