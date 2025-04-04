// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JRaw
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.Globalization;
using System.IO;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JRaw : JValue
  {
    public JRaw(JRaw other)
      : base((JValue) other)
    {
    }

    public JRaw(object rawJson)
      : base(rawJson, JTokenType.Raw)
    {
    }

    public static JRaw Create(JsonReader reader)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
        {
          jsonTextWriter.WriteToken(reader);
          return new JRaw((object) stringWriter.ToString());
        }
      }
    }

    internal override JToken CloneToken() => (JToken) new JRaw(this);
  }
}
