// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Misc.ErrorManager
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Misc
{
  internal class ErrorManager
  {
    public static void InternalError(object error, Exception e)
    {
      StackFrame managerCodeLocation = ErrorManager.GetLastNonErrorManagerCodeLocation(e);
      ErrorManager.Error((object) ("Exception " + (object) e + "@" + (object) managerCodeLocation + ": " + error));
    }

    public static void InternalError(object error)
    {
      ErrorManager.Error((object) (ErrorManager.GetLastNonErrorManagerCodeLocation(new Exception()).ToString() + ": " + error));
    }

    private static StackFrame GetLastNonErrorManagerCodeLocation(Exception e)
    {
      StackTrace stackTrace = new StackTrace(e);
      int index = 0;
      while (index < stackTrace.FrameCount && stackTrace.GetFrame(index).ToString().IndexOf(nameof (ErrorManager)) >= 0)
        ++index;
      return stackTrace.GetFrame(index);
    }

    public static void Error(object arg)
    {
      new StringBuilder().AppendFormat("internal error: {0} ", arg);
    }
  }
}
