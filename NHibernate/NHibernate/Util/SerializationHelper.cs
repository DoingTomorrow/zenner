// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.SerializationHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace NHibernate.Util
{
  public class SerializationHelper
  {
    private SerializationHelper()
    {
    }

    public static byte[] Serialize(object obj)
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, obj);
        return serializationStream.ToArray();
      }
    }

    public static object Deserialize(byte[] data)
    {
      using (MemoryStream serializationStream = new MemoryStream(data))
        return new BinaryFormatter().Deserialize((Stream) serializationStream);
    }
  }
}
