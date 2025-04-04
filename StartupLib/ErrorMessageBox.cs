// Decompiled with JetBrains decompiler
// Type: StartupLib.ErrorMessageBox
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

#nullable disable
namespace StartupLib
{
  public static class ErrorMessageBox
  {
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

    public static bool ShowDialog(string title, Exception exc)
    {
      return ErrorMessageBox.ShowDialog((Window) null, title, exc);
    }

    public static bool ShowDialog(Window owner, string title, Exception exc)
    {
      string message = exc.Message;
      if (exc is AggregateException)
      {
        AggregateException aggregateException = exc as AggregateException;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Exception innerException in aggregateException.InnerExceptions)
          stringBuilder.AppendLine(innerException.Message);
        message = stringBuilder.ToString();
      }
      Type type = typeof (Form).Assembly.GetType("System.Windows.Forms.PropertyGridInternal.GridErrorDlg");
      Form instance = (Form) Activator.CreateInstance(type, (object) new PropertyGrid());
      instance.Text = title;
      instance.StartPosition = FormStartPosition.CenterParent;
      if (owner != null)
      {
        WindowInteropHelper windowInteropHelper = new WindowInteropHelper(owner);
        ErrorMessageBox.SetWindowLong(new HandleRef((object) instance, instance.Handle), -8, windowInteropHelper.Handle.ToInt32());
      }
      type.GetProperty("Details").SetValue((object) instance, (object) exc.ToString(), (object[]) null);
      type.GetProperty("Message").SetValue((object) instance, (object) message, (object[]) null);
      return instance.ShowDialog() == DialogResult.OK;
    }
  }
}
