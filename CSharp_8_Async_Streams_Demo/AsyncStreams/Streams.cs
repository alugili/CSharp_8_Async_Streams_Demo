using System;
using System.IO;
using System.Threading.Tasks;

namespace AsyncStreams
{
  public class StreamEnumerator : IAsyncEnumerator<byte>
  {
    private readonly Stream _stream;
    private readonly byte[] _buffer;
    private int _bytesRead;
    private int _bufferIndex;
    private const int BufferSize = 8000;

    public StreamEnumerator(Stream stream)
    {
      _stream = stream ?? throw new ArgumentNullException(nameof(stream));
      _buffer = new byte[BufferSize];
      _bytesRead = -1;
    }

    public async Task<bool> WaitForNextAsync()
    {
      await Task.Delay(1000);

      _bytesRead = await _stream.ReadAsync(_buffer, 0, BufferSize);
      _bufferIndex = 0;

      if (_bytesRead == 0)
      {
        Console.WriteLine();
        Console.WriteLine("Enumeration complete!");
        return false;
      }

      Console.WriteLine($"Read {_bytesRead: 0,0} bytes");

      return true;
    }

    public byte TryGetNext(out bool success)
    {
      if (_bufferIndex == _bytesRead)
      {
        success = false;
        return 0;
      }

      byte result = _buffer[_bufferIndex];
      _bufferIndex++;

      success = true;
      return result;
    }

    public Task DisposeAsync()
    {
      _stream.Dispose();
      Console.WriteLine("Stream disposed!");
      return Task.CompletedTask;
    }
  }

  public class EnumerableStream : IAsyncEnumerable<byte>
  {
    private readonly Stream _stream;

    public EnumerableStream(Stream stream) => _stream = stream ?? throw new ArgumentNullException(nameof(stream));

    public IAsyncEnumerator<byte> GetAsyncEnumerator() => new StreamEnumerator(_stream);
  }

  public static class StreamExtensions
  {
    public static IAsyncEnumerable<byte> AsEnumerable(this Stream stream) => new EnumerableStream(stream);
  }
}
