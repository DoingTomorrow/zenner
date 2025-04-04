// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.VariableLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Layouts;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("var")]
  [ThreadSafe]
  public class VariableLayoutRenderer : LayoutRenderer
  {
    [RequiredParameter]
    [DefaultParameter]
    public string Name { get; set; }

    public string Default { get; set; }

    protected override void InitializeLayoutRenderer()
    {
      SimpleLayout layout;
      if (this.TryGetLayout(out layout) && layout != null)
        layout.Initialize(this.LoggingConfiguration);
      base.InitializeLayoutRenderer();
    }

    private bool TryGetLayout(out SimpleLayout layout)
    {
      if (this.Name != null)
      {
        IDictionary<string, SimpleLayout> variables = this.LoggingConfiguration?.Variables;
        if (variables != null && variables.TryGetValue(this.Name, out layout))
          return true;
      }
      layout = (SimpleLayout) null;
      return false;
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (this.Name == null)
        return;
      SimpleLayout layout;
      if (this.TryGetLayout(out layout))
      {
        if (layout == null)
          return;
        builder.Append(layout.Render(logEvent));
      }
      else
      {
        if (this.Default == null)
          return;
        builder.Append(this.Default);
      }
    }
  }
}
