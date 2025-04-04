// Decompiled with JetBrains decompiler
// Type: RestSharp.Serializers.JsonSerializer
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp.Serializers
{
  public class JsonSerializer : ISerializer
  {
    public JsonSerializer() => this.ContentType = "application/json";

    public string Serialize(object obj) => SimpleJson.SerializeObject(obj);

    public string DateFormat { get; set; }

    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string ContentType { get; set; }
  }
}
