// Decompiled with JetBrains decompiler
// Type: HandlerLib.Parameter
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  [Serializable]
  public sealed class Parameter
  {
    public string Segment { get; private set; }

    public string Name { get; private set; }

    public ushort Address { get; private set; }

    public int Size { get; private set; }

    public Parameter(string segment, string name, ushort address, int size)
    {
      this.Segment = segment;
      this.Name = name;
      this.Address = address;
      this.Size = size;
    }

    public override string ToString()
    {
      return this.Name + ", 0x" + this.Address.ToString("X4") + ", " + this.Size.ToString() + " bytes, " + this.Segment;
    }
  }
}
