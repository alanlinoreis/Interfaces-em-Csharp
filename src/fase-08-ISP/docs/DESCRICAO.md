
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

# 📁 Estrutura do Repositório (Atualizado até a Fase 08)

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
    │   ├── Domain.App/
    │   │   ├── produtos.csv
    │   │   └── Program.cs
    │   │
    │   ├── Domain.Entities/
    │   │   ├── Models/
    │   │   ├── Repository/
    │   │   ├── Seletores/
    │   │   └── Service/
    │   │
    │   ├── Domain.Tests/
    │   │   ├── CsvProdutoRepositoryTests.cs
    │   │   ├── ProdutoRepositoryTests.cs
    │   │   ├── ProdutoServiceSelecaoTests.cs
    │   │   ├── ProdutoServiceTests.cs
    │   │   ├── SeletorEconomicoTests.cs
    │   │   ├── SeletorFactoryTests.cs
    │   │   ├── SeletorPremiumTests.cs
    │   │   └── SeletorQualidadeTests.cs
    │   │
    │   └── docs/
    │       └── DESCRICAO.md
    │
    ├── fase-07-Repository-Json/
    │   ├── Domain.App/
    │   │   ├── produtos.json
    │   │   └── Program.cs
    │   │
    │   ├── Domain.Entities/
    │   │   ├── Models/
    │   │   ├── Repository/
    │   │   ├── Seletores/
    │   │   └── Service/
    │   │
    │   ├── Domain.Tests/
    │   │   ├── JsonProdutoRepositoryTests.cs
    │   │   ├── ProdutoRepositoryTests.cs
    │   │   ├── ProdutoServiceSelecaoTests.cs
    │   │   ├── ProdutoServiceTests.cs
    │   │   ├── SeletorEconomicoTests.cs
    │   │   ├── SeletorFactoryTests.cs
    │   │   ├── SeletorPremiumTests.cs
    │   │   └── SeletorQualidadeTests.cs
    │   │
    │   └── docs/
    │       ├── DESCRICAO.md
    │       ├── diagrama_arquitetura_fase7.png
    │       ├── diagrama_fluxo_crud_fase7.png
    │       └── diagrama_json_flow_fase7.png
    │
    ├── fase-08-ISP/
        ├── Domain.App/
        │   └── Program.cs
        │
        ├── Domain.Entities/
        │   ├── Models/
        │   ├── Repository/
        │   │   ├── IReadRepository.cs
        │   │   ├── IWriteRepository.cs
        │   │   ├── IRepository.cs
        │   │   ├── InMemoryRepository.cs
        │   │   └── JsonProdutoRepository.cs
        │   ├── Seletores/
        │   └── Service/
        │
        ├── Domain.Tests/
        │   ├── ProdutoServiceTests.cs
        │   ├── ProdutoServiceSelecaoTests.cs
        │   ├── (demais testes se mantêm)
        │
        └── docs/
            └── DESCRICAO.md
```

---

# 📜 Resumo das Fases

## 🧩 Fase 00 — Aquecimento
- Definição do domínio
- Objetivo, contrato e peças alternáveis

## 🧩 Fase 01 — Heurística Antes do Código
- Mapa mental
- Comparação procedural vs OO

## 🧩 Fase 02 — Procedural
- Tudo em Program.cs
- Sem OO, sem interface

## 🧩 Fase 03 — OO Sem Interface
- Herança
- SeletorBase
- Implementações concretas

## 🧩 Fase 04 — Interface Plugável e Testável
- ISeletorDeProduto
- Factory
- Testes independentes
- Projetos separados

## 🧩 Fase 05 — Repository InMemory
- Contrato Repository
- Implementação em memória
- CRUD + testes

## 🧩 Fase 06 — Repository CSV
- Persistência real
- Leitura/escrita CSV robusta
- Testes com arquivos temporários
- Program integrado

## 🧩 Fase 07 — Repository JSON (System.Text.Json)
- Nova implementação do repository
- JSON indentado + camelCase
- Arquivo produtos.json
- Testes completos
- Diagramas documentando a arquitetura

## 🧩 Fase 08 — ISP (Interface Segregation Principle)
- Divisão do repository em:
  - `IReadRepository`
  - `IWriteRepository`
- Serviços separados por responsabilidade
- Testes ajustados para usar interfaces segregadas
- Program atualizado
- Domínio permanece inalterado

---

# ▶️ Como executar o projeto

Entre na pasta da fase desejada:

```
src/fase-XX/Domain.App
dotnet run
```

---

# 🧪 Rodar Testes

Entre na pasta:

```
src/fase-XX/Domain.Tests
dotnet test
```

---

# ✔️ Conclusão

O projeto evolui aumentando:

- Abstração  
- Reutilização  
- Testabilidade  
- Baixo acoplamento  
- Clareza arquitetural  

Com a Fase 08, entramos no mundo SOLID aplicando o ISP, preparando terreno para fases avançadas (injeção de dependência, banco de dados, DDD, etc.).
