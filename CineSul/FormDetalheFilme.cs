using System; // Carrega os recursos básicos do C# (como textos e mensagens de erro)
using System.Drawing; // Carrega recursos visuais (como cores, fontes e coordenadas)
using System.Windows.Forms; // Carrega as ferramentas para criar janelas, botões e painéis do Windows

namespace CineSulApp // Agrupa e organiza este arquivo dentro do aplicativo "CineSulApp"
{
    public partial class FormDetalheFilme : Form // Cria a tela visual de detalhes e compra do filme
    {
        private readonly string titulo; // Cria uma memória interna para guardar o nome do filme
        private readonly decimal preco; // Cria uma memória interna para guardar o valor/preço do ingresso

        public FormDetalheFilme(string tituloFilme, decimal precoFilme) // Ponto de partida chamado ao abrir a tela recebendo o nome e o preço do filme
        {
            titulo = tituloFilme; // Salva o nome do filme recebido dentro da memória 'titulo'
            preco = precoFilme; // Salva o valor do ingresso recebido dentro da memória 'preco'
            ConfigurarTela(); // Executa a rotina que ajusta o tamanho, a cor e a posição da tela
            ConstruirInterface(); // Executa a rotina que desenha os botões, textos e o pôster na tela
            {
                this.FormBorderStyle = FormBorderStyle.None; // Esconde a barra superior do Windows (onde ficam o botão de fechar e minimizar)
                this.WindowState = FormWindowState.Maximized; // Esconde a barra de tarefas do Windows e ocupa a tela inteira do computador
            }
        }

        private void ConfigurarTela() // Função que define a aparência inicial da janela
        {
            this.Text = $"Detalhes - {titulo}"; // Define o nome oficial da tela usando o nome do filme selecionado
            this.Size = new Size(1024, 900); // Ajusta o tamanho da janela para 1024 pixels de largura por 900 pixels de altura
            this.StartPosition = FormStartPosition.CenterScreen; // Garante que a janela seja aberta no centro exato da tela
            this.BackColor = Color.FromArgb(10, 15, 28); // Pinta o fundo da janela com um azul bem escuro
        }

        private void ConstruirInterface() // Função responsável por criar e organizar todos os elementos visuais
        {
            // Header padrão
            this.Controls.Add(NavigationHelper.CreateHeader(this)); // Coloca o menu/topo padrão de navegação do aplicativo

            // Main
            Panel main = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30), BackColor = Color.Transparent }; // Cria um painel invisível ocupando todo o espaço com uma margem de segurança de 30 pixels nas bordas
            this.Controls.Add(main); // Coloca esse painel principal dentro da janela
            main.BringToFront(); // Garante que o painel fique visível na frente do fundo ou de outros componentes

            // Coluna esquerda: poster e info rápida
            Panel left = new Panel { Size = new Size(320, 520), Location = new Point(30, 30), BackColor = Color.FromArgb(30, 30, 36) }; // Cria um retângulo escuro no lado esquerdo da tela para o pôster do filme
            Label lblPoster = new Label { Text = "[ PÔSTER ]", ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10) }; // Cria um texto temporário centralizado "[ PÔSTER ]"
            left.Controls.Add(lblPoster); // Coloca o texto temporário dentro da caixa do pôster
            main.Controls.Add(left); // Adiciona a coluna do pôster dentro do painel principal

            // Coluna direita: detalhes e compra
            Panel right = new Panel { Location = new Point(380, 30), Size = new Size(600, 520), BackColor = Color.Transparent }; // Cria uma coluna invisível no lado direito para colocar as informações e botões de compra

            Label lblTitulo = new Label { Text = titulo, ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(10, 8), AutoSize = true }; // Cria o texto com o título do filme em branco, grande e negrito
            Label lblPreco = new Label { Text = $"R$ {preco:N2}", ForeColor = Color.LimeGreen, Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(10, 48), AutoSize = true }; // Cria o texto do preço formatado em Reais na cor verde-limão

            Label lblSinopse = new Label { Text = "Descrição: Gru conhece seu irmão gêmeo Dru... (exemplo)", ForeColor = Color.Silver, Font = new Font("Segoe UI", 10), Location = new Point(10, 90), Size = new Size(560, 120) }; // Cria uma caixa de texto cinza para exibir o resumo da história do filme

            // Informações adicionais
            Label lblDuracao = new Label { Text = "Duração: 1h 30min", ForeColor = Color.Gray, Font = new Font("Segoe UI", 9), Location = new Point(10, 220), AutoSize = true }; // Cria um texto pequeno indicando o tempo de duração do filme

            // Opções de pagamento (reaproveita fluxo para Form6)
            Button btnCartao = new Button { Text = "Cartão de crédito", Size = new Size(520, 48), Location = new Point(10, 260), BackColor = Color.FromArgb(28, 28, 36), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand }; // Cria o botão cinza para pagar com Cartão de Crédito
            btnCartao.FlatAppearance.BorderSize = 0; // Remove a borda padrão do botão de crédito
            btnCartao.Click += (s, e) => IrParaPagamento(); // Configura o botão para iniciar a etapa de pagamento quando for clicado

            Button btnDebito = new Button { Text = "Cartão de débito", Size = new Size(520, 48), Location = new Point(10, 320), BackColor = Color.FromArgb(28, 28, 36), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand }; // Cria o botão cinza para pagar com Cartão de Débito
            btnDebito.FlatAppearance.BorderSize = 0; // Remove a borda padrão do botão de débito
            btnDebito.Click += (s, e) => IrParaPagamento(); // Configura o botão para iniciar a etapa de pagamento quando for clicado

            Button btnPix = new Button { Text = "Pix", Size = new Size(520, 48), Location = new Point(10, 380), BackColor = Color.FromArgb(28, 28, 36), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand }; // Cria o botão cinza para pagar com Pix
            btnPix.FlatAppearance.BorderSize = 0; // Remove a borda padrão do botão do Pix
            btnPix.Click += (s, e) => IrParaPagamento(); // Configura o botão para iniciar a etapa de pagamento quando for clicado

            right.Controls.AddRange(new Control[] { lblTitulo, lblPreco, lblSinopse, lblDuracao, btnCartao, btnDebito, btnPix }); // Adiciona todos os elementos (título, preço, descrição, botões) de uma só vez dentro da coluna direita
            main.Controls.Add(right); // Adiciona a coluna direita no painel principal da tela
        }

        private void IrParaPagamento() // Função responsável por direcionar a pessoa para a tela de pagamento
        {
            // Se não estiver logado, direciona para login
            if (!DadosCinema.IsLoggedIn) // Verifica se o usuário AINDA NÃO está conectado na conta dele
            {
                FormLogin login = new FormLogin(); // Prepara a janela de Login para ser aberta
                login.Show(); // Abre a janela de Login na tela
                this.Hide(); // Esconde a tela atual de detalhes do filme
                return; // Para a execução do código aqui para não continuar sem login
            }

            // Seta o filme escolhido e abre o fluxo de pagamento existente (Form6)
            DadosCinema.FilmeSelecionado = titulo; // Guarda no sistema o nome do filme escolhido para a compra
            Form6 telaPagamento = new Form6(); // Prepara a tela de pagamento (Form6) para ser exibida
            telaPagamento.Show(); // Abre a tela de pagamento para o usuário finalizar o pedido
            this.Hide(); // Esconde a tela atual de detalhes do filme
        }
    }
}