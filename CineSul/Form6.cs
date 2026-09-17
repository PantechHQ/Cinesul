// Tela de seleção da forma de pagamento
using System; // tipos básicos e eventos
using System.Drawing; // cores e posições
using System.Windows.Forms; // controles visuais
using Npgsql; // driver de acesso ao PostgreSQL

namespace CineSulApp
{
    // Classe que representa a janela de pagamento
    public partial class Form6 : Form
    {
        // Construtor
        public Form6()
        {
            InitializeComponent();
            {
                // Deixa sem bordas e em tela cheia
                this.FormBorderStyle = FormBorderStyle.None; // sem bordas
                this.WindowState = FormWindowState.Maximized; // maximizada

            }
            // Configura visual e monta a interface
            ConfigurarTela();
            ConstruirInterface();
        }

        // Define título, tamanho e cor da janela
        private void ConfigurarTela()
        {
            this.Text = "Forma de Pagamento - Cíne Sul";
            this.Size = new Size(1024, 900);
            this.BackColor = Color.FromArgb(10, 15, 28);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Monta os controles: sidebar e opções de pagamento
        private void ConstruirInterface()
        {
            this.Controls.Add(NavigationHelper.CreateHeader(this)); // adiciona cabeçalho

            Panel sidebar = CriarSidebarLateral(); // cria painel lateral com resumo do pedido
            this.Controls.Add(sidebar);

            Panel mainArea = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 15, 28) };
            this.Controls.Add(mainArea);
            mainArea.BringToFront();

            // Título da área
            Label lblTitulo = new Label { Text = "Selecione a forma de pagamento", ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(50, 40), AutoSize = true };
            mainArea.Controls.Add(lblTitulo);

            // Opções de pagamento: cartão crédito, débito e Pix
            mainArea.Controls.Add(CriarBotaoPagamento("💳  Cartão de crédito", 50, 120, () => AvancarParaCartao()));
            mainArea.Controls.Add(CriarBotaoPagamento("💳  Cartão de débito", 50, 210, () => AvancarParaCartao()));
           // mainArea.Controls.Add(CriarBotaoPagamento("📱  Pix", 50, 300, () => AvancarDiretoIngresso()));
        }

        // Cria um painel clicável que representa uma opção de pagamento
        private Panel CriarBotaoPagamento(string texto, int x, int y, Action acao)
        {
            Panel p = new Panel { Size = new Size(600, 70), Location = new Point(x, y), BackColor = Color.FromArgb(28, 28, 36), Cursor = Cursors.Hand };
            Label lblText = new Label { Text = texto, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 22), AutoSize = true, Cursor = Cursors.Hand };
            Label lblSeta = new Label { Text = "▶", ForeColor = Color.FromArgb(63, 114, 252), Font = new Font("Segoe UI", 12), Location = new Point(550, 24), AutoSize = true, Cursor = Cursors.Hand };

            p.Controls.AddRange(new Control[] { lblText, lblSeta });

            EventHandler clique = (s, e) => acao(); // quando clicado executa a ação passada
            p.Click += clique;
            lblText.Click += clique;
            lblSeta.Click += clique;

