// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.ElementPropertyMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public class ElementPropertyMapping : IPropertyMapping
  {
    private readonly string[] elementColumns;
    private readonly IType type;

    public ElementPropertyMapping(string[] elementColumns, IType type)
    {
      this.elementColumns = elementColumns;
      this.type = type;
    }

    public IType ToType(string propertyName)
    {
      if (propertyName == null || "id".Equals(propertyName))
        return this.type;
      throw new QueryException(string.Format("cannot dereference scalar collection element: {0}", (object) propertyName));
    }

    public bool TryToType(string propertyName, out IType outType)
    {
      try
      {
        outType = this.ToType(propertyName);
        return true;
      }
      catch (Exception ex)
      {
        outType = (IType) null;
        return false;
      }
    }

    public string[] ToColumns(string alias, string propertyName)
    {
      if (propertyName == null || "id".Equals(propertyName))
        return StringHelper.Qualify(alias, this.elementColumns);
      throw new QueryException(string.Format("cannot dereference scalar collection element: {0}", (object) propertyName));
    }

    public string[] ToColumns(string propertyName)
    {
      throw new NotSupportedException("References to collections must be define a SQL alias");
    }

    public IType Type => this.type;
  }
}
