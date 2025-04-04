// Decompiled with JetBrains decompiler
// Type: RestSharp.Serializers.SerializeAsAttribute
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Extensions;
using System;
using System.Globalization;

#nullable disable
namespace RestSharp.Serializers
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  public sealed class SerializeAsAttribute : System.Attribute
  {
    public SerializeAsAttribute()
    {
      this.NameStyle = NameStyle.AsIs;
      this.Index = int.MaxValue;
      this.Culture = CultureInfo.InvariantCulture;
    }

    public string Name { get; set; }

    public bool Attribute { get; set; }

    public CultureInfo Culture { get; set; }

    public NameStyle NameStyle { get; set; }

    public int Index { get; set; }

    public string TransformName(string input)
    {
      string lowercaseAndUnderscoredWord = this.Name ?? input;
      switch (this.NameStyle)
      {
        case NameStyle.CamelCase:
          return lowercaseAndUnderscoredWord.ToCamelCase(this.Culture);
        case NameStyle.LowerCase:
          return lowercaseAndUnderscoredWord.ToLower();
        case NameStyle.PascalCase:
          return lowercaseAndUnderscoredWord.ToPascalCase(this.Culture);
        default:
          return input;
      }
    }
  }
}
