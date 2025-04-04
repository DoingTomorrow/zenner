// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.NamedParameter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Param
{
  public class NamedParameter
  {
    public NamedParameter(string name, object value, IType type)
    {
      this.Name = name;
      this.Value = value;
      this.Type = type;
    }

    public string Name { get; private set; }

    public object Value { get; internal set; }

    public IType Type { get; internal set; }

    public bool Equals(NamedParameter other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.Name, (object) this.Name);
    }

    public override bool Equals(object obj) => this.Equals(obj as NamedParameter);

    public override int GetHashCode() => this.Name == null ? 0 : this.Name.GetHashCode();
  }
}
