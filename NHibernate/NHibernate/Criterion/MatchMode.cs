// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.MatchMode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace NHibernate.Criterion
{
  public abstract class MatchMode
  {
    private int _intCode;
    private string _name;
    private static Hashtable Instances = new Hashtable();
    public static readonly MatchMode Exact = (MatchMode) new MatchMode.ExactMatchMode();
    public static readonly MatchMode Start = (MatchMode) new MatchMode.StartMatchMode();
    public static readonly MatchMode End = (MatchMode) new MatchMode.EndMatchMode();
    public static readonly MatchMode Anywhere = (MatchMode) new MatchMode.AnywhereMatchMode();

    static MatchMode()
    {
      MatchMode.Instances.Add((object) MatchMode.Exact._intCode, (object) MatchMode.Exact);
      MatchMode.Instances.Add((object) MatchMode.Start._intCode, (object) MatchMode.Start);
      MatchMode.Instances.Add((object) MatchMode.End._intCode, (object) MatchMode.End);
      MatchMode.Instances.Add((object) MatchMode.Anywhere._intCode, (object) MatchMode.Anywhere);
    }

    protected MatchMode(int intCode, string name)
    {
      this._intCode = intCode;
      this._name = name;
    }

    public override string ToString() => this._name;

    public abstract string ToMatchString(string pattern);

    private class ExactMatchMode : MatchMode
    {
      public ExactMatchMode()
        : base(0, "EXACT")
      {
      }

      public override string ToMatchString(string pattern) => pattern;
    }

    private class StartMatchMode : MatchMode
    {
      public StartMatchMode()
        : base(1, "START")
      {
      }

      public override string ToMatchString(string pattern) => pattern + (object) '%';
    }

    private class EndMatchMode : MatchMode
    {
      public EndMatchMode()
        : base(2, "END")
      {
      }

      public override string ToMatchString(string pattern) => '%'.ToString() + pattern;
    }

    private class AnywhereMatchMode : MatchMode
    {
      public AnywhereMatchMode()
        : base(3, "ANYWHERE")
      {
      }

      public override string ToMatchString(string pattern)
      {
        return '%'.ToString() + pattern + (object) '%';
      }
    }
  }
}
