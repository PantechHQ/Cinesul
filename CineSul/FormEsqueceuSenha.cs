using System; // Carrega recursos básicos do C# (como funções do sistema e manipuladores de eventos)
using System.Drawing; // Carrega ferramentas visuais (como cores, posições de pixel e fontes)
using System.Windows.Forms; // Carrega os componentes visuais de janelas (caixas de texto, botões, painéis)
using Npgsql; // Carrega a biblioteca responsável por conectar e rodar comandos no banco PostgreSQL

namespace CineSulApp // Agrupa e organiza este código dentro do aplicativo "CineSulApp"
{
    public partial class FormEsqueceuSenha : Form // Cria a tela de recuperação de senha
    {
        private int etapaAtual = 1; // Cria um contador para saber em qual passo o usuário está (1: Digitar Email, 2: Código, 3: Nova Senha, 4: Sucesso)

        // Reservas de memória para os elementos visuais que mudam de texto e posição durante as etapas
        private Label lblTitulo; // Reserva espaço para o texto principal (ex: "Digite seu email")
        private Label lblSubtitulo; // Reserva espaço para o texto explicativo menor
        private TextBox txtInput1; // Reserva espaço para a primeira caixa de digitação (e-mail, código ou nova senha)
        private TextBox txtInput2; // Reserva espaço para a segunda caixa de digitação (confirmação da nova senha)
        private Button btnContinuar; // Reserva espaço para o botão principal de avançar

        public FormEsqueceuSenha() // Função executada imediatamente ao abrir a tela de recuperação
        {
            ConfigurarTela(); // Prepara o tamanho, a cor de fundo e a posição da janela
            ConstruirInterface(); // Desenha na tela as caixas de texto, o painel e o botão
            AtualizarEtapa(); // Ajusta os textos e os campos para a Etapa 1 (pedir e-mail)
            {
                this.FormBorderStyle = FormBorderStyle.None; // Remove as bordas do Windows e os botões de fechar/minimizar
                this.WindowState = FormWindowState.Maximized; // Ocupa a tela inteira do computador, escondendo a barra de tarefas
            }
        }

        // Guarda as informações de endereço, porta, banco de dados, usuário e senha de acesso ao PostgreSQL
        private readonly string pgConnString = "Host=localhost;Port=5432;Database=cinesuldb;Username=postgres;Password=root;";

        private void ConfigurarTela() // Define a aparência geral da janela
        {
            this.Text = "Recuperar Senha - CÍNE SUL"; // Define o nome oficial da tela
            this.Size = new Size(1024, 800); // Define a largura em 1024 pixels e altura em 800 pixels
            this.StartPosition = FormStartPosition.CenterScreen; // Abre a janela centralizada no meio da tela
            this.BackColor = Color.FromArgb(18, 18, 24); // Pinta o fundo da janela com um tom escuro
        }

