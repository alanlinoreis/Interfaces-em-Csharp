# 🧱 Arquitetura — Fase 12 (Mini-projeto de Consolidação com Banco de Dados)

**Domínio:** Catálogo de Produtos  
**Versão:** Fase 12  

---

# 📌 Objetivo da Arquitetura

Consolidar todos os padrões estudados e **evoluir a persistência** para incluir:

- Repository Pattern (InMemory + JSON + SQLite)
- ISP (IReadRepository / IWriteRepository)
- Serviços focados (CRUD, Seleção)
- CLI com composição explícita
- Testes unitários + integração
- Fluxos assíncronos (IAsyncEnumerable)
- **Nova camada de dados com Entity Framework Core + SQLite**

---

# 🧠 Componentes Principais

### **Models (Domain.Entities.Models)**
- `Produto`

### **Contratos (Domain.Entities.Repository)**
- `IReadRepository<T,TId>`
- `IWriteRepository<T,TId>`

### **Repositórios (Implementações)**
- `InMemoryRepository`  
  - Usado principalmente para testes.
- `JsonProdutoRepository`  
  - Persistência em arquivo `produtos.json`.
- `SqliteProdutoRepository`  
  - Persistência em banco SQLite via Entity Framework Core.
  - Implementa `IReadRepository<Produto,int>` e `IWriteRepository<Produto,int>`.

### **Camada de Dados (Domain.Data) – NOVA**

- `CatalogoDbContext`
  - `DbContext` do Entity Framework para o catálogo.
  - Expõe `DbSet<Produto> Produtos`.
  - Faz o mapeamento (chave primária, tamanhos, precisão de preço, etc.).
- `SqliteProdutoRepository`
  - Implementação de repositório baseada em `CatalogoDbContext`.
  - Responsável por Add / Update / Remove / List / GetById usando EF Core.
- `SqliteRepositoryFactory`
  - Fábrica estática responsável por:
    - Montar `DbContextOptions<CatalogoDbContext>` com `UseSqlite("Data Source=catalogo.db")`.
    - Garantir a criação do banco (`EnsureCreated` ou `Migrate`, se configurado).
    - Retornar um par `(IReadRepository<Produto,int>, IWriteRepository<Produto,int>)` pronto para uso.

### **Serviços (Domain.Entities.Service)**

- `ProdutoService`
  - CRUD
  - Exportação e importação JSON
  - Busca e filtros
  - Stream assíncrono (`IAsyncEnumerable<Produto>`)

- `ProdutoSelecaoService`
  - Seleção de produtos a partir de um **enum** de modo (ex: Econômico, Premium, Qualidade).
  - Usa repositórios via `IReadRepository`.

### **Seletores (Domain.Entities.Seletores)**

- `SeletorEconomico`
- `SeletorPremium`
- `SeletorQualidade`
- `SeletorFactory`
- `ModoSelecao` (enum)
  - Controla o modo de seleção usado por `ProdutoSelecaoService`.

### **Aplicação (Domain.App)**

- `Program.cs`  
  - Menu interativo com:
    - CRUD de produtos
    - Seleção via enum (ModoSelecao)
    - Export/Import JSON
    - Stream assíncrono
  - **Composição explícita de repositórios**:
    - Lê argumentos da linha de comando:
      - `--json` → usa `JsonProdutoRepository` (arquivo).
      - *Sem argumento* → usa `SqliteRepositoryFactory.Create()` (SQLite + EF Core).
    - Chama `GarantirArquivoInicial(leitor, escritor)` para fazer seed (independente se é JSON ou SQLite).

---

# 🗂 Persistência na Fase 12

### JSON (camada existente)

- Continua funcionando via:
  - `JsonProdutoRepository`
  - Arquivo `produtos.json`
- Ativado ao rodar:
  - `dotnet run -- --json`

### SQLite + Entity Framework (nova camada)

- Novo arquivo: `catalogo.db` (mais arquivos auxiliares `-wal` e `-shm` criados pelo SQLite).
- Repositório principal:
  - `SqliteProdutoRepository`
- Configuração e criação de banco:
  - `SqliteRepositoryFactory.Create("Data Source=catalogo.db")`
- Ativado por padrão (sem `--json`).

---

# 🧪 Testes

- Unitários: continuam usando `InMemoryRepository`.
- Integração:
  - Fluxos de export/import JSON.
  - Operações completas utilizando o repositório SQLite (`SqliteProdutoRepository`) podem ser adicionadas.
- Async:
  - Stream de produtos (`ProdutoService.StreamAsync`) funcional com qualquer repositório que implemente `IReadRepository`.

---

# ✔ Conclusão

A arquitetura evoluiu de arquivos em JSON para incluir uma camada de banco de dados com **SQLite + Entity Framework Core**, mantendo:

- Domínio limpo (sem dependência de EF Core).
- Contratos de repositório estáveis (`IReadRepository/IWriteRepository`).
- Aplicação de console desacoplada da tecnologia de persistência, escolhida apenas na composição (`Program.cs`).
