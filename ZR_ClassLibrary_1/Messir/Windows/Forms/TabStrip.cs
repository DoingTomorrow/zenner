// Decompiled with JetBrains decompiler
// Type: Messir.Windows.Forms.TabStrip
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

#nullable disable
namespace Messir.Windows.Forms
{
  public class TabStrip : ToolStrip
  {
    private TabStripRenderer myRenderer = new TabStripRenderer();
    protected TabStripButton mySelTab;
    private DesignerVerb insPage = (DesignerVerb) null;

    public TabStrip() => this.InitControl();

    public TabStrip(params TabStripButton[] buttons)
      : base((ToolStripItem[]) buttons)
    {
      this.InitControl();
    }

    protected void InitControl()
    {
      base.RenderMode = ToolStripRenderMode.ManagerRenderMode;
      base.Renderer = (ToolStripRenderer) this.myRenderer;
      this.myRenderer.RenderMode = this.RenderStyle;
      this.insPage = new DesignerVerb("Insert tab page", new EventHandler(this.OnInsertPageClicked));
    }

    public override ISite Site
    {
      get
      {
        ISite site = base.Site;
        if (site != null && site.DesignMode)
        {
          IContainer container = site.Container;
          if (container != null && container is IDesignerHost designerHost)
          {
            IDesigner designer = designerHost.GetDesigner(site.Component);
            if (designer != null && !designer.Verbs.Contains(this.insPage))
              designer.Verbs.Add(this.insPage);
          }
        }
        return site;
      }
      set => base.Site = value;
    }

    protected void OnInsertPageClicked(object sender, EventArgs e)
    {
      ISite site = base.Site;
      if (site == null || !site.DesignMode)
        return;
      IContainer container = site.Container;
      if (container != null)
      {
        TabStripButton tabStripButton = new TabStripButton();
        container.Add((IComponent) tabStripButton);
        tabStripButton.Text = tabStripButton.Name;
      }
    }

    public new ToolStripRenderer Renderer
    {
      get => (ToolStripRenderer) this.myRenderer;
      set => base.Renderer = (ToolStripRenderer) this.myRenderer;
    }

    public new ToolStripLayoutStyle LayoutStyle
    {
      get => base.LayoutStyle;
      set
      {
        switch (value)
        {
          case ToolStripLayoutStyle.StackWithOverflow:
          case ToolStripLayoutStyle.HorizontalStackWithOverflow:
          case ToolStripLayoutStyle.VerticalStackWithOverflow:
            base.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
            break;
          case ToolStripLayoutStyle.Flow:
            base.LayoutStyle = ToolStripLayoutStyle.Flow;
            break;
          case ToolStripLayoutStyle.Table:
            base.LayoutStyle = ToolStripLayoutStyle.Table;
            break;
          default:
            base.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
            break;
        }
      }
    }

    [Obsolete("Use RenderStyle instead")]
    [Browsable(false)]
    public new ToolStripRenderMode RenderMode
    {
      get => base.RenderMode;
      set => this.RenderStyle = value;
    }

    [Category("Appearance")]
    [Description("Gets or sets render style for TabStrip. You should use this property instead of RenderMode.")]
    public ToolStripRenderMode RenderStyle
    {
      get => this.myRenderer.RenderMode;
      set
      {
        this.myRenderer.RenderMode = value;
        this.Invalidate();
      }
    }

    protected override Padding DefaultPadding => Padding.Empty;

    [Browsable(false)]
    public new Padding Padding
    {
      get => this.DefaultPadding;
      set
      {
      }
    }

    [Category("Appearance")]
    [Description("Specifies if TabStrip should use system visual styles for painting items")]
    public bool UseVisualStyles
    {
      get => this.myRenderer.UseVS;
      set
      {
        this.myRenderer.UseVS = value;
        this.Invalidate();
      }
    }

    [Category("Appearance")]
    [Description("Specifies if TabButtons should be drawn flipped (for right- and bottom-aligned TabStrips)")]
    public bool FlipButtons
    {
      get => this.myRenderer.Mirrored;
      set
      {
        this.myRenderer.Mirrored = value;
        this.Invalidate();
      }
    }

    public TabStripButton SelectedTab
    {
      get => this.mySelTab;
      set
      {
        if (value == null || this.mySelTab == value)
          return;
        if (value.Owner != this)
          throw new ArgumentException("Cannot select TabButtons that do not belong to this TabStrip");
        this.OnItemClicked(new ToolStripItemClickedEventArgs((ToolStripItem) value));
      }
    }

    public event EventHandler<SelectedTabChangedEventArgs> SelectedTabChanged;

    protected void OnTabSelected(TabStripButton tab)
    {
      this.Invalidate();
      if (this.SelectedTabChanged == null)
        return;
      this.SelectedTabChanged((object) this, new SelectedTabChangedEventArgs(tab));
    }

    protected override void OnItemAdded(ToolStripItemEventArgs e)
    {
      base.OnItemAdded(e);
      if (!(e.Item is TabStripButton))
        return;
      this.SelectedTab = (TabStripButton) e.Item;
    }

    protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
    {
      if (e.ClickedItem is TabStripButton clickedItem)
      {
        this.SuspendLayout();
        this.mySelTab = clickedItem;
        this.ResumeLayout();
        this.OnTabSelected(clickedItem);
      }
      base.OnItemClicked(e);
    }
  }
}
