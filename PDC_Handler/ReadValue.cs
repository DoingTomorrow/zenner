// Decompiled with JetBrains decompiler
// Type: PDC_Handler.ReadValue
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;

#nullable disable
namespace PDC_Handler
{
  public struct ReadValue
  {
    public uint A;
    public uint B;

    public byte[] Buffer
    {
      get
      {
        byte[] bytes1 = BitConverter.GetBytes(this.A);
        byte[] bytes2 = BitConverter.GetBytes(this.B);
        byte[] buffer = new byte[bytes1.Length + bytes2.Length];
        bytes1.CopyTo((Array) buffer, 0);
        bytes2.CopyTo((Array) buffer, bytes1.Length);
        return buffer;
      }
    }

    public override string ToString()
    {
      return string.Format("A: {0}, B: {1}", (object) this.A, (object) this.B);
    }
  }
}
