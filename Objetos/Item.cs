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
        public string prompt                { get; set; }
        public string nomeFontePrompt       { get; set; }
        public string tamanhoFontePrompt    { get; set; }
        public string pesoFontePrompt       { get; set; }
        public string dica                  { get; set; }
        public string dicaFerramenta        { get; set; }
        public string altura                { get; set; }
        
        public bool hasCanvas()
        {
            return !string.IsNullOrEmpty(canvas);
        }

        public bool hasHints()
        {
            return !string.IsNullOrEmpty(dica) || !string.IsNullOrEmpty(dicaFerramenta);
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

        public TipoDado getTipoDado(Linguagem pLinguagem)
        {
            if (pLinguagem == Linguagem.Portugues)
            {
                switch (tiposDados)
                {
                    case "Número": return TipoDado.Numero;
                    case "Data": return TipoDado.Data;
                    default: return TipoDado.Caracter;
                }
            }
            else
            {
                switch (tiposDados)
                {
                    case "Number": return TipoDado.Numero;
                    case "Date": return TipoDado.Data;
                    default: return TipoDado.Caracter;
                }
            }
        }

        public string getAlinhamento(Linguagem pLinguagem = Linguagem.Portugues)
        {
            if(pLinguagem == Linguagem.Portugues)
            {
                if ((justificacao == "Final" || justificacao == "Direita"))
                    return "Direita";
                else
                    return "Esquerda";
            }
            else
            {
                if ((justificacao == "End" || justificacao == "Right"))
                    return "Direita";
                else
                    return "Esquerda";
            }
        }

        public TipoItem getTipoItem(Linguagem pLinguagem)
        {
            if (pLinguagem == Linguagem.Portugues)
            {
                switch (tipoItem)
                {
                    case "Item de Texto"   : return TipoItem.ItemTexto;
                    case "Item da Lista"   : return TipoItem.ItemLista;
                    case "Tecla"           : return TipoItem.Botao;
                    case "Caixa de Seleção": return TipoItem.CheckBox;
                    case "Grupo de Opções" : return TipoItem.RadioGroup;
                    case "Imagem"          : return TipoItem.Imagem;
                    default                : return TipoItem.Indefinido;
                }
            }
            else
            {
                switch (tipoItem)
                {
                    case "Text Item"   : return TipoItem.ItemTexto;
                    case "List Item"   : return TipoItem.ItemLista;
                    case "PushButton"  : return TipoItem.Botao;
                    case "CheckBox"    : return TipoItem.CheckBox;
                    case "Radio Group" : return TipoItem.RadioGroup;
                    case "Image"       : return TipoItem.Imagem;
                    default            : return TipoItem.Indefinido;
                }
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

}
