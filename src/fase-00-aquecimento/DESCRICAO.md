# 🧩 Fase 00 — Aquecimento Conceitual  
## Projeto: Seletor de Produtos por Preço e Qualidade

### 🧩 Exemplo 1 — Seleção de Produto (nosso domínio principal)
- **Objetivo:** escolher o melhor produto disponível para compra.  
- **Contrato:** garantir que o produto selecionado equilibre preço e qualidade.  
- **Peças alternáveis:**  
  - **Modo Econômico:** sempre escolhe o produto mais barato.  
  - **Modo Premium:** escolhe produto de maior qualidade, desde que o preço não ultrapasse R$200 em relação ao mais barato.  
- **Política:** usar “Modo Premium” quando o orçamento permitir pagar até R$200 a mais por melhor qualidade; caso contrário, usar “Modo Econômico”.

---

### 🧩 Exemplo 2 — Seleção de Fornecedor
- **Objetivo:** escolher o fornecedor mais vantajoso para compra em larga escala.  
- **Contrato:** garantir fornecimento estável com custo aceitável.  
- **Peças alternáveis:**  
  - **Fornecedor A:** preço mais baixo, entrega mais lenta.  
  - **Fornecedor B:** preço um pouco maior (até 10%), mas entrega mais rápida e confiável.  
- **Política:** usar Fornecedor B em pedidos urgentes ou quando o prazo for crítico; caso contrário, usar Fornecedor A.
