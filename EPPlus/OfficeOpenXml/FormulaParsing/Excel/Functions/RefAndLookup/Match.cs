// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Match
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class Match : LookupFunction
  {
    public Match()
      : base((ValueMatcher) new WildCardValueMatcher(), new CompileResultFactory())
    {
    }

    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      object obj = arguments.ElementAt<FunctionArgument>(0).Value;
      string str = this.ArgToString(arguments, 1);
      RangeAddress rangeAddress = new RangeAddressFactory(context.ExcelDataProvider).Create(str);
      Match.MatchType matchType = this.GetMatchType(arguments);
      LookupArguments args = new LookupArguments(obj, str, 0, 0, false);
      LookupNavigator lookupNavigator = LookupNavigatorFactory.Create(this.GetLookupDirection(rangeAddress), args, context);
      int? nullable = new int?();
      do
      {
        int num = this.IsMatch(lookupNavigator.CurrentValue, obj);
        if (matchType == Match.MatchType.ClosestBelow && num >= 0)
        {
          if (!nullable.HasValue)
            ;
          return this.CreateResult((object) (num == 0 ? lookupNavigator.Index + 1 : lookupNavigator.Index), DataType.Integer);
        }
        if (matchType == Match.MatchType.ClosestAbove && num <= 0)
        {
          if (!nullable.HasValue)
            ;
          return this.CreateResult((object) (num == 0 ? lookupNavigator.Index + 1 : lookupNavigator.Index), DataType.Integer);
        }
        if (matchType == Match.MatchType.ExactMatch && num == 0)
          return this.CreateResult((object) (lookupNavigator.Index + 1), DataType.Integer);
        nullable = new int?(num);
      }
      while (lookupNavigator.MoveNext());
      return this.CreateResult((object) null, DataType.Integer);
    }

    private Match.MatchType GetMatchType(IEnumerable<FunctionArgument> arguments)
    {
      Match.MatchType matchType = Match.MatchType.ClosestBelow;
      if (arguments.Count<FunctionArgument>() > 2)
        matchType = (Match.MatchType) this.ArgToInt(arguments, 2);
      return matchType;
    }

    private enum MatchType
    {
      ClosestAbove = -1, // 0xFFFFFFFF
      ExactMatch = 0,
      ClosestBelow = 1,
    }
  }
}
