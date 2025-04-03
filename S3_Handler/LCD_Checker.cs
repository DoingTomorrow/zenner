// Decompiled with JetBrains decompiler
// Type: S3_Handler.LCD_Checker
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class LCD_Checker
  {
    private S3_HandlerFunctions myFunctions;
    private ByteField lcdRamData;

    internal LCD_Checker(S3_HandlerFunctions myFunctions) => this.myFunctions = myFunctions;

    internal bool GetLcdDisplay(out LCD_Display theDisplay)
    {
      theDisplay = new LCD_Display();
      if (!this.myFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, 2592, 64, out this.lcdRamData))
        return false;
      theDisplay.lcdValue = this.GetLcdValue();
      theDisplay.lcdUnit = this.GetUnit();
      return true;
    }

    private string GetLcdValue()
    {
      StringBuilder stringBuilder = new StringBuilder(20);
      for (int index = 1; index < 9; ++index)
        stringBuilder.Append(this.GetSegValue(this.lcdRamData.Data[index]));
      if (((uint) this.lcdRamData.Data[7] & 16U) > 0U)
        stringBuilder.Insert(7, ".");
      if (((uint) this.lcdRamData.Data[6] & 16U) > 0U)
        stringBuilder.Insert(6, ".");
      if (((uint) this.lcdRamData.Data[5] & 16U) > 0U)
        stringBuilder.Insert(5, ".");
      if (((uint) this.lcdRamData.Data[4] & 16U) > 0U)
        stringBuilder.Insert(4, ".");
      if (((uint) this.lcdRamData.Data[3] & 16U) > 0U)
        stringBuilder.Insert(3, ":");
      if (((uint) this.lcdRamData.Data[1] & 16U) > 0U)
        stringBuilder.Insert(1, ".");
      return stringBuilder.ToString();
    }

    private char GetSegValue(byte dispRamByte)
    {
      switch ((byte) ((uint) dispRamByte & 239U))
      {
        case 0:
          return ' ';
        case 11:
          return 'L';
        case 12:
          return 'r';
        case 15:
          return 't';
        case 43:
          return 'C';
        case 46:
          return 'F';
        case 47:
          return 'E';
        case 102:
          return '°';
        case 109:
          return '2';
        case 110:
          return 'P';
        case 131:
          return 'u';
        case 143:
          return 'b';
        case 167:
          return '5';
        case 173:
          return 'S';
        case 175:
          return '6';
        case 192:
          return '1';
        case 198:
          return '4';
        case 205:
          return 'd';
        case 206:
          return 'H';
        case 224:
          return '7';
        case 225:
          return ']';
        case 229:
          return '3';
        case 231:
          return '9';
        case 235:
          return '0';
        case 238:
          return 'A';
        case 239:
          return '8';
        default:
          return '#';
      }
    }

    private string GetUnit()
    {
      string unit = "illegal unit";
      LCD_Checker.LcdUnitElements lcdUnitElements = (LCD_Checker.LcdUnitElements) 0;
      if (((uint) this.lcdRamData.Data[10] & 1U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.mega;
      if (((uint) this.lcdRamData.Data[10] & 2U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.qm;
      if (((uint) this.lcdRamData.Data[10] & 4U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.watt;
      if (((uint) this.lcdRamData.Data[10] & 8U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.kilo;
      if (((uint) this.lcdRamData.Data[10] & 16U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.hour;
      if (((uint) this.lcdRamData.Data[10] & 32U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.liter;
      if (((uint) this.lcdRamData.Data[10] & 64U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.per;
      if (((uint) this.lcdRamData.Data[9] & 128U) > 0U)
        lcdUnitElements |= LCD_Checker.LcdUnitElements.gigaJoul;
      switch (lcdUnitElements)
      {
        case (LCD_Checker.LcdUnitElements) 0:
          unit = "";
          break;
        case LCD_Checker.LcdUnitElements.qm:
          unit = "m\u00B3";
          break;
        case LCD_Checker.LcdUnitElements.watt:
          unit = "W";
          break;
        case LCD_Checker.LcdUnitElements.mega | LCD_Checker.LcdUnitElements.watt:
          unit = "MW";
          break;
        case LCD_Checker.LcdUnitElements.watt | LCD_Checker.LcdUnitElements.hour:
          unit = "Wh";
          break;
        case LCD_Checker.LcdUnitElements.mega | LCD_Checker.LcdUnitElements.watt | LCD_Checker.LcdUnitElements.hour:
          unit = "MWh";
          break;
        case LCD_Checker.LcdUnitElements.liter:
          unit = "l";
          break;
        case LCD_Checker.LcdUnitElements.qm | LCD_Checker.LcdUnitElements.hour | LCD_Checker.LcdUnitElements.per:
          unit = "m\u00B3/h";
          break;
        case LCD_Checker.LcdUnitElements.hour | LCD_Checker.LcdUnitElements.liter | LCD_Checker.LcdUnitElements.per:
          unit = "l/h";
          break;
        case LCD_Checker.LcdUnitElements.watt | LCD_Checker.LcdUnitElements.kilo:
          unit = "kW";
          break;
        case LCD_Checker.LcdUnitElements.watt | LCD_Checker.LcdUnitElements.hour | LCD_Checker.LcdUnitElements.kilo:
          unit = "kWh";
          break;
        case LCD_Checker.LcdUnitElements.gigaJoul:
          unit = "GJ";
          break;
        case LCD_Checker.LcdUnitElements.hour | LCD_Checker.LcdUnitElements.per | LCD_Checker.LcdUnitElements.gigaJoul:
          unit = "GJ/h";
          break;
      }
      if (((uint) this.lcdRamData.Data[0] & 4U) > 0U)
        unit += " *";
      if (((int) this.lcdRamData.Data[0] & 48) == 48)
      {
        unit += " delta";
      }
      else
      {
        if (((uint) this.lcdRamData.Data[0] & 16U) > 0U)
          unit += " flow";
        if (((uint) this.lcdRamData.Data[0] & 32U) > 0U)
          unit += " return";
      }
      return unit;
    }

    private enum LcdUnitElements
    {
      qm = 1,
      mega = 2,
      watt = 4,
      hour = 8,
      liter = 16, // 0x00000010
      per = 32, // 0x00000020
      kilo = 64, // 0x00000040
      gigaJoul = 128, // 0x00000080
    }
  }
}
