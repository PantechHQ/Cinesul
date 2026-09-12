using Npgsql; // Carrega a biblioteca para conversar com o banco de dados PostgreSQL
using System; // Carrega recursos básicos do C# (como mensagens e erros)
using System.Data; // Carrega tabelas e organizadores de dados em memória
using System.Drawing; // Carrega recursos de cores, fontes e tamanhos de tela
using System.Windows.Forms; // Carrega os componentes visuais do Windows (botões, caixas, janelas)

namespace CineSulApp // Organiza este arquivo dentro do projeto "CineSulApp"
{
    public partial class FormFavoritos : Form // Cria a tela visual de Favoritos
    {
        private FlowLayoutPanel flowFavoritos; // Cria uma área que organiza os filmes em grade automaticamente
        private string categoriaAtual = "Comédia"; // Guarda a palavra "Comédia" como a categoria padrão inicial

        // Guarda o endereço, porta, usuário, senha e nome do banco de dados na memória
        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=root;Database=cinesuldb";

        public FormFavoritos() // Esta função é executada assim que a tela é aberta
        {
            ConfigurarTela(); // Chama as configurações visuais da janela (tamanho, cor)
            ConstruirInterface(); // Desenha os botões, títulos e caixas na tela
            {
                this.FormBorderStyle = FormBorderStyle.None; // Remove as bordas do Windows e o botão de fechar (X)
                this.WindowState = FormWindowState.Maximized; // Faz a janela ocupar a tela inteira do computador
            }
        }

        private void ConfigurarTela() // Função que define a aparência inicial da janela
        {
            this.Text = "Filmes favoritos - Cíne Sul"; // Define o título oficial da janela
            this.Size = new Size(1024, 900); // Define a largura (1024) e altura (900) da tela em pixels
            this.BackColor = Color.FromArgb(10, 15, 28); // Pinta o fundo da janela com um tom azul bem escuro
            this.StartPosition = FormStartPosition.CenterScreen; // Abre a janela centralizada na tela do computador
        }

        private void ConstruirInterface() // Função responsável por desenhar todos os botões e componentes na tela
        {
            // Header (Cabeçalho)
            this.Controls.Add(NavigationHelper.CreateHeader(this)); // Adiciona a barra superior padrão do aplicativo

            // Área principal
            Panel main = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40, 20, 40, 20) }; // Cria um painel transparente com margens internas
            this.Controls.Add(main); // Adiciona o painel transparente dentro da janela
            main.BringToFront(); // Garante que esse painel fique na frente de outros elementos da tela

            // Cria o texto do título principal "Filmes favoritos" em branco e negrito
            Label lblTitulo = new Label { Text = "Filmes favoritos", ForeColor = Color.White, Font = new Font("Segoe UI", 20, FontStyle.Bold), Location = new Point(40, 20), AutoSize = true };
            main.Controls.Add(lblTitulo); // Coloca o título na tela

            // Barra de categorias
            string[] categorias = { "Comédia", "Ação", "Romance", "Terror", "Animação", "Drama" }; // Cria uma lista com as categorias
            int posX = 40; // Define a posição inicial na horizontal (eixo X) para colocar os botões
            foreach (var cat in categorias) // Passa por cada uma das categorias criando um botão para ela
            {
                // Cria um botão com o nome da categoria atual e define suas cores e tamanho
                Button btn = new Button { Text = cat, Size = new Size(110, 36), Location = new Point(posX, 80), FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = Color.FromArgb(24, 28, 34) };
                btn.FlatAppearance.BorderSize = 0; // Remove as bordas pretas do botão para deixar o visual mais moderno
                btn.Click += (s, e) => { categoriaAtual = cat; CarregarFavoritos(cat); }; // Ao clicar, muda a categoria e atualiza os filmes da tela
                main.Controls.Add(btn); // Adiciona o botão criado na tela
                posX += 120; // Anda 120 pixels para a direita para colocar o próximo botão lado a lado
            }

            // Cria uma linha fina e escura para separar visualmente os botões do conteúdo
            Panel linha = new Panel { Size = new Size(920, 1), Location = new Point(40, 130), BackColor = Color.FromArgb(40, 45, 60) };
            main.Controls.Add(linha); // Coloca a linha divisória na tela

            // Cria o painel organizador onde os cartões dos filmes favoritos vão aparecer com barra de rolagem
            flowFavoritos = new FlowLayoutPanel { Location = new Point(40, 150), Size = new Size(920, 600), AutoScroll = true, BackColor = Color.Transparent };
            main.Controls.Add(flowFavoritos); // Coloca esse painel na tela

