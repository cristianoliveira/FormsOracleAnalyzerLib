using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormsAnalizerLib.Objetos;

namespace OracleFormsAnalyzerLib.Objetos
{
    public class Form : Componente
    {
        public string TituloLivroAjuda         { get; set; }
        public string Titulo                   { get; set; }
        public List<Alerta> alertas            { get; set; }
        public List<Biblioteca> bibliotecas    { get; set; }
        public List<Bloco> blocos              { get; set; }
        public List<Canvas> cavases            { get; set; }
        public List<ListaDeValores> lovs       { get; set; }
        public List<GrupoDeRegistros> rgs      { get; set; }
    }
}
