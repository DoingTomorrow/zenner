// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.KnownTypeExpressionXmlConverter
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  public class KnownTypeExpressionXmlConverter : CustomExpressionXmlConverter
  {
    private TypeResolver resolver;

    public KnownTypeExpressionXmlConverter(TypeResolver resolver) => this.resolver = resolver;

    public override bool TryDeserialize(XElement x, out Expression e)
    {
      if (x.Name.LocalName == typeof (ConstantExpression).Name)
      {
        Type type = this.resolver.GetType(x.Element((XName) "Type").Value);
        if (this.resolver.HasMappedKnownType(type))
        {
          using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(x.Element((XName) "Value").Value)))
          {
            object obj = new DataContractSerializer(type, (IEnumerable<Type>) this.resolver.knownTypes).ReadObject((Stream) memoryStream);
            e = (Expression) Expression.Constant(obj);
            return true;
          }
        }
      }
      e = (Expression) null;
      return false;
    }

    public override bool TrySerialize(Expression e, out XElement x)
    {
      if (e.NodeType == ExpressionType.Constant && !typeof (IQueryable).IsAssignableFrom(e.Type))
      {
        ConstantExpression constantExpression = (ConstantExpression) e;
        Type type = constantExpression.Type;
        if (constantExpression.Value != null && constantExpression.Type != constantExpression.Value.GetType())
          type = constantExpression.Value.GetType();
        if (this.resolver.HasMappedKnownType(type, out Type _))
        {
          object graph = constantExpression.Value;
          using (MemoryStream memoryStream = new MemoryStream())
          {
            new DataContractSerializer(type, (IEnumerable<Type>) this.resolver.knownTypes).WriteObject((Stream) memoryStream, graph);
            memoryStream.Position = 0L;
            string end = new StreamReader((Stream) memoryStream, Encoding.UTF8).ReadToEnd();
            x = new XElement((XName) typeof (ConstantExpression).Name, new object[3]
            {
              (object) new XAttribute((XName) "NodeType", (object) type),
              (object) new XElement((XName) "Type", (object) constantExpression.Type),
              (object) new XElement((XName) "Value", (object) end)
            });
            return true;
          }
        }
      }
      x = (XElement) null;
      return false;
    }
  }
}
