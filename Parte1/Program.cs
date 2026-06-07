using System;
using Parte1; // Referenciando o namespace do seu AFD.cs

namespace ExecutarAFD
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Instancia o AFD com os estados padrão definidos no construtor
            AFD meuAfd = new AFD();
            Console.WriteLine("=== EXIBINDO O AFD PADRÃO ===");
            meuAfd.ExibirAFD();

            // 2. Testa a validação de palavras lendo o arquivo de texto
            Console.WriteLine("\n=== LENDO PALAVRAS DO ARQUIVO TXT ===");
            meuAfd.CarregarPalavrasDeArquivo("entradasAFD.txt");

            // 3. Testa a função 'Desafio' para carregar um novo AFD via JSON
            Console.WriteLine("\n=== TESTANDO O DESAFIO (ARQUIVO JSON) ===");
            AFD afdDoJson = meuAfd.Desafio("afd.json");
            afdDoJson.ExibirAFD();
            afdDoJson.AceitarPalavra("1");
        }
    }
}