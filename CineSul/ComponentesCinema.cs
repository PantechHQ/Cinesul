using System; // Importa tipos básicos do C# (como manipulação de eventos e exceções)
using System.Drawing; // Importa recursos visuais para definição de cores (Color), tamanhos (Size) e regiões
using System.Drawing.Drawing2D; // Importa ferramentas de desenho avançadas como vetores e caminhos gráficos (GraphicsPath)
using System.Windows.Forms; // Importa os controles nativos de interface do Windows Forms (como Button e Padding)

namespace CineSulApp // Organiza a classe dentro do namespace principal do aplicativo "CineSulApp"
{
    // Enumeração dos tipos de assentos
    public enum TipoAssento // Define uma lista de constantes que representam os estados e categorias de um assento
    {
        Indisponivel, // Assento já ocupado por outro cliente
        Disponivel, // Assento livre para seleção comum
        Cadeirante, // Assento reservado/adaptado para pessoas em cadeira de rodas
        Acompanhante, // Assento reservado para acompanhante de PCD
        Obeso, // Assento adaptado para pessoas obesas
        MobilidadeReduzida, // Assento adaptado para pessoas com mobilidade reduzida
        Selecionado // Assento que o usuário acabou de escolher
    }

    // Botão customizado para desenhar os assentos redondos no mapa
    public class BotaoAssento : Button // Declara a classe do botão personalizado herdando as funcionalidades do Button nativo
    {
        public string Fileira { get; set; } // Propriedade para armazenar a letra da fileira (ex: "A", "B", "C")
        public int Numero { get; set; } // Propriedade para armazenar o número da cadeira na fileira (ex: 1, 2, 3)
        public TipoAssento Tipo { get; private set; } // Propriedade que guarda o tipo atual do assento (leitura pública, alteração interna)

        public BotaoAssento(string fileira, int numero, TipoAssento tipoInicial) // Construtor: cria e inicializa o assento com suas informações
        {
            this.Fileira = fileira; // Define a letra da fileira recebida no parâmetro
            this.Numero = numero; // Define o número da cadeira recebido no parâmetro
            this.Size = new Size(18, 18); // Define o tamanho fixo do botão como 18x18 pixels (formato compacto)
            this.FlatStyle = FlatStyle.Flat; // Aplica o estilo visual plano sem os efeitos tridimensionais padrão do Windows
            this.FlatAppearance.BorderSize = 0; // Remove completamente as bordas externas do botão
            this.Margin = new Padding(2); // Aplica um espaçamento externo de 2 pixels em todos os lados para separar dos outros botões

            // Transforma o botão em um círculo perfeito
            using (GraphicsPath path = new GraphicsPath()) // Cria um gerenciador de caminho vetorial descartável
            {
                path.AddEllipse(0, 0, 18, 18); // Desenha uma elipse/círculo perfeita cobrindo toda a área do botão (18x18px)
                this.Region = new Region(path); // Recorta o formato físico do botão para que fique redondo
            }

            AtualizarTipo(tipoInicial); // Aplica as cores, ícones e estados correspondentes ao tipo inicial passado
        }

        public void AtualizarTipo(TipoAssento novoTipo) // Método responsável por alterar visualmente o estado e comportamento do assento
        {
            this.Tipo = novoTipo; // Atualiza a propriedade interna com o novo estado atribuído
            this.Text = ""; // Limpa qualquer texto/ícone anterior do botão
            this.Font = new Font("Segoe UI", 6, FontStyle.Bold); // Define a fonte para Segoe UI, tamanho 6 em negrito (para caber ícones pequenos)
            this.ForeColor = Color.White; // Ajusta a cor do texto/ícone para branco

            switch (novoTipo) // Avalia o tipo de assento para aplicar a estilização visual correspondente
            {
                case TipoAssento.Indisponivel: // Caso o assento esteja ocupado
                    this.BackColor = Color.FromArgb(60, 60, 65); // Aplica cor de fundo cinza-escuro
                    this.Text = "👤"; // Desenha o ícone de pessoa indicando que o lugar está ocupado
                    this.Enabled = false; // Desabilita interações para bloquear o clique no assento
                    break; // Finaliza o caso Indisponivel

                case TipoAssento.Disponivel: // Caso o assento esteja livre
                    this.BackColor = Color.FromArgb(0, 122, 255); // Aplica cor de fundo azul brilhante
                    this.Enabled = true; // Habilita o clique para permitir a seleção
                    break; // Finaliza o caso Disponivel

                case TipoAssento.Cadeirante: // Caso seja uma vaga reservada para cadeirantes
                    this.BackColor = Color.FromArgb(0, 122, 255); // Aplica cor de fundo azul brilhante
                    this.Text = "♿"; // Exibe o símbolo internacional de acessibilidade
                    this.Enabled = true; // Habilita o clique para seleção
                    break; // Finaliza o caso Cadeirante

                case TipoAssento.Acompanhante: // Caso seja lugar reservado para acompanhante
                    this.BackColor = Color.FromArgb(0, 60, 150); // Aplica cor de fundo azul-escuro
                    this.Text = "AC"; // Exibe a sigla "AC" de acompanhante no botão
                    this.Enabled = true; // Habilita o clique para seleção
                    break; // Finaliza o caso Acompanhante

                case TipoAssento.Selecionado: // Caso o assento tenha sido escolhido pelo usuário
                    this.BackColor = Color.FromArgb(180, 180, 180); // Aplica cor de fundo cinza-claro para indicar a escolha
                    this.Enabled = true; // Mantém o botão clicável (permite ao usuário desmarcar o assento)
                    break; // Finaliza o caso Selecionado
            }
        }
    }
}