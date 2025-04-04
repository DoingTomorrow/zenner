// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ReflectionDynamicObject
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Dynamic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace System.Web.WebPages
{
  internal sealed class ReflectionDynamicObject : DynamicObject
  {
    private object RealObject { get; set; }

    public static object WrapObjectIfInternal(object o)
    {
      if (o == null)
        return (object) null;
      if (o.GetType().IsPublic)
        return o;
      return (object) new ReflectionDynamicObject()
      {
        RealObject = o
      };
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      PropertyInfo property = this.RealObject.GetType().GetProperty(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
      if (property == (PropertyInfo) null)
      {
        result = (object) null;
      }
      else
      {
        result = property.GetValue(this.RealObject, (object[]) null);
        result = ReflectionDynamicObject.WrapObjectIfInternal(result);
      }
      return true;
    }

    public override bool TryInvokeMember(
      InvokeMemberBinder binder,
      object[] args,
      out object result)
    {
      result = this.RealObject.GetType().InvokeMember(binder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, this.RealObject, args, CultureInfo.InvariantCulture);
      return true;
    }

    public override bool TryConvert(ConvertBinder binder, out object result)
    {
      result = this.RealObject;
      return true;
    }

    public override string ToString() => this.RealObject.ToString();
  }
}
