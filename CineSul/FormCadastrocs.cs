using Npgsql;// Biblioteca para conexão e manipulação do banco de dados PostgreSQL.
using System;// Tipos básicos do C# (Exception, DateTime, Action, etc.).
using System.Drawing;// Manipulação de elementos visuais (Cores, Fontes, Tamanhos, Pontos).
using System.Linq;// Recurso para consultas em coleções de dados (ex: método All()).
using System.Security.Cryptography;// Criptografia e funções Hash (SHA256).
using System.Text;// Manipulação eficiente de strings com StringBuilder.
using System.Text.RegularExpressions;// Validação de padrões de texto através de Expressões Regulares (Regex).
using System.Windows.Forms;// Componentes de interface gráfica do WinForms (Form, TextBox, Button, etc.).

namespace CineSulApp
{
    // Classe do formulário de cadastro, herdando os recursos nativos da classe Form do WinForms.
    public partial class FormCadastro : Form
    {
        // Declaração dos campos de entrada de texto e controles que serão acessados em múltiplos métodos.
        private TextBox txtNome, txtCPF, txtEmail, txtTelefone, txtSenha, txtRepetirSenha;
        private MaskedTextBox txtDataNasc; // Campo de texto com máscara para datas.
        private CheckBox chkTermos;        // Caixa de seleção dos termos de uso.
        private Label lblCpfAviso;         // Rótulo para exibir mensagens de aviso caso o CPF seja inválido.

        // Construtor principal da classe do formulário.
        public FormCadastro()
        {
            // Aplica as configurações visuais base do formulário (tamanho, cor de fundo, etc.).
            ConfigurarTela();

            // Constrói dinamicamente todos os componentes da interface (Inputs, Labels, Botões).
            ConstruirInterface();

            // Configura a janela para rodar sem bordas padrão do Windows e em tela cheia.
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        // Define as propriedades básicas de exibição do Form.
        private void ConfigurarTela()
        {
            this.Text = "Cadastro - Cíne Sul";// Título da janela.
            this.Size = new Size(1024, 900);// Resolução inicial base.
            this.BackColor = Color.FromArgb(13, 16, 26);// Cor de fundo escura (Dark Theme).
            this.StartPosition = FormStartPosition.CenterScreen;// Centraliza a janela na tela do usuário.
        }

        // Instancia, posiciona e estrutura todos os elementos visuais da tela.
        private void ConstruirInterface()
        {
            // Adiciona um cabeçalho customizado herdado de uma classe utilitária de navegação.
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            // Painel container para agrupar os campos no centro da tela.
            Panel central = new Panel
            {
                Size = new Size(600, 650),
                // Centraliza o painel com base na resolução atual da janela.
                Location = new Point((this.ClientSize.Width - 600) / 2, (this.ClientSize.Height - 650) / 2),
                Anchor = AnchorStyles.None, // Impede distorção dos elementos na tela.
                BackColor = Color.Transparent
            };

            // Evento disparado sempre que a janela é redimensionada, garantindo o alinhamento central do formulário.
            this.Resize += (s, e) =>
            {
                central.Location = new Point((this.ClientSize.Width - central.Width) / 2, (this.ClientSize.Height - central.Height) / 2 + 20);
            };

            // Rótulo de título da página.
            Label lblTitulo = new Label
            {
                Text = "Crie a sua conta!",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(600, 40),
                Location = new Point(0, 10)
            };

            // Variáveis globais para manter o padrão visual de dimensão e centralização dos inputs.
            int campoLargura = 440;
            int posXCentral = (600 - campoLargura) / 2; // Deslocamento X para manter os campos centralizados no painel (80px).

            // Criação dos campos do formulário chamando os métodos construtores auxiliares.
            txtNome = CriarCampo("• Nome completo", campoLargura, posXCentral, 70, central);
            txtDataNasc = CriarCampoData("• Data de nascimento", campoLargura, posXCentral, 120, central);
            txtCPF = CriarCampo("• CPF", campoLargura, posXCentral, 170, central, apenasNumeros: true, maxLength: 11);

            // Rótulo de alerta para avisos sobre a digitação do CPF.
            lblCpfAviso = new Label
            {
                Text = "",
                ForeColor = Color.FromArgb(240, 90, 90), // Cor vermelha suave para erros.
                Font = new Font("Segoe UI", 8f),
                Location = new Point(posXCentral + 10, 212),
                Size = new Size(campoLargura, 16),
                AutoSize = false
            };

            // Evento acionado assim que o cursor do usuário sai do campo de CPF (desfoca).
            txtCPF.Leave += (s, e) =>
            {
                string valorCpf = ObterValor(txtCPF, "• CPF");
                // Valida o CPF e exibe a mensagem de erro caso o número inserido seja inválido.
                if (!string.IsNullOrEmpty(valorCpf) && !ValidarCPF(valorCpf))
                    lblCpfAviso.Text = "CPF inválido. Verifique os números digitados.";
                else
                    lblCpfAviso.Text = ""; // Limpa a mensagem se estiver correto.
            };

            // Instanciação dos demais campos do formulário.
            txtEmail = CriarCampo("• Email", campoLargura, posXCentral, 230, central);
            txtTelefone = CriarCampo("• Telefone", campoLargura, posXCentral, 280, central, apenasNumeros: true);
            txtSenha = CriarCampo("• Senha", campoLargura, posXCentral, 330, central, isPassword: true);
            txtRepetirSenha = CriarCampo("• Repetir senha", campoLargura, posXCentral, 380, central, isPassword: true);

            // Painel flexível para organizar a caixa dos termos de uso e os links inline lado a lado.
            FlowLayoutPanel pnlTermos = new FlowLayoutPanel
            {
                Size = new Size(campoLargura, 30),
                Location = new Point(posXCentral, 435),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            // Caixa para o aceite dos termos.
            chkTermos = new CheckBox
            {
                Text = "Li e aceito os",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8.5f),
                AutoSize = true,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 3, 0, 0)
            };

            // Link interativo para abrir a janela com os "Termos de Condições".
            LinkLabel lnkTermos = new LinkLabel
            {
                Text = "termos de condições",
                LinkColor = Color.FromArgb(63, 114, 252),
                Font = new Font("Segoe UI", 8.5f),
                AutoSize = true,
                Margin = new Padding(3, 4, 0, 0)
            };
            lnkTermos.Click += (s, e) =>
            {
                FormTermos termos = new FormTermos();
                termos.ShowDialog(); // Abre a tela como modal.
            };

            // Conjunção gráfica (" e a ").
            Label lblE = new Label
            {
                Text = "e a",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8.5f),
                AutoSize = true,
                Margin = new Padding(3, 4, 0, 0)
            };

