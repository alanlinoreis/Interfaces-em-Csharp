# 🧩 Fase 09 — Dublês Avançados e Testes Assíncronos  
## Repository Assíncrono + Retry + Backoff + Cancelamento

---

## 🎯 Objetivo da Fase

A Fase 09 expande o projeto evolutivo introduzindo três pilares fundamentais:

1. **APIs assíncronas (async/await)** aplicadas a leitura e escrita;  
2. **Dublês avançados** (ReaderFake, WriterFake, ClockFake);  
3. **PumpService**, um orquestrador robusto suportando retry, backoff exponencial e cancelamento.

Diferente das fases anteriores, aqui não alteramos o domínio “Produto” — toda a evolução acontece na **infraestrutura assíncrona**.

---

# 📁 Estrutura Completa da Fase 09

Abaixo está a estrutura **completa**, incluindo tudo herdado e tudo novo:

```
fase-09/
│
├── Domain.App/
│   ├── Program.cs
│   └── produtos.json
│
├── Domain.Entities/
│   ├── Models/
│   │   └── Produto.cs
│   │
│   ├── Repository/
│   │   ├── IRepository.cs
│   │   ├── InMemoryRepository.cs
│   │   ├── CsvProdutoRepository.cs
│   │   └── JsonProdutoRepository.cs
│   │
│   ├── Seletores/
│   │   ├── ISeletorDeProduto.cs
│   │   ├── SeletorEconomico.cs
│   │   ├── SeletorPremium.cs
│   │   ├── SeletorQualidade.cs
│   │   └── SeletorFactory.cs
│   │
│   ├── Service/
│   │   └── ProdutoService.cs
│   │
│   ├── Contracts/
│   │   ├── IAsyncReader.cs
│   │   ├── IAsyncWriter.cs
│   │   ├── IClock.cs
│   │   └── IIdGenerator.cs
│   │
│   ├── Doubles/
│   │   ├── ReaderFake.cs
│   │   ├── WriterFake.cs
│   │   └── ClockFake.cs
│   │
│   └── Services/
│       └── PumpService.cs
│
├── Domain.Tests/
│   ├── ProdutoRepositoryTests.cs
│   ├── ProdutoServiceTests.cs
│   ├── ProdutoServiceSelecaoTests.cs
│   ├── SeletorEconomicoTests.cs
│   ├── SeletorPremiumTests.cs
│   ├── SeletorQualidadeTests.cs
│   ├── SeletorFactoryTests.cs
│   └── PumpServiceTests.cs
│
└── docs/
    └── DESCRICAO.md
```

---

# 🧱 1. Contratos introduzidos na Fase 09

### **IAsyncReader<T>**
Suporte a leitura assíncrona:

```csharp
Task<T?> ReadAsync(CancellationToken ct = default);
IAsyncEnumerable<T> ReadAllAsync(CancellationToken ct = default);
```

### **IAsyncWriter<T>**
Escrita assíncrona:

```csharp
Task WriteAsync(T item, CancellationToken ct = default);
```

### **IClock**
Relógio virtual usado para testes:

```csharp
DateTime Now { get; }
Task Delay(TimeSpan delay, CancellationToken ct = default);
```

### **IIdGenerator**
Contrato simples ilustrando ISP:

```csharp
string NewId();
```

---

# 🧪 2. Dublês criados (Fake Objects)

## **ReaderFake<T>**
Simula um stream assíncrono:
- Retorna itens pré-definidos  
- Pode lançar exceção na próxima leitura  

## **WriterFake<T>**
Simula escrita assíncrona:
- Registra itens escritos  
- Pode falhar para testar retry  

## **ClockFake**
Simula o tempo:
- Avança o relógio sem esperar  
- Permite testar backoff em 0 ms  
- Suporta cancelamento  

---

# ⚙️ 3. PumpService — Retry, Backoff, Cancelamento

O PumpService é o orquestrador assíncrono central da fase:

- lê itens via `IAsyncReader<T>`
- escreve itens via `IAsyncWriter<T>`
- faz retry com backoff exponencial
- interrompe se cancelado
- usa `IClock` para tempos controlados

Fluxo:

```
Read item
   ↓
WriteAsync
   ↓ sucesso?
      → sim → próximo item
      → não → Retry + Delay(backoff)
                backoff *= fator
                se exceder MaxRetries → throw
```

---

# 🧪 4. Testes Unitários — PumpServiceTests.cs

O arquivo cobre:

### ✔ Fluxo normal  
Itens são lidos/escritos sem retry.

### ✔ Retry com backoff exponencial  
WriterFake falha → PumpService tenta novamente.

### ✔ Avanço do relógio virtual  
ClockFake simula as esperas do retry.

### ✔ Cancelamento  
CancellationToken interrompe o processo durante o backoff.

### ✔ Falha permanente  
Mesmo após retries, lança exceção.

---

# ▶️ 5. Como rodar

### App
```
dotnet run
```

### Testes
```
dotnet test
```

---

# ✔️ Conclusão

A Fase 09 consolida:
- domínio do async/await  
- testes determinísticos com dublês  
- controle avançado de fluxo (retry, backoff, cancelamento)  
- zero dependência de tempo real ou I/O real  

Essa base permite evoluir para integrações externas, mensageria, filas, pipelines e muito mais.
