// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.IGetter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace NHibernate.Properties
{
  public interface IGetter
  {
    object Get(object target);

    Type ReturnType { get; }

    string PropertyName { get; }

    MethodInfo Method { get; }

    object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session);
  }
}
