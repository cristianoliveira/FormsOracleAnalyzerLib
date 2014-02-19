using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OracleFormsAnalyzerLib.Objetos;
using System.IO;
using FormsAnalyzerLib;
using FormsAnalizerLib.Objetos;


/**
 * 
 * Autor: Cristian Oliveira (www.cristianoliveira.com.br)
 * 
 **/

namespace OracleFormsAnalyzerLib
{
    public class FormsAnalyzer
    {
        public RelatorioObjetos ro { get; set; }

        public System.ComponentModel.BackgroundWorker bgWork;

        public FormsAnalyzer(RelatorioObjetos pRO)
        {
            ro = pRO;
        }

        public Form getForm(System.ComponentModel.BackgroundWorker pBackgroundWorker)
        {
            bgWork = pBackgroundWorker;
            return getForm();
        }

        void reportProgress(int pPercentual, string pMensagem = null)
        {
            if(bgWork != null)
                bgWork.ReportProgress(pPercentual, pMensagem);
        }

        public Form getForm()
        {
            Form form = new Form();
            
            while (!ro.isFinal())
            {
                if (ro.isLabel("Nome") && form.nome == null)
                    form.nome = ro.getValor("Nome");
                else if (ro.isLabel("Informações sobre a Divisão em Subclasses") && form.subClassificacao == null)
                    form.subClassificacao = getSubclassificacao();
                else if (ro.isLabel("Gatilhos") && form.triggers == null)
                {
                    reportProgress(20, "Analisando Triggers...");
                    form.triggers = getTriggers();
                    ro.previus();
                }
                else if (ro.isLabel("Alertas") && form.alertas == null)
                {
                    reportProgress(25, "Analisando Alertas...");
                    form.alertas = getAlertas();
                    ro.previus();
                }
                else if (ro.isLabel("Bibliotecas PL/SQL Anexas") && form.bibliotecas == null)
                {
                    reportProgress(30, "Analisando Bibliotecas...");
                    form.bibliotecas = getBibliotecas();
                    ro.previus();
                }
                else if (ro.isLabel("Blocos") && form.blocos == null)
                {
                    reportProgress(35, "Analisando Blocos...");
                    form.blocos = getBlocks();
                    ro.previus();
                }
                else if (ro.isLabel("Canvases") && form.cavases == null)
                {
                    form.cavases = getCanvas();
                }
                else if (ro.isLabel("Listas de Valores") && form.lovs == null)
                {
                    reportProgress(80, "Analisando LOVs...");
                    form.lovs = getListasDeValores();
                }
                else if (ro.isLabel("Grupos de Registros"))
                {
                    reportProgress(85, "Analisando Record Groups...");
                    form.rgs = getGrupoDeRegistros();
                }
                ro.next();
            }

            return form;
        }

        public SubClassificacao     getSubclassificacao()
        {
            SubClassificacao subclass = new SubClassificacao();
            bool fimSubclass = false;

            ro.next();
            fimSubclass = (!ro.linha.Contains("----------"));

            while (!fimSubclass || ro.isFinal())
            {

                if (ro.isLabel("Local"))
                    subclass.local = ro.getValor("Local");
                else 
                    if (ro.isLabel("Arquivo de Origem"))
                    subclass.arquivoOrigem = ro.getValor("Arquivo de Origem");
                else 
                    if (ro.isLabel("Nome de Origem") && string.IsNullOrEmpty(subclass.nomeOrigem1))
                    subclass.nomeOrigem1 = ro.getValor("Nome de Origem");
                else if (ro.isLabel("Nome de Origem"))
                {
                    subclass.nomeOrigem2 = ro.getValor("Nome de Origem");
                    fimSubclass = true;    
                }
                else if (ro.isLabel("Título do Livro de Ajuda")
                       ||ro.isLabel("Comentários")
                       )
                {
                    fimSubclass = true;
                }
                ro.next();
            }

            ro.previus();
            return subclass;
        }

