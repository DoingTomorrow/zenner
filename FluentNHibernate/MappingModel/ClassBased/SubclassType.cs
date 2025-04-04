// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.SubclassType
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public class SubclassType
  {
    public static readonly SubclassType Subclass = new SubclassType("subclass");
    public static readonly SubclassType JoinedSubclass = new SubclassType("joined-subclass");
    public static readonly SubclassType UnionSubclass = new SubclassType("union-subclass");
    private readonly string elementName;

    private SubclassType(string elementName) => this.elementName = elementName;

    public string GetElementName() => this.elementName;

    public bool Equals(SubclassType other)
    {
      return object.Equals((object) other.elementName, (object) this.elementName);
    }

    public override bool Equals(object obj)
    {
      return obj.GetType() == typeof (SubclassType) && this.Equals((SubclassType) obj);
    }

    public override int GetHashCode()
    {
      return this.elementName == null ? 0 : this.elementName.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("ElementName: {0}", (object) this.elementName);
    }

    public static bool operator ==(SubclassType left, SubclassType right)
    {
      return object.Equals((object) left, (object) right);
    }

    public static bool operator !=(SubclassType left, SubclassType right) => !(left == right);
  }
}
