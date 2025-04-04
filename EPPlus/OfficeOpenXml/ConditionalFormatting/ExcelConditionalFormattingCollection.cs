// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.ExcelConditionalFormattingCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using OfficeOpenXml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  public class ExcelConditionalFormattingCollection : 
    XmlHelper,
    IEnumerable<IExcelConditionalFormattingRule>,
    IEnumerable
  {
    private List<IExcelConditionalFormattingRule> _rules = new List<IExcelConditionalFormattingRule>();
    private ExcelWorksheet _worksheet;

    internal ExcelConditionalFormattingCollection(ExcelWorksheet worksheet)
      : base(worksheet.NameSpaceManager, (XmlNode) worksheet.WorksheetXml.DocumentElement)
    {
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      this._worksheet = worksheet;
      this.SchemaNodeOrder = this._worksheet.SchemaNodeOrder;
      XmlNodeList xmlNodeList = this.TopNode.SelectNodes("//d:conditionalFormatting", this._worksheet.NameSpaceManager);
      if (xmlNodeList == null || xmlNodeList.Count <= 0)
        return;
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        if (xmlNode.Attributes["sqref"] == null)
          throw new Exception("Missing 'sqref' attribute in Conditional Formatting");
        ExcelAddress address = new ExcelAddress(xmlNode.Attributes["sqref"].Value);
        foreach (XmlNode selectNode in xmlNode.SelectNodes("d:cfRule", this._worksheet.NameSpaceManager))
        {
          if (selectNode.Attributes["type"] == null)
            throw new Exception("Missing 'type' attribute in Conditional Formatting Rule");
          string attribute = selectNode.Attributes["priority"] != null ? ExcelConditionalFormattingHelper.GetAttributeString(selectNode, "type") : throw new Exception("Missing 'priority' attribute in Conditional Formatting Rule");
          int attributeInt = ExcelConditionalFormattingHelper.GetAttributeInt(selectNode, "priority");
          ExcelConditionalFormattingRule conditionalFormattingRule = ExcelConditionalFormattingRuleFactory.Create(ExcelConditionalFormattingRuleType.GetTypeByAttrbiute(attribute, selectNode, this._worksheet.NameSpaceManager), address, attributeInt, this._worksheet, selectNode);
          if (conditionalFormattingRule != null)
            this._rules.Add((IExcelConditionalFormattingRule) conditionalFormattingRule);
        }
      }
    }

    private void EnsureRootElementExists()
    {
      if (this._worksheet.WorksheetXml.DocumentElement == null)
        throw new Exception("Missing 'worksheet' node");
    }

    private XmlNode GetRootNode()
    {
      this.EnsureRootElementExists();
      return (XmlNode) this._worksheet.WorksheetXml.DocumentElement;
    }

    private ExcelAddress ValidateAddress(ExcelAddress address)
    {
      Require.Argument<ExcelAddress>(address).IsNotNull<ExcelAddress>(nameof (address));
      return address;
    }

    private int GetNextPriority()
    {
      int num = 0;
      foreach (IExcelConditionalFormattingRule rule in this._rules)
      {
        if (rule.Priority > num)
          num = rule.Priority;
      }
      return num + 1;
    }

    public int Count => this._rules.Count;

    public IExcelConditionalFormattingRule this[int index]
    {
      get => this._rules[index];
      set => this._rules[index] = value;
    }

    IEnumerator<IExcelConditionalFormattingRule> IEnumerable<IExcelConditionalFormattingRule>.GetEnumerator()
    {
      return (IEnumerator<IExcelConditionalFormattingRule>) this._rules.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._rules.GetEnumerator();

    public void RemoveAll()
    {
      foreach (XmlNode selectNode in this.TopNode.SelectNodes("//d:conditionalFormatting", this._worksheet.NameSpaceManager))
        selectNode.ParentNode.RemoveChild(selectNode);
      this._rules.Clear();
    }

    public void Remove(IExcelConditionalFormattingRule item)
    {
      Require.Argument<IExcelConditionalFormattingRule>(item).IsNotNull<IExcelConditionalFormattingRule>(nameof (item));
      try
      {
        XmlNode parentNode = item.Node.ParentNode;
        parentNode.RemoveChild(item.Node);
        if (!parentNode.HasChildNodes)
          parentNode.ParentNode.RemoveChild(parentNode);
        this._rules.Remove(item);
      }
      catch
      {
        throw new Exception("Invalid remove rule operation");
      }
    }

    public void RemoveAt(int index)
    {
      Require.Argument<int>(index).IsInRange<int>(0, this.Count - 1, nameof (index));
      this.Remove(this[index]);
    }

    public void RemoveByPriority(int priority)
    {
      try
      {
        this.Remove(this.RulesByPriority(priority));
      }
      catch
      {
      }
    }

    public IExcelConditionalFormattingRule RulesByPriority(int priority)
    {
      return this._rules.Find((Predicate<IExcelConditionalFormattingRule>) (x => x.Priority == priority));
    }

    internal IExcelConditionalFormattingRule AddRule(
      eExcelConditionalFormattingRuleType type,
      ExcelAddress address)
    {
      Require.Argument<ExcelAddress>(address).IsNotNull<ExcelAddress>(nameof (address));
      address = this.ValidateAddress(address);
      this.EnsureRootElementExists();
      IExcelConditionalFormattingRule conditionalFormattingRule = (IExcelConditionalFormattingRule) ExcelConditionalFormattingRuleFactory.Create(type, address, this.GetNextPriority(), this._worksheet, (XmlNode) null);
      this._rules.Add(conditionalFormattingRule);
      return conditionalFormattingRule;
    }

    public IExcelConditionalFormattingAverageGroup AddAboveAverage(ExcelAddress address)
    {
      return (IExcelConditionalFormattingAverageGroup) this.AddRule(eExcelConditionalFormattingRuleType.AboveAverage, address);
    }

    public IExcelConditionalFormattingAverageGroup AddAboveOrEqualAverage(ExcelAddress address)
    {
      return (IExcelConditionalFormattingAverageGroup) this.AddRule(eExcelConditionalFormattingRuleType.AboveOrEqualAverage, address);
    }

    public IExcelConditionalFormattingAverageGroup AddBelowAverage(ExcelAddress address)
    {
      return (IExcelConditionalFormattingAverageGroup) this.AddRule(eExcelConditionalFormattingRuleType.BelowAverage, address);
    }

    public IExcelConditionalFormattingAverageGroup AddBelowOrEqualAverage(ExcelAddress address)
    {
      return (IExcelConditionalFormattingAverageGroup) this.AddRule(eExcelConditionalFormattingRuleType.BelowOrEqualAverage, address);
    }

    public IExcelConditionalFormattingStdDevGroup AddAboveStdDev(ExcelAddress address)
    {
      return (IExcelConditionalFormattingStdDevGroup) this.AddRule(eExcelConditionalFormattingRuleType.AboveStdDev, address);
    }

    public IExcelConditionalFormattingStdDevGroup AddBelowStdDev(ExcelAddress address)
    {
      return (IExcelConditionalFormattingStdDevGroup) this.AddRule(eExcelConditionalFormattingRuleType.BelowStdDev, address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddBottom(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTopBottomGroup) this.AddRule(eExcelConditionalFormattingRuleType.Bottom, address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddBottomPercent(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTopBottomGroup) this.AddRule(eExcelConditionalFormattingRuleType.BottomPercent, address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddTop(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTopBottomGroup) this.AddRule(eExcelConditionalFormattingRuleType.Top, address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddTopPercent(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTopBottomGroup) this.AddRule(eExcelConditionalFormattingRuleType.TopPercent, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddLast7Days(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.Last7Days, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddLastMonth(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.LastMonth, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddLastWeek(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.LastWeek, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddNextMonth(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.NextMonth, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddNextWeek(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.NextWeek, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddThisMonth(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.ThisMonth, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddThisWeek(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.ThisWeek, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddToday(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.Today, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddTomorrow(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.Tomorrow, address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddYesterday(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTimePeriodGroup) this.AddRule(eExcelConditionalFormattingRuleType.Yesterday, address);
    }

    public IExcelConditionalFormattingBeginsWith AddBeginsWith(ExcelAddress address)
    {
      return (IExcelConditionalFormattingBeginsWith) this.AddRule(eExcelConditionalFormattingRuleType.BeginsWith, address);
    }

    public IExcelConditionalFormattingBetween AddBetween(ExcelAddress address)
    {
      return (IExcelConditionalFormattingBetween) this.AddRule(eExcelConditionalFormattingRuleType.Between, address);
    }

    public IExcelConditionalFormattingContainsBlanks AddContainsBlanks(ExcelAddress address)
    {
      return (IExcelConditionalFormattingContainsBlanks) this.AddRule(eExcelConditionalFormattingRuleType.ContainsBlanks, address);
    }

    public IExcelConditionalFormattingContainsErrors AddContainsErrors(ExcelAddress address)
    {
      return (IExcelConditionalFormattingContainsErrors) this.AddRule(eExcelConditionalFormattingRuleType.ContainsErrors, address);
    }

    public IExcelConditionalFormattingContainsText AddContainsText(ExcelAddress address)
    {
      return (IExcelConditionalFormattingContainsText) this.AddRule(eExcelConditionalFormattingRuleType.ContainsText, address);
    }

    public IExcelConditionalFormattingDuplicateValues AddDuplicateValues(ExcelAddress address)
    {
      return (IExcelConditionalFormattingDuplicateValues) this.AddRule(eExcelConditionalFormattingRuleType.DuplicateValues, address);
    }

    public IExcelConditionalFormattingEndsWith AddEndsWith(ExcelAddress address)
    {
      return (IExcelConditionalFormattingEndsWith) this.AddRule(eExcelConditionalFormattingRuleType.EndsWith, address);
    }

    public IExcelConditionalFormattingEqual AddEqual(ExcelAddress address)
    {
      return (IExcelConditionalFormattingEqual) this.AddRule(eExcelConditionalFormattingRuleType.Equal, address);
    }

    public IExcelConditionalFormattingExpression AddExpression(ExcelAddress address)
    {
      return (IExcelConditionalFormattingExpression) this.AddRule(eExcelConditionalFormattingRuleType.Expression, address);
    }

    public IExcelConditionalFormattingGreaterThan AddGreaterThan(ExcelAddress address)
    {
      return (IExcelConditionalFormattingGreaterThan) this.AddRule(eExcelConditionalFormattingRuleType.GreaterThan, address);
    }

    public IExcelConditionalFormattingGreaterThanOrEqual AddGreaterThanOrEqual(ExcelAddress address)
    {
      return (IExcelConditionalFormattingGreaterThanOrEqual) this.AddRule(eExcelConditionalFormattingRuleType.GreaterThanOrEqual, address);
    }

    public IExcelConditionalFormattingLessThan AddLessThan(ExcelAddress address)
    {
      return (IExcelConditionalFormattingLessThan) this.AddRule(eExcelConditionalFormattingRuleType.LessThan, address);
    }

    public IExcelConditionalFormattingLessThanOrEqual AddLessThanOrEqual(ExcelAddress address)
    {
      return (IExcelConditionalFormattingLessThanOrEqual) this.AddRule(eExcelConditionalFormattingRuleType.LessThanOrEqual, address);
    }

    public IExcelConditionalFormattingNotBetween AddNotBetween(ExcelAddress address)
    {
      return (IExcelConditionalFormattingNotBetween) this.AddRule(eExcelConditionalFormattingRuleType.NotBetween, address);
    }

    public IExcelConditionalFormattingNotContainsBlanks AddNotContainsBlanks(ExcelAddress address)
    {
      return (IExcelConditionalFormattingNotContainsBlanks) this.AddRule(eExcelConditionalFormattingRuleType.NotContainsBlanks, address);
    }

    public IExcelConditionalFormattingNotContainsErrors AddNotContainsErrors(ExcelAddress address)
    {
      return (IExcelConditionalFormattingNotContainsErrors) this.AddRule(eExcelConditionalFormattingRuleType.NotContainsErrors, address);
    }

    public IExcelConditionalFormattingNotContainsText AddNotContainsText(ExcelAddress address)
    {
      return (IExcelConditionalFormattingNotContainsText) this.AddRule(eExcelConditionalFormattingRuleType.NotContainsText, address);
    }

    public IExcelConditionalFormattingNotEqual AddNotEqual(ExcelAddress address)
    {
      return (IExcelConditionalFormattingNotEqual) this.AddRule(eExcelConditionalFormattingRuleType.NotEqual, address);
    }

    public IExcelConditionalFormattingUniqueValues AddUniqueValues(ExcelAddress address)
    {
      return (IExcelConditionalFormattingUniqueValues) this.AddRule(eExcelConditionalFormattingRuleType.UniqueValues, address);
    }

    public IExcelConditionalFormattingThreeColorScale AddThreeColorScale(ExcelAddress address)
    {
      return (IExcelConditionalFormattingThreeColorScale) this.AddRule(eExcelConditionalFormattingRuleType.ThreeColorScale, address);
    }

    public IExcelConditionalFormattingTwoColorScale AddTwoColorScale(ExcelAddress address)
    {
      return (IExcelConditionalFormattingTwoColorScale) this.AddRule(eExcelConditionalFormattingRuleType.TwoColorScale, address);
    }

    public IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting3IconsSetType> AddThreeIconSet(
      ExcelAddress Address,
      eExcelconditionalFormatting3IconsSetType IconSet)
    {
      IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting3IconsSetType> formattingThreeIconSet = (IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting3IconsSetType>) this.AddRule(eExcelConditionalFormattingRuleType.ThreeIconSet, Address);
      formattingThreeIconSet.IconSet = IconSet;
      return formattingThreeIconSet;
    }

    public IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting4IconsSetType> AddFourIconSet(
      ExcelAddress Address,
      eExcelconditionalFormatting4IconsSetType IconSet)
    {
      IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting4IconsSetType> formattingFourIconSet = (IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting4IconsSetType>) this.AddRule(eExcelConditionalFormattingRuleType.FourIconSet, Address);
      formattingFourIconSet.IconSet = IconSet;
      return formattingFourIconSet;
    }

    public IExcelConditionalFormattingFiveIconSet AddFiveIconSet(
      ExcelAddress Address,
      eExcelconditionalFormatting5IconsSetType IconSet)
    {
      IExcelConditionalFormattingFiveIconSet formattingFiveIconSet = (IExcelConditionalFormattingFiveIconSet) this.AddRule(eExcelConditionalFormattingRuleType.FiveIconSet, Address);
      formattingFiveIconSet.IconSet = IconSet;
      return formattingFiveIconSet;
    }

    public IExcelConditionalFormattingDataBarGroup AddDatabar(ExcelAddress Address, Color color)
    {
      IExcelConditionalFormattingDataBarGroup formattingDataBarGroup = (IExcelConditionalFormattingDataBarGroup) this.AddRule(eExcelConditionalFormattingRuleType.DataBar, Address);
      formattingDataBarGroup.Color = color;
      return formattingDataBarGroup;
    }
  }
}
