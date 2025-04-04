// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.WebParameter
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Diagnostics;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  [DebuggerDisplay("{Name}:{Value}")]
  [Serializable]
  internal class WebParameter(string name, string value) : WebPair(name, value)
  {
  }
}
