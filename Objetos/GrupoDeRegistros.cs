using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleFormsAnalyzerLib.Objetos;

namespace FormsAnalizerLib.Objetos
{
    public class GrupoDeRegistros : Componente
    {
        public string tipoGrupoDeRegistros              { get; set; }
        public string consultaGrupoDeRegistros          { get; set; }
        public string tamanhoExtracaoGrupoDeRegistros   { get; set; }
        public List<EspecificacaoColuna> especificacesColuna { get; set; }
    }

    public class EspecificacaoColuna
    {
        public string nome               { get; set; }
        public string tamanhoMaximo      { get; set; }
        public string tipoDadosColuna    { get; set; }
        public string listaValoresColuna { get; set; }
    }
}
