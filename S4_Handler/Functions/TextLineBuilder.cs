// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.TextLineBuilder
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace S4_Handler.Functions
{
  public class TextLineBuilder
  {
    private int LineRowIndex;
    private List<int> LineAlignments = new List<int>();
    private StringBuilder LineResultAligned = new StringBuilder();
    private StringBuilder LineResultCondensed = new StringBuilder();

    public void AddFieldValue(string fieldValue = null)
    {
      if (fieldValue == null)
        fieldValue = "";
      this.AddField(fieldValue.Trim());
    }

    public void AddField(string field = null)
    {
      if (field == null)
        field = "";
      if (this.LineRowIndex > 0)
      {
        this.LineResultAligned.Append("; ");
        this.LineResultCondensed.Append(";");
      }
      if (this.LineRowIndex >= this.LineAlignments.Count)
        this.LineAlignments.Add(field.Length);
      else if (field.Length > this.LineAlignments[this.LineRowIndex])
        this.LineAlignments[this.LineRowIndex] = field.Length;
      this.LineResultCondensed.Append(field);
      this.LineResultAligned.Append(field.PadLeft(this.LineAlignments[this.LineRowIndex]));
      ++this.LineRowIndex;
    }

    public void ClearLine()
    {
      this.LineResultCondensed.Clear();
      this.LineResultAligned.Clear();
      this.LineRowIndex = 0;
    }

    private void ClearAlignments() => this.LineAlignments.Clear();

    public override string ToString() => this.LineResultAligned.ToString() + Environment.NewLine;

    public string ToCondencedString() => this.LineResultCondensed.ToString() + Environment.NewLine;
  }
}
