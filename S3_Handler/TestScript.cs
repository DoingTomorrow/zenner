// Decompiled with JetBrains decompiler
// Type: S3_Handler.TestScript
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using DeviceCollector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class TestScript
  {
    private const int maxRes = 20;
    private TestWindowCommunication windowCommunication;
    private S3_Meter myMeter;
    private double[] vars = new double[20];
    private List<RandomX> randoms = new List<RandomX>();
    private SortedList<int, string> scriptLines;
    private List<string[]> scriptLineElements;
    private SortedList<string, int> lables;
    private int scriptLineIndex;

    internal TestScript(S3_Meter myMeter, TestWindowCommunication windowCommunication)
    {
      this.myMeter = myMeter;
      this.windowCommunication = windowCommunication;
      for (int index = 0; index < 20; ++index)
        this.randoms.Add(new RandomX());
    }

    internal void RunScript(string fileName)
    {
      try
      {
        this.scriptLines = new SortedList<int, string>();
        this.scriptLineElements = new List<string[]>();
        this.lables = new SortedList<string, int>();
        using (StreamReader streamReader = new StreamReader(fileName))
        {
          int key = 0;
          this.WaitForSigleStep();
          string str1;
          while ((str1 = streamReader.ReadLine()) != null && !this.windowCommunication.breakRequest)
          {
            ++key;
            string str2 = str1.Trim();
            if (str2.Length > 0 && str2[0] != '#')
            {
              this.scriptLines.Add(key, str2);
              int length = str2.IndexOf("#");
              if (length >= 0)
                str2 = str2.Substring(0, length).Trim();
              string[] strArray = str2.Split(new string[1]
              {
                " "
              }, StringSplitOptions.RemoveEmptyEntries);
              this.scriptLineElements.Add(strArray);
              if (strArray[0] == "lable")
                this.lables.Add(strArray[1], this.scriptLineElements.Count - 1);
            }
          }
        }
        this.scriptLineIndex = 0;
        while (this.scriptLineIndex < this.scriptLines.Count)
          this.WorkScriptLine();
        this.windowCommunication.textBoxState.AppendText(Environment.NewLine);
        this.windowCommunication.textBoxState.AppendText(Environment.NewLine);
        this.windowCommunication.textBoxState.AppendText("*** Serie3 test script done ***");
        this.windowCommunication.textBoxState.AppendText(Environment.NewLine);
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("*** Exception ***");
        stringBuilder.AppendLine();
        if (this.scriptLines != null && this.scriptLines.Count > this.scriptLineIndex)
        {
          stringBuilder.AppendLine("Script line number:" + this.scriptLines.Keys[this.scriptLineIndex].ToString());
          stringBuilder.AppendLine("Script line: " + this.scriptLines.Values[this.scriptLineIndex]);
          stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine("Exception text:");
        stringBuilder.AppendLine(ex.ToString());
        string messageAndClearError = ZR_ClassLibMessages.GetLastErrorMessageAndClearError();
        stringBuilder.AppendLine("Error text:");
        stringBuilder.AppendLine(messageAndClearError);
        this.windowCommunication.textBoxState.AppendText(stringBuilder.ToString());
      }
    }

    private void WorkScriptLine()
    {
      string[] scriptLineElement = this.scriptLineElements[this.scriptLineIndex];
      string str = scriptLineElement[0];
      this.windowCommunication.textBoxState.AppendText(this.scriptLines.Values[this.scriptLineIndex]);
      this.windowCommunication.textBoxState.AppendText(Environment.NewLine);
      switch (str)
      {
        case "jump":
          this.scriptLineIndex = this.lables[scriptLineElement[1]];
          break;
        case "jump_!=":
          if (this.GetDoubleParameter(scriptLineElement[1]) != this.GetDoubleParameter(scriptLineElement[2]))
          {
            this.scriptLineIndex = this.lables[scriptLineElement[3]];
            break;
          }
          goto case "lable";
        case "lable":
        case "print":
          ++this.scriptLineIndex;
          break;
        case "pause":
          this.WaitForSigleStep();
          goto case "lable";
        default:
          if (str != "wait")
          {
            this.SingleStepIsOn();
            if (this.windowCommunication.breakRequest)
            {
              this.scriptLineIndex = this.scriptLines.Count;
              break;
            }
          }
          if (str == "wait")
            this.WorkScriptCommand_WaitSeconds(this.GetDoubleParameter(scriptLineElement[1]));
          else if (str.StartsWith("var"))
            this.WorkVar();
          else if (str.StartsWith("iniRandom"))
            this.WorkScriptCommand_IniRandom();
          else if (str.StartsWith("set"))
          {
            this.SetMeterVar();
          }
          else
          {
            if (!str.StartsWith("func"))
              throw new Exception("Unknown command");
            this.CallFunction(scriptLineElement);
          }
          goto case "lable";
      }
    }

    private double GetDoubleParameter(string param)
    {
      double doubleParameter;
      if (param.StartsWith("random"))
      {
        int index = int.Parse(param.Substring(6));
        doubleParameter = this.randoms[index].NextDouble() * this.randoms[index].randomFactor + this.randoms[index].randomOffset;
      }
      else
        doubleParameter = !param.StartsWith("var") ? (!param.StartsWith("dt:") ? (!char.IsLetter(param[0]) ? double.Parse(param) : this.GetMeterVar(param)) : this.GetDateTimeValue(param.Substring(3))) : this.vars[int.Parse(param.Substring(3))];
      return doubleParameter;
    }

    private void WorkVar()
    {
      string[] scriptLineElement = this.scriptLineElements[this.scriptLineIndex];
      int index = int.Parse(scriptLineElement[0].Substring(3));
      string str = scriptLineElement[1];
      if (str != null)
      {
        switch (str.Length)
        {
          case 1:
            if (str == "=")
            {
              double doubleParameter = this.GetDoubleParameter(scriptLineElement[2]);
              this.vars[index] = doubleParameter;
              break;
            }
            goto label_18;
          case 2:
            switch (str[0])
            {
              case '*':
                if (str == "*=")
                {
                  double doubleParameter = this.GetDoubleParameter(scriptLineElement[2]);
                  this.vars[index] *= doubleParameter;
                  break;
                }
                goto label_18;
              case '+':
                if (str == "+=")
                {
                  double doubleParameter = this.GetDoubleParameter(scriptLineElement[2]);
                  this.vars[index] += doubleParameter;
                  break;
                }
                goto label_18;
              case '-':
                if (str == "-=")
                {
                  double doubleParameter = this.GetDoubleParameter(scriptLineElement[2]);
                  this.vars[index] -= doubleParameter;
                  break;
                }
                goto label_18;
              case '/':
                if (str == "/=")
                {
                  double doubleParameter = this.GetDoubleParameter(scriptLineElement[2]);
                  this.vars[index] /= doubleParameter;
                  break;
                }
                goto label_18;
              default:
                goto label_18;
            }
            break;
          case 3:
            switch (str[2])
            {
              case 'a':
                if (str == "dta")
                {
                  double num = this.AddDateTimeMask(scriptLineElement[2], this.vars[index]);
                  this.vars[index] = num;
                  break;
                }
                goto label_18;
              case 'm':
                if (str == "dtm")
                {
                  double num = this.UseDateTimeMask(scriptLineElement[2], this.vars[index]);
                  this.vars[index] = num;
                  break;
                }
                goto label_18;
              default:
                goto label_18;
            }
            break;
          default:
            goto label_18;
        }
        this.windowCommunication.textBoxState.AppendText("> " + scriptLineElement[0] + " = " + this.vars[index].ToString());
        this.windowCommunication.textBoxState.AppendText(Environment.NewLine);
        return;
      }
label_18:
      throw new Exception("Illegal var operation");
    }

    private void SetMeterVar()
    {
      string[] scriptLineElement = this.scriptLineElements[this.scriptLineIndex];
      string key = scriptLineElement[1];
      double doubleParameter = this.GetDoubleParameter(scriptLineElement[2]);
      if (key == "SimulatedVolume")
      {
        this.myMeter.MyFunctions.SimulateVolume(doubleParameter);
      }
      else
      {
        S3_Parameter s3Parameter = this.myMeter.MyParameters.ParameterByName[key];
        byte[] ByteArray;
        switch (s3Parameter.Statics.S3_VarType)
        {
          case S3_VariableTypes.INT8:
            ByteArray = new byte[1]
            {
              BitConverter.GetBytes((short) doubleParameter)[0]
            };
            break;
          case S3_VariableTypes.UINT8:
            ByteArray = BitConverter.GetBytes((short) (byte) doubleParameter);
            break;
          case S3_VariableTypes.UINT16:
            ByteArray = BitConverter.GetBytes((ushort) doubleParameter);
            break;
          case S3_VariableTypes.INT16:
            ByteArray = BitConverter.GetBytes((short) doubleParameter);
            break;
          case S3_VariableTypes.UINT32:
          case S3_VariableTypes.MeterTime1980:
            ByteArray = BitConverter.GetBytes((uint) doubleParameter);
            break;
          case S3_VariableTypes.INT32:
            ByteArray = BitConverter.GetBytes((int) doubleParameter);
            break;
          case S3_VariableTypes.REAL32:
            ByteArray = BitConverter.GetBytes((float) doubleParameter);
            break;
          case S3_VariableTypes.INT64:
            ByteArray = BitConverter.GetBytes((long) doubleParameter);
            break;
          case S3_VariableTypes.UINT64:
            ByteArray = BitConverter.GetBytes((ulong) doubleParameter);
            break;
          case S3_VariableTypes.REAL64:
            ByteArray = BitConverter.GetBytes(doubleParameter);
            break;
          default:
            throw new Exception("Convertiong error. double -> meter");
        }
        ByteField data = new ByteField(ByteArray);
        ZR_ClassLibMessages.ClearErrors();
        if (!this.myMeter.MyFunctions.MyCommands.WriteMemory(MemoryLocation.RAM, s3Parameter.BlockStartAddress, data))
          throw new Exception("Write parameter error");
      }
    }

    private double GetMeterVar(string paramName)
    {
      S3_Parameter s3Parameter = this.myMeter.MyParameters.ParameterByName[paramName];
      ByteField MemoryData;
      if (!this.myMeter.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, s3Parameter.BlockStartAddress, s3Parameter.ByteSize, out MemoryData))
        throw new Exception("Read parameter error");
      double meterVar;
      switch (s3Parameter.Statics.S3_VarType)
      {
        case S3_VariableTypes.INT8:
          byte[] numArray = new byte[2]
          {
            MemoryData.Data[0],
            (byte) 0
          };
          numArray[1] = numArray[0] >= (byte) 128 ? byte.MaxValue : (byte) 0;
          meterVar = (double) BitConverter.ToInt16(numArray, 0);
          break;
        case S3_VariableTypes.UINT8:
          meterVar = (double) MemoryData.Data[0];
          break;
        case S3_VariableTypes.UINT16:
          meterVar = (double) BitConverter.ToUInt16(MemoryData.Data, 0);
          break;
        case S3_VariableTypes.INT16:
          meterVar = (double) BitConverter.ToInt16(MemoryData.Data, 0);
          break;
        case S3_VariableTypes.UINT32:
        case S3_VariableTypes.MeterTime1980:
          meterVar = (double) BitConverter.ToUInt32(MemoryData.Data, 0);
          break;
        case S3_VariableTypes.INT32:
          meterVar = (double) BitConverter.ToInt32(MemoryData.Data, 0);
          break;
        case S3_VariableTypes.REAL32:
          meterVar = (double) BitConverter.ToSingle(MemoryData.Data, 0);
          break;
        case S3_VariableTypes.INT64:
          meterVar = (double) BitConverter.ToInt64(MemoryData.Data, 0);
          break;
        case S3_VariableTypes.UINT64:
          meterVar = (double) BitConverter.ToUInt64(MemoryData.Data, 0);
          break;
        case S3_VariableTypes.REAL64:
          meterVar = BitConverter.ToDouble(MemoryData.Data, 0);
          break;
        default:
          throw new Exception("Convertiong error. Meter -> double");
      }
      return meterVar;
    }

    private void CallFunction(string[] lineElements)
    {
      switch (lineElements[0])
      {
        case "funcReset":
          if (this.myMeter.MyFunctions.MyCommands.ResetDevice())
          {
            this.ShowLine("> ok");
            break;
          }
          this.ShowLine("> err");
          break;
        case "funcIrReset":
          if (this.myMeter.MyFunctions.MyCommands.SetOptoTimeoutSeconds(0))
          {
            this.ShowLine("> ok");
            break;
          }
          this.ShowLine("> err");
          break;
        case "funcSetCom":
          this.myMeter.MyFunctions.MyCommands.DisableBusWriteOnDispose();
          this.myMeter.MyFunctions.MyCommands.SingleParameter(CommParameter.Baudrate, lineElements[2]);
          switch (lineElements[1])
          {
            case "irr":
              this.myMeter.MyFunctions.MyCommands.SingleParameter(CommParameter.IrDaSelection, IrDaSelection.RoundSide.ToString());
              break;
            case "ird":
              this.myMeter.MyFunctions.MyCommands.SingleParameter(CommParameter.IrDaSelection, IrDaSelection.DoveTailSide.ToString());
              break;
            case "uart":
              this.myMeter.MyFunctions.MyCommands.SingleParameter(CommParameter.IrDaSelection, IrDaSelection.None.ToString());
              break;
            default:
              throw new Exception("Illegal opto head setting");
          }
          this.myMeter.MyFunctions.MyCommands.ChangeDriverSettings();
          break;
        case "funcGetVersion":
          long Connected_Version;
          if (this.myMeter.MyFunctions.MyCommands.ReadVersion(out short _, out byte _, out byte _, out Connected_Version, out int _, out int _, out int _))
          {
            this.ShowLine("> ok: Version = " + ParameterService.GetVersionString(Connected_Version, 8));
            break;
          }
          this.ShowLine("> err");
          break;
        case "funcIoTest":
          byte[] numArray = this.myMeter.MyFunctions.MyCommands.RunIoTest(IoTestFunctions.IoTest_Run);
          if (numArray != null)
          {
            string str = string.Empty;
            for (int index = 0; index < numArray.Length; ++index)
              str = str + " " + numArray[index].ToString("x02");
            this.ShowLine("> ok: Result =" + str);
            break;
          }
          this.ShowLine("> err");
          break;
        default:
          throw new Exception("Illegal function");
      }
    }

    private void WorkScriptCommand_IniRandom()
    {
      string[] scriptLineElement = this.scriptLineElements[this.scriptLineIndex];
      double doubleParameter1 = this.GetDoubleParameter(scriptLineElement[1]);
      double doubleParameter2 = this.GetDoubleParameter(scriptLineElement[2]);
      int index = int.Parse(scriptLineElement[0].Substring(9));
      this.randoms[index].randomOffset = doubleParameter1;
      this.randoms[index].randomFactor = doubleParameter2 - doubleParameter1;
    }

    private double GetDateTimeValue(string dateTimeString)
    {
      double dateTimeValue = 0.0;
      string[] strArray1 = dateTimeString.Split('_');
      string[] strArray2 = strArray1[0].Split('.');
      string[] strArray3 = strArray1[1].Split(':');
      int meterTime = (int) ZR_Calendar.Cal_GetMeterTime(new DateTime(int.Parse(strArray2[2]), int.Parse(strArray2[1]), int.Parse(strArray2[0]), int.Parse(strArray3[0]), int.Parse(strArray3[1]), int.Parse(strArray3[2])));
      return dateTimeValue;
    }

    private double UseDateTimeMask(string dateTimeString, double valueToMask)
    {
      string[] strArray1 = dateTimeString.Split('_');
      string[] strArray2 = strArray1[0].Split('.');
      string[] strArray3 = strArray1[1].Split(':');
      DateTime dateTime = ZR_Calendar.Cal_GetDateTime((uint) valueToMask);
      int year = dateTime.Year;
      int month = dateTime.Month;
      int day = dateTime.Day;
      int hour = dateTime.Hour;
      int minute = dateTime.Minute;
      int second = dateTime.Second;
      int num1 = 0;
      int months = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      if (strArray2[2] != "=")
      {
        if (strArray2[2].StartsWith("+"))
          num1 += int.Parse(strArray2[2].Substring(1));
        else
          year = int.Parse(strArray2[2]);
      }
      if (strArray2[1] != "=")
      {
        if (strArray2[1].StartsWith("+"))
          months += int.Parse(strArray2[1].Substring(1));
        else
          month = int.Parse(strArray2[1]);
      }
      if (strArray2[0] != "=")
      {
        if (strArray2[0].StartsWith("+"))
          num2 += int.Parse(strArray2[0].Substring(1));
        else
          day = int.Parse(strArray2[0]);
      }
      if (strArray3[0] != "=")
      {
        if (strArray3[0].StartsWith("+"))
          num3 += int.Parse(strArray3[0].Substring(1));
        else
          hour = int.Parse(strArray3[0]);
      }
      if (strArray3[1] != "=")
      {
        if (strArray3[1].StartsWith("+"))
          num4 += int.Parse(strArray3[1].Substring(1));
        else
          minute = int.Parse(strArray3[1]);
      }
      if (strArray3[2] != "=")
      {
        if (strArray3[2].StartsWith("+"))
          num5 += int.Parse(strArray3[2].Substring(1));
        else
          second = int.Parse(strArray3[2]);
      }
      DateTime TheTime = new DateTime(year, month, day, hour, minute, second);
      TheTime = TheTime.AddYears(num1);
      TheTime = TheTime.AddMonths(months);
      TheTime = TheTime.AddDays((double) num2);
      TheTime = TheTime.AddHours((double) num3);
      TheTime = TheTime.AddMinutes((double) num4);
      TheTime = TheTime.AddSeconds((double) num5);
      double meterTime = (double) ZR_Calendar.Cal_GetMeterTime(TheTime);
      this.ShowLine("> " + TheTime.ToString("dd.MM.yyyy HH:mm:ss"));
      return meterTime;
    }

    private double AddDateTimeMask(string dateTimeString, double valueToMask)
    {
      double num = 0.0;
      string[] strArray1 = dateTimeString.Split('_');
      string[] strArray2 = strArray1[0].Split('.');
      string[] strArray3 = strArray1[1].Split(':');
      DateTime TheTime = ZR_Calendar.Cal_GetDateTime((uint) valueToMask);
      if (strArray2[2] != "=")
        TheTime = TheTime.AddYears(int.Parse(strArray2[2]));
      if (strArray2[1] != "=")
        TheTime = TheTime.AddMonths(int.Parse(strArray2[1]));
      if (strArray2[0] != "=")
        TheTime = TheTime.AddDays((double) int.Parse(strArray2[0]));
      if (strArray3[0] != "=")
        TheTime = TheTime.AddHours((double) int.Parse(strArray3[0]));
      if (strArray3[1] != "=")
        TheTime = TheTime.AddMinutes((double) int.Parse(strArray3[1]));
      if (strArray3[2] != "=")
        TheTime = TheTime.AddSeconds((double) int.Parse(strArray3[2]));
      int meterTime = (int) ZR_Calendar.Cal_GetMeterTime(TheTime);
      return num;
    }

    private bool WorkScriptCommand_SetTime(string parameter)
    {
      DateTime result;
      if (!DateTime.TryParse(parameter, out result))
      {
        this.ShowLine("> Illegal time value");
        return false;
      }
      ulong meterTime = (ulong) ZR_Calendar.Cal_GetMeterTime(result);
      this.ShowLine("> SetTime " + result.ToString("dd.MM.yyyy HH.mm.ss") + " = 0x" + meterTime.ToString("x08"));
      if (this.myMeter.SetDeviceTime(result))
        this.windowCommunication.textBoxState.AppendText("> done");
      else
        this.windowCommunication.textBoxState.AppendText("> set time error");
      this.windowCommunication.textBoxState.AppendText(Environment.NewLine);
      return true;
    }

    private bool WorkScriptCommand_WaitSeconds(double Seconds)
    {
      double num = Seconds;
      this.windowCommunication.textBoxState.AppendText("> Wait seconds: ");
      int length = this.windowCommunication.textBoxState.Text.Length;
      while (Seconds > 0.0 && !this.windowCommunication.breakRequest)
      {
        this.windowCommunication.textBoxState.AppendText(((int) Seconds).ToString());
        int millisecondsTimeout = (int) (Seconds * 1000.0);
        if (millisecondsTimeout > 1000)
          Thread.Sleep(1000);
        else
          Thread.Sleep(millisecondsTimeout);
        --Seconds;
        this.windowCommunication.textBoxState.Select(length, 1000);
        this.windowCommunication.textBoxState.Cut();
        this.windowCommunication.textBoxState.Refresh();
        Application.DoEvents();
      }
      this.ShowLine(num.ToString() + " --> done");
      return true;
    }

    private void SingleStepIsOn()
    {
      if (!this.windowCommunication.singleStepOn)
        return;
      this.WaitForSigleStep();
    }

    private void WaitForSigleStep()
    {
      int length = this.windowCommunication.textBoxState.Text.Length;
      this.windowCommunication.textBoxState.AppendText("> Wait for continue: 0");
      int num1 = 0;
      int num2 = 0;
      while (!this.windowCommunication.breakRequest && !this.windowCommunication.continueSingleStep)
      {
        ++num1;
        if (num1 == 10)
        {
          num1 = 0;
          ++num2;
          this.windowCommunication.textBoxState.Select(length, 1000);
          this.windowCommunication.textBoxState.Cut();
          this.windowCommunication.textBoxState.AppendText("> Wait for continue: " + num2.ToString());
          this.windowCommunication.textBoxState.Refresh();
          Application.DoEvents();
        }
        Thread.Sleep(100);
        Application.DoEvents();
      }
      this.windowCommunication.textBoxState.Select(length, 1000);
      this.windowCommunication.textBoxState.Cut();
      this.windowCommunication.textBoxState.Refresh();
      Application.DoEvents();
      this.windowCommunication.continueSingleStep = false;
    }

    private void ShowLine(string line)
    {
      this.windowCommunication.textBoxState.AppendText(line);
      this.windowCommunication.textBoxState.AppendText(Environment.NewLine);
    }
  }
}
