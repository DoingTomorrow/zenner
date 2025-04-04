// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonArray
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Bson
{
  internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
  {
    private readonly List<BsonToken> _children = new List<BsonToken>();

    public void Add(BsonToken token)
    {
      this._children.Add(token);
      token.Parent = (BsonToken) this;
    }

    public override BsonType Type => BsonType.Array;

    public IEnumerator<BsonToken> GetEnumerator()
    {
      return (IEnumerator<BsonToken>) this._children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
