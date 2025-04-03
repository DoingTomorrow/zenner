// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.CustomFilterDescriptor
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.Utils
{
  public class CustomFilterDescriptor : FilterDescriptorBase
  {
    private readonly CompositeFilterDescriptor _compositeFilterDescriptor;
    private static readonly ConstantExpression TrueExpression = Expression.Constant((object) true);
    private string _filterValue;

    public CustomFilterDescriptor(IEnumerable<GridViewColumn> columns)
    {
      this._compositeFilterDescriptor = new CompositeFilterDescriptor()
      {
        LogicalOperator = FilterCompositionLogicalOperator.Or
      };
      foreach (GridViewDataColumn column in columns)
        this._compositeFilterDescriptor.FilterDescriptors.Add(this.CreateFilterForColumn(column));
    }

    public string FilterValue
    {
      get => this._filterValue;
      set
      {
        if (this._filterValue == value)
          return;
        this._filterValue = value;
        this.UpdateCompositeFilterValues();
        this.OnPropertyChanged(nameof (FilterValue));
      }
    }

    protected override Expression CreateFilterExpression(ParameterExpression parameterExpression)
    {
      if (string.IsNullOrEmpty(this.FilterValue))
        return (Expression) CustomFilterDescriptor.TrueExpression;
      try
      {
        return this._compositeFilterDescriptor.CreateFilterExpression((Expression) parameterExpression);
      }
      catch
      {
      }
      return (Expression) CustomFilterDescriptor.TrueExpression;
    }

    private IFilterDescriptor CreateFilterForColumn(GridViewDataColumn column)
    {
      FilterOperator filterOperatorForType = CustomFilterDescriptor.GetFilterOperatorForType(column.DataType);
      return (IFilterDescriptor) new FilterDescriptor(column.UniqueName, filterOperatorForType, (object) this._filterValue)
      {
        MemberType = column.DataType
      };
    }

    private static FilterOperator GetFilterOperatorForType(Type dataType)
    {
      return dataType == typeof (string) ? FilterOperator.Contains : FilterOperator.IsEqualTo;
    }

    private void UpdateCompositeFilterValues()
    {
      foreach (FilterDescriptor filterDescriptor in (Collection<IFilterDescriptor>) this._compositeFilterDescriptor.FilterDescriptors)
      {
        object obj;
        try
        {
          obj = Convert.ChangeType((object) this.FilterValue, filterDescriptor.MemberType, (IFormatProvider) CultureInfo.CurrentCulture);
        }
        catch
        {
          obj = OperatorValueFilterDescriptorBase.UnsetValue;
        }
        DateTime result;
        if (filterDescriptor.MemberType.IsAssignableFrom(typeof (DateTime)) && DateTime.TryParse(this.FilterValue, out result))
          obj = (object) result;
        filterDescriptor.Value = obj;
      }
    }

    private static object DefaultValue(Type type)
    {
      return type.IsValueType ? Activator.CreateInstance(type) : (object) null;
    }
  }
}
