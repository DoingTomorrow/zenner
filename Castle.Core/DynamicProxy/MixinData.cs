// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.MixinData
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Castle.DynamicProxy
{
  public class MixinData
  {
    private readonly List<object> mixinsImpl = new List<object>();
    private readonly Dictionary<Type, int> mixinPositions = new Dictionary<Type, int>();

    public MixinData(IEnumerable<object> mixinInstances)
    {
      if (mixinInstances == null)
        return;
      List<Type> typeList = new List<Type>();
      Dictionary<Type, object> dictionary = new Dictionary<Type, object>();
      foreach (object mixinInstance in mixinInstances)
      {
        foreach (Type key in mixinInstance.GetType().GetInterfaces())
        {
          typeList.Add(key);
          if (dictionary.ContainsKey(key))
            throw new ArgumentException(string.Format("The list of mixins contains two mixins implementing the same interface '{0}': {1} and {2}. An interface cannot be added by more than one mixin.", (object) key.FullName, (object) dictionary[key].GetType().Name, (object) mixinInstance.GetType().Name), nameof (mixinInstances));
          dictionary[key] = mixinInstance;
        }
      }
      typeList.Sort((Comparison<Type>) ((x, y) => x.FullName.CompareTo(y.FullName)));
      for (int index = 0; index < typeList.Count; ++index)
      {
        Type key = typeList[index];
        object obj = dictionary[key];
        this.mixinPositions[key] = index;
        this.mixinsImpl.Add(obj);
      }
    }

    public IEnumerable<object> Mixins => (IEnumerable<object>) this.mixinsImpl;

    public IEnumerable<Type> MixinInterfaces => (IEnumerable<Type>) this.mixinPositions.Keys;

    public int GetMixinPosition(Type mixinInterfaceType) => this.mixinPositions[mixinInterfaceType];

    public bool ContainsMixin(Type mixinInterfaceType)
    {
      return this.mixinPositions.ContainsKey(mixinInterfaceType);
    }

    public object GetMixinInstance(Type mixinInterfaceType)
    {
      return this.mixinsImpl[this.mixinPositions[mixinInterfaceType]];
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) this, obj))
        return true;
      MixinData objA = obj as MixinData;
      if (object.ReferenceEquals((object) objA, (object) null) || this.mixinsImpl.Count != objA.mixinsImpl.Count)
        return false;
      for (int index = 0; index < this.mixinsImpl.Count; ++index)
      {
        if (this.mixinsImpl[index].GetType() != objA.mixinsImpl[index].GetType())
          return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      foreach (object obj in this.mixinsImpl)
        hashCode = 29 * hashCode + obj.GetType().GetHashCode();
      return hashCode;
    }
  }
}
