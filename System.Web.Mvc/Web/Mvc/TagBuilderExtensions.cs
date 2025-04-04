// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.TagBuilderExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  internal static class TagBuilderExtensions
  {
    internal static MvcHtmlString ToMvcHtmlString(
      this TagBuilder tagBuilder,
      TagRenderMode renderMode)
    {
      return new MvcHtmlString(tagBuilder.ToString(renderMode));
    }
  }
}
