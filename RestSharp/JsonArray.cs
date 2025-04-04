// Decompiled with JetBrains decompiler
// Type: RestSharp.JsonArray
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace RestSharp
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class JsonArray : List<object>
  {
    public JsonArray()
    {
    }

    public JsonArray(int capacity)
      : base(capacity)
    {
    }

    public override string ToString() => SimpleJson.SerializeObject((object) this) ?? string.Empty;
  }
}
