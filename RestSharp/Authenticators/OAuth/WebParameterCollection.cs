// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.WebParameterCollection
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  internal class WebParameterCollection : WebPairCollection
  {
    public WebParameterCollection(IEnumerable<WebPair> parameters)
      : base(parameters)
    {
    }

    public WebParameterCollection(NameValueCollection collection)
      : base(collection)
    {
    }

    public WebParameterCollection()
    {
    }

    public WebParameterCollection(int capacity)
      : base(capacity)
    {
    }

    public WebParameterCollection(IDictionary<string, string> collection)
      : base(collection)
    {
    }

    public override void Add(string name, string value)
    {
      this.Add((WebPair) new WebParameter(name, value));
    }
  }
}
