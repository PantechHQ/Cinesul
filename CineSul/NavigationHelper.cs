using System; // Importa tipos essenciais do C# (como Action e manipulação de objetos genéricos)
using System.Drawing; // Importa recursos visuais para definição de cores (Color), pontos de coordenadas (Point) e tamanhos (Size)
using System.Windows.Forms; // Importa os controles gráficos de interface do Windows Forms (Form, Panel, Button, Label, FlowLayoutPanel, Cursors)

namespace CineSulApp // Organiza a classe dentro do namespace principal do aplicativo "CineSulApp"
{
    // Classe estática auxiliar responsável por gerenciar e criar a barra de navegação (header) em todas as telas
    public static class NavigationHelper
    {
        public static Panel CriarHeader(Form currentForm) // Método em português para manter compatibilidade com chamadas antigas
        {
            return CreateHeader(currentForm); // Redireciona a chamada para o método principal CreateHeader
        }

        public static Button CreateHomeButton(Form currentForm, Point location) // Cria e configura o botão "Home" da barra de navegação
        {
            Button btnHome = new Button // Instancia um novo botão para a Home
            {
                Text = "Home", // Define o texto exibido no botão
                Size = new Size(80, 30), // Configura a dimensão para 80px de largura e 30px de altura
                Location = location, // Posiciona o botão de acordo com as coordenadas passadas
                FlatStyle = FlatStyle.Flat, // Aplica estilo plano sem efeito 3D
                ForeColor = Color.White, // Define a cor da fonte como branca
                BackColor = Color.Transparent, // Deixa o fundo do botão transparente
                Font = new Font("Segoe UI", 11, FontStyle.Bold), // Configura a fonte para Segoe UI 11pt em negrito
                Cursor = Cursors.Hand // Transforma o ponteiro do mouse na mãozinha de clique
            };
            btnHome.FlatAppearance.BorderSize = 0; // Remove completamente a borda do botão
            btnHome.Click += (s, e) => // Registra o evento de clique do botão Home
            {
                if (!(currentForm is Form1)) // Checa se a tela atual já não é a tela inicial (Form1)
                {
                    // Se o usuário estava na tela de assentos (Form3) e não finalizou
                    // a compra, descarta a seleção antes de navegar para outra tela.
                    Form3.ResetarSeAbandonado(currentForm);

                    // Limpa o pedido em andamento (ingressos, snacks, assentos) ao sair para a Home
                    DadosCinema.ResetarPedido();

                    Form1 telaHome = new Form1(); // Instancia a tela principal (Home)
                    telaHome.Show(); // Exibe a tela Home
                    currentForm.Hide(); // Oculta a tela atual que fez a navegação
                }
            };
            return btnHome; // Retorna o botão configurado
        }

        public static Button CreateFilmesButton(Form currentForm, Point location) // Cria e configura o botão "Filmes"
        {
            Button btnFilmes = new Button // Instancia o botão do catálogo de filmes
            {
                Text = "Filmes", // Texto do botão
                Size = new Size(80, 30), // Dimensão de 80x30 pixels
                Location = location, // Define a localização inicial
                FlatStyle = FlatStyle.Flat, // Visual plano
                ForeColor = Color.White, // Texto em cor branca
                BackColor = Color.Transparent, // Fundo transparente
                Font = new Font("Segoe UI", 11, FontStyle.Bold), // Fonte Segoe UI 11pt em negrito
                Cursor = Cursors.Hand // Ícone de mãozinha ao passar o mouse
            };
            btnFilmes.FlatAppearance.BorderSize = 0; // Oculta a borda
            btnFilmes.Click += (s, e) => // Registra o evento de clique
            {
                if (!(currentForm is Form9)) // Verifica se já não está na tela do catálogo de filmes (Form9)
                {
                    // Idem: descarta seleção pendente de assentos antes de sair do Form3.
                    Form3.ResetarSeAbandonado(currentForm);

                    // Limpa o pedido em andamento (ingressos, snacks, assentos) ao sair para Filmes
                    DadosCinema.ResetarPedido();

                    Form9 telaFilmes = new Form9(); // Instancia o formulário de filmes
                    telaFilmes.Show(); // Exibe a tela de filmes
                    currentForm.Hide(); // Esconde o formulário atual
                }
            };
            return btnFilmes; // Retorna o botão de filmes pronto
        }

