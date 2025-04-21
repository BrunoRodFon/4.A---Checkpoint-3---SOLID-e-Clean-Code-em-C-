# Sistema de Gerenciamento de Biblioteca - Refatoração com SOLID e Clean Code

## 📚 Descrição

Este projeto apresenta um sistema simples de gerenciamento de biblioteca desenvolvido em C#, originalmente com várias violações dos princípios SOLID e boas práticas de Clean Code.

O objetivo foi analisar o código original, identificar as principais violações e refatorar a aplicação para torná-la mais robusta, legível, reutilizável e de fácil manutenção.

---

## ✅ Parte 1: Identificação de Problemas

### 1. 🔧 Violação do **SRP (Single Responsibility Principle)**
- **Local:** Classe `GerenciadorBiblioteca`
- **Problema:** A classe realiza múltiplas responsabilidades: cadastro de livros, usuários, empréstimos, cálculo de multas e envio de notificações.
- **Justificativa:** Cada classe deve ter apenas uma razão para mudar. A junção de responsabilidades dificulta a manutenção e os testes.

---

### 2. 🔧 Violação do **OCP (Open/Closed Principle)**
- **Local:** Métodos `EnviarEmail` e `EnviarSMS`
- **Problema:** Para adicionar um novo tipo de notificação (ex: push), seria necessário alterar o código da classe principal.
- **Justificativa:** O sistema deve ser aberto para extensão, mas fechado para modificação. A solução ideal seria o uso de interfaces e injeção de dependência.

---

### 3. 🔧 Violação do **DIP (Dependency Inversion Principle)**
- **Local:** Dependência direta de métodos de notificação dentro da classe `GerenciadorBiblioteca`
- **Problema:** A classe depende de implementações concretas (e-mail e SMS).
- **Justificativa:** A aplicação deveria depender de abstrações (interfaces), facilitando a substituição ou extensão das formas de notificação.

---

### 4. 🔧 Violação de Clean Code – **Métodos com múltiplas responsabilidades**
- **Local:** Método `RealizarEmprestimo`
- **Problema:** O método realiza buscas, validações, atualizações de estado e envio de notificações.
- **Justificativa:** Métodos devem ser pequenos e focados em uma única tarefa para aumentar a legibilidade e testabilidade.

---

### 5. 🔧 Violação de Clean Code – **Nomes pouco expressivos**
- **Local:** Uso de variáveis como `l`, `u`, `e`
- **Problema:** Variáveis com nomes genéricos dificultam o entendimento.
- **Justificativa:** Devem ser utilizados nomes significativos que comuniquem claramente sua finalidade.

---

## 🛠️ Parte 2: Refatoração

A refatoração completa está disponível no arquivo [`BibliotecaRefatorada.cs`](./BibliotecaRefatorada.cs), que aplica:

- **Princípios SOLID**:
  - SRP: Separação das responsabilidades em serviços e entidades
  - OCP/DIP: Uso de interfaces para permitir extensão sem modificação
  - LSP e ISP aplicados indiretamente pela modelagem limpa

- **Boas práticas de Clean Code**:
  - Métodos curtos e focados
  - Nomes descritivos
  - Separação clara de camadas
  - Comentários apenas quando necessário

---

## 📁 Estrutura do Projeto

