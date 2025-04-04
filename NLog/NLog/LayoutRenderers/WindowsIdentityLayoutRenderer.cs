// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.WindowsIdentityLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Security.Principal;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("windows-identity")]
  [ThreadSafe]
  public class WindowsIdentityLayoutRenderer : LayoutRenderer
  {
    public WindowsIdentityLayoutRenderer()
    {
      this.UserName = true;
      this.Domain = true;
    }

    [DefaultValue(true)]
    public bool Domain { get; set; }

    [DefaultValue(true)]
    public bool UserName { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      WindowsIdentity current = WindowsIdentity.GetCurrent();
      if (current == null)
        return;
      string empty = string.Empty;
      string str;
      if (this.UserName)
      {
        if (this.Domain)
        {
          str = current.Name;
        }
        else
        {
          int num = current.Name.LastIndexOf('\\');
          str = num < 0 ? current.Name : current.Name.Substring(num + 1);
        }
      }
      else
      {
        if (!this.Domain)
          return;
        int length = current.Name.IndexOf('\\');
        str = length < 0 ? current.Name : current.Name.Substring(0, length);
      }
      builder.Append(str);
    }
  }
}
