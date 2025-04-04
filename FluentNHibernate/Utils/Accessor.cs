// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.Accessor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Utils
{
  public interface Accessor
  {
    string FieldName { get; }

    Type PropertyType { get; }

    Member InnerMember { get; }

    void SetValue(object target, object propertyValue);

    object GetValue(object target);

    Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression);

    string Name { get; }
  }
}
