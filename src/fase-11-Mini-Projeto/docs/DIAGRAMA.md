# Diagrama de Arquitetura — Fase 11

```
                        +-----------------------+
                        |      Program.cs       |
                        |  (Console / CLI)      |
                        +----------+------------+
                                   |
                                   v
                        +-----------------------+
                        |    COMPOSIÇÃO / DI    |
                        |  cria repositórios e  |
                        |  injeta nos serviços  |
                        +-----------+-----------+
                                    |
              +---------------------+----------------------+
              |                                            |
              v                                            v
+----------------------------+                +-----------------------------+
|     ProdutoService         |                |   ProdutoSelecaoService     |
| (CRUD + Export + Import +  |                | (Seleciona via enum +       |
| Busca + StreamAsync)       |                |  SeletorFactory)            |
+---------------+------------+                +---------------+-------------+
                |                                             |
                v                                             |
   +-------------------------------+                          |
   | IReadRepository / IWriteRepo |<--------------------------+
   |     (contratos ISP)          |
   +---------------+--------------+
                   |
         +---------+-----------+
         |                     |
         v                     v
+------------------+   +-----------------------+
| InMemoryRepo     |   | JsonProdutoRepository |
| (Testes)         |   | (Persistência real)   |
+------------------+   +-----------------------+
```

