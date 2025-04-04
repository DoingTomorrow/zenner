// Decompiled with JetBrains decompiler
// Type: RestSharp.Parameter
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp
{
  public class Parameter
  {
    public string Name { get; set; }

    public object Value { get; set; }

    public ParameterType Type { get; set; }

    public override string ToString() => string.Format("{0}={1}", (object) this.Name, this.Value);
  }
}
