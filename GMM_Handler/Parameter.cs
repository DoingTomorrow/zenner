// Decompiled with JetBrains decompiler
// Type: GMM_Handler.Parameter
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Globalization;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class Parameter : LinkObj
  {
    private static string[] ParameterGroupNames = Enum.GetNames(typeof (Parameter.ParameterGroups));
    internal string Name;
    public string FullName;
    internal string MeterResource = string.Empty;
    internal int FunctionNumber = -1;
    internal int FunctionIndex = -1;
    internal bool ExistOnCPU = false;
    internal bool ExistOnEprom = false;
    internal bool EpromValueIsInitialised = false;
    internal bool CPU_ValueIsInitialised = false;
    internal int AddressCPU = -1;
    internal LinkBlockTypes BlockMark;
    internal Parameter.ParamStorageType StoreType;
    internal bool[] GroupMember;
    internal long ValueCPU;
    internal long ValueEprom;
    internal long DefaultValue;
    internal long MinValue;
    internal long MaxValue;
    internal string Unit;
    internal Parameter.BaseParameterFormat ParameterFormat = Parameter.BaseParameterFormat.Integer;
    internal SortedList DifVifsByRes;
    internal long DifVifs;
    internal short DifVifSize = 0;
    internal bool MBusOn;
    internal bool MBusShortOn;
    internal int MBusParameterLength;
    internal string MBusNeadedResources = string.Empty;
    internal Parameter.MBusParameterConversion MBusParamConvertion = Parameter.MBusParameterConversion.None;
    internal LoggerParameterData LoggerData;
    internal int LoggerID;
    internal Parameter.MBusParameterOverrideType MBusParameterOverride;
    public string NameTranslated;
    public string ParameterInfo;
    internal int StructureNr;
    internal int StructureIndex;

    internal Parameter(string ParameterName, int ParameterSize, LinkBlockTypes Block)
    {
      this.GroupMember = new bool[Parameter.ParameterGroupNames.Length];
      this.Name = ParameterName;
      this.Size = ParameterSize;
      this.BlockMark = Block;
      if (this.BlockMark != LinkBlockTypes.RuntimeVars)
        return;
      this.ExistOnCPU = true;
    }

    internal bool LoadValueFromEprom(byte[] Eprom)
    {
      if (!this.ExistOnEprom || this.Size <= 0)
        return false;
      this.ValueEprom = 0L;
      for (int index = 0; index < this.Size; ++index)
        this.ValueEprom += (long) Eprom[this.Address + index] << index * 8;
      this.EpromValueIsInitialised = true;
      return true;
    }

    internal long GetValueFromMap(byte[] Eprom)
    {
      if (!this.ExistOnEprom || this.Size <= 0)
        throw new ArgumentException("Variable has no map size");
      long valueFromMap = 0;
      for (int index = 0; index < this.Size; ++index)
        valueFromMap += (long) Eprom[this.Address + index] << index * 8;
      return valueFromMap;
    }

    internal override void GetObjectInfo(StringBuilder InfoString, Meter TheMeter)
    {
      if (this.Address < 0)
        InfoString.Append("----");
      else
        InfoString.Append(this.Address.ToString("x04"));
      if (this.ExistOnCPU)
        InfoString.Append("=" + this.AddressCPU.ToString("x04"));
      InfoString.Append(TheMeter.GetEEPromWriteProtectionChar(this.LinkByteList, this.Address));
      if (this.Size < 1)
      {
        InfoString.Append(" -- -- -- -- -- -- -- --");
      }
      else
      {
        for (int index = 0; index < 8; ++index)
        {
          if (index >= this.Size)
            InfoString.Append(" ..");
          else if (this.LinkByteList == null)
            InfoString.Append(" --");
          else
            InfoString.Append(" " + this.LinkByteList[index].ToString("x02"));
        }
      }
      if (this.ExistOnEprom)
      {
        InfoString.Append(" V:0x");
        InfoString.Append(this.ValueEprom.ToString("x08"));
        InfoString.Append(" = ");
        if (this.ParameterFormat == Parameter.BaseParameterFormat.DateTime)
        {
          DateTime dateTime = ZR_Calendar.Cal_GetDateTime((uint) this.ValueEprom);
          InfoString.Append(" Time:'" + dateTime.ToString("dd.MM.yyyy HH:mm:ss") + "'");
        }
        else
          InfoString.Append(this.ValueEprom.ToString("d012"));
      }
      if (this.FunctionNumber >= 0)
      {
        if (TheMeter.MyHandler.MyInfoFlags.ShowFunctionNumbers)
          InfoString.Append(" F:" + this.FunctionNumber.ToString("d4"));
        if (TheMeter.MyHandler.MyInfoFlags.ShowFunctionNames)
          InfoString.Append(" Fn:" + ((Function) TheMeter.MyHandler.MyLoadedFunctions.LoadedFunctionHeaders[(object) (ushort) this.FunctionNumber]).Name);
      }
      InfoString.Append(" Pn:'" + this.Name + "'");
      InfoString.Append(ZR_Constants.SystemNewLine);
    }

    internal Parameter Clone()
    {
      Parameter parameter = new Parameter(this.Name, this.Size, this.BlockMark);
      parameter.MeterResource = this.MeterResource;
      parameter.FullName = this.FullName;
      parameter.FunctionNumber = this.FunctionNumber;
      parameter.FunctionIndex = this.FunctionIndex;
      parameter.ExistOnCPU = this.ExistOnCPU;
      parameter.ExistOnEprom = this.ExistOnEprom;
      parameter.EpromValueIsInitialised = this.EpromValueIsInitialised;
      parameter.CPU_ValueIsInitialised = this.CPU_ValueIsInitialised;
      parameter.AddressCPU = this.AddressCPU;
      parameter.StoreType = this.StoreType;
      if (this.GroupMember != null)
        parameter.GroupMember = (bool[]) this.GroupMember.Clone();
      parameter.ValueCPU = this.ValueCPU;
      parameter.ValueEprom = this.ValueEprom;
      parameter.DefaultValue = this.DefaultValue;
      parameter.MinValue = this.MinValue;
      parameter.MaxValue = this.MaxValue;
      parameter.Unit = this.Unit;
      parameter.ParameterFormat = this.ParameterFormat;
      parameter.DifVifsByRes = this.DifVifsByRes;
      parameter.DifVifs = this.DifVifs;
      parameter.DifVifSize = this.DifVifSize;
      parameter.MBusOn = this.MBusOn;
      parameter.MBusShortOn = this.MBusShortOn;
      parameter.MBusParameterLength = this.MBusParameterLength;
      parameter.MBusNeadedResources = this.MBusNeadedResources;
      parameter.MBusParamConvertion = this.MBusParamConvertion;
      parameter.LoggerID = this.LoggerID;
      parameter.MBusParameterOverride = this.MBusParameterOverride;
      parameter.NameTranslated = this.NameTranslated;
      parameter.ParameterInfo = this.ParameterInfo;
      parameter.StructureNr = this.StructureNr;
      parameter.StructureIndex = this.StructureIndex;
      parameter.Address = this.Address;
      if (this.LinkByteList != null)
        parameter.LinkByteList = (byte[]) this.LinkByteList.Clone();
      if (this.LinkByteComment != null)
        parameter.LinkByteComment = (string[]) this.LinkByteComment.Clone();
      return parameter;
    }

    internal bool UpdateByteList()
    {
      if (!this.ExistOnEprom)
        return true;
      if (this.LinkByteList == null || this.LinkByteList.Length != this.Size)
        this.LinkByteList = new byte[this.Size];
      for (int index = 0; index < this.Size; ++index)
        this.LinkByteList[index] = (byte) ((ulong) (this.ValueEprom >> 8 * index) & (ulong) byte.MaxValue);
      return true;
    }

    internal bool CopyToEprom(byte[] Eprom)
    {
      if (!this.ExistOnEprom)
        return false;
      for (int index = 0; index < this.LinkByteList.Length; ++index)
        Eprom[this.Address + index] = this.LinkByteList[index];
      return true;
    }

    internal void AddParameterToGroup(string GroupNamesString)
    {
      for (int index = 0; index < Parameter.ParameterGroupNames.Length; ++index)
      {
        if (GroupNamesString.IndexOf(Parameter.ParameterGroupNames[index]) >= 0)
          this.GroupMember[index] = true;
      }
    }

    internal bool SetDifVifValues(string DifVifString)
    {
      SortedList sortedList = (SortedList) null;
      this.DifVifs = 0L;
      this.DifVifSize = (short) 0;
      try
      {
        string str1 = DifVifString;
        char[] chArray = new char[1]{ '|' };
        foreach (string str2 in str1.Split(chArray))
        {
          string str3 = str2.Trim();
          if (str3.Length >= 1)
          {
            if (sortedList == null)
              sortedList = new SortedList();
            string[] strArray = str3.Split(' ');
            string key = string.Empty;
            byte[] numArray = (byte[]) null;
            int num = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
              if (index == 0)
              {
                if (strArray[index].StartsWith("R:"))
                {
                  key = strArray[0].Substring(2);
                  numArray = new byte[strArray.Length - 1];
                  sortedList.Add((object) key, (object) numArray);
                  continue;
                }
                numArray = new byte[strArray.Length];
                sortedList.Add((object) key, (object) numArray);
              }
              if (!strArray[index].StartsWith("0x"))
                return false;
              numArray[num++] = byte.Parse(strArray[index].Substring(2), NumberStyles.HexNumber);
            }
            if (key == string.Empty)
            {
              this.DifVifs = 0L;
              this.DifVifSize = (short) numArray.Length;
              for (int index = 0; index < numArray.Length; ++index)
                this.DifVifs += (long) numArray[index] << 8 * index;
            }
          }
          else
            break;
        }
      }
      catch
      {
        return false;
      }
      if (sortedList != null && sortedList.Count > 1)
        this.DifVifsByRes = sortedList;
      return true;
    }

    internal enum ParamStorageType
    {
      VALUE,
      PREPAIDFILE,
      INTERVALPOINT,
      INTERVAL,
      INTERVALOFFSET,
      STARTADDRESS,
      ENDADDRESS,
      WRITEPTR,
      FLAGS,
      COUNTERVAR,
      BYTEARRAY,
      TIMEPOINT,
    }

    public enum ParameterGroups
    {
      All,
      IDENT,
      CALIB,
      SHOWMBUS,
      CONSUMATION,
      EXTERNAL_IDENT,
    }

    internal enum MBusParameterOverrideType
    {
      None,
      Energy,
      Volume,
      Flow,
      Power,
      INPUT_1,
      INPUT_2,
    }

    internal enum MBusParameterConversion
    {
      None,
      Date,
      DateTime,
    }

    public enum BaseParameterFormat
    {
      Integer,
      BCD,
      Date,
      DateTime,
      TimeSpan,
    }
  }
}
