using System;
using Parte3;

namespace ExecutarTURING
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== TAREFA 3: MÁQUINA DE TURING ===");

            // 1. Testa a Máquina de Turing padrão L4 com os casos exigidos
            TURING mtL4 = TURING.CriarMT_L4();
            
            Console.WriteLine("\n=== LENDO CASOS DE TESTE OBRIGATÓRIOS PARA L4 ===");
            // O espaço vazio na matriz representa Epsilon (entrada vazia)
            string[] casosL4 = { "abc", "aabbcc", "aaabbbccc", "aabbc", "ab", "abc abc", "" };
            
            foreach (var caso in casosL4)
            {
                mtL4.Simulacao(caso);
            }

            // 2. Testa o Desafio f(n) = n + 1
            Console.WriteLine("\n\n=== LENDO CASOS DE TESTE: DESAFIO f(n) = n + 1 (UNÁRIO) ===");
            TURING mtUnario = TURING.CriarMT_Unario();
            
            string[] casosUnario = { "1", "111", "11111" };

            foreach (var caso in casosUnario)
            {
                mtUnario.Simulacao(caso);
            }

            // 3. (Opcional) Teste interativo que já estava no seu código
            Console.WriteLine("\n\n=== MODO INTERATIVO (L4) ===");
            Console.WriteLine("Digite uma palavra para simular a L4 (ou apenas aperte ENTER para encerrar):");
            string entrada = Console.ReadLine() ?? "";
            
            if (!string.IsNullOrEmpty(entrada))
            {
                mtL4.Simulacao(entrada);
            }
        }
    }
}