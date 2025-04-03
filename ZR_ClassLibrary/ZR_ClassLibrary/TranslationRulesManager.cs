// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.TranslationRulesManager
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace ZR_ClassLibrary
{
  public class TranslationRulesManager
  {
    private static Logger logger = LogManager.GetLogger(nameof (TranslationRulesManager));
    private TranslationRuleCollection cachedRules;
    private static TranslationRulesManager _instance;

    public event EventHandlerEx<string> MissedTranslationRules;

    private TranslationRulesManager() => this.cachedRules = new TranslationRuleCollection();

    public static TranslationRulesManager Instance
    {
      get
      {
        if (TranslationRulesManager._instance == null)
          TranslationRulesManager._instance = new TranslationRulesManager();
        return TranslationRulesManager._instance;
      }
      set => TranslationRulesManager._instance = value;
    }

    public TranslationRuleCollection LoadRules(bool useCache)
    {
      if (!useCache)
        this.cachedRules.Clear();
      return this.LoadRules(string.Empty, string.Empty, new int?(), new int?());
    }

    public TranslationRuleCollection LoadRules(string zdf, bool loadDefaultRules)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string parameter1;
      string parameter2;
      string parameter3;
      if (zdf.Contains("SEC_MAN") && zdf.Contains("SEC_VER") && zdf.Contains("SEC_MED"))
      {
        parameter1 = ParameterService.GetParameter(zdf, "SEC_MAN");
        parameter2 = ParameterService.GetParameter(zdf, "SEC_MED");
        parameter3 = ParameterService.GetParameter(zdf, "SEC_VER");
      }
      else
      {
        parameter1 = ParameterService.GetParameter(zdf, "MAN");
        parameter2 = ParameterService.GetParameter(zdf, "MED");
        parameter3 = ParameterService.GetParameter(zdf, "GEN");
      }
      string manufacturer = loadDefaultRules ? "DEFAULT" : parameter1;
      int? nullable = new int?();
      if (!string.IsNullOrEmpty(parameter3))
        nullable = new int?(int.Parse(parameter3));
      return this.LoadRules(manufacturer, parameter2, nullable, nullable);
    }

    public TranslationRuleCollection LoadRules(
      string manufacturer,
      string medium,
      int? versionMin,
      int? versionMax)
    {
      if (this.cachedRules.Count == 0)
      {
        List<TranslationRule> collection = MeterDatabase.LoadTranslationRules();
        if (collection != null && collection.Count > 0)
          this.cachedRules.AddRange((IEnumerable<TranslationRule>) collection);
      }
      return this.cachedRules.GetRules(manufacturer, medium, versionMin, versionMax);
    }

    private TranslationRuleCollection LoadSubDeviceRules(string zdf)
    {
      return this.LoadSubDeviceRules(zdf, new int?());
    }

    private TranslationRuleCollection LoadSubDeviceRules(string zdf, int? subDeviceIndex)
    {
      if (string.IsNullOrEmpty(zdf))
        return (TranslationRuleCollection) null;
      if (this.cachedRules.Count == 0)
      {
        List<TranslationRule> collection = MeterDatabase.LoadTranslationRules();
        if (collection != null && collection.Count > 0)
          this.cachedRules.AddRange((IEnumerable<TranslationRule>) collection);
      }
      string parameter1 = ParameterService.GetParameter(zdf, "MAN");
      string parameter2 = ParameterService.GetParameter(zdf, "GEN");
      return string.IsNullOrEmpty(parameter1) || string.IsNullOrEmpty(parameter2) || !Util.IsNumeric((object) parameter2) ? (TranslationRuleCollection) null : this.cachedRules.GetSubDeviceRules(parameter1, int.Parse(parameter2), subDeviceIndex);
    }

    public bool UpdateRule(TranslationRule oldRule, TranslationRule newRule)
    {
      return MeterDatabase.UpdateTranslationRule(oldRule, newRule) && this.LoadRules(false) != null;
    }

    public bool CreateRule(TranslationRule newRule)
    {
      if (!MeterDatabase.AddTranslationRule(newRule))
        return false;
      this.cachedRules.Add(newRule);
      return true;
    }

    public bool DeleteRule(TranslationRule oldRule)
    {
      if (DbBasis.PrimaryDB == null || oldRule == null || !MeterDatabase.DeleteTranslationRule(oldRule))
        return false;
      this.cachedRules.Remove(oldRule);
      return true;
    }

    public bool TryParse(
      string zdf,
      int subDeviceIndex,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (string.IsNullOrEmpty(zdf))
        return false;
      if (valueList == null)
        valueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      return subDeviceIndex == 0 ? this.TryParseMainDevice(zdf, ref valueList) : this.TryParseSubDevice(zdf, ref valueList, subDeviceIndex);
    }

    private bool TryParseMainDevice(
      string zdf,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      List<long> filter = (List<long>) null;
      if (valueList.Count > 0)
      {
        filter = new List<long>((IEnumerable<long>) valueList.Keys);
        valueList.Clear();
      }
      TranslationRuleCollection translationRuleCollection = this.LoadRules(zdf, false);
      if (translationRuleCollection == null || translationRuleCollection.Count == 0)
      {
        translationRuleCollection = this.LoadRules(zdf, true);
        if (translationRuleCollection == null || translationRuleCollection.Count == 0)
        {
          if (this.MissedTranslationRules != null)
            this.MissedTranslationRules((object) this, zdf);
          return false;
        }
      }
      foreach (TranslationRule rule in (List<TranslationRule>) translationRuleCollection)
      {
        if (rule.SubDeviceIndex <= 0)
        {
          string parameter = ParameterService.GetParameter(zdf, rule.MBusZDF);
          if (!string.IsNullOrEmpty(parameter) && ValueIdent.IsExpectedValueIdent(filter, rule.ValueIdent))
          {
            DateTime? correctTimePoint = this.GetCorrectTimePoint(zdf, rule);
            if (correctTimePoint.HasValue)
            {
              if (rule.SpecialTranslation != 0)
              {
                if (!this.SpecialTranslation(rule, parameter, correctTimePoint.Value, ref valueList))
                  ;
              }
              else
              {
                double num;
                try
                {
                  num = double.Parse(parameter, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
                }
                catch
                {
                  continue;
                }
                double y = num * rule.Multiplier;
                ReadingValue readingValue = new ReadingValue();
                readingValue.state = ReadingValueState.ok;
                readingValue.StateDetails = string.Empty;
                readingValue.value = y;
                if (valueList.ContainsKey(rule.ValueIdent))
                {
                  if (valueList[rule.ValueIdent].ContainsKey(correctTimePoint.Value))
                  {
                    if (!Util.AreEqual(valueList[rule.ValueIdent][correctTimePoint.Value].value, y))
                      TranslationRulesManager.logger.Error<long, DateTime?, double>("Duplication values detected! ValueIdent: {0}, TimePoint: {1}, Value: {2}", rule.ValueIdent, correctTimePoint, y);
                  }
                  else
                    valueList[rule.ValueIdent].Add(correctTimePoint.Value, readingValue);
                }
                else if (rule.ValueIdent == 0L)
                  TranslationRulesManager.logger.Error("Wrong rule detected! Value: " + rule.ToString());
                else
                  valueList.Add(rule.ValueIdent, new SortedList<DateTime, ReadingValue>(1)
                  {
                    {
                      correctTimePoint.Value,
                      readingValue
                    }
                  });
              }
            }
          }
        }
      }
      ValueIdent.CleanUpEmptyValueIdents(valueList);
      if (valueList.Count != 0)
        return true;
      if (this.MissedTranslationRules != null)
        this.MissedTranslationRules((object) this, zdf);
      return false;
    }

    private bool TryParseSubDevice(
      string zdf,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      int subDeviceIndex)
    {
      TranslationRuleCollection translationRuleCollection = this.LoadSubDeviceRules(zdf, new int?(subDeviceIndex));
      if (translationRuleCollection == null || translationRuleCollection.Count == 0)
      {
        if (this.MissedTranslationRules != null)
          this.MissedTranslationRules((object) this, zdf);
        return false;
      }
      string valueOfSubDevice1 = TranslationRulesManager.TryGetSpecialTranslationValueOfSubDevice(zdf, subDeviceIndex, ZR_ClassLibrary.SpecialTranslation.Serialnumber);
      if (string.IsNullOrEmpty(valueOfSubDevice1))
        return false;
      string valueOfSubDevice2 = TranslationRulesManager.TryGetSpecialTranslationValueOfSubDevice(zdf, subDeviceIndex, ZR_ClassLibrary.SpecialTranslation.MeterType);
      if (string.IsNullOrEmpty(valueOfSubDevice2))
        return false;
      string parameter1 = ParameterService.GetParameter(zdf, "MAN");
      string parameter2 = ParameterService.GetParameter(zdf, "GEN");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SID;").Append(valueOfSubDevice1).Append(";");
      stringBuilder.Append("MAN;").Append(parameter1).Append(";");
      stringBuilder.Append("GEN;").Append(parameter2).Append(";");
      stringBuilder.Append("MED;").Append(valueOfSubDevice2).Append(";");
      string str1 = stringBuilder.ToString();
      foreach (TranslationRule translationRule in (List<TranslationRule>) translationRuleCollection)
      {
        if (translationRule.SubDeviceIndex == subDeviceIndex && translationRule.SpecialTranslation == 0)
        {
          string parameter3 = ParameterService.GetParameter(zdf, translationRule.MBusZDF);
          if (!string.IsNullOrEmpty(parameter3))
          {
            string str2 = string.IsNullOrEmpty(translationRule.StorageTimeParam) ? "RTIME" : translationRule.StorageTimeParam;
            string parameter4 = ParameterService.GetParameter(zdf, str2);
            if (!string.IsNullOrEmpty(parameter3))
            {
              str1 = ParameterService.AddOrUpdateParameter(str1, translationRule.SubDeviceAttributeIdentifier, parameter3);
              str1 = ParameterService.AddOrUpdateParameter(str1, str2, parameter4);
            }
          }
        }
      }
      return this.TryParseMainDevice(str1, ref valueList);
    }

    public static List<GlobalDeviceId> GetSubDevices(string zdf)
    {
      if (string.IsNullOrEmpty(zdf) || zdf == "NoParameter")
        return (List<GlobalDeviceId>) null;
      List<GlobalDeviceId> subDevices = new List<GlobalDeviceId>();
      TranslationRuleCollection translationRuleCollection = TranslationRulesManager.Instance.LoadSubDeviceRules(zdf);
      if (translationRuleCollection == null || translationRuleCollection.Count == 0)
        return (List<GlobalDeviceId>) null;
      foreach (TranslationRule translationRule in (List<TranslationRule>) translationRuleCollection)
      {
        TranslationRule rule = translationRule;
        if (zdf.IndexOf(rule.MBusZDF) != -1 && rule.SubDeviceIndex != 0)
        {
          GlobalDeviceId globalDeviceId1 = subDevices.Find((Predicate<GlobalDeviceId>) (e => Convert.ToInt32(e.MeterNumber) == rule.SubDeviceIndex));
          if (globalDeviceId1 == null)
          {
            globalDeviceId1 = new GlobalDeviceId();
            globalDeviceId1.Manufacturer = rule.Manufacturer;
            GlobalDeviceId globalDeviceId2 = globalDeviceId1;
            int num = rule.SubDeviceIndex;
            string str1 = num.ToString();
            globalDeviceId2.MeterNumber = str1;
            GlobalDeviceId globalDeviceId3 = globalDeviceId1;
            num = rule.VersionMax;
            string str2 = num.ToString();
            globalDeviceId3.Generation = str2;
            subDevices.Add(globalDeviceId1);
          }
          if (rule.SpecialTranslation == ZR_ClassLibrary.SpecialTranslation.Serialnumber)
            globalDeviceId1.Serialnumber = ParameterService.GetParameter(zdf, rule.MBusZDF);
          if (rule.SpecialTranslation == ZR_ClassLibrary.SpecialTranslation.MeterType)
          {
            string parameter = ParameterService.GetParameter(zdf, rule.MBusZDF);
            if (Enum.IsDefined(typeof (MBusDeviceType), (object) parameter))
            {
              MBusDeviceType mbusMedium = (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), parameter, true);
              globalDeviceId1.MeterType = ValueIdent.ConvertToMeterType(mbusMedium);
              globalDeviceId1.DeviceTypeName = parameter;
            }
          }
        }
      }
      return subDevices;
    }

    private DateTime? GetCorrectTimePoint(string zdf, TranslationRule rule)
    {
      if (string.IsNullOrEmpty(zdf))
        return new DateTime?();
      string ParameterName = "RTIME";
      if (!string.IsNullOrEmpty(rule.StorageTimeParam))
        ParameterName = rule.StorageTimeParam;
      string parameter = ParameterService.GetParameter(zdf, ParameterName);
      if (string.IsNullOrEmpty(parameter))
        return new DateTime?();
      try
      {
        DateTime dateTime = DateTime.Parse(parameter, (IFormatProvider) FixedFormates.TheFormates.DateTimeFormat, DateTimeStyles.None);
        switch (rule.StorageTimeTranslation)
        {
          case SpecialStorageTimeTranslation.None:
            return new DateTime?(dateTime);
          case SpecialStorageTimeTranslation.SetCurrentYear:
            return new DateTime?(new DateTime(DateTime.Now.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second));
          case SpecialStorageTimeTranslation.MinusOneYear:
            return new DateTime?(dateTime.AddYears(-1));
          case SpecialStorageTimeTranslation.DefaultDueDate:
            return new DateTime?(new DateTime(dateTime.Year, 1, 1));
          case SpecialStorageTimeTranslation.LastMonth:
            return new DateTime?(new DateTime(dateTime.Year, dateTime.Month, 1));
          default:
            throw new NotImplementedException(rule.StorageTimeTranslation.ToString());
        }
      }
      catch (Exception ex)
      {
        TranslationRulesManager.logger.Error(ex.Message);
        return new DateTime?();
      }
    }

    private bool SpecialTranslation(
      TranslationRule rule,
      string value,
      DateTime timePoint,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      if (rule == null || string.IsNullOrEmpty(value))
        return false;
      switch (rule.SpecialTranslation)
      {
        case ZR_ClassLibrary.SpecialTranslation.DigiInput:
          return true;
        case ZR_ClassLibrary.SpecialTranslation.BitIndex:
          return !(value == "00000000") || true;
        case ZR_ClassLibrary.SpecialTranslation.BitMask:
          try
          {
            if ((Convert.ToUInt32(value, 16) & Convert.ToUInt32(rule.Multiplier)) > 0U)
            {
              valueList.Add(rule.ValueIdent, new SortedList<DateTime, ReadingValue>());
              valueList[rule.ValueIdent].Add(timePoint, new ReadingValue()
              {
                value = 1.0
              });
            }
            return true;
          }
          catch
          {
            return false;
          }
        case ZR_ClassLibrary.SpecialTranslation.NumberNibble1:
        case ZR_ClassLibrary.SpecialTranslation.NumberNibble2:
          int num = 0;
          if (rule.SpecialTranslation == ZR_ClassLibrary.SpecialTranslation.NumberNibble2)
            num = 4;
          try
          {
            if ((double) (byte) (Convert.ToUInt32(value, 16) >> num & 15U) == rule.Multiplier)
            {
              valueList.Add(rule.ValueIdent, new SortedList<DateTime, ReadingValue>());
              valueList[rule.ValueIdent].Add(timePoint, new ReadingValue()
              {
                value = 1.0
              });
            }
            return true;
          }
          catch
          {
            return false;
          }
        default:
          return false;
      }
    }

    public static long GetValueIdent(
      string manufacturer,
      string medium,
      int version,
      string zdfKey)
    {
      TranslationRule rule = new TranslationRulesManager().TryGetRule(manufacturer, medium, version, zdfKey);
      return rule != null ? Convert.ToInt64(rule.ValueIdent) : -1L;
    }

    public TranslationRule TryGetRule(
      string manufacturer,
      string medium,
      int version,
      string zdfKey)
    {
      TranslationRuleCollection translationRuleCollection = this.LoadRules(manufacturer, medium, new int?(version), new int?(version));
      if (translationRuleCollection == null)
        return (TranslationRule) null;
      foreach (TranslationRule rule in (List<TranslationRule>) translationRuleCollection)
      {
        if (rule.Manufacturer == manufacturer && rule.Medium == medium && rule.MBusZDF == zdfKey)
          return rule;
      }
      return (TranslationRule) null;
    }

    public static string TryGetSpecialTranslationValueOfSubDevice(
      string zdf,
      int subDeviceIndex,
      ZR_ClassLibrary.SpecialTranslation specialTranslation)
    {
      string translationKeyOfSubDevice = TranslationRulesManager.TryGetSpecialTranslationKeyOfSubDevice(zdf, subDeviceIndex, specialTranslation);
      return translationKeyOfSubDevice == null ? (string) null : ParameterService.GetParameter(zdf, translationKeyOfSubDevice);
    }

    public static string TryGetSpecialTranslationKeyOfSubDevice(
      string zdf,
      int subDeviceIndex,
      ZR_ClassLibrary.SpecialTranslation specialTranslation)
    {
      TranslationRuleCollection translationRuleCollection = TranslationRulesManager.Instance.LoadSubDeviceRules(zdf, new int?(subDeviceIndex));
      if (translationRuleCollection == null || translationRuleCollection.Count == 0)
        return (string) null;
      TranslationRule translationRule = translationRuleCollection.Find((Predicate<TranslationRule>) (e => e.SpecialTranslation == specialTranslation));
      if (translationRule != null)
        return translationRule.MBusZDF;
      string str = string.Format("Can not find translation rule of {0} by sub device! SubDeviceIndex: {1} ZDF: {2}", (object) specialTranslation, (object) subDeviceIndex, (object) zdf);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
      TranslationRulesManager.logger.Error(str);
      return (string) null;
    }

    public static void ImportRulesIntoDatabase(string fileName)
    {
      if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
        return;
      using (StreamReader streamReader = new StreamReader(fileName))
      {
        TranslationRuleCollection translationRuleCollection = (TranslationRuleCollection) new XmlSerializer(typeof (TranslationRuleCollection)).Deserialize((TextReader) streamReader);
        if (translationRuleCollection == null || translationRuleCollection.Count == 0)
          return;
        foreach (TranslationRule loadTranslationRule in MeterDatabase.LoadTranslationRules())
          MeterDatabase.DeleteTranslationRule(loadTranslationRule);
        foreach (TranslationRule newRule in (List<TranslationRule>) translationRuleCollection)
          MeterDatabase.AddTranslationRule(newRule);
      }
    }
  }
}
