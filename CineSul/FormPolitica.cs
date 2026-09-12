using System; // Importa tipos básicos do C# como eventos, tipos de dados essenciais e manipulação de objetos
using System.Drawing; // Importa recursos visuais como cores, coordenadas (Point) e tamanhos (Size)
using System.Windows.Forms; // Importa a biblioteca de componentes visuais para janelas do Windows (Form, Panel, Label)

namespace CineSulApp // Organiza a classe dentro do namespace principal do aplicativo "CineSulApp"
{
    // Declaração da classe da janela de Política de Privacidade, herdando da classe base Form
    public partial class FormPolitica : Form
    {
        // Construtor principal da classe: executado quando o formulário é instanciado no sistema
        public FormPolitica()
        {
            ConfigurarTela(); // Chama o método responsável por ajustar as propriedades básicas da janela
            ConstruirInterface(); // Chama o método responsável por desenhar e posicionar os elementos na tela
            {
                this.FormBorderStyle = FormBorderStyle.None; // Remove as bordas padrão, barra de título e botões de fechar/minimizar
                this.WindowState = FormWindowState.Maximized; // Redimensiona a janela para ocupar 100% da tela do monitor

            }
        }

        // Método privado para definir parâmetros estruturais e de aparência da janela
        private void ConfigurarTela()
        {
            this.Text = "Política de Privacidade - Cíne Sul"; // Define o título oficial identificador da janela no Windows
            this.Size = new Size(1024, 900); // Configura o tamanho padrão do formulário para 1024x900 pixels
            this.BackColor = Color.FromArgb(13, 16, 26); // Define a cor de fundo com o tom escuro padrão do aplicativo
            this.StartPosition = FormStartPosition.CenterScreen; // Centraliza a posição da janela na tela do usuário
        }

