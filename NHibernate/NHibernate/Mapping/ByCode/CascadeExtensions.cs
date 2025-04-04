// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.CascadeExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class CascadeExtensions
  {
    private const Cascade AnyButOrphans = Cascade.Persist | Cascade.Refresh | Cascade.Merge | Cascade.Remove | Cascade.Detach | Cascade.ReAttach;

    public static bool Has(this Cascade source, Cascade value) => (source & value) == value;

    public static Cascade Include(this Cascade source, Cascade value)
    {
      return CascadeExtensions.Cleanup(source | value);
    }

    private static Cascade Cleanup(Cascade cascade)
    {
      bool flag = cascade.Has(Cascade.All) || cascade.Has(Cascade.Persist | Cascade.Refresh | Cascade.Merge | Cascade.Remove | Cascade.Detach | Cascade.ReAttach);
      if (flag && cascade.Has(Cascade.DeleteOrphans))
        return Cascade.DeleteOrphans | Cascade.All;
      return flag ? Cascade.All : cascade;
    }

    public static Cascade Exclude(this Cascade source, Cascade value)
    {
      return source.Has(Cascade.All) && !value.Has(Cascade.All) ? CascadeExtensions.Cleanup((source & ~Cascade.All | Cascade.Persist | Cascade.Refresh | Cascade.Merge | Cascade.Remove | Cascade.Detach | Cascade.ReAttach) & ~value) : CascadeExtensions.Cleanup(source & ~value);
    }
  }
}
