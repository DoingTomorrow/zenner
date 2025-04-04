// Decompiled with JetBrains decompiler
// Type: NHibernate.IdentityEqualityComparer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class IdentityEqualityComparer : IEqualityComparer
  {
    public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);

    public bool Equals(object x, object y) => object.ReferenceEquals(x, y);
  }
}
