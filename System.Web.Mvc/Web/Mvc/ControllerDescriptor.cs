// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ControllerDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class ControllerDescriptor : ICustomAttributeProvider, IUniquelyIdentifiable
  {
    private readonly Lazy<string> _uniqueId;

    protected ControllerDescriptor()
    {
      this._uniqueId = new Lazy<string>(new Func<string>(this.CreateUniqueId));
    }

    public virtual string ControllerName
    {
      get
      {
        string name = this.ControllerType.Name;
        return name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ? name.Substring(0, name.Length - "Controller".Length) : name;
      }
    }

    public abstract Type ControllerType { get; }

    public virtual string UniqueId => this._uniqueId.Value;

    private string CreateUniqueId()
    {
      return DescriptorUtil.CreateUniqueId((object) this.GetType(), (object) this.ControllerName, (object) this.ControllerType);
    }

    public abstract ActionDescriptor FindAction(
      ControllerContext controllerContext,
      string actionName);

    public abstract ActionDescriptor[] GetCanonicalActions();

    public virtual object[] GetCustomAttributes(bool inherit)
    {
      return this.GetCustomAttributes(typeof (object), inherit);
    }

    public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return !(attributeType == (Type) null) ? (object[]) Array.CreateInstance(attributeType, 0) : throw new ArgumentNullException(nameof (attributeType));
    }

    public virtual IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
    {
      return this.GetCustomAttributes(typeof (FilterAttribute), true).Cast<FilterAttribute>();
    }

    public virtual bool IsDefined(Type attributeType, bool inherit)
    {
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      return false;
    }
  }
}
