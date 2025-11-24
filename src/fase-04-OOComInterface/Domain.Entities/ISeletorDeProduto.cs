using System.Collections.Generic;

namespace Fase04.Domain.Entities
{
    public interface ISeletorDeProduto
    {
        Produto Selecionar(List<Produto> produtos);
    }
}
