// Decompiled with JetBrains decompiler
// Type: RestSharp.Deserializers.DotNetXmlDeserializer
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.IO;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace RestSharp.Deserializers
{
  public class DotNetXmlDeserializer : IDeserializer
  {
    public string DateFormat { get; set; }

    public string Namespace { get; set; }

    public string RootElement { get; set; }

    public T Deserialize<T>(IRestResponse response)
    {
      if (string.IsNullOrEmpty(response.Content))
        return default (T);
      using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content)))
        return (T) new XmlSerializer(typeof (T)).Deserialize((Stream) memoryStream);
    }
  }
}
