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

# 📁 Estrutura do Repositório

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
    │   │   ├── Produto.cs
    │   │   ├── ISeletorDeProduto.cs
    │   │   ├── SeletorEconomico.cs
    │   │   ├── SeletorPremium.cs
    │   │   ├── SeletorQualidade.cs
    │   │   └── SeletorFactory.cs
    │   │
    │   ├── domain.app/
    │   │   └── Program.cs
    │   │
    │   ├── domain.tests/
    │   │   ├── SeletorEconomicoTests.cs
    │   │   ├── SeletorPremiumTests.cs
    │   │   ├── SeletorQualidadeTests.cs
    │   │   └── SeletorFactoryTests.cs
    │   │
    │   └── DESCRICAO.md
    │
    ├── fase-05-repository-inmemory/
    │   └── DESCRICAO.md
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

## 🧩 Fase 05 — Repository InMemory (conceitual)
- Documento explicativo (DESCRICAO.md) com:
  - Diagrama
  - Snippets de contrato de Repository
  - Snippets da implementação InMemory
  - Snippets de serviço + cliente usando o repo
  - Snippets de testes unitários
  - Política de ID documentada
- Nenhum arquivo .cs criado nesta fase (somente documentação)

---

# ▶️ Como executar o projeto

Na pasta `domain.app`:

```bash
dotnet run
```

---

# 🧪 Como rodar os testes

Na pasta `domain.tests`:

```bash
dotnet test
```

---
