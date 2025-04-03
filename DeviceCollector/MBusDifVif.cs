// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusDifVif
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MBusDifVif
  {
    private static Logger loggerMBusDifVif = LogManager.GetLogger(nameof (MBusDifVif));
    public bool DifSizeUnchangable;
    private List<byte> difVifList = new List<byte>();

    public MBusDifVif()
    {
    }

    public MBusDifVif(MBusDifVif.DifVifOptions options)
    {
      if (options != MBusDifVif.DifVifOptions.DifSizeUnchangabel)
        return;
      this.DifSizeUnchangable = true;
    }

    public byte[] DifVifArray => this.difVifList.ToArray();

    public int DifByteSize
    {
      get
      {
        if (this.difVifList == null)
          return 0;
        int index = 0;
        while (((int) this.difVifList[index] & 128) != 0)
          ++index;
        return index + 1;
      }
      set
      {
        int difByteSize = this.DifByteSize;
        if (value == difByteSize)
          return;
        if (value < difByteSize)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "can not reduce DifByteSize");
        }
        else
        {
          for (; difByteSize < value; ++difByteSize)
          {
            this.difVifList[difByteSize - 1] |= (byte) 128;
            this.difVifList.Insert(difByteSize, (byte) 0);
          }
        }
      }
    }

    public int VifByteSize
    {
      get => this.difVifList.Count - this.DifByteSize;
      set
      {
        int vifByteSize = this.VifByteSize;
        if (value == vifByteSize)
          return;
        if (value < vifByteSize)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "can not reduce VifByteSize");
        }
        else
        {
          for (; vifByteSize < value; ++vifByteSize)
          {
            this.difVifList[this.difVifList.Count - 1] |= (byte) 128;
            this.difVifList.Add((byte) 0);
          }
        }
      }
    }

    public bool IsBCD
    {
      get
      {
        return this.difVifList.Count != 0 && MBusDevice.DifParamCodeTable[(int) this.difVifList[0] & 15] == MBusDevice.ParamCode.BCD;
      }
    }

    public bool IsDateTime_32bit
    {
      get
      {
        if (this.difVifList == null || this.difVifList.Count == 0)
          return false;
        int difByteSize = this.DifByteSize;
        if (difByteSize == this.difVifList.Count - 1)
          return ((int) this.difVifList[difByteSize] & (int) sbyte.MaxValue) == 109;
        if (this.difVifList[difByteSize] == (byte) 253)
          return ((int) this.difVifList[0] & 15) == 4 && ((int) this.difVifList[difByteSize + 1] & (int) sbyte.MaxValue) == 48;
        return this.difVifList[difByteSize] != (byte) 251 && ((int) this.difVifList[0] & 15) == 4 && ((int) this.difVifList[difByteSize + 1] & 114) == 66;
      }
    }

    public bool IsDate_16bit
    {
      get
      {
        if (this.difVifList == null || this.difVifList.Count == 0)
          return false;
        int difByteSize = this.DifByteSize;
        if (difByteSize == this.difVifList.Count - 1)
          return ((int) this.difVifList[difByteSize] & (int) sbyte.MaxValue) == 108;
        if (this.difVifList[difByteSize] == (byte) 253)
          return ((int) this.difVifList[0] & 15) == 2 && ((int) this.difVifList[difByteSize + 1] & (int) sbyte.MaxValue) == 48;
        return this.difVifList[difByteSize] != (byte) 251 && ((int) this.difVifList[0] & 15) == 2 && ((int) this.difVifList[difByteSize + 1] & 114) == 66;
      }
    }

    public int StorageNumber
    {
      get
      {
        int storageNumber = 0;
        int index = 0;
        while (true)
        {
          if (index == 0)
            storageNumber = ((int) this.difVifList[index] & 64) >> 6;
          else
            storageNumber += ((int) this.difVifList[index] & 15) << index * 4 - 3;
          if (((int) this.difVifList[index] & 128) != 0)
            ++index;
          else
            break;
        }
        return storageNumber;
      }
      set
      {
        int num1 = value;
        int index = 0;
        while (true)
        {
          byte num2;
          if (index == 0)
          {
            this.difVifList[index] &= (byte) 191;
            num2 = (byte) ((num1 & 1) << 6);
            num1 >>= 1;
          }
          else
          {
            this.difVifList[index] &= (byte) 240;
            num2 = (byte) (num1 & 15);
            num1 >>= 4;
          }
          this.difVifList[index] |= num2;
          if (num1 != 0)
          {
            if (((int) this.difVifList[index] & 128) == 0)
            {
              this.difVifList[index] |= (byte) 128;
              this.difVifList.Insert(index + 1, (byte) 0);
            }
            ++index;
          }
          else
            break;
        }
        while (((uint) this.difVifList[index] & 128U) > 0U)
        {
          ++index;
          this.difVifList[index] &= (byte) 240;
        }
        this.DeleteNotUsedDifs();
      }
    }

    public int TarifNumber
    {
      get
      {
        if (this.DifByteSize < 2)
          return 0;
        int tarifNumber = 0;
        int index = 1;
        while (true)
        {
          tarifNumber += ((int) this.difVifList[index] & 48) >> 4 << index * 2 - 2;
          if (((int) this.difVifList[index] & 128) != 0)
            ++index;
          else
            break;
        }
        return tarifNumber;
      }
      set
      {
        int num = value;
        int index = 0;
        while (num > 0)
        {
          if (((int) this.difVifList[index] & 128) == 0)
          {
            this.difVifList[index] |= (byte) 128;
            this.difVifList.Insert(index + 1, (byte) 0);
          }
          else
            this.difVifList[index + 1] &= (byte) 207;
          this.difVifList[index + 1] |= (byte) ((num & 3) << 4);
          num >>= 2;
          ++index;
        }
        for (; ((uint) this.difVifList[index] & 128U) > 0U; ++index)
          this.difVifList[index + 1] &= (byte) 207;
        this.DeleteNotUsedDifs();
      }
    }

    public int Subunit
    {
      get
      {
        if (this.DifByteSize < 2)
          return 0;
        int subunit = 0;
        int index = 1;
        while (true)
        {
          subunit += ((int) this.difVifList[index] & 64) >> 6 << index - 1;
          if (((int) this.difVifList[index] & 128) != 0)
            ++index;
          else
            break;
        }
        return subunit;
      }
      set
      {
        int num = value;
        int index = 0;
        while (num > 0)
        {
          if (((int) this.difVifList[index] & 128) == 0)
          {
            this.difVifList[index] |= (byte) 128;
            this.difVifList.Insert(index + 1, (byte) 0);
          }
          else
            this.difVifList[index + 1] &= (byte) 191;
          this.difVifList[index + 1] |= (byte) ((num & 1) << 6);
          num >>= 1;
          ++index;
        }
        for (; ((uint) this.difVifList[index] & 128U) > 0U; ++index)
          this.difVifList[index + 1] &= (byte) 191;
        this.DeleteNotUsedDifs();
      }
    }

    private void DeleteNotUsedDifs()
    {
      if (this.DifSizeUnchangable)
        return;
      int index = this.DifByteSize - 1;
      while (index > 0 && this.difVifList[index] == (byte) 0)
      {
        this.difVifList.RemoveAt(index);
        --index;
        this.difVifList[index] &= (byte) 127;
      }
    }

    public FunctionFiled FunctionFiled
    {
      get
      {
        return this.difVifList == null || this.difVifList.Count < 1 ? FunctionFiled.InstantaneousValue : (FunctionFiled) Enum.ToObject(typeof (FunctionFiled), ((int) this.difVifList[0] & 48) >> 4);
      }
      set
      {
        if (this.difVifList == null || this.difVifList.Count < 1)
          return;
        this.difVifList[0] &= (byte) 207;
        this.difVifList[0] |= (byte) ((uint) value << 4);
      }
    }

    public int ByteSize => this.difVifList.Count;

    public void SetScalingVif(int theVif)
    {
      if (theVif < 128 && theVif >= 0)
      {
        int difByteSize = this.DifByteSize;
        if ((int) this.difVifList[difByteSize] == (int) (byte) theVif)
          return;
        if (MBusDifVif.loggerMBusDifVif.IsTraceEnabled)
          MBusDifVif.loggerMBusDifVif.Trace("Change VIF from 0x" + this.difVifList[difByteSize].ToString("x02") + " to 0x" + theVif.ToString("x02"));
        this.difVifList[difByteSize] = (byte) (((int) this.difVifList[difByteSize] & 128) + theVif);
      }
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not change vif", MBusDifVif.loggerMBusDifVif);
    }

    public bool LoadDifVif(byte[] difVif) => this.LoadDifVif(difVif, 0);

    public bool LoadDifVif(byte[] byteBuffer, int difVifStartOffset)
    {
      this.difVifList.Clear();
      bool flag = false;
      for (int index = 0; difVifStartOffset + index < byteBuffer.Length; ++index)
      {
        byte num = byteBuffer[difVifStartOffset + index];
        this.difVifList.Add(num);
        if (((int) num & 128) == 0)
        {
          if (flag)
            return true;
          flag = true;
        }
      }
      return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "DifVif not complete", MBusDifVif.loggerMBusDifVif);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("(DifVif:");
      for (int index = 0; index < this.difVifList.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(' ');
        stringBuilder.Append(this.difVifList[index].ToString("x02"));
      }
      stringBuilder.Append(") (ZDF: " + this.GetZdfString() + ")");
      return stringBuilder.ToString();
    }

    public string GetZdfString()
    {
      StringBuilder zdfString = new StringBuilder();
      int difByteSize = this.DifByteSize;
      int vifByteSize = this.VifByteSize;
      if (vifByteSize >= 1)
      {
        if (this.DifVifArray[difByteSize] == (byte) 253)
        {
          if (vifByteSize < 2)
            return "IllegalVifFD";
          int index = (int) this.difVifList[difByteSize + 1] & (int) sbyte.MaxValue;
          zdfString.Append(MBusDevice.VifEList_0xFD[index].VifString);
          this.AddOrtogonalVIF(difByteSize + 1, zdfString);
        }
        else if (this.DifVifArray[difByteSize] == (byte) 251)
        {
          if (vifByteSize < 2)
            return "IllegalVifFB";
          int index = (int) this.difVifList[difByteSize + 1] & (int) sbyte.MaxValue;
          zdfString.Append(MBusDevice.VifEList_0xFB[index].VifString);
          this.AddOrtogonalVIF(difByteSize + 1, zdfString);
        }
        else if (this.DifVifArray[difByteSize] == (byte) 252 || this.DifVifArray[difByteSize] == (byte) 124)
        {
          zdfString.Append("ManSpec");
        }
        else
        {
          int index = (int) this.difVifList[difByteSize] & (int) sbyte.MaxValue;
          zdfString.Append(MBusDevice.VifList[index].VifString);
          if (index == (int) sbyte.MaxValue)
            zdfString.Append("ManSpec");
          else
            this.AddOrtogonalVIF(difByteSize, zdfString);
        }
      }
      int storageNumber = this.StorageNumber;
      int subunit = this.Subunit;
      int tarifNumber = this.TarifNumber;
      FunctionFiled functionFiled = this.FunctionFiled;
      if (storageNumber > 0 || storageNumber > 0)
        zdfString.Append("[" + storageNumber.ToString() + "]");
      if (subunit > 0)
        zdfString.Append("[" + subunit.ToString() + "]");
      switch (functionFiled)
      {
        case FunctionFiled.MaximumValue:
          zdfString.Append("_MAX");
          break;
        case FunctionFiled.MinimumValue:
          zdfString.Append("_MIN");
          break;
        case FunctionFiled.ValueDuringErrorState:
          zdfString.Append("_ERR");
          break;
      }
      if (tarifNumber > 0)
        zdfString.Append("_TAR[" + tarifNumber.ToString() + "]");
      return zdfString.ToString();
    }

    private void AddOrtogonalVIF(int offset, StringBuilder zdfString)
    {
      if (((int) this.difVifList[offset] & 128) == 0)
        return;
      ++offset;
      if (offset >= this.difVifList.Count)
        zdfString.Append("IllegalOrtoVif");
      int index = (int) this.difVifList[offset] & (int) sbyte.MaxValue;
      zdfString.Append(MBusDevice.VifEListOrto[index].VifStringToAdd);
    }

    public static DateTime? GetMBusDateTime(byte[] buffer, int offset)
    {
      int minute = (int) buffer[offset] & 63;
      int hour = (int) buffer[offset + 1] & 31;
      int day = (int) buffer[offset + 2] & 31;
      int month = (int) buffer[offset + 3] & 15;
      int num1 = ((int) buffer[offset + 2] >> 5) + (((int) buffer[offset + 3] & 240) >> 1);
      int num2 = ((int) buffer[offset + 1] & 96) >> 5;
      bool flag1 = ((int) buffer[offset] & 128) == 0;
      bool flag2 = ((uint) buffer[offset + 1] & 128U) > 0U;
      int year = num1 > 80 ? num1 + (1900 + 100 * num2) : num1 + 2000;
      if (month < 1 || month > 12)
        return new DateTime?();
      if (hour < 1 || hour > 24)
        return new DateTime?();
      try
      {
        DateTime dateTime = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Unspecified);
        if (flag2)
          dateTime = dateTime.AddHours(-1.0);
        if (!flag1)
          dateTime = DateTime.MinValue;
        return new DateTime?(dateTime);
      }
      catch
      {
        return new DateTime?();
      }
    }

    public enum DifVifOptions
    {
      Non,
      DifSizeUnchangabel,
    }
  }
}
