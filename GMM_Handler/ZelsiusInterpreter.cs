// Decompiled with JetBrains decompiler
// Type: GMM_Handler.ZelsiusInterpreter
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Diagnostics;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class ZelsiusInterpreter
  {
    private ZelsiusMath MyZelsiusMath;
    private uint TheEepAdr;
    private byte[] TheRam = new byte[512];
    private byte[] TheEEProm;
    private uint TheEEPromSize;
    private bool[] TheDisplay;
    private int Dis_Flags = 0;
    private uint Dis_Address = 0;
    private ushort Sta_Status = 0;
    private ushort Mai_Events2 = 0;
    private ushort Itr_Prozessabbild = 0;
    private const byte DII_FRAME_SEGS1 = 1;
    private const byte DII_FRAME_SEGS2 = 2;
    private const byte DII_FRAME_SEGS3 = 4;
    private const byte DII_FRAME_SEGS4 = 8;
    private const byte DII_FRAME_MENUPOS = 16;
    private const byte DII_FRAME_BLINK = 32;
    private const byte DII_FRAME_TEXT = 128;
    private const byte DII_FRAME_SEGS1_DEC1 = 1;
    private const byte DII_FRAME_SEGS1_DEC2 = 2;
    private const byte DII_FRAME_SEGS1_DEC3 = 4;
    private const byte DII_FRAME_SEGS1_FRAME1 = 8;
    private const byte DII_FRAME_SEGS1_FRAME2 = 16;
    private const byte DII_FRAME_SEGS1_FRAME3 = 32;
    private const byte DII_FRAME_SEGS1_VL_TEMP = 64;
    private const byte DII_FRAME_SEGS1_RL_TEMP = 128;
    private const byte DII_FRAME_SEGS2_CUBIC_M = 1;
    private const byte DII_FRAME_SEGS2_LITRE = 2;
    private const byte DII_FRAME_SEGS2_SLASH = 4;
    private const byte DII_FRAME_SEGS2_HOUR = 8;
    private const byte DII_FRAME_SEGS2_KILO = 16;
    private const byte DII_FRAME_SEGS2_WATT = 32;
    private const byte DII_FRAME_SEGS2_MEGA = 64;
    private const byte DII_FRAME_SEGS2_MEGA_JOULE = 128;
    private const byte DII_FRAME_SEGS3_DEC4 = 1;
    private const byte DII_FRAME_SEGS3_DEC7 = 2;
    private const byte DII_FRAME_SEGS3_COLON = 4;
    private const byte DII_FRAME_SEGS3_VL_FLOW = 8;
    private const byte DII_FRAME_SEGS3_D0_SEGB = 16;
    private const byte DII_FRAME_SEGS3_D0_SEGC = 32;
    private const byte DII_FRAME_SEGS3_DOOR = 64;
    private const byte DII_FRAME_SEGS3_WARNING = 128;
    private const byte DII_FRAME_SEGS4_COLD = 1;
    private const byte DII_FRAME_SEGS4_CAL = 2;
    private const byte DII_FRAME_SEGS4_MAX_VAL = 4;
    private const byte DII_FRAME_SEGS4_RADIO = 8;
    private const byte DII_FRAME_SEGS4_TOWER = 16;
    private const byte DII_FRAME_SEGS4_Special = 224;
    private const byte DII_FRAME_SEGS4frei_20 = 32;
    private const byte DII_FRAME_SEGS4frei_40 = 64;
    private const byte DII_FRAME_SEGS4_Status_On = 128;
    private const byte DII_FRAME_TEXTFOLLOW = 16;
    private static byte[] LCDZ1_FrameByteList = new byte[32]
    {
      (byte) 16,
      (byte) 16,
      (byte) 16,
      (byte) 16,
      (byte) 2,
      (byte) 1,
      (byte) 16,
      (byte) 128,
      (byte) 8,
      (byte) 128,
      (byte) 64,
      (byte) 16,
      (byte) 2,
      (byte) 4,
      (byte) 1,
      (byte) 32,
      (byte) 16,
      (byte) 16,
      (byte) 16,
      (byte) 16,
      (byte) 32,
      (byte) 64,
      (byte) 32,
      (byte) 8,
      (byte) 4,
      (byte) 4,
      (byte) 64,
      (byte) 16,
      (byte) 128,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private const int DESIGNER_LCD_MENU2_BIT = 0;
    private const int DESIGNER_LCD_MENU3_BIT = 1;
    private const int DESIGNER_LCD_MENU_ZEILE = 2;
    private const int DESIGNER_LCD_MONEY_BIT = 3;
    private const int DESIGNER_LCD_TAP_BIT = 4;
    private const int DESIGNER_LCD_WARNING_BIT = 5;
    private const int DESIGNER_LCD_BATTERY_BIT = 6;
    private const int DESIGNER_LCD_DOSE_BIT = 7;
    private const int DESIGNER_LCD_LETTER_BIT = 8;
    private const int DESIGNER_LCD_TARIF_BIT = 9;
    private const int DESIGNER_LCD_MONTH_BIT = 10;
    private const int DESIGNER_LCD_DAY_BIT = 11;
    private const int DESIGNER_LCD_SINUS_BIT = 12;
    private const int DESIGNER_LCD_UP_BIT = 13;
    private const int DESIGNER_LCD_DOWN_BIT = 14;
    private const int DESIGNER_LCD_LEFT_BIT = 15;
    private const int DESIGNER_LCD_RIGHT_BIT = 16;
    private const int DESIGNER_LCD_STORAGE_BIT = 17;
    private const int DESIGNER_LCD_WARM_BIT = 18;
    private const int DESIGNER_LCD_DOOR_BIT = 19;
    private const int DESIGNER_LCD_SLASH_BIT = 20;
    private const int DESIGNER_LCD_LITRE_BIT = 21;
    private const int DESIGNER_LCD_HOUR_BIT = 22;
    private const int DESIGNER_LCD_CUBIC_M_BIT = 23;
    private const int DESIGNER_LCD_DEC4_K_BIT = 24;
    private const int DESIGNER_LCD_DEC4_BIT = 26;
    private const int DESIGNER_LCD_DEC3_BIT = 27;
    private const int DESIGNER_LCD_DEC2_BIT = 28;
    private const int DESIGNER_LCD_DEC1_BIT = 29;
    private const int DESIGNER_LCD_COLON_BIT = 30;
    private const int DESIGNER_LCD_DEC7_BIT = 31;
    private const int DESIGNER_LCD_FRAME1_BIT = 32;
    private const int DESIGNER_LCD_FRAME2_BIT = 33;
    private const int DESIGNER_LCD_FRAME3_BIT = 34;
    private const int DESIGNER_LCD_VL_TEMP_BIT = 35;
    private const int DESIGNER_LCD_RL_TEMP_BIT = 36;
    private const int DESIGNER_LCD_KILO_BIT = 37;
    private const int DESIGNER_LCD_WATT_BIT = 38;
    private const int DESIGNER_LCD_MEGA_BIT = 39;
    private const int DESIGNER_LCD_MEGA_JOULE_BIT = 40;
    private const int DESIGNER_LCD_VL_FLOW_BIT = 41;
    private const int DESIGNER_LCD_D0_SEGC_BIT = 42;
    private const int DESIGNER_LCD_D0_SEGB_BIT = 43;
    private const int DESIGNER_LCD_COLD_BIT = 44;
    private const int DESIGNER_LCD_CAL_BIT = 45;
    private const int DESIGNER_LCD_MAX_VAL_BIT = 46;
    private const int DESIGNER_LCD_RADIO_BIT = 47;
    private const int DESIGNER_LCD_TOWER_BIT = 48;
    private const int DESIGNER_LCD_ROWSEG_DP = 49;
    private const int DESIGNER_LCD_ROWSEG_A = 50;
    private const int DESIGNER_LCD_ROWSEG_B = 51;
    private const int DESIGNER_LCD_ROWSEG_C = 52;
    private const int DESIGNER_LCD_ROWSEG_D = 53;
    private const int DESIGNER_LCD_ROWSEG_E = 54;
    private const int DESIGNER_LCD_ROWSEG_F = 55;
    private const int DESIGNER_LCD_ROWSEG_G = 56;
    private const int DESIGNER_LCD_MENU1_BIT = 57;
    private const int DESIGNER_LCD_TEXT_BIT_ADR = 60;
    private const int DESIGNER_LCD_DIGIT_MASK_S1 = 254;
    private const int DESIGNER_LCD_DIGIT_MASK_Z1 = 239;
    private const int DESIGNER_LCD_SEG_A = 1;
    private const int DESIGNER_LCD_SEG_B = 2;
    private const int DESIGNER_LCD_SEG_C = 3;
    private const int DESIGNER_LCD_SEG_D = 4;
    private const int DESIGNER_LCD_SEG_E = 5;
    private const int DESIGNER_LCD_SEG_F = 6;
    private const int DESIGNER_LCD_SEG_G = 7;
    private const int DESIGNER_LCD_SEG_DP = 0;
    private ZelsiusInterpreter.DisplayFunction[] Itr_DispFunctionTab;
    private ZelsiusInterpreter.RuntimeFunction[] Itr_RunFunctionTab;
    private const byte DII_BY_CLICK_FOLLOW = 8;
    private const byte DII_BY_PRESS_FOLLOW = 4;
    private const byte DII_BY_HOLD_FOLLOW = 2;
    private const byte DII_BY_TIMEOUT_FOLLOW = 1;
    private const byte DII_BY_CLICK_JUMP = 128;
    private const byte DII_BY_PRESS_JUMP = 64;
    private const byte DII_BY_HOLD_JUMP = 32;
    private const byte DII_BY_TIMEOUT_JUMP = 16;
    private const byte DII_BY_CLICK_NONE = 0;
    private const byte DII_BY_PRESS_NONE = 0;
    private const byte DII_BY_HOLD_NONE = 0;
    private const byte DII_BY_TIMEOUT_NONE = 0;
    private const ushort ITR_EDIT_ACTIVE = 256;
    private const ushort ITR_DISPLAY_DOOR = 512;
    private const ushort ITR_DISPLAY_BATTERY = 1024;
    private const ushort ITR_EEPROM_RUNTIME = 2048;
    private const ushort ITR_EEPROM_TEMP_RUNTIME = 4096;
    private const ushort ITR_RUNTIME_STOP = 8192;
    private const ushort ITR_DISPLAY_DIRECT = 16384;
    private const ushort ITR_DOOR_ON = 32768;
    private const ushort ITR_JUMP_MASK = 255;
    private const ushort ITR_FUNC_RESET_SAMPLE = 0;
    private const bool ITR_CODE_FROM_EEPROM = true;
    private const bool ITR_CODE_FROM_RAM = false;
    private const byte RUI_CODE_Set = 129;
    private const byte RUI_CODE_Clear = 1;
    private const byte RUI_CODE_SkipT = 130;
    private const byte RUI_CODE_SkipF = 2;
    private const byte RUI_PAB_OUT1 = 0;
    private const byte RUI_PAB_OUT2 = 8;
    private const byte RUI_PAB_IN1 = 16;
    private const byte RUI_PAB_IN2 = 24;
    private const byte RUI_PAB_Flow = 32;
    private const byte RUI_PAB_Carry = 40;
    private const byte RUI_PAB_Zero = 48;
    private const byte RUI_PAB_INDIRECT = 56;
    private const byte RUI_PAB_IR_PER_KEY = 64;
    private const byte RUI_PAB_IR_ON = 72;
    private const byte RUI_PAB_2400BAUD = 80;
    private const byte RUI_PAB_Flag0 = 88;
    private const byte RUI_PAB_Flag1 = 96;
    private const byte RUI_PAB_Flag2 = 104;
    private const byte RUI_PAB_Flag3 = 112;
    private const byte RUI_PAB_Unlocked = 120;
    private const ushort ITR_PAB_OUT1 = 1;
    private const ushort ITR_PAB_OUT2 = 2;
    private const ushort ITR_PAB_IN1 = 4;
    private const ushort ITR_PAB_IN2 = 8;
    private const ushort ITR_PAB_FLOW = 16;
    private const ushort ITR_PAB_CARRY = 32;
    private const ushort ITR_PAB_ZERO = 64;
    private const ushort ITR_PAB_INDIRECT = 128;
    private const ushort ITR_PAB_IR_PER_KEY = 256;
    private const ushort ITR_PAB_IR_ON = 512;
    private const ushort ITR_PAB_2400BAUD = 1024;
    private const ushort ITR_PAB_BACK = 0;
    private const ushort ITR_PAB_RESET_CLR_MASK = 63740;
    private const ushort ITR_PAB_RESET_SET_MASK = 3;
    private const ushort ITR_PAB_RESET_BUS_START = 32768;
    private const byte RUI_CODE_SkipGE = 42;
    private const byte RUI_CODE_SkipLT = 170;
    private const byte RUI_CODE_SkipEQ = 178;
    private const byte RUI_CODE_SkipNE = 50;
    private const byte RUI_CODE_Load = 131;
    private const byte RUI_CODE_Store = 3;
    private const byte RUI_CODE_Add = 132;
    private const byte RUI_CODE_Sub = 4;
    private const byte RUI_CODE_Mul = 133;
    private const byte RUI_CODE_Div = 5;
    private const byte RUI_VAR_EEPROM = 64;
    private const byte RUI_VAR_1BYTE = 0;
    private const byte RUI_VAR_2BYTE = 8;
    private const byte RUI_VAR_4BYTE = 24;
    private const byte RUI_KONST_1BYTE = 32;
    private const byte RUI_KONST_2BYTE = 40;
    private const byte RUI_KONST_4BYTE = 56;
    private const byte RUI_VAR_UINT32_1 = 96;
    private const byte RUI_VAR_UINT32_2 = 104;
    private const byte RUI_VAR_UINT32_3 = 112;
    private const byte RUI_VAR_UINT32_4 = 120;
    private const byte RUI_VAR_Volume = 16;
    private const byte RUI_VAR_AkkuX = 48;
    private const byte RUI_VAR_Flow = 80;
    private const byte RUI_CODE_And = 192;
    private const byte RUI_CODE_Or = 200;
    private const byte RUI_CODE_Xor = 208;
    private const byte RUI_CODE_Mod = 216;
    private const byte RUI_CODE_Mul64 = 224;
    private const byte RUI_CODE_Div64 = 232;
    private const byte RUI_CODE_Compare = 240;
    private const byte RUI_CODE_Load_Volume = 147;
    private const byte RUI_CODE_Load_AkkuX = 179;
    private const byte RUI_CODE_Load_PulsTime = 211;
    private const byte RUI_CODE_Load_HighResVol = 19;
    private const byte RUI_CODE_Load_SysTime = 51;
    private const byte RUI_CODE_Load_Energie = 83;
    private const byte RUI_CODE_LOGGER_STORE_1BYTE = 35;
    private const byte RUI_CODE_LOGGER_STORE_2BYTE = 43;
    private const byte RUI_CODE_LOGGER_STORE_4BYTE = 59;
    private const byte RUI_CODE_End = 0;
    private const byte RUI_CODE_IntervalNew = 8;
    private const byte RUI_CODE_DecDiff = 16;
    private const byte RUI_CODE_SetClick = 24;
    private const byte RUI_CODE_SetPress = 32;
    private const byte RUI_CODE_SetHold = 40;
    private const byte RUI_CODE_Load0 = 48;
    private const byte RUI_CODE_NextLoggerEntry = 56;
    private const byte RUI_CODE_Xch = 64;
    private const byte RUI_CODE_SetDisplay = 72;
    private const byte RUI_CODE_Backup = 80;
    private const byte RUI_CODE_Store_CycleTime = 88;
    private const byte RUI_CODE_Store_TimeoutTime = 96;
    private const byte RUI_CODE_ExtractMonth = 104;
    private const byte RUI_CODE_ExtractYear = 112;
    private const byte RUI_CODE_EepRuntime = 120;
    private const byte RUI_CODE_ChangeMonth = 128;
    private const byte RUI_CODE_SetToNextYear = 136;
    private const byte RUI_CODE_Copy = 144;
    private const byte RUI_CODE_Past = 152;
    private const byte RUI_CODE_Load_ColdEnergie = 160;
    private const byte RUI_CODE_ManageOpto = 168;
    private const byte RUI_CODE_Jump = 6;
    private const byte RUI_CODE_Interval = 7;
    private const byte RUI_CODE_Logger = 135;
    private const byte RUI_CODE_IntervalTest = 127;
    private const byte RUI_TIME_Week = 0;
    private const byte RUI_TIME_Day = 8;
    private const byte RUI_TIME_Hours12 = 16;
    private const byte RUI_TIME_Hours6 = 24;
    private const byte RUI_TIME_Hours2 = 32;
    private const byte RUI_TIME_Hour = 40;
    private const byte RUI_TIME_Minutes30 = 48;
    private const byte RUI_TIME_Minutes15 = 56;
    private const byte RUI_TIME_Minutes10 = 64;
    private const byte RUI_TIME_Minutes5 = 72;
    private const byte RUI_TIME_Year = 80;
    private const byte RUI_TIME_Month6 = 88;
    private const byte RUI_TIME_Month3 = 96;
    private const byte RUI_TIME_Month = 104;
    private const byte RUI_TIME_Extern = 112;
    private const byte LOG_FLAGS_OVERRUN = 128;
    private const byte LOG_FLAGS_EVENT_SIZE = 127;
    private const byte MAI_EVENT2_BACKUP = 1;
    private const byte MAI_EVENT2_ADC_MEASUREMENT = 2;
    private const byte MAI_EVENT2_SOFTWARE_RESET = 4;
    private const byte MAI_EVENT2_500MS_TASK = 8;
    private const ushort STA_GS_ADC_ERROR_MASK = 7;
    private const ushort STA_GS_ERR_FLAGS_MASK = 65528;
    private const ushort STA_GS_ERR_RESET_MASK = 65535;
    private const ushort STA_GS_INTERPRETER_ERROR = 256;
    private const ushort STA_GS_ERROR_WRITE_PERMISSION = 512;
    private const ushort STA_GS_BATTERY_END = 4096;
    private const ushort STA_GS_DATE_END = 8192;
    private const ushort STA_GS_SPECIAL_STATE = 16384;
    private bool Itr_SkipRequest;
    private ushort Itr_State;
    private ushort Itr_BitMask;
    private uint Itr_Akku;
    private uint Itr_Akku2;
    private uint Itr_AkkuX;
    private byte Itr_Code;
    private byte Itr_LoopCounter;
    private static byte[] SkipBytes = new byte[16]
    {
      (byte) 2,
      (byte) 2,
      (byte) 0,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 0,
      (byte) 4,
      (byte) 2,
      (byte) 2,
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private const int DII_VARIABLE_FLAG_BYTES = 768;
    private const int DII_VARIABLE_1_BYTE = 0;
    private const int DII_VARIABLE_2_BYTE = 256;
    private const int DII_VARIABLE_4_BYTE = 768;
    private const int DII_VARIABLE_FLAG_RUNTIME = 512;
    private const int DII_VARIABLE_FLAG_REFRESH = 1024;
    private const int DII_VARIABLE_FLAG_EEPROM = 2048;
    private const int DII_VARIABLE_FLAG_ITR_NEXT = 4096;
    private const int DII_VARIABLE_FLAG_DIS_INT = 8192;
    private const int DII_VARIABLE_FLAG_TIME_FORM = 16384;
    private const int DII_VARIABLE_FLAG_INDIRECT = 32768;
    private const int DII_VARIABLE_FLAG_DIGITS = 7;
    private const int DII_VARIABLE_1_DIGIT = 0;
    private const int DII_VARIABLE_2_DIGIT = 1;
    private const int DII_VARIABLE_3_DIGIT = 2;
    private const int DII_VARIABLE_4_DIGIT = 3;
    private const int DII_VARIABLE_5_DIGIT = 4;
    private const int DII_VARIABLE_6_DIGIT = 5;
    private const int DII_VARIABLE_7_DIGIT = 6;
    private const int DII_VARIABLE_8_DIGIT = 7;
    private const int DII_VARIABLE_POSSITION_MASK = 56;
    private const int DII_VARIABLE_POSSITION_0 = 0;
    private const int DII_VARIABLE_POSSITION_1 = 8;
    private const int DII_VARIABLE_POSSITION_2 = 16;
    private const int DII_VARIABLE_POSSITION_3 = 24;
    private const int DII_VARIABLE_POSSITION_4 = 32;
    private const int DII_VARIABLE_POSSITION_5 = 40;
    private const int DII_VARIABLE_POSSITION_6 = 48;
    private const int DII_VARIABLE_POSSITION_7 = 56;
    private const int DII_VARIABLE_FLAG_FILL_0 = 64;
    private const int DII_VARIABLE_FLAG_HEX = 128;
    private const int DII_VARIABLE_FLAG_DATE = 0;
    private const int DII_VARIABLE_FLAG_TIME = 16;
    private const int DII_VARIABLE_FLAG_TEMP = 64;
    private const int DII_VARIABLE_FLAG_ST_MONTH = 32;
    private const int DII_VARIABLE_FLAG_DAY_MONTH = 64;
    private const int DII_VARIABLE_SF_TIME = 16403;
    private const int DII_VARIABLE_SF_TEMP = 16464;
    private const int DII_VARIABLE_SF_DATE = 16389;
    private const int DII_VARIABLE_SF_ST_MONTH = 16417;
    private const int DII_VARIABLE_SF_DAY_MONTH = 16451;
    private const int DII_VARIABLE_FLAG_MASK = 1024;
    private const int DII_VARIABLE_FLAG_MIN_MAX = 4096;
    private const int DII_VARIABLE_FLAG_FUTURE = 128;
    private const int SLCD_DIGIT_BASE_ADR = 153;
    private const int SLCD_RAM_NIBBLE_ADR = 157;
    private const int SLCD_BLINK_MASK_ADR = 158;
    private const int SLCD_BLINK_OFFSET_ADR = 159;
    private const int SLCD_BLINK_STATE_ADR = 160;
    private const byte SLCD_DIGIT_MASK = 239;
    private const byte SLCD_RAM_CLEAR = 16;
    private const int SLCD_DEC1_ADR = 152;
    private const int SLCD_DEC2_ADR = 151;
    private const int SLCD_DEC3_ADR = 150;
    private const int SLCD_DEC4_ADR = 149;
    private const int SLCD_DEC7_ADR = 146;
    private const int SLCD_FRAME1_ADR = 153;
    private const int SLCD_FRAME2_ADR = 155;
    private const int SLCD_FRAME3_ADR = 155;
    private const int SLCD_COLON_ADR = 148;
    private const int SLCD_MENU1_ADR = 145;
    private const int SLCD_MENU2_ADR = 145;
    private const int SLCD_MENU3_ADR = 145;
    private const int SLCD_DOOR_ADR = 156;
    private const int SLCD_WARNING_ADR = 155;
    private const int SLCD_COLD_ADR = 145;
    private const int SLCD_CAL_ADR = 155;
    private const int SLCD_MAX_VAL_ADR = 155;
    private const int SLCD_RADIO_ADR = 155;
    private const int SLCD_TOWER_ADR = 155;
    private const int SLCD_VL_FLOW_ADR = 147;
    private const int SLCD_VL_TEMP_ADR = 145;
    private const int SLCD_RL_TEMP_ADR = 145;
    private const int SLCD_CUBIC_M_ADR = 156;
    private const int SLCD_LITRE_ADR = 156;
    private const int SLCD_SLASH_ADR = 156;
    private const int SLCD_HOUR_ADR = 156;
    private const int SLCD_KILO_ADR = 156;
    private const int SLCD_WATT_ADR = 156;
    private const int SLCD_MEGA_ADR = 156;
    private const int SLCD_MEGA_JOULE_ADR = 155;
    private const int SLCD_D0_SEGB_ADR = 145;
    private const int SLCD_D0_SEGC_ADR = 145;
    private const int SLCD_DEC1_BIT = 16;
    private const int SLCD_DEC2_BIT = 16;
    private const int SLCD_DEC3_BIT = 16;
    private const int SLCD_DEC4_BIT = 16;
    private const int SLCD_DEC7_BIT = 16;
    private const int SLCD_FRAME1_BIT = 16;
    private const int SLCD_FRAME2_BIT = 2;
    private const int SLCD_FRAME3_BIT = 1;
    private const int SLCD_COLON_BIT = 16;
    private const int SLCD_MENU1_BIT = 8;
    private const int SLCD_MENU2_BIT = 2;
    private const int SLCD_MENU3_BIT = 1;
    private const int SLCD_DOOR_BIT = 32;
    private const int SLCD_WARNING_BIT = 8;
    private const int SLCD_COLD_BIT = 4;
    private const int SLCD_CAL_BIT = 4;
    private const int SLCD_MAX_VAL_BIT = 64;
    private const int SLCD_RADIO_BIT = 16;
    private const int SLCD_TOWER_BIT = 128;
    private const int SLCD_VL_FLOW_BIT = 16;
    private const int SLCD_RL_TEMP_BIT = 128;
    private const int SLCD_VL_TEMP_BIT = 16;
    private const int SLCD_CUBIC_M_BIT = 8;
    private const int SLCD_LITRE_BIT = 128;
    private const int SLCD_SLASH_BIT = 64;
    private const int SLCD_HOUR_BIT = 16;
    private const int SLCD_KILO_BIT = 2;
    private const int SLCD_WATT_BIT = 4;
    private const int SLCD_MEGA_BIT = 1;
    private const int SLCD_MEGA_JOULE_BIT = 32;
    private const int SLCD_D0_SEGB_BIT = 32;
    private const int SLCD_D0_SEGC_BIT = 64;
    private const int SLCD_DIGIT_SEG_A_Bit = 128;
    private const int SLCD_DIGIT_SEG_B_Bit = 64;
    private const int SLCD_DIGIT_SEG_C_Bit = 32;
    private const int SLCD_DIGIT_SEG_D_Bit = 1;
    private const int SLCD_DIGIT_SEG_E_Bit = 2;
    private const int SLCD_DIGIT_SEG_F_Bit = 8;
    private const int SLCD_DIGIT_SEG_G_Bit = 4;
    private const int LCD_FORMAT_DIGITS = 7;
    private const int LCD_FORMAT_DIGIT_NR = 56;
    private const int LCD_FORMAT_FILL_BIT = 64;
    private const int LCD_FORMAT_HEX = 128;
    private byte[] Lcd_CodeList = new byte[16]
    {
      (byte) 235,
      (byte) 96,
      (byte) 199,
      (byte) 229,
      (byte) 108,
      (byte) 173,
      (byte) 175,
      (byte) 224,
      (byte) 239,
      (byte) 237,
      (byte) 238,
      (byte) 47,
      (byte) 139,
      (byte) 103,
      (byte) 143,
      (byte) 142
    };
    private byte[] Lcd_DecimalList = new byte[10]
    {
      (byte) 152,
      (byte) 151,
      (byte) 150,
      (byte) 149,
      (byte) 146,
      (byte) 16,
      (byte) 16,
      (byte) 16,
      (byte) 16,
      (byte) 16
    };
    private byte[] LCD_FrameByteList = new byte[58]
    {
      (byte) 152,
      (byte) 16,
      (byte) 151,
      (byte) 16,
      (byte) 150,
      (byte) 16,
      (byte) 153,
      (byte) 16,
      (byte) 155,
      (byte) 2,
      (byte) 155,
      (byte) 1,
      (byte) 145,
      (byte) 16,
      (byte) 145,
      (byte) 128,
      (byte) 156,
      (byte) 8,
      (byte) 156,
      (byte) 128,
      (byte) 156,
      (byte) 64,
      (byte) 156,
      (byte) 16,
      (byte) 156,
      (byte) 2,
      (byte) 156,
      (byte) 4,
      (byte) 156,
      (byte) 1,
      (byte) 155,
      (byte) 32,
      (byte) 149,
      (byte) 16,
      (byte) 146,
      (byte) 16,
      (byte) 148,
      (byte) 16,
      (byte) 147,
      (byte) 16,
      (byte) 145,
      (byte) 32,
      (byte) 145,
      (byte) 64,
      (byte) 156,
      (byte) 32,
      (byte) 155,
      (byte) 8,
      (byte) 145,
      (byte) 4,
      (byte) 155,
      (byte) 4,
      (byte) 155,
      (byte) 64,
      (byte) 155,
      (byte) 16,
      (byte) 155,
      (byte) 128
    };
    private byte[] Lcd_MenueColumnList = new byte[7]
    {
      (byte) 8,
      (byte) 2,
      (byte) 1,
      (byte) 10,
      (byte) 9,
      (byte) 3,
      (byte) 11
    };

    public ZelsiusInterpreter(ZelsiusMath ZelsiusMath)
    {
      this.MyZelsiusMath = ZelsiusMath;
      this.Itr_DispFunctionTab = new ZelsiusInterpreter.DisplayFunction[7]
      {
        new ZelsiusInterpreter.DisplayFunction(this.ITR_Nop),
        new ZelsiusInterpreter.DisplayFunction(this.Itr_JumpAbsolut),
        new ZelsiusInterpreter.DisplayFunction(this.Dis_Edit),
        new ZelsiusInterpreter.DisplayFunction(this.Itr_RuntimeBlock),
        new ZelsiusInterpreter.DisplayFunction(this.Dis_Constant),
        new ZelsiusInterpreter.DisplayFunction(this.Dis_Variable),
        new ZelsiusInterpreter.DisplayFunction(this.Itr_CalcJump)
      };
      this.Itr_RunFunctionTab = new ZelsiusInterpreter.RuntimeFunction[8]
      {
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_SpecialFunction),
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_SetClear),
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_SkipT_SkipF),
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_LoadStore),
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_AddSub),
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_MulDiv),
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_Jump),
        new ZelsiusInterpreter.RuntimeFunction(this.Itr_Interval)
      };
    }

    internal static string GetUnitString(byte[] EEP_Data, int FrameOffset)
    {
      byte[] numArray1 = new byte[5];
      int num1 = FrameOffset + 1;
      if (((uint) EEP_Data[FrameOffset] & 1U) > 0U)
        numArray1[1] = EEP_Data[num1++];
      if (((uint) EEP_Data[FrameOffset] & 2U) > 0U)
        numArray1[2] = EEP_Data[num1++];
      if (((uint) EEP_Data[FrameOffset] & 4U) > 0U)
        numArray1[3] = EEP_Data[num1++];
      if (((uint) EEP_Data[FrameOffset] & 8U) > 0U)
      {
        byte[] numArray2 = numArray1;
        byte[] numArray3 = EEP_Data;
        int index = num1;
        int num2 = index + 1;
        int num3 = (int) numArray3[index];
        numArray2[4] = (byte) num3;
      }
      string str = ((uint) numArray1[2] & 128U) <= 0U ? (((uint) numArray1[2] & 1U) <= 0U ? (((uint) numArray1[2] & 2U) <= 0U ? (((uint) numArray1[2] & 32U) <= 0U ? "" : "W") : "L") : "m\u00B3") : "GJ";
      if (((uint) numArray1[2] & 16U) > 0U)
        str = "k" + str;
      else if (((uint) numArray1[2] & 64U) > 0U)
        str = "M" + str;
      if (((int) numArray1[2] & 4) != 0 && ((uint) numArray1[2] & 8U) > 0U)
        str += "/h";
      else if (((uint) numArray1[2] & 8U) > 0U)
        str += "h";
      return ((uint) numArray1[1] & 1U) <= 0U ? (((uint) numArray1[1] & 2U) <= 0U ? (((uint) numArray1[1] & 4U) <= 0U ? (((uint) numArray1[3] & 1U) <= 0U ? "0" + str : "0.0000" + str) : "0.000" + str) : "0.00" + str) : "0.0" + str;
    }

    internal bool GetDisplay(
      ByteField EEProm,
      uint EEPromSize,
      uint EEPromStartOffset,
      out bool[] Display)
    {
      this.TheEEProm = EEProm.Data;
      this.TheEEPromSize = EEPromSize;
      this.TheEepAdr = EEPromStartOffset;
      return this.GetDisplay(out Display);
    }

    internal bool GetDisplay(
      byte[] EEProm,
      uint EEPromSize,
      uint EEPromStartOffset,
      out bool[] Display)
    {
      this.TheEEProm = EEProm;
      this.TheEEPromSize = EEPromSize;
      this.TheEepAdr = EEPromStartOffset;
      return this.GetDisplay(out Display);
    }

    internal bool GetDisplay(out bool[] Display)
    {
      this.Itr_Prozessabbild = (ushort) 0;
      byte[] Data;
      if (this.MyZelsiusMath.GetCpuDataWork("DefaultFunction.Itr_Prozessabbild", 2, MemoryLocation.RAM, out Data) && Data != null && Data.Length == 2)
        this.Itr_Prozessabbild = (ushort) ((uint) Data[0] + ((uint) Data[1] << 8));
      int num1 = 0;
      Display = new bool[200];
      this.TheDisplay = Display;
      try
      {
        byte index1;
        do
        {
          index1 = (byte) this.LoadEEPromBYTE((int) this.TheEepAdr);
          if ((int) index1 >= this.Itr_DispFunctionTab.Length)
            return false;
          ++this.TheEepAdr;
          ++num1;
          if (num1 > 20)
            return false;
        }
        while (this.Itr_DispFunctionTab[(int) index1]());
        Display[57] = ((uint) this.TheRam[145] & 8U) > 0U;
        Display[0] = ((uint) this.TheRam[145] & 2U) > 0U;
        Display[1] = ((uint) this.TheRam[145] & 1U) > 0U;
        Display[5] = ((uint) this.TheRam[155] & 8U) > 0U;
        Display[19] = ((uint) this.TheRam[156] & 32U) > 0U;
        Display[20] = ((uint) this.TheRam[156] & 64U) > 0U;
        Display[21] = ((uint) this.TheRam[156] & 128U) > 0U;
        Display[22] = ((uint) this.TheRam[156] & 16U) > 0U;
        Display[23] = ((uint) this.TheRam[156] & 8U) > 0U;
        Display[26] = ((uint) this.TheRam[149] & 16U) > 0U;
        Display[27] = ((uint) this.TheRam[150] & 16U) > 0U;
        Display[28] = ((uint) this.TheRam[151] & 16U) > 0U;
        Display[29] = ((uint) this.TheRam[152] & 16U) > 0U;
        Display[30] = ((uint) this.TheRam[148] & 16U) > 0U;
        Display[35] = ((uint) this.TheRam[145] & 16U) > 0U;
        Display[36] = ((uint) this.TheRam[145] & 128U) > 0U;
        Display[37] = ((uint) this.TheRam[156] & 2U) > 0U;
        Display[38] = ((uint) this.TheRam[156] & 4U) > 0U;
        Display[39] = ((uint) this.TheRam[156] & 1U) > 0U;
        Display[40] = ((uint) this.TheRam[155] & 32U) > 0U;
        Display[32] = ((uint) this.TheRam[153] & 16U) > 0U;
        Display[33] = ((uint) this.TheRam[155] & 2U) > 0U;
        Display[34] = ((uint) this.TheRam[155] & 1U) > 0U;
        Display[44] = ((uint) this.TheRam[145] & 4U) > 0U;
        Display[45] = ((uint) this.TheRam[155] & 4U) > 0U;
        Display[46] = ((uint) this.TheRam[155] & 64U) > 0U;
        Display[47] = ((uint) this.TheRam[155] & 16U) > 0U;
        Display[48] = ((uint) this.TheRam[155] & 128U) > 0U;
        Display[41] = ((uint) this.TheRam[147] & 16U) > 0U;
        Display[43] = ((uint) this.TheRam[145] & 32U) > 0U;
        Display[42] = ((uint) this.TheRam[145] & 64U) > 0U;
        Display[31] = ((uint) this.TheRam[146] & 16U) > 0U;
        int index2 = 146;
        int num2 = 60;
        for (int index3 = 0; index3 < 8; ++index3)
        {
          Display[num2 + 1] = ((uint) this.TheRam[index2] & 128U) > 0U;
          Display[num2 + 2] = ((uint) this.TheRam[index2] & 64U) > 0U;
          Display[num2 + 3] = ((uint) this.TheRam[index2] & 32U) > 0U;
          Display[num2 + 4] = ((uint) this.TheRam[index2] & 1U) > 0U;
          Display[num2 + 5] = ((uint) this.TheRam[index2] & 2U) > 0U;
          Display[num2 + 6] = ((uint) this.TheRam[index2] & 8U) > 0U;
          Display[num2 + 7] = ((uint) this.TheRam[index2] & 4U) > 0U;
          num2 += 8;
          ++index2;
        }
        return true;
      }
      catch (Exception ex)
      {
        Debug.WriteLine("IErr: " + ex.ToString());
        int num3 = 60;
        for (int index = 0; index < 8; ++index)
        {
          Display[num3 + 1] = true;
          Display[num3 + 2] = false;
          Display[num3 + 3] = false;
          Display[num3 + 4] = false;
          Display[num3 + 5] = false;
          Display[num3 + 6] = false;
          Display[num3 + 7] = false;
          num3 += 8;
        }
        return false;
      }
    }

    private bool ITR_Nop() => true;

    private bool Itr_JumpAbsolut()
    {
      this.TheEepAdr = (uint) this.TheEEProm[(int) this.TheEepAdr] + ((uint) this.TheEEProm[(int) this.TheEepAdr + 1] << 8);
      return true;
    }

    private bool Dis_Edit()
    {
      this.Dis_VariableParam();
      if ((this.Dis_Flags & 1024) != 0)
        this.TheEepAdr += 8U;
      if ((this.Dis_Flags & 4096) != 0)
        this.TheEepAdr += 8U;
      this.LCD_Clear();
      this.LCD_WorkFrame();
      this.Dis_VariableRef();
      this.Itr_State |= (ushort) 272;
      return false;
    }

    private bool Itr_RuntimeBlock()
    {
      int num = (int) this.Itr_RunInter();
      return true;
    }

    private bool Dis_Constant()
    {
      this.LCD_Clear();
      this.LCD_WorkFrame();
      return false;
    }

    private bool Itr_CalcJump()
    {
      uint num1 = this.Itr_RunInter();
      byte num2 = this.TheEEProm[(int) this.TheEepAdr];
      if (num2 == byte.MaxValue)
      {
        this.TheEepAdr = num1;
      }
      else
      {
        if (num1 >= (uint) num2)
          num1 = 0U;
        uint index = (uint) ((int) this.TheEepAdr + 1 + (int) num1 * 2);
        this.TheEepAdr = (uint) this.TheEEProm[(int) index] + ((uint) this.TheEEProm[(int) index + 1] << 8);
      }
      return true;
    }

    private bool Value_WorkValueOverrun(ref uint DisplayValue)
    {
      if (DisplayValue <= 99999999U)
        return false;
      if ((DisplayValue & 2147483648U) > 0U)
        DisplayValue += 100000000U;
      else
        DisplayValue -= 100000000U;
      return true;
    }

    private uint Itr_RunInter()
    {
      if (((uint) this.Sta_Status & 256U) > 0U)
        return 0;
      if (this.TheEepAdr != 0U)
      {
        this.Itr_State &= (ushort) 53247;
        this.Itr_LoopCounter = (byte) 0;
        this.Itr_SkipRequest = false;
        do
        {
          this.Itr_Code = this.TheEEProm[(int) this.TheEepAdr];
          if (this.Itr_Code != (byte) 0)
          {
            if (this.Itr_LoopCounter++ != byte.MaxValue)
            {
              this.Itr_Code >>= 3;
              this.Itr_BitMask = (ushort) (1 << ((int) this.Itr_Code & 15));
              this.Itr_RunFunctionTab[(int) this.TheEEProm[(int) this.TheEepAdr++] & 7]();
            }
            else
              goto label_6;
          }
          else
            goto label_4;
        }
        while (((uint) this.Itr_State & 8192U) <= 0U);
        goto label_9;
label_4:
        ++this.TheEepAdr;
        goto label_9;
label_6:
        this.MyZelsiusMath.LastError = MeterMath.Errors.InternalError;
        this.MyZelsiusMath.LastErrorInfo = "Runtime Loop";
      }
label_9:
      return this.Itr_Akku;
    }

    private void Itr_SpecialFunction()
    {
      byte itrCode = this.Itr_Code;
      if (itrCode >= (byte) 24)
      {
        this.Itr_Code = (byte) ((uint) this.TheEEProm[(int) this.TheEepAdr++] >> 3);
        if (this.Itr_SkipIfRequest())
          return;
        this.Itr_LoadStoreWork();
      }
      else if (this.Itr_SkipRequest)
      {
        this.Itr_SkipRequest = false;
        return;
      }
      switch (itrCode)
      {
        case 2:
          this.Value_WorkValueOverrun(ref this.Itr_Akku);
          break;
        case 6:
          this.Itr_Akku = 0U;
          break;
        case 7:
          this.Itr_NextLoggerEntry();
          break;
        case 8:
          uint itrAkku = this.Itr_Akku;
          this.Itr_Akku = this.Itr_AkkuX;
          this.Itr_AkkuX = itrAkku;
          break;
        case 9:
          this.TheEepAdr += 2U;
          break;
        case 10:
          this.Mai_Events2 |= (ushort) 1;
          break;
        case 13:
          this.Itr_Akku = ZR_Calendar.Cal_Sec80ToDate(this.Itr_Akku).Month;
          break;
        case 14:
          this.Itr_Akku = ZR_Calendar.Cal_Sec80ToDate(this.Itr_Akku).Year;
          break;
        case 16:
          this.MyZelsiusMath.LastError = MeterMath.Errors.InternalError;
          this.MyZelsiusMath.LastErrorInfo = "Illegal funtime function ChangeMonth";
          break;
        case 17:
          this.MyZelsiusMath.LastError = MeterMath.Errors.InternalError;
          this.MyZelsiusMath.LastErrorInfo = "Illegal funtime function SetToNextYear";
          break;
        case 18:
          this.Itr_AkkuX = this.Itr_Akku;
          break;
        case 19:
          this.Itr_Akku = this.Itr_AkkuX;
          break;
        case 20:
          this.Itr_Akku = this.LoadRamUINT("Energ_KaelteEnergDisplay");
          break;
        case 24:
          this.Itr_Akku &= this.Itr_Akku2;
          this.Itr_SetFlags();
          break;
        case 25:
          this.Itr_Akku |= this.Itr_Akku2;
          this.Itr_SetFlags();
          break;
        case 26:
          this.Itr_Akku ^= this.Itr_Akku2;
          this.Itr_SetFlags();
          break;
        case 27:
          this.Itr_Akku = this.Itr_Akku2 % this.Itr_Akku;
          this.Itr_SetFlags();
          break;
        case 28:
          ulong num = (ulong) this.Itr_Akku * (ulong) this.Itr_Akku2;
          this.Itr_Akku = (uint) (ushort) num;
          this.Itr_AkkuX = (uint) (ushort) (num >> 32);
          break;
        case 29:
          if (this.Itr_Akku == 0U)
          {
            this.Itr_AkkuX = 0U;
            break;
          }
          this.Itr_Akku = (uint) (((ulong) this.Itr_Akku + (ulong) this.Itr_Akku2) / (ulong) this.Itr_Akku);
          break;
        case 30:
          this.Itr_Akku = this.Itr_Akku2 - this.Itr_Akku;
          this.Itr_SetFlags();
          this.Itr_Akku = this.Itr_Akku2;
          break;
      }
    }

    private void Itr_SetClear()
    {
      if (this.Itr_SkipRequest)
        this.Itr_SkipRequest = false;
      else if (((uint) this.Itr_Code & 16U) > 0U)
        this.Itr_Prozessabbild |= this.Itr_BitMask;
      else
        this.Itr_Prozessabbild &= ~this.Itr_BitMask;
    }

    private void Itr_SkipT_SkipF()
    {
      if (((uint) this.Itr_Code & 16U) > 0U)
      {
        if (((uint) this.Itr_Prozessabbild & (uint) this.Itr_BitMask) <= 0U)
          return;
        this.Itr_SkipRequest = true;
      }
      else if (((int) this.Itr_Prozessabbild & (int) this.Itr_BitMask) == 0)
        this.Itr_SkipRequest = true;
    }

    private void Itr_LoadStore()
    {
      if (this.Itr_SkipIfRequest())
        return;
      this.Itr_LoadStoreWork();
    }

    private void Itr_AddSub()
    {
      if (this.Itr_SkipIfRequest())
        return;
      this.Itr_Akku = this.Itr_Preload() <= (ushort) 0 ? this.Itr_Akku2 - this.Itr_Akku : this.Itr_Akku2 + this.Itr_Akku;
      this.Itr_SetFlags();
    }

    private void Itr_MulDiv()
    {
      if (this.Itr_SkipIfRequest())
        return;
      ushort num = this.Itr_Preload();
      if (this.Itr_Akku == 0U)
      {
        this.Itr_AkkuX = 0U;
      }
      else
      {
        if (num > (ushort) 0)
          this.Itr_Akku *= this.Itr_Akku2;
        else
          this.Itr_Akku = this.Itr_Akku2 / this.Itr_Akku;
        this.Itr_SetFlags();
      }
    }

    private void Itr_Jump()
    {
      ushort num;
      if (this.Itr_Code == (byte) 0)
      {
        num = (ushort) ((ulong) this.TheEepAdr + (ulong) (sbyte) this.TheEEProm[(int) this.TheEepAdr]);
        ++this.TheEepAdr;
      }
      else
        num = (ushort) (this.TheEepAdr + (uint) this.Itr_Code);
      if (this.Itr_SkipRequest)
        this.Itr_SkipRequest = false;
      else
        this.TheEepAdr = (uint) num;
    }

    private void Itr_Interval()
    {
      this.MyZelsiusMath.LastError = MeterMath.Errors.InternalError;
      this.MyZelsiusMath.LastErrorInfo = "Illegal runtime interval function";
    }

    private void Itr_NextLoggerEntry()
    {
      byte[] Data = new byte[14];
      uint num1 = this.LoadEEPromUSHORT((int) this.TheEepAdr);
      this.TheEepAdr += 2U;
      this.MyZelsiusMath.GetCpuDataWork("0x" + (num1 - 3U).ToString("x04"), Data.Length, MemoryLocation.EEPROM, out Data);
      int num2 = this.LoadArrayUSHORT(Data, 1) - (int) Data[0];
      if (num2 < this.LoadArrayUSHORT(Data, 7))
        num2 = (((int) Data[13] & 128) != 128 ? this.LoadArrayUSHORT(Data, 11) : this.LoadArrayUSHORT(Data, 9)) - (int) Data[0];
      this.TheEEProm[(int) num1 - 2] = (byte) num2;
      this.TheEEProm[(int) num1 - 1] = (byte) (num2 >> 8);
    }

    private void Itr_SetFlags()
    {
      this.Itr_Prozessabbild &= (ushort) 65439;
      if (this.Itr_Akku == 0U)
        this.Itr_Prozessabbild |= (ushort) 64;
      if (this.Itr_Akku <= this.Itr_Akku2)
        return;
      this.Itr_Prozessabbild |= (ushort) 32;
    }

    private bool Itr_SkipIfRequest()
    {
      if (!this.Itr_SkipRequest)
        return false;
      this.TheEepAdr += (uint) ZelsiusInterpreter.SkipBytes[(int) this.Itr_Code & 15];
      this.Itr_SkipRequest = false;
      return true;
    }

    private void Itr_LoadStoreWork()
    {
      if (((uint) this.Itr_Code & 16U) > 0U)
        this.Itr_Akku2 = this.Itr_Akku;
      uint num = (uint) (((int) this.Itr_Code & 3) + 1);
      if (num == 3U)
      {
        if (this.Itr_Code == (byte) 18)
          this.Itr_Akku = this.LoadRamUINT("Vol_VolumenDisplay");
        else if (this.Itr_Code == (byte) 22)
          this.Itr_Akku = this.Itr_AkkuX;
        else if (this.Itr_Code == (byte) 26)
          this.Itr_Akku = this.LoadRamUSHORT("Vol_Flow");
        else if (this.Itr_Code == (byte) 2)
        {
          this.MyZelsiusMath.LastError = MeterMath.Errors.InternalError;
          this.MyZelsiusMath.LastErrorInfo = "Illegal access to high resolution infos";
        }
        else if (this.Itr_Code == (byte) 6)
        {
          this.Itr_Akku = this.LoadRamUINT("Sta_Secounds");
        }
        else
        {
          if (this.Itr_Code != (byte) 10)
            return;
          this.Itr_Akku = this.LoadRamUINT("Energ_WaermeEnergDisplay");
        }
      }
      else if (((uint) this.Itr_Code & 4U) > 0U)
      {
        if (((uint) this.Itr_Code & 16U) <= 0U)
          return;
        uint theEepAdr = this.TheEepAdr;
        this.TheEepAdr += num;
        switch (num)
        {
          case 1:
            this.Itr_Akku = this.LoadEEPromBYTE((int) theEepAdr);
            break;
          case 2:
            this.Itr_Akku = this.LoadEEPromUSHORT((int) theEepAdr);
            break;
          case 4:
            this.Itr_Akku = this.LoadEEPromUINT((int) theEepAdr);
            break;
        }
      }
      else
      {
        uint Address;
        if (((uint) this.Itr_Prozessabbild & 128U) > 0U)
        {
          Address = this.Itr_Akku;
          this.Itr_Prozessabbild &= (ushort) 65407;
        }
        else
        {
          Address = this.LoadEEPromUSHORT((int) this.TheEepAdr);
          this.TheEepAdr += 2U;
        }
        if (((uint) this.Itr_Code & 8U) > 0U)
        {
          if (((uint) this.Itr_Code & 16U) <= 0U)
            return;
          switch (num)
          {
            case 1:
              this.Itr_Akku = this.LoadEEPromBYTE((int) Address);
              break;
            case 2:
              this.Itr_Akku = this.LoadEEPromUSHORT((int) Address);
              break;
            case 4:
              this.Itr_Akku = this.LoadEEPromUINT((int) Address);
              break;
          }
        }
        else
        {
          if (((uint) this.Itr_Code & 16U) <= 0U)
            return;
          switch (num)
          {
            case 1:
              this.Itr_Akku = this.LoadRamBYTE("0x" + Address.ToString("x04"));
              break;
            case 2:
              this.Itr_Akku = this.LoadRamUSHORT("0x" + Address.ToString("x04"));
              break;
            case 4:
              this.Itr_Akku = this.LoadRamUINT("0x" + Address.ToString("x04"));
              break;
          }
        }
      }
    }

    private ushort Itr_Preload()
    {
      ushort num = (ushort) ((uint) this.Itr_Code & 16U);
      this.Itr_Code |= (byte) 16;
      this.Itr_LoadStoreWork();
      return num;
    }

    private bool Dis_Variable()
    {
      this.Dis_VariableParam();
      if ((this.Dis_Flags & 4096) == 0)
      {
        if (((int) this.TheRam[157] & 16) == 0)
          this.LCD_Clear();
        this.LCD_WorkFrame();
        byte num = ~(byte) 16;
        this.TheRam[157] &= (byte) 239;
      }
      else if (((int) this.TheRam[157] & 16) == 0)
      {
        this.LCD_Clear();
        this.TheRam[157] |= (byte) 16;
      }
      if ((this.Dis_Flags & 768) == 512)
        this.Dis_Address = this.TheEepAdr;
      this.Dis_VariableRef();
      return (this.Dis_Flags & 4096) != 0;
    }

    private void Dis_VariableParam()
    {
      this.Dis_Flags = (int) this.LoadEEPromUSHORT((int) this.TheEepAdr);
      this.TheEepAdr += 2U;
      if ((this.Dis_Flags & 768) == 512)
        return;
      this.Dis_Address = this.LoadEEPromUSHORT((int) this.TheEepAdr);
      this.TheEepAdr += 2U;
      if ((this.Dis_Flags & 32768) != 0)
        this.Dis_Address = (uint) this.TheEEProm[(int) this.Dis_Address] + ((uint) this.TheEEProm[(int) this.Dis_Address + 1] << 8);
    }

    private void Dis_VariableRef()
    {
      uint num = this.Dis_VariableLoadValue();
      if ((this.Dis_Flags & 16384) == 0)
        this.LCD_Number(num, (byte) this.Dis_Flags);
      else if ((this.Dis_Flags & 16) != 0)
      {
        if ((this.Dis_Flags & 64) != 0)
        {
          this.LCD_ShowTemperature((ushort) num);
        }
        else
        {
          CalTime time = ZR_Calendar.Cal_Sec80ToTime(num);
          this.LCD_Number(time.Minute, this.LCD_FORMAT_FILL_DEC(2, 3));
          this.LCD_Number(time.Hour, this.LCD_FORMAT_FILL_DEC(2, 5));
        }
      }
      else
      {
        CalDate date = ZR_Calendar.Cal_Sec80ToDate(num);
        if ((this.Dis_Flags & 64) != 0)
        {
          this.LCD_Number(date.Month, this.LCD_FORMAT_FILL_DEC(2, 0));
          this.LCD_Number(date.Day, this.LCD_FORMAT_FILL_DEC(2, 2));
        }
        else
        {
          this.LCD_Number(date.Year, this.LCD_FORMAT_FILL_DEC(2, 0));
          this.LCD_Number(date.Month, this.LCD_FORMAT_FILL_DEC(2, 2));
          this.LCD_Number(date.Day, this.LCD_FORMAT_FILL_DEC(2, 4));
        }
      }
    }

    private uint Dis_VariableLoadValue()
    {
      byte num1 = (byte) (((this.Dis_Flags & 768) >> 8) + 1);
      uint num2 = 0;
      if ((this.Dis_Flags & 2048) != 0)
      {
        switch (num1)
        {
          case 1:
            num2 = this.LoadEEPromBYTE((int) this.Dis_Address);
            break;
          case 2:
            num2 = this.LoadEEPromUSHORT((int) this.Dis_Address);
            break;
          case 4:
            num2 = this.LoadEEPromUINT((int) this.Dis_Address);
            break;
        }
      }
      else if ((this.Dis_Flags & 768) != 512)
      {
        switch (num1)
        {
          case 1:
            num2 = this.LoadRamBYTE("0x" + this.Dis_Address.ToString("x04"));
            break;
          case 2:
            num2 = this.LoadRamUSHORT("0x" + this.Dis_Address.ToString("x04"));
            break;
          case 4:
            num2 = this.LoadRamUINT("0x" + this.Dis_Address.ToString("x04"));
            break;
        }
      }
      else
      {
        this.TheEepAdr = this.Dis_Address;
        num2 = this.Itr_RunInter();
      }
      return num2;
    }

    private byte LCD_FORMAT_BLANK_DEC(int digits, int start_digit)
    {
      return (byte) (digits - 1 | start_digit << 3);
    }

    private byte LCD_FORMAT_FILL_DEC(int digits, int start_digit)
    {
      return (byte) (digits - 1 | start_digit << 3 | 64);
    }

    private byte LCD_FORMAT_BLANK_HEX(int digits, int start_digit)
    {
      return (byte) (digits - 1 | start_digit << 3 | 128);
    }

    private byte LCD_FORMAT_FILL_HEX(int digits, int start_digit)
    {
      return (byte) (digits - 1 | start_digit << 3 | 64 | 128);
    }

    private void LCD_Clear()
    {
      for (int index = 145; index <= 160; ++index)
        this.TheRam[index] = (byte) 0;
    }

    private void LCD_WorkFrame()
    {
      byte[] CopyList = new byte[15];
      for (int index = 0; index < 15; ++index)
        CopyList[index] = this.TheEEProm[(long) this.TheEepAdr + (long) index];
      this.TheEepAdr += this.LCD_WorkFrameInternal(CopyList);
    }

    private uint LCD_WorkFrameInternal(byte[] CopyList)
    {
      int index1 = 0;
      uint index2 = 1;
      for (int index3 = 1; index3 <= 8; index3 <<= 1)
      {
        if (((uint) CopyList[0] & (uint) index3) > 0U)
        {
          for (byte index4 = 1; index4 > (byte) 0; index4 <<= 1)
          {
            if (index3 != 8 || ((uint) index4 & 224U) <= 0U)
            {
              if (((uint) CopyList[(int) index2] & (uint) index4) > 0U)
                this.TheRam[(int) this.LCD_FrameByteList[index1]] |= this.LCD_FrameByteList[index1 + 1];
              index1 += 2;
            }
          }
          ++index2;
        }
        else
          index1 += 16;
      }
      if (((uint) CopyList[0] & 16U) > 0U)
      {
        int index5 = ((int) CopyList[(int) index2] & 240) >> 4;
        if (index5 < 7)
          this.TheRam[145] |= this.Lcd_MenueColumnList[index5];
        ++index2;
      }
      if (((uint) CopyList[0] & 32U) > 0U)
      {
        byte[] theRam1 = this.TheRam;
        byte[] numArray1 = CopyList;
        int index6 = (int) index2;
        uint num1 = (uint) (index6 + 1);
        int num2 = (int) numArray1[index6];
        theRam1[158] = (byte) num2;
        byte[] theRam2 = this.TheRam;
        byte[] numArray2 = CopyList;
        int index7 = (int) num1;
        index2 = (uint) (index7 + 1);
        int num3 = (int) numArray2[index7];
        theRam2[159] = (byte) num3;
        if (this.TheRam[158] == byte.MaxValue)
        {
          for (int index8 = 145; index8 <= 156; ++index8)
            this.TheRam[index8] = byte.MaxValue;
        }
      }
      if (((uint) CopyList[0] & 128U) > 0U)
      {
        int index9 = 146;
        do
        {
          this.TheRam[index9] = (byte) ((int) this.TheRam[index9] & -240 | (int) CopyList[(int) index2] & 239);
          ++index9;
        }
        while (((uint) CopyList[(int) index2++] & 16U) > 0U);
      }
      return index2;
    }

    private void LCD_Number(uint zahl, byte format)
    {
      bool flag = false;
      byte num = (byte) (((int) format & 56) >> 3);
      for (byte index1 = (byte) ((uint) num + ((uint) format & 7U)); (int) num <= (int) index1; ++num)
      {
        this.TheRam[153 - (int) num] &= (byte) 16;
        if (!flag)
        {
          byte index2;
          if (((uint) format & 128U) > 0U)
          {
            index2 = (byte) (zahl & 15U);
            zahl >>= 4;
          }
          else
          {
            if (zahl == uint.MaxValue)
            {
              this.TheRam[153 - (int) num] |= (byte) 4;
              continue;
            }
            index2 = (byte) (zahl % 10U);
            zahl /= 10U;
          }
          this.TheRam[153 - (int) num] |= this.Lcd_CodeList[(int) index2];
          if (zahl == 0U && ((int) format & 64) == 0)
          {
            for (byte index3 = 0; index3 < (byte) 5; ++index3)
            {
              if (((uint) this.TheRam[(int) this.Lcd_DecimalList[(int) index3]] & (uint) this.Lcd_DecimalList[(int) index3 + 5]) > 0U)
              {
                if ((int) num > (int) index3)
                  break;
                goto label_15;
              }
            }
            flag = true;
label_15:;
          }
        }
      }
    }

    private void LCD_ShowTemperature(ushort temperature)
    {
      byte num = 0;
      if (((uint) temperature & 32768U) > 0U)
      {
        num = (byte) 1;
        temperature = ~temperature;
        ++temperature;
      }
      this.LCD_Number((uint) temperature, this.LCD_FORMAT_BLANK_DEC(5, 2));
      if (num != (byte) 1)
        return;
      if (temperature > (ushort) 9999)
        this.TheRam[146] = (byte) 4;
      else if (temperature > (ushort) 999)
        this.TheRam[147] = (byte) 4;
      else
        this.TheRam[148] = (byte) 4;
    }

    private uint LoadEEPromUINT(int Address) => this.LoadEEProm(Address, 4);

    private uint LoadEEPromUSHORT(int Address) => this.LoadEEProm(Address, 2);

    private uint LoadEEPromBYTE(int Address) => this.LoadEEProm(Address, 1);

    private uint LoadEEProm(int Address, int Size)
    {
      uint num = 0;
      if ((long) Address < (long) this.TheEEPromSize - (long) Size)
      {
        for (int index = 0; index < Size; ++index)
          num += (uint) this.TheEEProm[Address + index] << index * 8;
      }
      else
      {
        byte[] Data;
        if (this.MyZelsiusMath.GetCpuDataWork("0x" + Address.ToString("x04"), Size, MemoryLocation.EEPROM, out Data))
        {
          for (int index = 0; index < Size; ++index)
            num += (uint) Data[index] << index * 8;
        }
      }
      return num;
    }

    private uint LoadRamUINT(string Address)
    {
      byte[] Data;
      if (!this.MyZelsiusMath.GetCpuDataWork(Address, 4, MemoryLocation.RAM, out Data))
        return 0;
      uint num = 0;
      for (int index = 0; index < 4; ++index)
        num += (uint) Data[index] << index * 8;
      return num;
    }

    private uint LoadRamUSHORT(string Address)
    {
      byte[] Data;
      if (!this.MyZelsiusMath.GetCpuDataWork(Address, 2, MemoryLocation.RAM, out Data))
        return 0;
      uint num = 0;
      for (int index = 0; index < 2; ++index)
        num += (uint) Data[index] << index * 8;
      return num;
    }

    private int LoadArrayUSHORT(byte[] TheArray, int Address)
    {
      return (int) TheArray[Address] + ((int) TheArray[Address + 1] << 8);
    }

    private uint LoadRamBYTE(string Address)
    {
      byte[] Data;
      return !this.MyZelsiusMath.GetCpuDataWork(Address, 1, MemoryLocation.RAM, out Data) ? 0U : (uint) Data[0];
    }

    private delegate bool DisplayFunction();

    private delegate void RuntimeFunction();
  }
}
