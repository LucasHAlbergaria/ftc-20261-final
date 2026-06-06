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
        // Usando Dictionary para a fita porque ela é infinita. A chave é o índice (pode ser negativo) e o valor é o caractere.
        Dictionary<int, char> fita;
        
        // O valor retornado: (novoEstado, simboloEscrito, direcaoDoCabecote)
        Dictionary<(string, char), (string, char, char)> transicoes;
        
        int cabecote; // Posição atual da leitura/escrita na fita
        string estadoAtual;
        string estadoInicial;

        // Construtor que recebe a tabela de transições já pronta (facilita pra criar máquinas diferentes)
        public TURING(Dictionary<(string, char), (string, char, char)> transicoes, string estadoInicial = "q0")
        {
            this.fita = new Dictionary<int, char>();
            this.transicoes = transicoes;
            this.estadoInicial = estadoInicial;
        }

        public bool Simulacao(string entrada, int maxPassos = 10000)
        {
            Console.WriteLine($"\n=================================================");
            // Se a entrada vier vazia, trata como palavra vazia (Epsilon)
            Console.WriteLine($"Entrada: '{(string.IsNullOrEmpty(entrada) ? "ε (vazia)" : entrada)}'");

            // Limpa a fita antes de começar pra não dar problema se rodar mais de uma vez seguida
            fita.Clear();
            if (!string.IsNullOrEmpty(entrada))
            {
                // Preenche a fita do índice 0 em diante com a palavra digitada
                for (int i = 0; i < entrada.Length; i++)
                {
                    fita[i] = entrada[i];
                }
            }
            
            cabecote = 0; // Sempre começa a ler no índice 0
            estadoAtual = estadoInicial;
            int passo = 0; // Contador pra não deixar o programa travar em loop infinito

            // Loop principal da máquina de Turing
            while (true)
            {
                // Se a posição atual não existe no dicionário, significa que é um espaço em branco ('_')
                char simboloAtual = fita.ContainsKey(cabecote) ? fita[cabecote] : '_';

                Console.WriteLine($"[Passo: {passo}] | Estado: {estadoAtual} | Cabeçote: {cabecote}");

                // Lógica pra descobrir onde a fita começa e termina pra conseguir imprimir na tela
                int minIndex = fita.Count > 0 ? Math.Min(fita.Keys.Min(), cabecote) : cabecote;
                int maxIndex = fita.Count > 0 ? Math.Max(fita.Keys.Max(), cabecote) : cabecote;

                var sb = new StringBuilder();
                for (int i = minIndex; i <= maxIndex; i++)
                {
                    char s = fita.ContainsKey(i) ? fita[i] : '_';
                    // Coloca os colchetes [ ] no caractere onde o cabeçote está agora 
                    if (i == cabecote) sb.Append($"[{s}]");
                    else sb.Append(s);
                }
                
                Console.WriteLine($"Fita: {sb}");

                // Verifica se a máquina chegou num estado de parada
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
                
                // Se não achar a transição na tabela a máquina trava/rejeita
                if (!transicoes.TryGetValue(chave, out var trans))
                {
                    Console.WriteLine($"Sem transição definida para δ({estadoAtual}, '{simboloAtual}') — forçando rejeição.");
                    estadoAtual = "qreject";
                    continue; // Faz o while rodar mais uma vez só pra imprimir a fita no estado de rejeição e depois sair
                }

                var (novoEstado, escrever, direcao) = trans;

                // Escreve o símbolo novo na fita
                if (escrever == '_')
                {
                    // remove a chave do dicionário
                    if (fita.ContainsKey(cabecote)) fita.Remove(cabecote);
                }
                else
                {
                    fita[cabecote] = escrever;
                }

                // Anda com o cabeçote para a Direita (D) ou Esquerda (E)
                if (direcao == 'D') cabecote++;
                else if (direcao == 'E') cabecote--;

                // Atualiza pro próximo estado
                estadoAtual = novoEstado;

                // Proteção contra loop infinito da exigência "e"
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

            // Lógica do a^n b^n c^n:
          
            // ================== Q0: Buscando o 'a' ==================
            trans.Add(("q0", 'a'), ("q1", 'X', 'D')); // Achou o 'a', marca com 'X' e vai procurar o 'b'
            trans.Add(("q0", 'X'), ("q0", 'X', 'D')); // Pula os 'a's que já foram marcados
            trans.Add(("q0", 'Y'), ("q4", 'Y', 'D')); // Se não tem mais 'a', mas achou 'Y', vai verificar se sobrou algo 
            // Qualquer outra coisa no começo tá errado (rejeita)
            trans.Add(("q0", 'b'), ("qreject", 'b', 'D'));
            trans.Add(("q0", 'c'), ("qreject", 'c', 'D'));
            trans.Add(("q0", '_'), ("qreject", '_', 'D')); // Se a palavra for vazia, tem que rejeitar

            // ================== Q1: Buscando o 'b' ==================
            trans.Add(("q1", 'a'), ("q1", 'a', 'D')); // Pula os 'a's normais
            trans.Add(("q1", 'Y'), ("q1", 'Y', 'D')); // Pula os 'b's que já foram marcados (Y)
            trans.Add(("q1", 'b'), ("q2", 'Y', 'D')); // Achou o 'b', marca com 'Y' e vai procurar o 'c'
            trans.Add(("q1", 'c'), ("qreject", 'c', 'D')); // Achou 'c' antes de 'b'
            trans.Add(("q1", 'Z'), ("qreject", 'Z', 'D'));
            trans.Add(("q1", '_'), ("qreject", '_', 'D')); // Fita acabou antes de achar o 'b'

            // ================== Q2: Buscando o 'c' ==================
            trans.Add(("q2", 'b'), ("q2", 'b', 'D')); // Pula os 'b's normais
            trans.Add(("q2", 'Z'), ("q2", 'Z', 'D')); // Pula os 'c's já marcados
            trans.Add(("q2", 'c'), ("q3", 'Z', 'E')); // Achou o 'c'! Marca com 'Z' e agora começa a voltar pra esquerda 
            trans.Add(("q2", 'a'), ("qreject", 'a', 'D')); // 'a' fora de ordem
            trans.Add(("q2", 'Y'), ("qreject", 'Y', 'D'));
            trans.Add(("q2", '_'), ("qreject", '_', 'D')); // Fita acabou sem achar o 'c'

            // ================== Q3: Voltando para o começo ==================
            // Volta tudo pra esquerda até achar o último 'X' que foi marcado
            trans.Add(("q3", 'a'), ("q3", 'a', 'E'));
            trans.Add(("q3", 'b'), ("q3", 'b', 'E'));
            trans.Add(("q3", 'Y'), ("q3", 'Y', 'E'));
            trans.Add(("q3", 'Z'), ("q3", 'Z', 'E'));
            trans.Add(("q3", 'X'), ("q0", 'X', 'D')); // Bateu no 'X', dá um passo pra direita e reinicia o ciclo no q0
            trans.Add(("q3", '_'), ("qreject", '_', 'D')); 

            // ================== Q4: Verificando o final ==================
            // Já marcou todos os 'a's. Agora vai até o final da fita conferir se não sobrou lixo (um 'b' ou 'c' a mais)
            trans.Add(("q4", 'Y'), ("q4", 'Y', 'D')); // Pula os marcadores
            trans.Add(("q4", 'Z'), ("q4", 'Z', 'D'));
            trans.Add(("q4", '_'), ("qaccept", '_', 'D')); // Chegou no branco sem sobrar nada! Palavra ACEITA!
            // Se sobrou alguma letra sem marcar, é porque as quantidades não batem. Rejeita.
            trans.Add(("q4", 'a'), ("qreject", 'a', 'D'));
            trans.Add(("q4", 'b'), ("qreject", 'b', 'D'));
            trans.Add(("q4", 'c'), ("qreject", 'c', 'D'));
            trans.Add(("q4", 'X'), ("qreject", 'X', 'D'));

            return new TURING(trans);
        }

        public static TURING CriarMT_Unario()
        {
            var trans = new Dictionary<(string, char), (string, char, char)>();

            // Desafio obrigatório: f(n) = n + 1 em unário.

            // Fica no q0 pulando os '1's pra direita
            trans.Add(("q0", '1'), ("q0", '1', 'D')); 
            
            // Achou o branco no final da palavra, escreve o '1' a mais e vai pro estado de aceitação
            trans.Add(("q0", '_'), ("qaccept", '1', 'D'));

            return new TURING(trans);
        }
    }
}