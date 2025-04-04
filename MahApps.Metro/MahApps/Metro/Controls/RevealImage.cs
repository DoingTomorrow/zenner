// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.RevealImage
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class RevealImage : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (RevealImage), (PropertyMetadata) new UIPropertyMetadata((object) ""));
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof (Image), typeof (ImageSource), typeof (RevealImage), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    internal RevealImage revealImage;
    internal System.Windows.Media.Animation.BeginStoryboard OnMouseLeave1_BeginStoryboard;
    internal Grid grid;
    internal Border border;
    internal TextBlock textBlock;
    private bool _contentLoaded;

    public string Text
    {
      get => (string) this.GetValue(RevealImage.TextProperty);
      set => this.SetValue(RevealImage.TextProperty, (object) value);
    }

    public ImageSource Image
    {
      get => (ImageSource) this.GetValue(RevealImage.ImageProperty);
      set => this.SetValue(RevealImage.ImageProperty, (object) value);
    }

    public RevealImage() => this.InitializeComponent();

    private static void TypewriteTextblock(string textToAnimate, TextBlock txt, TimeSpan timeSpan)
    {
      Storyboard storyboard1 = new Storyboard();
      storyboard1.FillBehavior = FillBehavior.HoldEnd;
      Storyboard storyboard2 = storyboard1;
      StringAnimationUsingKeyFrames animationUsingKeyFrames = new StringAnimationUsingKeyFrames();
      animationUsingKeyFrames.Duration = new Duration(timeSpan);
      StringAnimationUsingKeyFrames element = animationUsingKeyFrames;
      string empty = string.Empty;
      foreach (char ch in textToAnimate)
      {
        DiscreteStringKeyFrame discreteStringKeyFrame = new DiscreteStringKeyFrame();
        discreteStringKeyFrame.KeyTime = KeyTime.Paced;
        DiscreteStringKeyFrame keyFrame = discreteStringKeyFrame;
        empty += ch.ToString();
        keyFrame.Value = empty;
        element.KeyFrames.Add((StringKeyFrame) keyFrame);
      }
      Storyboard.SetTargetName((DependencyObject) element, txt.Name);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) TextBlock.TextProperty));
      storyboard2.Children.Add((Timeline) element);
      storyboard2.Begin((FrameworkElement) txt);
    }

    private void GridMouseEnter(object sender, MouseEventArgs e)
    {
      RevealImage.TypewriteTextblock(this.Text.ToUpper(), this.textBlock, TimeSpan.FromSeconds(0.25));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MahApps.Metro;component/controls/revealimage.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.revealImage = (RevealImage) target;
          break;
        case 2:
          this.OnMouseLeave1_BeginStoryboard = (System.Windows.Media.Animation.BeginStoryboard) target;
          break;
        case 3:
          this.grid = (Grid) target;
          this.grid.MouseEnter += new MouseEventHandler(this.GridMouseEnter);
          break;
        case 4:
          this.border = (Border) target;
          break;
        case 5:
          this.textBlock = (TextBlock) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