        // Método privado para criar, configurar e posicionar os componentes visuais
        private void ConstruirInterface()
        {
            // Cabeçalho
            this.Controls.Add(NavigationHelper.CreateHeader(this)); // Adiciona a barra de cabeçalho padrão no topo do formulário

            // Painel Principal com Rolagem
            Panel pnlConteudo = new Panel // Instancia o container que vai abrigar o texto da política
            {
                Size = new Size(920, 750), // Define a dimensão visível do painel em 920px de largura por 750px de altura
                Location = new Point(45, 80), // Posiciona o painel a 45px da esquerda e 80px do topo da janela
                AutoScroll = true, // Habilita a barra de rolagem automática caso o texto exceda a altura do painel
                BackColor = Color.Transparent // Mantém o fundo transparente para exibir a cor original da janela
            };

            // Botão / Link Voltar
            Label lblVoltar = new Label // Instancia o rótulo que funciona como botão para fechar a janela
            {
                Text = "‹ Voltar", // Texto exibindo o símbolo de seta e a instrução Voltar
                ForeColor = Color.FromArgb(63, 114, 252), // Define a cor do texto com o tom azul padrão de destaques da interface
                Font = new Font("Segoe UI", 12, FontStyle.Bold), // Configura a fonte para Segoe UI, tamanho 12, em negrito
                Location = new Point(0, 10), // Posiciona o elemento no canto superior esquerdo do painel
                AutoSize = true, // Redimensiona a largura do rótulo automaticamente ao tamanho do texto
                Cursor = Cursors.Hand // Altera o ponteiro do mouse para o ícone de mãozinha clicável
            };
            lblVoltar.Click += (s, e) => this.Close(); // Associa o evento de clique ao método que encerra e fecha a tela atual

            // Título
            Label lblTitulo = new Label // Instancia o rótulo do título principal da página
            {
                Text = "Política de privacidades", // Define o texto do título exibido na tela
                ForeColor = Color.White, // Define a cor do texto para branco
                Font = new Font("Segoe UI", 22, FontStyle.Bold), // Configura a fonte para tamanho 22 em negrito
                Location = new Point(0, 45), // Posiciona o título logo abaixo do botão de voltar
                AutoSize = true // Ajusta a largura da caixa de texto automaticamente
            };

            // Texto da Política de Privacidade
            Label lblTexto = new Label // Instancia o bloco de texto responsável por exibir o corpo da política de privacidade
            {
                Text = @"1. POLÍTICA DE PRIVACIDADE DE DADOS 
Para usar alguns recursos da plataforma, é necessário criar uma conta com dados verdadeiros. As informações pessoais inseridas voluntariamente em nosso site na Internet estão sujeitas às normas de confidencialidade e privacidade, incluindo informações referentes à criação do perfil pessoal. Para essa condição, a informação solicitada é diferenciada e armazenada em bases de dados separadas que declaram o total sigilo de seus dados. Nesse sentido, a equipe de colaboradores da CineSul tem envidado seus melhores esforços para oferecer a tecnologia mais moderna e atualizada a fim de oferecer a maior segurança possível.  Seus dados são usados apenas para o funcionamento da plataforma e não são vendidos a terceiros. 
2. DESCONTO SOCIAL 
Os descontos são destinados a pessoas de baixa renda, mediante comprovação quando solicitado. O uso indevido do benefício pode levar ao cancelamento da conta e retorno do ingresso pago de forma prévia. A comprovação pode incluir documentos ou cadastro em programas sociais reconhecidos, e o CineSul se reserva o direito de verificar essas informações a qualquer momento. O uso indevido do benefício, como declarações falsas ou tentativa de burlar os critérios, pode levar ao cancelamento da conta e à perda do acesso aos descontos. 
3. ALUGUEL DE FILMES 
O aluguel online dá acesso temporário ao conteúdo, pelo prazo informado na página do filme. Não é permitido copiar, redistribuir ou compartilhar o acesso com terceiros. 
4. PAGAMENTOS E CANCELAMENTOS 
Os valores e formas de pagamento disponíveis são exibidos antes da confirmação da compra, incluindo eventuais taxas aplicáveis. Cancelamentos seguem as regras informadas no momento da compra, que podem variar conforme o tipo de produto (ingresso ou aluguel de filme) e o tempo decorrido desde a transação. Reembolsos, quando aplicáveis, são processados pelo mesmo meio de pagamento utilizado na compra, dentro do prazo informado no ato do cancelamento. 
5. DIREITOS AUTORAIS 
Todo o conteúdo disponibilizado (filmes, imagens, textos) pertence aos seus respectivos criadores ou distribuidores. O uso da plataforma não transfere nenhum direito autoral ao usuário. 
6. CONTATO 
Dúvidas, sugestões ou reclamações sobre estes termos podem ser enviadas para a equipe do CineSul pelos canais informados no site, como e-mail ou formulário de contato. Faremos o possível para responder dentro de um prazo razoável e ajudar a resolver qualquer problema relacionado ao uso da plataforma. ", // Texto longo da política de privacidade em múltiplas linhas

                ForeColor = Color.FromArgb(220, 225, 235), // Define a cor do texto com um tom cinza-claro para facilitar a leitura no fundo escuro
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular), // Define a fonte para Segoe UI, tamanho 10.5 regular
                Location = new Point(0, 100), // Posiciona o bloco de texto a 100px do topo do painel
                Width = 880, // Define a largura fixa de 880px
                AutoSize = true, // Permite que a altura do rótulo expanda verticalmente conforme o tamanho do texto
                MaximumSize = new Size(880, 0) // Define a largura máxima de quebra de linha em 880px e altura sem limite
            };

            pnlConteudo.Controls.Add(lblVoltar); // Adiciona o elemento "Voltar" dentro do painel de conteúdo
            pnlConteudo.Controls.Add(lblTitulo); // Adiciona o título dentro do painel de conteúdo
            pnlConteudo.Controls.Add(lblTexto); // Adiciona o texto descritivo dentro do painel de conteúdo

            this.Controls.Add(pnlConteudo); // Adiciona o painel de conteúdo estruturado dentro do formulário principal
        }
    }
}