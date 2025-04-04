// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ErrorContext
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class ErrorContext
  {
    internal ErrorContext(object originalObject, object member, string path, Exception error)
    {
      this.OriginalObject = originalObject;
      this.Member = member;
      this.Error = error;
      this.Path = path;
    }

    internal bool Traced { get; set; }

    public Exception Error { get; private set; }

    public object OriginalObject { get; private set; }

    public object Member { get; private set; }

    public string Path { get; private set; }

    public bool Handled { get; set; }
  }
}