        public static Button CreateFavoritosButton(Form currentForm, Point location) // Cria e configura o botão "Favoritos"
        {
            Button btnFav = new Button // Instancia o botão de favoritos
            {
                Text = "Favoritos", // Texto do botão
                Size = new Size(95, 30), // Largura de 95px para acomodar a palavra "Favoritos" por 30px de altura
                Location = location, // Posição do botão
                FlatStyle = FlatStyle.Flat, // Estilo plano sem elevação
                ForeColor = Color.White, // Texto em cor branca
                BackColor = Color.Transparent, // Fundo transparente
                Font = new Font("Segoe UI", 11, FontStyle.Bold), // Fonte Segoe UI 11pt em negrito
                Cursor = Cursors.Hand // Ícone de mãozinha
            };
            btnFav.FlatAppearance.BorderSize = 0; // Remove a borda
            btnFav.Click += (s, e) => // Evento disparado ao clicar
            {
                if (currentForm.GetType().Name != "FormFavoritos") // Verifica se a tela atual não é a FormFavoritos
                {
                    // Idem: descarta seleção pendente de assentos antes de sair do Form3.
                    Form3.ResetarSeAbandonado(currentForm);

                    // Limpa o pedido em andamento (ingressos, snacks, assentos) ao sair para Favoritos
                    DadosCinema.ResetarPedido();

                    FormFavoritos telaFav = new FormFavoritos(); // Instancia a tela de favoritos
                    telaFav.Show(); // Exibe a janela de favoritos
                    currentForm.Hide(); // Oculta a tela anterior
                }
            };
            return btnFav; // Retorna o botão configurado
        }