            // Link interativo para abrir a "Política de Privacidade".
            LinkLabel lnkPolitica = new LinkLabel
            {
                Text = "política de privacidade",
                LinkColor = Color.FromArgb(63, 114, 252),
                Font = new Font("Segoe UI", 8.5f),
                AutoSize = true,
                Margin = new Padding(3, 4, 0, 0)
            };
            lnkPolitica.Click += (s, e) =>
            {
                FormPolitica politica = new FormPolitica();
                politica.ShowDialog();
            };

            // Adiciona todos os subelementos dos termos dentro do FlowLayoutPanel.
            pnlTermos.Controls.AddRange(new Control[] { chkTermos, lnkTermos, lblE, lnkPolitica });

            // Botão principal de confirmação de cadastro.
            Button btnCriar = new Button
            {
                Text = "Criar conta",
                Size = new Size(220, 42),
                Location = new Point((600 - 220) / 2, 485), // Posição centralizada no container.
                BackColor = Color.FromArgb(24, 28, 38),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCriar.FlatAppearance.BorderColor = Color.FromArgb(63, 114, 252);
            btnCriar.FlatAppearance.BorderSize = 2;
            btnCriar.Click += BtnCriar_Click; // Associa o clique ao método do cadastro.

            // Adiciona os componentes principais ao painel central e depois à janela.
            central.Controls.AddRange(new Control[] { lblTitulo, lblCpfAviso, pnlTermos, btnCriar });
            this.Controls.Add(central);
        }

        // Método auxiliar responsável por fabricar visualmente o campo de data mascarado (MaskedTextBox).
        private MaskedTextBox CriarCampoData(string placeholder, int largura, int posX, int posY, Panel container)
        {
            // Fundo escuro com cantos arredondados simulados por um Panel.
            Panel pnlBg = new Panel
            {
                Size = new Size(largura, 40),
                Location = new Point(posX, posY),
                BackColor = Color.FromArgb(38, 42, 50)
            };

            MaskedTextBox msk = new MaskedTextBox
            {
                Mask = "00/00/0000",// Máscara rígida do padrão de data no Brasil.
                Text = placeholder,// Exibe o texto instrutivo inicial.
                ForeColor = Color.Gray,// Cor cinza para o efeito "placeholder".
                Font = new Font("Segoe UI", 9),
                Size = new Size(largura - 20, 22),
                Location = new Point(10, 9),
                BackColor = Color.FromArgb(38, 42, 50),
                BorderStyle = BorderStyle.None,
                PromptChar = '_'// Caractere que indica onde digitar.
            };

            // Evento ao focar no campo da data: limpa a máscara do placeholder e prepara o cursor.
            msk.Enter += (s, e) =>
            {
                if (msk.ForeColor == Color.Gray)
                {
                    msk.Text = "";
                    msk.ForeColor = Color.White;
                }

                // Posiciona o cursor de digitação no primeiro dígito com um pequeno atraso de thread.
                msk.BeginInvoke((Action)(() =>
                {
                    msk.SelectionStart = 0;
                }));
            };

            // Evento ao perder o foco: se não digitou nada, restaura o estado de placeholder cinza.
            msk.Leave += (s, e) =>
            {
                string valorLimpo = msk.Text.Replace("/", "").Replace("_", "").Replace(" ", "").Trim();
                if (string.IsNullOrEmpty(valorLimpo))
                {
                    msk.Mask = "";
                    msk.Text = placeholder;
                    msk.ForeColor = Color.Gray;
                    msk.Mask = "00/00/0000";
                }
            };

            pnlBg.Controls.Add(msk);
            container.Controls.Add(pnlBg);
            return msk;
        }

