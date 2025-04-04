// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.TypeNameHandling
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json
{
  [Flags]
  public enum TypeNameHandling
  {
    None = 0,
    Objects = 1,
    Arrays = 2,
    All = Arrays | Objects, // 0x00000003
    Auto = 4,
  }
}
