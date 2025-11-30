# 🧩 Fase 10 — Cheiros e Antídotos (Refatorações com Diffs Pequenos)

Esta fase introduz o conceito de **códigos de alerta (code smells)** e seus **antídotos**, aplicados dentro do projeto do Seletor de Produtos.  
O foco é **refatorar com pequenas mudanças**, sem alterar comportamento — apenas melhorar clareza, desacoplamento e design.

---

## 🎯 Objetivos da Fase

- Identificar cheiros comuns em projetos C# reais.  
- Aplicar antídotos usando refatorações pequenas e seguras.  
- Manter testes passando durante todo o processo.  
- Extrair responsabilidades, melhorar nomes e reduzir acoplamentos.  
- Praticar leitura e escrita de *diffs pequenos*.  

---

## 🧪 Cheiros detectados no projeto e antídotos aplicados

Abaixo estão os cheiros reais encontrados no seu projeto **e as refatorações já aplicadas no código**.

### 1) Code Smell: `ProdutoService` com dupla responsabilidade
- **Cheiro:** misturava CRUD e seleção de produto (duas responsabilidades).
- **Antídoto aplicado:** criamos `ProdutoSelecaoService`. Agora `ProdutoService` contém **somente CRUD**; toda a lógica de seleção foi movida para `ProdutoSelecaoService`.

**Diff resumido**:
```diff
- public static Produto ExecutarSelecao(IReadRepository<Produto, int> repo, string modo)
+ // remova o método de seleção do ProdutoService
+ // e crie ProdutoSelecaoService com método Selecionar(repo, ModoSelecao modo)
```

---

### 2) Code Smell: uso de strings literais para modos de seleção
- **Cheiro:** chamadas como `ProdutoService.ExecutarSelecao(repo, "QUALIDADE")` eram frágeis e sujeitas a erro de digitação.
- **Antídoto aplicado:** introduzimos um `enum` chamado `ModoSelecao` e passamos o enum nos lugares necessários.

**Enum criado**:
```csharp
namespace Domain.Entities.Seletores;

public enum ModoSelecao
{
    Economico,
    Premium,
    Qualidade
}
```

**Exemplo de uso (antes/depois)**
```diff
- var melhor = ProdutoService.ExecutarSelecao(repo, "QUALIDADE");
+ var selecao = new ProdutoSelecaoService();
+ var melhor = selecao.Selecionar(repo, ModoSelecao.Qualidade);
```

---

### 3) Code Smell: Factory baseada em switch/string
- **Cheiro:** `SeletorFactory` usava `switch` em strings — código frágil e pouco extensível.
- **Antídoto aplicado:** refatoramos a factory para usar um `Dictionary<ModoSelecao, Func<ISeletorDeProduto>>` ou `Dictionary<ModoSelecao, ISeletorDeProduto>` estático.

**Difusão aplicada**:
```diff
- public static ISeletorDeProduto Criar(string modo) { switch (modo.ToUpper()) { ... } }
+ private static readonly Dictionary<ModoSelecao, Func<ISeletorDeProduto>> _map = new() { ... };
+ public static ISeletorDeProduto Criar(ModoSelecao modo) => _map[modo]();
```

Esta mudança remove strings mágicas e facilita adicionar novos seletores sem tocar a factory (só registrar no dicionário).

---

### 4) Code Smell: cálculo de backoff duplicado no `PumpService`
- **Cheiro:** o cálculo exponencial do backoff estava inline dentro do loop, repetido e menos legível.
- **Antídoto aplicado:** extraímos um método privado `ProximoBackoff(TimeSpan atual)` e substituímos o cálculo inline por chamadas a esse método.

**Antes**
```csharp
backoff = TimeSpan.FromMilliseconds(backoff.TotalMilliseconds * BackoffFactor);
```

**Depois**
```csharp
backoff = ProximoBackoff(backoff);

private TimeSpan ProximoBackoff(TimeSpan atual)
    => TimeSpan.FromMilliseconds(atual.TotalMilliseconds * BackoffFactor);
```

