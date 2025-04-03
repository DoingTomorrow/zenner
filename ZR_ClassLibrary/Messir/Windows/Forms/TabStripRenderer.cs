// Decompiled with JetBrains decompiler
// Type: Messir.Windows.Forms.TabStripRenderer
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

#nullable disable
namespace Messir.Windows.Forms
{
  internal class TabStripRenderer : ToolStripRenderer
  {
    private const int selOffset = 2;
    private ToolStripRenderer currentRenderer = (ToolStripRenderer) null;
    private ToolStripRenderMode renderMode = ToolStripRenderMode.Custom;
    private bool mirrored = false;
    private bool useVS = Application.RenderWithVisualStyles;

    public ToolStripRenderMode RenderMode
    {
      get => this.renderMode;
      set
      {
        this.renderMode = value;
        switch (this.renderMode)
        {
          case ToolStripRenderMode.System:
            this.currentRenderer = (ToolStripRenderer) new ToolStripSystemRenderer();
            break;
          case ToolStripRenderMode.Professional:
            this.currentRenderer = (ToolStripRenderer) new ToolStripProfessionalRenderer();
            break;
          default:
            this.currentRenderer = (ToolStripRenderer) null;
            break;
        }
      }
    }

    public bool Mirrored
    {
      get => this.mirrored;
      set => this.mirrored = value;
    }

    public bool UseVS
    {
      get => this.useVS;
      set
      {
        if (value && !Application.RenderWithVisualStyles)
          return;
        this.useVS = value;
      }
    }

