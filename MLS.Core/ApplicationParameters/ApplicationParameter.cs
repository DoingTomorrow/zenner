// Decompiled with JetBrains decompiler
// Type: MLS.Core.Model.ApplicationParameters.ApplicationParameter
// Assembly: MLS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7FC8C31B-A3E8-40E1-84D2-E43683A764BC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MLS.Core.dll

#nullable disable
namespace MLS.Core.Model.ApplicationParameters
{
  public class ApplicationParameter
  {
    public virtual int Id { get; set; }

    public virtual string Parameter { get; set; }

    public virtual string Value { get; set; }

    public virtual string Label { get; set; }

    public virtual string Category { get; set; }

    public virtual string Description { get; set; }

    public virtual ViewObjectTypeEnum ViewObjectType { get; set; }

    public virtual DataTypeEnum DataType { get; set; }

    public virtual string Scope { get; set; }

    public virtual int OrderInCategory { get; set; }

    public virtual int OrderInPage { get; set; }
  }
}
