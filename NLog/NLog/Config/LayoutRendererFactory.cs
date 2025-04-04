// Decompiled with JetBrains decompiler
// Type: NLog.Config.LayoutRendererFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Config
{
  internal class LayoutRendererFactory(ConfigurationItemFactory parentFactory) : 
    Factory<LayoutRenderer, LayoutRendererAttribute>(parentFactory)
  {
    private Dictionary<string, FuncLayoutRenderer> _funcRenderers;

    public void ClearFuncLayouts()
    {
      this._funcRenderers = (Dictionary<string, FuncLayoutRenderer>) null;
    }

    public void RegisterFuncLayout(string name, FuncLayoutRenderer renderer)
    {
      this._funcRenderers = this._funcRenderers ?? new Dictionary<string, FuncLayoutRenderer>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this._funcRenderers[name] = renderer;
    }

    public override bool TryCreateInstance(string itemName, out LayoutRenderer result)
    {
      FuncLayoutRenderer funcLayoutRenderer;
      if (this._funcRenderers == null || !this._funcRenderers.TryGetValue(itemName, out funcLayoutRenderer))
        return base.TryCreateInstance(itemName, out result);
      result = (LayoutRenderer) funcLayoutRenderer;
      return true;
    }
  }
}
