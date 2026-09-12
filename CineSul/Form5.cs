// Tela responsável pela seleção de snacks (bomboniere)
using System; // tipos básicos
using System.Drawing; // cores, tamanhos e posições
using System.Windows.Forms; // controles visuais

namespace CineSulApp
{
    // Esta classe representa a janela de seleção de snacks
    public partial class Form5 : Form
    {
        // Labels para exibir número de itens e valor no resumo lateral
        private Label lblTotalItensLateral;
        private Label lblTotalValorLateral;
        // Painel que conterá os cards de produtos
        private FlowLayoutPanel containerProdutos;

        // Construtor: é chamado quando a janela é criada
        public Form5()
        {
            InitializeComponent();
            {
                // Deixa a janela em tela cheia sem bordas
                this.FormBorderStyle = FormBorderStyle.None; // sem bordas
                this.WindowState = FormWindowState.Maximized; // maximizada

            }
            // Define propriedades visuais e monta a interface
            ConfigurarTela();
            ConstruirInterface();
            // Carrega a categoria padrão (Combos)
            CarregarCategoria("Combos");
        }

        // Define título, tamanho e cor de fundo da janela
        private void ConfigurarTela()
        {
            this.Text = "Bomboniere - Cíne Sul";
            this.Size = new Size(1024, 900);
            this.BackColor = Color.FromArgb(10, 15, 28);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Constrói a interface: sidebar, lista de categorias, container de produtos
        private void ConstruirInterface()
        {
            // Adiciona o cabeçalho comum da aplicação
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            // Sidebar à esquerda com informações do pedido
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = Color.FromArgb(8, 11, 20), Padding = new Padding(20) };
            this.Controls.Add(sidebar);

            // Label com o nome do filme selecionado
            Label lblFilme = new Label { Text = DadosCinema.FilmeSelecionado, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 30), Size = new Size(200, 50) };

            // Painel que exibirá total de itens e valor
            Panel boxValores = new Panel { Size = new Size(200, 120), Location = new Point(20, 220), BackColor = Color.FromArgb(24, 28, 40) };
            lblTotalItensLateral = new Label { Text = "Itens: 0", ForeColor = Color.White, Font = new Font("Segoe UI", 9), Location = new Point(15, 20), AutoSize = true };
            lblTotalValorLateral = new Label { Text = "Total: R$ 0,00", ForeColor = Color.LimeGreen, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 65), AutoSize = true };
            boxValores.Controls.AddRange(new Control[] { lblTotalItensLateral, lblTotalValorLateral });
            sidebar.Controls.AddRange(new Control[] { lblFilme, boxValores });

