﻿// Decompiled with JetBrains decompiler
// Type: NLog.Targets.NLogViewerTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Layouts;
using System.Collections.Generic;

#nullable disable
namespace NLog.Targets
{
  [Target("NLogViewer")]
  public class NLogViewerTarget : NetworkTarget, IIncludeContext
  {
    private readonly Log4JXmlEventLayout _layout = new Log4JXmlEventLayout();

    public NLogViewerTarget()
    {
      this.Parameters = (IList<NLogViewerParameterInfo>) new List<NLogViewerParameterInfo>();
      this.Renderer.Parameters = this.Parameters;
      this.OnConnectionOverflow = NetworkTargetConnectionsOverflowAction.Block;
      this.MaxConnections = 16;
      this.NewLine = false;
      this.OptimizeBufferReuse = this.GetType() == typeof (NLogViewerTarget);
    }

    public NLogViewerTarget(string name)
      : this()
    {
      this.Name = name;
    }

    public bool IncludeNLogData
    {
      get => this.Renderer.IncludeNLogData;
      set => this.Renderer.IncludeNLogData = value;
    }

    public string AppInfo
    {
      get => this.Renderer.AppInfo;
      set => this.Renderer.AppInfo = value;
    }

    public bool IncludeCallSite
    {
      get => this.Renderer.IncludeCallSite;
      set => this.Renderer.IncludeCallSite = value;
    }

    public bool IncludeSourceInfo
    {
      get => this.Renderer.IncludeSourceInfo;
      set => this.Renderer.IncludeSourceInfo = value;
    }

    public bool IncludeMdc
    {
      get => this.Renderer.IncludeMdc;
      set => this.Renderer.IncludeMdc = value;
    }

    public bool IncludeNdc
    {
      get => this.Renderer.IncludeNdc;
      set => this.Renderer.IncludeNdc = value;
    }

    public bool IncludeMdlc
    {
      get => this.Renderer.IncludeMdlc;
      set => this.Renderer.IncludeMdlc = value;
    }

    public bool IncludeNdlc
    {
      get => this.Renderer.IncludeNdlc;
      set => this.Renderer.IncludeNdlc = value;
    }

    public string NdlcItemSeparator
    {
      get => this.Renderer.NdlcItemSeparator;
      set => this.Renderer.NdlcItemSeparator = value;
    }

    public bool IncludeAllProperties
    {
      get => this.Renderer.IncludeAllProperties;
      set => this.Renderer.IncludeAllProperties = value;
    }

    public string NdcItemSeparator
    {
      get => this.Renderer.NdcItemSeparator;
      set => this.Renderer.NdcItemSeparator = value;
    }

    public Layout LoggerName
    {
      get => this.Renderer.LoggerName;
      set => this.Renderer.LoggerName = value;
    }

    [ArrayParameter(typeof (NLogViewerParameterInfo), "parameter")]
    public IList<NLogViewerParameterInfo> Parameters { get; private set; }

    public Log4JXmlEventLayoutRenderer Renderer => this._layout.Renderer;

    public override Layout Layout
    {
      get => (Layout) this._layout;
      set
      {
      }
    }
  }
}
