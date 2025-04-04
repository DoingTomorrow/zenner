// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.PropertyPath
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class PropertyPath
  {
    private readonly int hashCode;
    private readonly MemberInfo localMember;
    private readonly PropertyPath previousPath;

    public PropertyPath(PropertyPath previousPath, MemberInfo localMember)
    {
      if (localMember == null)
        throw new ArgumentNullException(nameof (localMember));
      this.previousPath = previousPath;
      this.localMember = localMember;
      this.hashCode = localMember.GetHashCode() ^ (previousPath != null ? previousPath.GetHashCode() : 41);
    }

    public PropertyPath PreviousPath => this.previousPath;

    public MemberInfo LocalMember => this.localMember;

    public MemberInfo GetRootMember()
    {
      PropertyPath propertyPath = this;
      while (propertyPath.previousPath != null)
        propertyPath = propertyPath.previousPath;
      return propertyPath.localMember;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as PropertyPath);
    }

    public bool Equals(PropertyPath other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || this.hashCode == other.GetHashCode();
    }

    public override int GetHashCode() => this.hashCode;

    public string ToColumnName()
    {
      return this.PreviousPath != null ? this.PreviousPath.ToColumnName() + this.LocalMember.Name : this.LocalMember.Name;
    }

    public override string ToString()
    {
      return this.PreviousPath != null ? this.PreviousPath.ToString() + "." + this.LocalMember.Name : this.LocalMember.Name;
    }
  }
}
