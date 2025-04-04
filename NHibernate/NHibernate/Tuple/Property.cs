// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Property
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Tuple
{
  [Serializable]
  public abstract class Property
  {
    private readonly string name;
    private readonly string node;
    private readonly IType type;

    protected Property(string name, string node, IType type)
    {
      this.name = name;
      this.node = node;
      this.type = type;
    }

    public string Name => this.name;

    public string Node => this.node;

    public IType Type => this.type;

    public override string ToString()
    {
      return "Property(" + this.name + (object) ':' + this.type.Name + (object) ')';
    }
  }
}
