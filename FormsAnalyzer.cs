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
        int indiceAtual = 0;

        List<string> blocosDefault = new List<string> { "CONTROLE", "TABS", "TOOLBAR", "PROCESSOS", "LOGOEMP", "TEMPRESAS" };

        List<Bloco> blocos = new List<Bloco>();
        Bloco bloco = new Bloco();

        public void setLinhasArquivo(string[] pLinhas)
        {
            linhasArquivo = pLinhas;
        }

        public void setIndiceAtual(int pIndice)
        {
            indiceAtual = pIndice;
        }

        public List<Bloco> getBlocks(bool pFiltraBlocosDefault) 
        {
            var listBlocos = new List<Bloco>();
            //realiza novo loop apartir da linha referente aos dados do bloco
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                indiceAtual = i;

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
                    bloco.subClassificacao = getSubClassificacao();
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
                else if (bloco.grupoAtributosVisuais==null && linha.Contains(" Grupo de Atributos Visuais   "))
                    bloco.grupoAtributosVisuais = getValor("Grupo de Atributos Visuais");
                else if (linha.Contains(" Gatilhos  "))
                {
                    bloco.triggers = getTriggers();
                    i = indiceAtual-1;
                }
                else if (linha.Contains(" Itens  "))
                {
                    bloco.itens = getItens();
                    i = indiceAtual;
                }
                if (linha.Contains("* Canvases  ")) //fim area de blocos
                    return listBlocos;
            }
            return listBlocos; //informa que acabou
        }

        public SubClassificacao getSubClassificacao()
        {
            indiceAtual += 1;

            SubClassificacao subClassificacao = new SubClassificacao();
           
            //realiza novo loop apartir da linha referente aos dados da subclassificacao
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                indiceAtual = i;
                string linha = linhasArquivo[i];

                if (isValor(linha, " Local  "))
                    subClassificacao.local = getValor("* Local");
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

        public List<Item> getItens()
        {
            indiceAtual += 1; 

            List<Item> itens = new List<Item>();
            Item item = new Item();
                
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                indiceAtual = i;
                string linha = linhasArquivo[i];

                if (linha.Contains("* Nome   "))
                    item = new Item { nome = getValor("Nome") };
                else if (linha.Contains(" Tipo de Item  "))
                    item.tipoItem = getValor("Tipo de Item");
                else if (linha.Contains("* Informações sobre a Divisão em Subclasses     "))
                    item.subClassificacao = getSubClassificacao();
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
                    item.triggers = getTriggers();
                    itens.Add(item);
                }
                else if (linha.Contains(" Relações "))
                {
                    if (linhasArquivo[indiceAtual + 2].Contains(" Tipo de Relação "))
                        indiceAtual = pulaParteRelations();
                    else
                        return itens;
                    // Fim dos itens do Bloco
                } 
                
            }
            return itens; 
        }

        internal List<Trigger> getTriggers()
        {
            indiceAtual += 1;
            List<Trigger> listTrigger = new List<Trigger>();
            Trigger trigger = null;
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
			{
                indiceAtual = i;
			    string linha = linhasArquivo[indiceAtual];

                if (linha.Contains(" Nome "))
                    trigger = new Trigger { nome = getValor(" Nome ") };
                else if (linha.Contains(" Estilo do Gatilho "))
                    trigger.estiloGatilho = getValor(" Estilo do Gatilho ");
                else if (linha.Contains(" Texto do Gatilho "))
                {
                    bool fimDaTrigger=false;
                    while (!fimDaTrigger)
                    {
                        indiceAtual += 1;
                        linha = linhasArquivo[indiceAtual];
                        if (!linha.Contains(" Disparar no Modo Entrar Consultar "))
                        {
                            trigger.textoGatilho += linha + "\n";
                        }
                        else
                            fimDaTrigger = true;
                    }
                    trigger.DispararModoConsultar = getValor("Disparar no Modo Entrar Consultar");
                }
                else if (linha.Contains(" Hirarquia de Execução "))
                    trigger.HirarquiaExecucao = getValor("Hirarquia de Execução");
                else if (linha.Contains(" Etapas do Gatilho "))
                    listTrigger.Add(trigger);
                else if(linha.Contains(" Itens "))
                    return listTrigger;
			}
            return listTrigger; 
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

        internal int pulaFimDadosBloco()
        {
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                indiceAtual = i;
                string linha = linhasArquivo[i];
                if (linha.Contains(" Relações  "))
                {
                    indiceAtual = i + 2;
                    return indiceAtual;
                }
            }
            return indiceAtual;
        }

        internal int pulaParteRelations()
        {
            for (int i = indiceAtual; i < linhasArquivo.Length; i++)
            {
                indiceAtual = i;
                string linha = linhasArquivo[i];
                if (linha.Contains(" Consulta Automática  "))
                {
                    indiceAtual = i + 2;
                    return indiceAtual;
                }
            }
            return indiceAtual;
        }

        internal string relatorioErros(List<Bloco> pListBlocos
                                      , bool validaHints                 
                                      , bool validaSubClassificacao      
                                      , bool validaAlinhamento           
                                      , bool validaSomenteCamposComCanvas
                                      , bool validaSalvaWPesquisa
                                      , bool validaAtributoVisual)   
        {
            string erros = "";

            if(blocos!=null)
            foreach (var bloco in blocos)
            {
                if (validaSubClassificacao && !bloco.isSubClassificado())
                    erros += "- Bloco " + bloco.nome + " não está sub classificado. \n";
                if(validaAtributoVisual)
                try 
	            {	        
		            if (int.Parse(bloco.numeroRegistrosExibidos)>1 && bloco.grupoAtributosVisuaisRegistroAtual != "CG$CURRENT_RECORD")
                        erros += "- Bloco " + bloco.nome + " é multirecord e o 'Grupo de Atributos Visuais do Registro Atual' não está como: CG$CURRENT_RECORD";
                    else if(bloco.grupoAtributosVisuais != "CG$SCROLLBAR")
                        erros += "- Bloco " + bloco.nome + " é não está com 'Grupo de Atributos Visuais' não está como: CG$CURRENT_RECORD";
                }
	            catch (ArgumentNullException){}
                catch (FormatException){}

                if(bloco.itens!=null)
                foreach (var item in bloco.itens)
                {
                    if (validaSubClassificacao && !item.isSubClassificado())
                        erros += "  - Item " + bloco.nome+"."+item.nome + " não está sub classificado. \n";
                    
                    if (!string.IsNullOrEmpty(item.tipoItem)
                       && item.hasCanvas()
                       && item.getSubClassificacao() != "MASK_BUTTON")
                    {
                        if (item.tipoItem == "Item de Texto")
                        {

                            if (item.nomeFonte != "MS Sans Serif")
                                erros += "  - Item " + bloco.nome + "." + item.nome + " não está com a fonte MS Sans Serif. \n";
                            if (!string.IsNullOrEmpty(item.prompt) && item.nomeFontePrompt != "MS Sans Serif")
                                erros += "  - Item " + bloco.nome + "." + item.nome + " o prompt não está com a fonte MS Sans Serif. \n";
                            
                            if(validaAlinhamento)
                            if (item.tiposDados == "Número" && !(item.justificacao == "Final" || item.justificacao == "Direita"))
                                erros += "  - Item " + bloco.nome + "." + item.nome + " é um número e deve ser alinhado a direita.\n";
                            else if (item.tiposDados == "Car" && !(item.justificacao == "Inicial" || item.justificacao == "Esquerda"))
                                erros += "  - Item " + bloco.nome + "." + item.nome + " é um alpha e deve ser alinhado a esquerda.\n";
                        }

                        if(validaHints)
                        if ((new string[]{"Item de Texto","Caixa de Seleção","Item da Lista"}.Contains(item.tipoItem))
                         && (string.IsNullOrEmpty(item.dica) || string.IsNullOrEmpty(item.dicaFerramenta)))
                            erros += "  - Item " + bloco.nome + "." + item.nome + " está sem um dos hints.\n";
                    
                    
                    }
                    
                }
            }
            return erros;
        }
    }
}
