// Decompiled with JetBrains decompiler
// Type: S3_Handler.MeterResources
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class MeterResources
  {
    private static Logger logger = LogManager.GetLogger(nameof (MeterResources));
    private S3_Meter MyMeter;
    internal SortedList<string, int> AvailableMeterResources = new SortedList<string, int>();
    private StringBuilder ResourceEvents;
    internal int RemoveObjectCounter;

    private MeterResources()
    {
    }

    internal MeterResources(S3_Meter MyMeter)
    {
      this.MyMeter = MyMeter;
      this.SetRescourcesFromHardwareTypes();
      this.SetRescourcesFromParameter();
    }

    internal bool ReloadResources()
    {
      MeterResources.logger.Debug("Start ReloadResources " + Environment.NewLine + this?.ToString());
      try
      {
        this.AvailableMeterResources.Clear();
        return this.SetRescourcesFromHardwareTypes() && this.AddResource(this.MyMeter.MyFunctionManager.GetSetResources()) && this.SetRescourcesFromParameter();
      }
      finally
      {
        MeterResources.logger.Debug("Resouces after ReloadResources: " + Environment.NewLine + this?.ToString());
      }
    }

    internal MeterResources Clone(S3_Meter theCloneMeter)
    {
      MeterResources meterResources = new MeterResources();
      meterResources.MyMeter = theCloneMeter;
      foreach (KeyValuePair<string, int> availableMeterResource in this.AvailableMeterResources)
        meterResources.AvailableMeterResources.Add(availableMeterResource.Key, availableMeterResource.Value);
      return meterResources;
    }

    internal bool SetRescourcesFromHardwareTypes()
    {
      if (this.MyMeter.MyIdentification.HardwareResources == null)
        return true;
      foreach (string key in (IEnumerable<string>) this.MyMeter.MyIdentification.HardwareResources.Keys)
        this.AvailableMeterResources.Add(key, 1);
      if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.MBus.ToString()) || this.AvailableMeterResources.ContainsKey(S3_MeterResources.RS485.ToString()) || this.AvailableMeterResources.ContainsKey(S3_MeterResources.RS232.ToString()))
        this.AvailableMeterResources.Add(S3_MeterResources.Bus.ToString(), 1);
      return true;
    }

    internal bool SetRescourcesFromParameter()
    {
      ushort ushortValue = this.MyMeter.MyParameters.ParameterByName["Device_Setup"].GetUshortValue();
      bool flag1 = ((uint) ushortValue & 256U) > 0U;
      bool flag2 = ((uint) ushortValue & 512U) > 0U;
      this.AvailableMeterResources.Add(S3_MeterResources.Volume_Active.ToString(), 0);
      bool flag3 = false;
      if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.Heating.ToString()) & flag1)
      {
        this.AvailableMeterResources.Add(S3_MeterResources.Heating_Active.ToString(), 0);
        flag3 = true;
      }
      if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.Cooling.ToString()) & flag2)
      {
        this.AvailableMeterResources.Add(S3_MeterResources.Cooling_Active.ToString(), 0);
        if (!flag3)
          this.AvailableMeterResources.Add(S3_MeterResources.OnlyCooling_Active.ToString(), 0);
        flag3 = true;
      }
      if (flag3)
        this.AvailableMeterResources.Add(S3_MeterResources.Energy_Active.ToString(), 0);
      if (!this.MyMeter.MyIdentification.IsLoRa)
      {
        if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.InOut_1.ToString()))
        {
          if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_Meter.inputFactor[0]))
            this.DeleteAllInputResources(0);
          else if (this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[0]].GetUshortValue() > (ushort) 0)
          {
            this.DeleteAllInputOutputResources(0);
            this.AvailableMeterResources.Add(S3_MeterResources.IO_1_Input.ToString(), 0);
          }
        }
        else
          this.DeleteAllInputOutputResources(0);
        if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.InOut_2.ToString()))
        {
          if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_Meter.inputFactor[1]))
            this.DeleteAllInputResources(1);
          else if (this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[1]].GetUshortValue() > (ushort) 0)
          {
            this.DeleteAllInputOutputResources(1);
            this.AvailableMeterResources.Add(S3_MeterResources.IO_2_Input.ToString(), 0);
          }
        }
        else
          this.DeleteAllInputOutputResources(1);
        if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.InOut_3.ToString()))
        {
          if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_Meter.inputFactor[2]))
            this.DeleteAllInputResources(2);
          else if (this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[2]].GetUshortValue() > (ushort) 0)
          {
            this.DeleteAllInputOutputResources(2);
            this.AvailableMeterResources.Add(S3_MeterResources.IO_3_Input.ToString(), 0);
          }
        }
        else
          this.DeleteAllInputOutputResources(2);
      }
      else
      {
        if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.InOut_1.ToString()))
          this.DeleteAllInputResources(0);
        else
          this.DeleteAllInputOutputResources(0);
        if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.InOut_2.ToString()))
          this.DeleteAllInputResources(1);
        else
          this.DeleteAllInputOutputResources(1);
        if (this.AvailableMeterResources.ContainsKey(S3_MeterResources.InOut_3.ToString()))
          this.DeleteAllInputResources(2);
        else
          this.DeleteAllInputOutputResources(2);
      }
      return true;
    }

    internal bool AddResource(string absoluteResourceName)
    {
      if (absoluteResourceName == null || this.AvailableMeterResources.IndexOfKey(absoluteResourceName) >= 0)
        return false;
      this.AvailableMeterResources.Add(absoluteResourceName, 0);
      return true;
    }

    internal bool AddResource(List<string> list)
    {
      if (list == null)
        return false;
      foreach (string key in list)
      {
        if (!this.AvailableMeterResources.ContainsKey(key))
          this.AvailableMeterResources.Add(key, 0);
      }
      return true;
    }

    internal bool AddResource(string resourceName, int virtualDeviceIndex)
    {
      return this.AddResource(MeterResources.GetVirtualDeviceString(virtualDeviceIndex) + resourceName);
    }

    internal bool DeleteResource(string resourceName) => this.DeleteResource(resourceName, 0);

    internal bool DeleteResource(string resourceName, int virtualDeviceIndex)
    {
      string key = MeterResources.GetVirtualDeviceString(virtualDeviceIndex) + resourceName;
      int index = this.AvailableMeterResources.IndexOfKey(key);
      if (index < 0)
        return false;
      MeterResources.logger.Trace("Remove ressource: " + key);
      this.AvailableMeterResources.RemoveAt(index);
      return true;
    }

    internal bool UseResource(string resourceName, int virtualDeviceIndex)
    {
      int index = this.AvailableMeterResources.IndexOfKey(MeterResources.GetVirtualDeviceString(virtualDeviceIndex) + resourceName);
      if (index < 0)
        return false;
      this.AvailableMeterResources.Values[index]++;
      return true;
    }

    internal bool IsResourceAvailable(string resourceName, int virtualDeviceIndex)
    {
      if (this.MyMeter.MyFunctions.baseTypeEditMode || resourceName == null || resourceName.Length == 0)
        return true;
      if (!resourceName.Contains<char>(';'))
        return this.IsResourceAvailableNoCheck(resourceName, virtualDeviceIndex);
      string str = resourceName;
      char[] chArray = new char[1]{ ';' };
      foreach (string removeEmptyEntry in Util.RemoveEmptyEntries(str.Split(chArray)))
      {
        if (!this.IsResourceAvailableNoCheck(removeEmptyEntry, virtualDeviceIndex))
          return false;
      }
      return true;
    }

    private bool IsResourceAvailableNoCheck(string resourceName, int virtualDeviceIndex)
    {
      return this.AvailableMeterResources.IndexOfKey(virtualDeviceIndex <= 0 ? resourceName : MeterResources.GetVirtualDeviceString(virtualDeviceIndex) + resourceName) >= 0;
    }

    internal static bool IsSingleResourceAvailableInMultiResourceString(
      string resourceString,
      string singleResourceName)
    {
      string str = ";" + singleResourceName + ";";
      return (";" + resourceString + ";").Contains(str);
    }

    internal List<string> GetInputOutputResources(int virtualDeviceIndex)
    {
      string str1 = (virtualDeviceIndex + 1).ToString();
      string key1 = "OutHW_" + str1;
      List<string> inputOutputResources = new List<string>();
      inputOutputResources.Add(InputOutputFunctions.BusControlled.ToString());
      if (!this.AvailableMeterResources.ContainsKey(key1))
        inputOutputResources.Add(InputOutputFunctions.Input.ToString());
      string str2 = "IO_" + str1 + "_";
      string str3 = str2 + "Output";
      foreach (string key2 in (IEnumerable<string>) this.AvailableMeterResources.Keys)
      {
        if (key2.StartsWith(str3))
        {
          string str4 = key2.Substring(str2.Length);
          inputOutputResources.Add(str4);
        }
      }
      return inputOutputResources;
    }

    internal void DeleteAllInputOutputResources(int virtualDeviceIndex)
    {
      string str = "IO_" + (virtualDeviceIndex + 1).ToString() + "_";
      for (int index = this.AvailableMeterResources.Count - 1; index >= 0; --index)
      {
        if (this.AvailableMeterResources.Keys[index].StartsWith(str))
          this.AvailableMeterResources.RemoveAt(index);
      }
    }

    internal void DeleteAllInputResources(int virtualDeviceIndex)
    {
      string str1 = "IO_" + (virtualDeviceIndex + 1).ToString() + "_";
      string str2 = str1 + "Output";
      for (int index = this.AvailableMeterResources.Count - 1; index >= 0; --index)
      {
        if (this.AvailableMeterResources.Keys[index].StartsWith(str1) && !this.AvailableMeterResources.Keys[index].StartsWith(str2))
          this.AvailableMeterResources.RemoveAt(index);
      }
    }

    internal bool IsResourceAvailable(string res)
    {
      return string.IsNullOrEmpty(res) || this.AvailableMeterResources.IndexOfKey(res) > -1;
    }

    internal bool AreAllResourcesAvailable(List<string> list, out string missedResource)
    {
      missedResource = (string) null;
      if (list == null || list.Count == 0)
        return false;
      foreach (string res in list)
      {
        if (!this.IsResourceAvailable(res))
        {
          missedResource = res;
          return false;
        }
      }
      return true;
    }

    internal void SetSelectedInOutResource(InputOutputFunctions theFunction, int virtualDeviceIndex)
    {
      this.DeleteAllInOutFunctionsResources(virtualDeviceIndex);
      this.AddResource(MeterResources.GetVirtualDeviceString(virtualDeviceIndex) + theFunction.ToString(), virtualDeviceIndex);
    }

    internal void DeleteAllInOutFunctionsResources(int virtualDeviceIndex)
    {
      string virtualDeviceString = MeterResources.GetVirtualDeviceString(virtualDeviceIndex);
      foreach (string str in Util.GetNamesOfEnum(typeof (InputOutputFunctions)))
        this.DeleteResource(virtualDeviceString + str, virtualDeviceIndex);
    }

    private static string GetVirtualDeviceString(int virtualDeviceIndex)
    {
      switch (virtualDeviceIndex)
      {
        case 0:
          return string.Empty;
        case 1:
          return "IO_1_";
        case 2:
          return "IO_2_";
        case 3:
          return "IO_3_";
        default:
          ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal virtual device number");
          return string.Empty;
      }
    }

    internal void ResourceEventsReset()
    {
      if (this.ResourceEvents == null)
        this.ResourceEvents = new StringBuilder();
      else
        this.ResourceEvents.Length = 0;
      this.RemoveObjectCounter = 0;
    }

    internal void ResourceEventDeleteObjectByMissedResource(
      string objectInfo,
      List<string> missedResources)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string missedResource in missedResources)
        stringBuilder.Append(missedResource).Append(", ");
      this.ResourceEventDeleteObjectByMissedResource(objectInfo, stringBuilder.ToString().TrimEnd(' ', ','));
    }

    internal void ResourceEventDeleteObjectByMissedResource(
      string objectInfo,
      string missedResource)
    {
      this.ResourceEvents.Append(objectInfo);
      this.ResourceEvents.Append("; missed resource: ");
      this.ResourceEvents.AppendLine(missedResource);
      ++this.RemoveObjectCounter;
      MeterResources.logger.Debug<string, string>("Remove: {0}, Missing resources: {1}", objectInfo, missedResource);
    }

    internal string GetResouceEventsInfo() => this.ResourceEvents.ToString();

    internal bool AreValid()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.AvailableMeterResources.Keys);
      List<string> all1 = stringList.FindAll((Predicate<string>) (e => e.StartsWith(MeterResources.GetVirtualDeviceString(1))));
      List<string> all2 = stringList.FindAll((Predicate<string>) (e => e.StartsWith(MeterResources.GetVirtualDeviceString(2))));
      List<string> all3 = stringList.FindAll((Predicate<string>) (e => e.StartsWith(MeterResources.GetVirtualDeviceString(3))));
      return (all1.Count == 0 || all1.Count == 1) && (all2.Count == 0 || all2.Count == 1) && (all3.Count == 0 || all3.Count == 1);
    }

    internal void CleanupExclusivesResources()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.AvailableMeterResources.Keys);
      List<string> all1 = stringList.FindAll((Predicate<string>) (e => e.StartsWith(MeterResources.GetVirtualDeviceString(1))));
      List<string> all2 = stringList.FindAll((Predicate<string>) (e => e.StartsWith(MeterResources.GetVirtualDeviceString(2))));
      List<string> all3 = stringList.FindAll((Predicate<string>) (e => e.StartsWith(MeterResources.GetVirtualDeviceString(3))));
      for (int index = 1; index < all1.Count; ++index)
        this.AvailableMeterResources.Remove(all1[index]);
      for (int index = 1; index < all2.Count; ++index)
        this.AvailableMeterResources.Remove(all2[index]);
      for (int index = 1; index < all3.Count; ++index)
        this.AvailableMeterResources.Remove(all3[index]);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, int> availableMeterResource in this.AvailableMeterResources)
        stringBuilder.Append((object) availableMeterResource).AppendLine(" ");
      return stringBuilder.ToString();
    }
  }
}
