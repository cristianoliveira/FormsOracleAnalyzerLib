using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleFormsAnalyzerLib.Objetos;

namespace FormsAnalyzerLib.Objetos
{
    public class ListaValores : Componente
    {                                                  
        public string comentarios                        { get; set; }
        public string titulo                             { get; set; }
        public string tipoLista                          { get; set; }
        public string grupoRegistros                     { get; set; }
        public List<MapeamentoColuna> mapeamentosColuna  { get; set; }
        public string filtrarAntesExibicao               { get; set; }
        public string exibicaoAutomatica                 { get; set; }
        public string renovacaoAutomatica                { get; set; }
        public string selecaoAutomatica                  { get; set; }
        public string saltoAutomatico                    { get; set; }
        public string posicaoAutomatica                  { get; set; }
        public string larguraAutomaticaColuna            { get; set; }
        public string posicaoX                           { get; set; }
        public string posicaoY                           { get; set; }
        public string largura                            { get; set; }
        public string altura                             { get; set; }
        public string corFundo                           { get; set; }
        public string nomeFonte                          { get; set; }
        public string tamanhoFonte                       { get; set; }
        public string pesoFonte                          { get; set; }
    }

    public class MapeamentoColuna
    {
        public string nome                      { get; set; }
        public string titulo                    { get; set; }
        public string retornaNoItem             { get; set; }
        public string larguraExibicao           { get; set; }
    }
}
