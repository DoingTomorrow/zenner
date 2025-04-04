// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.MachineKey40CryptoSystem
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Net;
using System.Text;
using System.Web.Security;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class MachineKey40CryptoSystem : ICryptoSystem
  {
    private const uint MagicHeader = 2240279142;
    private readonly Func<string, MachineKeyProtection, byte[]> _decoder;
    private readonly Func<byte[], MachineKeyProtection, string> _encoder;

    public MachineKey40CryptoSystem()
      : this(new Func<byte[], MachineKeyProtection, string>(MachineKey.Encode), new Func<string, MachineKeyProtection, byte[]>(MachineKey.Decode))
    {
    }

    internal MachineKey40CryptoSystem(
      Func<byte[], MachineKeyProtection, string> encoder,
      Func<string, MachineKeyProtection, byte[]> decoder)
    {
      this._encoder = encoder;
      this._decoder = decoder;
    }

    public string Protect(byte[] data)
    {
      byte[] dst = new byte[data.Length + 4];
      Buffer.BlockCopy((Array) data, 0, (Array) dst, 4, data.Length);
      dst[0] = (byte) 133;
      dst[1] = (byte) 135;
      dst[2] = (byte) 242;
      dst[3] = (byte) 102;
      return MachineKey40CryptoSystem.HexToBase64(this._encoder(dst, MachineKeyProtection.All));
    }

    public byte[] Unprotect(string protectedData)
    {
      byte[] src = this._decoder(MachineKey40CryptoSystem.Base64ToHex(protectedData), MachineKeyProtection.All);
      if (src == null || src.Length < 4 || IPAddress.NetworkToHostOrder(BitConverter.ToInt32(src, 0)) != -2054688154)
        return (byte[]) null;
      byte[] dst = new byte[src.Length - 4];
      Buffer.BlockCopy((Array) src, 4, (Array) dst, 0, dst.Length);
      return dst;
    }

    internal static string Base64ToHex(string base64)
    {
      StringBuilder stringBuilder = new StringBuilder((int) ((double) base64.Length * 1.5));
      foreach (byte num in HttpServerUtility.UrlTokenDecode(base64))
      {
        stringBuilder.Append(MachineKey40CryptoSystem.HexDigit((int) num >> 4));
        stringBuilder.Append(MachineKey40CryptoSystem.HexDigit((int) num & 15));
      }
      return stringBuilder.ToString();
    }

    private static char HexDigit(int value)
    {
      return value > 9 ? (char) (value + 55) : (char) (value + 48);
    }

    private static int HexValue(char digit) => digit <= '9' ? (int) digit - 48 : (int) digit - 55;

    internal static string HexToBase64(string hex)
    {
      int length = hex.Length / 2;
      byte[] input = new byte[length];
      for (int index = 0; index < length; ++index)
        input[index] = (byte) ((MachineKey40CryptoSystem.HexValue(hex[index * 2]) << 4) + MachineKey40CryptoSystem.HexValue(hex[index * 2 + 1]));
      return HttpServerUtility.UrlTokenEncode(input);
    }
  }
}