        private List<Trigger> getTriggers()
        {
            List<Trigger> triggers = new List<Trigger>();
            Trigger trigger = new Trigger();

            bool fimParteTrigger = false;
            while (!fimParteTrigger)
            {

                if (ro.isLabel("Nome"))
                {
                    trigger = new Trigger { nome = ro.getValor("Nome") };
                    triggers.Add(trigger);
                }
                else if (ro.isLabel("Informações sobre a Divisão em Subclasses"))
                    trigger.subClassificacao = getSubclassificacao();
                else if (ro.isLabel("Estilo do Gatilho"))
                    trigger.estiloGatilho = ro.getValor("Estilo do Gatilho");
                else if (ro.isLabel("Texto do Gatilho"))
                {
                    while (!ro.isLabel("Disparar no Modo Entrar Consultar"))
                    {
                        trigger.textoGatilho += ro.linha + "\n";
                        ro.next();
                    }
                    trigger.dispararModoConsultar = ro.getValor("Disparar no Modo Entrar Consultar");
                }
                else if (ro.isLabel("Hirarquia de Execução"))
                    trigger.HirarquiaExecucao = ro.getValor("Hirarquia de Execução");
                

                fimParteTrigger = ro.isLabel("Alertas") 
                               || ro.isLabel("Itens")
                               || ro.getLinhaNoIndice(ro.getIndice() + 2).Contains("Tipo de Item")
                               || ro.isLabel("Relações");
                ro.next();
            }
            ro.previus(2);
            return triggers;
        }

        private List<Alerta> getAlertas()
        {
            List<Alerta> alertas = new List<Alerta>();
            Alerta alerta = new Alerta();

            bool fimParte = false;
            while (!fimParte)
            {

                if (ro.isLabel("Nome") && !ro.isLabel("Nome da Fonte"))
                {
                    alerta = new Alerta { nome = ro.getValor("Nome") };
                    alertas.Add(alerta);
                }
                else if (ro.isLabel("Informações sobre a Divisão em Subclasses"))
                    alerta.subClassificacao = getSubclassificacao();

                fimParte = ro.isLabel("Bibliotecas PL/SQL Anexas");
                ro.next();
            }
            ro.previus(2);
            return alertas;
        }

        private List<Biblioteca> getBibliotecas()
        {
            List<Biblioteca> bibliotecas = new List<Biblioteca>();
            Biblioteca biblioteca = new Biblioteca();

            bool fimParte = false;
            while (!fimParte)
            {

                if (ro.isLabel("Nome"))
                {
                    biblioteca = new Biblioteca { nome = ro.getValor("Nome") };
                    bibliotecas.Add(biblioteca);
                }
                else if (ro.isLabel("Origem da Biblioteca PL/SQL"))
                    biblioteca.origemBiblioteca = ro.getValor("Origem da Biblioteca PL/SQL");
                else if (ro.isLabel("Local da Biblioteca PL/SQL"))
                    biblioteca.localBiblioteca = ro.getValor("Local da Biblioteca PL/SQL");

                fimParte = ro.isLabel("Blocos");
                ro.next();
            }
            ro.previus();
            return bibliotecas;
        }

