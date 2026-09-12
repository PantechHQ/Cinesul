// Este arquivo define a janela principal (home) do aplicativo CineSul.
using System; // Permite usar tipos básicos como strings, datas e eventos
using System.Drawing; // Permite trabalhar com cores, tamanhos e posições
using System.IO; // Permite acessar arquivos no disco (usado para carregar posters)
using System.Windows.Forms; // Permite criar janelas, botões, labels e outros controles visuais

// Define o agrupamento de classes do aplicativo
namespace CineSulApp
{
    // Declara que esta classe é uma parte (partial) do formulário e herda comportamentos de uma janela (Form)
    public partial class Form1 : Form
    {
        // Guarda referência ao container principal para poder recalcular a centralização no Resize
        private FlowLayoutPanel container;

        // Guarda os controles que precisam ficar centralizados horizontalmente,
        // para reaplicar a centralização sempre que a janela for redimensionada.
        private readonly System.Collections.Generic.List<Control> controlesCentralizados = new System.Collections.Generic.List<Control>();

        // Construtor: executado quando a janela é criada
        public Form1()
        {
            // Chama o método gerado pelo designer (se houver componentes visuais definidos no Designer)
            InitializeComponent();
            {
                // Remove a barra de título e a moldura da janela
                this.FormBorderStyle = FormBorderStyle.None; // deixa a janela sem bordas
                // Abre a janela em modo tela cheia
                this.WindowState = FormWindowState.Maximized; // ocupa toda a tela

            }
            // Define propriedades visuais gerais da janela
            ConfigurarTela();
            // Cria os controles e organiza a interface do usuário
            ConstruirInterface();
        }


        // Método que define o título, tamanho, cor e posicionamento da janela
        private void ConfigurarTela()
        {
            this.Text = "Cíne Sul - Home"; // texto do título
            this.Size = new Size(1024, 900); // tamanho padrão
            this.BackColor = Color.FromArgb(18, 18, 24); // cor de fundo
            this.StartPosition = FormStartPosition.CenterScreen; // abre centralizado
        }

        // Método que monta a interface visível para o usuário
        private void ConstruirInterface()
        {
            // Adiciona o cabeçalho comum com navegação
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            // Cria um painel que organiza outros controles verticalmente e permite rolagem
            container = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, // ocupa toda a área disponível
                AutoScroll = true, // permite rolar se o conteúdo exceder a área
                FlowDirection = FlowDirection.TopDown, // organiza os controles de cima para baixo
                WrapContents = false, // não quebra para a próxima linha
                Padding = new Padding(30, 20, 30, 20), // espaço interno
                BackColor = Color.FromArgb(18, 18, 24) // cor de fundo do painel
            };
            // Adiciona o painel principal à janela
            this.Controls.Add(container);
            container.BringToFront(); // garante que o painel fique visível acima de outros controles

            // CRIA UM BANNER DE DESTAQUE
            Panel panelBanner = new Panel { Size = new Size(930, 280), Margin = new Padding(0, 0, 0, 25), BackColor = Color.FromArgb(30, 30, 40) };
            // Texto grande de destaque no banner
            Label lblBannerTitle = new Label { Text = "O AUTO DA COMPADECIDA\nDESTAQUE DO CINEMA NACIONAL", ForeColor = Color.Yellow, Font = new Font("Segoe UI", 20, FontStyle.Bold), Location = new Point(50, 80), AutoSize = true, BackColor = Color.Transparent };
            // Botão que leva o usuário para ver horários deste filme
            Button btnComprarBanner = new Button { Text = "VER HORÁRIOS ▷", ForeColor = Color.Black, BackColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Size = new Size(200, 40), Location = new Point(50, 180), Cursor = Cursors.Hand };
            btnComprarBanner.FlatAppearance.BorderSize = 0; // remove borda do botão

            // Evento: quando o botão for clicado, salva o filme selecionado e abre a tela de horários
            btnComprarBanner.Click += (s, e) => {
                DadosCinema.FilmeSelecionado = "O Auto da Compadecida"; // guarda o filme escolhido
                Form2 telaHorarios = new Form2(); // cria a próxima janela
                telaHorarios.Show(); // mostra a janela de horários
                this.Hide(); // esconde a janela atual
            };

            // Adiciona o título e o botão ao banner
            panelBanner.Controls.AddRange(new Control[] { lblBannerTitle, btnComprarBanner });
            // Adiciona o banner ao container principal
            container.Controls.Add(panelBanner);
            controlesCentralizados.Add(panelBanner); // marca para ser centralizado

            // --- SEÇÃO: EM CARTAZ --- (título da seção)
            Label lblEmCartaz = new Label { Text = "Em Cartaz", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), Margin = new Padding(0, 10, 0, 10), AutoSize = true };
            container.Controls.Add(lblEmCartaz);
            controlesCentralizados.Add(lblEmCartaz); // marca para ser centralizado

            // Painel que lista os filmes em uma linha com rolagem horizontal
            FlowLayoutPanel flowEmCartaz = new FlowLayoutPanel { Size = new Size(940, 230), FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoScroll = true };

            // Adiciona cards de filmes ao painel (cada card é criado por CriarCardFilme)
            flowEmCartaz.Controls.Add(CriarCardFilme("O Auto da Compadecida", "★ 8.6", "Comédia • Drama"));
            flowEmCartaz.Controls.Add(CriarCardFilme("Central do Brasil", "★ 8.0", "Drama • Nacional"));
            flowEmCartaz.Controls.Add(CriarCardFilme("Hoje Eu Quero Voltar Sozinho", "★ 7.9", "Romance • Drama"));
            flowEmCartaz.Controls.Add(CriarCardFilme("Deus e o Diabo na Terra do Sol", "★ 9.0", "Drama • Clássico"));

