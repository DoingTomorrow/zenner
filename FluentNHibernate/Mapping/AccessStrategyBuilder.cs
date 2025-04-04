// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.AccessStrategyBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class AccessStrategyBuilder
  {
    private const string InvalidPrefixCamelCaseFieldM = "m is not a valid prefix for a CamelCase Field.";
    private const string InvalidPrefixCamelCaseFieldMUnderscore = "m_ is not a valid prefix for a CamelCase Field.";
    private const string InvalidPrefixLowerCaseFieldM = "m is not a valid prefix for a LowerCase Field.";
    private const string InvalidPrefixLowerCaseFieldMUnderscore = "m_ is not a valid prefix for a LowerCase Field.";
    private const string InvalidPrefixPascalCaseFieldNone = "None is not a valid prefix for a PascalCase Field.";
    internal Action<string> setValue;

    protected AccessStrategyBuilder(Action<string> setter) => this.setValue = setter;

    public void Property() => this.setValue("property");

    public void Field() => this.setValue("field");

    public void BackingField() => this.setValue("backfield");

    public void ReadOnly() => this.setValue("readonly");

    public void CamelCaseField() => this.CamelCaseField(Prefix.None);

    public void CamelCaseField(Prefix prefix)
    {
      if (prefix == Prefix.m)
        throw new InvalidPrefixException("m is not a valid prefix for a CamelCase Field.");
      if (prefix == Prefix.mUnderscore)
        throw new InvalidPrefixException("m_ is not a valid prefix for a CamelCase Field.");
      this.setValue("field.camelcase" + prefix.Value);
    }

    public void LowerCaseField() => this.LowerCaseField(Prefix.None);

    public void LowerCaseField(Prefix prefix)
    {
      if (prefix == Prefix.m)
        throw new InvalidPrefixException("m is not a valid prefix for a LowerCase Field.");
      if (prefix == Prefix.mUnderscore)
        throw new InvalidPrefixException("m_ is not a valid prefix for a LowerCase Field.");
      this.setValue("field.lowercase" + prefix.Value);
    }

    public void PascalCaseField(Prefix prefix)
    {
      if (prefix == Prefix.None)
        throw new InvalidPrefixException("None is not a valid prefix for a PascalCase Field.");
      this.setValue("field.pascalcase" + prefix.Value);
    }

    public void ReadOnlyPropertyThroughCamelCaseField()
    {
      this.ReadOnlyPropertyThroughCamelCaseField(Prefix.None);
    }

    public void ReadOnlyPropertyThroughCamelCaseField(Prefix prefix)
    {
      if (prefix == Prefix.m)
        throw new InvalidPrefixException("m is not a valid prefix for a CamelCase Field.");
      if (prefix == Prefix.mUnderscore)
        throw new InvalidPrefixException("m_ is not a valid prefix for a CamelCase Field.");
      this.setValue("nosetter.camelcase" + prefix.Value);
    }

    public void ReadOnlyPropertyThroughLowerCaseField()
    {
      this.ReadOnlyPropertyThroughLowerCaseField(Prefix.None);
    }

    public void ReadOnlyPropertyThroughLowerCaseField(Prefix prefix)
    {
      if (prefix == Prefix.m)
        throw new InvalidPrefixException("m is not a valid prefix for a LowerCase Field.");
      if (prefix == Prefix.mUnderscore)
        throw new InvalidPrefixException("m_ is not a valid prefix for a LowerCase Field.");
      this.setValue("nosetter.lowercase" + prefix.Value);
    }

    public void ReadOnlyPropertyThroughPascalCaseField(Prefix prefix)
    {
      if (prefix == Prefix.None)
        throw new InvalidPrefixException("None is not a valid prefix for a PascalCase Field.");
      this.setValue("nosetter.pascalcase" + prefix.Value);
    }

    public void Using(string propertyAccessorAssemblyQualifiedClassName)
    {
      this.setValue(propertyAccessorAssemblyQualifiedClassName);
    }

    public void Using(Type propertyAccessorClassType)
    {
      this.Using(propertyAccessorClassType.AssemblyQualifiedName);
    }

    public void Using<TPropertyAccessorClass>() => this.Using(typeof (TPropertyAccessorClass));

    public void NoOp() => this.setValue("noop");

    public void None() => this.setValue("none");
  }
}
