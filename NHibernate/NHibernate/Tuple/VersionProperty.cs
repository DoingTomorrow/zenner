// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.VersionProperty
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Tuple
{
  [Serializable]
  public class VersionProperty : StandardProperty
  {
    private readonly VersionValue unsavedValue;

    public VersionProperty(
      string name,
      string node,
      IType type,
      bool lazy,
      bool insertable,
      bool updateable,
      bool insertGenerated,
      bool updateGenerated,
      bool nullable,
      bool checkable,
      bool versionable,
      CascadeStyle cascadeStyle,
      VersionValue unsavedValue)
      : base(name, node, type, lazy, insertable, updateable, insertGenerated, updateGenerated, nullable, checkable, versionable, cascadeStyle, new NHibernate.FetchMode?())
    {
      this.unsavedValue = unsavedValue;
    }

    public VersionValue UnsavedValue => this.unsavedValue;
  }
}
