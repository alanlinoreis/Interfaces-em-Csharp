using Domain.Entities.Models;

namespace Domain.Entities.Seletores;

public static class SeletorFactory
{
    private static readonly Dictionary<ModoSelecao, Func<ISeletorDeProduto>> _map =
        new()
        {
            { ModoSelecao.Economico, () => new SeletorEconomico() },
            { ModoSelecao.Premium,   () => new SeletorPremium() },
            { ModoSelecao.Qualidade, () => new SeletorQualidade() }
        };

    public static ISeletorDeProduto Criar(ModoSelecao modo)
        => _map[modo]();
}
