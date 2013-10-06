using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleFormsAnalyzerLib.Objetos
{
    public class Trigger : Componente
    {
        public string estiloGatilho         { get; set; }
        public string textoGatilho          { get; set; }
        public string DispararModoConsultar { get; set; }
        public string HirarquiaExecucao     { get; set; }
    
    }
}
