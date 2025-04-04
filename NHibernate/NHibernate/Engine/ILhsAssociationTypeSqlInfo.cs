// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.ILhsAssociationTypeSqlInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Engine
{
  public interface ILhsAssociationTypeSqlInfo
  {
    string[] GetAliasedColumnNames(IAssociationType type, int begin);

    string[] GetColumnNames(IAssociationType type, int begin);

    string GetTableName(IAssociationType type);
  }
}