            CarregarFavoritos(categoriaAtual); // Busca e mostra os filmes favoritos da categoria inicial ("Comédia")
        }

        private void CarregarFavoritos(string categoria) // Função que busca no banco de dados os filmes de uma categoria
        {
            flowFavoritos.Controls.Clear(); // Apaga todos os filmes exibidos anteriormente para não duplicar

            string userEmail = DadosCinema.LoggedUserEmail; // Pega o e-mail do usuário atualmente conectado

            if (string.IsNullOrEmpty(userEmail)) // Verifica se NÃO há nenhum usuário conectado (e-mail em branco)
            {
                // Cria uma mensagem avisando o usuário para fazer login
                Label lblAviso = new Label
                {
                    Text = "Faça login para visualizar seus filmes favoritos.",
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 12),
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                flowFavoritos.Controls.Add(lblAviso); // Exibe o aviso na tela
                return; // Encerra a função aqui sem tentar buscar no banco
            }

            try // Tenta realizar a busca no banco de dados com segurança
            {
                // Conecta ao banco usando as configurações definidas na linha 13
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open(); // Abre a conexão ativa com o banco de dados
                    // Escreve a instrução em banco de dados para buscar o Título e Detalhes do filme favorito do usuário
                    string sql = "SELECT Titulo, Detalhes FROM Favoritos WHERE UserEmail = @email AND Categoria = @categoria";

                    using (var cmd = new NpgsqlCommand(sql, conn)) // Prepara o comando SQL para ser executado
                    {
                        cmd.Parameters.AddWithValue("@email", userEmail); // Substitui @email pelo e-mail real do usuário
                        cmd.Parameters.AddWithValue("@categoria", categoria); // Substitui @categoria pela categoria selecionada

                        using (var reader = cmd.ExecuteReader()) // Executa a busca e prepara para ler os resultados
                        {
                            bool encontrou = false; // Cria um indicador avisando que ainda não achou nada
                            while (reader.Read()) // Faz uma leitura linha por linha de todos os filmes encontrados
                            {
                                encontrou = true; // Confirma que encontrou ao menos um filme
                                string titulo = reader["Titulo"].ToString(); // Guarda o título do filme encontrado
                                string detalhes = reader["Detalhes"].ToString(); // Guarda os detalhes do filme encontrado
                                flowFavoritos.Controls.Add(CriarCard(titulo, detalhes, categoria)); // Desenha o cartão do filme na tela
                            }

                            if (!encontrou) // Se a busca não retornar nenhum filme
                            {
                                // Cria um texto dizendo que a categoria está vazia
                                Label lblVazio = new Label
                                {
                                    Text = $"Nenhum filme favorito na categoria {categoria}.",
                                    ForeColor = Color.Gray,
                                    Font = new Font("Segoe UI", 11),
                                    AutoSize = true,
                                    Margin = new Padding(10)
                                };
                                flowFavoritos.Controls.Add(lblVazio); // Adiciona o aviso de categoria vazia na tela
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // Se acontecer algum erro no processo acima
            {
                // Exibe uma caixinha de aviso na tela informando o erro
                MessageBox.Show("Erro ao carregar favoritos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CriarCard(string titulo, string detalhes, string categoria) // Função que constrói o quadrinho (card) de um filme
        {
            Panel card = new Panel { Size = new Size(200, 240), Margin = new Padding(10), BackColor = Color.FromArgb(24, 24, 32) }; // Cria o fundo do quadrinho
            Panel poster = new Panel { Size = new Size(200, 140), BackColor = Color.FromArgb(40, 40, 50), Dock = DockStyle.Top }; // Cria o espaço reservado para a imagem
            Label lblTitulo = new Label { Text = titulo, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(5, 150), Size = new Size(190, 22) }; // Cria o texto do título do filme
            Label lblDetalhes = new Label { Text = detalhes, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8), Location = new Point(5, 175), Size = new Size(190, 20) }; // Cria o texto dos detalhes do filme

            // Cria o botão azul escrito "Remover"
            Button btnRemover = new Button { Text = "Remover", Size = new Size(80, 28), Location = new Point(60, 200), BackColor = Color.FromArgb(63, 114, 252), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnRemover.FlatAppearance.BorderSize = 0; // Remove a borda padrão do botão

            btnRemover.Click += (s, e) => // Configura a ação que acontece ao clicar no botão "Remover"
            {
                RemoverFavorito(titulo); // Apaga o filme do banco de dados
                CarregarFavoritos(categoria); // Recarrega a tela para sumir com o filme removido
            };

            card.Controls.AddRange(new Control[] { poster, lblTitulo, lblDetalhes, btnRemover }); // Agrupa todos os elementos dentro do quadrinho
            return card; // Retorna o quadrinho pronto para ser exibido
        }

        private void RemoverFavorito(string titulo) // Função que apaga o filme favorito do banco de dados
        {
            try // Tenta realizar a exclusão com segurança
            {
                using (var conn = new NpgsqlConnection(connectionString)) // Conecta ao banco de dados
                {
                    conn.Open(); // Abre a conexão ativa com o banco
                    string sql = "DELETE FROM Favoritos WHERE UserEmail = @email AND Titulo = @titulo"; // Escreve o comando de apagar
                    using (var cmd = new NpgsqlCommand(sql, conn)) // Prepara o comando
                    {
                        cmd.Parameters.AddWithValue("@email", DadosCinema.LoggedUserEmail); // Substitui pelo e-mail do usuário logado
                        cmd.Parameters.AddWithValue("@titulo", titulo); // Substitui pelo título do filme que deve ser apagado
                        cmd.ExecuteNonQuery(); // Executa o comando de exclusão no banco de dados
                    }
                }
            }
            catch (Exception ex) // Se der erro ao tentar apagar
            {
                // Mostra uma caixinha avisando que não foi possível remover
                MessageBox.Show("Erro ao remover favorito: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}