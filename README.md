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

# 📁 Estrutura do Repositório (Atualizado até a Fase 09)

```
repo-raiz/
│
├── README.md
└── src/
    ├── fase-00-aquecimento/
    ├── fase-01-heuristica/
    ├── fase-02-procedural/
    ├── fase-03-oo-sem-interface/
    ├── fase-04-interface/
    ├── fase-05-Repository-In-Memory/
    ├── fase-06-Repository-CSV/
    ├── fase-07-Repository-Json/
    ├── fase-08-ISP/
    └── fase-09-Dubles-Async/
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

## 🧩 Fase 09 — **Dublês Avançados e Testes Assíncronos**
Nesta fase, três grandes evoluções:

### ✔️ 1. **API Assíncrona**
Novos contratos:
- `IAsyncReader<T>`
- `IAsyncWriter<T>`
- `IClock`

### ✔️ 2. **Dublês (Fakes) para Testes**
- `ReaderFake<T>` → gera itens assíncronos.
- `WriterFake<T>` → simula falhas configuráveis.
- `ClockFake` → avança tempo virtual para testar retry/backoff.

### ✔️ 3. **PumpService**
Novo serviço responsável por:
- consumir itens de um leitor assíncrono;
- escrever usando um writer assíncrono;
- aplicar retry configurável;
- aplicar backoff exponencial;
- honrar cancelamento (`CancellationToken`);
- usar relógio injetável (fake/real).

Testes cobrem:
- retry e recuperação;
- cálculo de backoff exponencial;
- cancelamento no meio do processo;
- escrita correta dos itens.

---

# ▶️ Como executar o projeto

Escolha a fase:

```
cd src/fase-*/Domain.App
dotnet run
```

---

# 🧪 Como rodar os testes

```
cd src/fase-*/Domain.Tests
dotnet test
```

---

# ✔️ Conclusão

Até a Fase 09, o projeto evoluiu de um simples procedural para um ecossistema:

- orientado a contratos;
- desacoplado;
- testável;
- com infraestrutura substituível;
- com dublês avançados;
- com operações assíncronas;
- pronto para um backend real (Fase 10).

A jornada demonstra **como projetos reais evoluem** através de camadas, princípios SOLID e testes consistentes.
