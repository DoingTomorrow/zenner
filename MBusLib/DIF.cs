// Decompiled with JetBrains decompiler
// Type: MBusLib.DIF
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class DIF : IPrintable
  {
    public byte Value { get; set; }

    public bool HasExtension => ((int) this.Value & 128) == 128;

    public long LsbOfStorageNumber => (long) (((int) this.Value & 64) >> 6);

    public FunctionField FunctionField => (FunctionField) (((int) this.Value & 48) >> 4);

    public DataField DataField => (DataField) ((uint) this.Value & 15U);

    public int DataFieldLength
    {
      get
      {
        switch (this.DataField)
        {
          case DataField.NoData:
            return 0;
          case DataField.Integer8bit:
            return 1;
          case DataField.Integer16bit:
            return 2;
          case DataField.Integer24bit:
            return 3;
          case DataField.Integer32bit:
            return 4;
          case DataField.Real32bit:
            return 4;
          case DataField.Integer48bit:
            return 6;
          case DataField.Integer64bit:
            return 8;
          case DataField.SelectionForReadout:
            return 0;
          case DataField.Bcd2Digit:
            return 1;
          case DataField.Bcd4Digit:
            return 2;
          case DataField.Bcd6Digit:
            return 3;
          case DataField.Bcd8Digit:
            return 4;
          case DataField.VariableLength:
            return 0;
          case DataField.Bcd12Digit:
            return 6;
          case DataField.SpecialFunctions:
            return 0;
          default:
            throw new NotImplementedException("DataField");
        }
      }
    }

    public override string ToString() => "0x" + this.Value.ToString("X2");

    public string Print(int spaces = 0) => Util.PrintObject((object) this);

    public static DIF Parse(byte value)
    {
      return new DIF() { Value = value };
    }
  }
}
