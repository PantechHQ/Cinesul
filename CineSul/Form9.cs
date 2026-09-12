// Tela que apresenta o catálogo de filmes com categorias e ações (favoritar, ver comentários, ver detalhes)
using System; // tipos fundamentais e eventos
using System.Drawing; // controle de cores, pontos e tamanhos
using System.IO; // manipulação de arquivos (usada indiretamente via PosterHelper)
using System.Windows.Forms; // controles visuais (Form, Panel, Button, Label, etc.)

namespace CineSulApp
{
    // Classe que representa a janela de catálogo de filmes
    public partial class Form9 : Form
    {
        // Painel com rolagem que vai receber os cards de filme dinamicamente
        private FlowLayoutPanel flowLayoutFilmes;

        // Guarda qual categoria está selecionada no momento (inicia em "Ação")
        private string categoriaAtual = "Ação";

        // Construtor: é executado automaticamente quando a tela é criada/aberta
        public Form9()
        {
            // Inicializa os componentes gerados pelo Designer do Visual Studio
            InitializeComponent();
            {
                // Remove a borda padrão da janela (sem barra de título do Windows)
                this.FormBorderStyle = FormBorderStyle.None;
                // Abre a janela já maximizada, ocupando a tela toda
                this.WindowState = FormWindowState.Maximized;
            }

            // Define título, tamanho, cor de fundo etc.
            ConfigurarTela();
            // Monta os elementos visuais: cabeçalho, categorias, painel de filmes
            ConstruirInterface();
            // Carrega os filmes da categoria padrão ("Ação") assim que a tela abre
            CarregarFilmesPorCategoria(categoriaAtual);
        }

        // Evento de Load exigido pelo Designer; não faz nada porque toda a montagem
        // da tela já é feita manualmente no construtor
        private void Form9_Load(object sender, EventArgs e)
        {
            // Mantido para compatibilidade com o Designer do Visual Studio
        }

        // Define as propriedades básicas da janela (título, tamanho, cor de fundo, posição)
        private void ConfigurarTela()
        {
            this.Text = "Catálogo de Filmes - Cíne Sul";
            this.Size = new Size(1024, 900);
            this.BackColor = Color.FromArgb(10, 15, 28); // azul bem escuro (tema dark)
            this.StartPosition = FormStartPosition.CenterScreen; // abre centralizada
        }

        // Monta a estrutura da interface: cabeçalho, título, barra de categorias,
        // linha divisória e painel de rolagem onde os cards serão inseridos
        private void ConstruirInterface()
        {
            // Adiciona o cabeçalho padrão (menu/topo) reutilizado em outras telas
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            // Painel principal que ocupa o restante da tela, abaixo do cabeçalho
            Panel mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40, 20, 40, 20) };
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront(); // garante que fique acima de outros controles sobrepostos

            // Título grande no topo da tela
            Label lblTitulo = new Label
            {
                Text = "Catálogo de filmes",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(40, 20),
                AutoSize = true
            };
            mainPanel.Controls.Add(lblTitulo);

            // Categorias disponíveis para filtrar os filmes
            string[] categorias = { "Ação", "Comédia", "Animação", "Drama" };
            int posX = 40; // posição horizontal inicial dos botões de categoria

            // Cria um botão para cada categoria, lado a lado
            foreach (string cat in categorias)
            {
                Button btnCat = new Button
                {
                    Text = cat,
                    Size = new Size(110, 36),
                    Location = new Point(posX, 70),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };

                // Se esta categoria for a que está ativa, destaca visualmente em azul;
                // caso contrário, deixa em tom neutro/cinza
                if (cat == categoriaAtual)
                {
                    btnCat.BackColor = Color.FromArgb(63, 114, 252);
                    btnCat.ForeColor = Color.White;
                }
                else
                {
                    btnCat.BackColor = Color.FromArgb(30, 35, 48);
                    btnCat.ForeColor = Color.Gray;
                }

                btnCat.FlatAppearance.BorderSize = 0; // remove borda do botão flat

                // Ao clicar em uma categoria:
                // 1) atualiza a categoria selecionada
                // 2) repinta todos os botões de categoria (destaca o clicado)
                // 3) recarrega a lista de filmes filtrada pela nova categoria
                btnCat.Click += (s, e) =>
                {
                    categoriaAtual = cat;

                    // Percorre todos os controles do painel principal procurando
                    // os botões de categoria para atualizar a cor de cada um
                    foreach (Control ctrl in mainPanel.Controls)
                    {
                        if (ctrl is Button btn && Array.IndexOf(categorias, btn.Text) >= 0)
                        {
                            if (btn.Text == categoriaAtual)
                            {
                                btn.BackColor = Color.FromArgb(63, 114, 252);
                                btn.ForeColor = Color.White;
                            }
                            else
                            {
                                btn.BackColor = Color.FromArgb(30, 35, 48);
                                btn.ForeColor = Color.Gray;
                            }
                        }
                    }

                    // Recarrega os cards de filme de acordo com a categoria escolhida
                    CarregarFilmesPorCategoria(categoriaAtual);
                };

                mainPanel.Controls.Add(btnCat);
                posX += 120; // desloca a posição X para o próximo botão de categoria
            }