        public static Button CreateProfileButton(Form currentForm, Point location) // Cria o botão de perfil do usuário (ícone 👤)
        {
            Button btnPerfil = new Button // Instancia o botão de perfil
            {
                Text = "👤", // Desenha o ícone visual de usuário
                Size = new Size(40, 40), // Define o tamanho quadrado de 40x40 pixels
                Location = location, // Coordenada de inserção
                FlatStyle = FlatStyle.Flat, // Estilo plano
                ForeColor = Color.White, // Cor branca para o ícone
                BackColor = Color.Transparent, // Fundo transparente
                Font = new Font("Segoe UI", 14), // Fonte maior (14pt) para destacar o emoji/ícone
                Cursor = Cursors.Hand // Mãozinha de clique
            };
            btnPerfil.FlatAppearance.BorderSize = 0; // Remove bordas
            btnPerfil.Click += (s, e) => // Ação do clique no perfil
            {
                if (DadosCinema.IsLoggedIn) // Checa se o usuário já está autenticado na sessão
                {
                    MessageBox.Show("Você já está logado!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information); // Exibe aviso de usuário logado
                }
                else // Caso o usuário seja um visitante não logado
                {
                    // Se o usuário estava na tela de assentos (Form3) e não finalizou
                    // a compra, descarta a seleção antes de navegar para o login.
                    Form3.ResetarSeAbandonado(currentForm);

                    // Limpa o pedido em andamento (ingressos, snacks, assentos) ao ir para o login,
                    // já que o login abre direto na Home e não retoma a compra anterior automaticamente.
                    DadosCinema.ResetarPedido();

                    FormLogin login = new FormLogin(); // Cria a janela de Login
                    login.Show(); // Exibe a tela para entrar na conta
                    currentForm.Hide(); // Esconde a janela atual
                }
            };
            return btnPerfil; // Retorna o botão de perfil
        }

        // Botão de logout/deslogar separado para ficar visível apenas quando o usuário estiver logado
        public static Button CreateLogoutButton(Form currentForm, Point location) // Cria o botão dedicado para deslogar
        {
            Button btnLogout = new Button // Instancia o botão de saída
            {
                Text = "Sair", // Texto exibido no botão
                Size = new Size(70, 30), // Tamanho compactado (70x30 pixels)
                Location = location, // Posição de origem
                FlatStyle = FlatStyle.Flat, // Estilo plano
                ForeColor = Color.White, // Texto em cor branca
                BackColor = Color.FromArgb(200, 40, 40), // Cor vermelho-alerta para destaque visual de encerramento
                Font = new Font("Segoe UI", 9, FontStyle.Bold), // Fonte em negrito tamanho 9pt
                Cursor = Cursors.Hand, // Cursor em modo de mãozinha
                Visible = DadosCinema.IsLoggedIn // Só exibe o botão na tela se o usuário estiver autenticado (true)
            };
            btnLogout.FlatAppearance.BorderSize = 0; // Sem borda externa
            btnLogout.Click += (s, e) => // Ação disparada ao clicar no botão Sair
            {
                if (!DadosCinema.IsLoggedIn) // Validação de segurança extra se por acaso não estiver logado
                {
                    // Se não estiver logado, abre a tela de login
                    Form3.ResetarSeAbandonado(currentForm); // Descarta seleção pendente de assentos, se houver


                    FormLogin login = new FormLogin(); // Cria o formulário de login
                    login.Show(); // Abre o login
                    currentForm.Hide(); // Esconde a tela atual
                    return; // Interrompe o método
                }

                // Se o usuário estava na tela de assentos (Form3) e não finalizou
                // a compra, descarta a seleção antes de deslogar e sair da tela.
                Form3.ResetarSeAbandonado(currentForm);

                // Desloga o usuário
                DadosCinema.IsLoggedIn = false; // Altera o status global para não-autenticado
                DadosCinema.LoggedUserEmail = string.Empty; // Limpa o e-mail do usuário logado na memória
                MessageBox.Show("Você foi deslogado.", "Sessão encerrada", MessageBoxButtons.OK, MessageBoxIcon.Information); // Alerta o usuário do término da sessão

                // Abre sempre o Form1 (home) e fecha o formulário atual
                Form1 telaHome = new Form1(); // Instancia uma nova página inicial
                telaHome.Show(); // Exibe a página inicial
                // Fecha o formulário atual em vez de apenas escondê-lo
                try // Bloco seguro para fechamento de janela
                {
                    currentForm.Close(); // Encerra a janela atual e liberta seus recursos
                }
                catch { } // Trata silenciosamente eventuais exceções de descarte
            };
            return btnLogout; // Retorna o botão de logout configurado
        }

        public static Panel CreateHeader(Form currentForm) // Função principal responsável por construir a barra de cabeçalho completa
        {
            // Tom de azul idêntico ao da referência da segunda imagem
            Panel header = new Panel // Instancia o painel do cabeçalho que ficará no topo da tela
            {
                Dock = DockStyle.Top, // Fixa o painel no topo da janela acompanhando a largura total
                Height = 65, // Define a altura fixa em 65 pixels
                BackColor = Color.FromArgb(28, 79, 186) // Cor azul royal característica do CineSul
            };

            // Logo CÍNE SUL em duas linhas conforme o design
            Label lblLogo = new Label // Instancia o texto da marca da empresa
            {
                Text = "CÍNE\nSUL", // Define o nome com uma quebra de linha para formar duas camadas
                ForeColor = Color.White, // Cor da fonte em branco
                Font = new Font("Segoe UI", 11, FontStyle.Bold), // Fonte Segoe UI 11pt em negrito
                AutoSize = true // Ajusta a caixa automaticamente com base na tipografia
            };

            // Menu Central
            FlowLayoutPanel navPanel = new FlowLayoutPanel // Painel organizador automático para alinhar os botões do menu horizontalmente
            {
                FlowDirection = FlowDirection.LeftToRight, // Empilha os itens da esquerda para a direita
                WrapContents = false, // Impede a quebra de linha dos botões
                AutoSize = true, // Ajusta o tamanho da área do menu dinamicamente conforme ganha conteúdo
                BackColor = Color.Transparent // Mantém o fundo transparente para harmonizar com o azul do header
            };

            Button btnFilmes = CreateFilmesButton(currentForm, new Point(0, 0)); // Cria o botão "Filmes"
            Label sep1 = new Label { Text = "|", ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true }; // Cria o primeiro separador vertical "|"
            Button btnHome = CreateHomeButton(currentForm, new Point(0, 0)); // Cria o botão "Home"
            Label sep2 = new Label { Text = "|", ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true }; // Cria o segundo separador vertical "|"
            Button btnFavoritos = CreateFavoritosButton(currentForm, new Point(0, 0)); // Cria o botão "Favoritos"

            // Ajuste dos espaçamentos internos para alinhar perfeitamente o "Filmes | Home | Favoritos"
            btnFilmes.Margin = new Padding(0, 2, 0, 0); // Ajusta a margem do botão Filmes
            btnHome.Margin = new Padding(0, 2, 0, 0); // Ajusta a margem do botão Home
            btnFavoritos.Margin = new Padding(0, 0, 0, 0); // Reseta a margem do botão Favoritos
            sep1.Margin = new Padding(4, 6, 4, 0); // Define espaço nas laterais do separador 1 para descolar dos botões
            sep2.Margin = new Padding(4, 6, 4, 0); // Define espaço nas laterais do separador 2 para descolar dos botões

            navPanel.Controls.AddRange(new Control[] { btnFilmes, sep1, btnHome, sep2, btnFavoritos }); // Insere os elementos ordenadamente dentro do FlowLayoutPanel

            // Botão de Perfil (Canto direito)
            Button btnPerfil = CreateProfileButton(currentForm, new Point(0, 0)); // Cria o botão do perfil do usuário

            header.Controls.Add(lblLogo); // Adiciona o logo da marca ao painel do cabeçalho
            header.Controls.Add(navPanel); // Adiciona o menu de navegação central ao painel
            header.Controls.Add(btnPerfil); // Adiciona o botão do perfil ao painel
            // Adiciona o botão de logout ao lado direito do perfil
            Button btnLogout = CreateLogoutButton(currentForm, new Point(0, 0)); // Instancia o botão para deslogar da conta
            header.Controls.Add(btnLogout); // Adiciona o botão de logout ao painel do cabeçalho

            // Método de reposicionamento responsivo
            Action reposition = () => // Função anônima/lambda responsável pelo cálculo e centralização em caso de redimensionamento
            {
                try // Bloco seguro para evitar exceções durante ajustes de tela
                {
                    lblLogo.Left = 20; // Fixa a marca CÍNE SUL a 20px da borda esquerda
                    lblLogo.Top = Math.Max(0, (header.ClientSize.Height - lblLogo.Height) / 2); // Centraliza a marca na vertical

                    int centered = (header.ClientSize.Width - navPanel.Width) / 2; // Calcula a posição exata do centro da janela para os links
                    navPanel.Left = Math.Max(lblLogo.Right + 20, centered); // Posiciona o menu no centro (desde que não atropele o logo)
                    navPanel.Top = Math.Max(15, (header.ClientSize.Height - navPanel.Height) / 2); // Centraliza o menu na vertical

                    btnPerfil.Left = header.ClientSize.Width - btnPerfil.Width - 95; // Alinha o perfil na direita (dando 95px de margem para acomodar o botão "Sair")
                    btnPerfil.Top = Math.Max(0, (header.ClientSize.Height - btnPerfil.Height) / 2); // Centraliza o botão de perfil na vertical

                    // Reposiciona o botão de logout, caso exista
                    foreach (Control c in header.Controls) // Percorre os elementos contidos no cabeçalho
                    {
                        if (c is Button b && b.Text == "Sair") // Filtra e encontra o botão correspondente ao Logout
                        {
                            b.Left = header.ClientSize.Width - b.Width - 15; // Fixa o botão Sair no extremo direito com 15px de margem
                            b.Top = Math.Max(0, (header.ClientSize.Height - b.Height) / 2); // Centraliza o botão na vertical
                        }
                    }
                }
                catch { } // Ignora erros de layout se a tela estiver minimizada ou em fechamento
            };

            header.SizeChanged += (s, e) => reposition(); // Dispara o reposicionamento responsivo ao redimensionar o painel
            currentForm.Resize += (s, e) => reposition(); // Dispara o reposicionamento responsivo ao redimensionar o formulário principal

            reposition(); // Executa o cálculo e posicionamento inicial de forma imediata
            header.BringToFront(); // Garante que a barra do cabeçalho fique sobreposta a qualquer outro controle da página

            return header; // Retorna o painel do cabeçalho completamente pronto
        }
    }
}