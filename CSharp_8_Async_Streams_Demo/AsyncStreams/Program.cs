using System;
using System.IO;
using System.Threading.Tasks;

namespace AsyncStreams
{
  public class Program
  {
    private static (Stream stream, long checkSum) CreateStream()
    {
      long checksum = 0L;

      byte[] bytes = new byte[20000];

      for (int i = 0; i < bytes.Length; i++)
      {
        byte value = (byte)(i % byte.MaxValue);
        bytes[i] = value;

        unchecked { checksum += value; }
      }

      MemoryStream stream = new MemoryStream(bytes);
      return (stream, checksum);
    }

    private static async Task Main(string[] args)
    {
      (Stream stream, long checksum) = CreateStream();

      long c = 0L;

      foreach await (byte b in stream.AsEnumerable())
      {
        unchecked { c += b; }
      }

      if (c == checksum)
      {
        Console.WriteLine("Checksums match!");
      }
    }
  }
}