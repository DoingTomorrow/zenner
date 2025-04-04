// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.Values.Property`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using System;

#nullable disable
namespace FluentNHibernate.Testing.Values
{
  public class Property<T, TProperty> : Property<T>
  {
    private static readonly Action<T, Accessor, TProperty> DefaultValueSetter = (Action<T, Accessor, TProperty>) ((target, propertyAccessor, value) => propertyAccessor.SetValue((object) target, (object) value));
    private readonly Accessor _propertyAccessor;
    private readonly TProperty _value;
    private Action<T, Accessor, TProperty> _valueSetter;

    public Property(Accessor property, TProperty value)
    {
      this._propertyAccessor = property;
      this._value = value;
    }

    public virtual Action<T, Accessor, TProperty> ValueSetter
    {
      get
      {
        return this._valueSetter != null ? this._valueSetter : Property<T, TProperty>.DefaultValueSetter;
      }
      set => this._valueSetter = value;
    }

    protected Accessor PropertyAccessor => this._propertyAccessor;

    protected TProperty Value => this._value;

    public override void SetValue(T target)
    {
      try
      {
        this.ValueSetter(target, this.PropertyAccessor, this.Value);
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Error while trying to set property " + this._propertyAccessor.Name, ex);
      }
    }

    public override void CheckValue(object target)
    {
      object obj = this.PropertyAccessor.GetValue(target);
      if (!(this.EntityEqualityComparer == null ? ((object) this.Value != null ? this.Value.Equals(obj) : obj == null) : this.EntityEqualityComparer.Equals((object) this.Value, obj)))
        throw new ApplicationException(this.GetInequalityComparisonMessage(obj));
    }

    private string GetInequalityComparisonMessage(object actual)
    {
      string str1 = actual != null ? actual.ToString() : "(null)";
      string fullName = this.PropertyAccessor.PropertyType.FullName;
      string str2 = (object) this.Value != null ? this.Value.ToString() : "(null)";
      string str3 = (object) this.Value != null ? this.Value.GetType().FullName : "(null)";
      string comparisonMessage;
      if (str1 != str2 && fullName != str3)
        comparisonMessage = string.Format("For property '{0}' expected '{1}' of type '{2}' but got '{3}' of type '{4}'", (object) this.PropertyAccessor.Name, (object) str2, (object) str3, (object) str1, (object) fullName);
      else if (str1 != str2)
        comparisonMessage = string.Format("For property '{0}' of type '{1}' expected '{2}' but got '{3}'", (object) this.PropertyAccessor.Name, (object) fullName, (object) str2, (object) str1);
      else
        comparisonMessage = !(fullName != str3) ? (!(fullName != str1) ? string.Format("For property '{0}' expected same element, but got different element of type '{1}'." + Environment.NewLine + "Tip: override ToString() on the type to find out the difference.", (object) this.PropertyAccessor.Name, (object) fullName) : string.Format("For property '{0}' expected same element, but got different element with the same value '{1}' of type '{2}'." + Environment.NewLine + "Tip: use a CustomEqualityComparer when creating the PersistenceSpecification object.", (object) this.PropertyAccessor.Name, (object) str1, (object) fullName)) : string.Format("For property '{0}' expected type '{1}' but got '{2}'", (object) this.PropertyAccessor.Name, (object) str3, (object) fullName);
      return comparisonMessage;
    }
  }
}
