# Trabalho Prático - Simuladores de Modelos Computacionais

## 👥 Equipe
* **Arthur Candido Teixeira** | Matrícula: 72400072
* **Lucas Henrique Albergaria** | Matrícula: 72401400
* **Mateus Soier Ximenes Melo** | Matrícula: 72401494

---

## 📝 Descrição dos Projetos

Este repositório contém as implementações de diferentes modelos computacionais desenvolvidos para a disciplina de **Fundamentos Teóricos da Computação**. O projeto acompanha a evolução da Hierarquia de Chomsky e é estruturado nas seguintes etapas:

### 🔹 Parte 1: Autômato Finito Determinista (AFD)
Simulador de um AFD focado no reconhecimento de Linguagens Regulares. 
* **Características:** O programa permite tanto a execução de um AFD padrão em código quanto o carregamento dinâmico da 5-tupla formal através de um arquivo `afd.json`.
* **Entradas e Validação:** O simulador lê as cadeias de teste a partir do arquivo `entradasAFD.txt` e exibe em lote se foram aceitas ou rejeitadas.

### 🔹 Parte 2: Autômato de Pilha (AP)
Simulador de Autômato de Pilha utilizando o critério de aceitação **exclusivo por pilha vazia** (sem a necessidade de verificação de estado final).
* **Linguagem L2 ($a^n b^n \mid n \geq 1$):** Implementação de um AP determinístico focado na contagem balanceada de dois símbolos.
* **Linguagem L3 (Palíndromos):** Implementação de um AP não-determinístico que utiliza movimentos-lambda (representados por `\0`) para explorar ramos de execução paralelos e encontrar o meio da cadeia.
* **Saída Detalhada:** Exibe a configuração instantânea a cada transição (estado atual, conteúdo da pilha e o restante da cadeia). As palavras são consumidas via `entradasPILHA.txt`.

### 🔹 Parte 3: Máquina de Turing (MT)
Simulador de uma Máquina de Turing padrão com fita bidirecional e infinita, capaz de reconhecer linguagens sensíveis ao contexto e computar funções unárias.
* **Linguagem L4 ($a^n b^n c^n \mid n \geq 1$):** Reconhecedor que utiliza uma lógica de casamento de padrões, substituindo os caracteres por marcadores (`X`, `Y`, `Z`) e movendo o cabeçote consecutivamente para garantir o balanceamento dos três blocos.
* **Função Unária ($f(n) = n + 1$):** Implementação do desafio obrigatório de um transdutor unário, onde a máquina percorre a cadeia de `1`s e adiciona um símbolo extra ao atingir o primeiro espaço em branco (`_`).
* **Visualização da Fita:** A simulação exibe o estado atual, o número do passo e o estado real da fita, destacando a posição exata do cabeçote com colchetes `[ ]`. Conta também com um limitador de passos (`maxPassos`) para evitar travamentos por loops infinitos.

---

## 🚀 Instruções de Compilação e Execução

### 🛠️ Pré-requisitos de Ambiente
Os projetos foram desenvolvidos em **C#** e exigem o **SDK do .NET 6.0** (ou versão superior) instalado no sistema.

### 💻 Como Executar
Para rodar qualquer uma das partes, navegue até a pasta do projeto específico pelo terminal e utilize os comandos do .NET CLI:

1. **Restaurar as dependências e compilar:**
   ```bash
   dotnet build

2. **Inicializar a Parte**
   ```bash
   dotnet build

---

### Link Video de Defesa

