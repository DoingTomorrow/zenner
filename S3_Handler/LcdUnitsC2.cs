// Decompiled with JetBrains decompiler
// Type: S3_Handler.LcdUnitsC2
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public static class LcdUnitsC2
  {
    private static Logger S3_LcdUnitsC2Logger = LogManager.GetLogger(nameof (S3_LcdUnitsC2Logger));
    private const byte DII_FRAME_SEGS1 = 1;
    private const byte DII_FRAME_SEGS2 = 2;
    private const byte DII_FRAME_SEGS3 = 4;
    private const byte DII_FRAME_SEGS4 = 8;
    private const byte DII_FRAME_MENUPOS = 16;
    private const byte DII_FRAME_BLINK = 32;
    private const byte DII_FRAME_UNIT = 64;
    private const byte DII_FRAME_TEXT = 128;
    private const byte SEGS1_DEC1 = 1;
    private const byte SEGS1_DEC2 = 2;
    private const byte SEGS1_DEC3 = 4;
    private const byte SEGS1_FRAME1 = 8;
    private const byte SEGS1_FRAME2 = 16;
    private const byte SEGS1_FRAME3 = 32;
    private const byte SEGS2_CUBIC_M = 1;
    private const byte SEGS2_LITRE = 2;
    private const byte SEGS2_SLASH = 4;
    private const byte SEGS2_HOUR = 8;
    private const byte SEGS2_KILO = 16;
    private const byte SEGS2_WATT = 32;
    private const byte SEGS2_MEGA = 64;
    private const byte SEGS2_GIGA_JOULE = 128;
    private const byte SEGS3_DEC4 = 1;
    private const byte SEGS3_DEC7 = 2;
    private const byte SEGS3_INPUT_1 = 16;
    private const byte SEGS3_INPUT_2 = 32;
    private static byte[] allUnitSegs;
    private static byte[] allUnitSegsWithoutAfterDigitFrames;
    internal static SortedList<uint, string> resolutionStringFromFrameBits;
    internal static SortedList<uint, LcdUnitsC2FrameType> frameTypeFromFrameBits;
    internal static SortedList<string, byte> resolutionIdFromResolutionString;
    internal static SortedList<byte, string> resolutionStringFromResolutionId;
    public static LcdUnitsC2.LcdUnitData[] AllLcdUnits = new LcdUnitsC2.LcdUnitData[59]
    {
      new LcdUnitsC2.LcdUnitData((byte) 0, "0.0000Wh", LcdUnitsC2FrameType.EnergyFrame, (byte) 0, (byte) 40, (byte) 1),
      new LcdUnitsC2.LcdUnitData((byte) 1, "0.000Wh", LcdUnitsC2FrameType.EnergyFrame, (byte) 4, (byte) 40, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 2, "0.00Wh", LcdUnitsC2FrameType.EnergyFrame, (byte) 2, (byte) 40, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 3, "0.0Wh", LcdUnitsC2FrameType.EnergyFrame, (byte) 1, (byte) 40, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 4, "0Wh", LcdUnitsC2FrameType.EnergyFrame, (byte) 0, (byte) 40, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 5, "0.000kWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 4, (byte) 56, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 6, "0.00kWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 2, (byte) 56, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 7, "0.0kWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 1, (byte) 56, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 8, "0kWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 0, (byte) 56, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 9, "0.000MWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 4, (byte) 104, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 10, "0.00MWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 2, (byte) 104, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 11, "0.0MWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 1, (byte) 104, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 12, "0MWh", LcdUnitsC2FrameType.EnergyFrame, (byte) 0, (byte) 104, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 13, "0.000GJ", LcdUnitsC2FrameType.EnergyFrame, (byte) 4, (byte) 128, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 14, "0.00GJ", LcdUnitsC2FrameType.EnergyFrame, (byte) 2, (byte) 128, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 15, "0.0GJ", LcdUnitsC2FrameType.EnergyFrame, (byte) 1, (byte) 128, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 16, "0GJ", LcdUnitsC2FrameType.EnergyFrame, (byte) 0, (byte) 128, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 17, "0.0000W", LcdUnitsC2FrameType.PowerFrame, (byte) 0, (byte) 32, (byte) 1),
      new LcdUnitsC2.LcdUnitData((byte) 18, "0.000W", LcdUnitsC2FrameType.PowerFrame, (byte) 4, (byte) 32, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 19, "0.00W", LcdUnitsC2FrameType.PowerFrame, (byte) 2, (byte) 32, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 20, "0.0W", LcdUnitsC2FrameType.PowerFrame, (byte) 1, (byte) 32, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 21, "0W", LcdUnitsC2FrameType.PowerFrame, (byte) 0, (byte) 32, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 22, "0.000kW", LcdUnitsC2FrameType.PowerFrame, (byte) 4, (byte) 48, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 23, "0.00kW", LcdUnitsC2FrameType.PowerFrame, (byte) 2, (byte) 48, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 24, "0.0kW", LcdUnitsC2FrameType.PowerFrame, (byte) 1, (byte) 48, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 25, "0kW", LcdUnitsC2FrameType.PowerFrame, (byte) 0, (byte) 48, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 26, "0.000MW", LcdUnitsC2FrameType.PowerFrame, (byte) 4, (byte) 96, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 27, "0.00MW", LcdUnitsC2FrameType.PowerFrame, (byte) 2, (byte) 96, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 28, "0.0MW", LcdUnitsC2FrameType.PowerFrame, (byte) 1, (byte) 96, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 29, "0MW", LcdUnitsC2FrameType.PowerFrame, (byte) 0, (byte) 96, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 30, "0.000GJ/h", LcdUnitsC2FrameType.PowerFrame, (byte) 4, (byte) 140, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 31, "0.00GJ/h", LcdUnitsC2FrameType.PowerFrame, (byte) 2, (byte) 140, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 32, "0.0GJ/h", LcdUnitsC2FrameType.PowerFrame, (byte) 1, (byte) 140, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 33, "0GJ/h", LcdUnitsC2FrameType.PowerFrame, (byte) 0, (byte) 140, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 34, "0.000L", LcdUnitsC2FrameType.VolumeFrame, (byte) 4, (byte) 2, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 35, "0.00L", LcdUnitsC2FrameType.VolumeFrame, (byte) 2, (byte) 2, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 36, "0.0L", LcdUnitsC2FrameType.VolumeFrame, (byte) 1, (byte) 2, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 37, "0L", LcdUnitsC2FrameType.VolumeFrame, (byte) 0, (byte) 2, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 38, "0.000m\u00B3", LcdUnitsC2FrameType.VolumeFrame, (byte) 4, (byte) 1, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 39, "0.00m\u00B3", LcdUnitsC2FrameType.VolumeFrame, (byte) 2, (byte) 1, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 40, "0.0m\u00B3", LcdUnitsC2FrameType.VolumeFrame, (byte) 1, (byte) 1, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 41, "0m\u00B3", LcdUnitsC2FrameType.VolumeFrame, (byte) 0, (byte) 1, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 42, "0.000L/h", LcdUnitsC2FrameType.FlowFrame, (byte) 4, (byte) 14, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 43, "0.00L/h", LcdUnitsC2FrameType.FlowFrame, (byte) 2, (byte) 14, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 44, "0.0L/h", LcdUnitsC2FrameType.FlowFrame, (byte) 1, (byte) 14, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 45, "0L/h", LcdUnitsC2FrameType.FlowFrame, (byte) 0, (byte) 14, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 46, "0.000m\u00B3/h", LcdUnitsC2FrameType.FlowFrame, (byte) 4, (byte) 13, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 47, "0.00m\u00B3/h", LcdUnitsC2FrameType.FlowFrame, (byte) 2, (byte) 13, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 48, "0.0m\u00B3/h", LcdUnitsC2FrameType.FlowFrame, (byte) 1, (byte) 13, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 49, "0m\u00B3/h", LcdUnitsC2FrameType.FlowFrame, (byte) 0, (byte) 13, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 50, "0.000", LcdUnitsC2FrameType.NoUnitFrame, (byte) 4, (byte) 0, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 51, "0.00", LcdUnitsC2FrameType.NoUnitFrame, (byte) 2, (byte) 0, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 52, "0.0", LcdUnitsC2FrameType.NoUnitFrame, (byte) 1, (byte) 0, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 53, "0", LcdUnitsC2FrameType.NoUnitFrame, (byte) 0, (byte) 0, (byte) 0),
      new LcdUnitsC2.LcdUnitData((byte) 54, "0.0000GJ", LcdUnitsC2FrameType.EnergyFrame, (byte) 0, (byte) 128, (byte) 1),
      new LcdUnitsC2.LcdUnitData((byte) 55, "0.0000000GJ", LcdUnitsC2FrameType.EnergyFrame, (byte) 0, (byte) 128, (byte) 2),
      new LcdUnitsC2.LcdUnitData((byte) 56, "0.0000GJ/h", LcdUnitsC2FrameType.PowerFrame, (byte) 0, (byte) 140, (byte) 1),
      new LcdUnitsC2.LcdUnitData((byte) 57, "0.0000000GJ/h", LcdUnitsC2FrameType.PowerFrame, (byte) 0, (byte) 140, (byte) 2),
      new LcdUnitsC2.LcdUnitData((byte) 58, "0.0000L", LcdUnitsC2FrameType.VolumeFrame, (byte) 0, (byte) 2, (byte) 1)
    };
    public static string[] AllDisplayUnits = new string[42]
    {
      "0.0000Wh",
      "0.000Wh",
      "0.00Wh",
      "0.0Wh",
      "0Wh",
      "0.000kWh",
      "0.00kWh",
      "0.0kWh",
      "0kWh",
      "0.000MWh",
      "0.00MWh",
      "0.0MWh",
      "0MWh",
      "0.000GJ",
      "0.00GJ",
      "0.0GJ",
      "0GJ",
      "0.0000W",
      "0.000W",
      "0.00W",
      "0.0W",
      "0W",
      "0.000kW",
      "0.00kW",
      "0.0kW",
      "0kW",
      "0.000MW",
      "0.00MW",
      "0.0MW",
      "0MW",
      "0.000GJ/h",
      "0.00GJ/h",
      "0.0GJ/h",
      "0GJ/h",
      "0.000L",
      "0.00L",
      "0.0L",
      "0L",
      "0.000m\u00B3",
      "0.00m\u00B3",
      "0.0m\u00B3",
      "0m\u00B3"
    };
    public static string[] AllInputUnits = new string[24]
    {
      "0.000m\u00B3",
      "0.00m\u00B3",
      "0.0m\u00B3",
      "0m\u00B3",
      "0.000MWh",
      "0.00MWh",
      "0.000kWh",
      "0.00kWh",
      "0.0kWh",
      "0kWh",
      "0.000Wh",
      "0.00Wh",
      "0.0Wh",
      "0Wh",
      "0.000GJ",
      "0.00GJ",
      "0.000L",
      "0.00L",
      "0.0L",
      "0L",
      "0.000",
      "0.00",
      "0.0",
      "0"
    };

    static LcdUnitsC2()
    {
      LcdUnitsC2.allUnitSegs = new byte[4]
      {
        (byte) 63,
        byte.MaxValue,
        (byte) 1,
        (byte) 2
      };
      LcdUnitsC2.allUnitSegsWithoutAfterDigitFrames = new byte[4]
      {
        (byte) 7,
        byte.MaxValue,
        (byte) 1,
        (byte) 2
      };
      LcdUnitsC2.resolutionStringFromFrameBits = new SortedList<uint, string>();
      LcdUnitsC2.frameTypeFromFrameBits = new SortedList<uint, LcdUnitsC2FrameType>();
      LcdUnitsC2.resolutionIdFromResolutionString = new SortedList<string, byte>();
      LcdUnitsC2.resolutionStringFromResolutionId = new SortedList<byte, string>();
      for (int index = 0; index < LcdUnitsC2.AllLcdUnits.Length; ++index)
      {
        uint key = LcdUnitsC2.UintFromFrameBytes(LcdUnitsC2.AllLcdUnits[index].frameBits);
        string resolutionString = LcdUnitsC2.AllLcdUnits[index].resolutionString;
        LcdUnitsC2.resolutionStringFromFrameBits.Add(key, resolutionString);
        LcdUnitsC2.resolutionIdFromResolutionString.Add(resolutionString, (byte) index);
        LcdUnitsC2.resolutionStringFromResolutionId.Add(LcdUnitsC2.AllLcdUnits[index].resolutionId, resolutionString);
        LcdUnitsC2.frameTypeFromFrameBits.Add(key, LcdUnitsC2.AllLcdUnits[index].frameType);
      }
    }

    private static uint UintFromFrameBytes(byte[] frameBytes)
    {
      return (uint) frameBytes[0] | (uint) frameBytes[1] << 8 | (uint) frameBytes[2] << 16;
    }

    private static bool GetUnitFromInterpreterFrame(
      byte[] interpreterFrame,
      out string unit,
      out bool hasAfterPointFrames)
    {
      unit = "";
      hasAfterPointFrames = false;
      return false;
    }

    internal static bool FrameCodeAdjustLayer(
      ref byte[] frameCodeStorage,
      int frameCodeStartOffset,
      int layer,
      out string layerInfo)
    {
      layerInfo = "no layer info";
      if (((int) frameCodeStorage[frameCodeStartOffset] & 16) == 0)
        return true;
      if (layer == 0)
      {
        layerInfo = "not a menu function";
        return true;
      }
      if (layer > 6)
      {
        layerInfo = "illegal layer info";
        return false;
      }
      int num1 = 1;
      if (((uint) frameCodeStorage[frameCodeStartOffset] & 1U) > 0U)
        ++num1;
      if (((uint) frameCodeStorage[frameCodeStartOffset] & 2U) > 0U)
        ++num1;
      if (((uint) frameCodeStorage[frameCodeStartOffset] & 4U) > 0U)
        ++num1;
      if (((uint) frameCodeStorage[frameCodeStartOffset] & 8U) > 0U)
        ++num1;
      layerInfo = "set layer: " + layer.ToString();
      byte num2 = (byte) ((uint) frameCodeStorage[frameCodeStartOffset + num1] & 15U);
      frameCodeStorage[frameCodeStartOffset + num1] = (byte) ((uint) num2 | (uint) (layer - 1 << 4));
      return true;
    }

    internal static bool GetFrameType(
      ref byte[] frameCodeStorage,
      int frameCodeStartOffset,
      out string oldFrameString,
      out LcdUnitsC2FrameType lcdUnitC2FrameType,
      out int virtualDeviceNumber)
    {
      lcdUnitC2FrameType = LcdUnitsC2FrameType.EnergyFrame;
      virtualDeviceNumber = 0;
      oldFrameString = string.Empty;
      if (frameCodeStorage.Length <= frameCodeStartOffset)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "IsUnitChangeable: no frame code", LcdUnitsC2.S3_LcdUnitsC2Logger);
      byte num1 = (byte) ((uint) frameCodeStorage[frameCodeStartOffset] & 7U);
      bool flag1 = num1 == (byte) 3;
      bool flag2 = num1 == (byte) 7;
      if (!(flag1 | flag2))
        return false;
      try
      {
        byte[] frameBytes = new byte[3];
        uint key;
        if (flag2)
        {
          if (frameCodeStorage.Length < frameCodeStartOffset + 4)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "frameCodeStorage: illegal length", LcdUnitsC2.S3_LcdUnitsC2Logger);
          byte num2 = (byte) ((uint) frameCodeStorage[frameCodeStartOffset + 3] & 48U);
          if (num2 > (byte) 0)
            virtualDeviceNumber = ((uint) num2 & 16U) <= 0U ? 2 : (((uint) num2 & 32U) <= 0U ? 1 : 3);
          for (int index = 0; index < 3; ++index)
            frameBytes[index] = (byte) ((uint) frameCodeStorage[frameCodeStartOffset + index + 1] & (uint) LcdUnitsC2.allUnitSegsWithoutAfterDigitFrames[index]);
          key = LcdUnitsC2.UintFromFrameBytes(frameBytes);
          if (!LcdUnitsC2.resolutionStringFromFrameBits.ContainsKey(key))
          {
            oldFrameString = "Frame not defined";
            return false;
          }
        }
        else
        {
          if (frameCodeStorage.Length < frameCodeStartOffset + 3)
          {
            oldFrameString = "Frame not changable";
            return false;
          }
          for (int index = 0; index < 2; ++index)
            frameBytes[index] = (byte) ((uint) frameCodeStorage[frameCodeStartOffset + index + 1] & (uint) LcdUnitsC2.allUnitSegsWithoutAfterDigitFrames[index]);
          frameBytes[2] = (byte) 0;
          key = LcdUnitsC2.UintFromFrameBytes(frameBytes);
          if (!LcdUnitsC2.resolutionStringFromFrameBits.ContainsKey(key))
          {
            oldFrameString = "Frame not defined";
            return false;
          }
        }
        oldFrameString = LcdUnitsC2.resolutionStringFromFrameBits[key];
        lcdUnitC2FrameType = LcdUnitsC2.frameTypeFromFrameBits[key];
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription("Exception");
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "GetFrameType error", LcdUnitsC2.S3_LcdUnitsC2Logger);
      }
      return true;
    }

    internal static bool SetInterpreterFrameToUnit(
      string unitString,
      ref byte[] frameCodeStorage,
      int frameCodeStartOffset,
      bool useVisibleAfterPointFrame)
    {
      bool flag = (byte) ((uint) frameCodeStorage[frameCodeStartOffset] & 7U) == (byte) 3;
      try
      {
        int num = 3;
        if (flag)
          num = 2;
        for (int index = 0; index < num; ++index)
          frameCodeStorage[frameCodeStartOffset + 1 + index] &= ~LcdUnitsC2.allUnitSegs[index];
        byte[] frameBits = LcdUnitsC2.AllLcdUnits[(int) LcdUnitsC2.resolutionIdFromResolutionString[unitString]].frameBits;
        if (frameBits.Length > num)
        {
          for (int index = num; index < frameBits.Length; ++index)
          {
            if (frameBits[index] > (byte) 0)
              throw new Exception("Frame unit change outside of available frame storage");
          }
        }
        for (int index = 0; index < num; ++index)
          frameCodeStorage[frameCodeStartOffset + 1 + index] |= frameBits[index];
        if (useVisibleAfterPointFrame)
        {
          if (((uint) frameCodeStorage[frameCodeStartOffset + 1] & 1U) > 0U)
            frameCodeStorage[frameCodeStartOffset + 1] |= (byte) 8;
          else if (((uint) frameCodeStorage[frameCodeStartOffset + 1] & 2U) > 0U)
            frameCodeStorage[frameCodeStartOffset + 1] |= (byte) 24;
          else if (((uint) frameCodeStorage[frameCodeStartOffset + 1] & 4U) > 0U)
            frameCodeStorage[frameCodeStartOffset + 1] |= (byte) 56;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription("Exception");
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "SetInterpreterFrameToUnit error", LcdUnitsC2.S3_LcdUnitsC2Logger);
      }
      return true;
    }

    internal static bool SetHightResolutionShift(
      byte newShift,
      ref byte[] frameCodeStorage,
      int frameCodeStartOffset,
      out string shiftInfo)
    {
      shiftInfo = string.Empty;
      byte num1 = frameCodeStorage[frameCodeStartOffset];
      int num2 = 4;
      if (((uint) num1 & 8U) > 0U)
        ++num2;
      if (((uint) num1 & 16U) > 0U)
        ++num2;
      if (((uint) num1 & 224U) > 0U)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "SetHightResolutionShift: Format not supported", LcdUnitsC2.S3_LcdUnitsC2Logger);
      if (frameCodeStorage.Length < frameCodeStartOffset + num2 + 4)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "SetHightResolutionShift: frameCodeStorage to short", LcdUnitsC2.S3_LcdUnitsC2Logger);
      int index = frameCodeStartOffset + num2 + 3;
      byte num3 = frameCodeStorage[index];
      if ((int) num3 == (int) newShift)
      {
        shiftInfo = " shift:" + num3.ToString();
        return true;
      }
      shiftInfo = " shift:" + num3.ToString() + " changed to:" + newShift.ToString();
      frameCodeStorage[index] = newShift;
      return true;
    }

    public static void GetUnitDataFromDisplayString(
      string dispString,
      out string unitString,
      out byte afterPointDigits,
      out double factorFromBaseUnit)
    {
      LcdUnitsC2.GetDisplayStringElements(dispString, out afterPointDigits, out unitString);
      Decimal num = 1M;
      for (byte index = 0; (int) index < (int) afterPointDigits; ++index)
        num /= 10M;
      string str = unitString;
      if (str != null)
      {
        switch (str.Length)
        {
          case 1:
            if (str == "L")
              break;
            break;
          case 2:
            switch (str[0])
            {
              case 'W':
                if (str == "Wh")
                  break;
                break;
              case 'm':
                if (str == "m\u00B3")
                {
                  num *= 1000M;
                  break;
                }
                break;
            }
            break;
          case 3:
            switch (str[0])
            {
              case 'G':
                if (str == "GWh")
                {
                  num *= 1000000000M;
                  break;
                }
                break;
              case 'M':
                if (str == "MWh")
                {
                  num *= 1000000M;
                  break;
                }
                break;
              case 'k':
                if (str == "kWh")
                {
                  num *= 1000M;
                  break;
                }
                break;
            }
            break;
        }
      }
      factorFromBaseUnit = (double) num;
    }

    private static void GetDisplayStringElements(
      string dispString,
      out byte afterPointDigits,
      out string unitString)
    {
      afterPointDigits = (byte) 0;
      unitString = "";
      int num = dispString.IndexOf('.');
      int startIndex;
      if (num < 0)
      {
        startIndex = 1;
      }
      else
      {
        for (int index = num + 1; dispString.Length > index && dispString[index] == '0'; ++index)
          ++afterPointDigits;
        startIndex = num + (int) afterPointDigits + 1;
      }
      if (dispString.Length <= startIndex)
        return;
      unitString = dispString.Substring(startIndex);
    }

    public struct LcdUnitData(
      byte unitId,
      string resolutionString,
      LcdUnitsC2FrameType frameType,
      byte frameBits0,
      byte frameBits1,
      byte frameBits2)
    {
      public byte resolutionId = unitId;
      public string resolutionString = resolutionString;
      public LcdUnitsC2FrameType frameType = frameType;
      public byte[] frameBits = new byte[3]
      {
        frameBits0,
        frameBits1,
        frameBits2
      };
    }
  }
}
