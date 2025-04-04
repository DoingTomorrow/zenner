// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.AntiXsrf.AntiForgeryTokenSerializer
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.IO;
using System.Web.Mvc;

#nullable disable
namespace System.Web.Helpers.AntiXsrf
{
  internal sealed class AntiForgeryTokenSerializer : IAntiForgeryTokenSerializer
  {
    private const byte TokenVersion = 1;
    private readonly ICryptoSystem _cryptoSystem;

    internal AntiForgeryTokenSerializer(ICryptoSystem cryptoSystem)
    {
      this._cryptoSystem = cryptoSystem;
    }

    public AntiForgeryToken Deserialize(string serializedToken)
    {
      try
      {
        using (MemoryStream input = new MemoryStream(this._cryptoSystem.Unprotect(serializedToken)))
        {
          using (BinaryReader reader = new BinaryReader((Stream) input))
          {
            AntiForgeryToken antiForgeryToken = AntiForgeryTokenSerializer.DeserializeImpl(reader);
            if (antiForgeryToken != null)
              return antiForgeryToken;
          }
        }
      }
      catch
      {
      }
      throw HttpAntiForgeryException.CreateDeserializationFailedException();
    }

    private static AntiForgeryToken DeserializeImpl(BinaryReader reader)
    {
      if (reader.ReadByte() != (byte) 1)
        return (AntiForgeryToken) null;
      AntiForgeryToken antiForgeryToken = new AntiForgeryToken();
      byte[] data1 = reader.ReadBytes(16);
      antiForgeryToken.SecurityToken = new BinaryBlob(128, data1);
      antiForgeryToken.IsSessionToken = reader.ReadBoolean();
      if (!antiForgeryToken.IsSessionToken)
      {
        if (reader.ReadBoolean())
        {
          byte[] data2 = reader.ReadBytes(32);
          antiForgeryToken.ClaimUid = new BinaryBlob(256, data2);
        }
        else
          antiForgeryToken.Username = reader.ReadString();
        antiForgeryToken.AdditionalData = reader.ReadString();
      }
      return reader.BaseStream.ReadByte() != -1 ? (AntiForgeryToken) null : antiForgeryToken;
    }

    public string Serialize(AntiForgeryToken token)
    {
      using (MemoryStream output = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          binaryWriter.Write((byte) 1);
          binaryWriter.Write(token.SecurityToken.GetData());
          binaryWriter.Write(token.IsSessionToken);
          if (!token.IsSessionToken)
          {
            if (token.ClaimUid != null)
            {
              binaryWriter.Write(true);
              binaryWriter.Write(token.ClaimUid.GetData());
            }
            else
            {
              binaryWriter.Write(false);
              binaryWriter.Write(token.Username);
            }
            binaryWriter.Write(token.AdditionalData);
          }
          binaryWriter.Flush();
          return this._cryptoSystem.Protect(output.ToArray());
        }
      }
    }
  }
}
