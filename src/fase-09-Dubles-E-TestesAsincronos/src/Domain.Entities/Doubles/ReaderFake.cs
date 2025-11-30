using Domain.Entities.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Entities.Doubles;

public class ReaderFake<T> : IAsyncReader<T>
{
    private readonly IEnumerable<T> _items;

    public ReaderFake(IEnumerable<T> items)
    {
        _items = items;
    }

    public async IAsyncEnumerable<T> ReadAllAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        foreach (var item in _items)
        {
            ct.ThrowIfCancellationRequested();
            yield return item;
            await Task.Yield();
        }
    }
}