        public List<Bloco> getBlocks()
        {
            int percentualAnalise = 35;

            List<Bloco> blocos = new List<Bloco>();
            Bloco bloco = new Bloco();
            bool fimParteBlocos = false;
            
            while(!ro.isLabel("Blocos"))
                ro.next();

            while (!fimParteBlocos || ro.isFinal())
            {
                if (ro.isLabel("Nome"))//inicio bloco
                {
                    bloco = new Bloco { nome = ro.getValor("Nome") };
                    reportProgress(percentualAnalise++,"Analisando Bloco " + bloco.nome+" ...");
                    blocos.Add(bloco);
                }
                else if (ro.isLabel("Informações sobre a Divisão em Subclasses") && bloco.subClassificacao == null)
                    bloco.subClassificacao = getSubclassificacao();
                else if (ro.isLabel("Estilo de Navegação"))
                    bloco.estiloNavegacao = ro.getValor("Estilo de Navegação");
                else if (ro.isLabel("Bloco de Dados Anterior de Navegação"))
                    bloco.blocoDadosAnteriorNavegacao = ro.getValor("Bloco de Dados Anterior de Navegação");
                else if (ro.isLabel("Próximo Bloco de Dados de Navegação"))
                    bloco.blocoDadosProximoNavegacao = ro.getValor("Próximo Bloco de Dados de Navegação");
                else if (ro.isLabel("Grupo de Atributos Visuais do Registro Atual"))
                    bloco.grupoAtributosVisuaisRegistroAtual = ro.getValor("Grupo de Atributos Visuais do Registro Atual");
                else if (ro.isLabel("Tamanho do Array de Consulta"))
                    bloco.tamanhoArrayConsulta = ro.getValor("Tamanho do Array de Consulta");
                else if (ro.isLabel("Número de Registros Armazenados no Buffer"))
                    bloco.numeroRegistrosArmazenadosBuffer = ro.getValor("Número de Registros Armazenados no Buffer");
                else if (ro.isLabel("Número de Registros Exibidos"))
                    bloco.numeroRegistrosExibidos = ro.getValor("Número de Registros Exibidos");
                else if (ro.isLabel("Consultar Todos os Registros"))
                    bloco.consultarTodosRegistros = ro.getValor("Consultar Todos os Registros");
                else if (ro.isLabel("Bloco de Dados do Banco de Dados"))
                    bloco.blocoDadosBancoDados = ro.getValor("Bloco de Dados do Banco de Dados");
                else if (ro.isLabel("Tipo de Origem de Dados de Consulta"))
                    bloco.tipoOrigemDadosConsulta = ro.getValor("Tipo de Origem de Dados de Consulta");
                else if (ro.isLabel("Nome de Origem dos Dados de Consulta"))
                    bloco.nomeOrigemDadosConsulta = ro.getValor("Nome de Origem dos Dados de Consulta");
                else if (ro.isLabel("Mostrar Barra de Rolagem"))
                    bloco.mostrarBarraRolagem = ro.getValor("Mostrar Barra de Rolagem");
                else if (ro.isLabel("Canvas da Barra de Rolagem"))
                    bloco.canvasBarraRolagem = ro.getValor("Canvas da Barra de Rolagem");
                else if (bloco.grupoAtributosVisuais == null && ro.isLabel("Grupo de Atributos Visuais"))
                    bloco.grupoAtributosVisuais = ro.getValor("Grupo de Atributos Visuais");
                else if (ro.isLabel("Gatilhos") && bloco.triggers == null)
                    bloco.triggers = getTriggers();
                else if (ro.isLabel("Itens") && bloco.itens == null)
                    bloco.itens = getItens();
                else if (ro.getLinhaNoIndice(ro.getIndice() + 2).Contains(" Tipo de Relação "))
                    getRelations();
                
                fimParteBlocos = ro.isLabel("Canvases");
               
                ro.next();
            }
            ro.previus();
            return blocos;
        }

