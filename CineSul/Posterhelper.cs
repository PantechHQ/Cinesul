// Classe utilitária responsável por carregar o pôster de um filme a partir da pasta "Posters",
// com fallback para um placeholder de texto quando a imagem não existe ou não pode ser lida.
// Centralizar essa lógica aqui evita duplicação entre Form1 e Form9 (e qualquer tela futura
// que precise exibir capas de filmes).
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CineSulApp
{
    public static class PosterHelper
    {
        // Carrega o pôster do filme "titulo" dentro do PictureBox "pic".
        // Convenção de nome de arquivo: espaços viram "_", extensão .jpg ou .png,
        // dentro da pasta "Posters" na raiz do executável.
        public static void CarregarPoster(PictureBox pic, string titulo)
        {
            try
            {
                string postersDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posters");
                string fileName = titulo.Replace(' ', '_');
                string pathJpg = Path.Combine(postersDir, fileName + ".jpg");
                string pathPng = Path.Combine(postersDir, fileName + ".png");

                string caminhoEncontrado = File.Exists(pathJpg) ? pathJpg
                                           : File.Exists(pathPng) ? pathPng
                                           : null;

                if (caminhoEncontrado != null)
                {
                    // Libera a imagem anterior do PictureBox, se houver, para não vazar memória
                    // (relevante quando o card é recriado, ex: troca de categoria no Form9)
                    pic.Image?.Dispose();

                    // Abre o arquivo, decodifica e copia para um novo Bitmap em memória.
                    // Isso é necessário porque Image.FromStream mantém uma referência interna
                    // ao stream para decodificação preguiçosa; se o stream for fechado (pelo using)
                    // antes de copiarmos os dados, a imagem pode corromper ou lançar exceção depois.
                    using (var fs = File.OpenRead(caminhoEncontrado))
                    using (var imgOriginal = Image.FromStream(fs))
                    {
                        pic.Image = new Bitmap(imgOriginal);
                    }
                }
                else
                {
                    MostrarPlaceholder(pic, titulo);
                }
            }
            catch
            {
                // Qualquer erro de leitura/decodificação também cai no placeholder,
                // para nunca deixar o card quebrado visualmente.
                MostrarPlaceholder(pic, titulo);
            }
        }

        // Mostra um texto substituto dentro do PictureBox quando não há imagem disponível.
        // Limpa controles anteriores primeiro para não empilhar labels caso o método
        // seja chamado mais de uma vez no mesmo PictureBox.
        private static void MostrarPlaceholder(PictureBox pic, string titulo)
        {
            pic.Image = null;
            pic.Controls.Clear();

            var placeholder = new Label
            {
                Text = $"[ Capa:\n{titulo} ]",
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(40, 40, 50)
            };
            pic.Controls.Add(placeholder);
        }
    }
}