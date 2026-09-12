// Tela que permite ler e enviar comentários sobre um filme.
// Comentários explicativos adicionados para facilitar manutenção,
// sem alterar comportamento funcional existente.
using System;
using System.Drawing;
using System.Windows.Forms;


namespace CineSulApp
{
    // Formulário de comentários do filme.
    // Fornece interface para leitura e envio de comentários e avaliação por estrelas.
    public partial class FormComentarios : Form
    {
        // Título do filme atual exibido no formulário
        private string filme;
        private TextBox txtComentario;
        private FlowLayoutPanel flowComentarios;
        private int avaliacao = 0; // 0-5
        private Label lblSemComentarios;
        private FlowLayoutPanel estrelasTop;
        private FlowLayoutPanel estrelasBottom;

        // Construtor: recebe o título do filme e prepara a interface
        public FormComentarios(string tituloFilme)
        {
            filme = tituloFilme;
            // Não chamar InitializeComponent manualmente — o designer parcial fornece o método.
            ConfigurarTela();
            ConstruirInterface();
            {
                this.FormBorderStyle = FormBorderStyle.None; // Remove as bordas e a barra de título
                this.WindowState = FormWindowState.Maximized; // Ocupa a tela inteira, cobrindo a barra de tarefas

            }
        }


        // Define propriedades básicas da janela (título, tamanho, posição e cor)
        private void ConfigurarTela()
        {
            this.Text = $"Comentários - {filme}";
            this.Size = new Size(1024, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 24);
        }

        // Monta a interface gráfica do formulário programaticamente.
        // Cria os controles, define propriedades e associa eventos.
        private void ConstruirInterface()
        {
            // 1. Usa o NavigationHelper oficial da aplicação
            Panel header = NavigationHelper.CriarHeader(this);
            this.Controls.Add(header);

            Label lblFilme = new Label
            {
                Text = $"Filme: {filme}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 85),
                AutoSize = true
            };
            this.Controls.Add(lblFilme);

            // Area de comentário
            Panel box = new Panel
            {
                Size = new Size(740, 130),
                Location = new Point(20, 125),
                BackColor = Color.FromArgb(30, 30, 36)
            };

            // TextBox principal na esquerda
            txtComentario = new TextBox
            {
                Multiline = true,
                Size = new Size(500, 90),
                Location = new Point(15, 15),
                BackColor = Color.FromArgb(40, 40, 48),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            box.Controls.Add(txtComentario);

            // Estrelas: 3 em cima
            estrelasTop = new FlowLayoutPanel
            {
                Location = new Point(530, 15),
                Size = new Size(80, 30),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            for (int i = 1; i <= 3; i++)
            {
                Label star = new Label { Text = "☆", ForeColor = Color.Gold, Font = new Font("Segoe UI", 14), Cursor = Cursors.Hand, Tag = i, AutoSize = true };
                star.Margin = new Padding(1, 0, 1, 0);
                star.Click += Star_Click;
                estrelasTop.Controls.Add(star);
            }
            box.Controls.Add(estrelasTop);

            // Estrelas: 2 embaixo
            estrelasBottom = new FlowLayoutPanel
            {
                Location = new Point(530, 48),
                Size = new Size(60, 30),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            for (int i = 4; i <= 5; i++)
            {
                Label star = new Label { Text = "☆", ForeColor = Color.Gold, Font = new Font("Segoe UI", 14), Cursor = Cursors.Hand, Tag = i, AutoSize = true };
                star.Margin = new Padding(1, 0, 1, 0);
                star.Click += Star_Click;
                estrelasBottom.Controls.Add(star);
            }
            box.Controls.Add(estrelasBottom);

            // Botão comentar
            Button btnComentar = new Button
            {
                Text = "Comentar",
                Size = new Size(110, 35),
                Location = new Point(615, 80),
                BackColor = Color.FromArgb(63, 114, 252),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnComentar.FlatAppearance.BorderSize = 0;
            btnComentar.Click += BtnComentar_Click;
            box.Controls.Add(btnComentar);

            this.Controls.Add(box);
            // Lista de comentários (inicialmente vazia)
            flowComentarios = new FlowLayoutPanel
            {
                Location = new Point(20, 260),
                Size = new Size(740, 280),
                AutoScroll = true
            };
            this.Controls.Add(flowComentarios);

            // Mensagem quando não houver comentários
            lblSemComentarios = new Label { Text = "Não há comentários sobre o filme.", ForeColor = Color.Silver, Font = new Font("Segoe UI", 10), AutoSize = true, Location = new Point(10, 10) };
            flowComentarios.Controls.Add(lblSemComentarios);
        }

        // Evento disparado ao clicar em uma estrela: atualiza a avaliação e redesenha as estrelas.
        private void Star_Click(object sender, EventArgs e)
        {
            if (sender is Label starClicked && starClicked.Tag != null)
            {
                avaliacao = Convert.ToInt32(starClicked.Tag);
                UpdateStars();
            }
        }

        // Atualiza visualmente as estrelas conforme o valor atual de 'avaliacao'.
        private void UpdateStars()
        {
            foreach (Control c in estrelasTop.Controls)
            {
                if (c is Label lbl && lbl.Tag != null)
                {
                    int val = Convert.ToInt32(lbl.Tag);
                    lbl.Text = val <= avaliacao ? "★" : "☆";
                }
            }
            foreach (Control c in estrelasBottom.Controls)
            {
                if (c is Label lbl && lbl.Tag != null)
                {
                    int val = Convert.ToInt32(lbl.Tag);
                    lbl.Text = val <= avaliacao ? "★" : "☆";
                }
            }
        }

        // Handler do botão de comentar: valida a entrada, remove placeholder e adiciona o comentário
        private void BtnComentar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtComentario.Text))
            {
                MessageBox.Show("Escreva um comentário antes de enviar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (lblSemComentarios != null && flowComentarios.Controls.Contains(lblSemComentarios))
            {
                flowComentarios.Controls.Remove(lblSemComentarios);
            }

            AdicionarComentarioExemplo("Você", txtComentario.Text.Trim(), avaliacao);

            // Reset do form de envio
            txtComentario.Clear();
            avaliacao = 0;
            UpdateStars();
        }

        // Cria um painel representando um comentário e o adiciona à lista visual.
        // Obs: os comentários adicionados aqui não são persistidos em banco de dados.
        private void AdicionarComentarioExemplo(string usuario, string texto, int estrelas)
        {
            Panel p = new Panel { Size = new Size(710, 80), BackColor = Color.FromArgb(30, 30, 36), Margin = new Padding(0, 0, 0, 10) };
            Label lblUser = new Label { Text = usuario, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
            Label lblText = new Label { Text = texto, ForeColor = Color.Silver, Font = new Font("Segoe UI", 9), Location = new Point(10, 35), Size = new Size(550, 40) };
            Label lblStars = new Label { Text = new string('★', estrelas) + new string('☆', 5 - estrelas), ForeColor = Color.Gold, Location = new Point(580, 20), AutoSize = true, Font = new Font("Segoe UI", 11) };

            p.Controls.AddRange(new Control[] { lblUser, lblText, lblStars });
            flowComentarios.Controls.Add(p);
        }
    }
}