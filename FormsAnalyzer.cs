using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OracleFormsAnalyzerLib.Objetos;
using System.IO;


/**
 * 
 * Autor: Cristian Oliveira (www.cristianoliveira.com.br)
 * 
 **/

namespace OracleFormsAnalyzerLib
{
    public class FormsAnalyser
    {
        string[] linhasArquivo = new string[]{""};
        Linguagem linguagem  = Linguagem.Portugues;
        int _indiceAtual = 0;
        int indiceAtual { get { return _indiceAtual; } }
        
        Regex regex;

        List<string> blocosDefault = new List<string> { "CONTROLE", "TABS", "TOOLBAR", "PROCESSOS", "LOGOEMP", "TEMPRESAS" };

        List<Bloco> blocos = new List<Bloco>();
        Bloco bloco = new Bloco();

        public void setLinhasArquivo(string[] pLinhas)
        {
            linhasArquivo = pLinhas;
        }

        public void setIndiceAtual(int pIndice)
        {
            _indiceAtual = pIndice;
        }

        public void setLinguagem(Linguagem pLinguagem)
        {
            linguagem = pLinguagem;
        }

        public bool isLabel(string pLinha, string pLabel)
        {
            regex = new Regex(@" ([\*\-o\^]) (" + pLabel.Trim() + ") ");
            return regex.IsMatch(pLinha);
            //var match = Regex.Match(pLinha, @"^ ([\*\-o\^]) ("+pLabel.Trim()+") ");
            //return match.Success;
        }

        internal bool isValor(string linha, string valor)
        {
            return linha.Contains(valor);
        }

        internal string getValor(string valor)
        {
            string returno = null;
            string linha = linhasArquivo[indiceAtual];
            returno = linha.Substring(linha.IndexOf(valor) + valor.Length).Trim();
            return returno;
        }


        public List<Bloco> getBlocks(bool pFiltraBlocosDefault) 
        {
            var listBlocos = new List<Bloco>();
            listBlocos = linguagem == Linguagem.Portugues ? getBlocksPT(pFiltraBlocosDefault) : getBlocksEN(pFiltraBlocosDefault);
            return listBlocos; //informa que acabou
        }

        public List<Bloco> getBlocksPT(bool pFiltraBlocosDefault)
        {
            var listBlocos = new List<Bloco>();
            //realiza novo loop apartir da linha referente aos dados do bloco
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);

