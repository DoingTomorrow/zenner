// Decompiled with JetBrains decompiler
// Type: MVVM.ViewModel.ValidationViewModelBase
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using MSS.Localisation;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace MVVM.ViewModel
{
  public class ValidationViewModelBase : ViewModelBase, IDataErrorInfo
  {
    private readonly Dictionary<string, Func<ValidationViewModelBase, object>> propertyGetters;
    private readonly Dictionary<string, ValidationAttribute[]> validators;
    private bool isValid;
    private int validationExceptionCount;

    public string this[string propertyName]
    {
      get
      {
        List<string> values = this.ErrorMessages(propertyName);
        return values.Count > 0 ? string.Join(Environment.NewLine, (IEnumerable<string>) values) : string.Empty;
      }
    }

    private List<string> ErrorMessages(string propertyName)
    {
      List<string> stringList = new List<string>();
      if (this.propertyGetters.ContainsKey(propertyName))
      {
        object propertyValue = this.propertyGetters[propertyName](this);
        stringList = ((IEnumerable<ValidationAttribute>) this.validators[propertyName]).Where<ValidationAttribute>((Func<ValidationAttribute, bool>) (v => !v.IsValid(propertyValue))).Select<ValidationAttribute, string>((Func<ValidationAttribute, string>) (v => CultureResources.GetValue(v.ErrorMessage))).ToList<string>();
      }
      stringList.AddRange((IEnumerable<string>) this.ValidateProperty(propertyName));
      return stringList;
    }

    public bool IsValid
    {
      get => this.isValid;
      protected set
      {
        this.isValid = value;
        this.OnPropertyChanged(nameof (IsValid));
      }
    }

    public string Error
    {
      get
      {
        return string.Join(Environment.NewLine, this.validators.SelectMany((Func<KeyValuePair<string, ValidationAttribute[]>, IEnumerable<ValidationAttribute>>) (validator => (IEnumerable<ValidationAttribute>) validator.Value), (validator, attribute) => new
        {
          validator = validator,
          attribute = attribute
        }).Where(_param1 => !_param1.attribute.IsValid(this.propertyGetters[_param1.validator.Key](this))).Select(_param1 => _param1.attribute.ErrorMessage).ToArray<string>());
      }
    }

    public int ValidPropertiesCount
    {
      get
      {
        return this.validators.Where<KeyValuePair<string, ValidationAttribute[]>>((Func<KeyValuePair<string, ValidationAttribute[]>, bool>) (validator => ((IEnumerable<ValidationAttribute>) validator.Value).All<ValidationAttribute>((Func<ValidationAttribute, bool>) (attribute => attribute.IsValid(this.propertyGetters[validator.Key](this)))))).Count<KeyValuePair<string, ValidationAttribute[]>>() - this.validationExceptionCount;
      }
    }

    public int TotalPropertiesWithValidationCount
    {
      get => this.validators.Count<KeyValuePair<string, ValidationAttribute[]>>();
    }

    public ValidationViewModelBase()
    {
      this.validators = ((IEnumerable<PropertyInfo>) this.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => this.GetValidations(p).Length != 0)).ToDictionary<PropertyInfo, string, ValidationAttribute[]>((Func<PropertyInfo, string>) (p => p.Name), (Func<PropertyInfo, ValidationAttribute[]>) (p => this.GetValidations(p)));
      this.propertyGetters = ((IEnumerable<PropertyInfo>) this.GetType().GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => this.GetValidations(p).Length != 0)).ToDictionary<PropertyInfo, string, Func<ValidationViewModelBase, object>>((Func<PropertyInfo, string>) (p => p.Name), (Func<PropertyInfo, Func<ValidationViewModelBase, object>>) (p => this.GetValueGetter(p)));
    }

    private ValidationAttribute[] GetValidations(PropertyInfo property)
    {
      return (ValidationAttribute[]) property.GetCustomAttributes(typeof (ValidationAttribute), true);
    }

    private Func<ValidationViewModelBase, object> GetValueGetter(PropertyInfo property)
    {
      return (Func<ValidationViewModelBase, object>) (viewmodel => property.GetValue((object) viewmodel, (object[]) null));
    }

    public void ValidationExceptionsChanged(int count)
    {
      this.validationExceptionCount = count;
      this.OnPropertyChanged("ValidPropertiesCount");
    }

    protected override void OnPropertyChanged(string propertyName)
    {
      base.OnPropertyChanged(propertyName);
      if (!(propertyName != "IsValid") || !(propertyName != "MessageUserControl"))
        return;
      List<string> source = this.ErrorMessages(propertyName);
      if (string.IsNullOrEmpty(this.Error) && !source.Any<string>() && this.ValidPropertiesCount == this.TotalPropertiesWithValidationCount)
      {
        if (this.CheckAll())
          this.IsValid = true;
        else
          this.isValid = false;
      }
      else
        this.IsValid = false;
    }

    public virtual List<string> ValidateProperty(string propertyName) => new List<string>();

    private bool CheckAll()
    {
      List<string> errorMessages = new List<string>();
      this.propertyGetters.ForEach<KeyValuePair<string, Func<ValidationViewModelBase, object>>>((Action<KeyValuePair<string, Func<ValidationViewModelBase, object>>>) (x =>
      {
        string key = x.Key;
        object propertyValue = this.propertyGetters[key](this);
        List<string> list = ((IEnumerable<ValidationAttribute>) this.validators[key]).Where<ValidationAttribute>((Func<ValidationAttribute, bool>) (v => !v.IsValid(propertyValue))).Select<ValidationAttribute, string>((Func<ValidationAttribute, string>) (v => CultureResources.GetValue(v.ErrorMessage))).ToList<string>();
        List<string> collection = this.ErrorMessages(key);
        errorMessages.AddRange((IEnumerable<string>) list);
        errorMessages.AddRange((IEnumerable<string>) collection);
      }));
      return !errorMessages.Any<string>();
    }

    public string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
      return propertyExpression.Body is MemberExpression body ? body.Member.Name : (string) null;
    }
  }
}