    protected override void Initialize(ToolStrip ts) => base.Initialize(ts);

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
      Color color = SystemColors.AppWorkspace;
      if (this.UseVS)
        color = new VisualStyleRenderer(VisualStyleElement.Tab.Pane.Normal).GetColor(ColorProperty.BorderColorHint);
      using (Pen pen1 = new Pen(color))
      {
        using (Pen pen2 = new Pen(e.BackColor))
        {
          Rectangle bounds1 = e.ToolStrip.Bounds;
          int num1 = this.Mirrored ? 0 : bounds1.Width - 1 - e.ToolStrip.Padding.Horizontal;
          int num2 = this.Mirrored ? 0 : bounds1.Height - 1;
          if (e.ToolStrip.Orientation == Orientation.Horizontal)
          {
            e.Graphics.DrawLine(pen1, 0, num2, bounds1.Width, num2);
          }
          else
          {
            e.Graphics.DrawLine(pen1, num1, 0, num1, bounds1.Height);
            if (!this.Mirrored)
            {
              for (int index = num1 + 1; index < bounds1.Width; ++index)
                e.Graphics.DrawLine(pen2, index, 0, index, bounds1.Height);
            }
          }
          foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) e.ToolStrip.Items)
          {
            if (!toolStripItem.IsOnOverflow && toolStripItem is TabStripButton tabStripButton)
            {
              Rectangle bounds2 = tabStripButton.Bounds;
              int num3 = this.Mirrored ? bounds2.Left : bounds2.Right;
              int num4 = this.Mirrored ? bounds2.Top : bounds2.Bottom - 1;
              int num5 = this.Mirrored ? 0 : 1;
              if (e.ToolStrip.Orientation == Orientation.Horizontal)
              {
                e.Graphics.DrawLine(pen1, bounds2.Left, num4, bounds2.Right, num4);
                if (tabStripButton.Checked)
                  e.Graphics.DrawLine(pen2, bounds2.Left + 2 - num5, num4, bounds2.Right - 2 - num5, num4);
              }
              else
              {
                e.Graphics.DrawLine(pen1, num3, bounds2.Top, num3, bounds2.Bottom);
                if (tabStripButton.Checked)
                  e.Graphics.DrawLine(pen2, num3, bounds2.Top + 2 - num5, num3, bounds2.Bottom - 2 - num5);
              }
            }
          }
        }
      }
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawToolStripBackground(e);
      else
        base.OnRenderToolStripBackground(e);
    }

    protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
    {
      Graphics graphics = e.Graphics;
      TabStrip toolStrip = e.ToolStrip as TabStrip;
      TabStripButton tabStripButton = e.Item as TabStripButton;
      if (toolStrip == null || tabStripButton == null)
      {
        if (this.currentRenderer != null)
          this.currentRenderer.DrawButtonBackground(e);
        else
          base.OnRenderButtonBackground(e);
      }
      else
      {
        bool flag = tabStripButton.Checked;
        bool selected = tabStripButton.Selected;
        int y = 0;
        int x = 0;
        Rectangle bounds1 = tabStripButton.Bounds;
        int num1 = bounds1.Width - 1;
        bounds1 = tabStripButton.Bounds;
        int num2 = bounds1.Height - 1;
        if (this.UseVS)
        {
          Rectangle bounds2;
          if (toolStrip.Orientation == Orientation.Horizontal)
          {
            if (!flag)
            {
              y = 2;
              --num2;
            }
            else
              y = 1;
            bounds2 = new Rectangle(0, 0, num1, num2);
          }
          else
          {
            if (!flag)
            {
              x = 2;
              --num1;
            }
            else
              x = 1;
            bounds2 = new Rectangle(0, 0, num2, num1);
          }
          using (Bitmap bitmap = new Bitmap(bounds2.Width, bounds2.Height))
          {
            VisualStyleElement element = VisualStyleElement.Tab.TabItem.Normal;
            if (flag)
              element = VisualStyleElement.Tab.TabItem.Pressed;
            if (selected)
              element = VisualStyleElement.Tab.TabItem.Hot;
            if (!tabStripButton.Enabled)
              element = VisualStyleElement.Tab.TabItem.Disabled;
            if (!flag | selected)
              ++bounds2.Width;
            else
              ++bounds2.Height;
            using (Graphics dc = Graphics.FromImage((Image) bitmap))
            {
              new VisualStyleRenderer(element).DrawBackground((IDeviceContext) dc, bounds2);
              if (toolStrip.Orientation == Orientation.Vertical)
              {
                if (this.Mirrored)
                  bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                else
                  bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
              }
              else if (this.Mirrored)
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
              if (this.Mirrored)
              {
                bounds1 = tabStripButton.Bounds;
                x = bounds1.Width - bitmap.Width - x;
                bounds1 = tabStripButton.Bounds;
                y = bounds1.Height - bitmap.Height - y;
              }
              graphics.DrawImage((Image) bitmap, x, y);
            }
          }
        }
        else
        {
          if (toolStrip.Orientation == Orientation.Horizontal)
          {
            int num3;
            if (!flag)
            {
              num3 = 2;
              --num2;
            }
            else
              num3 = 1;
            if (this.Mirrored)
            {
              x = 1;
              y = 0;
            }
            else
              y = num3 + 1;
            --num1;
          }
          else
          {
            if (!flag)
            {
              x = 2;
              --num1;
            }
            else
              x = 1;
            if (this.Mirrored)
            {
              x = 0;
              y = 1;
            }
          }
          int height = num2 - 1;
          Rectangle rectangle = new Rectangle(x, y, num1, height);
          using (GraphicsPath path = new GraphicsPath())
          {
            if (this.Mirrored && toolStrip.Orientation == Orientation.Horizontal)
            {
              path.AddLine(rectangle.Left, rectangle.Top, rectangle.Left, rectangle.Bottom - 2);
              path.AddArc(rectangle.Left, rectangle.Bottom - 3, 2, 2, 90f, 90f);
              path.AddLine(rectangle.Left + 2, rectangle.Bottom, rectangle.Right - 2, rectangle.Bottom);
              path.AddArc(rectangle.Right - 2, rectangle.Bottom - 3, 2, 2, 0.0f, 90f);
              path.AddLine(rectangle.Right, rectangle.Bottom - 2, rectangle.Right, rectangle.Top);
            }
            else if (!this.Mirrored && toolStrip.Orientation == Orientation.Horizontal)
            {
              path.AddLine(rectangle.Left, rectangle.Bottom, rectangle.Left, rectangle.Top + 2);
              path.AddArc(rectangle.Left, rectangle.Top + 1, 2, 2, 180f, 90f);
              path.AddLine(rectangle.Left + 2, rectangle.Top, rectangle.Right - 2, rectangle.Top);
              path.AddArc(rectangle.Right - 2, rectangle.Top + 1, 2, 2, 270f, 90f);
              path.AddLine(rectangle.Right, rectangle.Top + 2, rectangle.Right, rectangle.Bottom);
            }
            else if (this.Mirrored && toolStrip.Orientation == Orientation.Vertical)
            {
              path.AddLine(rectangle.Left, rectangle.Top, rectangle.Right - 2, rectangle.Top);
              path.AddArc(rectangle.Right - 2, rectangle.Top + 1, 2, 2, 270f, 90f);
              path.AddLine(rectangle.Right, rectangle.Top + 2, rectangle.Right, rectangle.Bottom - 2);
              path.AddArc(rectangle.Right - 2, rectangle.Bottom - 3, 2, 2, 0.0f, 90f);
              path.AddLine(rectangle.Right - 2, rectangle.Bottom, rectangle.Left, rectangle.Bottom);
            }
            else
            {
              path.AddLine(rectangle.Right, rectangle.Top, rectangle.Left + 2, rectangle.Top);
              path.AddArc(rectangle.Left, rectangle.Top + 1, 2, 2, 180f, 90f);
              path.AddLine(rectangle.Left, rectangle.Top + 2, rectangle.Left, rectangle.Bottom - 2);
              path.AddArc(rectangle.Left, rectangle.Bottom - 3, 2, 2, 90f, 90f);
              path.AddLine(rectangle.Left + 2, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
            }
            if (flag | selected)
            {
              Color color = selected ? Color.WhiteSmoke : Color.White;
              if (this.renderMode == ToolStripRenderMode.Professional)
              {
                Color color1 = selected ? ProfessionalColors.ButtonCheckedGradientBegin : ProfessionalColors.ButtonCheckedGradientEnd;
                using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(tabStripButton.ContentRectangle, color1, ProfessionalColors.ButtonCheckedGradientMiddle, LinearGradientMode.Vertical))
                  graphics.FillPath((Brush) linearGradientBrush, path);
              }
              else
              {
                using (SolidBrush solidBrush = new SolidBrush(color))
                  graphics.FillPath((Brush) solidBrush, path);
              }
            }
            using (Pen pen = new Pen(flag ? ControlPaint.Dark(SystemColors.AppWorkspace) : SystemColors.AppWorkspace))
              graphics.DrawPath(pen, path);
          }
        }
      }
    }

    protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
    {
      Rectangle imageRectangle = e.ImageRectangle;
      if (e.Item is TabStripButton tabStripButton)
      {
        int num = (this.Mirrored ? -1 : 1) * (tabStripButton.Checked ? 1 : 2);
        if (e.ToolStrip.Orientation == Orientation.Horizontal)
          imageRectangle.Offset(this.Mirrored ? 2 : 1, num + (this.Mirrored ? 1 : 0));
        else
          imageRectangle.Offset(num + 2, 0);
      }
      ToolStripItemImageRenderEventArgs e1 = new ToolStripItemImageRenderEventArgs(e.Graphics, e.Item, e.Image, imageRectangle);
      if (this.currentRenderer != null)
        this.currentRenderer.DrawItemImage(e1);
      else
        base.OnRenderItemImage(e1);
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
      Rectangle textRectangle = e.TextRectangle;
      TabStripButton tabStripButton = e.Item as TabStripButton;
      Color textColor = e.TextColor;
      Font textFont = e.TextFont;
      if (tabStripButton != null)
      {
        int num = (this.Mirrored ? -1 : 1) * (tabStripButton.Checked ? 1 : 2);
        if (e.ToolStrip.Orientation == Orientation.Horizontal)
          textRectangle.Offset(this.Mirrored ? 2 : 1, num + (this.Mirrored ? 1 : -1));
        else
          textRectangle.Offset(num + 2, 0);
        if (tabStripButton.Selected)
          textColor = tabStripButton.HotTextColor;
        else if (tabStripButton.Checked)
          textColor = tabStripButton.SelectedTextColor;
        if (tabStripButton.Checked)
          textFont = tabStripButton.SelectedFont;
      }
      ToolStripItemTextRenderEventArgs e1 = new ToolStripItemTextRenderEventArgs(e.Graphics, e.Item, e.Text, textRectangle, textColor, textFont, e.TextFormat);
      e1.TextDirection = e.TextDirection;
      if (this.currentRenderer != null)
        this.currentRenderer.DrawItemText(e1);
      else
        base.OnRenderItemText(e1);
    }

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawArrow(e);
      else
        base.OnRenderArrow(e);
    }

    protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawDropDownButtonBackground(e);
      else
        base.OnRenderDropDownButtonBackground(e);
    }

    protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawGrip(e);
      else
        base.OnRenderGrip(e);
    }

    protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawImageMargin(e);
      else
        base.OnRenderImageMargin(e);
    }

    protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawItemBackground(e);
      else
        base.OnRenderItemBackground(e);
    }

    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawItemCheck(e);
      else
        base.OnRenderItemCheck(e);
    }

    protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawLabelBackground(e);
      else
        base.OnRenderLabelBackground(e);
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawMenuItemBackground(e);
      else
        base.OnRenderMenuItemBackground(e);
    }

    protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawOverflowButtonBackground(e);
      else
        base.OnRenderOverflowButtonBackground(e);
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawSeparator(e);
      else
        base.OnRenderSeparator(e);
    }

    protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawSplitButton(e);
      else
        base.OnRenderSplitButtonBackground(e);
    }

    protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawStatusStripSizingGrip(e);
      else
        base.OnRenderStatusStripSizingGrip(e);
    }

    protected override void OnRenderToolStripContentPanelBackground(
      ToolStripContentPanelRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawToolStripContentPanelBackground(e);
      else
        base.OnRenderToolStripContentPanelBackground(e);
    }

    protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawToolStripPanelBackground(e);
      else
        base.OnRenderToolStripPanelBackground(e);
    }

    protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
    {
      if (this.currentRenderer != null)
        this.currentRenderer.DrawToolStripStatusLabelBackground(e);
      else
        base.OnRenderToolStripStatusLabelBackground(e);
    }
  }
}