        private void ConstruirInterface() // Cria e organiza o layout visual inicial
        {
            // Header padrão
            Panel header = NavigationHelper.CriarHeader(this); // Cria a barra superior de navegação padrão
            this.Controls.Add(header); // Adiciona o cabeçalho no topo da tela

            // Container central para alinhar todos os elementos
            Panel pnlCentral = new Panel // Cria uma caixa invisível no centro da tela para segurar as caixas de texto e botões
            {
                Size = new Size(500, 350), // Define a caixa com 500 pixels de largura e 350 de altura
                BackColor = Color.Transparent // Deixa o fundo dessa caixa transparente
            };
            pnlCentral.Location = new Point((this.ClientSize.Width - pnlCentral.Width) / 2, 180); // Calcula a posição exata para centralizar a caixa horizontalmente
            pnlCentral.Anchor = AnchorStyles.None; // Impede que o painel se desloque de forma errada ao redimensionar

            // Título Principal
            lblTitulo = new Label // Cria o elemento de texto do título
            {
                ForeColor = Color.White, // Cor do texto em branco
                Font = new Font("Segoe UI", 16, FontStyle.Bold), // Fonte moderna, tamanho 16 e em negrito
                Dock = DockStyle.Top, // Gruda o título no topo da caixa central
                Height = 40, // Define a altura da caixa do título
                TextAlign = ContentAlignment.MiddleCenter // Alinha o texto exatamente no centro
            };

            // Subtítulo
            lblSubtitulo = new Label // Cria o elemento do texto explicativo
            {
                ForeColor = Color.FromArgb(180, 180, 190), // Cor cinza-claro para o texto
                Font = new Font("Segoe UI", 10), // Tamanho de fonte 10
                Dock = DockStyle.Top, // Gruda logo abaixo do título principal
                Height = 30, // Define a altura
                TextAlign = ContentAlignment.MiddleCenter // Alinha o texto no centro
            };

            // Input 1 (Email, Código ou Nova Senha)
            txtInput1 = new TextBox // Cria o primeiro campo para o usuário digitar
            {
                Size = new Size(380, 40), // Define a largura (380) e altura (40) da caixa
                Location = new Point(60, 90), // Define onde ela fica dentro do painel
                BackColor = Color.FromArgb(38, 38, 45), // Define a cor de fundo interna da caixa
                ForeColor = Color.White, // As letras digitadas serão brancas
                Font = new Font("Segoe UI", 11), // Tamanho da letra digitada
                BorderStyle = BorderStyle.FixedSingle // Define uma borda simples e discreta
            };

            // Input 2 (Confirmação de Senha)
            txtInput2 = new TextBox // Cria o segundo campo para o usuário digitar (usado só no final)
            {
                Size = new Size(380, 40), // Define tamanho igual ao do primeiro campo
                Location = new Point(60, 145), // Posiciona logo abaixo da primeira caixa
                BackColor = Color.FromArgb(38, 38, 45), // Cor de fundo interna
                ForeColor = Color.White, // Cor da letra digitada
                Font = new Font("Segoe UI", 11), // Tamanho da letra
                BorderStyle = BorderStyle.FixedSingle, // Estilo da borda
                PasswordChar = '•', // Transforma os caracteres digitados em bolinhas para esconder a senha
                Visible = false // Inicia ESCONDIDO (só vai aparecer na etapa de digitação de senha)
            };

            // Botão Continuar
            btnContinuar = new Button // Cria o botão de confirmação
            {
                Text = "Continuar", // Texto exibido dentro do botão
                Size = new Size(380, 42), // Tamanho do botão
                Location = new Point(60, 200), // Posição abaixo dos campos de digitação
                BackColor = Color.Transparent, // Fundo transparente
                ForeColor = Color.White, // Letras em branco
                Font = new Font("Segoe UI", 11, FontStyle.Bold), // Texto em negrito
                FlatStyle = FlatStyle.Flat, // Visual plano sem relevo antigo
                Cursor = Cursors.Hand // Transforma o ponteiro do mouse em "mãozinha" de clique
            };
            btnContinuar.FlatAppearance.BorderColor = Color.FromArgb(40, 120, 240); // Cor azul para a borda do botão
            btnContinuar.FlatAppearance.BorderSize = 2; // Espessura da borda azul
            btnContinuar.Click += BtnContinuar_Click; // Associa o clique do botão com a função que executa a lógica do passo a passo

            pnlCentral.Controls.AddRange(new Control[] { lblTitulo, lblSubtitulo, txtInput1, txtInput2, btnContinuar }); // Junta todos esses componentes dentro do painel central
            this.Controls.Add(pnlCentral); // Adiciona o painel central montado na tela principal

            // Ajusta posição central ao redimensionar
            this.Resize += (s, e) => // Ação que roda toda vez que a janela muda de tamanho
            {
                pnlCentral.Location = new Point((this.ClientSize.Width - pnlCentral.Width) / 2, 180); // Recalcula o centro para manter tudo alinhado
            };
        }

