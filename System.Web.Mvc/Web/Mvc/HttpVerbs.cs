// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.HttpVerbs
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  [Flags]
  public enum HttpVerbs
  {
    Get = 1,
    Post = 2,
    Put = 4,
    Delete = 8,
    Head = 16, // 0x00000010
    Patch = 32, // 0x00000020
    Options = 64, // 0x00000040
  }
}
