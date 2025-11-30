using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Seletores;

namespace Domain.Entities.Service;

public class ProdutoSelecaoService
{
    private readonly Dictionary<ModoSelecao, ISeletorDeProduto> _map =
        new()
        {
            { ModoSelecao.Economico, new SeletorEconomico() },
            { ModoSelecao.Premium,   new SeletorPremium() },
            { ModoSelecao.Qualidade, new SeletorQualidade() }
        };

    public Produto Selecionar(IReadRepository<Produto, int> repo, ModoSelecao modo)
    {
        var seletor = _map[modo];

        var produtos = repo.ListAll();

        return seletor.Selecionar(produtos);
    }
}
