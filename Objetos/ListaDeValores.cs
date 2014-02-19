using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleFormsAnalyzerLib.Objetos;

namespace FormsAnalizerLib.Objetos
{
    public class ListaDeValores : Componente
    {
        public string titulo                            { get; set; }
        public string tipoLista                         { get; set; }
        public string GrupoRegistros                    { get; set; }
        public string nomeFonte { get; set; }
        public string tamanhoFonte { get; set; }
        public List<MapeamentoColuna> mapeamentoColunas { get; set; } 

    }

    public class MapeamentoColuna
    {
        public string nome              { get; set; }
        public string titulo            { get; set; }
        public string retornaItem       { get; set; }
        public string larguraExibicao   { get; set; }
    }
}
