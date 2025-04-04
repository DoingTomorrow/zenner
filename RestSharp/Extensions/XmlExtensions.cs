// Decompiled with JetBrains decompiler
// Type: RestSharp.Extensions.XmlExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.Xml.Linq;

#nullable disable
namespace RestSharp.Extensions
{
  public static class XmlExtensions
  {
    public static XName AsNamespaced(this string name, string @namespace)
    {
      XName xname = (XName) name;
      if (@namespace.HasValue())
        xname = XName.Get(name, @namespace);
      return xname;
    }
  }
}
