// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.tdcData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;

#nullable disable
namespace S4_Handler.Functions
{
  public class tdcData
  {
    internal DeviceMemory theMemory;
    internal uint structAddress;
    private uint structSize;
    public int[] highSampledTofDiff = new int[8];
    public byte[] highSampledWvrUp = new byte[8];
    public byte[] highSampledWvrDn = new byte[8];

    public DateTime ReadTime { get; private set; }

    public float flowWithoutCor { get; private set; }

    public float flowWithCor { get; private set; }

    public float speedOfSound { get; private set; }

    public int tofDiffAvg { get; private set; }

    public uint calibration { get; private set; }

    public int usedTofDiffAvg { get; private set; }

    public uint avgDn { get; private set; }

    public uint avgUp { get; private set; }

    public uint t1Avg { get; private set; }

    public uint t3Avg { get; private set; }

    public ushort intStatusReg { get; private set; }

    public short temperature { get; private set; }

    public ushort errorFlags { get; private set; }

    public byte wvrDnReal { get; private set; }

    public byte wvrDnIdeal { get; private set; }

    public byte wvrUpReal { get; private set; }

    public byte wvrUpIdeal { get; private set; }

    public byte tofCycles { get; private set; }

    public byte tempCycles { get; private set; }

    public ushort cOffsetUp { get; private set; }

    public ushort cOffsetDn { get; private set; }

    public byte isUnit2 { get; private set; }

    public byte wvrDnUsed { get; private set; }

    public byte wvrUpUsed { get; private set; }

    public byte latestTofCycles { get; private set; }

    public int latestTofDiffValue { get; private set; }

    public int latestTofDiffSum { get; private set; }

    public int latestTofDiffMean { get; private set; }

    public int latestTofDiffAvgNotUsed { get; private set; }

    public byte latestUsedCycles { get; private set; }

    public byte highSampledNextValue { get; private set; }

    public byte latestDummy2 { get; private set; }

    public byte latestDummy3 { get; private set; }

    public string highSampledTofDiffList { get; private set; }

    public string highSampledWvrUpList { get; private set; }

    public string highSampledWvrDnList { get; private set; }

    internal tdcData(DeviceMemory theMemory, uint structAddress, uint structSize)
    {
      this.theMemory = theMemory;
      this.structAddress = structAddress;
      this.structSize = structSize;
      this.ReadTime = DateTime.Now;
    }

