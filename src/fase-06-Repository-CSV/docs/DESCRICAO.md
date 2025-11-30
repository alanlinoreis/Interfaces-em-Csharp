# 🧩 FASE 06 — Repository CSV
### Persistência simples em arquivo, mantendo o contrato da Fase 05

---

# 🎯 Objetivo da Fase

Na Fase 05, tivermos um **Repository InMemory**, onde os dados só existiam durante a execução.

Na Fase 06 evoluímos para um repositório **persistido em arquivo CSV**, mantendo:

- o **mesmo contrato** (`IRepository<T, TId>`)
- a compatibilidade total com:
  - `Produto`
  - `ProdutoService`
  - Seletores (`SeletorEconomico`, `Premium`, `Qualidade`)
- separação clara entre camadas
- testes unitários reais, manipulando arquivo temporário

O domínio **não muda**.  
A única novidade é **a infraestrutura** → agora usando CSV.

---

# 🧱 Estrutura da Fase 06 (exatamente como no projeto)

```
fase-06-Repository-CSV/
│
├── Domain.App/
│   ├── produtos.csv
│   └── Program.cs
│
├── Domain.Entities/
│   ├── Models/
│   │   └── Produto.cs
│   │
│   ├── Repository/
│   │   ├── CsvProdutoRepository.cs
│   │   ├── InMemoryRepository.cs
│   │   └── IRepository.cs
│   │
│   ├── Seletores/
│   │   ├── ISeletorDeProduto.cs
│   │   ├── SeletorEconomico.cs
│   │   ├── SeletorFactory.cs
│   │   ├── SeletorPremium.cs
│   │   └── SeletorQualidade.cs
│   │
│   ├── Service/
│   │   └── ProdutoService.cs
│   │
│   └── Domain.Entities.csproj
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
├── docs/
│   ├── DESCRICAO.md
│   └── Program.cs
```

---

# 🧠 1. O contrato permanece o mesmo (IRepository)

```csharp
public interface IRepository<T, TId>
{
    T Add(T entity);
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
    bool Update(T entity);
    bool Remove(TId id);
}
```

Nenhuma mudança na API.  
Somente a implementação é nova.

---

# 📄 2. CsvProdutoRepository (novo repositório CSV)

Ele implementa:

- leitura do CSV
- criação de arquivo se não existir
- atualização de registros
- remoção
- suporte a vírgulas e aspas
- persistência real no disco

Funciona lado a lado com o `InMemoryRepository` (que ainda está no projeto).

---

# ⚙️ 3. ProdutoService continua igual

Assim como na Fase 05, ele recebe qualquer repository:

```csharp
ProdutoService.ExecutarSelecao(repo, "ECONOMICO");
```

O serviço não sabe qual infraestrutura está sendo usada (InMemory ou CSV).  
Esse é o objetivo da abstração.

---

# 🖥️ 4. Program.cs atualizado

Demonstra:

- inicialização do CSV se não existir
- listagem de produtos
- testes dos seletores
- CRUD completo
- estrutura sem top-level statements

Exemplo:

```csharp
var repo = new CsvProdutoRepository("produtos.csv");
ListarProdutos(repo);
TestarSeletores(repo);
TestarCrud(repo);
```

---

# 🧪 5. Testes Unitários da Fase 06

Todos os testes são executados em:

```
Domain.Tests/
```

Os arquivos relevantes:

- ✔ `CsvProdutoRepositoryTests.cs`  ← principal da fase 06
- Demais testes da fase 05 continuam funcionando

Os testes garantem:

- arquivo inexistente → retorna vazio
- arquivo vazio → retorna vazio
- inserir produto
- buscar por Id
- atualizar
- remover
- textos com vírgulas
- textos com aspas
- integração com ProdutoService

Todos usando arquivos temporários:

```csharp
var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
```

Assim o teste **nunca interfere no Program** ou no `produtos.csv` real.

---

# 🧩 6. Diagrama da Arquitetura (Baseado no seu projeto)

```
         +------------------------+
         |       Program.cs       |
         +-----------+------------+
                     |
                     v
         +-----------+------------+
         |     ProdutoService     |
         +-----------+------------+
                     |
                     v
     +---------------+----------------+
     |   IRepository<Produto, int>    |
     +-------+---------------+--------+
             |               |
             v               v
   +----------------+   +------------------+
   | InMemoryRepo   |   | CsvProdutoRepo   |
   +----------------+   +------------------+
                             |
                             v
                     produtos.csv
```

---

# ✔️ Conclusão da Fase 06

A fase 06 introduce:

- Persistência real
- Repositório alternativo ao InMemory
- Testes cobrindo todos os fluxos
- Program completo com CRUD + seletores
- Infraestrutura substituível sem alterar domínio

Você agora tem um projeto:

- modular  
- extensível  
- testável  
- com baixo acoplamento  
- pronto para evoluir para banco real (fase 07)