        // Método auxiliar genérico para a criação dos campos de texto (TextBox) com tratamento de placeholder customizado.
        private TextBox CriarCampo(string placeholder, int largura, int posX, int posY, Panel container, bool isPassword = false, bool apenasNumeros = false, int maxLength = 0)
        {
            Panel pnlBg = new Panel
            {
                Size = new Size(largura, 40),
                Location = new Point(posX, posY),
                BackColor = Color.FromArgb(38, 42, 50)
            };

            TextBox txt = new TextBox
            {
                Text = placeholder,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9),
                Size = new Size(largura - 20, 22),
                Location = new Point(10, 9),
                BackColor = Color.FromArgb(38, 42, 50),
                BorderStyle = BorderStyle.None
            };

            if (maxLength > 0)
                txt.MaxLength = maxLength;

            // Bloqueia a digitação de caracteres não numéricos se a propriedade for true.
            if (apenasNumeros)
            {
                txt.KeyPress += (s, e) =>
                {
                    // Permite somente números e a tecla Backspace (apagar).
                    if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                        e.Handled = true; // Cancela a tecla pressionada.
                };
            }

            // Lógica do Placeholder: Ao focar no campo, remove a palavra-chave e altera a cor do texto.
            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.White;
                    // Oculta a digitação se for um campo do tipo senha.
                    if (isPassword) txt.UseSystemPasswordChar = true;
                }
            };

            // Lógica do Placeholder: Ao desfocar do campo se ele estiver vazio, recupera a instrução.
            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                    if (isPassword) txt.UseSystemPasswordChar = false;
                }
            };

            pnlBg.Controls.Add(txt);
            container.Controls.Add(pnlBg);
            return txt;
        }

        // Criptografa a senha informada utilizando o algoritmo de Hash irreversível SHA-256.
        private string GerarHashSHA256(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Converte a string original em um array de bytes UTF-8.
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                var sb = new System.Text.StringBuilder();

                // Converte cada byte para representação hexadecimal (2 caracteres).
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // Realiza a validação matemática completa do documento CPF através do algoritmo oficial de dígitos verificadores.
        private bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            cpf = cpf.Trim();

            // Verifica se o CPF possui exatamente 11 caracteres e é composto apenas por números.
            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
                return false;

            // Rejeita sequências conhecidas de CPF inválidos (Ex: 111.111.111-11, 222.222.222-22, etc.).
            bool todosIguais = true;
            for (int i = 1; i < 11; i++)
            {
                if (cpf[i] != cpf[0]) { todosIguais = false; break; }
            }
            if (todosIguais) return false;

            // Transforma a string de texto em um array de inteiros.
            int[] numeros = new int[11];
            for (int i = 0; i < 11; i++)
                numeros[i] = cpf[i] - '0';

            // Cálculo para validar o 1º Dígito Verificador (posição 10 do CPF).
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += numeros[i] * (10 - i);
            int resto = soma % 11;
            int digito1 = (resto < 2) ? 0 : 11 - resto;
            if (numeros[9] != digito1) return false; // Falhou na validação do primeiro dígito.

            // Cálculo para validar o 2º Dígito Verificador (posição 11 do CPF).
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += numeros[i] * (11 - i);
            resto = soma % 11;
            int digito2 = (resto < 2) ? 0 : 11 - resto;
            if (numeros[10] != digito2) return false; // Falhou na validação do segundo dígito.

            return true; // CPF válido.
        }

        // Evento principal acionado ao clicar no botão "Criar conta". Valida todas as regras de negócio antes do cadastro no BD.
        private void BtnCriar_Click(object sender, EventArgs e)
        {
            // Extrai e sanitiza os dados informados nos componentes visuais.
            string nome = ObterValor(txtNome, "• Nome completo");
            string dataNascStr = ObterValor(txtDataNasc, "• Data de nascimento");
            string cpf = ObterValor(txtCPF, "• CPF");
            string email = ObterValor(txtEmail, "• Email");
            string telefone = ObterValor(txtTelefone, "• Telefone");
            string senha = ObterValor(txtSenha, "• Senha");
            string repetirSenha = ObterValor(txtRepetirSenha, "• Repetir senha");

            // Validação 1: Campos obrigatórios não podem ficar vazios.
            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validação 2: Checa a estrutura do e-mail usando expressão regular (Regex).
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Por favor, insira um e-mail válido (ex: nome@dominio.com).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validação 3: Checa o CPF se o usuário o preencheu.
            if (!string.IsNullOrEmpty(cpf) && !ValidarCPF(cpf))
            {
                MessageBox.Show("O CPF informado é inválido. Verifique os números digitados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validação 4: Garante que o campo "Senha" e "Repetir senha" são exatamente idênticos.
            if (senha != repetirSenha)
            {
                MessageBox.Show("As senhas informadas não coincidem.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validação 5: Garante que a caixa de aceite dos termos está marcada.
            if (!chkTermos.Checked)
            {
                MessageBox.Show("Por favor, aceite os termos e condições para continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validação 6: Validação de data de nascimento.
            DateTime? dataNascimentoValida = null;
            if (!string.IsNullOrEmpty(dataNascStr) && dataNascStr != "__/__/____")
            {
                // Converte e verifica se a data informada é real (ex: rejeita 30 de fevereiro).
                if (DateTime.TryParseExact(dataNascStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataConvertida))
                {
                    if (dataConvertida > DateTime.Now)
                    {
                        MessageBox.Show("A data de nascimento não pode ser uma data futura.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (dataConvertida.Year < 1900)
                    {
                        MessageBox.Show("A data de nascimento informada é inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    dataNascimentoValida = dataConvertida;
                }
                else
                {
                    MessageBox.Show("A data de nascimento informada é inválida. Verifique o dia, mês e ano.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Início da comunicação com o Banco de Dados PostgreSQL (Npgsql).
            try
            {
                // String de conexão contendo as credenciais do banco PostgreSQL local.
                string pgConnString = "Host=localhost;Port=5432;Database=cinesuldb;Username=postgres;Password=root;";

                using (var conn = new Npgsql.NpgsqlConnection(pgConnString))
                {
                    conn.Open(); // Abre a conexão com o banco.

                    // Consulta de segurança para prevenir cadastros duplicados com o mesmo e-mail.
                    using (var cmdCheck = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM users WHERE email = @email", conn))
                    {
                        // Passagem de parâmetro para evitar ataques de SQL Injection.
                        cmdCheck.Parameters.AddWithValue("@email", email);
                        long count = Convert.ToInt64(cmdCheck.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Este e-mail já está cadastrado em nosso sistema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Comando SQL para inserir os dados do novo usuário no banco.
                    string sql = "INSERT INTO users (nome, cpf, telefone, email, senha) VALUES (@nome, @cpf, @telefone, @email, @senha)";
                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        // Atribuição segura dos parâmetros no comando.
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@cpf", cpf);
                        cmd.Parameters.AddWithValue("@telefone", telefone);
                        cmd.Parameters.AddWithValue("@email", email);

                        // A senha é gravada no banco aplicando a criptografia Hash SHA-256 por segurança.
                        cmd.Parameters.AddWithValue("@senha", GerarHashSHA256(senha));

                        // Executa a instrução no BD e retorna o número de linhas inseridas.
                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Conta criada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Redireciona o usuário para a tela de Login e fecha a tela atual de Cadastro.
                            FormLogin login = new FormLogin();
                            login.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível criar a conta. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura falhas de conexão ou erros na execução de comandos do PostgreSQL.
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar para extrair o texto de um input, desconsiderando placeholders cinzas e máscaras vazias.
        private string ObterValor(Control campo, string placeholder)
        {
            // Trata o campo de Data MaskedTextBox.
            if (campo is MaskedTextBox msk)
            {
                string valorLimpo = msk.Text.Replace("/", "").Replace("_", "").Replace(" ", "").Trim();
                if (string.IsNullOrEmpty(valorLimpo) || msk.Text == placeholder) return "";
                return msk.Text.Trim();
            }

            // Trata os campos padrão TextBox.
            if (campo is TextBox txt)
            {
                if (txt.Text == placeholder || string.IsNullOrWhiteSpace(txt.Text)) return "";
                return txt.Text.Trim();
            }

            return "";
        }
    }
}