    internal void RefreshData()
    {
      this.ReadTime = DateTime.Now;
      uint num1 = 0;
      this.flowWithoutCor = Parameter32bit.GetValue<float>(this.structAddress + num1, this.theMemory);
      uint num2 = num1 + 4U;
      this.flowWithCor = Parameter32bit.GetValue<float>(this.structAddress + num2, this.theMemory);
      uint num3 = num2 + 4U;
      this.speedOfSound = Parameter32bit.GetValue<float>(this.structAddress + num3, this.theMemory);
      uint num4 = num3 + 4U;
      this.tofDiffAvg = Parameter32bit.GetValue<int>(this.structAddress + num4, this.theMemory);
      uint num5 = num4 + 4U;
      this.calibration = Parameter32bit.GetValue<uint>(this.structAddress + num5, this.theMemory);
      uint num6 = num5 + 4U;
      this.usedTofDiffAvg = Parameter32bit.GetValue<int>(this.structAddress + num6, this.theMemory);
      uint num7 = num6 + 4U;
      this.avgDn = Parameter32bit.GetValue<uint>(this.structAddress + num7, this.theMemory);
      uint num8 = num7 + 4U;
      this.avgUp = Parameter32bit.GetValue<uint>(this.structAddress + num8, this.theMemory);
      uint num9 = num8 + 4U;
      this.t1Avg = Parameter32bit.GetValue<uint>(this.structAddress + num9, this.theMemory);
      uint num10 = num9 + 4U;
      this.t3Avg = Parameter32bit.GetValue<uint>(this.structAddress + num10, this.theMemory);
      uint num11 = num10 + 4U;
      this.intStatusReg = Parameter32bit.GetValue<ushort>(this.structAddress + num11, this.theMemory);
      uint num12 = num11 + 2U;
      this.temperature = Parameter32bit.GetValue<short>(this.structAddress + num12, this.theMemory);
      uint num13 = num12 + 2U;
      this.errorFlags = Parameter32bit.GetValue<ushort>(this.structAddress + num13, this.theMemory);
      uint num14 = num13 + 2U;
      this.wvrDnReal = Parameter32bit.GetValue<byte>(this.structAddress + num14, this.theMemory);
      uint num15 = num14 + 1U;
      this.wvrDnIdeal = Parameter32bit.GetValue<byte>(this.structAddress + num15, this.theMemory);
      uint num16 = num15 + 1U;
      this.wvrUpReal = Parameter32bit.GetValue<byte>(this.structAddress + num16, this.theMemory);
      uint num17 = num16 + 1U;
      this.wvrUpIdeal = Parameter32bit.GetValue<byte>(this.structAddress + num17, this.theMemory);
      uint num18 = num17 + 1U;
      this.tofCycles = Parameter32bit.GetValue<byte>(this.structAddress + num18, this.theMemory);
      uint num19 = num18 + 1U;
      this.tempCycles = Parameter32bit.GetValue<byte>(this.structAddress + num19, this.theMemory);
      uint num20 = num19 + 1U;
      this.cOffsetUp = (ushort) Parameter32bit.GetValue<byte>(this.structAddress + num20, this.theMemory);
      uint num21 = num20 + 2U;
      this.cOffsetDn = (ushort) Parameter32bit.GetValue<byte>(this.structAddress + num21, this.theMemory);
      uint num22 = num21 + 2U;
      this.isUnit2 = Parameter32bit.GetValue<byte>(this.structAddress + num22, this.theMemory);
      uint num23 = num22 + 1U;
      this.wvrDnUsed = Parameter32bit.GetValue<byte>(this.structAddress + num23, this.theMemory);
      uint num24 = num23 + 1U;
      this.wvrUpUsed = Parameter32bit.GetValue<byte>(this.structAddress + num24, this.theMemory);
      if (this.structSize > 60U)
      {
        uint num25 = num24 + 1U;
        this.latestTofCycles = Parameter32bit.GetValue<byte>(this.structAddress + num25, this.theMemory);
        uint num26 = num25 + 1U;
        this.latestTofDiffValue = Parameter32bit.GetValue<int>(this.structAddress + num26, this.theMemory);
        uint num27 = num26 + 4U;
        this.latestTofDiffSum = Parameter32bit.GetValue<int>(this.structAddress + num27, this.theMemory);
        uint num28 = num27 + 4U;
        this.latestTofDiffMean = Parameter32bit.GetValue<int>(this.structAddress + num28, this.theMemory);
        uint num29 = num28 + 4U;
        this.latestTofDiffAvgNotUsed = Parameter32bit.GetValue<int>(this.structAddress + num29, this.theMemory);
        num24 = num29 + 4U;
        this.latestUsedCycles = Parameter32bit.GetValue<byte>(this.structAddress + num24, this.theMemory);
      }
      if (this.structSize != 128U)
        return;
      uint num30 = num24 + 1U;
      this.highSampledNextValue = Parameter32bit.GetValue<byte>(this.structAddress + num30, this.theMemory);
      uint num31 = num30 + 1U;
      this.latestDummy2 = Parameter32bit.GetValue<byte>(this.structAddress + num31, this.theMemory);
      uint num32 = num31 + 1U;
      this.latestDummy3 = Parameter32bit.GetValue<byte>(this.structAddress + num32, this.theMemory);
      uint num33 = num32 + 1U;
      for (int index = 0; index < 8; ++index)
      {
        this.highSampledTofDiff[index] = Parameter32bit.GetValue<int>(this.structAddress + num33, this.theMemory);
        num33 += 4U;
        this.highSampledTofDiffList = index != 0 ? this.highSampledTofDiffList + ";" + this.highSampledTofDiff[index].ToString() : this.highSampledTofDiff[index].ToString();
      }
      for (int index = 0; index < 8; ++index)
      {
        this.highSampledWvrUp[index] = Parameter32bit.GetValue<byte>(this.structAddress + num33, this.theMemory);
        ++num33;
        this.highSampledWvrUpList = index != 0 ? this.highSampledWvrUpList + ";" + this.highSampledWvrUp[index].ToString() : this.highSampledWvrUp[index].ToString();
      }
      for (int index = 0; index < 8; ++index)
      {
        this.highSampledWvrDn[index] = Parameter32bit.GetValue<byte>(this.structAddress + num33, this.theMemory);
        ++num33;
        this.highSampledWvrDnList = index != 0 ? this.highSampledWvrDnList + ";" + this.highSampledWvrDn[index].ToString() : this.highSampledWvrDn[index].ToString();
      }
    }

