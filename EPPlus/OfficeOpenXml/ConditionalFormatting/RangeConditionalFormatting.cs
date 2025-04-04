// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ConditionalFormatting.RangeConditionalFormatting
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.ConditionalFormatting.Contracts;
using OfficeOpenXml.Utils;
using System.Drawing;

#nullable disable
namespace OfficeOpenXml.ConditionalFormatting
{
  internal class RangeConditionalFormatting : IRangeConditionalFormatting
  {
    public ExcelWorksheet _worksheet;
    public ExcelAddress _address;

    public RangeConditionalFormatting(ExcelWorksheet worksheet, ExcelAddress address)
    {
      Require.Argument<ExcelWorksheet>(worksheet).IsNotNull<ExcelWorksheet>(nameof (worksheet));
      Require.Argument<ExcelAddress>(address).IsNotNull<ExcelAddress>(nameof (address));
      this._worksheet = worksheet;
      this._address = address;
    }

    public IExcelConditionalFormattingAverageGroup AddAboveAverage()
    {
      return this._worksheet.ConditionalFormatting.AddAboveAverage(this._address);
    }

    public IExcelConditionalFormattingAverageGroup AddAboveOrEqualAverage()
    {
      return this._worksheet.ConditionalFormatting.AddAboveOrEqualAverage(this._address);
    }

    public IExcelConditionalFormattingAverageGroup AddBelowAverage()
    {
      return this._worksheet.ConditionalFormatting.AddBelowAverage(this._address);
    }

    public IExcelConditionalFormattingAverageGroup AddBelowOrEqualAverage()
    {
      return this._worksheet.ConditionalFormatting.AddBelowOrEqualAverage(this._address);
    }

    public IExcelConditionalFormattingStdDevGroup AddAboveStdDev()
    {
      return this._worksheet.ConditionalFormatting.AddAboveStdDev(this._address);
    }

