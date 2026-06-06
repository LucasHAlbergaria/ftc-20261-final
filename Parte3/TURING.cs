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
        string estadoInicial;

        public TURING(Dictionary<(string, char), (string, char, char)> transicoes, string estadoInicial = "q0")
        {
            this.fita = new Dictionary<int, char>();
            this.transicoes = transicoes;
            this.estadoInicial = estadoInicial;
        }

        public bool Simulacao(string entrada, int maxPassos = 10000)
        {
            Console.WriteLine($"\n=================================================");
            Console.WriteLine($"Entrada: '{(string.IsNullOrEmpty(entrada) ? "ε (vazia)" : entrada)}'");

            fita.Clear();
            if (!string.IsNullOrEmpty(entrada))
            {
                for (int i = 0; i < entrada.Length; i++)
                {
                    fita[i] = entrada[i];
                }
            }
            
            cabecote = 0; 
            estadoAtual = estadoInicial;
            int passo = 0; 

            while (true)
            {
                char simboloAtual = fita.ContainsKey(cabecote) ? fita[cabecote] : '_';

                Console.WriteLine($"[Passo: {passo}] | Estado: {estadoAtual} | Cabeçote: {cabecote}");

                int minIndex = fita.Count > 0 ? Math.Min(fita.Keys.Min(), cabecote) : cabecote;
                int maxIndex = fita.Count > 0 ? Math.Max(fita.Keys.Max(), cabecote) : cabecote;

                var sb = new StringBuilder();
                for (int i = minIndex; i <= maxIndex; i++)
                {
                    char s = fita.ContainsKey(i) ? fita[i] : '_';
                    if (i == cabecote) sb.Append($"[{s}]");
                    else sb.Append(s);
                }
                
                Console.WriteLine($"Fita: {sb}");

                if (estadoAtual == "qaccept")
                {
                    Console.WriteLine("-> Resultado: ACEITA");
                    return true;
                }
                if (estadoAtual == "qreject")
                {
                    Console.WriteLine("-> Resultado: REJEITA");
                    return false;
                }

                var chave = (estadoAtual, simboloAtual);
                
                if (!transicoes.TryGetValue(chave, out var trans))
                {
                    Console.WriteLine($"Sem transição definida para δ({estadoAtual}, '{simboloAtual}') — forçando rejeição.");
                    estadoAtual = "qreject";
                    continue; 
                }

                var (novoEstado, escrever, direcao) = trans;

                if (escrever == '_')
                {
                    if (fita.ContainsKey(cabecote)) fita.Remove(cabecote);
                }
                else
                {
                    fita[cabecote] = escrever;
                }

                if (direcao == 'D') cabecote++;
                else if (direcao == 'E') cabecote--;

                estadoAtual = novoEstado;

                passo++;
                if (passo > maxPassos)
                {
                    Console.WriteLine("Limite de passos alcançado — abortando (loop infinito).");
                    return false;
                }
            }
        }

        public static TURING CriarMT_L4()
        {
            var trans = new Dictionary<(string, char), (string, char, char)>();

          
            trans.Add(("q0", 'a'), ("q1", 'X', 'D')); 
            trans.Add(("q0", 'X'), ("q0", 'X', 'D')); 
            trans.Add(("q0", 'Y'), ("q4", 'Y', 'D')); 

            trans.Add(("q0", 'b'), ("qreject", 'b', 'D'));
            trans.Add(("q0", 'c'), ("qreject", 'c', 'D'));
            trans.Add(("q0", '_'), ("qreject", '_', 'D')); 

            trans.Add(("q1", 'a'), ("q1", 'a', 'D')); 
            trans.Add(("q1", 'Y'), ("q1", 'Y', 'D')); 
            trans.Add(("q1", 'b'), ("q2", 'Y', 'D')); 
            trans.Add(("q1", 'c'), ("qreject", 'c', 'D')); 
            trans.Add(("q1", 'Z'), ("qreject", 'Z', 'D'));
            trans.Add(("q1", '_'), ("qreject", '_', 'D')); 

            trans.Add(("q2", 'b'), ("q2", 'b', 'D')); 
            trans.Add(("q2", 'Z'), ("q2", 'Z', 'D')); 
            trans.Add(("q2", 'c'), ("q3", 'Z', 'E'));  
            trans.Add(("q2", 'a'), ("qreject", 'a', 'D'));
            trans.Add(("q2", 'Y'), ("qreject", 'Y', 'D'));
            trans.Add(("q2", '_'), ("qreject", '_', 'D')); 

            trans.Add(("q3", 'a'), ("q3", 'a', 'E'));
            trans.Add(("q3", 'b'), ("q3", 'b', 'E'));
            trans.Add(("q3", 'Y'), ("q3", 'Y', 'E'));
            trans.Add(("q3", 'Z'), ("q3", 'Z', 'E'));
            trans.Add(("q3", 'X'), ("q0", 'X', 'D')); 
            trans.Add(("q3", '_'), ("qreject", '_', 'D')); 

            trans.Add(("q4", 'Y'), ("q4", 'Y', 'D')); 
            trans.Add(("q4", 'Z'), ("q4", 'Z', 'D'));
            trans.Add(("q4", '_'), ("qaccept", '_', 'D')); 

            trans.Add(("q4", 'a'), ("qreject", 'a', 'D'));
            trans.Add(("q4", 'b'), ("qreject", 'b', 'D'));
            trans.Add(("q4", 'c'), ("qreject", 'c', 'D'));
            trans.Add(("q4", 'X'), ("qreject", 'X', 'D'));

            return new TURING(trans);
        }

        public static TURING CriarMT_Unario()
        {
            var trans = new Dictionary<(string, char), (string, char, char)>();


            trans.Add(("q0", '1'), ("q0", '1', 'D')); 
            
            trans.Add(("q0", '_'), ("qaccept", '1', 'D'));

            return new TURING(trans);
        }
    }
}