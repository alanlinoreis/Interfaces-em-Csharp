# 🧱 Projeto — Seletor de Produtos por Preço e Qualidade

**Atividade: Tarefa por Fases — Interfaces em C#**

---

## 👥 Equipe

| Integrante                           | RA       |
| ------------------------------------ | -------- |
| **Alan Lino dos Reis**               | a2724332 |
| **Pedro Lucas Reis**                 | a2716020 |
| **Pedro Gabriel Sepulveda Borgheti** | a2706059 |

---

# 📁 Estrutura Geral do Repositório (Atualizada até a Fase 12)

Cada fase possui:

* sua própria aplicação (`Domain.App`)
* suas próprias entidades (`Domain.Entities`)
* seus próprios testes (`Domain.Tests`)
* **(NOVO na Fase 12)** uma camada de dados (`Domain.Data`) para banco SQLite

```
src/
├── fase-00-*/
├── fase-01-*/
├── fase-02-*/
├── fase-03-*/
├── fase-04-*/
├── fase-05-*/
├── fase-06-*/
├── fase-07-*/
├── fase-08-*/
├── fase-09-*/
├── fase-10-*/
├── fase-11-Mini-Projeto/
└── fase-12-Projeto-Com-BD/
    └── src/
        ├── Domain.App/
        ├── Domain.Entities/
        ├── Domain.Data/   ← NOVO
        └── Domain.Tests/
```

---

# 📦 Conteúdo da Fase 12

A Fase 12 evolui a Fase 11 adicionando:

* ✅ Persistência em **SQLite com Entity Framework Core**
* ✅ Nova camada `Domain.Data`
* ✅ Factory de repositório para alternar entre JSON e SQLite
* ✅ Testes de integração do SQLite
* ✅ Manutenção total da arquitetura limpa

---

# 📁 Domain.Entities (Fase 12)

```
Domain.Entities/
├── Contracts/
│   ├── IAsyncReader.cs
│   ├── IAsyncWriter.cs
│   ├── IClock.cs
│   └── IIdGenerator.cs
│
├── Doubles/
│   ├── ClockFake.cs
│   ├── ReaderFake.cs
│   └── WriterFake.cs
│
├── Models/
│   └── Produto.cs
│
├── Repository/
│   ├── InMemoryRepository.cs
│   ├── IReadRepository.cs
│   ├── IRepository.cs
│   ├── IWriteRepository.cs
│   └── JsonProdutoRepository.cs
│
├── Seletores/
│   ├── ISeletorDeProduto.cs
│   ├── ModoSelecao.cs
│   ├── SeletorEconomico.cs
│   ├── SeletorFactory.cs
│   ├── SeletorPremium.cs
│   └── SeletorQualidade.cs
│
└── Service/
    ├── ProdutoSelecaoService.cs
    ├── ProdutoService.cs
    └── PumpService.cs
```

---

# 📁 Domain.Data (Fase 12 — NOVO)

```
Domain.Data/
├── CatalogoDbContext.cs
├── SqliteProdutoRepository.cs
└── SqliteRepositoryFactory.cs
```

### Responsabilidades:

* `CatalogoDbContext`

  * DbContext do Entity Framework Core
  * Expõe `DbSet<Produto>`
  * Configura mapeamento do Produto

* `SqliteProdutoRepository`

  * Implementa `IReadRepository<Produto,int>` e `IWriteRepository<Produto,int>`
  * Executa CRUD real no SQLite

* `SqliteRepositoryFactory`

  * Cria o `DbContext`
  * Garante criação automática do banco (`catalogo.db`)
  * Retorna repositórios prontos para uso

---

# 📁 Domain.App (Fase 12)

```
Domain.App/
├── Program.cs
├── produtos.json
└── catalogo.db   (gerado automaticamente)
```

### Program.cs contém:

* Menu completo:

  * CRUD
  * Seleção por enum
  * Exportação / Importação JSON
  * Stream assíncrono
* Composição explícita de repositórios:

  * `--json` → usa `JsonProdutoRepository`
  * padrão → usa `SQLite + Entity Framework`
* Seed automático no banco ou no JSON

---

# 📁 Domain.Tests (Fase 12)

```
Domain.Tests/
├── JsonProdutoRepositoryTests.cs
├── ProdutoRepositoryTests.cs
├── ProdutoServiceTests.cs
├── ProdutoServiceFase11Tests.cs
├── ProdutoServiceSelecaoTests.cs
├── PumpServiceTests.cs
├── SeletorEconomicoTests.cs
├── SeletorPremiumTests.cs
├── SeletorQualidadeTests.cs
├── SeletorFactoryTests.cs
└── SqliteProdutoRepositoryTests.cs   ← NOVO
```

### Testes adicionados na Fase 12:

* Testes de integração do SQLite:

  * `Add`
  * `ListAll`
  * `GetById`
  * `Update`
  * `Remove`
  * Verificação da criação automática do banco

---

# ▶️ Como executar a Fase 12

### Usando SQLite (padrão):

```
cd src/fase-12-Projeto-Com-BD/src/Domain.App
dotnet run
```

### Usando JSON:

```
dotnet run -- --json
```

---

# 🧪 Como rodar os testes da Fase 12

```
cd src/fase-12-Projeto-Com-BD/src/Domain.Tests
dotnet test
```

---

# ✔️ Conclusão da Fase 12

A Fase 12 consolida totalmente o projeto com:

* Arquitetura limpa em camadas
* Contratos de repositório bem definidos
* Persistência em JSON e SQLite
* Entity Framework Core integrado corretamente
* Testes unitários + testes de integração
* Composição explícita no `Program.cs`

O projeto está agora pronto para evoluir para APIs, interfaces gráficas ou troca de banco de dados (PostgreSQL, MySQL, etc.) sem alteração no domínio.
