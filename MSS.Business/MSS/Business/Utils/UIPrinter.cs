// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.UIPrinter
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Business.Utils
{
  public class UIPrinter : IUIPrinter
  {
    public int VerticalOffset { get; set; }

    public int HorizontalOffset { get; set; }

    public string Title { get; set; }

    public Grid Content { get; set; }

    public UIPrinter()
    {
      this.HorizontalOffset = 20;
      this.VerticalOffset = 20;
      this.Title = "Print " + (object) DateTime.Now;
    }

    public void Print()
    {
      PrintDialog printDialog = new PrintDialog();
      if (!printDialog.ShowDialog().GetValueOrDefault())
        return;
      FixedDocument fixedDocument = this.GetFixedDocument((FrameworkElement) this.Content, printDialog, new Thickness(48.0, 48.0, 48.0, 48.0));
      printDialog.PrintDocument(fixedDocument.DocumentPaginator, this.Title);
    }

    public void Print(Grid content, PrintDialog printDialog, Thickness margin)
    {
      FixedDocument documentFromRadGridView = this.GetFixedDocumentFromRadGridView((FrameworkElement) content, printDialog, margin);
      printDialog.PrintDocument(documentFromRadGridView.DocumentPaginator, this.Title);
    }

    private FixedDocument GetFixedDocument(
      FrameworkElement toPrint,
      PrintDialog printDialog,
      Thickness margin)
    {
      PrintCapabilities printCapabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
      Size size1 = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
      Size size2 = new Size(printCapabilities.PageImageableArea.ExtentWidth - margin.Left - margin.Right, printCapabilities.PageImageableArea.ExtentHeight - margin.Top - margin.Bottom);
      FixedDocument fixedDocument = new FixedDocument();
      toPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      toPrint.Arrange(new Rect(new System.Windows.Point(0.0, 0.0), toPrint.DesiredSize));
      Size desiredSize = toPrint.DesiredSize;
      for (double y = 0.0; y < desiredSize.Height; y += size2.Height)
      {
        VisualBrush visualBrush = new VisualBrush((Visual) toPrint);
        visualBrush.Stretch = Stretch.None;
        visualBrush.AlignmentX = AlignmentX.Left;
        visualBrush.AlignmentY = AlignmentY.Top;
        visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
        visualBrush.TileMode = TileMode.None;
        visualBrush.Viewbox = new Rect(0.0, y, size2.Width, size2.Height);
        PageContent newPageContent = new PageContent();
        FixedPage fixedPage = new FixedPage();
        ((IAddChild) newPageContent).AddChild((object) fixedPage);
        fixedDocument.Pages.Add(newPageContent);
        fixedPage.Width = size1.Width;
        fixedPage.Height = size1.Height;
        Canvas element = new Canvas();
        FixedPage.SetLeft((UIElement) element, printCapabilities.PageImageableArea.OriginWidth);
        FixedPage.SetTop((UIElement) element, printCapabilities.PageImageableArea.OriginHeight);
        element.Width = size2.Width;
        element.Height = size2.Height;
        element.Background = (Brush) visualBrush;
        element.Margin = margin;
        fixedPage.Children.Add((UIElement) element);
      }
      return fixedDocument;
    }

    private FixedDocument GetFixedDocumentFromRadGridView(
      FrameworkElement toPrint,
      PrintDialog printDialog,
      Thickness margin)
    {
      PrintCapabilities printCapabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
      Size size1 = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
      Size size2 = new Size(printCapabilities.PageImageableArea.ExtentWidth - margin.Left - margin.Right, printCapabilities.PageImageableArea.ExtentHeight - margin.Top - margin.Bottom);
      FixedDocument documentFromRadGridView = new FixedDocument();
      toPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      toPrint.Arrange(new Rect(new System.Windows.Point(0.0, 0.0), toPrint.DesiredSize));
      Size desiredSize = toPrint.DesiredSize;
      double num1 = 0.0;
      TextBlock childByType1 = toPrint.FindChildByType<TextBlock>();
      if (childByType1 != null)
        num1 = childByType1.ActualHeight;
      RadGridView childByType2 = toPrint.FindChildByType<RadGridView>();
      double rowHeight = childByType2.RowHeight;
      int count = childByType2.Items.Count;
      double num2 = childByType2.RenderSize.Height - rowHeight * (double) count;
      double num3;
      for (double y = 0.0; y < desiredSize.Height; y += num3)
      {
        double width = size2.Width;
        double num4 = 0.0;
        if (y == 0.0)
          num4 += num1 + num2;
        int int32 = Convert.ToInt32(Math.Truncate((size2.Height - num4) / rowHeight));
        num3 = num4 + (double) int32 * rowHeight;
        VisualBrush visualBrush = new VisualBrush((Visual) toPrint);
        visualBrush.Stretch = Stretch.None;
        visualBrush.AlignmentX = AlignmentX.Left;
        visualBrush.AlignmentY = AlignmentY.Top;
        visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
        visualBrush.TileMode = TileMode.None;
        visualBrush.Viewbox = new Rect(0.0, y, size2.Width, size2.Height);
        PageContent newPageContent = new PageContent();
        FixedPage fixedPage = new FixedPage();
        ((IAddChild) newPageContent).AddChild((object) fixedPage);
        documentFromRadGridView.Pages.Add(newPageContent);
        fixedPage.Width = size1.Width;
        fixedPage.Height = size1.Height;
        Canvas element = new Canvas();
        FixedPage.SetLeft((UIElement) element, printCapabilities.PageImageableArea.OriginWidth);
        FixedPage.SetTop((UIElement) element, printCapabilities.PageImageableArea.OriginHeight);
        element.Width = width;
        element.Height = num3;
        element.Background = (Brush) visualBrush;
        element.Margin = margin;
        fixedPage.Children.Add((UIElement) element);
      }
      return documentFromRadGridView;
    }
  }
}
