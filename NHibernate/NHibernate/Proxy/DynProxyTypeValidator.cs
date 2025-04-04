// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DynProxyTypeValidator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Proxy
{
  public class DynProxyTypeValidator : IProxyValidator
  {
    private readonly List<string> errors = new List<string>();

    public ICollection<string> ValidateType(Type type)
    {
      this.errors.Clear();
      if (type.IsInterface)
        return (ICollection<string>) null;
      this.CheckHasVisibleDefaultConstructor(type);
      this.CheckAccessibleMembersAreVirtual(type);
      this.CheckNotSealed(type);
      return this.errors.Count <= 0 ? (ICollection<string>) null : (ICollection<string>) this.errors;
    }

    protected void EnlistError(Type type, string text)
    {
      this.errors.Add(string.Format("{0}: {1}", (object) type, (object) text));
    }

    protected virtual void CheckHasVisibleDefaultConstructor(Type type)
    {
      if (this.HasVisibleDefaultConstructor(type))
        return;
      this.EnlistError(type, "type should have a visible (public or protected) no-argument constructor");
    }

    protected virtual void CheckAccessibleMembersAreVirtual(Type type)
    {
      foreach (MemberInfo member in type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        switch (member)
        {
          case PropertyInfo _:
            PropertyInfo propertyInfo = (PropertyInfo) member;
            if (propertyInfo.ShouldBeProxiable())
            {
              MethodInfo[] accessors = propertyInfo.GetAccessors(true);
              if (accessors != null)
              {
                foreach (MethodInfo method in accessors)
                  this.CheckMethodIsVirtual(type, method);
                break;
              }
              break;
            }
            break;
          case MethodInfo _:
            MethodInfo methodInfo = (MethodInfo) member;
            if (!this.IsPropertyMethod(methodInfo) && methodInfo.ShouldBeProxiable())
            {
              this.CheckMethodIsVirtual(type, methodInfo);
              break;
            }
            break;
          case FieldInfo _:
            FieldInfo fieldInfo = (FieldInfo) member;
            if (fieldInfo.IsPublic || fieldInfo.IsAssembly || fieldInfo.IsFamilyOrAssembly)
            {
              this.EnlistError(type, "field " + member.Name + " should not be public nor internal (ecapsulate it in a property).");
              break;
            }
            break;
        }
      }
    }

    private bool IsPropertyMethod(MethodInfo methodInfo)
    {
      if (!methodInfo.IsSpecialName)
        return false;
      return methodInfo.Name.StartsWith("get_") || methodInfo.Name.StartsWith("set_");
    }

    protected virtual void CheckMethodIsVirtual(Type type, MethodInfo method)
    {
      if (this.IsProxeable(method))
        return;
      this.EnlistError(type, "method " + method.Name + " should be 'public/protected virtual' or 'protected internal virtual'");
    }

    public virtual bool IsProxeable(MethodInfo method) => method.IsProxiable();

    protected virtual bool HasVisibleDefaultConstructor(Type type)
    {
      ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
      return constructor != null && !constructor.IsPrivate;
    }

    protected void CheckNotSealed(Type type)
    {
      if (!ReflectHelper.IsFinalClass(type))
        return;
      this.EnlistError(type, "type should not be sealed");
    }
  }
}
