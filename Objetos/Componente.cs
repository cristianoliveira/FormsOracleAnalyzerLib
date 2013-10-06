using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleFormsAnalyzerLib.Objetos
{
    public class Componente
    {
        public string nome             { get; set; }
        public SubClassificacao subClassificacao { get; set; }
        public List<Trigger> triggers { get; set; }
        public string strSubClass
        {
            get { return getSubClassificacao(); }
        }

        public bool isSubClassificado()
        {
            return (subClassificacao != null && !string.IsNullOrEmpty(subClassificacao.nomeOrigem2));
        }

        public string getSubClassificacao()
        {
            if (subClassificacao == null)
                return "NULL";
            else
                return subClassificacao.nomeOrigem2;
        }
    }
}
