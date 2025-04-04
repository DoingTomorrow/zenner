// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.SingleTableSubclass
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class SingleTableSubclass(PersistentClass superclass) : Subclass(superclass)
  {
    protected internal override IEnumerable<Property> NonDuplicatedPropertyIterator
    {
      get
      {
        return (IEnumerable<Property>) new JoinedEnumerable<Property>(this.Superclass.UnjoinedPropertyIterator, this.UnjoinedPropertyIterator);
      }
    }

    protected internal override IEnumerable<ISelectable> DiscriminatorColumnIterator
    {
      get
      {
        return this.IsDiscriminatorInsertable && !this.Discriminator.HasFormula ? this.Discriminator.ColumnIterator : base.DiscriminatorColumnIterator;
      }
    }

    public override void Validate(IMapping mapping)
    {
      if (this.Discriminator == null)
        throw new MappingException("No discriminator found for " + this.EntityName + ". Discriminator is needed when 'single-table-per-hierarchy' is used and a class has subclasses");
      base.Validate(mapping);
    }
  }
}
