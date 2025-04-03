// Decompiled with JetBrains decompiler
// Type: PDC_Handler.Parameter
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System.Diagnostics;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  [DebuggerDisplay("{Name} {Address} {Type}")]
  public sealed class Parameter
  {
    public int MapID { get; set; }

    public string Name { get; set; }

    public ushort Address { get; set; }

    public int Size { get; set; }

    public string DifVif { get; set; }

    public S3_VariableTypes Type { get; set; }

    public override string ToString() => this.Name;
  }
}
