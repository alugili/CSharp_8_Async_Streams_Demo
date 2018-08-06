using System.Threading.Tasks;

namespace AsyncStreams
{
  public interface IAsyncDisposable
  {
    Task DiskposeAsync();
  }

  public interface IAsyncEnumerable<out T>
  {
    IAsyncEnumerator<T> GetAsyncEnumerator();
  }

  public interface IAsyncEnumerator<out T>
  {
    Task<bool> WaitForNextAsync();
    T TryGetNext(out bool success);
  }
}