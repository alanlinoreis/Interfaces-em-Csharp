# 🧱 Arquitetura — Fase 11 (Mini-projeto de Consolidação)

**Domínio:** Catálogo de Produtos  
**Versão:** Fase 11  

---

# 📌 Objetivo da Arquitetura
Consolidar todos os padrões estudados:
- Repository Pattern (InMemory + JSON)
- ISP (IReadRepository / IWriteRepository)
- Serviços focados (CRUD, Seleção)
- CLI com composição explícita
- Testes unitários + integração
- Fluxos assíncronos (IAsyncEnumerable)

---

# 🧠 Componentes Principais

### **Models**
- `Produto`

### **Contratos**
- `IReadRepository<T,TId>`
- `IWriteRepository<T,TId>`

### **Repositórios**
- `InMemoryRepository`
- `JsonProdutoRepository`

### **Serviços**
- `ProdutoService` (CRUD + export/import + filtros)
- `ProdutoSelecaoService` (enum + seletores)

### **Seletores**
- `SeletorEconomico`
- `SeletorPremium`
- `SeletorQualidade`
- `SeletorFactory`

### **Aplicação**
- `Program.cs` → Menu interativo com CRUD, seleção, export/import, stream async

---

# 🖼 Diagrama Geral
Arquivo separado: **DIAGRAMA.md**

---

# 🧪 Testes
- Unitários: usando InMemoryRepository
- Integração: export/import JSON
- Async: stream de produtos

---

# ✔ Conclusão
Arquitetura está modular, testável, extensível e sólida para Fase 11.
