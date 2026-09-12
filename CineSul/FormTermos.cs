using System; // Importa tipos básicos do C# como eventos, tipos numéricos e manipulação de objetos
using System.Drawing; // Importa recursos visuais como cores, coordenadas (Point) e dimensões (Size)
using System.Windows.Forms; // Importa a biblioteca de componentes visuais para janelas do Windows (Form, Panel, Label)

namespace CineSulApp // Organiza a classe dentro do namespace principal do projeto
{
    // Declaração da classe da janela de Termos e Condições, herdando da classe base Form
    public partial class FormTermos : Form
    {
        // Construtor principal da classe: executado quando o formulário é instanciado
        public FormTermos()
        {
            ConfigurarTela(); // Chama a função que define as propriedades visuais básicas da janela
            ConstruirInterface(); // Chama a função que constrói e posiciona os elementos na tela
            {
                this.FormBorderStyle = FormBorderStyle.None; // Remove as bordas padrão, botões de fechar/minimizar e a barra de título
                this.WindowState = FormWindowState.Maximized; // Redimensiona a janela para ocupar 100% da tela do monitor

            }
        }

        // Método privado para definir parâmetros estruturais e de aparência da janela
        private void ConfigurarTela()
        {
            this.Text = "Termos e Condições - Cíne Sul"; // Define o texto identificador do formulário
            this.Size = new Size(1024, 900); // Configura a resolução padrão da janela para 1024x900 pixels
            this.BackColor = Color.FromArgb(13, 16, 26); // Define a cor de fundo com o tom escuro padrão do aplicativo
            this.StartPosition = FormStartPosition.CenterScreen; // Centraliza a posição da janela na tela
        }

        // Método privado para criar, configurar e posicionar os controles da interface visual
        private void ConstruirInterface()
        {
            // Cabeçalho
            this.Controls.Add(NavigationHelper.CreateHeader(this)); // Instancia e insere a barra de cabeçalho padrão no topo da tela

            // Painel Principal com Rolagem
            Panel pnlConteudo = new Panel // Instancia o container que abrigará o texto dos termos
            {
                Size = new Size(920, 750), // Define a dimensão da área visível em 920px de largura por 750px de altura
                Location = new Point(45, 80), // Posiciona o painel a 45px da esquerda e 80px do topo
                AutoScroll = true, // Habilita a barra de rolagem automática caso o conteúdo ultrapasse a altura do painel
                BackColor = Color.Transparent // Mantém o fundo transparente para exibir a cor da janela principal
            };

            // Botão / Link Voltar
            Label lblVoltar = new Label // Instancia o rótulo que funciona como botão de navegação para fechar a tela
            {
                Text = "‹ Voltar", // Texto que exibe a seta e a palavra Voltar
                ForeColor = Color.FromArgb(63, 114, 252), // Define a cor do texto com o tom azul destaque da marca
                Font = new Font("Segoe UI", 12, FontStyle.Bold), // Define a fonte Segoe UI, tamanho 12 e em negrito
                Location = new Point(0, 10), // Posiciona no canto superior esquerdo dentro do painel
                AutoSize = true, // Redimensiona a caixa de texto dinamicamente de acordo com o conteúdo
                Cursor = Cursors.Hand // Modifica o ponteiro do mouse para o ícone de mão de clique
            };
            lblVoltar.Click += (s, e) => this.Close(); // Associa o evento de clique ao método que fecha a janela atual

            // Título
            Label lblTitulo = new Label // Instancia o rótulo do título principal da página
            {
                Text = "Termos e condições", // Texto do título
                ForeColor = Color.White, // Define a cor do texto para branco
                Font = new Font("Segoe UI", 22, FontStyle.Bold), // Configura a fonte para tamanho 22 em negrito
                Location = new Point(0, 45), // Posiciona o título logo abaixo do botão "Voltar"
                AutoSize = true // Ajusta a largura da caixa de texto automaticamente
            };

            // Texto dos Termos
            Label lblTexto = new Label // Instancia o bloco de texto longo que exibe o corpo dos termos
            {
                Text = @"
                – CineSul Em vigor a partir de agosto de 2026; 
                O CineSul é uma plataforma que busca democratizar o acesso ao cinema
                nacional, oferecendo descontos sociais em ingressos e aluguel online de filmes
                para quem não tem cinema disponível na sua cidade. Ao usar o CineSul, você 
                reconhece ter lido e entendido seu conteúdo e declara estar de acordo com. 
                Também reconhece que a Cinesul, a qualquer tempo, e ao seu exclusivo critério, 
                independentemente de comunicação prévia ao Usuário, poderá alterar, total ou parcialmente, 
                ou atualizar, esses Termos e Condições. ", // Conteúdo jurídico/fictício em várias linhas
                ForeColor = Color.FromArgb(220, 225, 235), // Define a cor do texto com tom cinza-claro para facilitar a leitura
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular), // Define a fonte para tamanho 10.5 regular
                Location = new Point(0, 100), // Posiciona o bloco de texto a 100px do topo do painel
                Width = 880, // Define a largura fixa de 880px para o texto
                AutoSize = true, // Permite que a altura do rótulo cresça dinamicamente conforme o texto aumenta
                MaximumSize = new Size(880, 0) // Define a largura máxima de quebra de linha em 880px e altura ilimitada
            };

            pnlConteudo.Controls.Add(lblVoltar); // Adiciona o controle "Voltar" para dentro do painel de conteúdo
            pnlConteudo.Controls.Add(lblTitulo); // Adiciona o controle do título para dentro do painel de conteúdo
            pnlConteudo.Controls.Add(lblTexto); // Adiciona o controle de texto para dentro do painel de conteúdo

            this.Controls.Add(pnlConteudo); // Adiciona o painel de conteúdo montado dentro do formulário
        }
    }
}