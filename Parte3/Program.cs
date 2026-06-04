using System;
using Parte3; // Referenciando o namespace do seu TURING.cs

namespace ExecutarTURING
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Instancia o TURING com os estados padrão definidos no construtor
            TURING meuTuring = new TURING();
            Console.WriteLine("=== EXIBINDO O TURING PADRÃO ===");
            Console.WriteLine("Digite uma palavra para simular a máquina de Turing (ex: 'aaabbbccc'):");
            string entrada = Console.ReadLine();
            meuTuring.Simulacao(entrada);
        }
    }
}