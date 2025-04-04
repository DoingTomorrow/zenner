// Decompiled with JetBrains decompiler
// Type: NHibernate.Transform.AliasToBeanResultTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Properties;
using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace NHibernate.Transform
{
  [Serializable]
  public class AliasToBeanResultTransformer : IResultTransformer
  {
    private const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private readonly Type resultClass;
    private ISetter[] setters;
    private readonly IPropertyAccessor propertyAccessor;
    private readonly ConstructorInfo constructor;

    public AliasToBeanResultTransformer(Type resultClass)
    {
      this.resultClass = resultClass != null ? resultClass : throw new ArgumentNullException(nameof (resultClass));
      this.constructor = resultClass.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
      if (this.constructor == null && resultClass.IsClass)
        throw new ArgumentException("The target class of a AliasToBeanResultTransformer need a parameter-less constructor", nameof (resultClass));
      this.propertyAccessor = (IPropertyAccessor) new ChainedPropertyAccessor(new IPropertyAccessor[2]
      {
        PropertyAccessorFactory.GetPropertyAccessor((string) null),
        PropertyAccessorFactory.GetPropertyAccessor("field")
      });
    }

    public object TransformTuple(object[] tuple, string[] aliases)
    {
      if (aliases == null)
        throw new ArgumentNullException(nameof (aliases));
      object target;
      try
      {
        if (this.setters == null)
        {
          this.setters = new ISetter[aliases.Length];
          for (int index = 0; index < aliases.Length; ++index)
          {
            string alias = aliases[index];
            if (alias != null)
              this.setters[index] = this.propertyAccessor.GetSetter(this.resultClass, alias);
          }
        }
        target = this.resultClass.IsClass ? this.constructor.Invoke((object[]) null) : NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(this.resultClass, true);
        for (int index = 0; index < aliases.Length; ++index)
        {
          if (this.setters[index] != null)
            this.setters[index].Set(target, tuple[index]);
        }
      }
      catch (InstantiationException ex)
      {
        throw new HibernateException("Could not instantiate result class: " + this.resultClass.FullName, (Exception) ex);
      }
      catch (MethodAccessException ex)
      {
        throw new HibernateException("Could not instantiate result class: " + this.resultClass.FullName, (Exception) ex);
      }
      return target;
    }

    public IList TransformList(IList collection) => collection;

    public override bool Equals(object obj) => this.Equals(obj as AliasToBeanResultTransformer);

    public bool Equals(AliasToBeanResultTransformer other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.resultClass, (object) this.resultClass);
    }

    public override int GetHashCode() => this.resultClass.GetHashCode();
  }
}