            // Área principal onde serão exibidos os produtos
            Panel mainArea = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 15, 28), Padding = new Padding(30) };
            this.Controls.Add(mainArea);
            mainArea.BringToFront();

            // Título da tela
            Label lblTitulo = new Label { Text = "Escolha um Snack para ver com o filme!", ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(30, 20), AutoSize = true };
            mainArea.Controls.Add(lblTitulo);

            // Botões para filtrar por categoria (Combos, Pipocas, Bebidas, Doces)
            string[] categorias = { "Combos", "Pipocas", "Bebidas", "Doces" };
            for (int i = 0; i < categorias.Length; i++)
            {
                string cat = categorias[i];
                Button btnCat = new Button { Text = cat, Size = new Size(110, 35), Location = new Point(30 + (i * 120), 60), BackColor = Color.FromArgb(24, 28, 40), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
                btnCat.FlatAppearance.BorderColor = Color.FromArgb(13, 71, 161);
                // Quando clicar, carrega a categoria correspondente
                btnCat.Click += (s, e) => CarregarCategoria(cat);
                mainArea.Controls.Add(btnCat);
            }

            // Painel que conterá os cards de produtos e permite rolagem
            containerProdutos = new FlowLayoutPanel { Location = new Point(30, 110), Size = new Size(700, 400), AutoScroll = true, BackColor = Color.FromArgb(24, 24, 30) };
            mainArea.Controls.Add(containerProdutos);

            // Botão para avançar ao pagamento
            Button btnPagamento = new Button { Text = "Ir para pagamento", Size = new Size(240, 45), Location = new Point(230, 530), BackColor = Color.FromArgb(13, 71, 161), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            btnPagamento.FlatAppearance.BorderSize = 0;
            btnPagamento.Click += (s, e) => {
                // Se o usuário não estiver logado, guarda a tela destino e abre o login
                if (!DadosCinema.IsLoggedIn)
                {
                    DadosCinema.TelaRetorno = () => new Form6(); // lembra que após login deve abrir Form6
                    FormLogin login = new FormLogin();
                    login.Show();
                    this.Hide();
                    return;
                }

                // Se estiver logado, abre a tela de pagamento (Form6)
                Form6 telaPagamento = new Form6();
                telaPagamento.Show();
                this.Hide();
            };
            mainArea.Controls.Add(btnPagamento);

            // Atualiza o resumo lateral com valores iniciais
            AtualizarInterfaceValores();
        }

        // Carrega os produtos de acordo com a categoria selecionada
        private void CarregarCategoria(string categoria)
        {
            containerProdutos.Controls.Clear();

            // Dependendo da categoria, cria cards com produtos com preço
            if (categoria == "Combos")
            {
                containerProdutos.Controls.Add(CriarCardProduto("Balde Personalizado Minions", 75.00m));
                containerProdutos.Controls.Add(CriarCardProduto("Combo Duplo Minions", 61.00m));
            }
            else if (categoria == "Pipocas")
            {
                containerProdutos.Controls.Add(CriarPipocaCard("Pipoca Grande Salgada", 22.00m));
                containerProdutos.Controls.Add(CriarPipocaCard("Pipoca Média Doce (Caramelo)", 19.50m));
            }
            else if (categoria == "Bebidas")
            {
                containerProdutos.Controls.Add(CriarPipocaCard("Refrigerante 700ml", 14.00m));
                containerProdutos.Controls.Add(CriarPipocaCard("Água Mineral 500ml", 7.00m));
            }
            else if (categoria == "Doces")
            {
                containerProdutos.Controls.Add(CriarPipocaCard("Chocolate M&Ms", 12.00m));
                containerProdutos.Controls.Add(CriarPipocaCard("Bala de Goma Fini", 9.00m));
            }
        }

        // Cria um card visual para um produto com nome, preço e controles de quantidade
        private Panel CriarCardProduto(string nome, decimal preco)
        {
            Panel p = new Panel { Size = new Size(320, 110), Margin = new Padding(10), BackColor = Color.FromArgb(40, 40, 48) };

            // Espaço reservado para a imagem do produto
            Panel imgDummy = new Panel { Size = new Size(100, 90), Location = new Point(10, 10), BackColor = Color.FromArgb(60, 60, 70) };
            Label lblImg = new Label { Text = "[ Snack ]", ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            imgDummy.Controls.Add(lblImg);

            // Nome e preço do produto
            Label lblNome = new Label { Text = nome, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(120, 15), Size = new Size(190, 35) };
            Label lblPreco = new Label { Text = $"R${preco:N2}", ForeColor = Color.FromArgb(63, 114, 252), Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(120, 50), AutoSize = true };

            // Recupera quantidade previamente adicionada (persistida em DadosCinema)
            int localQtd = 0;
            if (DadosCinema.SnackQuantities.TryGetValue(nome, out int saved)) localQtd = saved;
            // CORREÇÃO: registra o preço unitário deste snack em DadosCinema.SnackPrices.
            // Sem isso, o Form6 não teria como saber o valor de cada snack para salvar no banco
            // nem o Form8 para exibir no ticket.
            DadosCinema.SnackPrices[nome] = preco;

            Button btnMenos = new Button { Text = "-", Size = new Size(25, 22), Location = new Point(120, 75), BackColor = Color.FromArgb(63, 114, 252), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            Label lblQtd = new Label { Text = "0", ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(150, 77), Size = new Size(25, 20), TextAlign = ContentAlignment.MiddleCenter };
            Button btnMais = new Button { Text = "+", Size = new Size(25, 22), Location = new Point(180, 75), BackColor = Color.FromArgb(63, 114, 252), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            // Inicializa label com valor recuperado
            lblQtd.Text = localQtd.ToString();

            // Evento ao aumentar quantidade
            btnMais.Click += (s, e) => {
                localQtd++;
                lblQtd.Text = localQtd.ToString();
                DadosCinema.TotalSnacks += preco; // soma o preço ao total de snacks global
                DadosCinema.SnackQuantities[nome] = localQtd; // persiste quantidade do snack
                AtualizarInterfaceValores(); // atualiza o resumo lateral
            };

            // Evento ao reduzir quantidade
            btnMenos.Click += (s, e) => {
                if (localQtd > 0)
                {
                    localQtd--;
                    lblQtd.Text = localQtd.ToString();
                    DadosCinema.TotalSnacks -= preco;
                    DadosCinema.SnackQuantities[nome] = localQtd;
                    AtualizarInterfaceValores();
                }
            };

            p.Controls.AddRange(new Control[] { imgDummy, lblNome, lblPreco, btnMenos, lblQtd, btnMais });
            return p;
        }

        // Helper: pipoca usa o mesmo visual que CriarCardProduto
        private Panel CriarPipocaCard(string nome, decimal preco) => CriarCardProduto(nome, preco);

        // Atualiza a área lateral que mostra itens e valor
        private void AtualizarInterfaceValores()
        {
            lblTotalItensLateral.Text = $"Itens: {DadosCinema.TotalItens}";
            lblTotalValorLateral.Text = $"Total: R$ {DadosCinema.TotalValor:N2}";
        }
    }
}