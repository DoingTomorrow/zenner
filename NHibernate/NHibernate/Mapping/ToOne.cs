// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ToOne
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public abstract class ToOne : SimpleValue, IFetchable
  {
    private FetchMode fetchMode;
    private bool lazy = true;
    internal string referencedPropertyName;
    private string referencedEntityName;
    private bool embedded;
    private bool unwrapProxy;

    public ToOne(Table table)
      : base(table)
    {
    }

    public override FetchMode FetchMode
    {
      get => this.fetchMode;
      set => this.fetchMode = value;
    }

    public string ReferencedPropertyName
    {
      get => this.referencedPropertyName;
      set => this.referencedPropertyName = StringHelper.InternedIfPossible(value);
    }

    public string ReferencedEntityName
    {
      get => this.referencedEntityName;
      set => this.referencedEntityName = StringHelper.InternedIfPossible(value);
    }

    public bool IsLazy
    {
      get => this.lazy;
      set => this.lazy = value;
    }

    public bool Embedded
    {
      get => this.embedded;
      set => this.embedded = value;
    }

    public abstract override void CreateForeignKey();

    public override void SetTypeUsingReflection(
      string className,
      string propertyName,
      string accesorName)
    {
      if (this.referencedEntityName != null)
        return;
      this.referencedEntityName = ReflectHelper.ReflectedPropertyClass(className, propertyName, accesorName).FullName;
    }

    public override bool IsValid(IMapping mapping)
    {
      if (this.referencedEntityName == null)
        throw new MappingException("association must specify the referenced entity");
      return base.IsValid(mapping);
    }

    public abstract override IType Type { get; }

    public override bool IsTypeSpecified => this.referencedEntityName != null;

    public bool UnwrapProxy
    {
      get => this.unwrapProxy;
      set => this.unwrapProxy = value;
    }
  }
}
