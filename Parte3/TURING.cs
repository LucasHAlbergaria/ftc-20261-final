using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Parte3
{
    internal class TURING
    {
        Dictionary<int, char> fita;
        Dictionary<(string, char), (string, char, char)> transicoes;
        int cabecote;
        string estadoAtual;

        public TURING()
        {
            fita = new Dictionary<int, char>();
            transicoes = new Dictionary<(string, char), (string, char, char)>();
            cabecote = 0;
            estadoAtual = "q0";

            // ========================== Q0 ============================
            transicoes.Add(
                ("q0", 'a'),
                ("q1", 'X', 'D')
            );

            transicoes.Add(
                ("q0", 'X'),
                ("q0", 'X', 'D')
            );

            transicoes.Add(
                ("q0", 'Y'),
                ("q4", 'Y', 'D')
            );

            transicoes.Add(
                ("q0", 'Z'),
                ("q4", 'Z', 'D')
            );

            transicoes.Add(
                ("q0", 'b'),
                ("qrej", 'b', 'D')
            );

            transicoes.Add(
                ("q0", 'c'),
                ("qrej", 'c', 'D')
            );

            transicoes.Add(
                ("q0", '_'),
                ("qrej", '_', 'D')
            );

            // ========================== Q1 ============================
            transicoes.Add(
                ("q1", 'a'),
                ("q1", 'a', 'D')
            );

            transicoes.Add(
                ("q1", 'Y'),
                ("q1", 'Y', 'D')
            );

            transicoes.Add(
                ("q1", 'b'),
                ("q2", 'Y', 'D')
            );

            transicoes.Add(
                ("q1", 'c'),
                ("qrej", 'c', 'D')
            );

            transicoes.Add(
                ("q1", 'Z'),
                ("qrej", 'Z', 'D')
            );

            transicoes.Add(
                ("q1", '_'),
                ("qrej", '_', 'D')
            );

            // ========================== Q2 ============================
            transicoes.Add(
                ("q2", 'b'),
                ("q2", 'b', 'D')
            );

            transicoes.Add(
                ("q2", 'Z'),
                ("q2", 'Z', 'D')
            );

            transicoes.Add(
                ("q2", 'c'),
                ("q3", 'Z', 'E')
            );

            transicoes.Add(
                ("q2", 'a'),
                ("qrej", 'a', 'D')
            );

            transicoes.Add(
                ("q2", 'Y'),
                ("qrej", 'Y', 'D')
            );

            transicoes.Add(
                ("q2", '_'),
                ("qrej", '_', 'D')
            );

            // ========================== Q3 ============================
            transicoes.Add(
                ("q3", 'a'),
                ("q3", 'a', 'E')
            );

            transicoes.Add(
                ("q3", 'b'),
                ("q3", 'b', 'E')
            );

            transicoes.Add(
                ("q3", 'Y'),
                ("q3", 'Y', 'E')
            );

            transicoes.Add(
                ("q3", 'Z'),
                ("q3", 'Z', 'E')
            );

            transicoes.Add(
                ("q3", 'X'),
                ("q0", 'X', 'D')
            );

            transicoes.Add(
                ("q3", '_'),
                ("qrej", '_', 'D')
            );

            // ========================== Q4 ============================
            transicoes.Add(
                ("q4", 'Y'),
                ("q4", 'Y', 'D')
            );

            transicoes.Add(
                ("q4", 'Z'),
                ("q4", 'Z', 'D')
            );

            transicoes.Add(
                ("q4", '_'),
                ("qacc", '_', 'D')
            );

            transicoes.Add(
                ("q4", 'a'),
                ("qrej", 'a', 'D')
            );

            transicoes.Add(
                ("q4", 'b'),
                ("qrej", 'b', 'D')
            );

            transicoes.Add(
                ("q4", 'c'),
                ("qrej", 'c', 'D')
            );

            transicoes.Add(
                ("q4", 'X'),
                ("qrej", 'X', 'D')
            );
        }

        public void Simulacao(string entrada)
        {
            Console.WriteLine($"Entrada: {entrada}");

            // Inicializa fita a partir da entrada
            fita.Clear();
            for (int i = 0; i < entrada.Length; i++)
            {
                fita[i] = entrada[i];
            }
            cabecote = 0;
            estadoAtual = "q0";

            int passo = 0;
            const int maxPassos = 10000;

            while (true)
            {
                char simboloAtual = fita.ContainsKey(cabecote) ? fita[cabecote] : '_';

                // Exibe estado atual
                Console.WriteLine($"Estado: {estadoAtual}");

                // Calcula intervalo da fita a ser exibido
                int minIndex = fita.Count > 0 ? Math.Min(fita.Keys.Min(), cabecote) : cabecote;
                int maxIndex = fita.Count > 0 ? Math.Max(fita.Keys.Max(), cabecote) : cabecote;

                var sb = new StringBuilder();
                for (int i = minIndex; i <= maxIndex; i++)
                {
                    char s = fita.ContainsKey(i) ? fita[i] : '_';
                    if (i == cabecote) sb.Append($"[{s}]");
                    else sb.Append(s);
                }

                // Exibe conteúdo da fita e posição do cabeçote
                Console.WriteLine($"Fita: {sb}");
                Console.WriteLine($"Cabeçote: {cabecote}");

                if (estadoAtual == "qacc" || estadoAtual == "qrej") break;

                var chave = (estadoAtual, simboloAtual);
                if (!transicoes.TryGetValue(chave, out var trans))
                {
                    Console.WriteLine("Sem transição definida — rejeitando.");
                    estadoAtual = "qrej";
                    break;
                }

                var (novoEstado, escrever, direcao) = trans;

                // Escreve símbolo na fita
                if (escrever == '_')
                {
                    if (fita.ContainsKey(cabecote)) fita.Remove(cabecote);
                }
                else
                {
                    fita[cabecote] = escrever;
                }

                // Move cabeçote
                if (direcao == 'D') cabecote++;
                else if (direcao == 'E') cabecote--;

                // Atualiza estado
                estadoAtual = novoEstado;

                passo++;
                if (passo > maxPassos)
                {
                    Console.WriteLine("Limite de passos alcançado — abortando.");
                    break;
                }
            }

            Console.WriteLine($"Resultado: {estadoAtual}");
        }
    }
}
