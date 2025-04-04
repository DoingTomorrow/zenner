// Decompiled with JetBrains decompiler
// Type: NHibernate.Context.ReflectiveHttpContext
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Context
{
  public static class ReflectiveHttpContext
  {
    static ReflectiveHttpContext()
    {
      ReflectiveHttpContext.CreateCurrentHttpContextGetter();
      ReflectiveHttpContext.CreateHttpContextItemsGetter();
    }

    public static Func<object> HttpContextCurrentGetter { get; private set; }

    public static Func<object, IDictionary> HttpContextItemsGetter { get; private set; }

    public static IDictionary HttpContextCurrentItems
    {
      get
      {
        return ReflectiveHttpContext.HttpContextItemsGetter(ReflectiveHttpContext.HttpContextCurrentGetter());
      }
    }

    private static Type HttpContextType
    {
      get
      {
        return Type.GetType(string.Format("System.Web.HttpContext, System.Web, Version={0}, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", (object) Environment.Version));
      }
    }

    private static void CreateCurrentHttpContextGetter()
    {
      ReflectiveHttpContext.HttpContextCurrentGetter = (Func<object>) Expression.Lambda((Expression) Expression.Convert((Expression) Expression.Property((Expression) null, ReflectiveHttpContext.HttpContextType.GetProperty("Current", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)), typeof (object))).Compile();
    }

    private static void CreateHttpContextItemsGetter()
    {
      ParameterExpression parameterExpression = Expression.Parameter(typeof (object), "context");
      ReflectiveHttpContext.HttpContextItemsGetter = (Func<object, IDictionary>) Expression.Lambda((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, ReflectiveHttpContext.HttpContextType), "Items"), parameterExpression).Compile();
    }
  }
}
