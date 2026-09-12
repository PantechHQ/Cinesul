// Esta janela exibe o mapa de assentos e permite que o usuário selecione seus lugares
using System; // tipos básicos e eventos
using System.Drawing; // manipulação de cores e posições
using System.Drawing.Drawing2D; // desenho avançado (usado para formas, se necessário)
using System.Windows.Forms; // controles visuais

namespace CineSulApp
{
    // Classe que representa a tela de seleção de assentos
    public partial class Form3 : Form
    {
        // Painel que conterá os assentos desenhados
        private Panel painelSala;
        // Labels que mostram total de itens e valor (na lateral)
        private Label lblTotalItens;
        private Label lblTotalValor;
        // Constantes para espaçamento e deslocamento ao desenhar os assentos
        private const int ESPACAMENTO = 22;
        private const int OFFSET_X = 40;

        // Contador local de assentos selecionados nesta tela
        private int assentosSelecionadosNestaTela = 0;

        // Flag que indica se o usuário realmente avançou para a próxima etapa da compra
        // (clicou em "Comprar ingressos" com assentos válidos). Se ele sair da tela
        // de qualquer outra forma (voltar, fechar, trocar de sessão) sem essa flag
        // estar true, entendemos que a compra foi abandonada e o contador de
        // assentos deve ser zerado, para não "vazar" o limite para outra sessão.
        private bool avancouParaProximaEtapa = false;

        // Construtor: inicializa o form
        public Form3()
        {
            InitializeComponent();
            {
                // Remove bordas e abre em tela cheia para maior experiência visual
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

            }
            ConfigurarTela();
            ConstruirLayout();
            GerarMapaAssentos(); // desenha os botões de assentos

            // Garante que, independentemente de como a janela seja fechada
            // (X, Alt+F4, código chamando Close()), o contador seja resetado
            // caso o usuário não tenha avançado para a compra.
            this.FormClosing += Form3_FormClosing;
        }

        // Define título, tamanho e cor da janela
        private void ConfigurarTela()
        {
            this.Text = "Escolha seus assentos - Cíne Sul";
            this.Size = new Size(1100, 900);
            this.BackColor = Color.FromArgb(10, 15, 28);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Monta a estrutura visual da tela (sidebar, área do mapa, botões)
        private void ConstruirLayout()
        {
            // Adiciona o cabeçalho comum
            this.Controls.Add(NavigationHelper.CreateHeader(this));

            // Painel lateral com informações do carrinho
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = Color.FromArgb(8, 11, 20), Padding = new Padding(20) };
            this.Controls.Add(sidebar);

            // Label com o nome do filme
            Label lblFilme = new Label { Text = DadosCinema.FilmeSelecionado, ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(20, 30), Size = new Size(200, 50) };
            // Label com cinema e horário
            Label lblSessao = new Label { Text = $"{DadosCinema.CinemaSelecionado}\n\nSessão: {DadosCinema.HorarioSelecionado}", ForeColor = Color.Silver, Font = new Font("Segoe UI", 9), Location = new Point(20, 90), Size = new Size(200, 80) };

            // Caixa que mostra o total de assentos e o valor
            Panel boxValores = new Panel { Size = new Size(200, 120), Location = new Point(20, 190), BackColor = Color.FromArgb(24, 28, 40) };
            lblTotalItens = new Label { Text = "Assentos escolhidos: 0", ForeColor = Color.White, Font = new Font("Segoe UI", 9), Location = new Point(15, 20), AutoSize = true };
            lblTotalValor = new Label { Text = "A definir", ForeColor = Color.Yellow, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(15, 65), AutoSize = true };
            boxValores.Controls.AddRange(new Control[] { lblTotalItens, lblTotalValor });

            // Botão para voltar e escolher outro horário
            Button btnMudarSessao = new Button { Text = "Mudar horário", Size = new Size(200, 35), Location = new Point(20, 330), FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnMudarSessao.FlatAppearance.BorderColor = Color.FromArgb(13, 71, 161);
            btnMudarSessao.Click += (s, e) => {
                // O usuário está desistindo desta seleção de assentos para trocar
                // de horário/sessão: reseta o contador de assentos antes de sair,
                // já que a compra não foi finalizada.
                ResetarSelecaoDeAssentos();

                Form2 horarios = new Form2();
                horarios.Show();
                this.Hide();
            };

            sidebar.Controls.AddRange(new Control[] { lblFilme, lblSessao, boxValores, btnMudarSessao });

            // Área central que terá o mapa de assentos e botões de ação
            Panel mainArea = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 15, 28), AutoScroll = true };
            this.Controls.Add(mainArea);
            mainArea.BringToFront();

            Label lblTitle = new Label { Text = "Escolha seus assentos", ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), Location = new Point(30, 20), AutoSize = true };
            mainArea.Controls.Add(lblTitle);

            // Painel preto que representa a sala do cinema (onde os assentos serão desenhados)
            painelSala = new Panel { Size = new Size(780, 420), Location = new Point(30, 60), BackColor = Color.Black };
            mainArea.Controls.Add(painelSala);