            container.Controls.Add(flowEmCartaz);
            controlesCentralizados.Add(flowEmCartaz); // marca para ser centralizado

            // --- SEÇÃO: EM BREVE / DEMAIS EXIBIÇÕES ---
            Label lblEmBreve = new Label { Text = "Em Breve / Outras Sessões", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), Margin = new Padding(0, 20, 0, 10), AutoSize = true };
            container.Controls.Add(lblEmBreve);
            controlesCentralizados.Add(lblEmBreve); // marca para ser centralizado

            // Painel horizontal para filmes "em breve"
            FlowLayoutPanel flowEmBreve = new FlowLayoutPanel { Size = new Size(940, 230), FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoScroll = true };

            // Adiciona vários cards de filmes à seção "Em breve"
            flowEmBreve.Controls.Add(CriarCardFilme("Turma da Mônica em Uma Aventura no Tempo", "★ 7.3", "Animação • Infantil"));
            flowEmBreve.Controls.Add(CriarCardFilme("Internet - O Filme", "★ 3.1", "Comédia • Nacional"));
            flowEmBreve.Controls.Add(CriarCardFilme("Xuxinha e Guto Contra os Monstros do Espaço", "★ 2.9", "Animação • Infantil"));
            flowEmBreve.Controls.Add(CriarCardFilme("Ratatoing", "★ 1.9", "Animação • Infantil"));
            flowEmBreve.Controls.Add(CriarCardFilme("Barquinhos", "★ 1.2", "Animação • Infantil"));

            container.Controls.Add(flowEmBreve);
            controlesCentralizados.Add(flowEmBreve); // marca para ser centralizado

            // Centraliza tudo pela primeira vez, já com o tamanho atual da janela
            AplicarCentralizacao();

            // Reaplica a centralização sempre que a janela for redimensionada
            // (ex: maximizar/restaurar, ou trocar de monitor com resolução diferente)
            container.Resize += (s, e) => AplicarCentralizacao();
        }

        // Ajusta a margem esquerda de cada controle marcado, de forma que ele
        // fique visualmente centralizado dentro da largura útil do container.
        // Um FlowLayoutPanel não centraliza os filhos sozinho, então isso é feito
        // manualmente calculando o espaço sobrando e jogando metade dele como margem esquerda.
        private void AplicarCentralizacao()
        {
            // Largura útil disponível (descontando padding esquerdo/direito do container e a barra de rolagem)
            int larguraUtil = container.ClientSize.Width - container.Padding.Left - container.Padding.Right;
            if (container.VerticalScroll.Visible)
                larguraUtil -= SystemInformation.VerticalScrollBarWidth;

            foreach (Control c in controlesCentralizados)
            {
                int margemEsquerda = (larguraUtil - c.Width) / 2;
                if (margemEsquerda < 0) margemEsquerda = 0; // nunca deixa margem negativa

                // Mantém a margem superior/inferior já definida, só troca a esquerda/direita
                c.Margin = new Padding(margemEsquerda, c.Margin.Top, 0, c.Margin.Bottom);
            }
        }

        // Cria um card visual para representar um filme com imagem, título e descrição
        private Panel CriarCardFilme(string titulo, string nota, string info)
        {
            // Painel que contém a imagem e os textos do filme
            Panel card = new Panel { Size = new Size(200, 210), Margin = new Padding(0, 0, 20, 0), BackColor = Color.FromArgb(24, 24, 32), Cursor = Cursors.Hand };

            // Espaço reservado para a imagem do filme
            PictureBox pic = new PictureBox { Size = new Size(200, 130), Dock = DockStyle.Top, BackColor = Color.FromArgb(40, 40, 50), Cursor = Cursors.Hand, SizeMode = PictureBoxSizeMode.Zoom };
            // Label que mostra a nota do filme sobre a imagem
            Label lblNota = new Label { Text = nota, ForeColor = Color.Gold, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(10, 105), AutoSize = true, BackColor = Color.FromArgb(160, 0, 0, 0), Cursor = Cursors.Hand };

            // Carrega o pôster do filme (ou mostra placeholder de texto) usando o helper
            // compartilhado com o Form9, evitando duplicar a lógica de leitura de arquivo.
            PosterHelper.CarregarPoster(pic, titulo);

            // Adiciona a label de nota sobre a imagem
            pic.Controls.Add(lblNota);

            // Título e descrição exibidos abaixo da imagem
            Label lblTitulo = new Label { Text = titulo, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(5, 138), Size = new Size(190, 34), Cursor = Cursors.Hand };
            Label lblDesc = new Label { Text = info, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8), Location = new Point(5, 175), Size = new Size(190, 20), Cursor = Cursors.Hand };

            card.Controls.AddRange(new Control[] { pic, lblTitulo, lblDesc });

            // Evento que será executado quando o card (ou seus elementos) for clicado
            EventHandler gerenciarClique = (s, e) => {
                DadosCinema.FilmeSelecionado = titulo; // guarda o filme escolhido
                Form2 telaHorarios = new Form2(); // cria a tela de horários
                telaHorarios.Show(); // mostra a tela de horários
                this.Hide(); // esconde a janela atual
            };

            // Anexa o evento de clique a diversos elementos do card para melhorar a usabilidade
            card.Click += gerenciarClique;
            pic.Click += gerenciarClique;
            lblNota.Click += gerenciarClique;
            lblTitulo.Click += gerenciarClique;
            lblDesc.Click += gerenciarClique;

            return card; // retorna o painel pronto para ser adicionado à lista
        }
    }
}