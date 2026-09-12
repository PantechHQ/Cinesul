// Esta janela mostra os detalhes de um filme e os horários disponíveis
using System; // tipos básicos
using System.Collections.Generic; // permite usar dicionários e listas
using System.Drawing; // trabalho com cores/tamanhos
using System.Linq; // para usar operações em coleções (OrderBy, Take)
using System.Windows.Forms; // elementos gráficos

namespace CineSulApp
{
    // Classe que representa a janela de detalhes e horários
    public partial class Form2 : Form
    {
        // Gerador de números aleatórios para sortear horários
        private static Random rnd = new Random();

        // Lista fixa de horários possíveis para as sessões
        private readonly string[] horariosPossiveis = new string[]
        {
            "13:00", "14:15", "15:30", "16:45", "17:30", "18:20", "19:00", "20:15", "21:30", "22:00"
        };

        // Dicionário de sinopses para cada filme (texto explicativo)
        private readonly Dictionary<string, string> sinopses = new Dictionary<string, string>
        {
            { "Ratatoing", "Filme conta a história de Marcell Toing, um rato dono do famoso restaurante Ratatoing. Juntamente com Carol e Greg, ele sai em busca de ingredientes raros nos estabelecimentos dos humanos, enfrentando de ratoeiras a gatos famintos." },
            { "Xuxinha e Guto Contra os Monstros do Espaço", "Guto, um garoto de 7 anos, recebe a ajuda de sua anjinha da guarda para combater monstros comedores de lixo oriundos do planeta XYZ, que fica localizado em uma galáxia distante." },
            { "Deus e o Diabo na Terra do Sol", "O vaqueiro Manuel se revolta contra a exploração imposta pelo coronel Moraes e acaba matando o seu algoz. Ele passa a ser perseguido por jagunços e foge com a esposa, Rosa. O casal se junta aos seguidores do beato Sebastião..." },
            { "Hoje Eu Quero Voltar Sozinho", "Leonardo, um adolescente cego, tenta lidar com a mãe superprotetora ao mesmo tempo, em que busca sua independência. Quando Gabriel chega em seu colégio, novos sentimentos começam a surgir em Leonardo." },
            { "Internet - O Filme", "Em uma convenção de youtubers, os personagens entram em vários conflitos uma vez que todos eles estão em busca da fama a qualquer preço." },
            { "Central do Brasil", "Dora, uma amargurada ex-professora, ganha a vida escrevendo cartas para pessoas analfabetas. Um dia, Josué, filho de uma de suas clientes, acaba sozinho e ela se junta a ele em uma viagem pelo interior do nordeste em busca do pai do menino." },
            { "O Auto da Compadecida", "As aventuras dos nordestinos João Grilo, um sertanejo pobre e mentiroso, e Chicó, o mais covarde dos homens. A dupla luta para sobreviver aplicando golpes no pequeno vilarejo de Taperoá, no sertão da Paraíba." },
            { "Barquinhos", "Jota é um jet ski muito corajoso e curioso que vive se metendo em tremendas confusões em alto-mar. O seu fiel e atrapalhado amigo Beto Bote e sua irmã Lili são os que o ajudam a sair das enrascadas em Oceanópolis." }
        };

        // Construtor: inicializa a janela
        public Form2()
        {
            InitializeComponent();
            {
                // Remove borda e abre em tela cheia (mesmo efeito visual que em outros forms)
                this.FormBorderStyle = FormBorderStyle.None; // sem moldura
                this.WindowState = FormWindowState.Maximized; // tela cheia

            }
            // Configura propriedades visuais e cria a interface
            ConfigurarTela();
            ConstruirInterface();
        }

