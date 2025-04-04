// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.DLinqCustomExpressionXmlConverter
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  internal class DLinqCustomExpressionXmlConverter : CustomExpressionXmlConverter
  {
    private DataContext dc;
    private TypeResolver resolver;

    public IQueryable QueryKind { get; private set; }

    public DLinqCustomExpressionXmlConverter(DataContext dc, TypeResolver resolver)
    {
      this.dc = dc;
      this.resolver = resolver;
    }

    public override bool TryDeserialize(XElement expressionXml, out Expression e)
    {
      if (expressionXml.Name.LocalName == "Table")
      {
        ITable table = this.dc.GetTable(this.resolver.GetType(expressionXml.Attribute((XName) "Type").Value));
        this.QueryKind = (IQueryable) table;
        e = (Expression) Expression.Constant((object) table);
        return true;
      }
      e = (Expression) null;
      return false;
    }

    public override bool TrySerialize(Expression expression, out XElement x)
    {
      if (typeof (IQueryService).IsAssignableFrom(expression.Type))
      {
        x = new XElement((XName) "Table", (object) new XAttribute((XName) "Type", (object) expression.Type.GetGenericArguments()[0].FullName));
        return true;
      }
      x = (XElement) null;
      return false;
    }
  }
}
