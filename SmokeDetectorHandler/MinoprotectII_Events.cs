// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.MinoprotectII_Events
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class MinoprotectII_Events
  {
    public DateTime? DateActivation1 { get; private set; }

    public DateTime? DateActivation2 { get; private set; }

    public DateTime? DateActivation3 { get; private set; }

    public DateTime? DateActivation4 { get; private set; }

    public DateTime? DateActivation5 { get; private set; }

    public DateTime? DateAlarmEvent1 { get; private set; }

    public DateTime? DateAlarmEvent2 { get; private set; }

    public DateTime? DateAlarmEvent3 { get; private set; }

    public DateTime? DateAlarmEvent4 { get; private set; }

    public DateTime? DateAlarmEvent5 { get; private set; }

    public DateTime? DateAlarmEvent6 { get; private set; }

    public DateTime? DateAlarmEvent7 { get; private set; }

    public DateTime? DateAlarmEvent8 { get; private set; }

    public DateTime? DateAlarmEvent9 { get; private set; }

    public DateTime? DateAlarmEvent10 { get; private set; }

    public uint RemoteAlarmID1 { get; private set; }

    public uint RemoteAlarmID2 { get; private set; }

    public uint RemoteAlarmID3 { get; private set; }

    public uint RemoteAlarmID4 { get; private set; }

    public uint RemoteAlarmID5 { get; private set; }

    public uint RemoteAlarmID6 { get; private set; }

    public uint RemoteAlarmID7 { get; private set; }

    public uint RemoteAlarmID8 { get; private set; }

    public uint RemoteAlarmID9 { get; private set; }

    public uint RemoteAlarmID10 { get; private set; }

    public byte AlarmTypeOfRemoteDevice1 { get; private set; }

    public byte AlarmTypeOfRemoteDevice2 { get; private set; }

    public byte AlarmTypeOfRemoteDevice3 { get; private set; }

    public byte AlarmTypeOfRemoteDevice4 { get; private set; }

    public byte AlarmTypeOfRemoteDevice5 { get; private set; }

    public byte AlarmTypeOfRemoteDevice6 { get; private set; }

    public byte AlarmTypeOfRemoteDevice7 { get; private set; }

    public byte AlarmTypeOfRemoteDevice8 { get; private set; }

    public byte AlarmTypeOfRemoteDevice9 { get; private set; }

    public byte AlarmTypeOfRemoteDevice10 { get; private set; }

    public FaultIdentification IdentificationFirstFault { get; private set; }

    public DateTime? DateFirstFault { get; private set; }

    public FaultIdentification IdentificationFault1 { get; private set; }

    public FaultIdentification IdentificationFault2 { get; private set; }

    public FaultIdentification IdentificationFault3 { get; private set; }

    public FaultIdentification IdentificationFault4 { get; private set; }

    public FaultIdentification IdentificationFault5 { get; private set; }

    public FaultIdentification IdentificationFault6 { get; private set; }

    public FaultIdentification IdentificationFault7 { get; private set; }

    public FaultIdentification IdentificationFault8 { get; private set; }

    public FaultIdentification IdentificationFault9 { get; private set; }

    public FaultIdentification IdentificationFault10 { get; private set; }

    public DateTime? DateFault1 { get; private set; }

    public DateTime? DateFault2 { get; private set; }

    public DateTime? DateFault3 { get; private set; }

    public DateTime? DateFault4 { get; private set; }

    public DateTime? DateFault5 { get; private set; }

    public DateTime? DateFault6 { get; private set; }

    public DateTime? DateFault7 { get; private set; }

    public DateTime? DateFault8 { get; private set; }

    public DateTime? DateFault9 { get; private set; }

    public DateTime? DateFault10 { get; private set; }

    public byte[] Buffer { get; private set; }

    internal static MinoprotectII_Events Parse(byte[] buffer)
    {
      if (buffer == null)
        throw new NullReferenceException("Can not parse events of smoke detector! The buffer is null.");
      if (buffer.Length != 166)
        throw new NullReferenceException("Can not parse events of smoke detector! Wrong length of buffer. Expected: 166 bytes, Actual: " + buffer.Length.ToString() + " bytes.");
      MinoprotectII_Events minoprotectIiEvents1 = new MinoprotectII_Events();
      minoprotectIiEvents1.Buffer = buffer;
      int offset1 = 0;
      minoprotectIiEvents1.DateActivation1 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset1);
      int offset2 = offset1 + 4;
      minoprotectIiEvents1.DateActivation2 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset2);
      int offset3 = offset2 + 4;
      minoprotectIiEvents1.DateActivation3 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset3);
      int offset4 = offset3 + 4;
      minoprotectIiEvents1.DateActivation4 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset4);
      int offset5 = offset4 + 4;
      minoprotectIiEvents1.DateActivation5 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset5);
      int offset6 = offset5 + 4;
      minoprotectIiEvents1.DateAlarmEvent1 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset6);
      int offset7 = offset6 + 4;
      minoprotectIiEvents1.DateAlarmEvent2 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset7);
      int offset8 = offset7 + 4;
      minoprotectIiEvents1.DateAlarmEvent3 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset8);
      int offset9 = offset8 + 4;
      minoprotectIiEvents1.DateAlarmEvent4 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset9);
      int offset10 = offset9 + 4;
      minoprotectIiEvents1.DateAlarmEvent5 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset10);
      int offset11 = offset10 + 4;
      minoprotectIiEvents1.DateAlarmEvent6 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset11);
      int offset12 = offset11 + 4;
      minoprotectIiEvents1.DateAlarmEvent7 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset12);
      int offset13 = offset12 + 4;
      minoprotectIiEvents1.DateAlarmEvent8 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset13);
      int offset14 = offset13 + 4;
      minoprotectIiEvents1.DateAlarmEvent9 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset14);
      int offset15 = offset14 + 4;
      minoprotectIiEvents1.DateAlarmEvent10 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset15);
      int index1 = offset15 + 4;
      minoprotectIiEvents1.RemoteAlarmID1 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index1],
        buffer[index1 + 1],
        buffer[index1 + 2],
        (byte) 0
      }, 0);
      int num1 = index1 + 3;
      MinoprotectII_Events minoprotectIiEvents2 = minoprotectIiEvents1;
      byte[] numArray1 = buffer;
      int index2 = num1;
      int index3 = index2 + 1;
      int num2 = (int) numArray1[index2];
      minoprotectIiEvents2.AlarmTypeOfRemoteDevice1 = (byte) num2;
      minoprotectIiEvents1.RemoteAlarmID2 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index3],
        buffer[index3 + 1],
        buffer[index3 + 2],
        (byte) 0
      }, 0);
      int num3 = index3 + 3;
      MinoprotectII_Events minoprotectIiEvents3 = minoprotectIiEvents1;
      byte[] numArray2 = buffer;
      int index4 = num3;
      int index5 = index4 + 1;
      int num4 = (int) numArray2[index4];
      minoprotectIiEvents3.AlarmTypeOfRemoteDevice2 = (byte) num4;
      minoprotectIiEvents1.RemoteAlarmID3 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index5],
        buffer[index5 + 1],
        buffer[index5 + 2],
        (byte) 0
      }, 0);
      int num5 = index5 + 3;
      MinoprotectII_Events minoprotectIiEvents4 = minoprotectIiEvents1;
      byte[] numArray3 = buffer;
      int index6 = num5;
      int index7 = index6 + 1;
      int num6 = (int) numArray3[index6];
      minoprotectIiEvents4.AlarmTypeOfRemoteDevice3 = (byte) num6;
      minoprotectIiEvents1.RemoteAlarmID4 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index7],
        buffer[index7 + 1],
        buffer[index7 + 2],
        (byte) 0
      }, 0);
      int num7 = index7 + 3;
      MinoprotectII_Events minoprotectIiEvents5 = minoprotectIiEvents1;
      byte[] numArray4 = buffer;
      int index8 = num7;
      int index9 = index8 + 1;
      int num8 = (int) numArray4[index8];
      minoprotectIiEvents5.AlarmTypeOfRemoteDevice4 = (byte) num8;
      minoprotectIiEvents1.RemoteAlarmID5 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index9],
        buffer[index9 + 1],
        buffer[index9 + 2],
        (byte) 0
      }, 0);
      int num9 = index9 + 3;
      MinoprotectII_Events minoprotectIiEvents6 = minoprotectIiEvents1;
      byte[] numArray5 = buffer;
      int index10 = num9;
      int index11 = index10 + 1;
      int num10 = (int) numArray5[index10];
      minoprotectIiEvents6.AlarmTypeOfRemoteDevice5 = (byte) num10;
      minoprotectIiEvents1.RemoteAlarmID6 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index11],
        buffer[index11 + 1],
        buffer[index11 + 2],
        (byte) 0
      }, 0);
      int num11 = index11 + 3;
      MinoprotectII_Events minoprotectIiEvents7 = minoprotectIiEvents1;
      byte[] numArray6 = buffer;
      int index12 = num11;
      int index13 = index12 + 1;
      int num12 = (int) numArray6[index12];
      minoprotectIiEvents7.AlarmTypeOfRemoteDevice6 = (byte) num12;
      minoprotectIiEvents1.RemoteAlarmID7 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index13],
        buffer[index13 + 1],
        buffer[index13 + 2],
        (byte) 0
      }, 0);
      int num13 = index13 + 3;
      MinoprotectII_Events minoprotectIiEvents8 = minoprotectIiEvents1;
      byte[] numArray7 = buffer;
      int index14 = num13;
      int index15 = index14 + 1;
      int num14 = (int) numArray7[index14];
      minoprotectIiEvents8.AlarmTypeOfRemoteDevice7 = (byte) num14;
      minoprotectIiEvents1.RemoteAlarmID8 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index15],
        buffer[index15 + 1],
        buffer[index15 + 2],
        (byte) 0
      }, 0);
      int num15 = index15 + 3;
      MinoprotectII_Events minoprotectIiEvents9 = minoprotectIiEvents1;
      byte[] numArray8 = buffer;
      int index16 = num15;
      int index17 = index16 + 1;
      int num16 = (int) numArray8[index16];
      minoprotectIiEvents9.AlarmTypeOfRemoteDevice8 = (byte) num16;
      minoprotectIiEvents1.RemoteAlarmID9 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index17],
        buffer[index17 + 1],
        buffer[index17 + 2],
        (byte) 0
      }, 0);
      int num17 = index17 + 3;
      MinoprotectII_Events minoprotectIiEvents10 = minoprotectIiEvents1;
      byte[] numArray9 = buffer;
      int index18 = num17;
      int index19 = index18 + 1;
      int num18 = (int) numArray9[index18];
      minoprotectIiEvents10.AlarmTypeOfRemoteDevice9 = (byte) num18;
      minoprotectIiEvents1.RemoteAlarmID10 = BitConverter.ToUInt32(new byte[4]
      {
        buffer[index19],
        buffer[index19 + 1],
        buffer[index19 + 2],
        (byte) 0
      }, 0);
      int num19 = index19 + 3;
      MinoprotectII_Events minoprotectIiEvents11 = minoprotectIiEvents1;
      byte[] numArray10 = buffer;
      int index20 = num19;
      int startIndex1 = index20 + 1;
      int num20 = (int) numArray10[index20];
      minoprotectIiEvents11.AlarmTypeOfRemoteDevice10 = (byte) num20;
      minoprotectIiEvents1.IdentificationFirstFault = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex1);
      int offset16 = startIndex1 + 2;
      minoprotectIiEvents1.DateFirstFault = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset16);
      int startIndex2 = offset16 + 4;
      minoprotectIiEvents1.IdentificationFault1 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex2);
      int offset17 = startIndex2 + 2;
      minoprotectIiEvents1.DateFault1 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset17);
      int startIndex3 = offset17 + 4;
      minoprotectIiEvents1.IdentificationFault2 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex3);
      int offset18 = startIndex3 + 2;
      minoprotectIiEvents1.DateFault2 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset18);
      int startIndex4 = offset18 + 4;
      minoprotectIiEvents1.IdentificationFault3 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex4);
      int offset19 = startIndex4 + 2;
      minoprotectIiEvents1.DateFault3 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset19);
      int startIndex5 = offset19 + 4;
      minoprotectIiEvents1.IdentificationFault4 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex5);
      int offset20 = startIndex5 + 2;
      minoprotectIiEvents1.DateFault4 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset20);
      int startIndex6 = offset20 + 4;
      minoprotectIiEvents1.IdentificationFault5 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex6);
      int offset21 = startIndex6 + 2;
      minoprotectIiEvents1.DateFault5 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset21);
      int startIndex7 = offset21 + 4;
      minoprotectIiEvents1.IdentificationFault6 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex7);
      int offset22 = startIndex7 + 2;
      minoprotectIiEvents1.DateFault6 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset22);
      int startIndex8 = offset22 + 4;
      minoprotectIiEvents1.IdentificationFault7 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex8);
      int offset23 = startIndex8 + 2;
      minoprotectIiEvents1.DateFault7 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset23);
      int startIndex9 = offset23 + 4;
      minoprotectIiEvents1.IdentificationFault8 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex9);
      int offset24 = startIndex9 + 2;
      minoprotectIiEvents1.DateFault8 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset24);
      int startIndex10 = offset24 + 4;
      minoprotectIiEvents1.IdentificationFault9 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex10);
      int offset25 = startIndex10 + 2;
      minoprotectIiEvents1.DateFault9 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset25);
      int startIndex11 = offset25 + 4;
      minoprotectIiEvents1.IdentificationFault10 = (FaultIdentification) BitConverter.ToUInt16(buffer, startIndex11);
      int offset26 = startIndex11 + 2;
      minoprotectIiEvents1.DateFault10 = Util.ConvertToDateTime_MBus_CP32_TypeF(buffer, offset26);
      int num21 = offset26 + 4;
      return minoprotectIiEvents1;
    }

    public string ToString(int spaces)
    {
      StringBuilder sb = new StringBuilder();
      this.Add(ref sb, spaces, this.DateActivation1, "1  Date Activation:");
      this.Add(ref sb, spaces, this.DateActivation2, "2  Date Activation:");
      this.Add(ref sb, spaces, this.DateActivation3, "3  Date Activation:");
      this.Add(ref sb, spaces, this.DateActivation4, "4  Date Activation:");
      this.Add(ref sb, spaces, this.DateActivation5, "5  Date Activation:");
      sb.AppendLine();
      this.Add(ref sb, spaces, this.DateAlarmEvent1, "1  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent2, "2  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent3, "3  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent4, "4  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent5, "5  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent6, "6  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent7, "7  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent8, "8  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent9, "9  Date Alarm Event:");
      this.Add(ref sb, spaces, this.DateAlarmEvent10, "10 Date Alarm Event:");
      sb.AppendLine();
      this.Add(ref sb, spaces, this.RemoteAlarmID1, "1  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice1, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID2, "2  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice2, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID3, "3  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice3, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID4, "4  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice4, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID5, "5  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice5, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID6, "6  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice6, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID7, "7  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice7, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID8, "8  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice8, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID9, "9  Remote Alarm ID:", this.AlarmTypeOfRemoteDevice9, "Type:");
      this.Add(ref sb, spaces, this.RemoteAlarmID10, "10 Remote Alarm ID:", this.AlarmTypeOfRemoteDevice10, "Type:");
      sb.AppendLine();
      this.Add(ref sb, spaces, this.IdentificationFirstFault, "Identification First Fault:", this.DateFirstFault, "Date:");
      sb.AppendLine();
      this.Add(ref sb, spaces, this.IdentificationFault1, "1  Identification Fault:", this.DateFault1, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault2, "2  Identification Fault:", this.DateFault2, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault3, "3  Identification Fault:", this.DateFault3, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault4, "4  Identification Fault:", this.DateFault4, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault5, "5  Identification Fault:", this.DateFault5, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault6, "6  Identification Fault:", this.DateFault6, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault7, "7  Identification Fault:", this.DateFault7, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault8, "8  Identification Fault:", this.DateFault8, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault9, "9  Identification Fault:", this.DateFault9, "Date:");
      this.Add(ref sb, spaces, this.IdentificationFault10, "10 Identification Fault:", this.DateFault10, "Date:");
      return sb.ToString();
    }

    private void Add(
      ref StringBuilder sb,
      int spaces,
      FaultIdentification value1,
      string text1,
      DateTime? value2,
      string text2)
    {
      if (value2.HasValue)
        sb.Append(text1.PadRight(spaces, ' ')).Append(value1.ToString()).Append(", ").Append(text2).Append(' ').AppendLine(value2.Value.ToString());
      else
        sb.Append(text1.PadRight(spaces, ' ')).Append(value1.ToString()).Append(", ").Append(text2).Append(' ').AppendLine("Invalid date");
    }

    private void Add(ref StringBuilder sb, int spaces, FaultIdentification value, string text)
    {
      sb.Append(text.PadRight(spaces, ' ')).AppendLine(value.ToString());
    }

    private void Add(
      ref StringBuilder sb,
      int spaces,
      uint value1,
      string text1,
      byte value2,
      string text2)
    {
      sb.Append(text1.PadRight(spaces, ' ')).Append(value1.ToString()).Append(", ").Append(text2).Append(' ').AppendLine(value2.ToString());
    }

    private void Add(ref StringBuilder sb, int spaces, DateTime? date, string text)
    {
      if (date.HasValue)
        sb.Append(text.PadRight(spaces, ' ')).AppendLine(date.Value.ToString());
      else
        sb.Append(text.PadRight(spaces, ' ')).AppendLine("Invalid date");
    }

    internal MinoprotectII_Events DeepCopy()
    {
      return new MinoprotectII_Events()
      {
        DateActivation1 = this.DateActivation1,
        DateActivation2 = this.DateActivation2,
        DateActivation3 = this.DateActivation3,
        DateActivation4 = this.DateActivation4,
        DateActivation5 = this.DateActivation5,
        DateAlarmEvent1 = this.DateAlarmEvent1,
        DateAlarmEvent2 = this.DateAlarmEvent2,
        DateAlarmEvent3 = this.DateAlarmEvent3,
        DateAlarmEvent4 = this.DateAlarmEvent4,
        DateAlarmEvent5 = this.DateAlarmEvent5,
        DateAlarmEvent6 = this.DateAlarmEvent6,
        DateAlarmEvent7 = this.DateAlarmEvent7,
        DateAlarmEvent8 = this.DateAlarmEvent8,
        DateAlarmEvent9 = this.DateAlarmEvent9,
        DateAlarmEvent10 = this.DateAlarmEvent10,
        RemoteAlarmID1 = this.RemoteAlarmID1,
        RemoteAlarmID2 = this.RemoteAlarmID2,
        RemoteAlarmID3 = this.RemoteAlarmID3,
        RemoteAlarmID4 = this.RemoteAlarmID4,
        RemoteAlarmID5 = this.RemoteAlarmID5,
        RemoteAlarmID6 = this.RemoteAlarmID6,
        RemoteAlarmID7 = this.RemoteAlarmID7,
        RemoteAlarmID8 = this.RemoteAlarmID8,
        RemoteAlarmID9 = this.RemoteAlarmID9,
        RemoteAlarmID10 = this.RemoteAlarmID10,
        AlarmTypeOfRemoteDevice1 = this.AlarmTypeOfRemoteDevice1,
        AlarmTypeOfRemoteDevice2 = this.AlarmTypeOfRemoteDevice2,
        AlarmTypeOfRemoteDevice3 = this.AlarmTypeOfRemoteDevice3,
        AlarmTypeOfRemoteDevice4 = this.AlarmTypeOfRemoteDevice4,
        AlarmTypeOfRemoteDevice5 = this.AlarmTypeOfRemoteDevice5,
        AlarmTypeOfRemoteDevice6 = this.AlarmTypeOfRemoteDevice6,
        AlarmTypeOfRemoteDevice7 = this.AlarmTypeOfRemoteDevice7,
        AlarmTypeOfRemoteDevice8 = this.AlarmTypeOfRemoteDevice8,
        AlarmTypeOfRemoteDevice9 = this.AlarmTypeOfRemoteDevice9,
        AlarmTypeOfRemoteDevice10 = this.AlarmTypeOfRemoteDevice10,
        IdentificationFirstFault = this.IdentificationFirstFault,
        DateFirstFault = this.DateFirstFault,
        IdentificationFault1 = this.IdentificationFault1,
        IdentificationFault2 = this.IdentificationFault2,
        IdentificationFault3 = this.IdentificationFault3,
        IdentificationFault4 = this.IdentificationFault4,
        IdentificationFault5 = this.IdentificationFault5,
        IdentificationFault6 = this.IdentificationFault6,
        IdentificationFault7 = this.IdentificationFault7,
        IdentificationFault8 = this.IdentificationFault8,
        IdentificationFault9 = this.IdentificationFault9,
        IdentificationFault10 = this.IdentificationFault10,
        DateFault1 = this.DateFault1,
        DateFault2 = this.DateFault2,
        DateFault3 = this.DateFault3,
        DateFault4 = this.DateFault4,
        DateFault5 = this.DateFault5,
        DateFault6 = this.DateFault6,
        DateFault7 = this.DateFault7,
        DateFault8 = this.DateFault8,
        DateFault9 = this.DateFault9,
        DateFault10 = this.DateFault10,
        Buffer = this.Buffer
      };
    }
  }
}
