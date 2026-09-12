using System; // Importa comandos básicos da linguagem (tratamento de texto, datas e erros)
using System.Configuration; // Importa recursos de configuração e leitura de arquivos do aplicativo
using Npgsql; // Importa o driver necessário para conectar e interagir com o banco de dados PostgreSQL
using System.Drawing; // Importa ferramentas visuais para definir cores, posições de telas e tipos de fontes
using System.Windows.Forms; // Importa os componentes gráficos do Windows (botões, caixas de texto, painéis)

namespace CineSulApp // Organiza este arquivo dentro da estrutura do projeto "CineSulApp"
{
    public partial class FormLogin : Form // Cria a tela gráfica de login do aplicativo
    {
        private TextBox txtEmail; // Reserva um espaço de memória para guardar a caixa de digitação do e-mail
        private TextBox txtSenha; // Reserva um espaço de memória para guardar a caixa de digitação da senha
        private Panel central; // Reserva espaço para um painel invisível que agrupa e centraliza o formulário de login
        // Guarda as informações de endereço (localhost), porta (5432), banco de dados, usuário e senha para conectar ao PostgreSQL
        private readonly string pgConnString = "Host=localhost;Port=5432;Database=cinesuldb;Username=postgres;Password=root;";

        public FormLogin() // Método que é acionado no exato momento em que a janela de login é chamada
        {
            InitializeComponent(); // Inicializa os componentes internos padrão do Windows Forms
            {
                this.FormBorderStyle = FormBorderStyle.None; // Esconde a barra superior tradicional do Windows (onde fica o botão de fechar X)
                this.WindowState = FormWindowState.Maximized; // Faz a janela do programa ocupar 100% da tela do computador

            }
            ConfigurarTela(); // Chama as instruções de estilo e cor da janela
            ConstruirInterface(); // Chama as instruções para desenhar os botões, textos e caixas de entrada
        }

        private void ConfigurarTela() // Define os detalhes estéticos da janela
        {
            this.Text = "Login - Cíne Sul"; // Define o título oficial do sistema da janela
            this.Size = new Size(1024, 900); // Estabelece a dimensão padrão de 1024 pixels de largura por 900 de altura
            this.BackColor = Color.FromArgb(13, 16, 26); // Pinta o fundo da tela com um tom azul-escuro suave
            this.StartPosition = FormStartPosition.CenterScreen; // Força a abertura da janela exatamente no centro da tela
        }

