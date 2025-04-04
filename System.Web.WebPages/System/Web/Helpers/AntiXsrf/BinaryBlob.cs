// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.BinaryBlob
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  [DebuggerDisplay("{DebuggerString}")]
  internal sealed class BinaryBlob : IEquatable<BinaryBlob>
  {
    private static readonly RNGCryptoServiceProvider _prng = new RNGCryptoServiceProvider();
    private readonly byte[] _data;

    public BinaryBlob(int bitLength)
      : this(bitLength, BinaryBlob.GenerateNewToken(bitLength))
    {
    }

    public BinaryBlob(int bitLength, byte[] data)
    {
      if (bitLength < 32 || bitLength % 8 != 0)
        throw new ArgumentOutOfRangeException(nameof (bitLength));
      if (data == null || data.Length != bitLength / 8)
        throw new ArgumentOutOfRangeException(nameof (data));
      this._data = data;
    }

    public int BitLength => checked (this._data.Length * 8);

    private string DebuggerString
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder("0x", 2 + this._data.Length * 2);
        for (int index = 0; index < this._data.Length; ++index)
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0:x2}", new object[1]
          {
            (object) this._data[index]
          });
        return stringBuilder.ToString();
      }
    }

    public override bool Equals(object obj) => this.Equals(obj as BinaryBlob);

    public bool Equals(BinaryBlob other)
    {
      return other != null && CryptoUtil.AreByteArraysEqual(this._data, other._data);
    }

    public byte[] GetData() => this._data;

    public override int GetHashCode() => BitConverter.ToInt32(this._data, 0);

    private static byte[] GenerateNewToken(int bitLength)
    {
      byte[] data = new byte[bitLength / 8];
      BinaryBlob._prng.GetBytes(data);
      return data;
    }
  }
}
