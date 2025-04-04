// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Any
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Any(Table table) : SimpleValue(table)
  {
    private string identifierTypeName;
    private string metaTypeName = NHibernateUtil.String.Name;
    private IDictionary<object, string> metaValues;
    private IType type;

    public virtual string IdentifierTypeName
    {
      get => this.identifierTypeName;
      set => this.identifierTypeName = value;
    }

    public override IType Type
    {
      get
      {
        if (this.type == null)
          this.type = (IType) new AnyType(this.metaValues == null ? ("class".Equals(this.metaTypeName) ? (IType) new ClassMetaType() : TypeFactory.HeuristicType(this.metaTypeName)) : (IType) new NHibernate.Type.MetaType(this.metaValues, TypeFactory.HeuristicType(this.metaTypeName)), TypeFactory.HeuristicType(this.identifierTypeName));
        return this.type;
      }
    }

    public void ResetCachedType() => this.type = (IType) null;

    public override void SetTypeUsingReflection(
      string className,
      string propertyName,
      string access)
    {
    }

    public virtual string MetaType
    {
      get => this.metaTypeName;
      set => this.metaTypeName = value;
    }

    public IDictionary<object, string> MetaValues
    {
      get => this.metaValues;
      set => this.metaValues = value;
    }
  }
}