        private void ConstruirInterface() // Método encarregado de montar todos os botões e elementos gráficos
        {
            this.Controls.Add(NavigationHelper.CriarHeader(this)); // Adiciona a barra de topo/navegação padrão no topo da janela

            central = new Panel // Instancia a caixa painel responsável por segurar os elementos do login
            {
                Size = new Size(500, 450), // Define o tamanho do bloco central em 500x450 pixels
                Location = new Point((this.ClientSize.Width - 500) / 2, 100), // Pinta o bloco no centro exato da tela na horizontal
                BackColor = Color.Transparent // Deixa o fundo desse painel totalmente invisível
            };

            Label lblBemVindo = new Label // Cria o texto "Bem-vindo(a) de volta!"
            {
                Text = "Bem-vindo(a) de volta!", // Define a mensagem que será mostrada ao usuário
                ForeColor = Color.White, // Ajusta a cor do texto para branco
                Font = new Font("Segoe UI", 16, FontStyle.Bold), // Usa a fonte Segoe UI, tamanho 16, em negrito
                TextAlign = ContentAlignment.MiddleCenter, // Alinha as letras exatamente no centro do campo
                Size = new Size(500, 40), // Define a dimensão do retângulo do texto
                Location = new Point(0, 10) // Ajusta a posição no topo do painel central
            };

            txtEmail = CriarTextBoxArredondado("• Email", 80, central); // Cria o campo de digitação para e-mail na altura de 80 pixels
            txtSenha = CriarTextBoxArredondado("• Senha", 140, central, true); // Cria o campo para senha na altura de 140 pixels ativando a proteção por senha

            Button btnEntrar = new Button // Instancia o botão principal de "Entrar"
            {
                Text = "Entrar", // Texto escrito no botão
                Size = new Size(320, 42), // Define que o botão terá 320px de largura e 42px de altura
                Location = new Point(90, 205), // Posiciona o botão no centro do bloco
                BackColor = Color.FromArgb(24, 28, 38), // Define uma cor cinza-escuro para o fundo do botão
                ForeColor = Color.White, // Define a cor da escrita do botão para branca
                Font = new Font("Segoe UI", 10, FontStyle.Bold), // Aplica estilo de fonte destacada em negrito
                FlatStyle = FlatStyle.Flat, // Aplica estilo moderno reto ao botão
                Cursor = Cursors.Hand // Muda a seta do mouse para a mãozinha de clique ao passar o cursor por cima
            };
            btnEntrar.FlatAppearance.BorderColor = Color.FromArgb(63, 114, 252); // Aplica uma borda azul ao redor do botão
            btnEntrar.FlatAppearance.BorderSize = 2; // Ajusta a largura da borda azul para 2 pixels
            btnEntrar.Click += BtnEntrar_Click; // Faz o botão chamar a validação de acesso ao ser clicado

            Label lblEsqueceu = new Label // Cria o texto clicável para recuperar a senha
            {
                Text = "Esqueceu a sua senha?", // Define a frase visível
                ForeColor = Color.Silver, // Pinta a palavra com um tom prateado/cinza-claro
                Font = new Font("Segoe UI", 9), // Ajusta para tamanho de letra 9
                TextAlign = ContentAlignment.MiddleCenter, // Centraliza o texto
                Size = new Size(500, 25), // Tamanho da área do texto
                Location = new Point(0, 260), // Posição abaixo do botão de entrar
                Cursor = Cursors.Hand // Transforma o cursor do mouse em mãozinha ao passar em cima
            };

            Label lblCriarConta = new Label // Cria o texto clicável para direcionar ao cadastro
            {
                Text = "Não tem uma conta ainda? Crie uma agora!", // Texto de incentivo ao cadastro
                ForeColor = Color.Silver, // Cor prateada do texto
                Font = new Font("Segoe UI", 9), // Fonte em tamanho 9
                TextAlign = ContentAlignment.MiddleCenter, // Texto centralizado
                Size = new Size(500, 25), // Tamanho do campo
                Location = new Point(0, 295), // Posicionado abaixo do link de recuperação
                Cursor = Cursors.Hand // Altera o cursor para mão de seleção
            };

            lblCriarConta.Click += (s, e) => // Ação que roda quando a pessoa clica em "Crie uma agora!"
            {
                FormCadastro cadastro = new FormCadastro(); // Prepara a janela de cadastro
                cadastro.Show(); // Abre a janela de cadastro na tela
                this.Hide(); // Esconde a janela atual de login
            };

            lblEsqueceu.Click += (s, e) => // Ação que roda quando a pessoa clica em "Esqueceu a sua senha?"
            {
                FormEsqueceuSenha recuperarSenha = new FormEsqueceuSenha(); // Prepara a tela de recuperação de senha
                recuperarSenha.Show(); // Abre a tela de recuperação
                this.Hide(); // Esconde a janela de login
            };

            central.Controls.AddRange(new Control[] { lblBemVindo, btnEntrar, lblEsqueceu, lblCriarConta }); // Insere todos esses elementos visuais juntos no bloco central
            this.Controls.Add(central); // Coloca o bloco central montado dentro da janela principal

            // Mantém a caixa centralizada ao redimensionar
            this.Resize += (s, e) => // Evento que vigia se o usuário mudou o tamanho da janela do sistema
            {
                central.Location = new Point((this.ClientSize.Width - central.Width) / 2, 100); // Mantém o painel exatamente no meio
            };
        }

        private TextBox CriarTextBoxArredondado(string placeholder, int posY, Panel container, bool isPassword = false) // Função auxiliar para desenhar as caixas de digitação bonitas
        {
            Panel pnlBg = new Panel // Cria um painel que serve como fundo estilizado para a caixa de texto
            {
                Size = new Size(320, 42), // Tamanho da caixa de fundo
                Location = new Point(90, posY), // Posição no painel central
                BackColor = Color.FromArgb(38, 42, 50) // Cor escura do interior da caixa
            };

            TextBox txt = new TextBox // Cria o campo real onde as letras e números entram ao digitar
            {
                Text = placeholder, // Texto provisório de fundo (ex: "• Email")
                ForeColor = Color.Gray, // Cor cinza indicando que é só uma dica inicial
                Font = new Font("Segoe UI", 10), // Tamanho de letra da digitação
                Size = new Size(290, 25), // Tamanho da área interna de texto
                Location = new Point(15, 9), // Posição do texto com margem interna
                BackColor = Color.FromArgb(38, 42, 50), // Mesma cor do fundo para mesclar
                BorderStyle = BorderStyle.None // Remove bordas brancas feias padrão do Windows
            };

            txt.Enter += (s, e) => // Quando o usuário clica dentro do campo para digitar
            {
                if (txt.Text == placeholder) // Se o texto ainda for o exemplo inicial (dica)
                {
                    txt.Text = ""; // Apaga a dica temporária
                    txt.ForeColor = Color.White; // Muda a cor da digitação do usuário para branco
                    if (isPassword) txt.UseSystemPasswordChar = true; // Se for o campo de senha, passa a esconder os caracteres com bolinhas
                }
            };

            txt.Leave += (s, e) => // Quando o usuário clica fora do campo de texto
            {
                if (string.IsNullOrWhiteSpace(txt.Text)) // Se o usuário não escreveu nada e saiu do campo
                {
                    txt.Text = placeholder; // Devolve o texto explicativo de exemplo
                    txt.ForeColor = Color.Gray; // Pinta a dica de cinza novamente
                    if (isPassword) txt.UseSystemPasswordChar = false; // Desativa a ocultação de senha enquanto exibe o exemplo
                }
            };

            pnlBg.Controls.Add(txt); // Coloca o campo de digitação dentro do retângulo de fundo
            container.Controls.Add(pnlBg); // Coloca o conjunto pronto dentro do painel principal
            return txt; // Devolve a caixa de texto para ser acessada pelas rotinas de código
        }

