// Decompiled with JetBrains decompiler
// Type: Messir.Windows.Forms.TabStripButton
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Messir.Windows.Forms
{
  [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
  public class TabStripButton : ToolStripButton
  {
    private Color m_HotTextColor = Control.DefaultForeColor;
    private Color m_SelectedTextColor = Control.DefaultForeColor;
    private Font m_SelectedFont;

    public TabStripButton() => this.InitButton();

    public TabStripButton(Image image)
      : base(image)
    {
      this.InitButton();
    }

    public TabStripButton(string text)
      : base(text)
    {
      this.InitButton();
    }

    public TabStripButton(string text, Image image)
      : base(text, image)
    {
      this.InitButton();
    }

    public TabStripButton(string Text, Image Image, EventHandler Handler)
      : base(Text, Image, Handler)
    {
      this.InitButton();
    }

    public TabStripButton(string Text, Image Image, EventHandler Handler, string name)
      : base(Text, Image, Handler, name)
    {
      this.InitButton();
    }

    private void InitButton() => this.m_SelectedFont = this.Font;

    public override Size GetPreferredSize(Size constrainingSize)
    {
      Size preferredSize = base.GetPreferredSize(constrainingSize);
      if (this.Owner != null && this.Owner.Orientation == Orientation.Vertical)
      {
        preferredSize.Width += 3;
        preferredSize.Height += 10;
      }
      return preferredSize;
    }

    protected override Padding DefaultMargin => new Padding(0);

    [Browsable(false)]
    public new Padding Margin
    {
      get => base.Margin;
      set
      {
      }
    }

    [Browsable(false)]
    public new Padding Padding
    {
      get => base.Padding;
      set
      {
      }
    }

    [Category("Appearance")]
    [Description("Text color when TabButton is highlighted")]
    public Color HotTextColor
    {
      get => this.m_HotTextColor;
      set => this.m_HotTextColor = value;
    }

    [Category("Appearance")]
    [Description("Text color when TabButton is selected")]
    public Color SelectedTextColor
    {
      get => this.m_SelectedTextColor;
      set => this.m_SelectedTextColor = value;
    }

    [Category("Appearance")]
    [Description("Font when TabButton is selected")]
    public Font SelectedFont
    {
      get => this.m_SelectedFont == null ? this.Font : this.m_SelectedFont;
      set => this.m_SelectedFont = value;
    }

    [Browsable(false)]
    [DefaultValue(false)]
    public new bool Checked
    {
      get => this.IsSelected;
      set
      {
      }
    }

    [Browsable(false)]
    public bool IsSelected
    {
      get => this.Owner is TabStrip owner && this == owner.SelectedTab;
      set
      {
        if (!value || !(this.Owner is TabStrip owner))
          return;
        owner.SelectedTab = this;
      }
    }

    protected override void OnOwnerChanged(EventArgs e)
    {
      if (this.Owner != null && !(this.Owner is TabStrip))
        throw new Exception("Cannot add TabStripButton to " + this.Owner.GetType().Name);
      base.OnOwnerChanged(e);
    }
  }
}
