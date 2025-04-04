// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.ForeignKeyDirection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class ForeignKeyDirection
  {
    public static ForeignKeyDirection ForeignKeyFromParent = (ForeignKeyDirection) new ForeignKeyDirection.ForeignKeyFromParentClass();
    public static ForeignKeyDirection ForeignKeyToParent = (ForeignKeyDirection) new ForeignKeyDirection.ForeignKeyToParentClass();

    public abstract bool CascadeNow(CascadePoint cascadePoint);

    [Serializable]
    private class ForeignKeyFromParentClass : ForeignKeyDirection
    {
      public override bool CascadeNow(CascadePoint cascadePoint)
      {
        return cascadePoint != CascadePoint.AfterInsertBeforeDelete;
      }

      public override bool Equals(object obj)
      {
        return this.Equals(obj as ForeignKeyDirection.ForeignKeyFromParentClass);
      }

      public bool Equals(
        ForeignKeyDirection.ForeignKeyFromParentClass other)
      {
        return !object.ReferenceEquals((object) null, (object) other);
      }

      public override int GetHashCode() => 37;
    }

    [Serializable]
    private class ForeignKeyToParentClass : ForeignKeyDirection
    {
      public override bool CascadeNow(CascadePoint cascadePoint)
      {
        return cascadePoint != CascadePoint.BeforeInsertAfterDelete;
      }

      public override bool Equals(object obj)
      {
        return this.Equals(obj as ForeignKeyDirection.ForeignKeyToParentClass);
      }

      public bool Equals(ForeignKeyDirection.ForeignKeyToParentClass other)
      {
        return !object.ReferenceEquals((object) null, (object) other);
      }

      public override int GetHashCode() => 17;
    }
  }
}
