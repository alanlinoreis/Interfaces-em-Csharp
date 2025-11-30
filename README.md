# 🧱 Projeto — Seletor de Produtos por Preço e Qualidade
**Atividade: Tarefa por Fases — Interfaces em C#**

---

## 👥 Equipe

| Integrante | RA / Identificação |
|-------------|--------------------|
| **Alan Lino dos Reis** | *(a2724332)* |
| **Pedro Lucas Reis** | *(a2716020)* |
| **Pedro Gabriel Sepulveda Borgheti** | *(a2706059)* |

---

# 📁 Estrutura do Repositório (Atualizado até a Fase 06)

```plaintext
repo-raiz/
│
├── README.md
└── src/
    ├── fase-00-aquecimento/
    │   └── DESCRICAO.md
    │
    ├── fase-01-heuristica/
    │   └── DESCRICAO.md
    │
    ├── fase-02-procedural/
    │   └── Program.cs
    │
    ├── fase-03-oo-sem-interface/
    │   ├── Program.cs
    │   └── DESCRICAO.md
    │
    ├── fase-04-interface/
    │   ├── domain.entities/
    │   ├── domain.app/
    │   ├── domain.tests/
    │   └── DESCRICAO.md
    │
    ├── fase-05-Repository-In-Memory/
    │   ├── Domain.Entities/
    │   ├── Domain.App/
    │   ├── Domain.Tests/
    │   └── docs/
    │       └── DESCRICAO.md
    │
    ├── fase-06-Repository-CSV/
        ├── Domain.App/
        │   ├── produtos.csv
        │   └── Program.cs
        │
        ├── Domain.Entities/
        │   ├── Models/
        │   ├── Repository/
        │   ├── Seletores/
        │   └── Service/
        │
        ├── Domain.Tests/
        │   ├── CsvProdutoRepositoryTests.cs
        │   ├── ProdutoRepositoryTests.cs
        │   ├── ProdutoServiceSelecaoTests.cs
        │   ├── ProdutoServiceTests.cs
        │   ├── SeletorEconomicoTests.cs
        │   ├── SeletorFactoryTests.cs
        │   ├── SeletorPremiumTests.cs
        │   └── SeletorQualidadeTests.cs
        │
        └── docs/
            └── DESCRICAO.md
```

---

# 📜 Resumo das Fases

## 🧩 Fase 00 — Aquecimento
- Definição do domínio (seletor de produtos por preço e qualidade)
- Objetivo, contrato, política e peças alternáveis

---

## 🧩 Fase 01 — Heurística Antes do Código
- Mapa mental
- Análise procedural vs OO sem interface vs OO com interface
- Identificação de pontos de dor e sinais de alerta

---

## 🧩 Fase 02 — Procedural Mínimo
- Implementação totalmente procedural
- Uso de if/switch
- Sem OO, sem interface
- Código centralizado em Program.cs

---

## 🧩 Fase 03 — OO Sem Interface
- Classe base abstrata SeletorBase
- Polimorfismo via herança
- Implementações concretas:
  - SeletorEconomico
  - SeletorPremium
  - SeletorQualidade
- Cliente ainda acoplado às concretas

---

## 🧩 Fase 04 — Interface Plugável e Testável
- Separação do código em três projetos:
  - domain.entities (negócio)
  - domain.app (aplicação)
  - domain.tests (testes)
- Interface ISeletorDeProduto
- Factory centralizada (SeletorFactory)
- Testes completos para cada implementação
- Cliente depende apenas da interface

---

## 🧩 Fase 05 — Repository InMemory
- Introdução do contrato de Repository
- Implementação InMemory para simular persistência
- Serviço de domínio atualizado para receber o repository
- Testes completos do CRUD InMemory
- Primeira fase com acoplamento mínimo entre domínio e armazenamento

---

## 🧩 Fase 06 — Repository CSV (Persistência Real em Arquivo)
- Evolução do repositório: agora persistência em CSV
- Mesmo contrato (`IRepository<T, TId>`)
- Implementação completa do `CsvProdutoRepository`
- Manipulação de arquivo com suporte a vírgulas e aspas
- Program.cs com CRUD + seletores + leitura de CSV
- Testes unitários usando arquivos temporários
- Infraestrutura substituível: CSV e InMemory coexistem
- Nenhuma alteração no domínio ou seletores — apenas na infraestrutura

---

# ▶️ Como executar o projeto

Na pasta:

```
src/fase-06-Repository-CSV/Domain.App
```

Execute:

```bash
dotnet run
```

---

# 🧪 Como rodar os testes

Na pasta:

```
src/fase-06-Repository-CSV/Domain.Tests
```

Rode:

```bash
dotnet test
```

---

# ✔️ Projeto em constante evolução

A cada fase o sistema ganha:

- mais abstração  
- menos acoplamento  
- testes mais confiáveis  
- camadas mais claras  
- evolução natural para um backend real  

A Fase 07 evoluirá o repositório para Banco de Dados ou múltiplas implementações coexistindo.

