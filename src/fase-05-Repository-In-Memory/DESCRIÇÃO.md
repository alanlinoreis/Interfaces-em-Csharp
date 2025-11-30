# 🧩 Fase 05 — Repository In-Memory, Serviços de Domínio e Arquitetura Evolutiva  
## Projeto: Seletor de Produtos por Preço e Qualidade  

---

## 🎯 Objetivo  
Na fase 05 evoluímos o sistema para incluir:

- Um **Repository In-Memory** para manipular produtos sem depender de listas locais.  
- Uma camada **ProdutoService**, responsável por coordenar CRUD e seleção.  
- Domínio organizado em **Models / Repository / Service / Seletores**.  
- Testes unitários cobrindo Repository + Services + Seletores.  

Essa fase consolida acoplamento mínimo, testabilidade e arquitetura limpa.

---

# 🧱 Estrutura da Fase

```
src/fase-05-repository-inmemory/
    Domain.Entities/
        Models/
            Produto.cs
        Repository/
            IRepository.cs
            InMemoryRepository.cs
        Seletores/
            ISeletorDeProduto.cs
            SeletorEconomico.cs
            SeletorPremium.cs
            SeletorQualidade.cs
            SeletorFactory.cs
        Service/
            ProdutoService.cs

    Domain.App/
        Program.cs

    Domain.Tests/
        Repository/
            ProdutoRepositoryTests.cs
        Service/
            ProdutoServiceTests.cs
            ProdutoServiceSelecaoTests.cs
        Seletores/
            SeletorEconomicoTests.cs
            SeletorPremiumTests.cs
            SeletorQualidadeTests.cs
            SeletorFactoryTests.cs
```

---

# 🧩 1. `Domain.Entities` — Domínio Atualizado  

Aqui ficam todas as regras do domínio:

### ✔️ **Models**
- `Produto` como record imutável

### ✔️ **Repository**
Define um contrato genérico de persistência:

```csharp
public interface IRepository<T, TId>
```

E sua implementação:

```csharp
public sealed class InMemoryRepository<T,TId>
```

### ✔️ **Seletores**
Mesma lógica da fase 04, agora consumindo `IReadOnlyList<Produto>`.

### ✔️ **Service**
Nova camada que coordena:

- CRUD de produtos via Repository
- Execução dos seletores via `SeletorFactory`

---

# 🧩 2. `Domain.App` — Camada de Aplicação

Agora o `Program.cs` usa:

- `InMemoryRepository<Produto,int>`
- `ProdutoService`
- `SeletorFactory` (indiretamente)

Demonstra:

- CRUD completo  
- Seleção econômica, premium, qualidade  
- Casos de erro (id inexistente)  

---

# 🧪 3. `Domain.Tests` — Testes Unitários da Fase 05  

A fase 05 exige novos testes além dos seletores:

### ✔️ Repository
- Add  
- ListAll  
- GetById existente / inexistente  
- Update existente / inexistente  
- Remove existente / inexistente  

### ✔️ ProdutoService
- Criar  
- Listar  
- Remover  
- Integração com seletores  

### ✔️ Seletores (mantidos da fase 04)
Agora usando a nova assinatura com `IReadOnlyList<Produto>`.

---

# 📄 Exemplo de Teste — Repository

```csharp
[Fact]
public void Add_deve_inserir_produto()
{
    var repo = new InMemoryRepository<Produto,int>(p => p.Id);
    repo.Add(new Produto(1,"TV",2500,9));

    var encontrado = repo.GetById(1);

    Assert.NotNull(encontrado);
    Assert.Equal("TV", encontrado!.Nome);
}
```

---

# ▶️ Como executar o projeto

Dentro de:

```
src/fase-05-repository-inmemory/Domain.App
```

Rode:

```
dotnet run
```

---

# 🧪 Como rodar os testes

```
src/fase-05-repository-inmemory/Domain.Tests
```

Execute:

```
dotnet test
```

---

# ✔️ Conclusão da Fase 05  

A Fase 05 introduz arquitetura limpa:

- domínio centrado  
- repositório plugável  
- serviços desacoplados  
- testabilidade total  
- organização profissional em pastas  
- evolução natural da Fase 04  

Agora o sistema está pronto para futuras fases como:

- ISP (Interface Segregation Principle)  
- Implementações de repositório com banco  
- API ou UI real  

---
