// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.SelectByPictureWindow
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class SelectByPictureWindow : Window, IComponentConnector
  {
    private ConnectionItemSelectObject selObject;
    internal GmmCorporateControl gmmCorporateControl1;
    internal WrapPanel WrapPanalPictures;
    private bool _contentLoaded;

    public SelectByPictureWindow(ConnectionItemSelectObject selObject)
    {
      if (selObject == null)
        throw new NullReferenceException("Connection select object not defined");
      if (selObject.itemList == null)
        throw new NullReferenceException("Connection items list not defined");
      if (selObject.itemList.Count == 0)
        throw new ArgumentException("Empty connection items list");
      if (selObject.selectedItem == null)
        selObject.selectedItem = selObject.itemList[0];
      this.selObject = selObject;
      this.InitializeComponent();
      foreach (IConnectionItem connectionItem in selObject.itemList)
      {
        TextBlock element1 = new TextBlock();
        element1.FontSize = 30.0;
        element1.FontWeight = FontWeights.ExtraBold;
        element1.Text = connectionItem.Name;
        element1.HorizontalAlignment = HorizontalAlignment.Center;
        Image image = new Image();
        image.Stretch = Stretch.None;
        BitmapImage image500x500 = connectionItem.Image500x500;
        image.Source = image500x500 == null ? (ImageSource) ReadoutConfigMain.NotDefinedImage : (ImageSource) image500x500;
        Button element2 = new Button();
        element2.Content = (object) image;
        element2.Tag = (object) connectionItem;
        element2.Click += new RoutedEventHandler(this.Button_Click);
        TextBlock element3 = new TextBlock();
        element3.Text = connectionItem.Description;
        element3.Margin = new Thickness(5.0);
        element3.HorizontalAlignment = HorizontalAlignment.Left;
        StackPanel stackPanel = new StackPanel();
        stackPanel.Children.Add((UIElement) element1);
        stackPanel.Children.Add((UIElement) element2);
        stackPanel.Children.Add((UIElement) element3);
        Border element4 = new Border();
        element4.Margin = new Thickness(5.0);
        element4.BorderThickness = new Thickness(5.0);
        element4.BorderBrush = (Brush) new SolidColorBrush(Colors.Black);
        element4.CornerRadius = new CornerRadius(15.0);
        element4.Child = (UIElement) stackPanel;
        this.WrapPanalPictures.Children.Add((UIElement) element4);
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      this.selObject.selectedItem = (IConnectionItem) ((FrameworkElement) sender).Tag;
      this.DialogResult = new bool?(true);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/selectbypicturewindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 2:
          this.WrapPanalPictures = (WrapPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
