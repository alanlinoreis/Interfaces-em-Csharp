# Diagrama de Arquitetura — Fase 12

```text
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
| (CRUD + Export + Import +  |                | (Seleciona via enum         |
| Busca + StreamAsync)       |                |  ModoSelecao + Factory)     |
+---------------+------------+                +---------------+-------------+
                |                                             |
                v                                             |
   +-------------------------------+                          |
   | IReadRepository / IWriteRepo |<--------------------------+
   |     (contratos ISP)          |
   +---------------+--------------+
                   |
         +---------+-----------------------------+
         |                                       |
         v                                       v
+------------------+                   +-----------------------+
| InMemoryRepo     |                   | JsonProdutoRepository |
| (Testes)         |                   | (Persistência JSON)   |
+------------------+                   +-----------------------+
                                                |
                                                |
                                                v
                                       +------------------------+
                                       |      Domain.Data       |
                                       |  (Camada de Banco)     |
                                       +-----------+------------+
                                                   |
                 +---------------------------------+------------------------+
                 |                                                          |
                 v                                                          v
        +-----------------------+                                 +---------------------------+
        |   CatalogoDbContext   |                                 |  SqliteProdutoRepository  |
        |  (EF Core + SQLite)   |                                 | (Repo baseado em EF Core)|
        +-----------------------+                                 +---------------------------+
                 |
                 v
        +-----------------------+
        |   catalogo.db         |
        | (arquivo SQLite)      |
        +-----------------------+