    public IExcelConditionalFormattingStdDevGroup AddBelowStdDev()
    {
      return this._worksheet.ConditionalFormatting.AddBelowStdDev(this._address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddBottom()
    {
      return this._worksheet.ConditionalFormatting.AddBottom(this._address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddBottomPercent()
    {
      return this._worksheet.ConditionalFormatting.AddBottomPercent(this._address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddTop()
    {
      return this._worksheet.ConditionalFormatting.AddTop(this._address);
    }

    public IExcelConditionalFormattingTopBottomGroup AddTopPercent()
    {
      return this._worksheet.ConditionalFormatting.AddTopPercent(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddLast7Days()
    {
      return this._worksheet.ConditionalFormatting.AddLast7Days(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddLastMonth()
    {
      return this._worksheet.ConditionalFormatting.AddLastMonth(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddLastWeek()
    {
      return this._worksheet.ConditionalFormatting.AddLastWeek(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddNextMonth()
    {
      return this._worksheet.ConditionalFormatting.AddNextMonth(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddNextWeek()
    {
      return this._worksheet.ConditionalFormatting.AddNextWeek(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddThisMonth()
    {
      return this._worksheet.ConditionalFormatting.AddThisMonth(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddThisWeek()
    {
      return this._worksheet.ConditionalFormatting.AddThisWeek(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddToday()
    {
      return this._worksheet.ConditionalFormatting.AddToday(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddTomorrow()
    {
      return this._worksheet.ConditionalFormatting.AddTomorrow(this._address);
    }

    public IExcelConditionalFormattingTimePeriodGroup AddYesterday()
    {
      return this._worksheet.ConditionalFormatting.AddYesterday(this._address);
    }

    public IExcelConditionalFormattingBeginsWith AddBeginsWith()
    {
      return this._worksheet.ConditionalFormatting.AddBeginsWith(this._address);
    }

    public IExcelConditionalFormattingBetween AddBetween()
    {
      return this._worksheet.ConditionalFormatting.AddBetween(this._address);
    }

    public IExcelConditionalFormattingContainsBlanks AddContainsBlanks()
    {
      return this._worksheet.ConditionalFormatting.AddContainsBlanks(this._address);
    }

    public IExcelConditionalFormattingContainsErrors AddContainsErrors()
    {
      return this._worksheet.ConditionalFormatting.AddContainsErrors(this._address);
    }

    public IExcelConditionalFormattingContainsText AddContainsText()
    {
      return this._worksheet.ConditionalFormatting.AddContainsText(this._address);
    }

    public IExcelConditionalFormattingDuplicateValues AddDuplicateValues()
    {
      return this._worksheet.ConditionalFormatting.AddDuplicateValues(this._address);
    }

    public IExcelConditionalFormattingEndsWith AddEndsWith()
    {
      return this._worksheet.ConditionalFormatting.AddEndsWith(this._address);
    }

    public IExcelConditionalFormattingEqual AddEqual()
    {
      return this._worksheet.ConditionalFormatting.AddEqual(this._address);
    }

    public IExcelConditionalFormattingExpression AddExpression()
    {
      return this._worksheet.ConditionalFormatting.AddExpression(this._address);
    }

    public IExcelConditionalFormattingGreaterThan AddGreaterThan()
    {
      return this._worksheet.ConditionalFormatting.AddGreaterThan(this._address);
    }

    public IExcelConditionalFormattingGreaterThanOrEqual AddGreaterThanOrEqual()
    {
      return this._worksheet.ConditionalFormatting.AddGreaterThanOrEqual(this._address);
    }

    public IExcelConditionalFormattingLessThan AddLessThan()
    {
      return this._worksheet.ConditionalFormatting.AddLessThan(this._address);
    }

    public IExcelConditionalFormattingLessThanOrEqual AddLessThanOrEqual()
    {
      return this._worksheet.ConditionalFormatting.AddLessThanOrEqual(this._address);
    }

    public IExcelConditionalFormattingNotBetween AddNotBetween()
    {
      return this._worksheet.ConditionalFormatting.AddNotBetween(this._address);
    }

    public IExcelConditionalFormattingNotContainsBlanks AddNotContainsBlanks()
    {
      return this._worksheet.ConditionalFormatting.AddNotContainsBlanks(this._address);
    }

    public IExcelConditionalFormattingNotContainsErrors AddNotContainsErrors()
    {
      return this._worksheet.ConditionalFormatting.AddNotContainsErrors(this._address);
    }

    public IExcelConditionalFormattingNotContainsText AddNotContainsText()
    {
      return this._worksheet.ConditionalFormatting.AddNotContainsText(this._address);
    }

    public IExcelConditionalFormattingNotEqual AddNotEqual()
    {
      return this._worksheet.ConditionalFormatting.AddNotEqual(this._address);
    }

    public IExcelConditionalFormattingUniqueValues AddUniqueValues()
    {
      return this._worksheet.ConditionalFormatting.AddUniqueValues(this._address);
    }

    public IExcelConditionalFormattingThreeColorScale AddThreeColorScale()
    {
      return (IExcelConditionalFormattingThreeColorScale) this._worksheet.ConditionalFormatting.AddRule(eExcelConditionalFormattingRuleType.ThreeColorScale, this._address);
    }

    public IExcelConditionalFormattingTwoColorScale AddTwoColorScale()
    {
      return (IExcelConditionalFormattingTwoColorScale) this._worksheet.ConditionalFormatting.AddRule(eExcelConditionalFormattingRuleType.TwoColorScale, this._address);
    }

    public IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting3IconsSetType> AddThreeIconSet(
      eExcelconditionalFormatting3IconsSetType IconSet)
    {
      IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting3IconsSetType> formattingThreeIconSet = (IExcelConditionalFormattingThreeIconSet<eExcelconditionalFormatting3IconsSetType>) this._worksheet.ConditionalFormatting.AddRule(eExcelConditionalFormattingRuleType.ThreeIconSet, this._address);
      formattingThreeIconSet.IconSet = IconSet;
      return formattingThreeIconSet;
    }

    public IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting4IconsSetType> AddFourIconSet(
      eExcelconditionalFormatting4IconsSetType IconSet)
    {
      IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting4IconsSetType> formattingFourIconSet = (IExcelConditionalFormattingFourIconSet<eExcelconditionalFormatting4IconsSetType>) this._worksheet.ConditionalFormatting.AddRule(eExcelConditionalFormattingRuleType.FourIconSet, this._address);
      formattingFourIconSet.IconSet = IconSet;
      return formattingFourIconSet;
    }

    public IExcelConditionalFormattingFiveIconSet AddFiveIconSet(
      eExcelconditionalFormatting5IconsSetType IconSet)
    {
      IExcelConditionalFormattingFiveIconSet formattingFiveIconSet = (IExcelConditionalFormattingFiveIconSet) this._worksheet.ConditionalFormatting.AddRule(eExcelConditionalFormattingRuleType.FiveIconSet, this._address);
      formattingFiveIconSet.IconSet = IconSet;
      return formattingFiveIconSet;
    }

    public IExcelConditionalFormattingDataBarGroup AddDatabar(Color Color)
    {
      IExcelConditionalFormattingDataBarGroup formattingDataBarGroup = (IExcelConditionalFormattingDataBarGroup) this._worksheet.ConditionalFormatting.AddRule(eExcelConditionalFormattingRuleType.DataBar, this._address);
      formattingDataBarGroup.Color = Color;
      return formattingDataBarGroup;
    }
  }
}
