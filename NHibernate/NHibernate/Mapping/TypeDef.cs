// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.TypeDef
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class TypeDef
  {
    private readonly string typeClass;
    private readonly IDictionary<string, string> parameters;

    public TypeDef(string typeClass, IDictionary<string, string> parameters)
    {
      this.typeClass = typeClass;
      this.parameters = parameters;
    }

    public string TypeClass => this.typeClass;

    public IDictionary<string, string> Parameters => this.parameters;
  }
}
