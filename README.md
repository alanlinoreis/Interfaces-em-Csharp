# 🧱 Projeto — Seletor de Produtos por Preço e Qualidade
**Atividade: Tarefa por Fases — Interfaces em C#**

---

## 👥 Equipe

| Integrante | RA / Identificação |
|-------------|--------------------|
| **Alan Lino dos Reis** | a2724332 |
| **Pedro Lucas Reis** | a2716020 |
| **Pedro Gabriel Sepulveda Borgheti** | a2706059 |

---

# 📁 Estrutura do Repositório (Atualizado até a Fase 10)

```
repo-raiz/
│
├── README.md
├── docs/
│   └── DESCRIÇÃO.md
└── src/
    ├── Domain.App/
    │   ├── Program.cs
    │   └── produtos.json
    │
    ├── Domain.Entities/
    │   ├── Contracts/
    │   │   ├── IAsyncReader.cs
    │   │   ├── IAsyncWriter.cs
    │   │   ├── IClock.cs
    │   │   └── IIdGenerator.cs
    │   │
    │   ├── Doubles/
    │   │   ├── ClockFake.cs
    │   │   ├── ReaderFake.cs
    │   │   └── WriterFake.cs
    │   │
    │   ├── Models/
    │   │   └── Produto.cs
    │   │
    │   ├── Repository/
    │   │   ├── InMemoryRepository.cs
    │   │   ├── IReadRepository.cs
    │   │   ├── IRepository.cs
    │   │   ├── IWriteRepository.cs
    │   │   └── JsonProdutoRepository.cs
    │   │
    │   ├── Seletores/
    │   │   ├── ISeletorDeProduto.cs
    │   │   ├── ModoSelecao.cs
    │   │   ├── SeletorEconomico.cs
    │   │   ├── SeletorPremium.cs
    │   │   ├── SeletorQualidade.cs
    │   │   └── SeletorFactory.cs
    │   │
    │   └── Service/
    │       ├── ProdutoSelecaoService.cs
    │       ├── ProdutoService.cs
    │       └── PumpService.cs
    │
    └── Domain.Tests/
        ├── JsonProdutoRepositoryTests.cs
        ├── ProdutoRepositoryTests.cs
        ├── ProdutoServiceSelecaoTests.cs
        ├── ProdutoServiceTests.cs
        ├── PumpServiceTests.cs
        ├── SeletorEconomicoTests.cs
        ├── SeletorFactoryTests.cs
        ├── SeletorPremiumTests.cs
        ├── SeletorQualidadeTests.cs
```

---

# 📜 Resumo das Fases

## 🧩 Fase 00 — Aquecimento
- Definição do domínio, objetivo e política do seletor de produtos.

## 🧩 Fase 01 — Heurística Antes do Código
- Análise de soluções (procedural, OO, OO com interface).
- Identificação de acoplamentos e pontos fracos.

## 🧩 Fase 02 — Procedural Mínimo
- Implementação 100% procedural.
- Tudo dentro de `Program.cs`.

## 🧩 Fase 03 — OO Sem Interface
- Polimorfismo via herança.
- Cliente ainda acoplado às classes concretas.

## 🧩 Fase 04 — Interface Plugável e Testável
- Introdução de **ISeletorDeProduto**.
- Testes independentes de implementação.
- Projetos separados: Entities / App / Tests.

## 🧩 Fase 05 — Repository InMemory
- Introdução do contrato de `IRepository<T, TId>`.
- Persistência simulada em memória.
- Serviço atualizado (`ProdutoService`).
- Testes completos de CRUD e seletores.

## 🧩 Fase 06 — Repository CSV
- Persistência real baseada em arquivo CSV.
- Mesmo contrato de Repository da fase anterior.
- Repositório concreto: `CsvProdutoRepository`.
- Testes com arquivos temporários.

## 🧩 Fase 07 — Repository JSON (System.Text.Json)
- Repositório real com leitura e escrita JSON.
- Arquivo `produtos.json` substitui o CSV.
- Testes preservados, usando dublês de arquivo.

## 🧩 Fase 08 — ISP (Interface Segregation Principle)
- Repository é segregado em:
  - `IReadRepository<T,TId>`
  - `IWriteRepository<T,TId>`
- `JsonProdutoRepository` implementa **ambas**.
- Cliente passa a depender apenas da interface necessária.
- Program reorganizado para leitura/escrita seletiva.

## 🧩 Fase 09 — Dublês Avançados e Testes Assíncronos
- Introdução das interfaces assíncronas:
  - `IAsyncReader<T>`
  - `IAsyncWriter<T>`
  - `IClock`
- Criação de dublês (`ReaderFake`, `WriterFake`, `ClockFake`).
- Implementação do `PumpService` com retry, backoff, cancelamento e tempo injetável.

## 🧩 Fase 10 — Cheiros e Antídotos
- `ProdutoService` separado em CRUD + `ProdutoSelecaoService`.
- Strings substituídas por enum `ModoSelecao`.
- `SeletorFactory` usando `Dictionary<ModoSelecao, Func<ISeletorDeProduto>>`.
- `PumpService` com cálculo de backoff extraído.
- `Program.cs` usando enum e serviço de seleção.

---

# ▶️ Como executar o projeto

```
cd src/Domain.App
dotnet run
```

---

# 🧪 Como rodar os testes

```
cd src/Domain.Tests
dotnet test
```

---

# ✔️ Conclusão

Com a Fase 10, o projeto está mais limpo, menos acoplado, sem strings mágicas e mais preparado para DI/DIP.
