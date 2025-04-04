// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.IJEnumerable`1
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public interface IJEnumerable<out T> : IEnumerable<T>, IEnumerable where T : JToken
  {
    IJEnumerable<JToken> this[object key] { get; }
  }
}
