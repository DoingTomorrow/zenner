// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  [Serializable]
  public abstract class HbmBase
  {
    protected static T Find<T>(object[] array)
    {
      return (T) Array.Find<object>(array, (Predicate<object>) (obj => obj is T));
    }

    protected static T[] FindAll<T>(object[] array)
    {
      object[] all1 = Array.FindAll<object>(array, (Predicate<object>) (obj => obj is T));
      T[] all2 = new T[all1.Length];
      for (int index = 0; index < all2.Length; ++index)
        all2[index] = (T) all1[index];
      return all2;
    }

    protected static string JoinString(string[] text) => text.JoinString();
  }
}
