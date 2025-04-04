// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.IndexBackref
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class IndexBackref : Property
  {
    private string collectionRole;
    private string entityName;

    public string CollectionRole
    {
      get => this.collectionRole;
      set => this.collectionRole = value;
    }

    public string EntityName
    {
      get => this.entityName;
      set => this.entityName = value;
    }

    public override bool BackRef => true;

    public override bool IsBasicPropertyAccessor => false;

    protected override IPropertyAccessor PropertyAccessor
    {
      get => (IPropertyAccessor) new IndexPropertyAccessor(this.collectionRole, this.entityName);
    }
  }
}
