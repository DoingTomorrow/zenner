// Decompiled with JetBrains decompiler
// Type: RestSharp.RestResponseCookie
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp
{
  public class RestResponseCookie
  {
    public string Comment { get; set; }

    public Uri CommentUri { get; set; }

    public bool Discard { get; set; }

    public string Domain { get; set; }

    public bool Expired { get; set; }

    public DateTime Expires { get; set; }

    public bool HttpOnly { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public string Port { get; set; }

    public bool Secure { get; set; }

    public DateTime TimeStamp { get; set; }

    public string Value { get; set; }

    public int Version { get; set; }
  }
}
