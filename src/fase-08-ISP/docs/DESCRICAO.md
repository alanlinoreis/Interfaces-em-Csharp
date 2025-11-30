# 🧩 Fase 08 — ISP (Interface Segregation Principle)

Nesta fase evoluímos o projeto aplicando o **Princípio da Segregação de Interfaces (ISP)**, um dos 5 princípios do SOLID.  
O objetivo é **eliminar interfaces grandes e genéricas**, dividindo-as em contratos menores e mais específicos.

Esta fase mantém integralmente tudo que foi feito até a Fase 07  
(seletores, serviços, arquivos JSON, testes), mas **divide o repositório em duas interfaces menores**.

---

# 🎯 Objetivo da Fase
Aplicar o ISP separando o contrato de persistência em **dois contratos especializados**:

- `IReadRepository<T, TId>` → somente leitura  
- `IWriteRepository<T, TId>` → somente escrita  

Antes, tínhamos apenas:

```csharp
IRepository<T,TId>  // grande e genérica
```

Agora, esse contrato é dividido para reduzir acoplamento.

---

# 🧱 Estrutura da Fase 08

```
src/fase-08-ISP/
│
├── Domain.Entities/
│   ├── Models/
│   ├── Seletores/
│   ├── Service/
│   └── Repository/
│       ├── IReadRepository.cs
│       ├── IWriteRepository.cs
│       ├── IRepository.cs
│       ├── InMemoryRepository.cs
│       └── JsonProdutoRepository.cs
│
├── Domain.App/
│   └── Program.cs
│
└── Domain.Tests/
    ├── ProdutoServiceTests.cs
    ├── ProdutoServiceSelecaoTests.cs
    └── (demais testes permanecem iguais)
```

---

# 🧠 Por que o ISP?

O problema do repositório anterior era que *todo método CRUD estava em uma única interface*:

```
Add
Update
Remove
ListAll
GetById
```

Mas o sistema tem métodos que **não precisam saber escrever**, como:

- Seletores  
- ProdutoService.ExecutarSelecao  
- Telas que só consultam dados  

Ou seja:

📌 *Um consumidor deveria depender apenas dos métodos que realmente usa.*

---

# 📝 Novos Contratos Criados

## 1️⃣ `IReadRepository<T, TId>`

```csharp
public interface IReadRepository<T, TId>
{
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
}
```

## 2️⃣ `IWriteRepository<T, TId>`

```csharp
public interface IWriteRepository<T, TId>
{
    T Add(T entity);
    bool Update(T entity);
    bool Remove(TId id);
}
```

## 3️⃣ `IRepository<T, TId>`

Para manter compatibilidade com fases anteriores:

```csharp
public interface IRepository<T, TId> :
    IReadRepository<T, TId>,
    IWriteRepository<T, TId>
{
}
```

---

# 🔧 Ajustes no ProdutoService

Antes:

```csharp
Criar(IRepository repo)
Listar(IRepository repo)
Atualizar(IRepository repo)
Remover(IRepository repo)
ExecutarSelecao(IRepository repo)
```

Depois (ISP):

```csharp
Criar(IWriteRepository repo)
Listar(IReadRepository repo)
Atualizar(IWriteRepository repo)
Remover(IWriteRepository repo)
ExecutarSelecao(IReadRepository repo)
```

Agora cada método depende **somente do necessário**.

---

# 💾 Repositórios Concretos

Tanto `InMemoryRepository` quanto `JsonProdutoRepository` implementam:

✔ IReadRepository  
✔ IWriteRepository  
✔ IRepository (total)

Nada muda neles — apenas passam a implementar as duas interfaces:

```csharp
public class JsonProdutoRepository :
    IReadRepository<Produto,int>,
    IWriteRepository<Produto,int>
```

---

# 🧪 Ajustes nos Testes

Apenas **2 testes** precisaram ser modificados:

### ✔ ProdutoServiceTests.cs

Criado:

```csharp
IReadRepository leitor = repo;
IWriteRepository escritor = repo;
```

### ✔ ProdutoServiceSelecaoTests.cs

Agora usa apenas `IReadRepository`.

Todos os outros testes continuam **idênticos**.

---

# 🖥 Ajustes no Program.cs

Separação entre leitura e escrita:

```csharp
IReadRepository<Produto,int> leitor = repo;
IWriteRepository<Produto,int> escritor = repo;
```

Seleção agora é:

```csharp
ProdutoService.ExecutarSelecao(leitor, "QUALIDADE");
```

---

# 📊 Diagrama — Antes vs Depois

```
ANTES (Fase 07)
--------------------------
    IRepository
   /    |     \
Add   List   Update
Remove  Get  etc.


DEPOIS (Fase 08 — ISP)
--------------------------

 IReadRepository        IWriteRepository
 -------------          ----------------
 GetById                Add
 ListAll                Update
                        Remove

 IRepository (herda dos dois)
```

---

# ✔ Conclusão da Fase 08

- O domínio agora usa **interfaces segregadas**  
- O serviço depende apenas do que realmente precisa  
- Traz mais clareza ao código  
- Permite futura substituição (ex: repositórios somente leitura)  
- Evita acoplamento desnecessário  
- Todas as fases anteriores continuam funcionando  
