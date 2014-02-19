using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OracleFormsAnalyzerLib.Objetos
{
    public class Bloco : Componente
    {
        public string estiloNavegacao                     { get; set; }
        public string blocoDadosAnteriorNavegacao         { get; set; }
        public string blocoDadosProximoNavegacao          { get; set; }
        public string grupoAtributosVisuaisRegistroAtual  { get; set; }
        public string tamanhoArrayConsulta                { get; set; }
        public string numeroRegistrosArmazenadosBuffer    { get; set; }
        public string numeroRegistrosExibidos             { get; set; }
        public string consultarTodosRegistros             { get; set; }
        public string blocoDadosBancoDados                { get; set; }
        public string tipoOrigemDadosConsulta             { get; set; }
        public string nomeOrigemDadosConsulta             { get; set; }
        public string canvasBarraRolagem                  { get; set; }
        public string grupoAtributosVisuais               { get; set; }
        public string mostrarBarraRolagem                 { get; set; }

        public List<Item> itens { get; set; }

        public bool isScrollbarVisible()
        {
            return mostrarBarraRolagem == "Sim" || mostrarBarraRolagem == "Yes";
        }
        
        public bool isMultirecord()
        {
        	int numRegistros = 0;
        	try {
        		numRegistros = int.Parse(numeroRegistrosExibidos);	
        	} catch (Exception) {}
        	
        	return numRegistros>1;
        }
    }
}
