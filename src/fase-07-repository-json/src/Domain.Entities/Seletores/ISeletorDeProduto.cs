using Domain.Entities.Models;

namespace Domain.Entities.Seletores
{
    public interface ISeletorDeProduto
    {
        Produto Selecionar(IReadOnlyList<Produto> produtos);
    }
}
