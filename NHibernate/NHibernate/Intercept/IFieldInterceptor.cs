// Decompiled with JetBrains decompiler
// Type: NHibernate.Intercept.IFieldInterceptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Intercept
{
  public interface IFieldInterceptor
  {
    bool IsDirty { get; }

    ISessionImplementor Session { get; set; }

    bool IsInitialized { get; }

    bool IsInitializedField(string field);

    void MarkDirty();

    void ClearDirty();

    object Intercept(object target, string fieldName, object value);

    string EntityName { get; }

    Type MappedClass { get; }
  }
}
