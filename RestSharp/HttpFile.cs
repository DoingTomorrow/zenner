// Decompiled with JetBrains decompiler
// Type: RestSharp.HttpFile
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.IO;

#nullable disable
namespace RestSharp
{
  public class HttpFile
  {
    public long ContentLength { get; set; }

    public Action<Stream> Writer { get; set; }

    public string FileName { get; set; }

    public string ContentType { get; set; }

    public string Name { get; set; }
  }
}
