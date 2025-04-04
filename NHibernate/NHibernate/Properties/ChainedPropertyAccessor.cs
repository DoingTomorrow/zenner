// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.ChainedPropertyAccessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Properties
{
  [Serializable]
  public class ChainedPropertyAccessor : IPropertyAccessor
  {
    private readonly IPropertyAccessor[] chain;

    public ChainedPropertyAccessor(IPropertyAccessor[] chain) => this.chain = chain;

    public IGetter GetGetter(Type theClass, string propertyName)
    {
      for (int index = 0; index < this.chain.Length; ++index)
      {
        IPropertyAccessor propertyAccessor = this.chain[index];
        try
        {
          return propertyAccessor.GetGetter(theClass, propertyName);
        }
        catch (PropertyNotFoundException ex)
        {
        }
      }
      throw new PropertyNotFoundException(theClass, propertyName, "getter");
    }

    public ISetter GetSetter(Type theClass, string propertyName)
    {
      for (int index = 0; index < this.chain.Length; ++index)
      {
        IPropertyAccessor propertyAccessor = this.chain[index];
        try
        {
          return propertyAccessor.GetSetter(theClass, propertyName);
        }
        catch (PropertyNotFoundException ex)
        {
        }
      }
      throw new PropertyNotFoundException(theClass, propertyName, "setter");
    }

    public bool CanAccessThroughReflectionOptimizer => false;
  }
}
