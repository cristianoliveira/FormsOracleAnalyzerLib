using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OracleFormsAnalyzerLib.Objetos;

namespace FormsAnalyzerLib.Objetos
{
    public class GrupoRegistro : Componente
    {
        public string tipoGrupoRegistros            { get; set; }
        public string consultaGrupoRegistros        { get; set; }
        public string tamanhoExtracaoGrupoRegistros { get; set; }
        public string especificacoesdaColuna        { get; set; }

    }

    public class ColunaGrupoRegistro
    {
        public string nome                  { get; set; }
        public string tamanhoMaximo         { get; set; }
        public TipoDado TiposDado                  { get; set; }
        
    }
}
