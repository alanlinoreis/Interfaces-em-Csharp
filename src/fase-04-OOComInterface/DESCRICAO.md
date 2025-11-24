
# 🧩 Fase 04 — Interface Plugável, Testável e Organizada por Domínio  
## Projeto: Seletor de Produtos por Preço e Qualidade  

---

## 🎯 Objetivo  
Nesta fase introduzimos um **contrato (interface)** para permitir alternância de implementações sem alterar o cliente.  
Também organizamos o código em **projetos separados**:

- `domain.entities` → regras de negócio e entidades  
- `domain.app` → camada de aplicação (tela)  
- `domain.tests` → testes unitários baseados em interface  

Tudo agora respeita o princípio de **baixo acoplamento**.

---

# 🧱 Estrutura da Fase

```
src/fase-04-interface/
    domain.entities/
        Produto.cs
        ISeletorDeProduto.cs
        SeletorEconomico.cs
        SeletorPremium.cs
        SeletorQualidade.cs
        SeletorFactory.cs

    domain.app/
        Program.cs

    domain.tests/
        SeletorEconomicoTests.cs
        SeletorPremiumTests.cs
        SeletorQualidadeTests.cs
        SeletorFactoryTests.cs
```

---

# 🧩 1. `domain.entities` — Domínio  

Armazena:

- Entidade **Produto**
- Interface **ISeletorDeProduto**
- Implementações concretas:
  - **SeletorEconomico**
  - **SeletorPremium**
  - **SeletorQualidade**
- **SeletorFactory**, responsável por criar instâncias conforme modo

Nada aqui depende da camada de aplicação ou testes.

---

# 🧩 2. `domain.app` — Camada de Aplicação

O `Program.cs` usa apenas:

- `ISeletorDeProduto`
- `SeletorFactory`

Nunca utiliza classes concretas diretamente.

Exemplo:

```csharp
var seletor = SeletorFactory.Criar("PREMIUM");
var resultado = seletor.Selecionar(produtos);
```

Se amanhã mudar a implementação, o `Program.cs` não sofre alteração.

---

# 🧪 3. `domain.tests` — Testes Unitários  
Os testes verificam comportamento baseado no contrato **ISeletorDeProduto**, validando cada implementação concreta separadamente.

Abaixo estão os arquivos exatamente como o projeto deve conter.

---

## ✔️ SeletorEconomicoTests.cs

```csharp
using Xunit;
using System.Collections.Generic;
using Fase04.Domain.Entities;

namespace Fase04.Domain.Tests
{
    public class SeletorEconomicoTests
    {
        [Fact]
        public void Deve_retornar_o_produto_mais_barato()
        {
            var produtos = new List<Produto>
            {
                new Produto("A", 200, 50),
                new Produto("B", 100, 70),
                new Produto("C", 150, 60)
            };

            var seletor = new SeletorEconomico();
            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("B", resultado.Nome);
        }
    }
}
```

---

## ✔️ SeletorPremiumTests.cs

```csharp
using Xunit;
using System.Collections.Generic;
using Fase04.Domain.Entities;

namespace Fase04.Domain.Tests
{
    public class SeletorPremiumTests
    {
        [Fact]
        public void Deve_escolher_melhor_qualidade_quando_dentro_do_limite()
        {
            var produtos = new List<Produto>
            {
                new Produto("Barato", 100, 50),
                new Produto("Qualidade", 250, 90)
            };

            var seletor = new SeletorPremium();
            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("Qualidade", resultado.Nome);
        }

        [Fact]
        public void Deve_voltar_para_economico_quando_acima_do_limite()
        {
            var produtos = new List<Produto>
            {
                new Produto("Barato", 100, 50),
                new Produto("Qualidade", 400, 90)
            };

            var seletor = new SeletorPremium();
            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("Barato", resultado.Nome);
        }
    }
}
```

---

## ✔️ SeletorQualidadeTests.cs

```csharp
using Xunit;
using System.Collections.Generic;
using Fase04.Domain.Entities;

namespace Fase04.Domain.Tests
{
    public class SeletorQualidadeTests
    {
        [Fact]
        public void Deve_retornar_o_produto_de_maior_qualidade()
        {
            var produtos = new List<Produto>
            {
                new Produto("A", 200, 50),
                new Produto("B", 180, 80),
                new Produto("C", 150, 60)
            };

            var seletor = new SeletorQualidade();
            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("B", resultado.Nome);
        }
    }
}
```

---

## ✔️ SeletorFactoryTests.cs

```csharp
using Xunit;
using Fase04.Domain.Entities;

namespace Fase04.Domain.Tests
{
    public class SeletorFactoryTests
    {
        [Fact]
        public void Deve_retornar_Economico_para_modo_invalido()
        {
            var seletor = SeletorFactory.Criar("qualquercoisa");
            Assert.IsType<SeletorEconomico>(seletor);
        }

        [Fact]
        public void Deve_retornar_Premium_quando_modo_premium()
        {
            var seletor = SeletorFactory.Criar("PREMIUM");
            Assert.IsType<SeletorPremium>(seletor);
        }
    }
}
```

---

# 🧰 Dependências necessárias (xUnit)

No `.csproj` de testes:

```xml
<ItemGroup>
  <PackageReference Include="xunit" Version="2.5.0" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.5.0" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\domain.entities\Fase04.Domain.Entities.csproj" />
</ItemGroup>
```

---

# ✔️ Conclusão da Fase 04  
Com o domínio separado da aplicação e com a interface como contrato único:

- O app depende apenas de **ISeletorDeProduto**  
- As implementações ficam isoladas  
- O SeletorFactory centraliza o ponto de composição  
- Os testes cobrem cada comportamento de forma independente  

A próxima fase (Fase 05) aprofunda os fundamentos de interfaces antes de avançar para **ISP (Fase 06)**.

