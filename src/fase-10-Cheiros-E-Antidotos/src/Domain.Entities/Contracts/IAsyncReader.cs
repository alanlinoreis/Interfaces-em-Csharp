using System.Collections.Generic;
using System.Threading;

namespace Domain.Entities.Contracts;

public interface IAsyncReader<T>
{
    IAsyncEnumerable<T> ReadAllAsync(CancellationToken ct = default);
}
