using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleFormsAnalyzerLib.Objetos
{
    public class Form : Componente
    {
        public string TituloLivroAjuda  { get; set; }
        public string Titulo            { get; set; }
        public string JanelaConsole     { get; set; }

        List<Bloco> blocos              { get; set; }
    }
}
