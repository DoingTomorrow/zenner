// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.DependencyChain
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  internal class DependencyChain
  {
    internal List<FormulaCell> list = new List<FormulaCell>();
    internal Dictionary<ulong, int> index = new Dictionary<ulong, int>();
    internal List<int> CalcOrder = new List<int>();

    internal void Add(FormulaCell f)
    {
      this.list.Add(f);
      f.Index = this.list.Count - 1;
      this.index.Add(ExcelCellBase.GetCellID(f.SheetID, f.Row, f.Column), f.Index);
    }
  }
}
