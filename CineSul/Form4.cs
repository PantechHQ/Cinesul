// Explicaçao linha a linha: este arquivo define a janela que permite escolher tipos de ingresso
using System;
// Importa as ferramentas para trabalhar com posições, cores e tamanhos na tela
using System.Drawing;
// Importa elementos visuais como botões, labels e painéis
using System.Windows.Forms;

// Declara o espaço de nomes do aplicativo, onde todas as telas estão agrupadas
namespace CineSulApp
{
    // Declara que esta classe representa uma janela (formulário) do aplicativo
    public partial class Form4 : Form
    {
        // Label que mostra a quantidade de itens no resumo lateral
        private Label lblTotalItensLateral;
        // Label que mostra o valor total no resumo lateral
        private Label lblTotalValorLateral;
        // Label que mostra a quantidade de ingressos do tipo inteira
        private Label lblQtdInteira;
        // Label que mostra a quantidade de ingressos do tipo meia
        private Label lblQtdMeia;

        // Construtor: método que é chamado quando a janela é criada
        public Form4()
        {
            // Chama o método que configura componentes gerados pelo designer (se houver)
            InitializeComponent();
            {
                // Remove as bordas e a barra de título da janela
                this.FormBorderStyle = FormBorderStyle.None; // visual: sem moldura
                // Faz a janela ocupar a tela inteira
                this.WindowState = FormWindowState.Maximized; // visual: tela cheia

            }
            // Configura propriedades gerais da janela (título, tamanho, cores)
            ConfigurarTela();
            // Monta os controles que o usuário verá (botões, textos, painéis)
            ConstruirInterface();
            // Atualiza o resumo lateral com valores iniciais
            AtualizarCarrinhoLateral();
        }

        // Método para definir título, tamanho e cor da janela
        private void ConfigurarTela()
        {
            // Texto que aparece na barra de título (quando visível)
            this.Text = "Tipo de Ingresso - Cíne Sul";
            // Tamanho preferido da janela (quando não estiver maximizada)
            this.Size = new Size(1024, 900);
            // Cor de fundo da janela
            this.BackColor = Color.FromArgb(10, 15, 28);
            // Posição inicial da janela ao abrir
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Método para criar e organizar todos os elementos visuais dentro da janela
        private void ConstruirInterface()
        {
            // Adiciona o cabeçalho padronizado do aplicativo (com logo e navegação)
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            // Cria uma coluna lateral à esquerda para informações do pedido
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = Color.FromArgb(8, 11, 20), Padding = new Padding(20) };
            // Adiciona o painel lateral à janela
            this.Controls.Add(sidebar);

            // Label que mostra o título do filme selecionado (pega do objeto DadosCinema)
            Label lblFilme = new Label { Text = DadosCinema.FilmeSelecionado, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 30), Size = new Size(200, 50) };
            // Label que mostra o cinema e horário da sessão (também pega de DadosCinema)
            Label lblSessao = new Label { Text = $"{DadosCinema.CinemaSelecionado}\n\nSessão: {DadosCinema.HorarioSelecionado}", ForeColor = Color.Silver, Font = new Font("Segoe UI", 9), Location = new Point(20, 90), Size = new Size(200, 80) };

            // Caixa que exibirá o total de itens e valor na lateral
            Panel boxValores = new Panel { Size = new Size(200, 120), Location = new Point(20, 220), BackColor = Color.FromArgb(24, 28, 40) };
            // Label que mostrará a quantidade de itens (inicia em 0)
            lblTotalItensLateral = new Label { Text = "Itens: 0", ForeColor = Color.White, Font = new Font("Segoe UI", 9), Location = new Point(15, 20), AutoSize = true };
            // Label que mostrará o valor total (inicia em 0,00)
            lblTotalValorLateral = new Label { Text = "Total: R$ 0,00", ForeColor = Color.LimeGreen, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 65), AutoSize = true };
            // Adiciona os labels ao painel de valores
            boxValores.Controls.AddRange(new Control[] { lblTotalItensLateral, lblTotalValorLateral });
            // Adiciona ao sidebar as informações do filme, sessão e o painel de valores
            sidebar.Controls.AddRange(new Control[] { lblFilme, lblSessao, boxValores });

