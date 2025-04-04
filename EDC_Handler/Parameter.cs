// Decompiled with JetBrains decompiler
// Type: EDC_Handler.Parameter
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System.Diagnostics;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
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
