// Decompiled with JetBrains decompiler
// Type: S4_Handler._MBUS_INFO_TYPE_
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace S4_Handler
{
  internal class _MBUS_INFO_TYPE_
  {
    internal _UNIT_SCALE_ scaleUnit;
    internal DIF_Function difFunction;
    internal _VAR_TYPE_ destType;
    internal byte tarif;
    internal ushort storeNum;
    internal uint selectedVif;
    internal byte lvar_len;
    internal byte spacing_control;
    internal byte spacing_value;
    private const _VAR_TYPE_ DefaultDestinationType = _VAR_TYPE_.SELECT_READ_OUT;

    internal void LoadFromBytes(byte[] paramBytes, uint MBusInfoLength, ref int offset)
    {
      this.scaleUnit = (_UNIT_SCALE_) BitConverter.ToUInt16(paramBytes, offset);
      offset += 2;
      this.difFunction = (DIF_Function) paramBytes[offset++];
      if (MBusInfoLength == 16U)
      {
        this.lvar_len = paramBytes[offset++];
        this.spacing_control = paramBytes[offset++];
        this.spacing_value = paramBytes[offset++];
      }
      else
        ++offset;
      this.destType = (_VAR_TYPE_) paramBytes[offset++];
      this.tarif = paramBytes[offset++];
      this.storeNum = BitConverter.ToUInt16(paramBytes, offset);
      offset += 2;
      if (MBusInfoLength == 16U)
        offset += 2;
      this.selectedVif = BitConverter.ToUInt32(paramBytes, offset);
      offset += 4;
      if (!S4_DifVif_Parameter.BaseUnitScale.ContainsKey(this.scaleUnit))
        throw new Exception("Not supported UNIT_SCALE: 0x" + ((uint) this.scaleUnit).ToString("x08"));
      uint num = this.selectedVif;
      if (this.destType == _VAR_TYPE_.VAR_LEN && ((int) this.selectedVif & 16744448) == 1277952)
        num = (uint) ((ulong) this.selectedVif & 18446744073692807167UL);
      if ((int) S4_DifVif_Parameter.BaseUnitScale[this.scaleUnit].VIF_value != (int) num)
        throw new Exception("Scale unit VIF is not equal to ScaleUnit: " + this.scaleUnit.ToString() + "; VIF: 0x" + this.selectedVif.ToString("x08"));
    }

    internal void GetBytes(List<byte> byteDestination, uint MBusInfoLength)
    {
      byteDestination.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) this.scaleUnit));
      byteDestination.Add((byte) this.difFunction);
      if (MBusInfoLength == 16U)
      {
        byteDestination.Add(this.lvar_len);
        byteDestination.Add(this.spacing_control);
        byteDestination.Add(this.spacing_value);
      }
      else
        byteDestination.Add((byte) 0);
      byteDestination.Add((byte) this.destType);
      byteDestination.Add(this.tarif);
      byteDestination.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.storeNum));
      if (MBusInfoLength == 16U)
      {
        byteDestination.Add((byte) 0);
        byteDestination.Add((byte) 0);
      }
      byteDestination.AddRange((IEnumerable<byte>) BitConverter.GetBytes(this.selectedVif));
    }

    internal _MBUS_INFO_TYPE_ Clone()
    {
      return new _MBUS_INFO_TYPE_()
      {
        scaleUnit = this.scaleUnit,
        difFunction = this.difFunction,
        destType = this.destType,
        tarif = this.tarif,
        storeNum = this.storeNum,
        selectedVif = this.selectedVif,
        lvar_len = this.lvar_len,
        spacing_control = this.spacing_control,
        spacing_value = this.spacing_value
      };
    }

    internal uint GetBaseVif()
    {
      if (this.destType != _VAR_TYPE_.VAR_LEN)
        return this.selectedVif;
      if (((long) this.selectedVif & (long) sbyte.MinValue) == 4992L)
        return this.selectedVif & (uint) sbyte.MaxValue;
      if (((long) this.selectedVif & (long) short.MinValue) == 1277952L)
        return this.selectedVif & (uint) short.MaxValue;
      return ((long) this.selectedVif & -8388608L) == 327155712L ? this.selectedVif & 8388607U : this.selectedVif;
    }

    internal bool GetCompletedParameter(ref _MBUS_INFO_TYPE_ completeParameter)
    {
      if (completeParameter == null)
      {
        completeParameter = this.Clone();
      }
      else
      {
        if (completeParameter.destType == _VAR_TYPE_.SELECT_READ_OUT)
          completeParameter.destType = this.destType;
        if (completeParameter.selectedVif == (uint) byte.MaxValue || completeParameter.selectedVif == 511U)
        {
          completeParameter.selectedVif = this.selectedVif;
          completeParameter.scaleUnit = this.scaleUnit;
        }
      }
      return completeParameter.destType != _VAR_TYPE_.SELECT_READ_OUT && completeParameter.selectedVif != (uint) byte.MaxValue && completeParameter.selectedVif != 511U;
    }

    public string GetParameterName() => S4_DifVif_Parameter.GetParameterName(this);

    public override string ToString()
    {
      return "Name=" + this.GetParameterName() + "; Scale=" + this.scaleUnit.ToString() + "; VIF:0x" + this.selectedVif.ToString("x") + "; DstT:" + this.destType.ToString() + "; Tar:" + this.tarif.ToString() + "; Stnu:" + this.storeNum.ToString();
    }
  }
}
