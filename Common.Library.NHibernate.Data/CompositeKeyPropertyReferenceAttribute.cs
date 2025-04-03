// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.CompositeKeyPropertyReferenceAttribute
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using System;

#nullable disable
namespace Common.Library.NHibernate.Data
{
  public class CompositeKeyPropertyReferenceAttribute : Attribute
  {
    public string ReferencedPrimaryKey { get; set; }

    public string CompositeIdProperties { get; set; }
  }
}
