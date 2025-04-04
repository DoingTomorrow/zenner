// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.PrintDataGridView
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class PrintDataGridView
  {
    private static StringFormat StrFormat;
    private static StringFormat StrFormatComboBox;
    private static Button CellButton;
    private static CheckBox CellCheckBox;
    private static ComboBox CellComboBox;
    private static int TotalWidth;
    private static int RowPos;
    private static bool NewPage;
    private static bool NoWrap;
    private static int PageNo;
    private static ArrayList ColumnLefts = new ArrayList();
    private static ArrayList ColumnWidths = new ArrayList();
    private static ArrayList ColumnTypes = new ArrayList();
    private static int CellHeight;
    private static int RowsPerPage;
    private static PrintDocument printDoc = new PrintDocument();
    private static string PrintTitle = "";
    private static DataGridView dgv;
    private static List<string> SelectedColumns = new List<string>();
    private static List<string> AvailableColumns = new List<string>();
    private static bool PrintAllRows = true;
    private static bool FitToPageWidth = true;
    private static int HeaderHeight = 0;

    public static void Print(DataGridView dgv1) => PrintDataGridView.Print(dgv1, string.Empty);

    public static void Print(DataGridView dgv1, string titel, bool nowrap = false)
    {
      try
      {
        PrintDataGridView.dgv = dgv1;
        PrintDataGridView.AvailableColumns.Clear();
        foreach (DataGridViewColumn column in (BaseCollection) PrintDataGridView.dgv.Columns)
        {
          if (column.Visible)
            PrintDataGridView.AvailableColumns.Add(column.HeaderText);
        }
        PrintDataGridViewOptions dataGridViewOptions = new PrintDataGridViewOptions(PrintDataGridView.AvailableColumns);
        dataGridViewOptions.PrintTitle = titel;
        if (dataGridViewOptions.ShowDialog() != DialogResult.OK)
          return;
        PrintDataGridView.PrintTitle = dataGridViewOptions.PrintTitle;
        PrintDataGridView.PrintAllRows = dataGridViewOptions.PrintAllRows;
        PrintDataGridView.FitToPageWidth = dataGridViewOptions.FitToPageWidth;
        PrintDataGridView.SelectedColumns = dataGridViewOptions.GetSelectedColumns();
        PrintDataGridView.NoWrap = nowrap;
        PrintDataGridView.RowsPerPage = 0;
        if (new PrintDialog()
        {
          AllowSomePages = true,
          Document = PrintDataGridView.printDoc,
          PrinterSettings = PrintDataGridView.printDoc.PrinterSettings
        }.ShowDialog() != DialogResult.OK)
          return;
        PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
        printPreviewDialog.Document = PrintDataGridView.printDoc;
        PrintDataGridView.printDoc.BeginPrint += new PrintEventHandler(PrintDataGridView.PrintDoc_BeginPrint);
        PrintDataGridView.printDoc.PrintPage += new PrintPageEventHandler(PrintDataGridView.PrintDoc_PrintPage);
        if (printPreviewDialog.ShowDialog() != DialogResult.OK)
        {
          PrintDataGridView.printDoc.BeginPrint -= new PrintEventHandler(PrintDataGridView.PrintDoc_BeginPrint);
          PrintDataGridView.printDoc.PrintPage -= new PrintPageEventHandler(PrintDataGridView.PrintDoc_PrintPage);
        }
        else
        {
          PrintDataGridView.printDoc.Print();
          PrintDataGridView.printDoc.BeginPrint -= new PrintEventHandler(PrintDataGridView.PrintDoc_BeginPrint);
          PrintDataGridView.printDoc.PrintPage -= new PrintPageEventHandler(PrintDataGridView.PrintDoc_PrintPage);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
      }
    }

    private static void PrintDoc_BeginPrint(object sender, PrintEventArgs e)
    {
      try
      {
        PrintDataGridView.StrFormat = new StringFormat();
        PrintDataGridView.StrFormat.Alignment = StringAlignment.Near;
        PrintDataGridView.StrFormat.LineAlignment = StringAlignment.Center;
        if (PrintDataGridView.NoWrap)
          PrintDataGridView.StrFormat.FormatFlags = StringFormatFlags.NoWrap;
        PrintDataGridView.StrFormat.Trimming = StringTrimming.EllipsisCharacter;
        PrintDataGridView.StrFormatComboBox = new StringFormat();
        PrintDataGridView.StrFormatComboBox.LineAlignment = StringAlignment.Center;
        PrintDataGridView.StrFormatComboBox.FormatFlags = StringFormatFlags.NoWrap;
        PrintDataGridView.StrFormatComboBox.Trimming = StringTrimming.EllipsisCharacter;
        PrintDataGridView.ColumnLefts.Clear();
        PrintDataGridView.ColumnWidths.Clear();
        PrintDataGridView.ColumnTypes.Clear();
        PrintDataGridView.CellHeight = 0;
        PrintDataGridView.RowsPerPage = 0;
        PrintDataGridView.CellButton = new Button();
        PrintDataGridView.CellCheckBox = new CheckBox();
        PrintDataGridView.CellComboBox = new ComboBox();
        PrintDataGridView.TotalWidth = 0;
        foreach (DataGridViewColumn column in (BaseCollection) PrintDataGridView.dgv.Columns)
        {
          if (column.Visible && PrintDataGridView.SelectedColumns.Contains(column.HeaderText))
            PrintDataGridView.TotalWidth += column.Width;
        }
        PrintDataGridView.PageNo = 1;
        PrintDataGridView.NewPage = true;
        PrintDataGridView.RowPos = 0;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private static void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
    {
      int y1 = e.MarginBounds.Top;
      int left1 = e.MarginBounds.Left;
      try
      {
        if (PrintDataGridView.PageNo == 1)
        {
          foreach (DataGridViewColumn column in (BaseCollection) PrintDataGridView.dgv.Columns)
          {
            if (column.Visible && PrintDataGridView.SelectedColumns.Contains(column.HeaderText))
            {
              int width = !PrintDataGridView.FitToPageWidth ? column.Width : (int) Math.Floor((double) column.Width / (double) PrintDataGridView.TotalWidth * (double) PrintDataGridView.TotalWidth * ((double) e.MarginBounds.Width / (double) PrintDataGridView.TotalWidth));
              PrintDataGridView.HeaderHeight = (int) e.Graphics.MeasureString(column.HeaderText, column.InheritedStyle.Font, width).Height + 11;
              PrintDataGridView.ColumnLefts.Add((object) left1);
              PrintDataGridView.ColumnWidths.Add((object) width);
              PrintDataGridView.ColumnTypes.Add((object) column.GetType());
              left1 += width;
            }
          }
        }
        while (PrintDataGridView.RowPos <= PrintDataGridView.dgv.Rows.Count - 1)
        {
          DataGridViewRow row = PrintDataGridView.dgv.Rows[PrintDataGridView.RowPos];
          if (row.IsNewRow || !PrintDataGridView.PrintAllRows && !row.Selected)
          {
            ++PrintDataGridView.RowPos;
          }
          else
          {
            PrintDataGridView.CellHeight = row.Height;
            int num1 = y1 + PrintDataGridView.CellHeight;
            Rectangle marginBounds = e.MarginBounds;
            int height1 = marginBounds.Height;
            marginBounds = e.MarginBounds;
            int top1 = marginBounds.Top;
            int num2 = height1 + top1;
            if (num1 >= num2)
            {
              PrintDataGridView.DrawFooter(e, PrintDataGridView.RowsPerPage);
              PrintDataGridView.NewPage = true;
              ++PrintDataGridView.PageNo;
              e.HasMorePages = true;
              return;
            }
            if (PrintDataGridView.NewPage)
            {
              Graphics graphics1 = e.Graphics;
              string printTitle1 = PrintDataGridView.PrintTitle;
              Font font1 = new Font(PrintDataGridView.dgv.Font, FontStyle.Bold);
              Brush black1 = Brushes.Black;
              marginBounds = e.MarginBounds;
              double left2 = (double) marginBounds.Left;
              marginBounds = e.MarginBounds;
              double top2 = (double) marginBounds.Top;
              Graphics graphics2 = e.Graphics;
              string printTitle2 = PrintDataGridView.PrintTitle;
              Font font2 = new Font(PrintDataGridView.dgv.Font, FontStyle.Bold);
              marginBounds = e.MarginBounds;
              int width1 = marginBounds.Width;
              double height2 = (double) graphics2.MeasureString(printTitle2, font2, width1).Height;
              double y2 = top2 - height2 - 13.0;
              graphics1.DrawString(printTitle1, font1, black1, (float) left2, (float) y2);
              string str = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
              Graphics graphics3 = e.Graphics;
              string s = str;
              Font font3 = new Font(PrintDataGridView.dgv.Font, FontStyle.Bold);
              Brush black2 = Brushes.Black;
              marginBounds = e.MarginBounds;
              double left3 = (double) marginBounds.Left;
              marginBounds = e.MarginBounds;
              double width2 = (double) marginBounds.Width;
              Graphics graphics4 = e.Graphics;
              string text = str;
              Font font4 = new Font(PrintDataGridView.dgv.Font, FontStyle.Bold);
              marginBounds = e.MarginBounds;
              int width3 = marginBounds.Width;
              double width4 = (double) graphics4.MeasureString(text, font4, width3).Width;
              double num3 = width2 - width4;
              double x = left3 + num3;
              marginBounds = e.MarginBounds;
              double top3 = (double) marginBounds.Top;
              Graphics graphics5 = e.Graphics;
              string printTitle3 = PrintDataGridView.PrintTitle;
              Font font5 = new Font(new Font(PrintDataGridView.dgv.Font, FontStyle.Bold), FontStyle.Bold);
              marginBounds = e.MarginBounds;
              int width5 = marginBounds.Width;
              double height3 = (double) graphics5.MeasureString(printTitle3, font5, width5).Height;
              double y3 = top3 - height3 - 13.0;
              graphics3.DrawString(s, font3, black2, (float) x, (float) y3);
              marginBounds = e.MarginBounds;
              int top4 = marginBounds.Top;
              int index = 0;
              foreach (DataGridViewColumn column in (BaseCollection) PrintDataGridView.dgv.Columns)
              {
                if (column.Visible && PrintDataGridView.SelectedColumns.Contains(column.HeaderText))
                {
                  e.Graphics.FillRectangle((Brush) new SolidBrush(Color.LightGray), new Rectangle((int) PrintDataGridView.ColumnLefts[index], top4, (int) PrintDataGridView.ColumnWidths[index], PrintDataGridView.HeaderHeight));
                  e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int) PrintDataGridView.ColumnLefts[index], top4, (int) PrintDataGridView.ColumnWidths[index], PrintDataGridView.HeaderHeight));
                  e.Graphics.DrawString(column.HeaderText, column.InheritedStyle.Font, (Brush) new SolidBrush(column.InheritedStyle.ForeColor), new RectangleF((float) (int) PrintDataGridView.ColumnLefts[index], (float) top4, (float) (int) PrintDataGridView.ColumnWidths[index], (float) PrintDataGridView.HeaderHeight), PrintDataGridView.StrFormat);
                  ++index;
                }
              }
              PrintDataGridView.NewPage = false;
              y1 = top4 + PrintDataGridView.HeaderHeight;
            }
            int index1 = 0;
            foreach (DataGridViewCell cell in (BaseCollection) row.Cells)
            {
              if (cell.OwningColumn.Visible && PrintDataGridView.SelectedColumns.Contains(cell.OwningColumn.HeaderText))
              {
                if (((Type) PrintDataGridView.ColumnTypes[index1]).Name == "DataGridViewTextBoxColumn" || ((Type) PrintDataGridView.ColumnTypes[index1]).Name == "DataGridViewLinkColumn")
                  e.Graphics.DrawString(cell.Value.ToString(), cell.InheritedStyle.Font, (Brush) new SolidBrush(cell.InheritedStyle.ForeColor), new RectangleF((float) (int) PrintDataGridView.ColumnLefts[index1], (float) y1, (float) (int) PrintDataGridView.ColumnWidths[index1], (float) PrintDataGridView.CellHeight), PrintDataGridView.StrFormat);
                else if (((Type) PrintDataGridView.ColumnTypes[index1]).Name == "DataGridViewButtonColumn")
                {
                  PrintDataGridView.CellButton.Text = cell.Value.ToString();
                  PrintDataGridView.CellButton.Size = new Size((int) PrintDataGridView.ColumnWidths[index1], PrintDataGridView.CellHeight);
                  Bitmap bitmap = new Bitmap(PrintDataGridView.CellButton.Width, PrintDataGridView.CellButton.Height);
                  PrintDataGridView.CellButton.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                  e.Graphics.DrawImage((Image) bitmap, new Point((int) PrintDataGridView.ColumnLefts[index1], y1));
                }
                else if (((Type) PrintDataGridView.ColumnTypes[index1]).Name == "DataGridViewCheckBoxColumn")
                {
                  PrintDataGridView.CellCheckBox.Size = new Size(14, 14);
                  PrintDataGridView.CellCheckBox.Checked = (bool) cell.Value;
                  Bitmap bitmap = new Bitmap((int) PrintDataGridView.ColumnWidths[index1], PrintDataGridView.CellHeight);
                  Graphics.FromImage((Image) bitmap).FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                  PrintDataGridView.CellCheckBox.DrawToBitmap(bitmap, new Rectangle((bitmap.Width - PrintDataGridView.CellCheckBox.Width) / 2, (bitmap.Height - PrintDataGridView.CellCheckBox.Height) / 2, PrintDataGridView.CellCheckBox.Width, PrintDataGridView.CellCheckBox.Height));
                  e.Graphics.DrawImage((Image) bitmap, new Point((int) PrintDataGridView.ColumnLefts[index1], y1));
                }
                else if (((Type) PrintDataGridView.ColumnTypes[index1]).Name == "DataGridViewComboBoxColumn")
                {
                  PrintDataGridView.CellComboBox.Size = new Size((int) PrintDataGridView.ColumnWidths[index1], PrintDataGridView.CellHeight);
                  Bitmap bitmap = new Bitmap(PrintDataGridView.CellComboBox.Width, PrintDataGridView.CellComboBox.Height);
                  PrintDataGridView.CellComboBox.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                  e.Graphics.DrawImage((Image) bitmap, new Point((int) PrintDataGridView.ColumnLefts[index1], y1));
                  e.Graphics.DrawString(cell.Value.ToString(), cell.InheritedStyle.Font, (Brush) new SolidBrush(cell.InheritedStyle.ForeColor), new RectangleF((float) ((int) PrintDataGridView.ColumnLefts[index1] + 1), (float) y1, (float) ((int) PrintDataGridView.ColumnWidths[index1] - 16), (float) PrintDataGridView.CellHeight), PrintDataGridView.StrFormatComboBox);
                }
                else if (((Type) PrintDataGridView.ColumnTypes[index1]).Name == "DataGridViewImageColumn")
                {
                  Rectangle rectangle = new Rectangle((int) PrintDataGridView.ColumnLefts[index1], y1, (int) PrintDataGridView.ColumnWidths[index1], PrintDataGridView.CellHeight);
                  Size size = ((Image) cell.FormattedValue).Size;
                  e.Graphics.DrawImage((Image) cell.FormattedValue, new Rectangle((int) PrintDataGridView.ColumnLefts[index1] + (rectangle.Width - size.Width) / 2, y1 + (rectangle.Height - size.Height) / 2, ((Image) cell.FormattedValue).Width, ((Image) cell.FormattedValue).Height));
                }
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int) PrintDataGridView.ColumnLefts[index1], y1, (int) PrintDataGridView.ColumnWidths[index1], PrintDataGridView.CellHeight));
                ++index1;
              }
            }
            y1 += PrintDataGridView.CellHeight;
            ++PrintDataGridView.RowPos;
            if (PrintDataGridView.PageNo == 1)
              ++PrintDataGridView.RowsPerPage;
          }
        }
        if (PrintDataGridView.RowsPerPage == 0)
          return;
        PrintDataGridView.DrawFooter(e, PrintDataGridView.RowsPerPage);
        e.HasMorePages = false;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private static void DrawFooter(PrintPageEventArgs e, int RowsPerPage)
    {
      double num = !PrintDataGridView.PrintAllRows ? (double) PrintDataGridView.dgv.SelectedRows.Count : (!PrintDataGridView.dgv.Rows[PrintDataGridView.dgv.Rows.Count - 1].IsNewRow ? (double) (PrintDataGridView.dgv.Rows.Count - 1) : (double) (PrintDataGridView.dgv.Rows.Count - 2));
      string str = PrintDataGridView.PageNo.ToString() + " of " + Math.Ceiling(num / (double) RowsPerPage).ToString();
      e.Graphics.DrawString(str, PrintDataGridView.dgv.Font, Brushes.Black, (float) e.MarginBounds.Left + (float) (((double) e.MarginBounds.Width - (double) e.Graphics.MeasureString(str, PrintDataGridView.dgv.Font, e.MarginBounds.Width).Width) / 2.0), (float) (e.MarginBounds.Top + e.MarginBounds.Height + 31));
    }
  }
}