    public void BuildValueLine(TextLineBuilder lineBuilder)
    {
      lineBuilder.AddFieldValue(this.flowWithoutCor.ToString());
      lineBuilder.AddFieldValue(this.flowWithCor.ToString());
      lineBuilder.AddFieldValue(this.speedOfSound.ToString());
      lineBuilder.AddFieldValue(this.tofDiffAvg.ToString());
      lineBuilder.AddFieldValue(this.calibration.ToString());
      lineBuilder.AddFieldValue(this.usedTofDiffAvg.ToString());
      lineBuilder.AddFieldValue(this.avgDn.ToString());
      lineBuilder.AddFieldValue(this.avgUp.ToString());
      lineBuilder.AddFieldValue(this.t1Avg.ToString());
      lineBuilder.AddFieldValue(this.t3Avg.ToString());
      lineBuilder.AddFieldValue(this.intStatusReg.ToString());
      lineBuilder.AddFieldValue(this.temperature.ToString());
      lineBuilder.AddFieldValue(this.errorFlags.ToString());
      lineBuilder.AddFieldValue(this.wvrDnReal.ToString());
      lineBuilder.AddFieldValue(this.wvrDnIdeal.ToString());
      lineBuilder.AddFieldValue(this.wvrUpReal.ToString());
      lineBuilder.AddFieldValue(this.wvrUpIdeal.ToString());
      lineBuilder.AddFieldValue(this.tofCycles.ToString());
      lineBuilder.AddFieldValue(this.tempCycles.ToString());
      lineBuilder.AddFieldValue(this.cOffsetUp.ToString());
      lineBuilder.AddFieldValue(this.cOffsetDn.ToString());
      lineBuilder.AddFieldValue(this.isUnit2.ToString());
      lineBuilder.AddFieldValue(this.wvrDnUsed.ToString());
      lineBuilder.AddFieldValue(this.wvrUpUsed.ToString());
      if (this.structSize > 60U)
      {
        lineBuilder.AddFieldValue(this.latestTofCycles.ToString());
        lineBuilder.AddFieldValue(this.latestTofDiffValue.ToString());
        lineBuilder.AddFieldValue(this.latestTofDiffSum.ToString());
        lineBuilder.AddFieldValue(this.latestTofDiffMean.ToString());
        lineBuilder.AddFieldValue(this.latestTofDiffAvgNotUsed.ToString());
        lineBuilder.AddFieldValue(this.latestUsedCycles.ToString());
      }
      if (this.structSize != 128U)
        return;
      lineBuilder.AddFieldValue(this.highSampledNextValue.ToString());
      lineBuilder.AddFieldValue(this.latestDummy2.ToString());
      lineBuilder.AddFieldValue(this.latestDummy3.ToString());
      lineBuilder.AddFieldValue(this.highSampledTofDiffList.ToString());
      lineBuilder.AddFieldValue(this.highSampledWvrUpList.ToString());
      lineBuilder.AddFieldValue(this.highSampledWvrDnList.ToString());
    }
  }
}