        // Método local para gerar hash SHA256 (mesma lógica do DatabaseHelper)
        private string GerarHashSHA256(string input) // Função matemática que criptografa a senha para maior segurança
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create()) // Prepara a ferramenta oficial de algoritmo SHA256
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)); // Transforma a senha em bytes e calcula a hash secreta
                var sb = new System.Text.StringBuilder(); // Cria um construtor de frases para formar o texto da hash
                foreach (var b in bytes) sb.Append(b.ToString("x2")); // Converte cada número criptografado em caracteres alfanuméricos
                return sb.ToString(); // Devolve a senha transformada em uma longa sequência indecifrável
            }
        }

        private void BtnEntrar_Click(object sender, EventArgs e) // Rotina executada no momento do clique no botão "Entrar"
        {
            string email = (txtEmail.Text == "• Email") ? "" : txtEmail.Text.Trim(); // Pega o e-mail limpando espaços vazios ou limpa se ainda for o texto de exemplo
            string senha = (txtSenha.Text == "• Senha") ? "" : txtSenha.Text; // Pega a senha ou considera vazia se ainda estiver com o texto padrão

            // Validação de campos vazios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha)) // Verifica se a pessoa esqueceu de digitar o e-mail ou a senha
            {
                MessageBox.Show("Preencha email e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Exibe aviso de alerta
                return; // Para o código imediatamente
            }

            try // Tenta realizar a autenticação no banco de dados
            {
                // 1. Conexão direta ao PostgreSQL
                using (var conn = new NpgsqlConnection(pgConnString)) // Abre a comunicação direta com o banco de dados
                {
                    conn.Open(); // Conecta ativamente ao servidor do banco

                    // 2. Consulta ajustada para o padrão minúsculo do PostgreSQL
                    string sql = "SELECT senha FROM users WHERE email = @email"; // Prepara a ordem SQL que lê a senha associada ao e-mail informado

                    using (var cmd = new NpgsqlCommand(sql, conn)) // Monta o comando do banco
                    {
                        cmd.Parameters.AddWithValue("@email", email); // Substitui o parâmetro @email pela variável e-mail com segurança
                        object result = cmd.ExecuteScalar(); // Roda a busca e devolve apenas a senha que está salva no banco de dados

                        if (result != null) // Se encontrou algum registro correspondente a esse e-mail
                        {
                            string dbSenha = result.ToString(); // Pega a senha que veio gravada no banco de dados

                            // 3. Utiliza o gerador de hash local
                            string hashInput = GerarHashSHA256(senha); // Criptografa a senha recém-digitada no formulário de login

                            // Valida se o hash gerado bate com o armazenado no banco de dados
                            if (dbSenha == hashInput) // Compara se a senha criptografada digitada bate exatamente com a do banco
                            {
                                DadosCinema.IsLoggedIn = true; // Grava no aplicativo que o usuário está validado e conectado
                                DadosCinema.FilmeSelecionado = DadosCinema.FilmeSelecionado; // Mantém o filme pré-selecionado na memória do app
                                DadosCinema.LoggedUserEmail = email; // Armazena o e-mail do usuário ativo na sessão

                                Form telaDestino; // Declara uma variável para segurar a próxima tela
                                if (DadosCinema.TelaRetorno != null) // Verifica se o usuário vinha tentando acessar alguma tela restrita antes de logar
                                {
                                    telaDestino = DadosCinema.TelaRetorno(); // Abre a tela que ele tentava acessar antes do login
                                    DadosCinema.TelaRetorno = null; // Limpa essa memória para não redirecionar de novo erroneamente
                                }
                                else // Caso ele estivesse logando direto
                                {
                                    telaDestino = new Form1(); // Direciona para a tela inicial do sistema (Form1)
                                }

                                telaDestino.Show(); // Exibe a tela de destino na tela
                                this.Hide(); // Esconde a janela de login
                            }
                            else // Se a senha digitada for incorreta
                            {
                                MessageBox.Show("Email ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Exibe caixinha de aviso de senha incorreta
                            }
                        }
                        else // Se o e-mail não existir na tabela de usuários
                        {
                            MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Alerta de usuário inexistente
                        }
                    }
                }
            }
            catch (Exception ex) // Se der qualquer falha física de conexão com o banco de dados
            {
                MessageBox.Show("Erro ao conectar ao banco: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Mostra o erro técnico em forma de popup
            }
        }
    }
}