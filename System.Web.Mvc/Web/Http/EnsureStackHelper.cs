// Decompiled with JetBrains decompiler
// Type: System.Web.Http.EnsureStackHelper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Runtime.CompilerServices;
using System.Security;

#nullable disable
namespace System.Web.Http
{
  internal class EnsureStackHelper
  {
    private static readonly Action _ensureStackAction = EnsureStackHelper.InitializeEnsureStackDelegate();

    internal static void EnsureStack()
    {
      if (EnsureStackHelper._ensureStackAction == null)
        return;
      EnsureStackHelper._ensureStackAction();
    }

    [SecuritySafeCritical]
    private static Action InitializeEnsureStackDelegate()
    {
      try
      {
        Action action = (Action) Delegate.CreateDelegate(typeof (Action), typeof (RuntimeHelpers), "EnsureSufficientExecutionStack");
        action();
        return action;
      }
      catch
      {
        return (Action) null;
      }
    }
  }
}
