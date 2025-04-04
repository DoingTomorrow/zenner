// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.OrdinalParameterDescriptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class OrdinalParameterDescriptor
  {
    private readonly int ordinalPosition;
    private readonly IType expectedType;

    public OrdinalParameterDescriptor(int ordinalPosition, IType expectedType)
    {
      this.ordinalPosition = ordinalPosition;
      this.expectedType = expectedType;
    }

    public int OrdinalPosition => this.ordinalPosition;

    public IType ExpectedType => this.expectedType;
  }
}