            return p;
        }

        // Cria o painel lateral com resumo do pedido (filme, sessão, total)
        private Panel CriarSidebarLateral()
        {
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = Color.FromArgb(8, 11, 20), Padding = new Padding(20) };
            Label lblFilme = new Label { Text = DadosCinema.FilmeSelecionado, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 30), Size = new Size(200, 50) };
            Label lblSessao = new Label { Text = $"{DadosCinema.CinemaSelecionado}\n\nSessão: {DadosCinema.HorarioSelecionado}", ForeColor = Color.Silver, Font = new Font("Segoe UI", 9), Location = new Point(20, 90), Size = new Size(200, 80) };

            Panel boxValores = new Panel { Size = new Size(200, 120), Location = new Point(20, 220), BackColor = Color.FromArgb(24, 28, 40) };
            Label lblItens = new Label { Text = $"Itens: {DadosCinema.TotalItens}", ForeColor = Color.White, Font = new Font("Segoe UI", 9), Location = new Point(15, 20), AutoSize = true };
            Label lblTotal = new Label { Text = $"Total: R$ {DadosCinema.TotalValor:N2}", ForeColor = Color.LimeGreen, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 65), AutoSize = true };
            boxValores.Controls.AddRange(new Control[] { lblItens, lblTotal });

            sidebar.Controls.AddRange(new Control[] { lblFilme, lblSessao, boxValores });
            return sidebar;
        }

        // Abre a tela do cartão (Form7)
        private void AvancarParaCartao()
        {
            Form7 telaCartao = new Form7();
            telaCartao.Show();
            this.Hide();
        }

        // Simula pagamento via Pix e avança para tela de sucesso
        private void AvancarDiretoIngresso()
        {
            MessageBox.Show("Código Copia e Cola do Pix gerado! Clique em OK após simular o pagamento no banco.", "Pagamento Pix", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                SalvarPedidoNoBanco(); // grava no PostgreSQL antes de mostrar o ticket
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar pedido no banco: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // não avança se falhar ao salvar
            }

            Form8 telaSucesso = new Form8();
            telaSucesso.Show();
            this.Hide();
        }

        internal static void SalvarPedidoNoBanco()
        {
            string connString = "Host=localhost;Username=postgres;Password=root;Database=cinesuldb";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                int pedidoId;

                // Insere o pedido principal e recupera o id gerado
                using (var cmd = new NpgsqlCommand(
                    "INSERT INTO pedidos (filme, cinema, horario, total) VALUES (@f, @c, @h, @t) RETURNING id",
                    conn))
                {
                    cmd.Parameters.AddWithValue("f", DadosCinema.FilmeSelecionado);
                    cmd.Parameters.AddWithValue("c", DadosCinema.CinemaSelecionado);
                    cmd.Parameters.AddWithValue("h", DadosCinema.HorarioSelecionado);
                    cmd.Parameters.AddWithValue("t", DadosCinema.TotalValor);
                    pedidoId = (int)cmd.ExecuteScalar();
                }

                // Insere ingressos Inteira, se houver
                if (DadosCinema.QtdInteira > 0)
                {
                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO itens_pedido (pedido_id, descricao, quantidade, valor_unitario) VALUES (@p, @d, @q, @v)",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("p", pedidoId);
                        cmd.Parameters.AddWithValue("d", "Ingresso Inteira");
                        cmd.Parameters.AddWithValue("q", DadosCinema.QtdInteira);
                        cmd.Parameters.AddWithValue("v", 30.00m);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Insere ingressos Meia, se houver
                if (DadosCinema.QtdMeia > 0)
                {
                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO itens_pedido (pedido_id, descricao, quantidade, valor_unitario) VALUES (@p, @d, @q, @v)",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("p", pedidoId);
                        cmd.Parameters.AddWithValue("d", "Ingresso Meia");
                        cmd.Parameters.AddWithValue("q", DadosCinema.QtdMeia);
                        cmd.Parameters.AddWithValue("v", 15.00m);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Insere cada snack com quantidade > 0
                foreach (var snack in DadosCinema.SnackQuantities)
                {
                    if (snack.Value <= 0) continue;

                    decimal precoUnitario = DadosCinema.SnackPrices.TryGetValue(snack.Key, out decimal p) ? p : 0;

                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO itens_pedido (pedido_id, descricao, quantidade, valor_unitario) VALUES (@p, @d, @q, @v)",
                        conn))
                    {
                        cmd.Parameters.AddWithValue("p", pedidoId);
                        cmd.Parameters.AddWithValue("d", snack.Key);
                        cmd.Parameters.AddWithValue("q", snack.Value);
                        cmd.Parameters.AddWithValue("v", precoUnitario);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}