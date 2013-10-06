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
            return canvas != null ? !string.IsNullOrEmpty(canvas) : false;
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

        public string getAlinhamento()
        {
            if ((justificacao == "Final" || justificacao == "Direita"))
                return "Direita";
            else
                return "Esquerda";
        }
    }
}
