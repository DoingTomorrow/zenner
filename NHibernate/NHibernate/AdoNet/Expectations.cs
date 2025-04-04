// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.Expectations
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Data;

#nullable disable
namespace NHibernate.AdoNet
{
  public class Expectations
  {
    private const int UsualExpectedCount = 1;
    public static readonly IExpectation None = (IExpectation) new Expectations.NoneExpectation();
    public static readonly IExpectation Basic = (IExpectation) new Expectations.BasicExpectation(1);

    public static IExpectation AppropriateExpectation(ExecuteUpdateResultCheckStyle style)
    {
      if (style.Equals((object) ExecuteUpdateResultCheckStyle.None))
        return Expectations.None;
      if (style.Equals((object) ExecuteUpdateResultCheckStyle.Count))
        return Expectations.Basic;
      throw new HibernateException("unknown check style : " + (object) style);
    }

    private Expectations()
    {
    }

    public static void VerifyOutcomeBatched(int expectedRowCount, int rowCount)
    {
      if (expectedRowCount > rowCount)
        throw new StaleStateException("Batch update returned unexpected row count from update; actual row count: " + (object) rowCount + "; expected: " + (object) expectedRowCount);
      if (expectedRowCount < rowCount)
        throw new TooManyRowsAffectedException("Batch update returned unexpected row count from update; actual row count: " + (object) rowCount + "; expected: " + (object) expectedRowCount, expectedRowCount, rowCount);
    }

    public class BasicExpectation : IExpectation
    {
      private readonly int expectedRowCount;

      public BasicExpectation(int expectedRowCount)
      {
        this.expectedRowCount = expectedRowCount;
        if (expectedRowCount < 0)
          throw new ArgumentException("Expected row count must be greater than zero");
      }

      public void VerifyOutcomeNonBatched(int rowCount, IDbCommand statement)
      {
        rowCount = this.DetermineRowCount(rowCount, statement);
        if (this.expectedRowCount > rowCount)
          throw new StaleStateException("Unexpected row count: " + (object) rowCount + "; expected: " + (object) this.expectedRowCount);
        if (this.expectedRowCount < rowCount)
          throw new TooManyRowsAffectedException("Unexpected row count: " + (object) rowCount + "; expected: " + (object) this.expectedRowCount, this.expectedRowCount, rowCount);
      }

      public virtual bool CanBeBatched => true;

      public virtual int ExpectedRowCount => this.expectedRowCount;

      protected virtual int DetermineRowCount(int reportedRowCount, IDbCommand statement)
      {
        return reportedRowCount;
      }
    }

    public class NoneExpectation : IExpectation
    {
      public void VerifyOutcomeNonBatched(int rowCount, IDbCommand statement)
      {
      }

      public bool CanBeBatched => false;

      public int ExpectedRowCount => 1;
    }
  }
}
