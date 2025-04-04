// Decompiled with JetBrains decompiler
// Type: NHibernate.IQueryExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate
{
  public interface IQueryExpression
  {
    IASTNode Translate(ISessionFactoryImplementor sessionFactory);

    string Key { get; }

    Type Type { get; }

    IList<NamedParameterDescriptor> ParameterDescriptors { get; }
  }
}
