// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.DependantValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class DependantValue : SimpleValue
  {
    private readonly IKeyValue wrappedValue;
    private bool isNullable = true;
    private bool isUpdateable = true;

    public DependantValue(Table table, IKeyValue prototype)
      : base(table)
    {
      this.wrappedValue = prototype;
    }

    public override IType Type => this.wrappedValue.Type;

    public void SetTypeUsingReflection(string className, string propertyName)
    {
    }

    public override bool IsNullable => this.isNullable;

    public void SetNullable(bool nullable) => this.isNullable = nullable;

    public override bool IsUpdateable => this.isUpdateable;

    public virtual void SetUpdateable(bool updateable) => this.isUpdateable = updateable;
  }
}
