// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.MvcFilter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public abstract class MvcFilter : IMvcFilter
  {
    protected MvcFilter()
    {
    }

    protected MvcFilter(bool allowMultiple, int order)
    {
      this.AllowMultiple = allowMultiple;
      this.Order = order;
    }

    public bool AllowMultiple { get; private set; }

    public int Order { get; private set; }
  }
}
