// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.JoinHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Engine
{
  public sealed class JoinHelper
  {
    private JoinHelper()
    {
    }

    public static ILhsAssociationTypeSqlInfo GetLhsSqlInfo(
      string alias,
      int property,
      IOuterJoinLoadable lhsPersister,
      IMapping mapping)
    {
      return (ILhsAssociationTypeSqlInfo) new PropertiesLhsAssociationTypeSqlInfo(alias, property, lhsPersister, mapping);
    }

    public static ILhsAssociationTypeSqlInfo GetIdLhsSqlInfo(
      string alias,
      IOuterJoinLoadable lhsPersister,
      IMapping mapping)
    {
      return (ILhsAssociationTypeSqlInfo) new IdPropertiesLhsAssociationTypeSqlInfo(alias, lhsPersister, mapping);
    }

    public static string[] GetRHSColumnNames(
      IAssociationType type,
      ISessionFactoryImplementor factory)
    {
      string uniqueKeyPropertyName = type.RHSUniqueKeyPropertyName;
      IJoinable associatedJoinable = type.GetAssociatedJoinable(factory);
      return uniqueKeyPropertyName == null ? associatedJoinable.KeyColumnNames : ((IOuterJoinLoadable) associatedJoinable).GetPropertyColumnNames(uniqueKeyPropertyName);
    }
  }
}