            // Uma pequena barra que representa a tela do cinema (apenas elemento visual)
            Panel painelTela = new Panel { Size = new Size(680, 20), Location = new Point(50, 385), BackColor = Color.FromArgb(80, 80, 85) };
            Label lblTelaText = new Label { Text = "T E L A", ForeColor = Color.DarkGray, Font = new Font("Segoe UI", 8, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill };
            painelTela.Controls.Add(lblTelaText);
            painelSala.Controls.Add(painelTela);

            // Botão que inicia o próximo passo (definir tipo de ingresso)
            Button btnComprar = new Button { Text = "Comprar ingressos", Size = new Size(260, 45), Location = new Point(290, 510), BackColor = Color.FromArgb(13, 71, 161), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            btnComprar.FlatAppearance.BorderSize = 0;
            btnComprar.Click += (s, e) => {
                if (assentosSelecionadosNestaTela > 0)
                {
                    // O usuário realmente avançou com assentos válidos:
                    // marca a flag para que o reset NÃO aconteça ao sair desta tela.
                    avancouParaProximaEtapa = true;

                    // Se há assentos selecionados, avança para a tela de escolha de ingressos
                    Form4 telaIngressos = new Form4();
                    telaIngressos.Show();
                    this.Hide();
                }
                else
                {
                    // Caso contrário, pede ao usuário que selecione pelo menos um assento
                    MessageBox.Show("Por favor, selecione pelo menos um assento antes de prosseguir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            mainArea.Controls.Add(btnComprar);
        }

        // Desenha os botões de assento em posições calculadas
        private void GerarMapaAssentos()
        {
            // Linhas de assentos (A, B, C, ...)
            char[] fileiras = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N' };
            int totalColunas = 28;

            for (int f = 0; f < fileiras.Length; f++)
            {
                char letraFileira = fileiras[f];
                int posY = 345 - (f * ESPACAMENTO);

                // Ajuste de espaço em algumas fileiras
                if (letraFileira != 'A' && letraFileira != 'B') posY -= 20;

                // Label que mostra a letra da fileira na lateral
                Label lblLetra = new Label { Text = letraFileira.ToString(), ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold), Location = new Point(15, posY + 2), Size = new Size(20, 20), TextAlign = ContentAlignment.MiddleCenter };
                painelSala.Controls.Add(lblLetra);

                for (int col = 1; col <= totalColunas; col++)
                {
                    // Pula posições que representam corredores
                    if ((col == 4 || col == 5 || col == 24 || col == 25) && (letraFileira != 'A' && letraFileira != 'B' && letraFileira != 'N'))
                        continue;

                    // Decide um tipo inicial para o assento (disponível, cadeira de rodas, acompanhante, ou indisponível)
                    TipoAssento tipo = TipoAssento.Disponivel;

                    if ((col + f) % 8 == 0) tipo = TipoAssento.Indisponivel;
                    else if (letraFileira == 'C' && (col == 6 || col == 10 || col == 14 || col == 20)) tipo = TipoAssento.Cadeirante;
                    else if (letraFileira == 'B' && (col == 3 || col == 8 || col == 21)) tipo = TipoAssento.Acompanhante;

                    int posX = OFFSET_X + (col * ESPACAMENTO);

                    // Cria um botão customizado para representar o assento
                    BotaoAssento assento = new BotaoAssento(letraFileira.ToString(), col, tipo)
                    {
                        Location = new Point(posX, posY)
                    };

                    // Quando o botão for clicado, chama Assento_Click
                    assento.Click += Assento_Click;
                    painelSala.Controls.Add(assento);
                }
            }
        }

        // Manipulador de clique em um assento
        private void Assento_Click(object sender, EventArgs e)
        {
            BotaoAssento assento = sender as BotaoAssento;

            // Se o assento estiver disponível (ou especial), marca como selecionado
            if (assento.Tipo == TipoAssento.Disponivel || assento.Tipo == TipoAssento.Cadeirante || assento.Tipo == TipoAssento.Acompanhante)
            {
                // Guarda o tipo original no Tag para poder reverter depois
                assento.Tag = assento.Tipo;
                // Atualiza visualmente para estado selecionado
                assento.AtualizarTipo(TipoAssento.Selecionado);
                assentosSelecionadosNestaTela++;
                DadosCinema.QtdAssentosSelecionados = assentosSelecionadosNestaTela; // propaga para dados globais
            }
            else if (assento.Tipo == TipoAssento.Selecionado)
            {
                // Se já estava selecionado, reverte ao tipo anterior
                TipoAssento tipoOriginal = assento.Tag != null ? (TipoAssento)assento.Tag : TipoAssento.Disponivel;
                assento.AtualizarTipo(tipoOriginal);
                assentosSelecionadosNestaTela--;
                DadosCinema.QtdAssentosSelecionados = assentosSelecionadosNestaTela;
            }

            // Atualiza as informações na lateral para mostrar quantos assentos foram escolhidos
            lblTotalItens.Text = $"Assentos escolhidos: {assentosSelecionadosNestaTela}";
            lblTotalValor.Text = "Definir no próximo passo"; // valor será calculado depois
        }

        // Centraliza a lógica de reset: zera o contador local e o global (DadosCinema),
        // garantindo que a próxima vez que o usuário abrir a tela de assentos
        // (nesta ou em outra sessão) ele comece do zero.
        private void ResetarSelecaoDeAssentos()
        {
            assentosSelecionadosNestaTela = 0;
            DadosCinema.QtdAssentosSelecionados = 0;
        }

        // Método público estático: permite que qualquer outra parte do app (como o
        // NavigationHelper) avise o Form3 que o usuário está saindo da tela sem
        // finalizar a compra, para que a seleção de assentos seja descartada.
        // Chamado automaticamente pelos botões do header (Home, Filmes, Favoritos)
        // quando a tela atual é um Form3 com uma compra não finalizada.
        public static void ResetarSeAbandonado(Form telaAtual)
        {
            if (telaAtual is Form3 form3 && !form3.avancouParaProximaEtapa)
            {
                form3.ResetarSelecaoDeAssentos();
            }
        }

        // Disparado quando a janela está prestes a fechar (X, Close(), Alt+F4 etc).
        // Se o usuário não avançou para a próxima etapa da compra, consideramos
        // a seleção abandonada e resetamos o contador global de assentos.
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!avancouParaProximaEtapa)
            {
                ResetarSelecaoDeAssentos();
            }
        }
    }
}