                if (pFiltraBlocosDefault)
                    if (blocosDefault.Contains(bloco.nome))
                    {
                        i = pulaFimDadosBloco();
                        listBlocos.Remove(bloco);
                        bloco = new Bloco();
                    }
                string linha = linhasArquivo[i];
                if (linha.Contains("* Nome                                            "))//inicio bloco
                {
                    bloco = new Bloco { nome = getValor("Nome") };
                    listBlocos.Add(bloco);
                }
                else if (linha.Contains(" Informações sobre a Divisão em Subclasses       "))
                {
                    bloco.subClassificacao = getSubClassificacaoPT();
                    i = indiceAtual;
                }
                else if (linha.Contains(" Estilo de Navegação  "))
                    bloco.estiloNavegacao = getValor("Estilo de Navegação");
                else if (linha.Contains(" Bloco de Dados Anterior de Navegação  "))
                    bloco.blocoDadosAnteriorNavegacao = getValor("Bloco de Dados Anterior de Navegação");
                else if (linha.Contains(" Próximo Bloco de Dados de Navegação"))
                    bloco.blocoDadosProximoNavegacao = getValor("Próximo Bloco de Dados de Navegação");
                else if (linha.Contains(" Grupo de Atributos Visuais do Registro Atual "))
                    bloco.grupoAtributosVisuaisRegistroAtual = getValor("Grupo de Atributos Visuais do Registro Atual");
                else if (linha.Contains(" Tamanho do Array de Consulta  "))
                    bloco.tamanhoArrayConsulta = getValor("Tamanho do Array de Consulta");
                else if (linha.Contains(" Número de Registros Armazenados no Buffer  "))
                    bloco.numeroRegistrosArmazenadosBuffer = getValor("Número de Registros Armazenados no Buffer");
                else if (linha.Contains(" Número de Registros Exibidos  "))
                    bloco.numeroRegistrosExibidos = getValor("Número de Registros Exibidos");
                else if (linha.Contains(" Consultar Todos os Registros "))
                    bloco.consultarTodosRegistros = getValor("Consultar Todos os Registros");
                else if (linha.Contains(" Bloco de Dados do Banco de Dados "))
                    bloco.blocoDadosBancoDados = getValor("Bloco de Dados do Banco de Dados");
                else if (linha.Contains(" Tipo de Origem de Dados de Consulta "))
                    bloco.tipoOrigemDadosConsulta = getValor("Tipo de Origem de Dados de Consulta");
                else if (linha.Contains(" Nome de Origem dos Dados de Consulta            "))
                    bloco.nomeOrigemDadosConsulta = getValor("Nome de Origem dos Dados de Consulta");
                else if (linha.Contains(" Mostrar Barra de Rolagem  "))
                    bloco.mostrarBarraRolagem = getValor("Mostrar Barra de Rolagem");
                else if (bloco.grupoAtributosVisuais == null && linha.Contains(" Grupo de Atributos Visuais   "))
                    bloco.grupoAtributosVisuais = getValor("Grupo de Atributos Visuais");
                else if (linha.Contains(" Gatilhos  "))
                {
                    bloco.triggers = getTriggersPT();
                    i = indiceAtual - 1;
                }
                else if (linha.Contains(" Itens  "))
                {
                    bloco.itens = getItensPT();
                    i = indiceAtual;
                }
                if (linha.Contains("* Canvases  ")) //fim area de blocos
                    return listBlocos;
            }
            return listBlocos; //informa que acabou
        }

        public List<Bloco> getBlocksEN(bool pFiltraBlocosDefault)
        {
            var listBlocos = new List<Bloco>();
            //realiza novo loop apartir da linha referente aos dados do bloco
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);

                if (pFiltraBlocosDefault)
                    if (blocosDefault.Contains(bloco.nome))
                    {
                        i = pulaFimDadosBloco();
                        listBlocos.Remove(bloco);
                        bloco = new Bloco();
                    }
                string linha = linhasArquivo[indiceAtual];
                if (isLabel(linha, "Name"))//inicio bloco
                {
                    bloco = new Bloco { nome = getValor("Name") };
                    listBlocos.Add(bloco);
                }
                else if (isLabel(linha, " Subclass Information  "))
                {
                    bloco.subClassificacao = getSubClassificacaoEN();
                    i = indiceAtual;
                }
                else if (isLabel(linha, " Navigation Style  "))
                    bloco.estiloNavegacao = getValor("Navigation Style");
                else if (isLabel(linha, " Previous Navigation Data Block  "))
                    bloco.blocoDadosAnteriorNavegacao = getValor("Previous Navigation Data Block");
                else if (isLabel(linha, " Next Navigation Data Block  "))
                    bloco.blocoDadosProximoNavegacao = getValor("Next Navigation Data Block");
                else if (isLabel(linha, " Current Record Visual Attribute Group  "))
                    bloco.grupoAtributosVisuaisRegistroAtual = getValor("Current Record Visual Attribute Group");
                else if (isLabel(linha, " Query Array Size  "))
                    bloco.tamanhoArrayConsulta = getValor("Query Array Size");
                else if (isLabel(linha, " Number of Records Buffered  "))
                    bloco.numeroRegistrosArmazenadosBuffer = getValor("Number of Records Buffered");
                else if (isLabel(linha, " Number of Records Displayed  "))
                    bloco.numeroRegistrosExibidos = getValor("Number of Records Displayed");
                else if (isLabel(linha, " Query All Records  "))
                    bloco.consultarTodosRegistros = getValor("Query All Records");
                else if (isLabel(linha, " Database Data Block  "))
                    bloco.blocoDadosBancoDados = getValor("Database Data Block");
                else if (isLabel(linha, " Query Data Source Type  "))
                    bloco.tipoOrigemDadosConsulta = getValor("Query Data Source Type");
                else if (isLabel(linha, " Query Data Source Name  "))
                    bloco.nomeOrigemDadosConsulta = getValor("Query Data Source Name");
                else if (isLabel(linha, " Show Scroll Bar  "))
                    bloco.mostrarBarraRolagem = getValor("Show Scroll Bar");
                else if (bloco.grupoAtributosVisuais == null && isLabel(linha, " Visual Attribute Group  "))
                    bloco.grupoAtributosVisuais = getValor("Visual Attribute Group");
                else if (isLabel(linha, " Triggers  "))
                {
                    bloco.triggers = getTriggersEN();
                    i = indiceAtual - 1;
                }
                else if (isLabel(linha, " Items  "))
                {
                    bloco.itens =  getItensEN();
                    i = indiceAtual;

                    linha = linhasArquivo[indiceAtual];
                    if (isLabel(linha, "Relations"))
                    {
                        string linhaPosterior = linhasArquivo[indiceAtual + 3];
                        if (isLabel(linhaPosterior, " Relation Type   "))
                        {
                            i = pulaParteRelations();
                        }
                    }   
                }
                else if (isLabel(linha, "Canvases")) //fim area de blocos
                    return listBlocos;
            }
            return listBlocos; //informa que acabou
        }

        public SubClassificacao getSubClassificacaoPT()
        {
            setIndiceAtual(indiceAtual + 1);

            SubClassificacao subClassificacao = new SubClassificacao();
           
            //realiza novo loop apartir da linha referente aos dados da subclassificacao
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[i];

                    if (isValor(linha, " Local  "))
                        subClassificacao.local = getValor("Local");
                    else if (isValor(linha, " Arquivo de Origem   "))
                        subClassificacao.arquivoOrigem = getValor("Arquivo de Origem");
                    else if (isValor(linha, " Nome de Origem  ") && string.IsNullOrEmpty(subClassificacao.nomeOrigem1))
                        subClassificacao.nomeOrigem1 = getValor("Nome de Origem");
                    else if (isValor(linha, " Nome de Origem   "))
                        subClassificacao.nomeOrigem2 = getValor("Nome de Origem");
                    else if (isValor(linha, " Comentários  "))
                        return subClassificacao;
                
            }
            return subClassificacao; //informa que acabou
        }

        public SubClassificacao getSubClassificacaoEN()
        {
            setIndiceAtual(indiceAtual + 1);

            SubClassificacao subClassificacao = new SubClassificacao();

            //realiza novo loop apartir da linha referente aos dados da subclassificacao
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[i];

                    if (isLabel(linha, " Location "))
                        subClassificacao.local = getValor("Location");
                    else if (isLabel(linha, " Source File  "))
                        subClassificacao.arquivoOrigem = getValor("Source File");
                    else if (isLabel(linha, " Source Name ") && string.IsNullOrEmpty(subClassificacao.nomeOrigem1))
                        subClassificacao.nomeOrigem1 = getValor("Source Name");
                    else if (isLabel(linha, " Source Name   "))
                        subClassificacao.nomeOrigem2 = getValor("Source Name");
                    else if (isLabel(linha, "Comments"))
                        return subClassificacao;
            }
            return subClassificacao; //informa que acabou
        }

        public List<Item> getItensPT()
        {
            setIndiceAtual(indiceAtual + 1); 

            List<Item> itens = new List<Item>();
            Item item = new Item();
                
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[i];

                if (linha.Contains("* Nome   "))
                    item = new Item { nome = getValor("Nome") };
                else if (linha.Contains(" Tipo de Item  "))
                    item.tipoItem = getValor("Tipo de Item");
                else if (linha.Contains("* Informações sobre a Divisão em Subclasses     "))
                    item.subClassificacao = getSubClassificacaoPT();
                else if (linha.Contains(" Tópico do Livro de Ajuda"))
                    item.topicoLivroAjuda = getValor("Tópico do Livro de Ajuda");
                else if (linha.Contains(" Ativado  "))
                    item.ativado = getValor("Ativado");
                else if (linha.Contains(" Justificação      "))
                    item.justificacao = getValor("Justificação");
                else if (linha.Contains(" Item Anterior de Navegação  "))
                    item.itemAnteriorNavegacao = getValor("Item Anterior de Navegação");
                else if (linha.Contains(" Próximo Item de Navegação  "))
                    item.proximoItemNavegacao = getValor("Próximo Item de Navegação");
                else if (linha.Contains(" Tipos de Dados  "))
                    item.tiposDados = getValor("Tipos de Dados");
                else if (linha.Contains(" Tamanho Máximo   "))
                    item.tamanhoMaximo = getValor("Tamanho Máximo");
                else if (linha.Contains(" Tamanho Fixo    "))
                    item.tamanhoFixo = getValor("Tamanho Fixo");
                else if (linha.Contains("Valor Inicial  "))
                    item.valorInicial = getValor("Valor Inicial");
                else if (linha.Contains(" Obrigatório   "))
                    item.obrigatorio = getValor("Obrigatório");
                else if (linha.Contains(" Máscara de Formato   "))
                    item.mascaraFormato = getValor("Máscara de Formato");
                else if (linha.Contains(" Canvas  "))
                    item.canvas = getValor("Canvas");
                else if (linha.Contains(" Nome da Fonte  "))
                    item.nomeFonte = getValor("Nome da Fonte");
                else if (linha.Contains(" Tamanho da Fonte  "))
                    item.tamanhoFonte = getValor("Tamanho da Fonte");
                else if (linha.Contains(" Nome da Fonte do Prompt  "))
                    item.nomeFontePrompt = getValor("Nome da Fonte do Prompt");
                else if (linha.Contains(" Tamanho da Fonte do Prompt  "))
                    item.tamanhoFontePrompt = getValor("Tamanho da Fonte do Prompt");
                else if (linha.Contains(" Altura  "))
                    item.altura = getValor("Altura");
                else if (linha.Contains(" Dica  "))
                    item.dica = getValor("Dica");
                else if (linha.Contains(" Dica de ferramenta "))
                    item.dicaFerramenta = getValor("Dica de ferramenta");
                else if (linha.Contains(" Gatilhos ")) //Fim do item
                {
                    item.triggers = getTriggersPT();
                    itens.Add(item);
                }
                else if (linha.Contains(" Relações "))
                {
                    if (linhasArquivo[indiceAtual + 2].Contains(" Tipo de Relação "))
                        setIndiceAtual(pulaParteRelations());
                    else
                        return itens;
                    // Fim dos itens do Bloco
                } 
                
            }
            return itens; 
        }

        public List<Item> getItensEN()
        {
            setIndiceAtual(indiceAtual + 1);

            List<Item> itens = new List<Item>();
            Item item = new Item();

            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[indiceAtual];

                if (isLabel(linha,"Name"))
                    item = new Item { nome = getValor("Name") };
                else if (isLabel(linha,"Item Type"))
                    item.tipoItem = getValor("Item Type");
                else if (isLabel(linha, (" Subclass Information  ")))
                {
                    item.subClassificacao = getSubClassificacaoEN();
                    i = indiceAtual;
                }
                else if (isLabel(linha, (" Help Book Topic  ")))
                    item.topicoLivroAjuda = getValor("Help Book Topic");
                else if (isLabel(linha, (" Enabled  ")))
                    item.ativado = getValor("Enabled");
                else if (isLabel(linha, (" Justification  ")))
                    item.justificacao = getValor("Justification");
                else if (isLabel(linha, (" Previous Navigation Item  ")))
                    item.itemAnteriorNavegacao = getValor("Previous Navigation Item");
                else if (isLabel(linha, (" Next Navigation Item  ")))
                    item.proximoItemNavegacao = getValor("Next Navigation Item");
                else if (isLabel(linha, (" Data Type  ")))
                    item.tiposDados = getValor("Data Type");
                else if (isLabel(linha, (" Maximum Length  ")))
                    item.tamanhoMaximo = getValor("Maximum Length");
                else if (isLabel(linha, (" Fixed Length  ")))
                    item.tamanhoFixo = getValor("Fixed Length");
                else if (isLabel(linha, " Initial Value  "))
                    item.valorInicial = getValor("Initial Value");
                else if (isLabel(linha, (" Required  ")))
                    item.obrigatorio = getValor("Required");
                else if (isLabel(linha, " Format Mask  "))
                    item.mascaraFormato = getValor("Format Mask");
                else if (isLabel(linha, " Canvas  "))
                    item.canvas = getValor("Canvas");
                else if (isLabel(linha, " Height  "))
                    item.altura = getValor("Height");
                else if (isLabel(linha, " Font Name  ") && !isLabel(linha, (" Prompt Font Name  ")))
                    item.nomeFonte = getValor("Font Name");
                else if (isLabel(linha, " Font Size  ") && !isLabel(linha, (" Prompt Font Size  ")))
                    item.tamanhoFonte = getValor("Font Size");
                else if (isLabel(linha, " Prompt Font Name  "))
                    item.nomeFontePrompt = getValor("Prompt Font Name");
                else if (isLabel(linha, " Prompt Font Size  "))
                    item.tamanhoFontePrompt = getValor("Prompt Font Size");
                else if (isLabel(linha, " Hint  "))
                    item.dica = getValor("Hint");
                else if (isLabel(linha, " Tooltip  ") && !isLabel(linha, "Tooltip Visual Attribute Group"))
                    item.dicaFerramenta = getValor("Tooltip");
                else if (isLabel(linha, " Triggers  ")) //Fim do item
                {
                    item.triggers = getTriggersEN();
                    itens.Add(item);
                    i = indiceAtual;
                }
                else if (isLabel(linha, " Relations  "))
                {
                    return itens;// Fim dos itens do Bloco
                }

            }
            return itens;
        }

        internal List<Trigger> getTriggersPT()
        {
            setIndiceAtual(indiceAtual + 1);
            List<Trigger> listTrigger = new List<Trigger>();
            Trigger trigger = null;
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[indiceAtual];
                
                    if (linha.Contains(" Nome "))
                        trigger = new Trigger { nome = getValor(" Nome ") };
                    else if (linha.Contains(" Estilo do Gatilho "))
                        trigger.estiloGatilho = getValor(" Estilo do Gatilho ");
                    else if (linha.Contains(" Texto do Gatilho "))
                    {
                        bool fimDaTrigger = false;
                        while (!fimDaTrigger)
                        {
                            setIndiceAtual(indiceAtual + 1);
                            linha = linhasArquivo[indiceAtual];
                            if (!linha.Contains(" Disparar no Modo Entrar Consultar "))
                            {
                                trigger.textoGatilho += linha + "\n";
                            }
                            else
                                fimDaTrigger = true;
                        }
                        trigger.dispararModoConsultar = getValor("Disparar no Modo Entrar Consultar");
                    }
                    else if (linha.Contains(" Hirarquia de Execução "))
                        trigger.HirarquiaExecucao = getValor("Hirarquia de Execução");
                    else if (linha.Contains(" Etapas do Gatilho "))
                        listTrigger.Add(trigger);
                    else if (linha.Contains(" Itens "))
                        return listTrigger;
            }
            return listTrigger; 
        }
        
        internal List<Trigger> getTriggersEN()
        {
            setIndiceAtual(indiceAtual + 1);
            List<Trigger> listTrigger = new List<Trigger>();
            Trigger trigger = null;
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[indiceAtual];

                if (isLabel(linha, " Name  "))
                        trigger = new Trigger { nome = getValor("Name") };
                else if (isLabel(linha, " Trigger Style  "))
                        trigger.estiloGatilho = getValor("Trigger Style");
                else if (isLabel(linha, " Trigger Text  "))
                {
                    bool fimDaTrigger = false;
                    while (!fimDaTrigger)
                    {
                        setIndiceAtual(indiceAtual + 1);
                        linha = linhasArquivo[indiceAtual];
                        if (!isLabel(linha, " Fire in Enter-Query Mode  "))
                        {
                            trigger.textoGatilho += linha + "\n";
                        }
                        else
                        {
                            fimDaTrigger = true;
                            i = indiceAtual;
                        }
                    }
                    trigger.dispararModoConsultar = getValor("Fire in Enter-Query Mode");
                    }
                else if (isLabel(linha, " Execution Hierarchy  "))
                        trigger.HirarquiaExecucao = getValor("Execution Hierarchy");
                else if (isLabel(linha, " Trigger Steps  "))
                {
                    listTrigger.Add(trigger);
                    if (linhasArquivo[indiceAtual + 1].Contains("----------") && linhasArquivo[indiceAtual + 2].Contains("----------"))
                        return listTrigger;
                }
                else if (isLabel(linha, " Items  "))
                    return listTrigger;
                else if (isLabel(linha, "Item Type") || isLabel(linha, " Relations "))
                {
                    setIndiceAtual(indiceAtual - 2);
                    return listTrigger;
                }
            }
            return listTrigger;
        }

        internal int pulaFimDadosBloco()
        {
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[i];
                if ((linguagem == Linguagem.Portugues && linha.Contains(" Relações  "))
                  || (linguagem == Linguagem.Ingles && isLabel(linha," Relations  ")))
                {
                    setIndiceAtual(i + 2);
                    return indiceAtual;
                }
            }
            return indiceAtual;
        }

        internal int pulaParteRelations()
        {
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                setIndiceAtual(i);
                string linha = linhasArquivo[i];
                if ((linguagem == Linguagem.Portugues && linha.Contains(" Consulta Automática  "))
                  ||(linguagem == Linguagem.Ingles && isLabel(linha," Automatic Query")))
                {
                    setIndiceAtual(i + 2);
                    return indiceAtual;
                }
            }
            return indiceAtual;
        }
    }
    public enum Linguagem { Portugues, Ingles };
}
