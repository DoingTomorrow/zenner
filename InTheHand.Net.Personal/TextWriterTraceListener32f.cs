// Decompiled with JetBrains decompiler
// Type: InTheHand.TextWriterTraceListener32f
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System.Diagnostics;
using System.IO;
using System.Reflection;

#nullable disable
namespace InTheHand
{
  public class TextWriterTraceListener32f : TraceListener
  {
    private readonly TextWriter wtr;
    private volatile bool disposed;

    public TextWriterTraceListener32f(string filename)
    {
      this.wtr = (TextWriter) File.AppendText(Path.Combine(TextWriterTraceListener32f.GetCurrentFolder(), filename));
    }

    private static string GetCurrentFolder()
    {
      return Path.GetDirectoryName(Assembly.GetCallingAssembly().GetName().CodeBase);
    }

    protected override void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
      try
      {
        this.wtr.Close();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public override void Flush()
    {
      if (this.disposed)
        return;
      this.wtr.Flush();
      base.Flush();
    }

    public override void Write(string message)
    {
      if (this.disposed)
        return;
      this.wtr.Write(message);
      if (!Debug.AutoFlush)
        return;
      this.Flush();
    }

    public override void WriteLine(string message)
    {
      if (this.disposed)
        return;
      this.wtr.WriteLine(message);
      if (!Debug.AutoFlush)
        return;
      this.Flush();
    }
  }
}