        private void BtnContinuar_Click(object sender, EventArgs e) // Função disparada ao clicar no botão azul de continuar
        {
            // Validações básicas por etapa
            if (etapaAtual == 1 && string.IsNullOrWhiteSpace(txtInput1.Text)) // Se estiver no Passo 1 e o e-mail estiver em branco
            {
                MessageBox.Show("Por favor, digite seu e-mail.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Mostra alerta
                return; // Impede de avançar
            }
            if (etapaAtual == 2 && string.IsNullOrWhiteSpace(txtInput1.Text)) // Se estiver no Passo 2 e o código estiver em branco
            {
                MessageBox.Show("Por favor, digite o código recebido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Mostra alerta
                return; // Impede de avançar
            }
            if (etapaAtual == 3) // Se estiver no Passo 3 (criar nova senha)
            {
                if (string.IsNullOrWhiteSpace(txtInput1.Text) || string.IsNullOrWhiteSpace(txtInput2.Text)) // Se algum dos dois campos de senha estiver em branco
                {
                    MessageBox.Show("Preencha ambos os campos de senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Alerta o usuário
                    return; // Interrompe
                }
                if (txtInput1.Text != txtInput2.Text) // Se as duas senhas digitadas forem diferentes uma da outra
                {
                    MessageBox.Show("As senhas não coincidem!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Mostra mensagem de erro
                    return; // Interrompe
                }
            }

            // Se for a última etapa, retorna para a tela de Login
            if (etapaAtual == 4) // Se a senha já foi alterada e o usuário clicar no botão final
            {
                FormLogin login = new FormLogin(); // Prepara a janela de Login
                login.Show(); // Abre o Login
                this.Hide(); // Esconde a janela de recuperação de senha
                return; // Finaliza
            }

            // Lógica especial por etapa
            if (etapaAtual == 1) // Se estiver na etapa de E-mail
            {
                string email = txtInput1.Text.Trim(); // Pega o e-mail digitado e remove espaços antes e depois

                // 1. Verifica existência do email diretamente no PostgreSQL
                try // Tenta consultar o banco com segurança
                {
                    using (var conn = new NpgsqlConnection(pgConnString)) // Abre a conexão com o banco PostgreSQL
                    {
                        conn.Open(); // Conecta
                        using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE email = @email", conn)) // Prepara o comando SQL para contar quantos usuários têm esse e-mail
                        {
                            cmd.Parameters.AddWithValue("@email", email); // Insere o e-mail digitado no parâmetro de busca
                            long count = Convert.ToInt64(cmd.ExecuteScalar()); // Executa a consulta e conta quantos resultados voltaram
                            if (count == 0) // Se for igual a zero, o e-mail não existe no sistema
                            {
                                MessageBox.Show("E-mail não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Mostra mensagem de erro
                                return; // Para o fluxo aqui
                            }
                        }
                    }
                }
                catch (Exception ex) // Se der qualquer falha no banco de dados
                {
                    MessageBox.Show("Erro ao verificar e-mail: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Mostra o erro do sistema
                    return; // Interrompe
                }
                DadosCinema.RecoveryEmail = email; // Guarda o e-mail validado na memória temporária do app
                var rnd = new Random(); // Prepara o gerador de números aleatórios
                string codigo = rnd.Next(100000, 999999).ToString(); // Gera um código numérico de 6 dígitos
                DadosCinema.RecoveryCode = codigo; // Salva esse código gerado na memória
                MessageBox.Show($"Código enviado para {email}: {codigo}", "Código enviado", MessageBoxButtons.OK, MessageBoxIcon.Information); // Simula o envio do e-mail mostrando o código em uma janela
                etapaAtual++; // Avança para o Passo 2
                AtualizarEtapa(); // Atualiza a tela com o layout do Passo 2
                return; // Finaliza o clique desta etapa
            }
            else if (etapaAtual == 2) // Se estiver no passo de Validação do Código
            {
                if (txtInput1.Text.Trim() != DadosCinema.RecoveryCode) // Se o código digitado for diferente do código gerado
                {
                    MessageBox.Show("Código inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Alerta código incorreto
                    return; // Interrompe
                }
                etapaAtual++; // Se estiver certo, avança para o Passo 3
                AtualizarEtapa(); // Atualiza a tela para a criação da nova senha
                return; // Finaliza
            }
            else if (etapaAtual == 3) // Se estiver na etapa de Gravação da Nova Senha
            {
                string novaSenha = txtInput1.Text; // Pega a nova senha digitada
                string email = DadosCinema.RecoveryEmail; // Recupera o e-mail que estava guardado na memória

                // 2. Atualiza a senha diretamente no PostgreSQL (armazenando SHA256)
                try // Tenta realizar a gravação com segurança
                {
                    using (var conn = new NpgsqlConnection(pgConnString)) // Abre a conexão com o banco
                    {
                        conn.Open(); // Conecta
                        using (var cmd = new NpgsqlCommand("UPDATE users SET senha = @senha WHERE email = @email", conn)) // Prepara o comando SQL de atualização de senha
                        {
                            cmd.Parameters.AddWithValue("@senha", GerarHashSHA256(novaSenha)); // Criptografa a nova senha em SHA256 e envia pro comando
                            cmd.Parameters.AddWithValue("@email", email); // Informa qual conta de e-mail deve ter a senha trocada
                            int rows = cmd.ExecuteNonQuery(); // Executa a alteração no banco e devolve quantas linhas foram afetadas
                            if (rows > 0) // Se alterou ao menos 1 linha no banco
                            {
                                etapaAtual++; // Avança para o Passo 4 (Tela de Sucesso)
                                AtualizarEtapa(); // Atualiza a interface
                                return; // Finaliza
                            }
                            else // Se não atualizou nenhuma linha
                            {
                                MessageBox.Show("E-mail não encontrado para atualização.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Alerta falha na atualização
                                return; // Interrompe
                            }
                        }
                    }
                }
                catch (Exception ex) // Se houver algum erro no banco de dados
                {
                    MessageBox.Show("Erro ao atualizar senha: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); // Exibe mensagem com o erro
                    return; // Interrompe
                }
            }

            etapaAtual++; // Se passar por qualquer outra situação, avança a etapa
            AtualizarEtapa(); // Atualiza o visual
        }

        // 3. CORRIGIDO E COMPLETADO: Método que atualiza os estados da interface
        private void AtualizarEtapa() // Função que redesenha os elementos da tela dependendo do número do passo atual
        {
            txtInput1.Clear(); // Limpa a primeira caixa de texto
            txtInput2.Clear(); // Limpa a segunda caixa de texto

            switch (etapaAtual) // Examina qual é a etapa atual
            {
                case 1: // Tela 1: Digitar o E-mail
                    lblTitulo.Text = "Digite seu email"; // Escreve o título
                    lblSubtitulo.Text = "Enviaremos um código para a recuperação de sua senha"; // Escreve a instrução
                    lblSubtitulo.Visible = true; // Garante que a instrução esteja visível
                    txtInput1.PasswordChar = '\0'; // Deixa o texto visível normalmente (sem bolinhas de senha)
                    txtInput2.Visible = false; // Esconde o segundo campo
                    btnContinuar.Text = "Enviar Código"; // Altera o texto do botão
                    break; // Sai do switch

                case 2: // Tela 2: Validação do Código
                    lblTitulo.Text = "Validar Código"; // Título
                    lblSubtitulo.Text = "Introduza o código de 6 dígitos enviado por e-mail"; // Instrução
                    lblSubtitulo.Visible = true; // Mantém visível
                    txtInput1.PasswordChar = '\0'; // Texto visível normalmente
                    txtInput2.Visible = false; // Mantém a segunda caixa escondida
                    btnContinuar.Text = "Validar Código"; // Texto do botão
                    break; // Sai do switch

                case 3: // Tela 3: Criar Nova Senha
                    lblTitulo.Text = "Nova Senha"; // Título
                    lblSubtitulo.Text = "Defina a sua nova credencial de acesso"; // Instrução
                    lblSubtitulo.Visible = true; // Mantém visível
                    txtInput1.PasswordChar = '•'; // Esconde a digitação da senha usando bolinhas
                    txtInput2.Visible = true;      // FAZ APARECER o segundo campo de texto para confirmar a senha
                    btnContinuar.Text = "Alterar Senha"; // Texto do botão
                    break; // Sai do switch

                case 4: // Tela 4: Conclusão com Sucesso
                    lblTitulo.Text = "Sucesso!"; // Título
                    lblSubtitulo.Text = "A sua senha foi atualizada com sucesso."; // Mensagem final
                    lblSubtitulo.Visible = true; // Mantém visível
                    txtInput1.Visible = false; // Esconde a primeira caixa de texto (pois já terminou)
                    txtInput2.Visible = false; // Esconde a segunda caixa de texto
                    btnContinuar.Text = "Voltar ao Login"; // Altera o texto do botão para voltar ao início
                    break; // Sai do switch
            }
        }

        // Método local para gerar hash SHA256 (usado ao atualizar senha)
        private string GerarHashSHA256(string input) // Função matemática de segurança que transforma uma senha legível (ex: "123456") em um código indecifrável
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create()) // Carrega o gerador de criptografia do sistema
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)); // Transforma o texto em um conjunto de bytes e calcula o código criptografado
                var sb = new System.Text.StringBuilder(); // Prepara um criador de texto dinâmico
                foreach (var b in bytes) sb.Append(b.ToString("x2")); // Converte cada pedaço do código em caracteres hexadecimais legíveis
                return sb.ToString(); // Retorna o texto criptografado pronto para ser salvo no banco
            }
        }
    }
}