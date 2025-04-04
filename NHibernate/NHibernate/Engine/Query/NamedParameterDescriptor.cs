// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.NamedParameterDescriptor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class NamedParameterDescriptor
  {
    private readonly string name;
    private readonly IType expectedType;
    private readonly bool jpaStyle;

    public NamedParameterDescriptor(string name, IType expectedType, bool jpaStyle)
    {
      this.name = name;
      this.expectedType = expectedType;
      this.jpaStyle = jpaStyle;
    }

    public string Name => this.name;

    public IType ExpectedType => this.expectedType;

    public bool JpaStyle => this.jpaStyle;
  }
}
