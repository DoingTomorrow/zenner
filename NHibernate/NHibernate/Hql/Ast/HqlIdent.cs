// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlIdent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public class HqlIdent : HqlExpression
  {
    internal HqlIdent(IASTFactory factory, string ident)
      : base(125, ident, factory)
    {
    }

    internal HqlIdent(IASTFactory factory, Type type)
      : base(125, "", factory)
    {
      if (HqlIdent.IsNullableType(type))
        type = HqlIdent.ExtractUnderlyingTypeFromNullable(type);
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Boolean:
          this.SetText("bool");
          break;
        case TypeCode.Int16:
          this.SetText("short");
          break;
        case TypeCode.Int32:
          this.SetText("integer");
          break;
        case TypeCode.Int64:
          this.SetText("long");
          break;
        case TypeCode.Single:
          this.SetText("single");
          break;
        case TypeCode.Double:
          this.SetText("double");
          break;
        case TypeCode.Decimal:
          this.SetText("decimal");
          break;
        case TypeCode.DateTime:
          this.SetText("datetime");
          break;
        case TypeCode.String:
          this.SetText("string");
          break;
        default:
          if (type == typeof (Guid))
          {
            this.SetText("guid");
            break;
          }
          if (type != typeof (DateTimeOffset))
            throw new NotSupportedException(string.Format("Don't currently support idents of type {0}", (object) type.Name));
          this.SetText("datetimeoffset");
          break;
      }
    }

    private static Type ExtractUnderlyingTypeFromNullable(Type type)
    {
      return type.GetGenericArguments()[0];
    }

    private static bool IsNullableType(Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }
  }
}
