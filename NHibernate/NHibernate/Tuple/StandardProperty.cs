// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.StandardProperty
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Tuple
{
  public class StandardProperty : Property
  {
    private readonly bool lazy;
    private readonly bool insertable;
    private readonly bool updateable;
    private readonly bool insertGenerated;
    private readonly bool updateGenerated;
    private readonly bool nullable;
    private readonly bool dirtyCheckable;
    private readonly bool versionable;
    private readonly CascadeStyle cascadeStyle;
    private readonly NHibernate.FetchMode? fetchMode;

    public StandardProperty(
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
      NHibernate.FetchMode? fetchMode)
      : base(name, node, type)
    {
      this.lazy = lazy;
      this.insertable = insertable;
      this.updateable = updateable;
      this.insertGenerated = insertGenerated;
      this.updateGenerated = updateGenerated;
      this.nullable = nullable;
      this.dirtyCheckable = checkable;
      this.versionable = versionable;
      this.cascadeStyle = cascadeStyle;
      this.fetchMode = fetchMode;
    }

    public bool IsLazy => this.lazy;

    public bool IsInsertable => this.insertable;

    public bool IsUpdateable => this.updateable;

    public bool IsInsertGenerated => this.insertGenerated;

    public bool IsUpdateGenerated => this.updateGenerated;

    public bool IsNullable => this.nullable;

    public bool IsDirtyCheckable(bool hasUninitializedProperties)
    {
      if (!this.IsDirtyCheckable())
        return false;
      return !hasUninitializedProperties || !this.IsLazy;
    }

    public bool IsDirtyCheckable() => this.dirtyCheckable;

    public bool IsVersionable => this.versionable;

    public CascadeStyle CascadeStyle => this.cascadeStyle;

    public NHibernate.FetchMode? FetchMode => this.fetchMode;
  }
}
