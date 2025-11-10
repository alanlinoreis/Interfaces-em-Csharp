# 🧩 Fase 01 — Heurística Antes do Código (Mapa Mental)  
## Projeto: Seletor de Produtos por Preço e Qualidade  

---

### 💡 Problema escolhido
Queremos permitir que o sistema escolha automaticamente o **melhor produto** considerando **preço e qualidade**, aplicando uma regra simples de seleção que evite escolhas ruins ou muito caras.

---

### 🧮 Quadro 1 — Procedural (onde surgem if/switch)
* Fluxo: recebe lista de produtos → encontra o mais barato → compara qualidade →  
  `if (qualidadeMelhor && preço - maisBarato <= 200)` → escolhe produto “premium”; senão → escolhe o mais barato.  
* Decisões embutidas diretamente no fluxo (várias comparações e condições aninhadas).  
* Sinais de dor: a cada novo critério (ex.: durabilidade, marca, entrega) surgem novos `if/switch`,  
  tornando o código rígido e difícil de testar.  

---

### 🧱 Quadro 2 — OO sem interface (quem encapsula o quê; o que ainda fica rígido)
* Encapsulamos a lógica de decisão em classes como `Produto` e `SeletorDeProduto`.  
* O `SeletorDeProduto` centraliza as regras e executa o fluxo de seleção.  
* Melhoras: maior coesão — cada classe tem papel claro (dados vs decisão).  
* Rigidez remanescente: o **cliente ainda conhece** o modo de seleção (“econômico” ou “premium”);  
  para alterar a política, o código cliente ainda precisa mudar.  

---

### 🧩 Quadro 3 — Com interface (contrato que permite alternar + ponto de composição)
* **Contrato (o que):** selecionar o melhor produto (`ISelecaoDeProduto`).  
* **Implementações (como):** `ModoEconomico` e `ModoPremium`.  
* **Ponto de composição:** a política de escolha (“premium se até R$200 acima do mais barato”) é definida fora do cliente.O cliente apenas consome a interface, sem conhecer as classes concretas.  
* **Efeito:** é possível alternar a regra de seleção sem alterar o cliente; testes se tornam simples,  
  podendo injetar dublês de seleção.  

---

### ⚠️ Três sinais de alerta previstos
1. O **cliente muda** sempre que trocamos o modo de seleção (acoplamento ao “como”).  
2. **Ramificações** (`if/switch`) começam a se espalhar por mais de um ponto do código.  
3. **Testes frágeis e lentos** se baseiam em listas reais de produtos em vez de mocks/dublês.  