        private List<Item> getItens()
        {
            List<Item> itens = new List<Item>();
            Item item = new Item();
            bool fimParteItens = false;

            while (!fimParteItens)
            {

                if (ro.isLabel("Nome"))
                {
                    item = new Item { nome = ro.getValor("Nome") };
                    itens.Add(item);
                }
                else if (ro.isLabel("Tipo de Item  "))
                    item.tipoItem = ro.getValor("Tipo de Item");
                else if (ro.isLabel("Informações sobre a Divisão em Subclasses"))
                    item.subClassificacao = getSubclassificacao();
                else if (ro.isLabel("Tópico do Livro de Ajuda"))
                    item.topicoLivroAjuda = ro.getValor("Tópico do Livro de Ajuda");
                else if (ro.isLabel("Ativado"))
                    item.ativado = ro.getValor("Ativado");
                else if (ro.isLabel("Justificação"))
                    item.justificacao = ro.getValor("Justificação");
                else if (ro.isLabel("Várias Linhas"))
                    item.variasLinhas = ro.getValor("Várias Linhas");
                else if (ro.isLabel("Restrição a Maiúsculas/Minúsculas"))
                    item.restriçãoMaiusculasMinusculas = ro.getValor("Restrição a Maiúsculas/Minúsculas");
                else if (ro.isLabel("Número de Itens Exibidos"))
                    item.numeroItensExibidos = ro.getValor("Número de Itens Exibidos");
                else if (ro.isLabel("Item Anterior de Navegação"))
                    item.itemAnteriorNavegacao = ro.getValor("Item Anterior de Navegação");
                else if (ro.isLabel("Próximo Item de Navegação"))
                    item.proximoItemNavegacao = ro.getValor("Próximo Item de Navegação");
                else if (ro.isLabel("Tipos de Dados"))
                    item.tiposDados = ro.getValor("Tipos de Dados");
                else if (ro.isLabel("Tamanho Máximo"))
                    item.tamanhoMaximo = ro.getValor("Tamanho Máximo");
                else if (ro.isLabel("Tamanho Fixo"))
                    item.tamanhoFixo = ro.getValor("Tamanho Fixo");
                else if (ro.isLabel("Valor Inicial"))
                    item.valorInicial = ro.getValor("Valor Inicial");
                else if (ro.isLabel("Obrigatório"))
                    item.obrigatorio = ro.getValor("Obrigatório");
                else if (ro.isLabel("Máscara de Formato"))
                    item.mascaraFormato = ro.getValor("Máscara de Formato");
                else if (ro.isLabel(" Canvas  "))
                    item.canvas = ro.getValor("Canvas");
                else if (ro.isLabel(" Nome da Fonte  "))
                    item.nomeFonte = ro.getValor("Nome da Fonte");
                else if (ro.isLabel(" Tamanho da Fonte  "))
                    item.tamanhoFonte = ro.getValor("Tamanho da Fonte");
                else if (ro.isLabel("Peso da Fonte") && !ro.isLabel("Peso da Fonte do Prompt"))
                    item.pesoFonte = ro.getValor("Peso da Fonte");
                else if (ro.isLabel("Cor de Fundo"))
                    item.corFundo = ro.getValor("Cor de Fundo");
                else if (ro.isLabel(" Nome da Fonte do Prompt  "))
                    item.nomeFontePrompt = ro.getValor("Nome da Fonte do Prompt");
                else if (ro.isLabel(" Tamanho da Fonte do Prompt  "))
                    item.tamanhoFontePrompt = ro.getValor("Tamanho da Fonte do Prompt");
                else if (ro.isLabel(" Altura  "))
                    item.altura = ro.getValor("Altura");
                else if (ro.isLabel(" Dica  "))
                    item.dica = ro.getValor("Dica");
                else if (ro.isLabel("Prompt"))
                    item.prompt = ro.getValor("Prompt");
                else if (ro.isLabel("Alinhamento do Prompt") && !ro.isLabel("Deslocamento do Alinhamento do Prompt"))
                    item.alinhamentoPrompt = ro.getValor("Alinhamento do Prompt");
                else if (ro.isLabel("Limite de Conexão do Prompt"))
                    item.limiteConexaoPrompt = ro.getValor("Limite de Conexão do Prompt");
                else if (ro.isLabel("Deslocamento de Conexão do Prompt"))
                    item.deslocamentoConexaoPrompt = ro.getValor("Deslocamento de Conexão do Prompt");
                else if (ro.isLabel("Deslocamento do Alinhamento do Prompt"))
                    item.deslocamentoAlinhamentoPrompt = ro.getValor("Deslocamento do Alinhamento do Prompt");
                else if (ro.isLabel("Peso da Fonte do Prompt"))
                    item.pesoFontePrompt = ro.getValor("Peso da Fonte do Prompt");
                else if (ro.isLabel(" Dica de ferramenta "))
                    item.dicaFerramenta = ro.getValor("Dica de ferramenta");
                else if (ro.isLabel(" Gatilhos ")) //Fim do item
                    item.triggers = getTriggers();
                else if (ro.isLabel("Relações"))
                    fimParteItens = true;
                ro.next();
            }
            ro.previus();
            return itens;
        }

        private void getRelations()
        {
            /* Como não vai ser usado vou "Pular este bloco" */
            while (!ro.isLabel("Consulta Automática") || ro.isFinal())
                ro.next();
        }

        private List<Canvas> getCanvas()
        {
            List<Canvas> canvases = new List<Canvas>();
            Canvas canvas = new Canvas();
            bool fimParteCanvas = false;

            while(!fimParteCanvas)
            {
                if (ro.isLabel("Nome") && ro.isLabel("Tipo de Canvas", ro.getIndice()+1))
                {
                    canvas = new Canvas { nome = ro.getValor("Nome") };
                    canvases.Add(canvas);
                }
                else if (ro.isLabel("Parâmetros do Form"))
                    fimParteCanvas = true;

                ro.next();
            }

            ro.previus();
            return canvases;
        }

