using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleFormsAnalyzerLib.Objetos
{
    public class Item : Componente
    {
        public string tipoItem              { get; set; }
        public string topicoLivroAjuda      { get; set; }
        public string ativado               { get; set; }
        public string justificacao          { get; set; }
        public string variasLinhas { get; set; }
        public string restriçãoMaiusculasMinusculas { get; set; }
        public string numeroItensExibidos   { get; set; }
        public string itemAnteriorNavegacao { get; set; }
        public string proximoItemNavegacao  { get; set; }
        public string tiposDados            { get; set; }
        public string tamanhoMaximo         { get; set; }
        public string tamanhoFixo           { get; set; }
        public string valorInicial          { get; set; }
        public string obrigatorio           { get; set; }
        public string mascaraFormato        { get; set; }
        public string canvas                { get; set; }
        public string nomeFonte             { get; set; }
        public string tamanhoFonte          { get; set; }
        public string pesoFonte             { get; set; }
        public string corFundo              { get; set; }
        public string prompt                { get; set; }
        public string nomeFontePrompt       { get; set; }
        public string tamanhoFontePrompt    { get; set; }
        public string pesoFontePrompt       { get; set; }
        public string limiteConexaoPrompt   { get; set; }
        public string alinhamentoPrompt     { get; set; }
        public string deslocamentoConexaoPrompt     { get; set; }
        public string deslocamentoAlinhamentoPrompt { get; set; }
        public string dica                  { get; set; }
        public string dicaFerramenta        { get; set; }
        public string altura                { get; set; }
        
        public bool hasCanvas()
        {
            return !string.IsNullOrEmpty(canvas);
        }

        public bool hasHints()
        {
            return !string.IsNullOrEmpty(dica) && dica.Length>5;
        }

        public float getFloatAltura()
        {
            try
            {
                return float.Parse(altura);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public float getFloatTamanhoFonte()
        {
            try
            {
                return float.Parse(tamanhoFonte);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool hasPrompt()
        {
            return !string.IsNullOrEmpty(prompt);
        }

        public float getFloatTamanhoFontePrompt()
        {
            try
            {
                return float.Parse(tamanhoFontePrompt);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public TipoDado getTipoDado()
        {
            switch (tiposDados)
            {
                case "Número": return TipoDado.Numero;
                case "Data": return TipoDado.Data;
                case "Number": return TipoDado.Numero;
                case "Date": return TipoDado.Data;
                default: return TipoDado.Caracter;
            }
        }

        public Alinhamento getAlinhamento()
        {
            if (string.IsNullOrEmpty(justificacao))
                return Alinhamento.Indefinido;
            else if (new string[] { "Final", "Direita", "End", "Right" }.Contains(justificacao))
                return Alinhamento.Direita;
            else
                return Alinhamento.Esquerda;
        }
        
        public Alinhamento getAlinhamentoPrompt()
        {
            if (string.IsNullOrEmpty(alinhamentoPrompt))
                return Alinhamento.Indefinido;
            else if (new string[] { "Final", "Direita", "End", "Right" }.Contains(alinhamentoPrompt))
                return Alinhamento.Direita;
            else if (alinhamentoPrompt.ToUpper() == "CENTRALIZADO")
                return Alinhamento.Centralizado;
            else
                return Alinhamento.Esquerda;
        }

        public TipoItem getTipoItem()
        {
            switch (tipoItem)
            {
                    case "Item de Texto"   : return TipoItem.ItemTexto;
                    case "Item da Lista"   : return TipoItem.ItemLista;
                    case "Tecla"           : return TipoItem.Botao;
                    case "Caixa de Seleção": return TipoItem.CheckBox;
                    case "Grupo de Opções" : return TipoItem.RadioGroup;
                    case "Imagem"          : return TipoItem.Imagem;
                     /* Inglês */
                    case "Text Item"   : return TipoItem.ItemTexto;
                    case "List Item"   : return TipoItem.ItemLista;
                    case "PushButton"  : return TipoItem.Botao;
                    case "CheckBox"    : return TipoItem.CheckBox;
                    case "Radio Group" : return TipoItem.RadioGroup;
                    case "Image"       : return TipoItem.Imagem;
                    default            : return TipoItem.Indefinido;
              }
         }
        
        public float getFloatDeslocamentoConexaoPrompt()
        {
        	try {
        		return float.Parse(deslocamentoConexaoPrompt);
        	} catch (Exception) {
        		return 0;
        	}
        }
        
        public float getDeslocamentoAlinhamentoPrompt()
        {
        	try {
        		return float.Parse(deslocamentoAlinhamentoPrompt);
        	} catch (Exception) {
        		return 0;
        	}
        }
        
        public int getIntNumeroItensExibidos()
        {
        	try {
        		return int.Parse(numeroItensExibidos);
        	} catch (Exception) {
        		return  0;
        	}
        }

        public bool isEnabled()
        {
            return ativado != null? ativado.ToUpper() == "SIM" : false;
        }

        public bool isMultilinha()
        {
            return variasLinhas!= null? variasLinhas.ToUpper() == "SIM" : false;
        }

        public Case getTipoCase()
        {
            switch (restriçãoMaiusculasMinusculas)
            {
                case "Superior": return Case.Upper; 
                case "Misto": return Case.Misto;
                default: return Case.Lower;
            }
        }
    }

    public enum TipoItem 
    {
        ItemTexto, ItemLista, Botao, CheckBox, RadioGroup, Indefinido, Imagem
    }

    public enum TipoDado
    {
        Numero, Data, Caracter
    }

    public enum Case
    {
        Upper, Lower, Misto
    }

    public enum Alinhamento
    {
        Direita, Esquerda, Centralizado, Indefinido
    }
}