Isso melhora legibilidade e facilita testes do comportamento de backoff se necessário.

---

### 5) Code Smell: Program.cs fazia lógica de seleção diretamente
- **Cheiro:** execução do fluxo no `Program.cs` chamava métodos estáticos com strings.
- **Antídoto aplicado:** Program agora usa `ProdutoSelecaoService` + `ModoSelecao` (enum) para delegar responsabilidades, deixando o entrypoint mais legível.

**Exemplo**
```diff
- var melhor = ProdutoService.ExecutarSelecao(leitor, "QUALIDADE");
+ var selecao = new ProdutoSelecaoService();
+ var melhor = selecao.Selecionar(leitor, ModoSelecao.Qualidade);
```

---

## 🛠 Lista de arquivos criados/modificados na Fase 10

### Criados
- `Domain.Entities/Service/ProdutoSelecaoService.cs` — serviço especializado em seleção.
- `Domain.Entities/Seletores/ModoSelecao.cs` — enum com modos de seleção.

### Modificados
- `Domain.Entities/Service/ProdutoService.cs` — remoção do método de seleção (agora só CRUD).
- `Domain.Entities/Seletores/SeletorFactory.cs` — refatorada para usar enum + dicionário.
- `Domain.Entities/Service/PumpService.cs` — extração do método `ProximoBackoff` e uso dele no loop.
- `Domain.App/Program.cs` — atualizado para usar `ProdutoSelecaoService` e `ModoSelecao`.

---

## 🧪 Testes — o que mudar (recomendações)

### Obrigatório: **nenhum teste novo é necessário**
A Fase 10 é refatoração. Os testes existentes **devem continuar passando**. Se algum teste quebrar, indica que a refatoração alterou comportamento — o que não é desejado.

### Opcionais (recomendados para maior cobertura)
- **ProdutoSelecaoServiceTests.cs** — testar se a seleção retorna o produto esperado para cada `ModoSelecao` (econômico/premium/qualidade).
- **SeletorFactoryTests.cs** — verificar que `SeletorFactory.Criar(ModoSelecao.X)` retorna o tipo correto.
- **PumpService backoff test** — isolar `ProximoBackoff` ou testar comportamento de avanço do relógio com dublês.

Exemplo de teste para `ProdutoSelecaoService`:
```csharp
[Fact]
public void Selecionar_deve_retornar_de_acordo_com_modo()
{
    var repo = new InMemoryRepository<Produto,int>(p => p.Id);
    repo.Add(new Produto(1, "A", 100, 50));
    repo.Add(new Produto(2, "B", 200, 90));

    var service = new ProdutoSelecaoService();
    var res = service.Selecionar(repo, ModoSelecao.Qualidade);

    Assert.Equal("B", res.Nome);
}
```

---

## 📁 Estrutura final desejada da Fase 10

```
fase-10-Cheiros-E-Antidotos/
│
├── Domain.App/
│   └── Program.cs   (atualizado)
│
├── Domain.Entities/
│   ├── Models/
│   ├── Repository/
│   ├── Seletores/
│   │   ├── ModoSelecao.cs
│   │   └── SeletorFactory.cs (com dicionário)
│   ├── Service/
│   │   ├── ProdutoService.cs (só CRUD)
│   │   ├── ProdutoSelecaoService.cs (novo)
│   │   └── PumpService.cs (com ProximoBackoff)
│   └── Doubles/
│
├── Domain.Tests/
│   (testes existentes devem continuar funcionando)
│
└── docs/
    └── DESCRICAO.md   (este arquivo)
```

---

## ✔ Conclusão

As refatorações aplicadas nesta fase são **pequenas, seguras e reversíveis** — exatamente o que o exercício exige. Depois dessas mudanças, o projeto:

- Está mais preparado para **DIP/DI (Fase 11)**;  
- Tem menos strings mágicas e menos pontos frágeis;  
- Tem responsabilidades melhor separadas;  
- Mantém testes e comportamento originais. 
