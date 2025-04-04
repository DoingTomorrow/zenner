// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.VBA.ExcelVbaReferenceControl
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.VBA
{
  public class ExcelVbaReferenceControl : ExcelVbaReference
  {
    public ExcelVbaReferenceControl() => this.ReferenceRecordID = 47;

    public string LibIdExternal { get; set; }

    public string LibIdTwiddled { get; set; }

    public Guid OriginalTypeLib { get; set; }

    internal uint Cookie { get; set; }
  }
}
