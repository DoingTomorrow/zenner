// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Filter
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public class Filter
  {
    public const int DefaultOrder = -1;

    public Filter(object instance, FilterScope scope, int? order)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (!order.HasValue && instance is IMvcFilter mvcFilter)
        order = new int?(mvcFilter.Order);
      this.Instance = instance;
      this.Order = order ?? -1;
      this.Scope = scope;
    }

    public object Instance { get; protected set; }

    public int Order { get; protected set; }

    public FilterScope Scope { get; protected set; }
  }
}
