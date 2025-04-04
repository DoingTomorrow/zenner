// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.CustomExpressionXmlConverter
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System.Linq.Expressions;
using System.Xml.Linq;

#nullable disable
namespace ExpressionSerialization
{
  public abstract class CustomExpressionXmlConverter
  {
    public abstract bool TryDeserialize(XElement expressionXml, out Expression e);

    public abstract bool TrySerialize(Expression expression, out XElement x);
  }
}