        // Define título, tamanho e cores da janela
        private void ConfigurarTela()
        {
            this.Text = "Cíne Sul - Detalhes e Horários";
            this.Size = new Size(1024, 900);
            this.BackColor = Color.FromArgb(12, 16, 28);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Monta os controles visuais da tela de detalhes e horários
        private void ConstruirInterface()
        {
            // Adiciona o cabeçalho padrão
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            // Determina qual filme mostrar: se não houver filme selecionado, usa um padrão
            string filmeAtual = string.IsNullOrEmpty(DadosCinema.FilmeSelecionado) ? "Ratatoing" : DadosCinema.FilmeSelecionado;
            // Busca a sinopse no dicionário; se não existir, mostra mensagem padrão
            string sinopseAtual = sinopses.ContainsKey(filmeAtual) ? sinopses[filmeAtual] : "Sinopse não disponível para este filme.";

            // Painel lateral esquerdo com poster, título e sinopse
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 280, BackColor = Color.FromArgb(8, 11, 20), Padding = new Padding(20) };
            this.Controls.Add(sidebar);

            // PictureBox no lugar do antigo Panel com placeholder fixo, para exibir a capa real do filme
            PictureBox picPoster = new PictureBox
            {
                Size = new Size(240, 310),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(30, 40, 60),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            // Carrega a capa (ou mostra placeholder de texto) usando o helper compartilhado
            // com o Form1 e o Form9, mantendo a mesma convenção de nomes de arquivo.
            PosterHelper.CarregarPoster(picPoster, filmeAtual);

            Label lblTitulo = new Label { Text = filmeAtual, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 345), Size = new Size(240, 45) };
            Label lblSinopse = new Label { Text = sinopseAtual, ForeColor = Color.Silver, Font = new Font("Segoe UI", 9), Location = new Point(20, 395), Size = new Size(240, 250) };

            sidebar.Controls.AddRange(new Control[] { picPoster, lblTitulo, lblSinopse });

            // Painel central que exibirá os horários agrupados por cinema
            Panel mainPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(12, 16, 28), Padding = new Padding(30) };
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront();

            Label lblHorariosTitle = new Label { Text = "Horários de Sessão", ForeColor = Color.White, Font = new Font("Segoe UI", 20, FontStyle.Bold), Location = new Point(30, 20), AutoSize = true };
            mainPanel.Controls.Add(lblHorariosTitle);

            // Botão que indica a data de hoje (apenas visual)
            Button btnHoje = new Button { Text = $"Hoje\n{DateTime.Now:dd/MMM}", Size = new Size(90, 45), Location = new Point(30, 75), BackColor = Color.FromArgb(13, 71, 161), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnHoje.FlatAppearance.BorderSize = 0;
            mainPanel.Controls.Add(btnHoje);

            // Adiciona caixas com horários para diferentes cinemas
            mainPanel.Controls.Add(CriarBoxCinema("Cinema Shopping Metro Itaquera", 30, 150));
            mainPanel.Controls.Add(CriarBoxCinema("Cinema Shopping Aricanduva", 30, 290));
            mainPanel.Controls.Add(CriarBoxCinema("Cinema Shopping Eldorado", 30, 430));

            // Botão que volta para a tela inicial
            Button btnVoltar = new Button { Text = "↩ Voltar para Home", Size = new Size(150, 35), Location = new Point(30, 600), FlatStyle = FlatStyle.Flat, ForeColor = Color.Gray, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            btnVoltar.FlatAppearance.BorderColor = Color.Gray;
            btnVoltar.Click += (s, e) => {
                Form1 home = new Form1();
                home.Show();
                this.Hide();
            };
            mainPanel.Controls.Add(btnVoltar);
        }

        // Cria um painel com botões de horários para um determinado cinema
        private Panel CriarBoxCinema(string nomeCinema, int x, int y)
        {
            Panel box = new Panel { Size = new Size(640, 110), Location = new Point(x, y), BackColor = Color.FromArgb(24, 28, 40) };
            Label lblCinemaName = new Label { Text = nomeCinema, ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };
            box.Controls.Add(lblCinemaName);

            // Sorteia aleatoriamente entre 3 e 5 sessões para exibir
            int quantidadeSessoes = rnd.Next(3, 6);
            List<string> sessoesAleatorias = horariosPossiveis.OrderBy(h => rnd.Next()).Take(quantidadeSessoes).OrderBy(h => h).ToList();

            int posX = 15;
            foreach (string horario in sessoesAleatorias)
            {
                Button btnSessao = new Button
                {
                    Text = horario,
                    Size = new Size(80, 35),
                    Location = new Point(posX, 55),
                    BackColor = Color.FromArgb(35, 45, 65),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnSessao.FlatAppearance.BorderColor = Color.FromArgb(13, 71, 161);

                string h = horario; // captura o horário para o closure
                btnSessao.Click += (s, e) => AbrirMapaAssentos(nomeCinema, h);

                box.Controls.Add(btnSessao);
                posX += 95; // avança a posição horizontal para o próximo botão
            }

            return box;
        }

        // Ao clicar em um horário, guarda o cinema e horário selecionados e abre a tela de mapa de assentos
        private void AbrirMapaAssentos(string cinema, string horario)
        {
            DadosCinema.CinemaSelecionado = cinema;
            DadosCinema.HorarioSelecionado = horario;

            Form3 telaMapa = new Form3();
            telaMapa.Show();
            this.Hide();
        }
    }
}