using System; // Importa tipos essenciais do C# (como Action, Func e manipulação de objetos básicos)
using System.Collections.Generic; // Importa coleções genéricas (como a estrutura de Dicionário)
using System.Windows.Forms; // Importa suporte para formulários e controles da interface gráfica do Windows

namespace CineSulApp // Organiza a classe dentro do namespace principal do aplicativo "CineSulApp"
{
    // Classe estática global responsável por armazenar os dados compartilhados de todo o sistema em memória
    public static class DadosCinema
    {
        public static string FilmeSelecionado { get; set; } = "Meu malvado favorito 3"; // Guarda o nome do filme escolhido com um valor padrão inicial
        public static string HorarioSelecionado { get; set; } = "18h50"; // Guarda o horário da sessão selecionada pelo cliente
        public static string CinemaSelecionado { get; set; } = "Shopping Itaquera"; // Guarda o nome da unidade/cinema escolhida

        // Controle de valores do Carrinho
        public static int QtdInteira { get; set; } = 0; // Armazena a quantidade de ingressos do tipo Inteira selecionados
        public static int QtdMeia { get; set; } = 0; // Armazena a quantidade de ingressos do tipo Meia-entrada selecionados
        public static decimal TotalSnacks { get; set; } = 0.00m; // Armazena o valor financeiro acumulado das compras da bomboniere

        // Quantidade de assentos selecionados na tela de escolha de assentos (Form3)
        public static int QtdAssentosSelecionados { get; set; } = 0; // Guarda a contagem total de cadeiras marcadas pelo usuário na sala

        // Quantidades por snack (nome -> quantidade) para persistir seleção entre trocas de categoria
        public static Dictionary<string, int> SnackQuantities { get; } = new Dictionary<string, int>(); // Dicionário chave-valor que guarda o nome do produto e sua respectiva quantidade comprada

        // Preços por snack (nome -> preço unitário), preenchido quando os cards são criados no Form5.
        // Necessário para salvar o valor correto no banco e exibir no ticket final.
        public static Dictionary<string, decimal> SnackPrices { get; } = new Dictionary<string, decimal>();

        public static int TotalItens => QtdInteira + QtdMeia; // Propriedade calculada que soma e retorna o total geral de ingressos escolhidos
        public static decimal TotalValor => (QtdInteira * 30.00m) + (QtdMeia * 15.00m) + TotalSnacks; // Propriedade calculada que calcula o custo total (Inteira R$30, Meia R$15 + Snacks)

        // Autenticação simples
        public static bool IsLoggedIn { get; set; } = false; // Indica se o usuário está autenticado no aplicativo (true = conectado, false = visitante)

        public static Func<Form> TelaRetorno { get; set; } = null; // Guarda uma referência da função da tela para onde o usuário deve voltar após fazer o login
        public static string LoggedUserEmail { get; set; } = string.Empty; // Armazena o endereço de e-mail do usuário atualmente conectado

        // Recuperação de senha (duplicação removida)
        public static string RecoveryEmail { get; set; } = string.Empty; // Guarda temporariamente o e-mail informado no processo de redefinição de senha
        public static string RecoveryCode { get; set; } = string.Empty; // Guarda o código de validação de verificação gerado para a troca de senha

        // Reseta todo o estado de um pedido em andamento.
        // Deve ser chamado ao INICIAR uma nova compra (ex: Form1 ao escolher um filme),
        // e também é chamado no Form8 depois que a compra é concluída com sucesso.
        // Isso corrige o bug de valores "presos" quando o usuário abandona a compra no meio do fluxo.
        public static void ResetarPedido()
        {
            QtdInteira = 0;
            QtdMeia = 0;
            TotalSnacks = 0.00m;
            SnackQuantities.Clear();
            SnackPrices.Clear();
            QtdAssentosSelecionados = 0;
        }
    }
}