// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.AttributeLayeredValues
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  [Serializable]
  public class AttributeLayeredValues
  {
    private readonly Dictionary<string, LayeredValues> inner = new Dictionary<string, LayeredValues>();

    public LayeredValues this[string attribute]
    {
      get
      {
        this.EnsureValueExists(attribute);
        return this.inner[attribute];
      }
    }

    private void EnsureValueExists(string attribute)
    {
      if (this.inner.ContainsKey(attribute))
        return;
      this.inner[attribute] = new LayeredValues();
    }

    public bool ContainsKey(string attribute) => this.inner.ContainsKey(attribute);

    public void CopyTo(AttributeLayeredValues other)
    {
      foreach (KeyValuePair<string, LayeredValues> keyValuePair1 in this.inner)
      {
        foreach (KeyValuePair<int, object> keyValuePair2 in (Dictionary<int, object>) keyValuePair1.Value)
          other[keyValuePair1.Key][keyValuePair2.Key] = keyValuePair2.Value;
      }
    }

    public bool ContentEquals(AttributeLayeredValues other)
    {
      if (other.inner.Keys.Count != this.inner.Keys.Count || !other.inner.Keys.All<string>((Func<string, bool>) (key => this.inner.Keys.Contains<string>(key))))
        return false;
      foreach (KeyValuePair<string, LayeredValues> keyValuePair in other.inner)
      {
        LayeredValues right = this.inner[keyValuePair.Key];
        if (!keyValuePair.Value.ContentEquals<int, object>((IDictionary<int, object>) right))
          return false;
      }
      return true;
    }

    public override bool Equals(object obj)
    {
      return obj is AttributeLayeredValues ? this.Equals((AttributeLayeredValues) obj) : base.Equals(obj);
    }

    public bool Equals(AttributeLayeredValues other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || other.inner.ContentEquals<string, LayeredValues>((IDictionary<string, LayeredValues>) this.inner);
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      foreach (KeyValuePair<string, LayeredValues> keyValuePair in this.inner)
      {
        int num = 0 + keyValuePair.Key.GetHashCode() + keyValuePair.Value.GetHashCode();
        hashCode += num ^ 367;
      }
      return hashCode;
    }
  }
}
