// Decompiled with JetBrains decompiler
// Type: CommonWPF.ExceptionLayer
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace CommonWPF
{
  public partial class ExceptionLayer : UserControl, IComponentConnector
  {
    internal Label LabelInnerException;
    internal TextBlock TextBlockMessage;
    internal TextBlock TextBlockStackTrace;
    internal ContentPresenter NextLayer;
    private bool _contentLoaded;

    public ExceptionLayer() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/exceptionlayer.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.LabelInnerException = (Label) target;
          break;
        case 2:
          this.TextBlockMessage = (TextBlock) target;
          break;
        case 3:
          this.TextBlockStackTrace = (TextBlock) target;
          break;
        case 4:
          this.NextLayer = (ContentPresenter) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
