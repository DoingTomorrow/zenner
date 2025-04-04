// Decompiled with JetBrains decompiler
// Type: NHibernate.Intercept.DefaultFieldInterceptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Intercept
{
  [Serializable]
  public class DefaultFieldInterceptor(
    ISessionImplementor session,
    ISet<string> uninitializedFields,
    ISet<string> unwrapProxyFieldNames,
    string entityName,
    Type mappedClass) : AbstractFieldInterceptor(session, uninitializedFields, unwrapProxyFieldNames, entityName, mappedClass)
  {
  }
}
