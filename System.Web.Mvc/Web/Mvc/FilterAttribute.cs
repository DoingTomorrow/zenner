// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FilterAttribute
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Concurrent;
using System.Linq;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public abstract class FilterAttribute : Attribute, IMvcFilter
  {
    private static readonly ConcurrentDictionary<Type, bool> _multiuseAttributeCache = new ConcurrentDictionary<Type, bool>();
    private int _order = -1;

    public bool AllowMultiple => FilterAttribute.AllowsMultiple(this.GetType());

    public int Order
    {
      get => this._order;
      set
      {
        this._order = value >= -1 ? value : throw new ArgumentOutOfRangeException(nameof (value), MvcResources.FilterAttribute_OrderOutOfRange);
      }
    }

    private static bool AllowsMultiple(Type attributeType)
    {
      return FilterAttribute._multiuseAttributeCache.GetOrAdd(attributeType, (Func<Type, bool>) (type => type.GetCustomAttributes(typeof (AttributeUsageAttribute), true).Cast<AttributeUsageAttribute>().First<AttributeUsageAttribute>().AllowMultiple));
    }
  }
}
