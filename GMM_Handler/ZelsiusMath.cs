// Decompiled with JetBrains decompiler
// Type: GMM_Handler.ZelsiusMath
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class ZelsiusMath : MeterMath
  {
    private ZelsiusInterpreter MyZelsiusInterpreter;
    public SortedList NeadedMeterVars;
    public ZelsiusBaseSettings MyBaseSettings;
    private const double PulsValueMax = 10000.0;
    private const double PulsValueMin = 0.001;
    private static MeterMath.BC_FRAMES[] ZelsiusBCFrames = new MeterMath.BC_FRAMES[17]
    {
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_H DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_H DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_H"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_H DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_d_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_H DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_E DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_n_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_C DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_C DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_H"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_C DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_d_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_C DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_E DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_n_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_0 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_0 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_H"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_0 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_d_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_0 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_E DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_n_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_F DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_F DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_5 DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_H"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_F DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_d_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_r_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_F DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_E DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_n_klein DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_L"
      }),
      new MeterMath.BC_FRAMES(new string[4]
      {
        "DII_FRAME_TEXT_MINUS DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_MINUS DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_MINUS DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_MINUS"
      })
    };
    private static ZelsiusMath.BC_Settings BC_SettingsMask = new ZelsiusMath.BC_Settings((byte) 32, (byte) 151);
    private static ZelsiusMath.BC_Settings[] BC_SettingsList = new ZelsiusMath.BC_Settings[16]
    {
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 3),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 0),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 5),
      new ZelsiusMath.BC_Settings(byte.MaxValue, byte.MaxValue),
      new ZelsiusMath.BC_Settings((byte) 32, (byte) 3),
      new ZelsiusMath.BC_Settings((byte) 32, (byte) 0),
      new ZelsiusMath.BC_Settings((byte) 32, (byte) 5),
      new ZelsiusMath.BC_Settings(byte.MaxValue, byte.MaxValue),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 131),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 128),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 133),
      new ZelsiusMath.BC_Settings(byte.MaxValue, byte.MaxValue),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 19),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 16),
      new ZelsiusMath.BC_Settings((byte) 0, (byte) 21),
      new ZelsiusMath.BC_Settings(byte.MaxValue, byte.MaxValue)
    };
    private static MeterMath.ENERGY_FRAMES[] ZelsiusEnergyFrames = new MeterMath.ENERGY_FRAMES[35]
    {
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[3]
      {
        "",
        "DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR",
        "DII_FRAME_SEGS3_DEC4"
      }, new string[3]
      {
        "",
        "DII_FRAME_SEGS2_WATT",
        "DII_FRAME_SEGS3_DEC4"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "",
        "DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "",
        "DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_HOUR"
      }, new string[2]
      {
        "",
        "DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_WATT"
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[3]
      {
        "",
        "DII_FRAME_SEGS2_MEGA_JOULE",
        "DII_FRAME_SEGS3_DEC7"
      }, new string[1]{ "" }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[3]
      {
        "",
        "DII_FRAME_SEGS2_MEGA_JOULE DII_FRAME_SEGS2_SLASH DII_FRAME_SEGS2_HOUR",
        "DII_FRAME_SEGS3_DEC4"
      }),
      new MeterMath.ENERGY_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_MEGA_JOULE"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_MEGA_JOULE DII_FRAME_SEGS2_SLASH DII_FRAME_SEGS2_HOUR"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_MEGA_JOULE"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_MEGA_JOULE DII_FRAME_SEGS2_SLASH DII_FRAME_SEGS2_HOUR"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_MEGA_JOULE"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_MEGA_JOULE DII_FRAME_SEGS2_SLASH DII_FRAME_SEGS2_HOUR"
      }),
      new MeterMath.ENERGY_FRAMES(new string[2]
      {
        "",
        "DII_FRAME_SEGS2_MEGA_JOULE"
      }, new string[2]
      {
        "",
        "DII_FRAME_SEGS2_MEGA_JOULE DII_FRAME_SEGS2_SLASH DII_FRAME_SEGS2_HOUR"
      })
    };
    private static MeterMath.VOLUME_FRAMES[] ZelsiusVolumeFrames = new MeterMath.VOLUME_FRAMES[11]
    {
      new MeterMath.VOLUME_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.VOLUME_FRAMES(new string[1]{ "" }, new string[1]
      {
        ""
      }),
      new MeterMath.VOLUME_FRAMES(new string[3]
      {
        "",
        "DII_FRAME_SEGS2_LITRE",
        "DII_FRAME_SEGS3_DEC4"
      }, new string[3]
      {
        "",
        "DII_FRAME_SEGS2_LITRE DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH",
        "DII_FRAME_SEGS3_DEC4"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_LITRE"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_LITRE DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_LITRE"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_LITRE DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_LITRE"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_LITRE DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "",
        "DII_FRAME_SEGS2_LITRE"
      }, new string[2]
      {
        "",
        "DII_FRAME_SEGS2_LITRE DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_CUBIC_M"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        "DII_FRAME_SEGS2_CUBIC_M DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_CUBIC_M"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        "DII_FRAME_SEGS2_CUBIC_M DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_CUBIC_M"
      }, new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        "DII_FRAME_SEGS2_CUBIC_M DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      }),
      new MeterMath.VOLUME_FRAMES(new string[2]
      {
        "",
        "DII_FRAME_SEGS2_CUBIC_M"
      }, new string[2]
      {
        "",
        "DII_FRAME_SEGS2_CUBIC_M DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_SLASH"
      })
    };
    private static MeterMath.EMPTY_FRAMES[] ZelsiusEmptyFrames = new MeterMath.EMPTY_FRAMES[8]
    {
      new MeterMath.EMPTY_FRAMES(new string[3]
      {
        "",
        "",
        "DII_FRAME_SEGS3_DEC7"
      }),
      new MeterMath.EMPTY_FRAMES(new string[1]{ "" }),
      new MeterMath.EMPTY_FRAMES(new string[1]{ "" }),
      new MeterMath.EMPTY_FRAMES(new string[3]
      {
        "",
        "",
        "DII_FRAME_SEGS3_DEC4"
      }),
      new MeterMath.EMPTY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC3",
        ""
      }),
      new MeterMath.EMPTY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC2",
        ""
      }),
      new MeterMath.EMPTY_FRAMES(new string[2]
      {
        "DII_FRAME_SEGS1_DEC1",
        ""
      }),
      new MeterMath.EMPTY_FRAMES(new string[2]{ "0x00", "" })
    };
    private static short[] ZelsiusInfoByte1 = new short[11]
    {
      (short) -1,
      (short) -1,
      (short) -1,
      (short) 48,
      (short) 49,
      (short) 50,
      (short) 51,
      (short) 99,
      (short) 100,
      (short) 101,
      (short) 102
    };
    private static short FirstLinearIndexEnergyInJoule = 16;
    private static short FirstIndexEnergyInJoule = 19;
    private static short[] ZelsiusInfoByte2 = new short[35]
    {
      (short) -1,
      (short) -1,
      (short) -1,
      (short) -1,
      (short) -1,
      (short) -1,
      (short) 0,
      (short) 48,
      (short) 49,
      (short) 50,
      (short) 51,
      (short) 99,
      (short) 100,
      (short) 101,
      (short) 102,
      (short) 150,
      (short) 151,
      (short) 152,
      (short) 153,
      (short) -1,
      (short) -1,
      (short) -1,
      (short) 0,
      (short) 48,
      (short) 49,
      (short) 50,
      (short) 51,
      (short) 99,
      (short) 100,
      (short) 101,
      (short) 102,
      (short) 150,
      (short) 151,
      (short) 152,
      (short) 153
    };
    private static MeterMath.ENERGY_FRAMES[] ZelsiusSonderFrames = new MeterMath.ENERGY_FRAMES[1]
    {
      new MeterMath.ENERGY_FRAMES(new string[3]
      {
        "",
        "DII_FRAME_SEGS2_MEGA_JOULE",
        "DII_FRAME_SEGS3_DEC7"
      }, new string[3]
      {
        "",
        "DII_FRAME_SEGS2_MEGA_JOULE DII_FRAME_SEGS2_SLASH DII_FRAME_SEGS2_HOUR",
        "DII_FRAME_SEGS3_DEC4"
      })
    };
    public static string[] ClearZelsiusUnitFrameMasks = new string[3]
    {
      "DII_FRAME_SEGS1_DEC1 DII_FRAME_SEGS1_DEC2 DII_FRAME_SEGS1_DEC3 DII_FRAME_SEGS1_FRAME1 DII_FRAME_SEGS1_FRAME2 DII_FRAME_SEGS1_FRAME3",
      "DII_FRAME_SEGS2_CUBIC_M DII_FRAME_SEGS2_LITRE DII_FRAME_SEGS2_SLASH DII_FRAME_SEGS2_HOUR DII_FRAME_SEGS2_KILO DII_FRAME_SEGS2_WATT DII_FRAME_SEGS2_MEGA DII_FRAME_SEGS2_MEGA_JOULE",
      "DII_FRAME_SEGS3_DEC4"
    };
    private static string[] RestrictedEnergyUnitsList = (string[]) null;
    private static string[] RestrictedVolumeUnitsList = (string[]) null;
    private static string[] RestrictedInputUnitsList = (string[]) null;

    public ZelsiusMath()
    {
      this.NeadedMeterVars = new SortedList();
      this.NeadedMeterVars.Add((object) "DefaultFunction.Vol_SumExpo", (object) null);
      this.NeadedMeterVars.Add((object) "DefaultFunction.PulsValue1", (object) null);
      this.NeadedMeterVars.Add((object) "DefaultFunction.Energ_SumExpo", (object) null);
      this.NeadedMeterVars.Add((object) "DefaultFunction.Energ_ImpulsFaktor", (object) null);
      this.NeadedMeterVars.Add((object) "DefaultFunction.Zaehler_Info_Byte1", (object) null);
      this.NeadedMeterVars.Add((object) "DefaultFunction.Zaehler_Info_Byte2", (object) null);
      this.NeadedMeterVars.Add((object) "DefaultFunction.Energie_Konfiguration", (object) null);
      this.NeadedMeterVars.Add((object) "DefaultFunction.RW_Typ_Konfiguration", (object) null);
      this.NeadedMeterVars.Add((object) "R:Inp1Factor", (object) null);
      this.NeadedMeterVars.Add((object) "R:Inp2Factor", (object) null);
    }

    public static string[] GetRestrictedEnergyUnitList()
    {
      if (ZelsiusMath.RestrictedEnergyUnitsList == null)
      {
        List<string> stringList = new List<string>();
        for (int index = 0; index < MeterMath.EnergyUnits.Length; ++index)
        {
          if (ZelsiusMath.ZelsiusEnergyFrames[index].EnergyFrame[0].Length > 0 || ZelsiusMath.ZelsiusEnergyFrames[index].EnergyFrame.Length > 1)
            stringList.Add(MeterMath.EnergyUnits[index].EnergieUnitString);
        }
        ZelsiusMath.RestrictedEnergyUnitsList = new string[stringList.Count];
        for (int index = 0; index < stringList.Count; ++index)
          ZelsiusMath.RestrictedEnergyUnitsList[index] = stringList[index];
      }
      return ZelsiusMath.RestrictedEnergyUnitsList;
    }

    public static string[] GetRestrictedVolumeUnitList()
    {
      if (ZelsiusMath.RestrictedVolumeUnitsList == null)
      {
        List<string> stringList = new List<string>();
        for (int index = 0; index < MeterMath.VolumeUnits.Length; ++index)
        {
          if (ZelsiusMath.ZelsiusVolumeFrames[index].VolumeFrame[0].Length > 0 || ZelsiusMath.ZelsiusVolumeFrames[index].VolumeFrame.Length > 1)
            stringList.Add(MeterMath.VolumeUnits[index].VolumeUnitString);
        }
        ZelsiusMath.RestrictedVolumeUnitsList = new string[stringList.Count];
        for (int index = 0; index < stringList.Count; ++index)
          ZelsiusMath.RestrictedVolumeUnitsList[index] = stringList[index];
      }
      return ZelsiusMath.RestrictedVolumeUnitsList;
    }

    public static string[] GetRestrictedInputUnitList()
    {
      if (ZelsiusMath.RestrictedInputUnitsList == null)
      {
        List<string> stringList = new List<string>();
        for (int index1 = 0; index1 < MeterMath.InputUnits.Length; ++index1)
        {
          MeterMath.INPUT_UNIT_DEFS inputUnit = MeterMath.InputUnits[index1];
          if (inputUnit.FrameType == MeterMath.InputFrameType.Energy)
          {
            for (int index2 = 0; index2 < MeterMath.EnergyUnits.Length; ++index2)
            {
              if (!(inputUnit.InputUnitString != MeterMath.EnergyUnits[index2].EnergieUnitString))
              {
                if (ZelsiusMath.ZelsiusEnergyFrames[index2].EnergyFrame[0].Length > 0 || ZelsiusMath.ZelsiusEnergyFrames[index2].EnergyFrame.Length > 1)
                {
                  stringList.Add(MeterMath.EnergyUnits[index2].EnergieUnitString);
                  break;
                }
                break;
              }
            }
          }
          else if (inputUnit.FrameType == MeterMath.InputFrameType.Volume)
          {
            for (int index3 = 0; index3 < MeterMath.VolumeUnits.Length; ++index3)
            {
              if (!(inputUnit.InputUnitString != MeterMath.VolumeUnits[index3].VolumeUnitString))
              {
                if (ZelsiusMath.ZelsiusVolumeFrames[index3].VolumeFrame[0].Length > 0 || ZelsiusMath.ZelsiusVolumeFrames[index3].VolumeFrame.Length > 1)
                {
                  stringList.Add(MeterMath.VolumeUnits[index3].VolumeUnitString);
                  break;
                }
                break;
              }
            }
          }
          else
          {
            for (int index4 = 0; index4 < MeterMath.EmptyUnits.Length; ++index4)
            {
              if (!(inputUnit.InputUnitString != MeterMath.EmptyUnits[index4].EmptyUnitString))
              {
                if (ZelsiusMath.ZelsiusEmptyFrames[index4].EmptyFrame[0].Length > 0 || ZelsiusMath.ZelsiusEmptyFrames[index4].EmptyFrame.Length > 1)
                {
                  stringList.Add(MeterMath.EmptyUnits[index4].EmptyUnitString);
                  break;
                }
                break;
              }
            }
          }
        }
        ZelsiusMath.RestrictedInputUnitsList = new string[stringList.Count];
        for (int index = 0; index < stringList.Count; ++index)
          ZelsiusMath.RestrictedInputUnitsList[index] = stringList[index];
      }
      return ZelsiusMath.RestrictedInputUnitsList;
    }

    public bool GetNotAvailableOverrides(SortedList OverridesList)
    {
      byte neadedMeterVar1 = (byte) (long) this.NeadedMeterVars[(object) "DefaultFunction.Energie_Konfiguration"];
      object neadedMeterVar2 = this.NeadedMeterVars[(object) "DefaultFunction.RW_Typ_Konfiguration"];
      byte num1 = neadedMeterVar2 != null ? (byte) (long) neadedMeterVar2 : (byte) 0;
      OverrideParameter overrideParameter1 = new OverrideParameter(OverrideID.WarmerPipe);
      overrideParameter1.Value = ((int) neadedMeterVar1 & 1) != 0 ? 1UL : 0UL;
      OverridesList.Add((object) overrideParameter1.ParameterID, (object) overrideParameter1);
      OverrideParameter TheOverrideParameter1 = new OverrideParameter(OverrideID.BaseConfig);
      TheOverrideParameter1.Value = 1000000UL;
      byte num2 = (byte) ((uint) neadedMeterVar1 & (uint) ZelsiusMath.BC_SettingsMask.Energie_Konfiguration);
      byte num3 = (byte) ((uint) num1 & (uint) ZelsiusMath.BC_SettingsMask.RW_Typ_Konfiguration);
      for (int index = 0; index < ZelsiusMath.BC_SettingsList.Length; ++index)
      {
        if ((int) num2 == (int) ZelsiusMath.BC_SettingsList[index].Energie_Konfiguration && (int) num3 == (int) ZelsiusMath.BC_SettingsList[index].RW_Typ_Konfiguration)
        {
          TheOverrideParameter1.Value = (ulong) index;
          break;
        }
      }
      if (TheOverrideParameter1.Value > 1000UL)
      {
        this.LastError = MeterMath.Errors.InternalError;
        this.LastErrorInfo = "Illegal BaseConfig";
        return false;
      }
      OverrideParameter.ChangeOrAddOverrideParameter(OverridesList, TheOverrideParameter1);
      if ((OverrideParameter) OverridesList[(object) OverrideID.EnergyResolution] == null)
      {
        OverrideParameter overrideParameter2 = new OverrideParameter(OverrideID.EnergyResolution);
        OverridesList.Add((object) overrideParameter2.ParameterID, (object) overrideParameter2);
      }
      OverrideParameter overrideParameter3 = (OverrideParameter) OverridesList[(object) OverrideID.VolumeResolution];
      if (overrideParameter3 == null)
      {
        overrideParameter3 = new OverrideParameter(OverrideID.VolumeResolution);
        OverridesList.Add((object) overrideParameter3.ParameterID, (object) overrideParameter3);
      }
      if ((OverrideParameter) OverridesList[(object) OverrideID.VolumePulsValue] == null)
      {
        Decimal neadedMeterVar3 = (Decimal) (long) this.NeadedMeterVars[(object) "DefaultFunction.PulsValue1"];
        OverrideParameter overrideParameter4 = new OverrideParameter(OverrideID.VolumePulsValue, ((Decimal) (1.0 / Math.Pow(2.0, (double) (Decimal) (long) this.NeadedMeterVars[(object) "DefaultFunction.Vol_SumExpo"])) / (Decimal) MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[overrideParameter3.Value].LinearUnitIndex].UnitFactorFromLiter * neadedMeterVar3).ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat), true);
        OverridesList.Add((object) overrideParameter4.ParameterID, (object) overrideParameter4);
      }
      OverrideParameter overrides1 = (OverrideParameter) OverridesList[(object) OverrideID.Input1Unit];
      OverrideParameter overrides2 = (OverrideParameter) OverridesList[(object) OverrideID.Input1PulsValue];
      if (overrides1 == null || overrides2 == null)
      {
        OverrideParameter TheOverrideParameter2 = new OverrideParameter(OverrideID.Input1Unit);
        OverrideParameter.ChangeOrAddOverrideParameter(OverridesList, TheOverrideParameter2);
        OverrideParameter TheOverrideParameter3 = new OverrideParameter(OverrideID.Input1PulsValue);
        OverrideParameter.ChangeOrAddOverrideParameter(OverridesList, TheOverrideParameter3);
      }
      OverrideParameter overrides3 = (OverrideParameter) OverridesList[(object) OverrideID.Input2Unit];
      OverrideParameter overrides4 = (OverrideParameter) OverridesList[(object) OverrideID.Input2PulsValue];
      if (overrides3 == null || overrides4 == null)
      {
        OverrideParameter TheOverrideParameter4 = new OverrideParameter(OverrideID.Input2Unit);
        OverrideParameter.ChangeOrAddOverrideParameter(OverridesList, TheOverrideParameter4);
        OverrideParameter TheOverrideParameter5 = new OverrideParameter(OverrideID.Input2PulsValue);
        OverrideParameter.ChangeOrAddOverrideParameter(OverridesList, TheOverrideParameter5);
      }
      return true;
    }

    public bool CreateBaseSettings(SortedList OverridesList)
    {
      this.MyBaseSettings = new ZelsiusBaseSettings();
      OverrideParameter overrides1 = (OverrideParameter) OverridesList[(object) OverrideID.BaseConfig];
      if (overrides1 != null)
        this.MyBaseSettings.BaseConfig = ((ConfigurationParameter.BaseConfigSettings) overrides1.Value).ToString();
      OverrideParameter overrides2 = (OverrideParameter) OverridesList[(object) OverrideID.EnergyResolution];
      if (overrides2 != null)
        this.MyBaseSettings.EnergyUnit = MeterMath.GetEnergyUnitOfID((int) overrides2.Value);
      OverrideParameter overrides3 = (OverrideParameter) OverridesList[(object) OverrideID.VolumeResolution];
      if (overrides3 != null)
        this.MyBaseSettings.VolumeUnit = MeterMath.GetVolumeUnitOfID((int) overrides3.Value);
      OverrideParameter overrides4 = (OverrideParameter) OverridesList[(object) OverrideID.VolumePulsValue];
      if (overrides4 != null && !OverrideParameter.PackedBCD_ToDouble((int) overrides4.Value, out this.MyBaseSettings.PulsValueInLiterPerImpuls))
      {
        this.LastError = MeterMath.Errors.InternalError;
        this.LastErrorInfo = "Volume puls value convertion error";
        return false;
      }
      OverrideParameter overrides5 = (OverrideParameter) OverridesList[(object) OverrideID.WarmerPipe];
      if (overrides5 == null)
      {
        this.LastError = MeterMath.Errors.InternalError;
        this.LastErrorInfo = "Missing WarmerPipe override";
        return false;
      }
      byte neadedMeterVar = (byte) (long) this.NeadedMeterVars[(object) "DefaultFunction.Energie_Konfiguration"];
      this.NeadedMeterVars[(object) "DefaultFunction.Energie_Konfiguration"] = (object) (overrides5.Value != 0UL ? (long) (byte) ((uint) neadedMeterVar | 1U) : (long) (byte) ((uint) neadedMeterVar & 254U));
      OverrideParameter overrides6 = (OverrideParameter) OverridesList[(object) OverrideID.Input1Unit];
      if (overrides6 != null)
        this.MyBaseSettings.Input1Unit = MeterMath.GetInputUnitOfID((int) overrides6.Value);
      OverrideParameter overrides7 = (OverrideParameter) OverridesList[(object) OverrideID.Input1PulsValue];
      if (overrides7 != null && !OverrideParameter.PackedBCD_ToDouble((int) overrides7.Value, out this.MyBaseSettings.Input1PulsValue))
      {
        this.LastError = MeterMath.Errors.InternalError;
        this.LastErrorInfo = "Input1 puls value convertion error";
        return false;
      }
      OverrideParameter overrides8 = (OverrideParameter) OverridesList[(object) OverrideID.Input2Unit];
      if (overrides8 != null)
        this.MyBaseSettings.Input2Unit = MeterMath.GetInputUnitOfID((int) overrides8.Value);
      OverrideParameter overrides9 = (OverrideParameter) OverridesList[(object) OverrideID.Input2PulsValue];
      if (overrides9 == null || OverrideParameter.PackedBCD_ToDouble((int) overrides9.Value, out this.MyBaseSettings.Input2PulsValue))
        return true;
      this.LastError = MeterMath.Errors.InternalError;
      this.LastErrorInfo = "Input2 puls value convertion error";
      return false;
    }

    public override double calcPulsValue(int PulsValue, string UnitString)
    {
      int decimalCounts;
      ParameterService.GetValueAndUnit(UnitString, out string _, out string _, out decimalCounts);
      if (decimalCounts < 0)
        decimalCounts = 0;
      double num = Math.Pow(10.0, (double) decimalCounts);
      return (double) (PulsValue / 64) / num;
    }

    public override double calcPulsValue(double PulsValue, double Vol_SumExpo, string VolumeUnit)
    {
      double num1 = PulsValue / Math.Pow(2.0, Vol_SumExpo);
      double num2 = -1.0;
      for (int index = 0; index < MeterMath.VolumeUnits.Length; ++index)
      {
        if (MeterMath.VolumeUnits[index].VolumeUnitString == VolumeUnit)
          num2 = MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[index].LinearUnitIndex].UnitFactorFromLiter;
      }
      if (num2 < 0.0)
        return -1.0;
      double num3 = num1 / num2;
      double num4 = 0.9999;
      double num5 = 1.0;
      for (double num6 = 1.0001; num3 > num4 && num6 < 100000.0; num6 *= 10.0)
      {
        if (num3 > num4 && num3 < num6)
        {
          num3 = num5;
          break;
        }
        num4 *= 10.0;
        num5 *= 10.0;
      }
      return num3;
    }

    public override bool GetSpecialOverrideFrame(
      FrameTypes FrameType,
      string SpecialOptions,
      out MeterMath.FrameDescription TheFrame,
      out int FactorShift)
    {
      TheFrame = (MeterMath.FrameDescription) null;
      FactorShift = 0;
      bool flag = false;
      int num = 0;
      string[] strArray1 = SpecialOptions.Split(' ');
      if (strArray1[0].Length > 0)
      {
        for (int index = 0; index < strArray1.Length; ++index)
        {
          string[] strArray2 = strArray1[index].Split('=');
          if (strArray2.Length != 2)
          {
            this.LastError = MeterMath.Errors.InternalError;
            this.LastErrorInfo = "Partial special frame option";
            return false;
          }
          if (strArray1[index] == "F=1")
            flag = true;
          else if (strArray2[0] == "S" || strArray2[0] == "OS")
          {
            try
            {
              num = int.Parse(strArray2[1]);
            }
            catch
            {
              this.LastError = MeterMath.Errors.InternalError;
              this.LastErrorInfo = "Illegal special frame constant";
              return false;
            }
          }
          else
          {
            this.LastError = MeterMath.Errors.InternalError;
            this.LastErrorInfo = "Unknown special frame option";
            return false;
          }
        }
      }
      if (num != 0)
      {
        if (this.MyBaseSettings.PulsValueInLiterPerImpuls < 0.0)
          return false;
        switch (FrameType)
        {
          case FrameTypes.Energy:
            int LinearUnitIndex = this.MyBaseSettings.EnergyLinearUnitIndex - num;
            int fromLiniarUnitIndex1 = MeterMath.GetEnergyUnitIndexFromLiniarUnitIndex(LinearUnitIndex);
            if (this.MyBaseSettings.EnergyLinearUnitIndex < (int) ZelsiusMath.FirstLinearIndexEnergyInJoule)
            {
              if (fromLiniarUnitIndex1 < 0 || ZelsiusMath.ZelsiusEnergyFrames[fromLiniarUnitIndex1].EnergyFrame.Length < 2)
              {
                this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
                this.LastErrorInfo = "Special frame format";
                return false;
              }
            }
            else
            {
              while (true)
              {
                if (fromLiniarUnitIndex1 >= (int) ZelsiusMath.FirstIndexEnergyInJoule)
                {
                  if (ZelsiusMath.ZelsiusEnergyFrames[fromLiniarUnitIndex1].EnergyFrame.Length < 3)
                  {
                    --FactorShift;
                    ++LinearUnitIndex;
                    fromLiniarUnitIndex1 = MeterMath.GetEnergyUnitIndexFromLiniarUnitIndex(LinearUnitIndex);
                  }
                  else
                    goto label_23;
                }
                else
                  break;
              }
              this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
              this.LastErrorInfo = "Special frame format";
              return false;
            }
label_23:
            TheFrame = new MeterMath.FrameDescription(ZR_Constants.FrameNames.EnergyFrame.ToString(), ZelsiusMath.ZelsiusEnergyFrames[fromLiniarUnitIndex1].EnergyFrame);
            break;
          case FrameTypes.Flow:
            int fromLiniarUnitIndex2 = this.GetVolumeUnitIndexFromLiniarUnitIndex(this.MyBaseSettings.VolumeLinearUnitIndex - num);
            if (fromLiniarUnitIndex2 < 0 || ZelsiusMath.ZelsiusVolumeFrames[fromLiniarUnitIndex2].FlowFrame.Length < 2)
            {
              this.LastError = MeterMath.Errors.FlowOutOfRange;
              this.LastErrorInfo = "Special frame format";
              return false;
            }
            TheFrame = new MeterMath.FrameDescription(ZR_Constants.FrameNames.FlowFrame.ToString(), ZelsiusMath.ZelsiusVolumeFrames[fromLiniarUnitIndex2].FlowFrame);
            break;
          case FrameTypes.Volume:
            int fromLiniarUnitIndex3 = this.GetVolumeUnitIndexFromLiniarUnitIndex(this.MyBaseSettings.VolumeLinearUnitIndex - num);
            if (fromLiniarUnitIndex3 < 0 || ZelsiusMath.ZelsiusVolumeFrames[fromLiniarUnitIndex3].VolumeFrame.Length < 2)
            {
              this.LastError = MeterMath.Errors.VolumeUnitNotAvailable;
              this.LastErrorInfo = "Special frame format";
              return false;
            }
            TheFrame = new MeterMath.FrameDescription(ZR_Constants.FrameNames.VolumeFrame.ToString(), ZelsiusMath.ZelsiusVolumeFrames[fromLiniarUnitIndex3].VolumeFrame);
            break;
          default:
            this.LastError = MeterMath.Errors.InternalError;
            this.LastErrorInfo = "Illegal frame type";
            return false;
        }
      }
      else
      {
        foreach (MeterMath.FrameDescription frame in this.MyBaseSettings.Frames)
        {
          if (frame.Type == FrameType)
          {
            TheFrame = frame.Clone();
            break;
          }
        }
      }
      if (TheFrame == null)
        return false;
      if (flag)
      {
        if (TheFrame.FrameByteDescription[0].IndexOf("DII_FRAME_SEGS1_DEC1") >= 0)
        {
          // ISSUE: explicit reference operation
          ^ref TheFrame.FrameByteDescription[0] += " DII_FRAME_SEGS1_FRAME1";
        }
        else if (TheFrame.FrameByteDescription[0].IndexOf("DII_FRAME_SEGS1_DEC2") >= 0)
        {
          // ISSUE: explicit reference operation
          ^ref TheFrame.FrameByteDescription[0] += " DII_FRAME_SEGS1_FRAME1 DII_FRAME_SEGS1_FRAME2";
        }
        else if (TheFrame.FrameByteDescription[0].IndexOf("DII_FRAME_SEGS1_DEC3") >= 0)
        {
          // ISSUE: explicit reference operation
          ^ref TheFrame.FrameByteDescription[0] += " DII_FRAME_SEGS1_FRAME1 DII_FRAME_SEGS1_FRAME2 DII_FRAME_SEGS1_FRAME3";
        }
      }
      return true;
    }

    public override bool CalculateMeterSettings(long FirmwareVersion)
    {
      try
      {
        this.MyBaseSettings.Frames = new ArrayList();
        this.MyBaseSettings.BaseConfigIndex = (int) Enum.Parse(typeof (BaseConfigID), this.MyBaseSettings.BaseConfig, true);
        OverrideParameter.GetBaseConfigStruct(this.MyBaseSettings.BaseConfig);
        this.MyBaseSettings.Frames.Add((object) new MeterMath.FrameDescription(ZR_Constants.FrameNames.BCFrame.ToString(), ZelsiusMath.ZelsiusBCFrames[this.MyBaseSettings.BaseConfigIndex].BC_Frame));
        this.MyBaseSettings.VolumeUnitIndex = MeterMath.GetVolumeUnitIndex(this.MyBaseSettings.VolumeUnit);
        if (this.MyBaseSettings.VolumeUnitIndex < 0 || this.MyBaseSettings.VolumeUnitIndex >= MeterMath.VolumeUnits.Length)
        {
          this.LastError = MeterMath.Errors.VolumeUnitNotAvailable;
          return false;
        }
        ArrayList frames1 = this.MyBaseSettings.Frames;
        ZR_Constants.FrameNames frameNames = ZR_Constants.FrameNames.VolumeFrame;
        MeterMath.FrameDescription frameDescription1 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusVolumeFrames[this.MyBaseSettings.VolumeUnitIndex].VolumeFrame);
        frames1.Add((object) frameDescription1);
        ArrayList frames2 = this.MyBaseSettings.Frames;
        frameNames = ZR_Constants.FrameNames.FlowFrame;
        MeterMath.FrameDescription frameDescription2 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusVolumeFrames[this.MyBaseSettings.VolumeUnitIndex].FlowFrame);
        frames2.Add((object) frameDescription2);
        this.MyBaseSettings.VolumeLinearUnitIndex = (int) MeterMath.VolumeUnits[this.MyBaseSettings.VolumeUnitIndex].LinearUnitIndex;
        if (this.MyBaseSettings.VolumeUnitIndex >= ZelsiusMath.ZelsiusInfoByte1.Length)
        {
          this.LastError = MeterMath.Errors.VolumeUnitNotAvailable;
          this.LastErrorInfo = "Zähler_Info_Byte1";
          return false;
        }
        short num1 = ZelsiusMath.ZelsiusInfoByte1[this.MyBaseSettings.VolumeUnitIndex];
        if (num1 == (short) -1)
        {
          this.LastError = MeterMath.Errors.VolumeUnitNotAvailable;
          this.LastErrorInfo = "Zähler_Info_Byte1";
          return false;
        }
        this.NeadedMeterVars[(object) "DefaultFunction.Zaehler_Info_Byte1"] = (object) (long) num1;
        this.MyBaseSettings.MBusVolumeVIF = MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[this.MyBaseSettings.VolumeUnitIndex].LinearUnitIndex].MBusVolumeVIF;
        this.MyBaseSettings.MBusFlowVIF = MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[this.MyBaseSettings.VolumeUnitIndex].LinearUnitIndex].MBusFlowVIF;
        long num2 = 0;
        this.MyBaseSettings.EnergyUnitIndex = MeterMath.GetEnergyUnitIndex(this.MyBaseSettings.EnergyUnit);
        if (this.MyBaseSettings.EnergyUnitIndex < 0 || this.MyBaseSettings.EnergyUnitIndex >= MeterMath.EnergyUnits.Length)
        {
          this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
          return false;
        }
        ArrayList frames3 = this.MyBaseSettings.Frames;
        frameNames = ZR_Constants.FrameNames.EnergyFrame;
        MeterMath.FrameDescription frameDescription3 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEnergyFrames[this.MyBaseSettings.EnergyUnitIndex].EnergyFrame);
        frames3.Add((object) frameDescription3);
        this.MyBaseSettings.EnergyLinearUnitIndex = (int) MeterMath.EnergyUnits[this.MyBaseSettings.EnergyUnitIndex].LinearUnitIndex;
        if (this.MyBaseSettings.EnergyUnitIndex >= ZelsiusMath.ZelsiusInfoByte2.Length)
        {
          this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
          this.LastErrorInfo = "Zähler_Info_Byte2";
          return false;
        }
        short num3 = ZelsiusMath.ZelsiusInfoByte2[this.MyBaseSettings.EnergyUnitIndex];
        if (num3 == (short) -1)
        {
          this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
          this.LastErrorInfo = "Zähler_Info_Byte2";
          return false;
        }
        this.NeadedMeterVars[(object) "DefaultFunction.Zaehler_Info_Byte2"] = (object) (long) num3;
        bool flag = this.MyBaseSettings.EnergyLinearUnitIndex >= (int) ZelsiusMath.FirstLinearIndexEnergyInJoule;
        this.MyBaseSettings.PowerLinearUnitIndex = (int) MeterMath.EnergyUnits[this.MyBaseSettings.EnergyUnitIndex].LinearUnitIndex;
        --this.MyBaseSettings.PowerLinearUnitIndex;
        this.MyBaseSettings.PowerUnitIndex = MeterMath.GetEnergyUnitIndexFromLiniarUnitIndex(this.MyBaseSettings.PowerLinearUnitIndex);
        if (this.MyBaseSettings.PowerUnitIndex < 0)
        {
          this.LastError = MeterMath.Errors.PowerUnitNotAvailable;
          return false;
        }
        if (flag && this.MyBaseSettings.PowerLinearUnitIndex < (int) ZelsiusMath.FirstLinearIndexEnergyInJoule)
        {
          this.LastError = MeterMath.Errors.PowerUnitNotAvailable;
          return false;
        }
        if (flag)
        {
          if (this.MyBaseSettings.PowerUnitIndex == 29)
          {
            ArrayList frames4 = this.MyBaseSettings.Frames;
            frameNames = ZR_Constants.FrameNames.PowerFrame;
            MeterMath.FrameDescription frameDescription4 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusSonderFrames[0].PowerFrame);
            frames4.Add((object) frameDescription4);
          }
          else
          {
            ArrayList frames5 = this.MyBaseSettings.Frames;
            frameNames = ZR_Constants.FrameNames.PowerFrame;
            MeterMath.FrameDescription frameDescription5 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEnergyFrames[this.MyBaseSettings.PowerUnitIndex].PowerFrame);
            frames5.Add((object) frameDescription5);
          }
        }
        else
        {
          ArrayList frames6 = this.MyBaseSettings.Frames;
          frameNames = ZR_Constants.FrameNames.PowerFrame;
          MeterMath.FrameDescription frameDescription6 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEnergyFrames[this.MyBaseSettings.PowerUnitIndex].PowerFrame);
          frames6.Add((object) frameDescription6);
        }
        if (FirmwareVersion <= 17039360L && this.MyBaseSettings.VolumeUnit != "?")
        {
          if (this.MyBaseSettings.VolumeUnitIndex < 0 || this.MyBaseSettings.VolumeUnitIndex >= ZelsiusMath.ZelsiusInfoByte1.Length)
          {
            this.LastError = MeterMath.Errors.VolumeUnitNotAvailable;
            this.LastErrorInfo = "Zähler_Info_Byte1";
            return false;
          }
          int num4 = (int) ZelsiusMath.ZelsiusInfoByte1[this.MyBaseSettings.VolumeUnitIndex];
          if (num4 == -1)
          {
            this.LastError = MeterMath.Errors.VolumeUnitNotAvailable;
            this.LastErrorInfo = "Zähler_Info_Byte1";
            return false;
          }
          int y = num4 & 15;
          if (this.MyBaseSettings.EnergyUnitIndex < 0 || this.MyBaseSettings.EnergyUnitIndex >= ZelsiusMath.ZelsiusInfoByte2.Length)
          {
            this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
            this.LastErrorInfo = "Zähler_Info_Byte2";
            return false;
          }
          int num5 = (int) ZelsiusMath.ZelsiusInfoByte2[this.MyBaseSettings.EnergyUnitIndex];
          if (num5 == -1)
          {
            this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
            this.LastErrorInfo = "Zähler_Info_Byte2";
            return false;
          }
          int num6 = num5 & 15;
          long num7 = 1000000;
          long num8 = (long) Math.Pow(10.0, (double) y);
          long num9;
          long num10;
          if (flag)
          {
            num9 = 1000L;
            num10 = (long) Math.Pow(10.0, (double) (num6 - 1));
          }
          else
          {
            num9 = 1L;
            num10 = (long) Math.Pow(10.0, (double) (num6 - 1));
          }
          num2 = num7 * num10 / (num9 * num8) / 100L;
          if (num2 <= 0L || num2 > 10000L)
          {
            this.LastError = MeterMath.Errors.PowerUnitNotAvailable;
            return false;
          }
        }
        this.MyBaseSettings.MBusEnergieVIF = MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[this.MyBaseSettings.EnergyUnitIndex].LinearUnitIndex].MBusEnergieVIF;
        this.MyBaseSettings.MBusPowerVIF = MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[this.MyBaseSettings.PowerUnitIndex].LinearUnitIndex].MBusPowerVIF;
        if (this.MyBaseSettings.PulsValueInLiterPerImpuls > 0.0)
        {
          if (this.MyBaseSettings.PulsValueInLiterPerImpuls > 10000.0 || this.MyBaseSettings.PulsValueInLiterPerImpuls < 0.001)
          {
            this.LastError = MeterMath.Errors.PulsValueOutOfRange;
            return false;
          }
          ArrayList frames7 = this.MyBaseSettings.Frames;
          frameNames = ZR_Constants.FrameNames.ImpulsValueFrame;
          MeterMath.FrameDescription frameDescription7 = new MeterMath.FrameDescription(frameNames.ToString(), this.GetImpulseValueFrame(this.MyBaseSettings.PulsValueInLiterPerImpuls));
          frames7.Add((object) frameDescription7);
          double a = this.MyBaseSettings.PulsValueInLiterPerImpuls * MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[this.MyBaseSettings.VolumeUnitIndex].LinearUnitIndex].UnitFactorFromLiter;
          int num11 = 0;
          while (a < 65536.0)
          {
            a *= 2.0;
            ++num11;
          }
          while (a >= 65535.5)
          {
            a /= 2.0;
            --num11;
          }
          this.MyBaseSettings.ZelsiusVolPulsValue = (int) Math.Round(a);
          this.MyBaseSettings.Vol_SumExpo = (sbyte) num11;
          this.MyBaseSettings.PulsValueInLiterPerImpulsUsed = (double) this.MyBaseSettings.ZelsiusVolPulsValue / Math.Pow(2.0, (double) this.MyBaseSettings.Vol_SumExpo);
          this.NeadedMeterVars[(object) "DefaultFunction.Vol_SumExpo"] = (object) (long) this.MyBaseSettings.Vol_SumExpo;
          this.NeadedMeterVars[(object) "DefaultFunction.PulsValue1"] = (object) (long) this.MyBaseSettings.ZelsiusVolPulsValue;
          double num12 = 1E-05;
          double num13 = 1.0 / MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[this.MyBaseSettings.VolumeUnitIndex].LinearUnitIndex].UnitFactorFromLiter;
          double unitFactorFromWh = MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[this.MyBaseSettings.EnergyUnitIndex].LinearUnitIndex].UnitFactorFromWh;
          if (this.MyBaseSettings.EnergyUnitIndex >= (int) ZelsiusMath.FirstIndexEnergyInJoule)
            unitFactorFromWh /= 3.6;
          double num14 = num12 * num13 * unitFactorFromWh;
          int num15 = 0;
          while (num14 < 65536.0)
          {
            num14 *= 2.0;
            ++num15;
          }
          while (num14 >= 65536.0)
          {
            num14 /= 2.0;
            --num15;
          }
          int num16 = (int) this.MyBaseSettings.Vol_SumExpo + num15 - 32;
          this.MyBaseSettings.ZelsiusEnergyPulsValue = (int) num14;
          this.MyBaseSettings.Energy_SumExpo = (byte) num16;
          this.NeadedMeterVars[(object) "DefaultFunction.Energ_SumExpo"] = (object) (long) this.MyBaseSettings.Energy_SumExpo;
          this.NeadedMeterVars[(object) "DefaultFunction.Energ_ImpulsFaktor"] = (object) (long) this.MyBaseSettings.ZelsiusEnergyPulsValue;
          if (64 - (int) this.MyBaseSettings.Vol_SumExpo < 28 || this.MyBaseSettings.Vol_SumExpo < (sbyte) 0)
          {
            this.LastError = MeterMath.Errors.VolumeUnitNotAvailable;
            this.LastErrorInfo = "Resolution out of range!";
            return false;
          }
          if (64 - (int) this.MyBaseSettings.Energy_SumExpo < 28 || this.MyBaseSettings.Energy_SumExpo < (byte) 0)
          {
            this.LastError = MeterMath.Errors.EnergyUnitNotAvailable;
            this.LastErrorInfo = "Resolution out of range!";
            return false;
          }
          if (FirmwareVersion <= 17039360L && this.MyBaseSettings.Vol_SumExpo < (sbyte) 4)
          {
            this.LastError = MeterMath.Errors.FlowOutOfRange;
            this.LastErrorInfo = "Firmware <= 1.4.0";
            return false;
          }
          byte num17 = (byte) (long) this.NeadedMeterVars[(object) "DefaultFunction.Energie_Konfiguration"];
          object neadedMeterVar = this.NeadedMeterVars[(object) "DefaultFunction.RW_Typ_Konfiguration"];
          byte num18 = neadedMeterVar != null ? (byte) (long) neadedMeterVar : (byte) 0;
          if (this.MyBaseSettings.EnergyUnit != "?")
          {
            if (this.MyBaseSettings.EnergyUnitIndex >= (int) ZelsiusMath.FirstIndexEnergyInJoule)
              num17 |= (byte) 8;
            else
              num17 &= (byte) 247;
          }
          if (this.MyBaseSettings.BaseConfig != "nil")
          {
            byte num19 = (byte) ((uint) num17 & (uint) ~ZelsiusMath.BC_SettingsMask.Energie_Konfiguration);
            byte num20 = (byte) ((uint) num18 & (uint) ~ZelsiusMath.BC_SettingsMask.RW_Typ_Konfiguration);
            num17 = (byte) ((uint) num19 | (uint) ZelsiusMath.BC_SettingsList[this.MyBaseSettings.BaseConfigIndex].Energie_Konfiguration);
            num18 = (byte) ((uint) num20 | (uint) ZelsiusMath.BC_SettingsList[this.MyBaseSettings.BaseConfigIndex].RW_Typ_Konfiguration);
          }
          this.NeadedMeterVars[(object) "DefaultFunction.Energie_Konfiguration"] = (object) (long) num17;
          if (neadedMeterVar != null)
            this.NeadedMeterVars[(object) "DefaultFunction.RW_Typ_Konfiguration"] = (object) (long) num18;
          if (FirmwareVersion <= 17039360L && num2 <= 0L)
          {
            this.LastError = MeterMath.Errors.PowerOutOfRange;
            this.LastErrorInfo = "Firmware <= 1.4.0";
            return false;
          }
        }
        for (int index1 = 1; index1 < 3; ++index1)
        {
          string DisplayValue;
          double num21;
          if (index1 == 1)
          {
            if (!(this.MyBaseSettings.Input1Unit == "?"))
            {
              DisplayValue = this.MyBaseSettings.Input1Unit;
              num21 = this.MyBaseSettings.Input1PulsValue;
            }
            else
              continue;
          }
          else if (!(this.MyBaseSettings.Input2Unit == "?"))
          {
            DisplayValue = this.MyBaseSettings.Input2Unit;
            num21 = this.MyBaseSettings.Input2PulsValue;
          }
          else
            continue;
          int inputUnitIndex = MeterMath.GetInputUnitIndex(DisplayValue);
          if (inputUnitIndex < 0)
          {
            this.LastError = MeterMath.Errors.Input1UnitNotAvailable;
            return false;
          }
          MeterMath.InputFrameType frameType = MeterMath.InputUnits[inputUnitIndex].FrameType;
          int index2;
          int afterPointDigits;
          switch (frameType)
          {
            case MeterMath.InputFrameType.Empty:
              index2 = MeterMath.GetEmptyUnitIndex(DisplayValue);
              if (index2 >= 0)
              {
                afterPointDigits = (int) MeterMath.EmptyUnits[index2].AfterPointDigits;
                break;
              }
              goto default;
            case MeterMath.InputFrameType.Energy:
              index2 = MeterMath.GetEnergyUnitIndex(DisplayValue);
              if (index2 >= 0)
              {
                afterPointDigits = (int) MeterMath.EnergyUnits[index2].AfterPointDigits;
                break;
              }
              goto default;
            case MeterMath.InputFrameType.Volume:
              index2 = MeterMath.GetVolumeUnitIndex(DisplayValue);
              if (index2 >= 0)
              {
                afterPointDigits = (int) MeterMath.VolumeUnits[index2].AfterPointDigits;
                break;
              }
              goto default;
            default:
              if (index1 == 1)
                this.LastError = MeterMath.Errors.Input1UnitNotAvailable;
              else
                this.LastError = MeterMath.Errors.Input2UnitNotAvailable;
              return false;
          }
          if (afterPointDigits > 3)
          {
            if (index1 == 1)
              this.LastError = MeterMath.Errors.Input1ToManyDecimalPlaces;
            else
              this.LastError = MeterMath.Errors.Input2ToManyDecimalPlaces;
            return false;
          }
          double num22 = 64.0 * Math.Pow(10.0, (double) afterPointDigits);
          long num23 = (long) (num21 * num22);
          double num24 = (double) num23 / num22;
          double num25 = num21 * 0.01;
          if (num23 <= 0L || num23 > (long) ushort.MaxValue || num24 > num21 + num25 || num24 < num21 - num25)
          {
            if (index1 == 1)
              this.LastError = MeterMath.Errors.Input1ToOutOfRange;
            else
              this.LastError = MeterMath.Errors.Input2ToOutOfRange;
            return false;
          }
          int index3 = 7 - (afterPointDigits + 1);
          if (index1 == 1)
          {
            switch (frameType)
            {
              case MeterMath.InputFrameType.Empty:
                ArrayList frames8 = this.MyBaseSettings.Frames;
                frameNames = ZR_Constants.FrameNames.Input1Frame;
                MeterMath.FrameDescription frameDescription8 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEmptyFrames[index2].EmptyFrame);
                frames8.Add((object) frameDescription8);
                this.MyBaseSettings.MBusInput1VIF = MeterMath.LinearEmptyUnits[(int) MeterMath.EmptyUnits[index2].LinearUnitIndex].MBusEmptyVIF;
                break;
              case MeterMath.InputFrameType.Energy:
                ArrayList frames9 = this.MyBaseSettings.Frames;
                frameNames = ZR_Constants.FrameNames.Input1Frame;
                MeterMath.FrameDescription frameDescription9 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEnergyFrames[index2].EnergyFrame);
                frames9.Add((object) frameDescription9);
                this.MyBaseSettings.MBusInput1VIF = MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[index2].LinearUnitIndex].MBusEnergieVIF;
                break;
              case MeterMath.InputFrameType.Volume:
                ArrayList frames10 = this.MyBaseSettings.Frames;
                frameNames = ZR_Constants.FrameNames.Input1Frame;
                MeterMath.FrameDescription frameDescription10 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusVolumeFrames[index2].VolumeFrame);
                frames10.Add((object) frameDescription10);
                this.MyBaseSettings.MBusInput1VIF = MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[index2].LinearUnitIndex].MBusVolumeVIF;
                break;
              default:
                this.LastError = MeterMath.Errors.Input1UnitNotAvailable;
                return false;
            }
            this.MyBaseSettings.Input1UnitIndex = inputUnitIndex;
            this.NeadedMeterVars[(object) "R:Inp1Factor"] = (object) num23;
            ArrayList frames11 = this.MyBaseSettings.Frames;
            frameNames = ZR_Constants.FrameNames.Input1ImpValFrame;
            MeterMath.FrameDescription frameDescription11 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEmptyFrames[index3].EmptyFrame);
            frames11.Add((object) frameDescription11);
          }
          else
          {
            switch (frameType)
            {
              case MeterMath.InputFrameType.Empty:
                ArrayList frames12 = this.MyBaseSettings.Frames;
                frameNames = ZR_Constants.FrameNames.Input2Frame;
                MeterMath.FrameDescription frameDescription12 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEmptyFrames[index2].EmptyFrame);
                frames12.Add((object) frameDescription12);
                this.MyBaseSettings.MBusInput2VIF = MeterMath.LinearEmptyUnits[(int) MeterMath.EmptyUnits[index2].LinearUnitIndex].MBusEmptyVIF;
                break;
              case MeterMath.InputFrameType.Energy:
                ArrayList frames13 = this.MyBaseSettings.Frames;
                frameNames = ZR_Constants.FrameNames.Input2Frame;
                MeterMath.FrameDescription frameDescription13 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEnergyFrames[index2].EnergyFrame);
                frames13.Add((object) frameDescription13);
                this.MyBaseSettings.MBusInput2VIF = MeterMath.LinearEnergyUnits[(int) MeterMath.EnergyUnits[index2].LinearUnitIndex].MBusEnergieVIF;
                break;
              case MeterMath.InputFrameType.Volume:
                ArrayList frames14 = this.MyBaseSettings.Frames;
                frameNames = ZR_Constants.FrameNames.Input2Frame;
                MeterMath.FrameDescription frameDescription14 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusVolumeFrames[index2].VolumeFrame);
                frames14.Add((object) frameDescription14);
                this.MyBaseSettings.MBusInput2VIF = MeterMath.LinearVolumeUnits[(int) MeterMath.VolumeUnits[index2].LinearUnitIndex].MBusVolumeVIF;
                break;
              default:
                this.LastError = MeterMath.Errors.Input1UnitNotAvailable;
                return false;
            }
            this.MyBaseSettings.Input2UnitIndex = inputUnitIndex;
            this.NeadedMeterVars[(object) "R:Inp2Factor"] = (object) num23;
            ArrayList frames15 = this.MyBaseSettings.Frames;
            frameNames = ZR_Constants.FrameNames.Input2ImpValFrame;
            MeterMath.FrameDescription frameDescription15 = new MeterMath.FrameDescription(frameNames.ToString(), ZelsiusMath.ZelsiusEmptyFrames[index3].EmptyFrame);
            frames15.Add((object) frameDescription15);
          }
        }
      }
      catch (Exception ex)
      {
        this.LastError = MeterMath.Errors.MathematicError;
        this.LastErrorInfo = ex.ToString();
        return false;
      }
      return true;
    }

    private string[] GetImpulseValueFrame(double PulsValueInLiterPerImpuls)
    {
      string[] impulseValueFrame = new string[12]
      {
        "0",
        "-",
        "0",
        "-",
        "DII_FRAME_TEXT_SPACE DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_SPACE DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_SPACE DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_SPACE DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_SPACE DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_SPACE DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_SPACE DII_FRAME_TEXTFOLLOW",
        "DII_FRAME_TEXT_SPACE"
      };
      Decimal num1 = Decimal.Truncate((Decimal) PulsValueInLiterPerImpuls);
      Decimal num2 = (Decimal) PulsValueInLiterPerImpuls % 1M;
      if (num1 == 0M)
      {
        if (num2 == 0M)
          throw new ArgumentException("Impuls value == 0");
        impulseValueFrame[2] = "DII_FRAME_SEGS3_DEC7";
        impulseValueFrame[4] = "DII_FRAME_TEXT_0 DII_FRAME_TEXTFOLLOW";
        for (int index = 5; index < 12; ++index)
        {
          Decimal d = num2 * 10M;
          Decimal Digit = Math.Truncate(d);
          num2 = d % 1M;
          impulseValueFrame[index] = this.GetDigitText(Digit);
          if (index != 11)
          {
            // ISSUE: explicit reference operation
            ^ref impulseValueFrame[index] += " DII_FRAME_TEXTFOLLOW";
          }
        }
      }
      else if (num2 > 0M)
      {
        if (num1 > 9999M)
          throw new ArgumentException("Wrong impuls value");
        impulseValueFrame[2] = "DII_FRAME_SEGS3_DEC4";
        int index1 = 7;
        while (num1 > 0M)
        {
          Decimal Digit = num1 % 10M;
          num1 = Decimal.Truncate(num1 / 10M);
          impulseValueFrame[index1] = this.GetDigitText(Digit);
          // ISSUE: explicit reference operation
          ^ref impulseValueFrame[index1] += " DII_FRAME_TEXTFOLLOW";
          --index1;
        }
        for (int index2 = 8; index2 < 12; ++index2)
        {
          Decimal d = num2 * 10M;
          Decimal Digit = Decimal.Truncate(d);
          num2 = d % 1M;
          impulseValueFrame[index2] = this.GetDigitText(Digit);
          if (index2 != 11)
          {
            // ISSUE: explicit reference operation
            ^ref impulseValueFrame[index2] += " DII_FRAME_TEXTFOLLOW";
          }
        }
      }
      else
      {
        if (num1 > 99999999M)
          throw new ArgumentException("Wrong impuls value");
        int index = 11;
        while (num1 > 0M)
        {
          Decimal Digit = num1 % 10M;
          num1 = Decimal.Truncate(num1 / 10M);
          impulseValueFrame[index] = this.GetDigitText(Digit);
          if (index != 11)
          {
            // ISSUE: explicit reference operation
            ^ref impulseValueFrame[index] += " DII_FRAME_TEXTFOLLOW";
          }
          --index;
        }
      }
      return impulseValueFrame;
    }

    private string GetDigitText(Decimal Digit)
    {
      if (Digit == 0M)
        return "DII_FRAME_TEXT_0";
      if (Digit == 1M)
        return "DII_FRAME_TEXT_1";
      if (Digit == 2M)
        return "DII_FRAME_TEXT_2";
      if (Digit == 3M)
        return "DII_FRAME_TEXT_3";
      if (Digit == 4M)
        return "DII_FRAME_TEXT_4";
      if (Digit == 5M)
        return "DII_FRAME_TEXT_5";
      if (Digit == 6M)
        return "DII_FRAME_TEXT_6";
      if (Digit == 7M)
        return "DII_FRAME_TEXT_7";
      if (Digit == 8M)
        return "DII_FRAME_TEXT_8";
      if (Digit == 9M)
        return "DII_FRAME_TEXT_9";
      throw new ArgumentException("Wrong impuls value digit");
    }

    public override string GetUnitString(byte[] EEP_Data, int FrameOffset)
    {
      return ZelsiusInterpreter.GetUnitString(EEP_Data, FrameOffset);
    }

    public override bool GetDisplay(
      ByteField EEProm,
      uint EEPromSize,
      uint EEPromStartOffset,
      out bool[] Display)
    {
      if (this.MyZelsiusInterpreter == null)
        this.MyZelsiusInterpreter = new ZelsiusInterpreter(this);
      return this.MyZelsiusInterpreter.GetDisplay(EEProm, EEPromSize, EEPromStartOffset, out Display);
    }

    public override bool GetDisplay(
      byte[] EEProm,
      uint EEPromSize,
      uint EEPromStartOffset,
      out bool[] Display)
    {
      if (this.MyZelsiusInterpreter == null)
        this.MyZelsiusInterpreter = new ZelsiusInterpreter(this);
      return this.MyZelsiusInterpreter.GetDisplay(EEProm, EEPromSize, EEPromStartOffset, out Display);
    }

    public event ZelsiusMath.CpuRead GetCpuData;

    internal bool GetCpuDataWork(
      string NameOrAddress,
      int NumberOfBytes,
      MemoryLocation Location,
      out byte[] Data)
    {
      Data = (byte[]) null;
      return this.GetCpuData != null && this.GetCpuData(NameOrAddress, NumberOfBytes, Location, out Data);
    }

    internal struct BC_Settings
    {
      internal byte Energie_Konfiguration;
      internal byte RW_Typ_Konfiguration;

      internal BC_Settings(byte Energie_KonfigurationIn, byte RW_Typ_KonfigurationIn)
      {
        this.Energie_Konfiguration = Energie_KonfigurationIn;
        this.RW_Typ_Konfiguration = RW_Typ_KonfigurationIn;
      }
    }

    public delegate bool CpuRead(
      string NameOrAddress,
      int NumberOfBytes,
      MemoryLocation Location,
      out byte[] Data);
  }
}
