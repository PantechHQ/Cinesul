// Tela de confirmação do pedido (mostra ingresso estilizado e resumo)
using System; // tipos básicos
using System.Drawing; // manipulação de cores e tamanhos
using System.Windows.Forms; // controles visuais

namespace CineSulApp
{
    // Classe que representa a tela final após compra
    public partial class Form8 : Form
    {
        // Construtor
        public Form8()
        {
            InitializeComponent();
            {
                // Deixa a janela sem moldura e em tela cheia
                this.FormBorderStyle = FormBorderStyle.None; // sem bordas
                this.WindowState = FormWindowState.Maximized; // maximizada

            }
            ConfigurarTela();
            ConstruirInterface();
        }

        // Define título, tamanho e fundo da janela
        private void ConfigurarTela()
        {
            this.Text = "Pedido Concluído - Cíne Sul";
            this.Size = new Size(1024, 900);
            this.BackColor = Color.FromArgb(10, 15, 28);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Monta a interface visual da confirmação do pedido
        private void ConstruirInterface()
        {
            this.Controls.Add(NavigationHelper.CreateHeader(this)); // adiciona cabeçalho

            Panel sidebar = CriarSidebarLateral(); // adiciona resumo lateral (apenas filme)
            this.Controls.Add(sidebar);

            // Mensagem de sucesso
            Label lblSucesso = new Label { Text = "Seu pedido foi feito! Aproveite o filme", ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(390, 110), AutoSize = true };
            this.Controls.Add(lblSucesso);

            // Painel que representa visualmente um ingresso (fundo claro).
            // Aumentado de altura (era 240) para caber a lista de itens comprados.
            Panel ticket = new Panel { Size = new Size(540, 320), Location = new Point(350, 180), BackColor = Color.FromArgb(245, 247, 250) };
            this.Controls.Add(ticket);

            // Monta a lista de itens comprados (ingressos + snacks) a partir de DadosCinema.
            // Isso é montado ANTES do reset do carrinho, senão os valores já viriam zerados.
            string itensTexto = "";

            if (DadosCinema.QtdInteira > 0)
                itensTexto += $"{DadosCinema.QtdInteira}x Ingresso Inteira - R$ {(DadosCinema.QtdInteira * 30.00m):N2}\n";

            if (DadosCinema.QtdMeia > 0)
                itensTexto += $"{DadosCinema.QtdMeia}x Ingresso Meia - R$ {(DadosCinema.QtdMeia * 15.00m):N2}\n";

            foreach (var snack in DadosCinema.SnackQuantities)
            {
                if (snack.Value <= 0) continue;
                decimal precoUnitario = DadosCinema.SnackPrices.TryGetValue(snack.Key, out decimal preco) ? preco : 0;
                itensTexto += $"{snack.Value}x {snack.Key} - R$ {(snack.Value * precoUnitario):N2}\n";
            }

            if (string.IsNullOrEmpty(itensTexto))
                itensTexto = "(nenhum item encontrado)\n";

            // Guarda o total antes do reset, para exibir no ticket
            decimal totalPedido = DadosCinema.TotalValor;

            // Informações textuais do ingresso (filme, cinema, horário e itens comprados)
            Label lblInfoTicket = new Label
            {
                Text = $"Filme: {DadosCinema.FilmeSelecionado}\n" +
                       $"Cinema: {DadosCinema.CinemaSelecionado}\n\n" +
                       $"-------------------------------------------\n" +
                       itensTexto +
                       $"-------------------------------------------\n" +
                       $"Total: R$ {totalPedido:N2}\n" +
                       $"Horário: {DadosCinema.HorarioSelecionado}\n" +
                       $"Assentos Escolhidos: Sorteados",
                ForeColor = Color.Black,
                Font = new Font("Consolas", 10, FontStyle.Bold),
                Location = new Point(25, 25),
                Size = new Size(490, 270)
            };
            ticket.Controls.Add(lblInfoTicket);

            // Pequeno círculo do canhoto do ingresso (visualmente redondo)
            Panel circuloCanhoto = new Panel { Size = new Size(50, 50), Location = new Point(440, 135), BackColor = Color.Black };
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, 50, 50);
            circuloCanhoto.Region = new Region(path);
            ticket.Controls.Add(circuloCanhoto);

            // Botão que finaliza e reinicia o fluxo (limpa carrinho e volta para a home)
            Button btnVoltarInicio = new Button { Text = "Comprar mais ingressos", Size = new Size(240, 45), Location = new Point(500, 550), BackColor = Color.FromArgb(13, 71, 161), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            btnVoltarInicio.FlatAppearance.BorderSize = 0;
            btnVoltarInicio.Click += (s, e) => {
                // Reseta o estado global do carrinho (usa o método central em DadosCinema)
                DadosCinema.ResetarPedido();

                Form1 home = new Form1();
                home.Show();
                this.Close();
            };
            this.Controls.Add(btnVoltarInicio);
        }

        // Cria o painel lateral simples que mostra o nome do filme selecionado
        private Panel CriarSidebarLateral()
        {
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = Color.FromArgb(8, 11, 20), Padding = new Padding(20) };
            Label lblFilme = new Label { Text = DadosCinema.FilmeSelecionado, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 30), Size = new Size(200, 50) };
            sidebar.Controls.Add(lblFilme);
            return sidebar;
        }
    }
}