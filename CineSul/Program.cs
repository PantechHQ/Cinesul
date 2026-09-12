using System; // Importa tipos básicos e essenciais do C# (como o atributo STAThread)
using System.Collections.Generic; // Importa estruturas de coleções genéricas (listas, dicionários, etc.)
using System.Linq; // Importa recursos do LINQ para consulta e manipulação de dados
using System.Threading.Tasks; // Importa suporte para tarefas assíncronas e concorrência
using System.Windows.Forms; // Importa as classes principais de controle e execução da biblioteca Windows Forms

namespace CineSulApp // Organiza a classe dentro do namespace principal do aplicativo "CineSulApp"
{
    internal static class Program // Declaração da classe estática interna principal que orquestra a inicialização da aplicação
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread] // Define o modelo de threading da aplicação como Single-Threaded Apartment, necessário para componentes visuais do Windows
        static void Main() // Método estático principal que é executado no momento em que o aplicativo é iniciado
        {
            Application.EnableVisualStyles(); // Ativa os estilos visuais modernos e temas do Windows para os controles do formulário
            Application.SetCompatibleTextRenderingDefault(false); // Define o renderizador de texto padrão para GDI+ visando melhor desempenho e renderização visual
            // Abre o formulário inicial (se você quiser testar o login direto, pode trocar Form1 por FormLogin)
            Application.Run(new Form1()); // Inicializa e executa o loop de eventos da aplicação abrindo a janela principal (Form1)
        }
    }
}