            // Linha fina para separar visualmente a barra de categorias da lista de filmes
            Panel linha1 = new Panel { Size = new Size(980, 1), Location = new Point(40, 125), BackColor = Color.FromArgb(40, 45, 60) };
            mainPanel.Controls.Add(linha1);

            // Painel com scroll automático que vai conter os cards de filme,
            // organizados automaticamente em grade/fluxo (FlowLayoutPanel)
            flowLayoutFilmes = new FlowLayoutPanel
            {
                Location = new Point(40, 140),
                Size = new Size(980, 660),
                AutoScroll = true,
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(flowLayoutFilmes);
        }

        // Adiciona um filme à lista de favoritos do usuário logado, gravando no banco (PostgreSQL)
        private void AdicionarFavorito(string titulo, string detalhes, string categoria)
        {
            // Bloqueia a ação se o usuário não estiver logado
            if (!DadosCinema.IsLoggedIn || string.IsNullOrEmpty(DadosCinema.LoggedUserEmail))
            {
                MessageBox.Show("Faça login para favoritar filmes.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // String de conexão com o banco local (host, porta, banco, usuário e senha)
                string connectionString = "Host=localhost;Port=5432;Database=cinesuldb;Username=postgres;Password=root;";
                using (var conn = new Npgsql.NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Verifica se o filme já foi favoritado por este usuário,
                    // para evitar registros duplicados na tabela Favoritos
                    string sqlCheck = "SELECT COUNT(*) FROM Favoritos WHERE UserEmail = @email AND Titulo = @titulo";
                    using (var cmdCheck = new Npgsql.NpgsqlCommand(sqlCheck, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@email", DadosCinema.LoggedUserEmail);
                        cmdCheck.Parameters.AddWithValue("@titulo", titulo);
                        long count = Convert.ToInt64(cmdCheck.ExecuteScalar());
                        if (count > 0)
                        {
                            // Já existe: avisa o usuário e encerra sem inserir de novo
                            MessageBox.Show("Esse filme já está nos seus favoritos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    // Insere o novo favorito na tabela, usando parâmetros para evitar SQL Injection
                    string sqlInsert = "INSERT INTO Favoritos (UserEmail, Categoria, Titulo, Detalhes) VALUES (@email, @categoria, @titulo, @detalhes)";
                    using (var cmd = new Npgsql.NpgsqlCommand(sqlInsert, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", DadosCinema.LoggedUserEmail);
                        cmd.Parameters.AddWithValue("@categoria", categoria);
                        cmd.Parameters.AddWithValue("@titulo", titulo);
                        cmd.Parameters.AddWithValue("@detalhes", detalhes);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Filme adicionado aos favoritos!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer erro de conexão/execução e exibe para o usuário
                MessageBox.Show("Erro ao favoritar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Limpa o painel de filmes e recria os cards de acordo com a categoria escolhida
        private void CarregarFilmesPorCategoria(string categoria)
        {
            // Remove todos os cards atualmente exibidos antes de montar os novos
            flowLayoutFilmes.Controls.Clear();

            // Lista de filmes "fixa" (mockada) por categoria — cada linha cria um card
            // passando título, nota e detalhes (gênero/tags)
            if (categoria == "Ação")
            {
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Deus e o Diabo na Terra do Sol", "9.0", "Ação • Drama"));
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Xuxinha e Guto Contra os Monstros do Espaço", "2.9", "Ação • Animação"));
            }
            else if (categoria == "Comédia")
            {
                flowLayoutFilmes.Controls.Add(CriarCardFilme("O Auto da Compadecida", "8.6", "Comédia • Aventura"));
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Internet - O Filme", "3.1", "Comédia • Nacional"));
            }
            else if (categoria == "Animação")
            {
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Ratatoing", "1.9", "Animação • Infantil"));
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Barquinhos", "1.2", "Animação • Infantil"));
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Xuxinha e Guto Contra os Monstros do Espaço", "2.9", "Animação • Aventura"));
            }
            else if (categoria == "Drama")
            {
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Central do Brasil", "8.0", "Drama • Nacional"));
                flowLayoutFilmes.Controls.Add(CriarCardFilme("Hoje Eu Quero Voltar Sozinho", "7.9", "Drama • Romance"));
            }
        }

        // Cria e retorna o Panel (card) visual de um filme, com poster, nota, título,
        // gênero/detalhes e os botões de ação (comentários, alugar, favoritar)
        private Panel CriarCardFilme(string titulo, string nota, string detalhes)
        {
            // Container principal do card
            Panel card = new Panel
            {
                Size = new Size(220, 340),
                Margin = new Padding(12),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            // Área reservada para a capa/poster do filme
            PictureBox picPoster = new PictureBox
            {
                Size = new Size(220, 230),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(35, 42, 60),
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand
            };

            PosterHelper.CarregarPoster(picPoster, titulo);

            // Nota do filme
            Label lblNota = new Label
            {
                Text = $"★ {nota}",
                ForeColor = Color.Gold,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                Location = new Point(0, 235),
                AutoSize = true
            };

            // Título do filme
            Label lblTitulo = new Label
            {
                Text = titulo,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(0, 252),
                Size = new Size(220, 20)
            };

            // Gênero(s)/tags do filme
            Label lblDetalhes = new Label
            {
                Text = detalhes,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8),
                Location = new Point(0, 272),
                Size = new Size(220, 20)
            };

            // 1. Botão Comentários (Padronizado: 100x30 px)
            Button btnComentarios = new Button
            {
                Text = "Comentários",
                Size = new Size(100, 30),
                Location = new Point(0, 298),
                BackColor = Color.FromArgb(24, 28, 38),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnComentarios.FlatAppearance.BorderColor = Color.FromArgb(63, 114, 252);
            btnComentarios.FlatAppearance.BorderSize = 1;
            btnComentarios.Click += (s, e) =>
            {
                FormComentarios tela = new FormComentarios(titulo);
                tela.Show();
                this.Close();
            };

            // Handler compartilhado para seleção do filme
            EventHandler selecionarFilme = (s, e) =>
            {
                DadosCinema.FilmeSelecionado = titulo;
                Form2 telaHorarios = new Form2();
                telaHorarios.Show();
                this.Hide();
            };

            // 2. Botão Alugar (Padronizado: 80x30 px)
            Button btnComprar = new Button
            {
                Text = "Alugar",
                Size = new Size(80, 30),
                Location = new Point(103, 298),
                BackColor = Color.FromArgb(63, 114, 252),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnComprar.FlatAppearance.BorderSize = 0;
            btnComprar.Click += selecionarFilme;

            // 3. Botão Favoritar (Padronizado: 34x30 px)
            Button btnFavoritar = new Button
            {
                Text = "❤",
                Size = new Size(34, 30),
                Location = new Point(186, 298),
                BackColor = Color.FromArgb(24, 28, 38),
                ForeColor = Color.FromArgb(220, 60, 90),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFavoritar.FlatAppearance.BorderColor = Color.FromArgb(220, 60, 90);
            btnFavoritar.FlatAppearance.BorderSize = 1;
            btnFavoritar.Click += (s, e) =>
                AdicionarFavorito(titulo, detalhes, categoriaAtual);

            // Adiciona todos os controles ao card
            card.Controls.AddRange(new Control[] { picPoster, lblNota, lblTitulo, lblDetalhes, btnComentarios, btnComprar, btnFavoritar });

            // Eventos de clique na área do card e poster
            card.Click += selecionarFilme;
            picPoster.Click += selecionarFilme;

            return card;
        }
    }
}