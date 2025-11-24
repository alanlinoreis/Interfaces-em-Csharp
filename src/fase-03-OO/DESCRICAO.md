
# 🧩 Fase 03 — OO Sem Interface  
## Projeto: Seletor de Produtos por Preço e Qualidade  

---

## 🎯 Objetivo  
Transformar a solução procedural da fase 2 em uma abordagem orientada a objetos, utilizando **herança + polimorfismo**, sem uso de interfaces.  
O foco é separar o comportamento variável em subclasses, mantendo o fluxo comum na classe base.

---

## 🧱 Estrutura de Classes

### **1. Classe Base — `SeletorBase`**
- Contém o fluxo comum de seleção:
  - Encontrar o produto mais barato
  - Encontrar o produto de maior qualidade
- Expõe o método `Selecionar`, responsável por orquestrar o processo.
- Delegação do comportamento variável via método abstrato:
  ```csharp
  protected abstract Produto SelecionarInterno(...);
  ```

---

### **2. Implementações Concretas**
Cada variação do modo de seleção possui sua própria classe:

- **`SeletorEconomico`**  
  Implementa o modo que sempre escolhe o produto mais barato.

- **`SeletorPremium`**  
  Escolhe o produto com maior qualidade, desde que não custe mais que R$200 acima do mais barato.  
  Caso contrário, usa o econômico.

- **`SeletorQualidade`**  
  Retorna exclusivamente o produto de melhor qualidade, ignorando o preço.

---

## 🔍 Melhorias obtidas
- Remoção de if/switch do cliente.
- Fluxo comum centralizado na classe base.
- Cada comportamento é isolado em sua própria classe (maior coesão).
- Facilita manutenção e testes de cada variação.

---

## ⚠️ O que ainda está rígido
- Cliente ainda conhece as classes concretas (`new SeletorPremium()`).
- Não existe contrato único para consumo — isso só será resolvido na Fase 4.
- O ponto de composição ainda está espalhado (política fora do cliente não existe aqui).

---

## 🧪 Exemplo de Uso (cliente)
```csharp
SeletorBase seletor;

seletor = new SeletorEconomico();
var resultado1 = seletor.Selecionar(produtos);

seletor = new SeletorPremium();
var resultado2 = seletor.Selecionar(produtos);

seletor = new SeletorQualidade();
var resultado3 = seletor.Selecionar(produtos);
```

---

## 📌 Conclusão da Fase 3
O código passa a contar com:
- Polimorfismo
- Especialização por subclasses
- Fluxo comum claro
- Redução de duplicação

Mas ainda mantém:
- Acoplamento do cliente às implementações concretas  
👉 Isso será resolvido totalmente na **Fase 4**, com uso de **interfaces** e **injeção**.

