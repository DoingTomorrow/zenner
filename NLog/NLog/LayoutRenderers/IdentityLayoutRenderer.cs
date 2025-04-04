// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.IdentityLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Security.Principal;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("identity")]
  [ThreadSafe]
  public class IdentityLayoutRenderer : LayoutRenderer
  {
    public IdentityLayoutRenderer()
    {
      this.Name = true;
      this.AuthType = true;
      this.IsAuthenticated = true;
      this.Separator = ":";
    }

    [DefaultValue(":")]
    public string Separator { get; set; }

    [DefaultValue(true)]
    public bool Name { get; set; }

    [DefaultValue(true)]
    public bool AuthType { get; set; }

    [DefaultValue(true)]
    public bool IsAuthenticated { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      IPrincipal currentPrincipal = Thread.CurrentPrincipal;
      if (currentPrincipal == null)
        return;
      IIdentity identity = currentPrincipal.Identity;
      if (identity == null)
        return;
      string str = string.Empty;
      if (this.IsAuthenticated)
      {
        builder.Append(str);
        str = this.Separator;
        builder.Append(identity.IsAuthenticated ? "auth" : "notauth");
      }
      if (this.AuthType)
      {
        builder.Append(str);
        str = this.Separator;
        builder.Append(identity.AuthenticationType);
      }
      if (!this.Name)
        return;
      builder.Append(str);
      builder.Append(identity.Name);
    }
  }
}
