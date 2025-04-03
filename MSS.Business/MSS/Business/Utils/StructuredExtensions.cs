// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.StructuredExtensions
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate;
using NHibernate.Type;
using System.Data;

#nullable disable
namespace MSS.Business.Utils
{
  public static class StructuredExtensions
  {
    private static readonly Sql2008StructuredForReadingValues StructuredForReadingValues = new Sql2008StructuredForReadingValues();
    private static readonly Sql2008StructuredStringList StructuredStringList = new Sql2008StructuredStringList();

    public static IQuery SetStructuredForReadingValues(
      this IQuery query,
      string name,
      DataTable dt)
    {
      return query.SetParameter(name, (object) dt, (IType) StructuredExtensions.StructuredForReadingValues);
    }

    public static IQuery SetStructuredStringList(this IQuery query, string name, DataTable dt)
    {
      return query.SetParameter(name, (object) dt, (IType) StructuredExtensions.StructuredStringList);
    }
  }
}
