// Decompiled with JetBrains decompiler
// Type: RestSharp.Deserializers.IDeserializer
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp.Deserializers
{
  public interface IDeserializer
  {
    T Deserialize<T>(IRestResponse response);

    string RootElement { get; set; }

    string Namespace { get; set; }

    string DateFormat { get; set; }
  }
}
