// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.DLinqSerializer
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  public static class DLinqSerializer
  {
    public static XElement SerializeQuery(this IQueryable query)
    {
      TypeResolver resolver = new TypeResolver(knownTypes: (IEnumerable<Type>) new Type[1]
      {
        query.ElementType
      });
      return new ExpressionSerializer(resolver)
      {
        Converters = {
          (CustomExpressionXmlConverter) new DLinqCustomExpressionXmlConverter((DataContext) null, resolver)
        }
      }.Serialize(query.Expression);
    }

    public static IQueryable DeserializeQuery(this DataContext dc, XElement rootXml)
    {
      TypeResolver resolver = new TypeResolver(knownTypes: DLinqSerializer.GetKnownTypesFromTables(dc));
      DLinqCustomExpressionXmlConverter expressionXmlConverter = new DLinqCustomExpressionXmlConverter(dc, resolver);
      Expression expression = new ExpressionSerializer(resolver, (IEnumerable<CustomExpressionXmlConverter>) new List<CustomExpressionXmlConverter>()
      {
        (CustomExpressionXmlConverter) expressionXmlConverter
      }).Deserialize(rootXml);
      if (expressionXmlConverter.QueryKind == null)
        throw new Exception(string.Format("CAnnot deserialize into DLinq query for datacontext {0} - no Table found", (object) dc));
      return expressionXmlConverter.QueryKind.Provider.CreateQuery(expression);
    }

    private static IEnumerable<Type> GetKnownTypesFromTables(DataContext dc)
    {
      HashSet<Type> collection = new HashSet<Type>(dc.Mapping.GetTables().Select<MetaTable, Type>((Func<MetaTable, Type>) (mt => mt.RowType.Type)));
      List<Type> knownTypesFromTables = new List<Type>((IEnumerable<Type>) collection);
      foreach (Type type in collection)
        knownTypesFromTables.Add(typeof (EntitySet<>).MakeGenericType(type));
      return (IEnumerable<Type>) knownTypesFromTables;
    }
  }
}
