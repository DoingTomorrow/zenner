// Decompiled with JetBrains decompiler
// Type: S3_Handler.FlyingTestDiagnostic
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class FlyingTestDiagnostic
  {
    private S3_HandlerFunctions myFunctions;
    internal FlyingTestData flyingTestData;
    private TdcStatusData tdcStatusData;

    internal FlyingTestDiagnostic(S3_HandlerFunctions myFunctions)
    {
      this.myFunctions = myFunctions;
    }

    internal TdcStatusData ReadTdcStatusData()
    {
      this.tdcStatusData = new TdcStatusData();
      if (this.myFunctions.MyMeters.ConnectedMeter == null)
        throw new InvalidOperationException("ConnectedMeter object not available.");
      S3_Meter connectedMeter = this.myFunctions.MyMeters.ConnectedMeter;
      List<S3_Parameter> s3ParameterList = new List<S3_Parameter>();
      int ReadAddress = 16777215;
      int num1 = 0;
      for (int index1 = 0; index1 < TdcStatusData.statusParameterList.Length; ++index1)
      {
        string key = TdcStatusData.statusParameterList[index1].ToString();
        int index2 = connectedMeter.MyParameters.ParameterByName.IndexOfKey(key);
        if (index2 < 0)
          throw new InvalidOperationException("TDC status parameters not available.");
        S3_Parameter s3Parameter = connectedMeter.MyParameters.ParameterByName.Values[index2];
        s3ParameterList.Add(s3Parameter);
        int blockStartAddress = s3Parameter.BlockStartAddress;
        int num2 = blockStartAddress + s3Parameter.ByteSize - 1;
        if (ReadAddress > blockStartAddress)
          ReadAddress = blockStartAddress;
        if (num1 < num2)
          num1 = num2;
      }
      if (!connectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(ReadAddress, num1 - ReadAddress + 1))
        throw new InvalidOperationException("Read error on ReadTdcStatusData");
      foreach (S3_Parameter s3Parameter in s3ParameterList)
      {
        uint num3 = s3Parameter.ByteSize != 1 ? s3Parameter.GetUintValue() : (uint) s3Parameter.GetByteValue();
        this.tdcStatusData.statusParameterValues.Add(s3Parameter.Name, num3);
      }
      return this.tdcStatusData;
    }

    internal string GetTdcStatusReport()
    {
      if (this.tdcStatusData == null)
        throw new InvalidOperationException("TdcStatus not available");
      StringBuilder testReport = new StringBuilder();
      testReport.AppendLine();
      testReport.AppendLine("-------------------------------------------------------------");
      testReport.AppendLine("TDC status report");
      testReport.AppendLine();
      for (int index = 0; index < this.tdcStatusData.statusParameterValues.Count; ++index)
      {
        string key = this.tdcStatusData.statusParameterValues.Keys[index];
        uint num = this.tdcStatusData.statusParameterValues.Values[index];
        testReport.Append(key + "= 0x");
        string str = key;
        if (str != null)
        {
          switch (str.Length)
          {
            case 14:
              if (str == "lastTdcHwError")
              {
                testReport.Append(num.ToString("x02"));
                this.AddHwFlags(num, testReport);
                testReport.AppendLine();
                continue;
              }
              continue;
            case 16:
              if (str == "lastTdcErrorTime")
              {
                testReport.Append(num.ToString("x04"));
                DateTime dateTime = ZR_Calendar.Cal_GetDateTime(num);
                testReport.AppendLine("; = " + dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString());
                continue;
              }
              continue;
            case 18:
              switch (str[0])
              {
                case 'l':
                  if (str == "lastTdcStatusError")
                  {
                    testReport.Append(num.ToString("x02"));
                    this.AddStatusFlags(num, testReport);
                    testReport.AppendLine();
                    continue;
                  }
                  continue;
                case 't':
                  if (str == "tdcHwErrorFlagsOld")
                  {
                    testReport.Append(num.ToString("x02"));
                    this.AddHwFlags(num, testReport);
                    testReport.AppendLine();
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            case 22:
              if (str == "tdcStatusErrorFlagsOld")
              {
                testReport.Append(num.ToString("x02"));
                this.AddStatusFlags(num, testReport);
                testReport.AppendLine();
                continue;
              }
              continue;
            case 24:
              if (str == "numberchangedTdcHwErrors")
              {
                testReport.Append(num.ToString("x02"));
                testReport.AppendLine();
                continue;
              }
              continue;
            case 28:
              if (str == "numberchangedTdcStatusErrors")
              {
                testReport.Append(num.ToString("x02"));
                testReport.AppendLine();
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      return testReport.ToString();
    }

    private void AddHwFlags(uint flagValue, StringBuilder testReport)
    {
      testReport.Append(" -> ");
      if ((flagValue & 1U) > 0U)
        testReport.Append(" STATUS_TIMEOUT_TDC ");
      if ((flagValue & 2U) > 0U)
        testReport.Append(" STATUS_TIMEOUT_PRECOUNTER ");
      if ((flagValue & 4U) > 0U)
        testReport.Append(" NOT_DEFINED");
      if ((flagValue & 16U) > 0U)
        testReport.Append(" NO_INT_TDC_MEAS");
      if ((flagValue & 32U) > 0U)
        testReport.Append(" NO_INT_TDC_CALIB ");
      if ((flagValue & 64U) > 0U)
        testReport.Append(" CHECK_SPI_NO_COM ");
      if ((flagValue & 128U) <= 0U)
        return;
      testReport.Append(" POR_CHECK_SPI_NO_COM ");
    }

    private void AddStatusFlags(uint flagValue, StringBuilder testReport)
    {
      testReport.Append(" -> ");
      switch (flagValue & 240U)
      {
        case 16:
          testReport.Append(" ULTRASONIC_MEAS_SYSTEM ");
          break;
        case 32:
          testReport.Append(" EMPTY_TUBE ");
          break;
        case 48:
          testReport.Append(" BACKFLOW ");
          break;
        case 64:
          testReport.Append(" BUBBLING ");
          break;
        case 80:
          testReport.Append(" OVERLOAD ");
          break;
        case 96:
          testReport.Append(" OVERLOAD_BACKFLOW ");
          break;
      }
    }

    internal FlyingTestData ReadFlyingTestData()
    {
      S3_Meter connectedMeter = this.myFunctions.MyMeters.ConnectedMeter;
      if (connectedMeter.MyIdentification.IsUltrasonic)
        this.ReadTdcStatusData();
      this.flyingTestData = (FlyingTestData) null;
      int index = connectedMeter.MyParameters.AddressLables.IndexOfKey("radioData");
      if (index < 0)
        throw new InvalidOperationException("FlyingTestDiagnostic not supported from this firmware or radioData parameter not prepared.");
      ByteField MemoryData;
      if (!this.myFunctions.MyCommands.ReadMemory(MemoryLocation.FLASH, connectedMeter.MyParameters.AddressLables.Values[index], 34, out MemoryData) || MemoryData.Data.Length != 34)
        throw new InvalidOperationException("Read error on ReadFlyingTestData");
      this.flyingTestData = new FlyingTestData(MemoryData.Data);
      return this.flyingTestData;
    }

    internal string GetFlyingTestReport()
    {
      if (this.flyingTestData == null)
        throw new InvalidOperationException("testReport not available");
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine();
      stringBuilder1.AppendLine("-------------------------------------------------------------");
      stringBuilder1.AppendLine("Flying test report");
      stringBuilder1.AppendLine(" PC- -> calculated by computer");
      StringBuilder stringBuilder2 = stringBuilder1;
      ushort num1 = this.flyingTestData.TimeStart_4ms;
      string str1 = "TimeStart (16Bit-Counter, 1/256s Auflösung)[Inc]: " + num1.ToString();
      stringBuilder2.Append(str1);
      StringBuilder stringBuilder3 = stringBuilder1;
      double num2 = (double) this.flyingTestData.TimeStart_4ms * (1.0 / 256.0);
      string str2 = "   PC-TimeStart[s]: " + num2.ToString();
      stringBuilder3.AppendLine(str2);
      StringBuilder stringBuilder4 = stringBuilder1;
      num1 = this.flyingTestData.TimeStop_4ms;
      string str3 = "TimeStop (16Bit-Counter, 1/256s Auflösung)[Inc]: " + num1.ToString();
      stringBuilder4.Append(str3);
      StringBuilder stringBuilder5 = stringBuilder1;
      num2 = (double) this.flyingTestData.TimeStop_4ms * (1.0 / 256.0);
      string str4 = "   PC-TimeStop[s] : " + num2.ToString();
      stringBuilder5.AppendLine(str4);
      stringBuilder1.Append("TimeVolUsed   [Inc]: " + this.flyingTestData.TimeVolUsed_4ms.ToString());
      StringBuilder stringBuilder6 = stringBuilder1;
      num2 = (double) this.flyingTestData.TimeVolUsed_4ms * (1.0 / 256.0);
      string str5 = "   PC-TimeVolUsed   [s]: " + num2.ToString();
      stringBuilder6.AppendLine(str5);
      StringBuilder stringBuilder7 = stringBuilder1;
      num1 = this.flyingTestData.TimeVolNotUsed_4ms;
      string str6 = "TimeVolNotUsed[Inc]: " + num1.ToString();
      stringBuilder7.Append(str6);
      StringBuilder stringBuilder8 = stringBuilder1;
      num2 = (double) this.flyingTestData.TimeVolNotUsed_4ms * (1.0 / 256.0);
      string str7 = "   PC-TimeVolNotUsed[s]: " + num2.ToString();
      stringBuilder8.AppendLine(str7);
      DateTime dateTime1 = ZR_Calendar.Cal_GetDateTime(this.flyingTestData.TimeStart_s);
      DateTime dateTime2 = ZR_Calendar.Cal_GetDateTime(this.flyingTestData.TimeStop_s);
      TimeSpan timeSpan = dateTime2.Subtract(dateTime1);
      StringBuilder stringBuilder9 = stringBuilder1;
      uint num3 = this.flyingTestData.TimeStart_s;
      string str8 = "TimeStart[s]: " + num3.ToString() + " ->   PC-" + dateTime1.ToLongTimeString();
      stringBuilder9.Append(str8);
      StringBuilder stringBuilder10 = stringBuilder1;
      num3 = this.flyingTestData.TimeStop_s;
      string str9 = "    TimeStop[s]: " + num3.ToString() + " ->   PC-" + dateTime2.ToLongTimeString();
      stringBuilder10.Append(str9);
      StringBuilder stringBuilder11 = stringBuilder1;
      num2 = timeSpan.TotalSeconds;
      string str10 = "   PC-RunTime[s]: " + num2.ToString("#.000");
      stringBuilder11.AppendLine(str10);
      double num4 = (double) ((int) (ushort) ((uint) this.flyingTestData.TimeStop_4ms - (uint) this.flyingTestData.TimeStart_4ms) + (int) (timeSpan.TotalSeconds * 256.0 / 65536.0) * 65536) / 256.0;
      stringBuilder1.AppendLine("*** PC-RunTime[s] calculated from s and 4ms counter: " + num4.ToString("#.00000"));
      stringBuilder1.Append("*** VolComplete[l]: " + this.flyingTestData.VolComplete.ToString());
      stringBuilder1.Append("    VolTimeVolUsed[l]: " + this.flyingTestData.VolTimeVolUsed.ToString());
      stringBuilder1.AppendLine("   PC-VolTimeVolNotUsed[l]: " + ((float) this.flyingTestData.TimeVolNotUsed_4ms / (float) this.flyingTestData.TimeVolUsed_4ms * this.flyingTestData.VolTimeVolUsed).ToString());
      stringBuilder1.Append("*** FlowStart[l/h]: " + this.flyingTestData.FlowStart.ToString());
      stringBuilder1.AppendLine("    FlowStop[l/h]: " + this.flyingTestData.FlowStop.ToString());
      if (this.myFunctions.MyMeters.ConnectedMeter.MyIdentification.IsUltrasonic)
        stringBuilder1.AppendLine(this.GetTdcStatusReport());
      return stringBuilder1.ToString();
    }
  }
}
