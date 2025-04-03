// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.FunctionPrecompiled
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  [Serializable]
  public sealed class FunctionPrecompiled
  {
    private const byte DII_CODE_VARIABLE = 5;
    private const byte DII_CODE_EDIT = 2;
    private const byte DII_CODE_CONSTANT = 4;
    private const ushort DII_VARIABLE_FLAG_BYTES = 768;
    private const ushort DII_VARIABLE_FLAG_RUNTIME = 512;
    private const ushort DII_VARIABLE_FLAG_MASK = 1024;
    private const ushort DII_VARIABLE_FLAG_MIN_MAX = 4096;
    private const byte DII_FRAME_SEGS1 = 1;
    private const byte SEGS1_FRAME1 = 8;
    private const byte SEGS1_FRAME2 = 16;
    private const byte SEGS1_FRAME3 = 32;
    public byte[] Codes;

    public int FunctionNumber { get; set; }

    public short RecordOrder { get; set; }

    public FunctionRecordType RecordType { get; set; }

    public string Name { get; set; }

    public int Offset { get; set; }

    public string CodesAsHex { get; set; }

    public int? OffsetOfVariableOrEditFramecode { get; set; }

    public bool? UseVisibleAfterPointFrame { get; set; }

    public FunctionPrecompiled(
      int FunctionNumber,
      short RecordOrder,
      FunctionRecordType RecordType,
      string Name,
      int Offset,
      byte[] Codes)
    {
      this.FunctionNumber = FunctionNumber;
      this.RecordOrder = RecordOrder;
      this.RecordType = RecordType;
      this.Name = Name;
      this.Offset = Offset;
      this.Codes = Codes;
      if (Codes == null || Codes.Length == 0)
        return;
      this.CodesAsHex = BitConverter.ToString(Codes).Replace("-", " ");
      if (RecordType == FunctionRecordType.DisplayCode)
      {
        ushort num1 = (ushort) ((uint) Codes[1] | (uint) Codes[2] << 8);
        if (Codes[0] == (byte) 5)
          this.OffsetOfVariableOrEditFramecode = ((int) num1 & 768) != 512 ? new int?(5) : new int?(3);
        else if (Codes[0] == (byte) 2)
        {
          int num2 = 2;
          int num3 = ((int) num1 & 768) != 512 ? num2 + 3 : num2 + 1;
          if (((uint) num1 & 1024U) > 0U)
            num3 += 9;
          if (((uint) num1 & 4096U) > 0U)
            num3 += 9;
          this.OffsetOfVariableOrEditFramecode = new int?(num3);
        }
        else if (Codes[0] == (byte) 4)
          this.OffsetOfVariableOrEditFramecode = new int?(1);
        int? variableOrEditFramecode = this.OffsetOfVariableOrEditFramecode;
        if (variableOrEditFramecode.HasValue)
        {
          byte[] codes1 = this.Codes;
          variableOrEditFramecode = this.OffsetOfVariableOrEditFramecode;
          int index1 = variableOrEditFramecode.Value;
          if (((uint) codes1[index1] & 1U) > 0U)
          {
            byte[] codes2 = this.Codes;
            variableOrEditFramecode = this.OffsetOfVariableOrEditFramecode;
            int index2 = variableOrEditFramecode.Value + 1;
            this.UseVisibleAfterPointFrame = ((uint) codes2[index2] & 56U) <= 0U ? new bool?(false) : new bool?(true);
          }
        }
      }
    }

    public override string ToString()
    {
      return string.Format("#{0} {1} {2} {3}", (object) this.FunctionNumber, (object) this.RecordOrder, (object) this.Name, (object) this.RecordType);
    }
  }
}
