// Decompiled with JetBrains decompiler
// Type: NHibernate.Intercept.FieldInterceptionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Intercept
{
  public static class FieldInterceptionHelper
  {
    public static bool IsInstrumented(Type entityClass)
    {
      return NHibernate.Cfg.Environment.BytecodeProvider.ProxyFactoryFactory.IsInstrumented(entityClass);
    }

    public static bool IsInstrumented(object entity) => entity is IFieldInterceptorAccessor;

    public static IFieldInterceptor ExtractFieldInterceptor(object entity)
    {
      return entity is IFieldInterceptorAccessor interceptorAccessor ? interceptorAccessor.FieldInterceptor : (IFieldInterceptor) null;
    }

    public static IFieldInterceptor InjectFieldInterceptor(
      object entity,
      string entityName,
      Type mappedClass,
      ISet<string> uninitializedFieldNames,
      ISet<string> unwrapProxyFieldNames,
      ISessionImplementor session)
    {
      if (!(entity is IFieldInterceptorAccessor interceptorAccessor))
        return (IFieldInterceptor) null;
      DefaultFieldInterceptor fieldInterceptor = new DefaultFieldInterceptor(session, uninitializedFieldNames, unwrapProxyFieldNames, entityName, mappedClass);
      interceptorAccessor.FieldInterceptor = (IFieldInterceptor) fieldInterceptor;
      return (IFieldInterceptor) fieldInterceptor;
    }

    public static void ClearDirty(object entity)
    {
      FieldInterceptionHelper.ExtractFieldInterceptor(entity)?.ClearDirty();
    }

    public static void MarkDirty(object entity)
    {
      FieldInterceptionHelper.ExtractFieldInterceptor(entity)?.MarkDirty();
    }
  }
}
