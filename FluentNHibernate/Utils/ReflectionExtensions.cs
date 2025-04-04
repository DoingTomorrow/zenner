// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.ReflectionExtensions
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils.Reflection;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Utils
{
  public static class ReflectionExtensions
  {
    public static Member ToMember<TMapping, TReturn>(
      this Expression<Func<TMapping, TReturn>> propertyExpression)
    {
      return ReflectionHelper.GetMember<TMapping, TReturn>(propertyExpression);
    }
  }
}
