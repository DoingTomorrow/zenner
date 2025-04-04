// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SqlScriptReader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  internal class SqlScriptReader(ScriptSplitter splitter) : ScriptReader(splitter)
  {
    protected override bool ReadNext()
    {
      if (this.EndOfLine)
      {
        this.Splitter.Append(this.Current);
        this.Splitter.SetParser((ScriptReader) new SeparatorLineReader(this.Splitter));
        return false;
      }
      this.Splitter.Append(this.Current);
      return false;
    }
  }
}