            // Área principal da janela onde ficarão os controles centrais
            Panel mainArea = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 15, 28) };
            this.Controls.Add(mainArea);
            // Garante que a área principal fique acima de outros controles
            mainArea.BringToFront();

            // Título principal na área central
            Label lblTitulo = new Label { Text = "Escolha o tipo de ingresso", ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(50, 40), AutoSize = true };
            mainArea.Controls.Add(lblTitulo);

            // Cria um painel para ingressos do tipo 'Inteira' e obtém a label que mostra a quantidade
            Panel panelInteira = CriarPainelIngresso("Inteira", "R$ 30.00", 50, 110, out lblQtdInteira, true);
            // Cria um painel para ingressos do tipo 'Meia' e obtém a label que mostra a quantidade
            Panel panelMeia = CriarPainelIngresso("Meia", "R$ 15.00", 50, 210, out lblQtdMeia, false);

            // Adiciona os dois painéis de tipo de ingresso à área principal
            mainArea.Controls.AddRange(new Control[] { panelInteira, panelMeia });

            // Botão que leva o usuário para a próxima etapa (bomboniere)
            Button btnAvancar = new Button { Text = "Ir para bomboniere", Size = new Size(240, 45), Location = new Point(230, 360), BackColor = Color.FromArgb(13, 71, 161), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            btnAvancar.FlatAppearance.BorderSize = 0;
            // Evento que ocorre quando o usuário clica no botão
            btnAvancar.Click += (s, e) => {
                // Se o usuário selecionou ao menos um ingresso, abre a tela da bomboniere
                if (DadosCinema.TotalItens > 0)
                {
                    Form5 bomboniere = new Form5();
                    bomboniere.Show();
                    this.Hide();
                }
                else
                {
                    // Se não selecionou nada, mostra uma mensagem pedindo para selecionar
                    MessageBox.Show("Selecione pelo menos um ingresso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            mainArea.Controls.Add(btnAvancar);
        }

        // Método que cria um painel para um tipo de ingresso (inteira ou meia)
        private Panel CriarPainelIngresso(string tipo, string preco, int x, int y, out Label lblQuantidade, bool isInteira)
        {
            // Cria um painel retangular com cor de fundo escura
            Panel p = new Panel { Size = new Size(600, 80), Location = new Point(x, y), BackColor = Color.FromArgb(28, 28, 36) };

            // Label que mostra o nome do tipo (Inteira/Meia)
            Label lblTipo = new Label { Text = tipo, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(70, 18), AutoSize = true };
            // Label que mostra o preço do ingresso
            Label lblPreco = new Label { Text = preco, ForeColor = Color.Gray, Font = new Font("Segoe UI", 10), Location = new Point(70, 42), AutoSize = true };

            // Botão para reduzir a quantidade
            Button btnMenos = new Button { Text = "-", Size = new Size(35, 30), Location = new Point(440, 25), BackColor = Color.FromArgb(63, 114, 252), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            // Label que mostra a quantidade selecionada para este tipo.
            // CORREÇÃO: nasce com o valor já salvo em DadosCinema (e não fixo em "0"),
            // pra não dessincronizar da contagem real caso o usuário volte a esta tela.
            Label lblQtd = new Label { Text = (isInteira ? DadosCinema.QtdInteira : DadosCinema.QtdMeia).ToString(), ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(480, 29), Size = new Size(30, 25), TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.FromArgb(18, 18, 24) };
            // Botão para aumentar a quantidade
            Button btnMais = new Button { Text = "+", Size = new Size(35, 30), Location = new Point(515, 25), BackColor = Color.FromArgb(63, 114, 252), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };

            btnMenos.FlatAppearance.BorderSize = 0;
            btnMais.FlatAppearance.BorderSize = 0;

            // Evento quando o botão '+' é clicado
            btnMais.Click += (s, e) => {
                // Limite: não permitir mais ingressos do que assentos selecionados em Form3
                int limite = DadosCinema.QtdAssentosSelecionados;
                int totalAntes = DadosCinema.TotalItens;
                if (totalAntes < limite)
                {
                    // Se for inteira, aumenta a contagem de inteira; senão aumenta meia
                    if (isInteira) DadosCinema.QtdInteira++; else DadosCinema.QtdMeia++;
                    // Atualiza o número exibido na interface
                    lblQtd.Text = isInteira ? DadosCinema.QtdInteira.ToString() : DadosCinema.QtdMeia.ToString();
                    // Atualiza o resumo lateral com os novos valores
                    AtualizarCarrinhoLateral();
                }
                else
                {
                    // Mostra mensagem se o usuário tentar exceder o limite de assentos
                    MessageBox.Show($"Você só pode selecionar até {limite} ingressos (igual ao número de assentos escolhidos).", "Limite atingido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Evento quando o botão '-' é clicado
            btnMenos.Click += (s, e) => {
                // Diminui a contagem, sem permitir abaixo de 0
                if (isInteira && DadosCinema.QtdInteira > 0) DadosCinema.QtdInteira--;
                if (!isInteira && DadosCinema.QtdMeia > 0) DadosCinema.QtdMeia--;
                // Atualiza o número exibido
                lblQtd.Text = isInteira ? DadosCinema.QtdInteira.ToString() : DadosCinema.QtdMeia.ToString();
                // Atualiza o resumo lateral
                AtualizarCarrinhoLateral();
            };

            // Adiciona todos os controles criados ao painel p
            p.Controls.AddRange(new Control[] { lblTipo, lblPreco, btnMenos, lblQtd, btnMais });
            // Retorna a label de quantidade para o chamador poder atualizar se necessário
            lblQuantidade = lblQtd;
            return p;
        }

        // Atualiza as labels do resumo lateral com valor e quantidade atuais
        private void AtualizarCarrinhoLateral()
        {
            // Mostra quantos itens estão no carrinho (inteira + meia)
            lblTotalItensLateral.Text = $"Itens: {DadosCinema.TotalItens}";
            // Mostra o valor total formatado com duas casas decimais
            lblTotalValorLateral.Text = $"Total: R$ {DadosCinema.TotalValor:N2}";
        }
    }
}