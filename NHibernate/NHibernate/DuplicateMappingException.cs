// Decompiled with JetBrains decompiler
// Type: NHibernate.DuplicateMappingException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class DuplicateMappingException : MappingException
  {
    private readonly string type;
    private readonly string name;

    public string Type => this.type;

    public string Name => this.name;

    public DuplicateMappingException(string customMessage, string type, string name)
      : base(customMessage)
    {
      this.type = type;
      this.name = name;
    }

    public DuplicateMappingException(string type, string name)
      : this(string.Format("Duplicate {0} mapping {1}", (object) type, (object) name), type, name)
    {
    }

    public DuplicateMappingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
