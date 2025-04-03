// Decompiled with JetBrains decompiler
// Type: AutoMapper.ExpressionResolutionResult
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace AutoMapper
{
  public class ExpressionResolutionResult
  {
    public Expression ResolutionExpression { get; private set; }

    public Type Type { get; private set; }

    public ExpressionResolutionResult(Expression resolutionExpression, Type type)
    {
      this.ResolutionExpression = resolutionExpression;
      this.Type = type;
    }
  }
}
