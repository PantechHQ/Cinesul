// Esta janela coleta os dados do cartão do usuário para processar o pagamento
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CineSulApp
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            ConfigurarTela();
            ConstruirInterface();
        }

        private void ConfigurarTela()
        {
            this.Text = "Dados do Cartão - Cíne Sul";
            this.Size = new Size(1024, 900);
            this.BackColor = Color.FromArgb(10, 15, 28);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void ConstruirInterface()
        {
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            Panel sidebar = CriarSidebarLateral();
            this.Controls.Add(sidebar);

            // Largura disponível descontando a sidebar (240px)
            int larguraUtil = this.ClientSize.Width - 240;
            int posXForm = 240 + ((larguraUtil - 640) / 2);
            int posYForm = (this.ClientSize.Height - 400) / 2;

            // Painel centralizado na área de conteúdo
            Panel formCard = new Panel
            {
                Size = new Size(640, 400),
                Location = new Point(posXForm, posYForm),
                BackColor = Color.FromArgb(34, 34, 42)
            };

            // Recalcula o centro se a tela for redimensionada
            this.Resize += (s, e) =>
            {
                int novaLarguraUtil = this.ClientSize.Width - 240;
                formCard.Location = new Point(240 + ((novaLarguraUtil - 640) / 2), (this.ClientSize.Height - 400) / 2);
            };

            this.Controls.Add(formCard);

            // Campos para número, validade, CVV e nome
            TextBox txtNumero = CriarCampoTexto(formCard, "Número do cartão", "0000 0000 0000 0000", 40, 30, 560);
            txtNumero.MaxLength = 19;

            TextBox txtValidade = CriarCampoTexto(formCard, "Data de validade", "MM/AA", 40, 120, 260);
            txtValidade.MaxLength = 5; // Formato MM/AA tem 5 caracteres

            // Formatação automática com a barra na validade (MM/AA)
            txtValidade.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true; // Permite apenas números e Backspace
                }
            };

            txtValidade.TextChanged += (s, e) =>
            {
                string texto = txtValidade.Text.Replace("/", "");
                if (texto.Length >= 2 && !txtValidade.Text.Contains("/"))
                {
                    txtValidade.Text = texto.Insert(2, "/");
                    txtValidade.SelectionStart = txtValidade.Text.Length; // Coloca o cursor no final
                }
            };

            TextBox txtCvv = CriarCampoTexto(formCard, "Código de segurança", "3 dígitos", 340, 120, 260);
            txtCvv.MaxLength = 3;

            TextBox txtNome = CriarCampoTexto(formCard, "Nome do titular do cartão", "Nome completo do titular", 40, 210, 560);

            // Botões organizados lado a lado no rodapé do painel
            int larguraBotoes = 140 + 20 + 240; // Voltar (140) + Espaço (20) + Finalizar (240)
            int posXBotoes = (640 - larguraBotoes) / 2;

            // Botão para voltar à etapa anterior
            Button btnVoltar = new Button
            {
                Text = "Voltar",
                Size = new Size(140, 45),
                Location = new Point(posXBotoes, 310),
                BackColor = Color.FromArgb(60, 60, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnVoltar.FlatAppearance.BorderSize = 0;
            btnVoltar.Click += (s, e) => {
                Form6 telaAnterior = new Form6();
                telaAnterior.Show();
                this.Close();
            };
            formCard.Controls.Add(btnVoltar);

            // Botão que finaliza o pagamento
            Button btnFinalizar = new Button
            {
                Text = "Finalizar pagamento",
                Size = new Size(240, 45),
                Location = new Point(posXBotoes + 160, 310),
                BackColor = Color.FromArgb(13, 71, 161),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFinalizar.FlatAppearance.BorderSize = 0;
            btnFinalizar.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtNumero.Text) ||
                    string.IsNullOrWhiteSpace(txtValidade.Text) ||
                    string.IsNullOrWhiteSpace(txtCvv.Text) ||
                    string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Preencha todos os campos do cartão.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarNumeroCartao(txtNumero.Text))
                {
                    MessageBox.Show("Número do cartão inválido. Digite os 16 dígitos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarValidade(txtValidade.Text))
                {
                    MessageBox.Show("Data de validade inválida. Use o formato MM/AA.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarCvv(txtCvv.Text))
                {
                    MessageBox.Show("Código de segurança (CVV) inválido. Digite 3 dígitos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidarNomeTitular(txtNome.Text))
                {
                    MessageBox.Show("Nome no cartão inválido. Use apenas letras e espaços.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    Form6.SalvarPedidoNoBanco();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao salvar pedido no banco: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Form8 telaSucesso = new Form8();
                telaSucesso.Show();
                this.Hide();
            };
            formCard.Controls.Add(btnFinalizar);
        }

        private bool ValidarNumeroCartao(string numero)
        {
            string apenasDigitos = numero.Replace(" ", "");
            return Regex.IsMatch(apenasDigitos, @"^\d{16}$");
        }

        private bool ValidarValidade(string validade)
        {
            Match match = Regex.Match(validade, @"^(\d{2})/(\d{2})$");
            if (!match.Success) return false;

            int mes = int.Parse(match.Groups[1].Value);
            int ano = int.Parse(match.Groups[2].Value);

            if (mes < 1 || mes > 12) return false;

            int anoAtual = DateTime.Now.Year % 100;
            int mesAtual = DateTime.Now.Month;

            if (ano < anoAtual) return false;
            if (ano == anoAtual && mes < mesAtual) return false;

            return true;
        }

        private bool ValidarCvv(string cvv)
        {
            return Regex.IsMatch(cvv, @"^\d{3}$");
        }

        private bool ValidarNomeTitular(string nome)
        {
            return Regex.IsMatch(nome.Trim(), @"^[A-Za-zÀ-ÖØ-öø-ÿ]+(\s[A-Za-zÀ-ÖØ-öø-ÿ]+)*$");
        }

        private TextBox CriarCampoTexto(Panel pai, string labelText, string placeholder, int x, int y, int largura)
        {
            Label lbl = new Label { Text = labelText, ForeColor = Color.LightGray, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(x, y), AutoSize = true };
            TextBox txt = new TextBox { Location = new Point(x, y + 25), Size = new Size(largura, 30), BackColor = Color.FromArgb(50, 50, 58), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 11) };

            pai.Controls.AddRange(new Control[] { lbl, txt });
            return txt;
        }

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
    }
}