        private List<ListaDeValores> getListasDeValores()
        {
            List<ListaDeValores> lovs = new List<ListaDeValores>();
            ListaDeValores lov = new ListaDeValores();
            bool fimParteLovs = false;

            while (!fimParteLovs)
            {
                if (ro.isLabel("Nome"))
                {
                    lov = new ListaDeValores { nome = ro.getValor("Nome") };
                    lovs.Add(lov);
                }
                else if (ro.isLabel("Informações sobre a Divisão em Subclasses"))
                    lov.subClassificacao = getSubclassificacao();
                else if (ro.isLabel("Propriedades de Mapeamento de Coluna"))
                {
                    lov.mapeamentoColunas = new List<MapeamentoColuna>();
                    MapeamentoColuna mapeamento = new MapeamentoColuna();
                    while (!ro.isLabel("Filtrar Antes da Exibição"))
                    {
                        if (ro.isLabel("Nome"))
                        {
                            mapeamento = new MapeamentoColuna { nome = ro.getValor("Nome") };
                            lov.mapeamentoColunas.Add(mapeamento);
                        }
                        else if (ro.isLabel("Título"))
                            mapeamento.titulo = ro.getValor("Título");
                        else if (ro.isLabel("Retornar Item"))
                            mapeamento.retornaItem = ro.getValor("Retornar Item");
                        else if (ro.isLabel("Largura da Exibição"))
                            mapeamento.larguraExibicao = ro.getValor("Largura da Exibição");
				
                        ro.next();
                    }
                    ro.previus();
                }
                else if (ro.isLabel("Nome da Fonte"))
                	lov.nomeFonte = ro.getValor("Nome da Fonte");
                else if (ro.isLabel("Tamanho da Fonte"))
                	lov.tamanhoFonte = ro.getValor("Tamanho da Fonte");
                else if (ro.isLabel("Menus"))
                    fimParteLovs = true;

                ro.next();
            }

            ro.previus();
            return lovs;
        }

        private List<GrupoDeRegistros> getGrupoDeRegistros()
        {
            List<GrupoDeRegistros> rgs = new List<GrupoDeRegistros>();
            GrupoDeRegistros rg = new GrupoDeRegistros();
            bool fimParteGrupoRegistros = false;

            while (!fimParteGrupoRegistros)
            {
                if (ro.isLabel("Nome") && ro.isLabel("Informações sobre a Divisão em Subclasses", ro.getIndice() + 1))
                {
                    rg = new GrupoDeRegistros { nome = ro.getValor("Nome") };
                    rgs.Add(rg);
                }
                else if (ro.isLabel("Informações sobre a Divisão em Subclasses"))
                    rg.subClassificacao = getSubclassificacao();
                else if (ro.isLabel("Tipo de Grupo de Registros"))
                    rg.tipoGrupoDeRegistros = ro.getValor("Tipo de Grupo de Registros");
                else if (ro.isLabel("Consulta do Grupo de Registros"))
                {
                    while (!ro.isLabel("Tamanho de Extração do Grupo de Registros"))
                    {
                        rg.consultaGrupoDeRegistros += ro.linha + "\n";
                        ro.next();
                    }
                    ro.previus(-3);
                }
                else if (ro.isLabel("Tamanho de Extração do Grupo de Registros"))
                    rg.tamanhoExtracaoGrupoDeRegistros = ro.getValor("Tamanho de Extração do Grupo de Registros");
                /* else if (ro.isLabel("Especificações da Coluna"))
                 {
                     rg.especificacesColuna = new List<EspecificacaoColuna>();
                     bool fimEspecificacaoColuna = false;
                     while (!fimEspecificacaoColuna)
                     {
                         EspecificacaoColuna espCol = new EspecificacaoColuna();
                         if (ro.isLabel("Nome") && !ro.isLabel("Informações sobre a Divisão em Subclasses", ro.getIndice() + 1))
                         {
                             espCol = new EspecificacaoColuna { nome = ro.getValor("Nome") };
                             rg.especificacesColuna.Add(espCol);
                         }
                         else if (ro.isLabel("Tamanho Máximo"))
                             espCol.tamanhoMaximo = ro.getValor("Tamanho Máximo");
                         else if (ro.isLabel("Tipo de Dados da Coluna"))
                             espCol.tipoDadosColuna = ro.getValor("Tipo de Dados da Coluna");
                         else if (ro.isLabel("Lista de Valores da Coluna"))
                             espCol.listaValoresColuna = ro.getValor("Lista de Valores da Coluna");

                         if (ro.isLabel("Relatórios"))
                             fimEspecificacaoColuna = true;
                         else if (ro.isLabel("Tipo de Grupo de Registros", ro.getIndice() + 3))
                             fimEspecificacaoColuna = true;
                         else if (ro.isLabel("Atributos Visuais", ro.getIndice() + 2))
                             fimEspecificacaoColuna = true;
                         ro.next();
                     }
                 *
                 }*/
                else if (ro.isLabel("Relatórios"))
                    fimParteGrupoRegistros = true;   
                 ro.next();
            }

            ro.previus();
            return rgs;

        }


    }
}
