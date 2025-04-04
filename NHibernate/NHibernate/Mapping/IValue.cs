// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.IValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  public interface IValue
  {
    int ColumnSpan { get; }

    IEnumerable<ISelectable> ColumnIterator { get; }

    IType Type { get; }

    Table Table { get; }

    bool HasFormula { get; }

    bool IsAlternateUniqueKey { get; }

    bool IsNullable { get; }

    bool[] ColumnUpdateability { get; }

    bool[] ColumnInsertability { get; }

    bool IsSimpleValue { get; }

    void CreateForeignKey();

    bool IsValid(IMapping mapping);

    FetchMode FetchMode { get; }

    void SetTypeUsingReflection(string className, string propertyName, string accesorName);

    object Accept(IValueVisitor visitor);
  }
}
