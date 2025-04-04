// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.MetaAttribute
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class MetaAttribute
  {
    private readonly string name;
    private readonly List<string> values = new List<string>();

    public MetaAttribute(string name) => this.name = name;

    public string Name => this.name;

    public IList<string> Values => (IList<string>) this.values.AsReadOnly();

    public string Value
    {
      get
      {
        return this.values.Count == 1 ? this.values[0] : throw new ArgumentException("No unique value");
      }
    }

    public bool IsMultiValued => this.values.Count > 1;

    public void AddValue(string value) => this.values.Add(value);

    public void AddValues(IEnumerable<string> range) => this.values.AddRange(range);

    public override string ToString()
    {
      return string.Format("[{0}={1}]", (object) this.name, (object) CollectionPrinter.ToString((IList) this.values));
    }
  }
}
