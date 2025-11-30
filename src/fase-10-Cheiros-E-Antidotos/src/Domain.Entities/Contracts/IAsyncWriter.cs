using System.Threading;
using System.Threading.Tasks;

namespace Domain.Entities.Contracts;

public interface IAsyncWriter<T>
{
    Task WriteAsync(T item, CancellationToken ct = default);
}
