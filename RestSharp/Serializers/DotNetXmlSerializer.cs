// Decompiled with JetBrains decompiler
// Type: RestSharp.Serializers.DotNetXmlSerializer
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.IO;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace RestSharp.Serializers
{
  public class DotNetXmlSerializer : ISerializer
  {
    public DotNetXmlSerializer()
    {
      this.ContentType = "application/xml";
      this.Encoding = Encoding.UTF8;
    }

    public DotNetXmlSerializer(string @namespace)
      : this()
    {
      this.Namespace = @namespace;
    }

    public string Serialize(object obj)
    {
      XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
      namespaces.Add(string.Empty, this.Namespace);
      System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
      DotNetXmlSerializer.EncodingStringWriter encodingStringWriter = new DotNetXmlSerializer.EncodingStringWriter(this.Encoding);
      xmlSerializer.Serialize((TextWriter) encodingStringWriter, obj, namespaces);
      return encodingStringWriter.ToString();
    }

    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string DateFormat { get; set; }

    public string ContentType { get; set; }

    public Encoding Encoding { get; set; }

    private class EncodingStringWriter : StringWriter
    {
      private readonly Encoding encoding;

      public EncodingStringWriter(Encoding encoding) => this.encoding = encoding;

      public override Encoding Encoding => this.encoding;
    }
  }
}
