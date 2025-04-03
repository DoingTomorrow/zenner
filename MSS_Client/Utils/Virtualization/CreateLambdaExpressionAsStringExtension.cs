// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.Virtualization.CreateLambdaExpressionAsStringExtension
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using System;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.Utils.Virtualization
{
  public static class CreateLambdaExpressionAsStringExtension
  {
    public static string CreateLambdaExpressionAsString(
      this CompositeFilterDescriptorCollection collection)
    {
      string str = "1==1";
      if (collection.Count > 0)
        str += " && ";
      return str + collection.CreateFilterExpressionAsString();
    }

    private static string CreateFilterExpressionAsString(this IFilterDescriptor filter)
    {
      string expressionAsString = "";
      if (filter.GetType() == typeof (FilterDescriptor))
      {
        FilterDescriptor filterDescriptor1 = (FilterDescriptor) filter;
        if (filterDescriptor1.Value == OperatorValueFilterDescriptorBase.UnsetValue)
          return "";
        FilterDescriptor filterDescriptor2 = new FilterDescriptor();
        filterDescriptor2.IsCaseSensitive = filterDescriptor1.IsCaseSensitive;
        filterDescriptor2.Member = filterDescriptor1.Member;
        filterDescriptor2.MemberType = filterDescriptor1.MemberType;
        filterDescriptor2.Operator = filterDescriptor1.Operator;
        filterDescriptor2.Value = filterDescriptor1.Value;
        FilterDescriptor fx = filterDescriptor2;
        expressionAsString += CreateLambdaExpressionAsStringExtension.GenerateExpressionString(fx);
      }
      else
      {
        if (filter.GetType() == typeof (MemberColumnFilterDescriptor))
        {
          MemberColumnFilterDescriptor filterDescriptor3 = (MemberColumnFilterDescriptor) filter;
          IColumnFilterDescriptor filterDescriptor4 = (IColumnFilterDescriptor) filterDescriptor3;
          if (filterDescriptor4.FieldFilter.Filter1?.Value != null && filterDescriptor4.FieldFilter.Filter1.Value.ToString() != "")
          {
            FilterDescriptor filterDescriptor5 = new FilterDescriptor();
            filterDescriptor5.IsCaseSensitive = filterDescriptor4.FieldFilter.Filter1.IsCaseSensitive;
            filterDescriptor5.Member = filterDescriptor3.Member;
            filterDescriptor5.MemberType = filterDescriptor3.MemberType;
            filterDescriptor5.Operator = filterDescriptor4.FieldFilter.Filter1.Operator;
            filterDescriptor5.Value = filterDescriptor4.FieldFilter.Filter1.Value;
            FilterDescriptor fx = filterDescriptor5;
            expressionAsString += CreateLambdaExpressionAsStringExtension.GenerateExpressionString(fx);
          }
          if (filterDescriptor4.FieldFilter.Filter2 != null && filterDescriptor4.FieldFilter.Filter2.Value != null && filterDescriptor4.FieldFilter.Filter2.Value.ToString() != "")
          {
            if (!string.IsNullOrWhiteSpace(expressionAsString))
              expressionAsString += filterDescriptor4.FieldFilter.LogicalOperator == FilterCompositionLogicalOperator.And ? " && " : " || ";
            FilterDescriptor filterDescriptor6 = new FilterDescriptor();
            filterDescriptor6.IsCaseSensitive = filterDescriptor4.FieldFilter.Filter2.IsCaseSensitive;
            filterDescriptor6.Member = filterDescriptor3.Member;
            filterDescriptor6.MemberType = filterDescriptor3.MemberType;
            filterDescriptor6.Operator = filterDescriptor4.FieldFilter.Filter2.Operator;
            filterDescriptor6.Value = filterDescriptor4.FieldFilter.Filter2.Value;
            FilterDescriptor fx = filterDescriptor6;
            expressionAsString += CreateLambdaExpressionAsStringExtension.GenerateExpressionString(fx);
          }
          return expressionAsString;
        }
        if (!(filter is ICompositeFilterDescriptor filterDescriptor))
          return expressionAsString;
        for (int index = 0; index < filterDescriptor.FilterDescriptors.Count; ++index)
        {
          string str = " (" + filterDescriptor.FilterDescriptors[index].CreateFilterExpressionAsString() + ") ";
          if (index < filterDescriptor.FilterDescriptors.Count - 1 && !string.IsNullOrWhiteSpace(str))
            str += filterDescriptor.LogicalOperator == FilterCompositionLogicalOperator.And ? " && " : " || ";
          expressionAsString = expressionAsString + " " + str;
        }
      }
      return expressionAsString;
    }

    private static string GenerateExpressionString(FilterDescriptor fx)
    {
      string str1 = "null";
      if (fx.Value != null)
        str1 = !(fx.MemberType == typeof (DateTime)) && !(fx.MemberType == typeof (DateTime?)) ? (!(fx.MemberType == typeof (string)) && !(fx.MemberType == typeof (char)) ? fx.Value.ToString() : "\"" + fx.Value + "\"") : "\"" + ((DateTime) fx.Value).ToString("yyyy/MM/dd") + "\"";
      string member = fx.Member;
      string str2 = fx.Member + "!=null && ";
      if (!fx.IsCaseSensitive && (fx.MemberType == typeof (string) || fx.MemberType == typeof (char)))
      {
        member += ".ToLower()";
        str1 += ".ToLower()";
      }
      string str3 = fx.Operator < FilterOperator.StartsWith || fx.Operator > FilterOperator.IsNotContainedIn ? member + CreateLambdaExpressionAsStringExtension.TranslateOperator(fx.Operator) + str1 : member + string.Format(CreateLambdaExpressionAsStringExtension.TranslateOperator(fx.Operator), (object) str1);
      return str2 + str3;
    }

    private static string TranslateOperator(FilterOperator op)
    {
      switch (op)
      {
        case FilterOperator.IsLessThan:
          return "<";
        case FilterOperator.IsLessThanOrEqualTo:
          return "<=";
        case FilterOperator.IsEqualTo:
          return "==";
        case FilterOperator.IsNotEqualTo:
          return "!=";
        case FilterOperator.IsGreaterThanOrEqualTo:
          return ">=";
        case FilterOperator.IsGreaterThan:
          return ">";
        case FilterOperator.StartsWith:
          return ".StartsWith({0})";
        case FilterOperator.EndsWith:
          return ".EndsWith({0})";
        case FilterOperator.Contains:
          return ".Contains({0})";
        case FilterOperator.DoesNotContain:
          return ".Contains({0})==false";
        case FilterOperator.IsContainedIn:
          return ".Contains({0})";
        case FilterOperator.IsNotContainedIn:
          return ".Contains({0})==false";
        case FilterOperator.IsNull:
          return "==null";
        case FilterOperator.IsNotNull:
          return "!=null";
        case FilterOperator.IsEmpty:
          return "==\"\"";
        case FilterOperator.IsNotEmpty:
          return "!=\"\"";
        default:
          return "==";
      }
    }
  }
}
