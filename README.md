# 🧱 Projeto — Seletor de Produtos por Preço e Qualidade
**Atividade: Tarefa por Fases — Interfaces em C#**

---

## 👥 Equipe

| Integrante | RA |
|-----------|----|
| **Alan Lino dos Reis** | a2724332 |
| **Pedro Lucas Reis** | a2716020 |
| **Pedro Gabriel Sepulveda Borgheti** | a2706059 |

---

# 📁 Estrutura Geral do Repositório (Atualizada até a Fase 11)

Cada fase possui:
- sua própria aplicação (`Domain.App`)
- suas próprias entidades (`Domain.Entities`)
- seus próprios testes (`Domain.Tests`)

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
└── fase-11-Mini-Projeto/
    └── src/
        ├── Domain.App/
        ├── Domain.Entities/
        └── Domain.Tests/
```

---

# 📦 Conteúdo da Fase 11

A seguir estão **todas as pastas e arquivos reais da fase 11**, conforme o projeto.

---

# 📁 Domain.Entities (Fase 11)

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

# 📁 Domain.App (Fase 11)

```
Domain.App/
├── Program.cs
└── produtos.json
```

*Program.cs contém:*
- Menu completo (CRUD, seleção, export, import, stream async)
- Composição explícita (repo JSON → leitor/escritor)
- Validações de entrada

---

# 📁 Domain.Tests (Fase 11)

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
└── SeletorFactoryTests.cs
```

Esses testes cobrem:
- Persistência JSON
- Repositórios em memória
- Seletores
- ProdutoService completo (CRUD + filtros + import/export + async)
- PumpService e dublês

---

# ▶️ Como executar qualquer fase

```
cd src/fase-XX-*/src/Domain.App
dotnet run
```

Exemplo:

```
cd src/fase-11-Mini-Projeto/src/Domain.App
dotnet run
```

---

# 🧪 Como rodar testes de qualquer fase

```
cd src/fase-XX-*/src/Domain.Tests
dotnet test
```

Exemplo:

```
cd src/fase-11-Mini-Projeto/src/Domain.Tests
dotnet test
```

---

# ✔️ Conclusão

O projeto evoluiu fase a fase aplicando:

- Princípios de design (ISP, SRP, DIP)
- Interfaces e polimorfismo
- Repository Pattern (InMemory + JSON)
- Testes unitários e doubles
- Persistência real em JSON
- Operações assíncronas com IAsyncEnumerable
- Arquitetura modular e separada por fases

A Fase 11 consolida tudo em um sistema completo, funcional, testável e bem documentado.

