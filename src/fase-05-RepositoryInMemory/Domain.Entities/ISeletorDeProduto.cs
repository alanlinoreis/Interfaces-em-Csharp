using System.Collections.Generic;

namespace Domain.Entities
{
    public interface ISeletorDeProduto
    {
        Produto Selecionar(List<Produto> produtos);
    }
}
