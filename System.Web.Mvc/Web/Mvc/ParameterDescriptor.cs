// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ParameterDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Reflection;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class ParameterDescriptor : ICustomAttributeProvider
  {
    private static readonly ParameterDescriptor.EmptyParameterBindingInfo _emptyBindingInfo = new ParameterDescriptor.EmptyParameterBindingInfo();

    public abstract ActionDescriptor ActionDescriptor { get; }

    public virtual ParameterBindingInfo BindingInfo
    {
      get => (ParameterBindingInfo) ParameterDescriptor._emptyBindingInfo;
    }

    public virtual object DefaultValue => (object) null;

    public abstract string ParameterName { get; }

    public abstract Type ParameterType { get; }

    public virtual object[] GetCustomAttributes(bool inherit)
    {
      return this.GetCustomAttributes(typeof (object), inherit);
    }

    public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return !(attributeType == (Type) null) ? (object[]) Array.CreateInstance(attributeType, 0) : throw new ArgumentNullException(nameof (attributeType));
    }

    public virtual bool IsDefined(Type attributeType, bool inherit)
    {
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      return false;
    }

    private sealed class EmptyParameterBindingInfo : ParameterBindingInfo
    {
    }
  }
}
