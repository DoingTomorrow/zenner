// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.Interpreter
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

#nullable disable
namespace SmartFunctionCompiler
{
  public class Interpreter
  {
    internal static Register RegA = new Register();
    internal static Register RegB = new Register();
    private static Register RegT = new Register();
    private static bool CompareResult;
    private static byte OpcodeCounter;
    internal static Interpreter.RunConditions RunCondition = Interpreter.RunConditions.NotDefined;
    internal static ushort InstructionPointer;
    private static FunctionAccessStorage fas;
    public static SmartFunctionResult Error = SmartFunctionResult.NoError;
    public static ushort ErrorOffset = 0;
    internal static float Flow = 5f;
    internal static float FlowIncrement = 1f;
    internal static double Volume = 12345.0;
    internal static double FlowVolume = 12345.0;
    internal static double ReturnVolume = 12345.0;
    internal static double VolumeIncrement = 1.0;
    internal static uint CycleTime = 3600;
    internal static uint DeviceTime = 1000;
    internal static StringBuilder RunLog = new StringBuilder();

    public static bool IsError => Interpreter.Error != 0;

    public static ObservableCollection<SimulatorParameter> SimulatorParameters { get; set; }

    static Interpreter()
    {
      Interpreter.SimulatorParameters = new ObservableCollection<SimulatorParameter>();
      double num = 10000.0;
      foreach (string name in Enum.GetNames(typeof (DeviceStateCounterID)))
      {
        SimulatorParameter simulatorParameter = new SimulatorParameter(name);
        simulatorParameter.StartValue = num;
        simulatorParameter.ValueIncrement = (double) Interpreter.CycleTime;
        simulatorParameter.Value = num + 1.0;
        num += 10000.0;
        Interpreter.SimulatorParameters.Add(simulatorParameter);
      }
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("A"));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("FlowMin", new double?(0.025)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("FlowQ1", new double?(0.05)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("FlowQ2", new double?(0.08)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("FlowQ3", new double?(40.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("FlowQ4", new double?(50.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("FlowMax", new double?(55.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("Temp", new double?(17.3)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("MaxHourlyFlow", new double?(20.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("MinHourlyFlow", new double?(0.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("MaxHourlyTemp", new double?(10.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("MinHourlyTemp", new double?(8.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("Max5MinutesFlow", new double?(20.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("Min5MinutesFlow", new double?(0.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("Max5MinutesTemp", new double?(10.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("Min5MinutesTemp", new double?(8.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("SmartFunctionState", new double?(0.0)));
      Interpreter.SimulatorParameters.Add(new SimulatorParameter("SmartFunctionTimoutActive", new double?(0.0)));
    }

    internal static void SimulationCycle()
    {
      Interpreter.VolumeIncrement = (double) Interpreter.Flow * (double) Interpreter.CycleTime / 3600.0;
      Interpreter.Flow += Interpreter.FlowIncrement;
      Interpreter.Volume += Interpreter.VolumeIncrement;
      if (Interpreter.VolumeIncrement > 0.0)
        Interpreter.FlowVolume += Interpreter.VolumeIncrement;
      else
        Interpreter.ReturnVolume += Interpreter.VolumeIncrement;
      Interpreter.DeviceTime += Interpreter.CycleTime;
      foreach (SimulatorParameter simulatorParameter in (Collection<SimulatorParameter>) Interpreter.SimulatorParameters)
        simulatorParameter.Value += simulatorParameter.ValueIncrement;
    }

    internal static void Run(ushort functionOffset)
    {
      Interpreter.Error = SmartFunctionResult.NoError;
      Interpreter.ErrorOffset = (ushort) 0;
      Interpreter.fas = new FunctionAccessStorage(FunctionLoader.FlashStorage, functionOffset);
      if (Interpreter.fas.SmartFunctionResult != 0)
      {
        Interpreter.RunLog.AppendLine();
        Interpreter.RunLog.AppendLine("***** Function block by previous error *****");
        Interpreter.RunLog.AppendLine();
      }
      else
      {
        FunctionRamHeader functionRamHeader = new FunctionRamHeader(Interpreter.fas.FunctionRamHeaderOffset);
        Interpreter.RunLog.AppendLine();
        switch (Interpreter.RunCondition)
        {
          case Interpreter.RunConditions.HardwareReset:
            Interpreter.RunLog.AppendLine("Run HardwareReset");
            break;
          case Interpreter.RunConditions.FunctionLoad:
            Interpreter.RunLog.AppendLine("Run FunctionLoad");
            break;
          default:
            Interpreter.RunLog.AppendLine("Run Event");
            ++functionRamHeader.FunctionCallCounter;
            break;
        }
        Interpreter.RunLog.AppendLine("FunctionCallCounter: " + functionRamHeader.FunctionCallCounter.ToString());
        if (Interpreter.RunCondition == Interpreter.RunConditions.Event)
        {
          Interpreter.InstructionPointer = Interpreter.fas.CodeStartOffset;
        }
        else
        {
          if (Interpreter.fas.NumberOfResetCodeBytes == (byte) 0)
            return;
          Interpreter.InstructionPointer = Interpreter.fas.ResetCodeStartOffset;
        }
        Interpreter.OpcodeCounter = (byte) 0;
        try
        {
          do
          {
            Interpreter.ErrorOffset = Interpreter.InstructionPointer;
          }
          while (Interpreter.WorkOpcode() && !Interpreter.IsError);
          if (Interpreter.IsError)
          {
            Interpreter.fas.SmartFunctionResult = Interpreter.Error;
            Interpreter.fas.ErrorOffset = (ushort) ((uint) Interpreter.ErrorOffset - (uint) Interpreter.fas.RuntimeCodeStorageOffset);
          }
        }
        catch (Exception ex)
        {
          throw new Exception("Runtime exception at offset: 0x" + ((int) Interpreter.ErrorOffset - (int) Interpreter.fas.RuntimeCodeStorageOffset).ToString("x04") + Environment.NewLine + Interpreter.RunLog.ToString() + Environment.NewLine, ex);
        }
      }
      if (Interpreter.IsError)
      {
        Interpreter.RunLog.AppendLine();
        Interpreter.RunLog.AppendLine();
        Interpreter.RunLog.AppendLine("***** Runtime ERROR *****");
        Interpreter.RunLog.AppendLine("Error: .............. " + Interpreter.Error.ToString());
        Interpreter.RunLog.AppendLine("FunctionErrorOffset = 0x" + Interpreter.fas.ErrorOffset.ToString("x04"));
      }
      Interpreter.RunCondition = Interpreter.RunConditions.NotDefined;
      Interpreter.RunLog.AppendLine();
    }

    private static bool WorkOpcode()
    {
      ++Interpreter.OpcodeCounter;
      if (Interpreter.OpcodeCounter > (byte) 250)
      {
        Interpreter.Error = SmartFunctionResult.RuntimeLoop;
        return false;
      }
      Interpreter.RunLog.AppendLine();
      Interpreter.LogOffset(">");
      byte num1 = FunctionLoader.FlashStorage[(int) Interpreter.InstructionPointer];
      Interpreter.LogCode();
      ++Interpreter.InstructionPointer;
      if (Enum.IsDefined(typeof (OpcodeNoParameter), (object) num1))
      {
        switch (num1)
        {
          case 1:
            return false;
          case 2:
            Register register = Interpreter.RegB.Clone();
            Interpreter.RegB.RegisterValue = Interpreter.RegA.RegisterValue;
            Interpreter.RegB.RegisterTypeCode = Interpreter.RegA.RegisterTypeCode;
            Interpreter.RegA.RegisterValue = register.RegisterValue;
            Interpreter.RegA.RegisterTypeCode = register.RegisterTypeCode;
            Interpreter.LogRegA();
            break;
          case 3:
            Interpreter.RegB.RegisterValue = Interpreter.RegA.RegisterValue;
            Interpreter.RegB.RegisterTypeCode = Interpreter.RegA.RegisterTypeCode;
            Interpreter.LogRegA();
            break;
          case 4:
            Interpreter.RegA.AccuToInt32();
            Interpreter.LogRegA();
            break;
          case 5:
            Interpreter.RegA.AccuToFloat();
            Interpreter.LogRegA();
            break;
          case 6:
            Interpreter.RegA.AccuToDouble();
            Interpreter.LogRegA();
            break;
          case 7:
            Interpreter.RunLog.Append("; *** Store logger ***");
            Interpreter.StoreLogger();
            break;
          case 8:
            Interpreter.CompareResult = Interpreter.RegA.IsNaN();
            Interpreter.LogCompareResult();
            break;
          case 9:
            Interpreter.CompareResult = Interpreter.RunCondition == Interpreter.RunConditions.FunctionLoad;
            Interpreter.LogCompareResult();
            break;
          default:
            Interpreter.Error = SmartFunctionResult.NotSupportedOpcode;
            break;
        }
      }
      else
      {
        byte num2 = (byte) ((uint) num1 - 32U);
        if (Enum.IsDefined(typeof (OpcodeJump_0), (object) num2))
        {
          switch (num2)
          {
            case 0:
              ushort uint16 = BitConverter.ToUInt16(FunctionLoader.FlashStorage, (int) Interpreter.InstructionPointer);
              Interpreter.InstructionPointer = (ushort) ((uint) Interpreter.fas.RuntimeCodeStorageOffset + (uint) uint16);
              Interpreter.RunLog.Append(" -> Jump to function offset: 0x" + uint16.ToString("x04"));
              break;
            case 1:
              Interpreter.WorkJump(Interpreter.CompareResult);
              break;
            case 2:
              Interpreter.WorkJump(!Interpreter.CompareResult);
              break;
            default:
              Interpreter.Error = SmartFunctionResult.NotSupportedOpcode;
              break;
          }
        }
        else
        {
          byte num3 = (byte) ((uint) num1 - 35U);
          if (Enum.IsDefined(typeof (OpcodeLoadParameter_0), (object) num3))
          {
            Interpreter.WorkLoad();
            switch (num3)
            {
              case 0:
                Interpreter.LogRegA();
                break;
              case 1:
                Interpreter.CompareResult = Interpreter.RegT.CompareEQ(Interpreter.RegA);
                Interpreter.LogCompareResult();
                Interpreter.RegA = Interpreter.RegT;
                break;
              case 2:
                Interpreter.CompareResult = !Interpreter.RegT.CompareEQ(Interpreter.RegA);
                Interpreter.LogCompareResult();
                Interpreter.RegA = Interpreter.RegT;
                break;
              case 3:
                Interpreter.CompareResult = Interpreter.RegT.CompareGE(Interpreter.RegA);
                Interpreter.LogCompareResult();
                Interpreter.RegA = Interpreter.RegT;
                break;
              case 4:
                Interpreter.CompareResult = Interpreter.RegT.CompareGT(Interpreter.RegA);
                Interpreter.LogCompareResult();
                Interpreter.RegA = Interpreter.RegT;
                break;
              case 5:
                Interpreter.CompareResult = Interpreter.RegT.CompareLE(Interpreter.RegA);
                Interpreter.LogCompareResult();
                Interpreter.RegA = Interpreter.RegT;
                break;
              case 6:
                Interpreter.CompareResult = Interpreter.RegT.CompareLT(Interpreter.RegA);
                Interpreter.LogCompareResult();
                Interpreter.RegA = Interpreter.RegT;
                break;
              case 7:
                Interpreter.RegT.Add(Interpreter.RegA);
                Interpreter.RegA = Interpreter.RegT;
                Interpreter.LogRegA();
                break;
              case 8:
                Interpreter.RegT.Sub(Interpreter.RegA);
                Interpreter.RegA = Interpreter.RegT;
                Interpreter.LogRegA();
                break;
              case 9:
                Interpreter.RegT.Mul(Interpreter.RegA);
                Interpreter.RegA = Interpreter.RegT;
                Interpreter.LogRegA();
                break;
              case 10:
                Interpreter.RegT.Div(Interpreter.RegA);
                Interpreter.RegA = Interpreter.RegT;
                Interpreter.LogRegA();
                break;
              case 11:
                Interpreter.RegT.Mod(Interpreter.RegA);
                Interpreter.RegA = Interpreter.RegT;
                Interpreter.LogRegA();
                break;
              default:
                Interpreter.Error = SmartFunctionResult.NotSupportedOpcode;
                break;
            }
          }
          else
          {
            byte num4 = (byte) ((uint) num1 - 64U);
            if (Enum.IsDefined(typeof (OpcodeSpacialParameter_0), (object) num4))
            {
              switch (num4)
              {
                case 0:
                  byte parameterNumber = FunctionLoader.FlashStorage[(int) Interpreter.InstructionPointer++];
                  Interpreter.RunLog.Append("; Store parameter: '" + Interpreter.fas.GetParameterObject(parameterNumber).Name + "' Value=" + Interpreter.RegA.GetValueAndRegisterType());
                  Interpreter.fas.StoreRegisterByParameterNumber(parameterNumber);
                  break;
                case 1:
                case 4:
                  Interpreter.WorkLoad();
                  if (Interpreter.RegA.RegisterTypeCode != DataTypeCodes.Int32)
                  {
                    Interpreter.Error = SmartFunctionResult.IllegalRegisterTypeForSaveEvent;
                    return false;
                  }
                  byte registerValue1 = (byte) (int) Interpreter.RegA.RegisterValue;
                  if (!Enum.IsDefined(typeof (SmartFunctionLoggerEventType), (object) registerValue1))
                  {
                    Interpreter.RunLog.Append("; *** Save to EventLogger: " + registerValue1.ToString());
                  }
                  else
                  {
                    SmartFunctionLoggerEventType registerValue2 = (SmartFunctionLoggerEventType) (int) Interpreter.RegA.RegisterValue;
                    Interpreter.RunLog.Append("; *** Save to EventLogger: " + registerValue2.ToString());
                  }
                  if (registerValue1 > (byte) 0)
                  {
                    SimulatorParameter simulatorParameter1 = Interpreter.SimulatorParameters.FirstOrDefault<SimulatorParameter>((Func<SimulatorParameter, bool>) (x => x.Name == "SmartFunctionState"));
                    if (simulatorParameter1 == null)
                      throw new Exception("Simulation parameter not found: SmartFunctionState");
                    SimulatorParameter simulatorParameter2 = Interpreter.SimulatorParameters.FirstOrDefault<SimulatorParameter>((Func<SimulatorParameter, bool>) (x => x.Name == "SmartFunctionTimoutActive"));
                    if (simulatorParameter2 == null)
                      throw new Exception("Simulation parameter not found: SmartFunctionTimoutActive");
                    if (registerValue1 < (byte) 128)
                    {
                      simulatorParameter1.Value = 1.0;
                      simulatorParameter2.Value = 1.0;
                    }
                    else
                    {
                      simulatorParameter1.Value = 0.0;
                      simulatorParameter2.Value = 0.0;
                    }
                  }
                  Interpreter.LogRegA();
                  if (num4 == (byte) 4)
                  {
                    Interpreter.RunLog.Append("; SendParam = RegB = " + Interpreter.RegB.GetValueAndRegisterType());
                    break;
                  }
                  break;
                case 2:
                  Interpreter.WorkLoad();
                  if (Interpreter.RegA.RegisterTypeCode != DataTypeCodes.Int32)
                  {
                    Interpreter.Error = SmartFunctionResult.IllegalRegisterTypeForLoRaSendAlarm;
                    return false;
                  }
                  uint registerValue3 = (uint) (int) Interpreter.RegA.RegisterValue;
                  if (!Enum.IsDefined(typeof (LoRaAlarm), (object) registerValue3))
                  {
                    Interpreter.RunLog.Append("; *** Send LoRa alarm: " + registerValue3.ToString());
                  }
                  else
                  {
                    LoRaAlarm registerValue4 = (LoRaAlarm) (int) Interpreter.RegA.RegisterValue;
                    Interpreter.RunLog.Append("; *** Send LoRa alarm: " + registerValue4.ToString());
                  }
                  Interpreter.LogRegA();
                  break;
                case 3:
                  Interpreter.Error = (SmartFunctionResult) ByteArrayScanner16Bit.ScanUInt16(FunctionLoader.FlashStorage, ref Interpreter.InstructionPointer);
                  return false;
                default:
                  Interpreter.Error = SmartFunctionResult.NotSupportedOpcode;
                  break;
              }
            }
            else
              Interpreter.Error = SmartFunctionResult.NotSupportedOpcode;
          }
        }
      }
      return true;
    }

    internal static void WorkLoad()
    {
      Interpreter.RegT = Interpreter.RegA.Clone();
      byte num = FunctionLoader.FlashStorage[(int) Interpreter.InstructionPointer++];
      if (num < (byte) 32)
      {
        Interpreter.RunLog.Append(" '" + Interpreter.fas.GetParameterObject(num).Name + "'");
        Interpreter.fas.LoadRegisterByParameterNumber(Interpreter.RegA, num);
        Interpreter.LogRegA();
      }
      else if (num < (byte) 64)
      {
        byte parameterNumber = (byte) ((uint) num - 32U);
        Interpreter.RunLog.Append(" '" + Interpreter.fas.GetParameterObject(parameterNumber).Name + "!'");
        Interpreter.fas.LoadRegisterByParameterNumberInit(Interpreter.RegA, parameterNumber);
        Interpreter.LogRegA();
      }
      else if (num < (byte) 96)
      {
        Interpreter.RunLog.Append(" Const: ");
        Interpreter.LogRegA();
        Interpreter.RegA.LoadValue((DataTypeCodes) num, FunctionLoader.FlashStorage, (int) Interpreter.InstructionPointer);
        Interpreter.InstructionPointer += RuntimeParameter.GetByteSizeFromType((DataTypeCodes) num);
      }
      else
        Interpreter.LoadFirmwareParameter((OpcodeLoadFirmwareParameter_0) ((uint) num - 96U));
    }

    internal static void LoadFirmwareParameter(OpcodeLoadFirmwareParameter_0 opcode_0)
    {
      OpcodeLoadFirmwareParameter_0 firmwareParameter0 = opcode_0;
      if ((uint) firmwareParameter0 <= 25U)
      {
        if ((uint) firmwareParameter0 <= 17U)
        {
          switch (firmwareParameter0)
          {
            case OpcodeLoadFirmwareParameter_0.LoadA:
              Interpreter.RunLog.Append("; LoadA");
              Interpreter.LogRegA();
              return;
            case OpcodeLoadFirmwareParameter_0.LoadB:
              Interpreter.RunLog.Append("; LoadB");
              Interpreter.RegA.RegisterValue = Interpreter.RegB.RegisterValue;
              Interpreter.RegA.RegisterTypeCode = Interpreter.RegB.RegisterTypeCode;
              Interpreter.LogRegA();
              return;
            case OpcodeLoadFirmwareParameter_0.LoadVolume:
              Interpreter.RunLog.Append("; LoadVolume");
              Interpreter.RegA.RegisterValue = (object) Interpreter.Volume;
              Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Double;
              Interpreter.LogRegA();
              return;
            case OpcodeLoadFirmwareParameter_0.LoadFlow:
              Interpreter.RunLog.Append("; LoadFlow");
              Interpreter.RegA.RegisterValue = (object) Interpreter.Flow;
              Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Float;
              Interpreter.LogRegA();
              return;
            case OpcodeLoadFirmwareParameter_0.LoadVolumeCycleSeconds:
              Interpreter.RunLog.Append("; LoadVolumeCycleSeconds");
              Interpreter.RegA.RegisterValue = (object) 2;
              Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Int32;
              Interpreter.LogRegA();
              return;
            case OpcodeLoadFirmwareParameter_0.LoadDay:
              Interpreter.RunLog.Append("; LoadDay");
              Interpreter.RegA.RegisterValue = (object) (int) CalendarBase2000.Cal_Sec2000ToDate(Interpreter.DeviceTime).Day;
              Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Int32;
              return;
            case OpcodeLoadFirmwareParameter_0.LoadMonth:
              Interpreter.RunLog.Append("; LoadMonth");
              Interpreter.RegA.RegisterValue = (object) (int) CalendarBase2000.Cal_Sec2000ToDate(Interpreter.DeviceTime).Month;
              Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Int32;
              return;
            case OpcodeLoadFirmwareParameter_0.LoadYear:
              Interpreter.RunLog.Append("; LoadYear");
              Interpreter.RegA.RegisterValue = (object) (int) CalendarBase2000.Cal_Sec2000ToDate(Interpreter.DeviceTime).Year;
              Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Int32;
              return;
          }
        }
        else if (firmwareParameter0 != OpcodeLoadFirmwareParameter_0.LoadFlowVolume)
        {
          if (firmwareParameter0 == OpcodeLoadFirmwareParameter_0.LoadReturnVolume)
          {
            Interpreter.RunLog.Append("; LoadReturnVolume");
            Interpreter.RegA.RegisterValue = (object) Interpreter.ReturnVolume;
            Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Double;
            Interpreter.LogRegA();
            return;
          }
        }
        else
        {
          Interpreter.RunLog.Append("; LoadFlowVolume");
          Interpreter.RegA.RegisterValue = (object) Interpreter.FlowVolume;
          Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Double;
          Interpreter.LogRegA();
          return;
        }
      }
      else if ((uint) firmwareParameter0 <= 31U)
      {
        switch (firmwareParameter0)
        {
          case OpcodeLoadFirmwareParameter_0.LoadCycleSeconds:
            Interpreter.RunLog.Append("; LoadCycleSeconds");
            string str1 = Interpreter.fas.FunctionEvent.ToString();
            if (!Enum.IsDefined(typeof (CycleSeconds), (object) str1))
            {
              Interpreter.Error = SmartFunctionResult.NotSupportedOpcode;
              return;
            }
            CycleSeconds cycleSeconds = (CycleSeconds) Enum.Parse(typeof (CycleSeconds), str1);
            Interpreter.RegA.RegisterValue = (object) (int) cycleSeconds;
            Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Int32;
            return;
          case OpcodeLoadFirmwareParameter_0.LoadStateCounter:
            DeviceStateCounterID stateCounterID = (DeviceStateCounterID) ByteArrayScanner16Bit.ScanByte(FunctionLoader.FlashStorage, ref Interpreter.InstructionPointer);
            if (!Enum.IsDefined(typeof (DeviceStateCounterID), (object) stateCounterID))
            {
              Interpreter.Error = SmartFunctionResult.IllegalStateCounter;
              return;
            }
            Interpreter.RegA.RegisterValue = (object) (int) (Interpreter.SimulatorParameters.FirstOrDefault<SimulatorParameter>((Func<SimulatorParameter, bool>) (x => x.Name == stateCounterID.ToString())) ?? throw new Exception("Simulation parameter not found: " + stateCounterID.ToString())).Value;
            Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Int32;
            Interpreter.LogRegA();
            return;
        }
      }
      else
      {
        switch (firmwareParameter0)
        {
          case OpcodeLoadFirmwareParameter_0.LoadSmartFunctionState:
            ++Interpreter.InstructionPointer;
            SimulatorParameter simulatorParameter1 = Interpreter.SimulatorParameters.FirstOrDefault<SimulatorParameter>((Func<SimulatorParameter, bool>) (x => x.Name == "SmartFunctionState"));
            if (simulatorParameter1 == null)
              throw new Exception("Simulation parameter not found: SmartFunctionState");
            Interpreter.RunLog.Append(" SmartFunctionState");
            Interpreter.CompareResult = simulatorParameter1.Value != 0.0;
            Interpreter.LogCompareResult();
            return;
          case OpcodeLoadFirmwareParameter_0.LoadSmartFunctionTimeoutActive:
            ++Interpreter.InstructionPointer;
            SimulatorParameter simulatorParameter2 = Interpreter.SimulatorParameters.FirstOrDefault<SimulatorParameter>((Func<SimulatorParameter, bool>) (x => x.Name == "SmartFunctionTimoutActive"));
            if (simulatorParameter2 == null)
              throw new Exception("Simulation parameter not found: SmartFunctionTimoutActive");
            Interpreter.RunLog.Append(" SmartFunctionTimoutActive");
            Interpreter.CompareResult = simulatorParameter2.Value != 0.0;
            Interpreter.LogCompareResult();
            return;
        }
      }
      string str2 = opcode_0.ToString();
      if (str2.StartsWith("Load"))
      {
        string parameterName = str2.Substring(4);
        SimulatorParameter simulatorParameter3 = Interpreter.SimulatorParameters.FirstOrDefault<SimulatorParameter>((Func<SimulatorParameter, bool>) (x => x.Name == parameterName));
        if (simulatorParameter3 != null)
        {
          Interpreter.RunLog.Append("; " + parameterName);
          if (parameterName.Contains("Flow") || parameterName.Contains("Temp"))
          {
            Interpreter.RegA.RegisterValue = (object) (float) simulatorParameter3.Value;
            Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Float;
          }
          else
          {
            Interpreter.RegA.RegisterValue = (object) simulatorParameter3.Value;
            Interpreter.RegA.RegisterTypeCode = DataTypeCodes.Double;
          }
          Interpreter.LogRegA();
          return;
        }
      }
      Interpreter.Error = SmartFunctionResult.NotSupportedOpcode;
    }

    internal static void WorkJump(bool condition)
    {
      if (condition)
      {
        ushort uint16 = BitConverter.ToUInt16(FunctionLoader.FlashStorage, (int) Interpreter.InstructionPointer);
        Interpreter.InstructionPointer = (ushort) ((uint) Interpreter.fas.RuntimeCodeStorageOffset + (uint) uint16);
        Interpreter.RunLog.Append(" -> Jump to function offset: 0x" + uint16.ToString("x04"));
      }
      else
      {
        Interpreter.RunLog.Append("; No Jump");
        Interpreter.InstructionPointer += (ushort) 2;
      }
    }

    internal static void LoadValue(byte parameterNumber)
    {
      ushort parameterFunctionOffset = Interpreter.fas.GetRuntimeParameterFunctionOffset(parameterNumber);
      ushort parameterDataOffset = Interpreter.fas.GetRuntimeParameterDataOffset(parameterNumber);
      StorageTypeCodes storageCode = (StorageTypeCodes) FunctionLoader.FlashStorage[(int) parameterFunctionOffset];
      DataTypeCodes typeCode = (DataTypeCodes) FunctionLoader.FlashStorage[(int) parameterFunctionOffset + 1];
      Interpreter.RegA.LoadValue(typeCode, Interpreter.StorageFromStorageCode(storageCode), (int) parameterDataOffset);
    }

    private static byte[] StorageFromStorageCode(StorageTypeCodes storageCode)
    {
      switch (storageCode)
      {
        case StorageTypeCodes.ram:
          return FunctionLoader.RamStorage;
        case StorageTypeCodes.flash:
          return FunctionLoader.FlashStorage;
        case StorageTypeCodes.backup:
          return FunctionLoader.BackupStorage;
        default:
          Interpreter.Error = SmartFunctionResult.IllegalStorageTypeCode;
          return (byte[]) null;
      }
    }

    private static void StoreLogger()
    {
      ushort writeOffset = 0;
      ushort num = writeOffset;
      LoggerHeader loggerHeader = (LoggerHeader) null;
      for (byte parameterNumber = 0; (int) parameterNumber < (int) Interpreter.fas.NumberOfRuntimeParameters; ++parameterNumber)
      {
        RuntimeParameter parameterObject = Interpreter.fas.GetParameterObject(parameterNumber);
        string name = parameterObject.Name;
        if (name == "Logger")
        {
          switch (parameterObject.StorageCode)
          {
            case StorageTypeCodes.ram:
              loggerHeader = new LoggerHeader(FunctionLoader.RamStorage, Interpreter.fas.GetRuntimeParameterDataOffset(parameterNumber), parameterObject.InitValue_UInt16);
              break;
            case StorageTypeCodes.flash:
              loggerHeader = new LoggerHeader(FunctionLoader.FlashStorage, Interpreter.fas.GetRuntimeParameterDataOffset(parameterNumber), parameterObject.InitValue_UInt16);
              break;
            default:
              Interpreter.Error = SmartFunctionResult.StorageNotAllowedForLogger;
              return;
          }
          writeOffset = loggerHeader.LoggerWriteOffset;
          loggerHeader.MoveAndSaveWriteOffset();
          byte[] bytes = BitConverter.GetBytes(Interpreter.DeviceTime);
          loggerHeader.SaveBytes(bytes, ref writeOffset);
          num = writeOffset;
        }
        else if (name.StartsWith("L_"))
        {
          if (writeOffset == (ushort) 0)
          {
            Interpreter.Error = SmartFunctionResult.LoggerParameterNotInitialised;
            return;
          }
          byte[] parameterValueBytes = Interpreter.fas.GetParameterValueBytes(parameterNumber);
          loggerHeader.SaveBytes(parameterValueBytes, ref writeOffset);
        }
      }
      if (loggerHeader == null || (int) num != (int) writeOffset)
        return;
      Interpreter.Error = SmartFunctionResult.LoggerWithoutData;
    }

    private static void LogOffset(string info = null)
    {
      int num = (int) Interpreter.InstructionPointer - (int) Interpreter.fas.RuntimeCodeStorageOffset;
      foreach (LineCode code in Compiler.TheOnlyOneCompiler.Codes)
      {
        if ((int) code.CodeStartOffset == num)
        {
          Interpreter.RunLog.Append(code.SourceLine.ToString("d03") + ":");
          Interpreter.RunLog.Append(code.CodeStartOffset.ToString("x03") + ":");
          break;
        }
      }
      Interpreter.RunLog.Append(Interpreter.InstructionPointer.ToString("x04") + ": ");
      if (info == null)
        return;
      Interpreter.RunLog.Append(info);
    }

    private static void LogCode()
    {
      byte theCode = FunctionLoader.FlashStorage[(int) Interpreter.InstructionPointer];
      Interpreter.RunLog.Append(Interpreter.GetOpcodeString(theCode));
    }

    public static string GetOpcodeString(byte theCode)
    {
      string opcodeString = "No opcode found";
      if (Enum.IsDefined(typeof (OpcodeNoParameter), (object) theCode))
        opcodeString = ((OpcodeNoParameter) theCode).ToString();
      else if (Enum.IsDefined(typeof (OpcodeJump), (object) theCode))
        opcodeString = ((OpcodeJump) theCode).ToString();
      else if (Enum.IsDefined(typeof (OpcodeLoadParameter), (object) theCode))
        opcodeString = ((OpcodeLoadParameter) theCode).ToString();
      else if (Enum.IsDefined(typeof (OpcodeSpacialParameter), (object) theCode))
        opcodeString = ((OpcodeSpacialParameter) theCode).ToString();
      else if (Enum.IsDefined(typeof (OpcodeLoadFirmwareParameter), (object) theCode))
        opcodeString = ((OpcodeLoadFirmwareParameter) theCode).ToString();
      return opcodeString;
    }

    private static void LogCompareResult()
    {
      Interpreter.RunLog.Append("; CompareResult:" + Interpreter.CompareResult.ToString());
    }

    private static void LogRegA()
    {
      Interpreter.RunLog.Append("; RegA = " + Interpreter.RegA.GetValueAndRegisterType());
    }

    internal enum RunConditions
    {
      NotDefined,
      Event,
      HardwareReset,
      FunctionLoad,
    }
  }
}
