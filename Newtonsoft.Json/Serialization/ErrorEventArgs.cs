// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ErrorEventArgs
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class ErrorEventArgs : EventArgs
  {
    public object CurrentObject { get; private set; }

    public ErrorContext ErrorContext { get; private set; }

    public ErrorEventArgs(object currentObject, ErrorContext errorContext)
    {
      this.CurrentObject = currentObject;
      this.ErrorContext = errorContext;
    }
  }
}
