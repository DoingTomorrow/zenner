// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Data.Entity
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Data
{
  [Serializable]
  public abstract class Entity : IEquatable<Entity>
  {
    public virtual long Id { get; set; }

    public virtual bool Equals(Entity obj)
    {
      if (object.ReferenceEquals((object) null, (object) obj))
        return false;
      if (object.ReferenceEquals((object) this, (object) obj))
        return true;
      return this.GetType() == obj.GetType() && obj.Id == this.Id;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return this.GetType() == obj.GetType() && this.Equals((Entity) obj);
    }

    public override int GetHashCode() => this.Id.GetHashCode() * 397 ^ this.GetType().GetHashCode();

    public static bool operator ==(Entity left, Entity right)
    {
      return object.Equals((object) left, (object) right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
      return !object.Equals((object) left, (object) right);
    }
  }
}
