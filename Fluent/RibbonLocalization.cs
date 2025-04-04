// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonLocalization
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Fluent
{
  public class RibbonLocalization : INotifyPropertyChanged
  {
    private CultureInfo culture;
    private string backstageButtonText = "File";
    private string backstageButtonTextProperty;
    private string backstageButtonKeyTip = "F";
    private string backstageButtonKeyTipProperty;
    private string minimizeButtonScreenTipTitle = "Minimize the Ribbon (Ctrl + F1)";
    private string minimizeButtonScreenTipTitleProperty;
    private string minimizeButtonScreenTipText = "Show or hide the Ribbon\n\nWhen the Ribbon is hidden, only\nthe tab names are shown";
    private string minimizeButtonScreenTipTextProperty;
    private string expandButtonScreenTipTitle = "Expand the Ribbon (Ctrl + F1)";
    private string expandButtonScreenTipTitleProperty;
    private string expandButtonScreenTipText = "Show or hide the Ribbon\n\nWhen the Ribbon is hidden, only\nthe tab names are shown";
    private string expandButtonScreenTipTextProperty;
    private string quickAccessToolBarDropDownButtonTooltip = "Customize Quick Access Toolbar";
    private string quickAccessToolBarDropDownButtonTooltipProperty;
    private string quickAccessToolBarMoreControlsButtonTooltip = "More controls";
    private string quickAccessToolBarMoreControlsButtonTooltipProperty;
    private string quickAccessToolBarMenuHeader = "Customize Quick Access Toolbar";
    private string quickAccessToolBarMenuHeaderProperty;
    private string quickAccessToolBarMenuShowBelow = "Show Below the Ribbon";
    private string quickAccessToolBarMenuShowBelowProperty;
    private string quickAccessToolBarMenuShowAbove = "Show Above the Ribbon";
    private string quickAccessToolBarMenuShowAboveProperty;
    private string ribbonContextMenuAddItem = "Add to Quick Access Toolbar";
    private string ribbonContextMenuAddItemProperty;
    private string ribbonContextMenuAddGroup = "Add Group to Quick Access Toolbar";
    private string ribbonContextMenuAddGroupProperty;
    private string ribbonContextMenuAddGallery = "Add Gallery to Quick Access Toolbar";
    private string ribbonContextMenuAddGalleryProperty;
    private string ribbonContextMenuAddMenu = "Add Menu to Quick Access Toolbar";
    private string ribbonContextMenuAddMenuProperty;
    private string ribbonContextMenuRemoveItem = "Remove from Quick Access Toolbar";
    private string ribbonContextMenuRemoveItemProperty;
    private string ribbonContextMenuCustomizeQuickAccessToolbar = "Customize Quick Access Toolbar...";
    private string ribbonContextMenuCustomizeQuickAccessToolbarProperty;
    private string ribbonContextMenuCustomizeRibbon = "Customize the Ribbon...";
    private string ribbonContextMenuCustomizeRibbonProperty;
    private string ribbonContextMenuMinimizeRibbon = "Minimize the Ribbon";
    private string ribbonContextMenuMinimizeRibbonProperty;
    private string ribbonContextMenuShowBelow = "Show Quick Access Toolbar Below the Ribbon";
    private string ribbonContextMenuShowBelowProperty;
    private string ribbonContextMenuShowAbove = "Show Quick Access Toolbar Above the Ribbon";
    private string ribbonContextMenuShowAboveProperty;
    private string screenTipDisableReasonHeader = "This command is currently disabled.";
    private string screenTipDisableReasonHeaderProperty;
    private string customizeStatusBar = "Customize Status Bar";
    private string customizeStatusBarProperty;

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public CultureInfo Culture
    {
      get => this.culture;
      set
      {
        if (value == this.culture)
          return;
        this.culture = value;
        this.LoadCulture(this.culture);
        this.RaisePropertyChanged(nameof (Culture));
      }
    }

    public string BackstageButtonText
    {
      get => this.backstageButtonTextProperty ?? this.backstageButtonText;
      set
      {
        if (!(this.backstageButtonTextProperty != value))
          return;
        this.backstageButtonTextProperty = value;
        this.RaisePropertyChanged(nameof (BackstageButtonText));
      }
    }

    public string BackstageButtonKeyTip
    {
      get => this.backstageButtonKeyTipProperty ?? this.backstageButtonKeyTip;
      set
      {
        if (!(this.backstageButtonKeyTipProperty != value))
          return;
        this.backstageButtonKeyTipProperty = value;
        this.RaisePropertyChanged(nameof (BackstageButtonKeyTip));
      }
    }

    public string MinimizeButtonScreenTipTitle
    {
      get => this.minimizeButtonScreenTipTitleProperty ?? this.minimizeButtonScreenTipTitle;
      set
      {
        if (!(this.minimizeButtonScreenTipTitleProperty != value))
          return;
        this.minimizeButtonScreenTipTitleProperty = value;
        this.RaisePropertyChanged(nameof (MinimizeButtonScreenTipTitle));
      }
    }

    public string MinimizeButtonScreenTipText
    {
      get => this.minimizeButtonScreenTipTextProperty ?? this.minimizeButtonScreenTipText;
      set
      {
        if (!(this.minimizeButtonScreenTipTextProperty != value))
          return;
        this.minimizeButtonScreenTipTextProperty = value;
        this.RaisePropertyChanged(nameof (MinimizeButtonScreenTipText));
      }
    }

    public string ExpandButtonScreenTipTitle
    {
      get => this.expandButtonScreenTipTitleProperty ?? this.expandButtonScreenTipTitle;
      set
      {
        if (!(this.expandButtonScreenTipTitleProperty != value))
          return;
        this.expandButtonScreenTipTitleProperty = value;
        this.RaisePropertyChanged(nameof (ExpandButtonScreenTipTitle));
      }
    }

    public string ExpandButtonScreenTipText
    {
      get => this.expandButtonScreenTipTextProperty ?? this.expandButtonScreenTipText;
      set
      {
        if (!(this.expandButtonScreenTipTextProperty != value))
          return;
        this.expandButtonScreenTipTextProperty = value;
        this.RaisePropertyChanged(nameof (ExpandButtonScreenTipText));
      }
    }

    public string QuickAccessToolBarDropDownButtonTooltip
    {
      get
      {
        return this.quickAccessToolBarDropDownButtonTooltipProperty ?? this.quickAccessToolBarDropDownButtonTooltip;
      }
      set
      {
        if (!(this.quickAccessToolBarDropDownButtonTooltipProperty != value))
          return;
        this.quickAccessToolBarDropDownButtonTooltipProperty = value;
        this.RaisePropertyChanged(nameof (QuickAccessToolBarDropDownButtonTooltip));
      }
    }

    public string QuickAccessToolBarMoreControlsButtonTooltip
    {
      get
      {
        return this.quickAccessToolBarMoreControlsButtonTooltipProperty ?? this.quickAccessToolBarMoreControlsButtonTooltip;
      }
      set
      {
        if (!(this.quickAccessToolBarMoreControlsButtonTooltipProperty != value))
          return;
        this.quickAccessToolBarMoreControlsButtonTooltipProperty = value;
        this.RaisePropertyChanged(nameof (QuickAccessToolBarMoreControlsButtonTooltip));
      }
    }

    public string QuickAccessToolBarMenuHeader
    {
      get => this.quickAccessToolBarMenuHeaderProperty ?? this.quickAccessToolBarMenuHeader;
      set
      {
        if (!(this.quickAccessToolBarMenuHeaderProperty != value))
          return;
        this.quickAccessToolBarMenuHeaderProperty = value;
        this.RaisePropertyChanged(nameof (QuickAccessToolBarMenuHeader));
      }
    }

    public string QuickAccessToolBarMenuShowBelow
    {
      get => this.quickAccessToolBarMenuShowBelowProperty ?? this.quickAccessToolBarMenuShowBelow;
      set
      {
        if (!(this.quickAccessToolBarMenuShowBelowProperty != value))
          return;
        this.quickAccessToolBarMenuShowBelowProperty = value;
        this.RaisePropertyChanged(nameof (QuickAccessToolBarMenuShowBelow));
      }
    }

    public string QuickAccessToolBarMenuShowAbove
    {
      get => this.quickAccessToolBarMenuShowAboveProperty ?? this.quickAccessToolBarMenuShowAbove;
      set
      {
        if (!(this.quickAccessToolBarMenuShowAboveProperty != value))
          return;
        this.quickAccessToolBarMenuShowAboveProperty = value;
        this.RaisePropertyChanged(nameof (QuickAccessToolBarMenuShowAbove));
      }
    }

    public string RibbonContextMenuAddItem
    {
      get => this.ribbonContextMenuAddItemProperty ?? this.ribbonContextMenuAddItem;
      set
      {
        if (!(this.ribbonContextMenuAddItemProperty != value))
          return;
        this.ribbonContextMenuAddItemProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuAddItem));
      }
    }

    public string RibbonContextMenuAddGroup
    {
      get => this.ribbonContextMenuAddGroupProperty ?? this.ribbonContextMenuAddGroup;
      set
      {
        if (!(this.ribbonContextMenuAddGroupProperty != value))
          return;
        this.ribbonContextMenuAddGroupProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuAddGroup));
      }
    }

    public string RibbonContextMenuAddGallery
    {
      get => this.ribbonContextMenuAddGalleryProperty ?? this.ribbonContextMenuAddGallery;
      set
      {
        if (!(this.ribbonContextMenuAddGalleryProperty != value))
          return;
        this.ribbonContextMenuAddGalleryProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuAddGallery));
      }
    }

    public string RibbonContextMenuAddMenu
    {
      get => this.ribbonContextMenuAddMenuProperty ?? this.ribbonContextMenuAddMenu;
      set
      {
        if (!(this.ribbonContextMenuAddMenuProperty != value))
          return;
        this.ribbonContextMenuAddMenuProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuAddMenu));
      }
    }

    public string RibbonContextMenuRemoveItem
    {
      get => this.ribbonContextMenuRemoveItemProperty ?? this.ribbonContextMenuRemoveItem;
      set
      {
        if (!(this.ribbonContextMenuRemoveItemProperty != value))
          return;
        this.ribbonContextMenuRemoveItemProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuRemoveItem));
      }
    }

    public string RibbonContextMenuCustomizeQuickAccessToolBar
    {
      get
      {
        return this.ribbonContextMenuCustomizeQuickAccessToolbarProperty ?? this.ribbonContextMenuCustomizeQuickAccessToolbar;
      }
      set
      {
        if (!(this.ribbonContextMenuCustomizeQuickAccessToolbarProperty != value))
          return;
        this.ribbonContextMenuCustomizeQuickAccessToolbarProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuCustomizeQuickAccessToolBar));
      }
    }

    public string RibbonContextMenuCustomizeRibbon
    {
      get => this.ribbonContextMenuCustomizeRibbonProperty ?? this.ribbonContextMenuCustomizeRibbon;
      set
      {
        if (!(this.ribbonContextMenuCustomizeRibbonProperty != value))
          return;
        this.ribbonContextMenuCustomizeRibbonProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuCustomizeRibbon));
      }
    }

    public string RibbonContextMenuMinimizeRibbon
    {
      get => this.ribbonContextMenuMinimizeRibbonProperty ?? this.ribbonContextMenuMinimizeRibbon;
      set
      {
        if (!(this.ribbonContextMenuMinimizeRibbonProperty != value))
          return;
        this.ribbonContextMenuMinimizeRibbonProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuMinimizeRibbon));
      }
    }

    public string RibbonContextMenuShowBelow
    {
      get => this.ribbonContextMenuShowBelowProperty ?? this.ribbonContextMenuShowBelow;
      set
      {
        if (!(this.ribbonContextMenuShowBelowProperty != value))
          return;
        this.ribbonContextMenuShowBelowProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuShowBelow));
      }
    }

    public string RibbonContextMenuShowAbove
    {
      get => this.ribbonContextMenuShowAboveProperty ?? this.ribbonContextMenuShowAbove;
      set
      {
        if (!(this.ribbonContextMenuShowAboveProperty != value))
          return;
        this.ribbonContextMenuShowAboveProperty = value;
        this.RaisePropertyChanged(nameof (RibbonContextMenuShowAbove));
      }
    }

    public string ScreenTipDisableReasonHeader
    {
      get => this.screenTipDisableReasonHeaderProperty ?? this.screenTipDisableReasonHeader;
      set
      {
        if (!(this.screenTipDisableReasonHeaderProperty != value))
          return;
        this.screenTipDisableReasonHeaderProperty = value;
        this.RaisePropertyChanged(nameof (ScreenTipDisableReasonHeader));
      }
    }

    public string CustomizeStatusBar
    {
      get => this.customizeStatusBarProperty ?? this.customizeStatusBar;
      set
      {
        if (!(this.customizeStatusBarProperty != value))
          return;
        this.customizeStatusBarProperty = value;
        this.RaisePropertyChanged(nameof (CustomizeStatusBar));
      }
    }

    public RibbonLocalization() => this.Culture = CultureInfo.CurrentUICulture;

    private void LoadCulture(CultureInfo culture)
    {
      switch (culture.TwoLetterISOLanguageName)
      {
        case "en":
          this.LoadEnglish();
          break;
        case "ru":
          this.LoadRussian();
          break;
        case "uk":
          this.LoadUkrainian();
          break;
        case "fa":
          this.LoadPersian();
          break;
        case "de":
          this.LoadGerman();
          break;
        case "hu":
          this.LoadHungarian();
          break;
        case "cs":
          this.LoadCzech();
          break;
        case "fr":
          this.LoadFrench();
          break;
        case "pl":
          this.LoadPolish();
          break;
        case "ja":
          this.LoadJapanese();
          break;
        case "nl":
          this.LoadDutch();
          break;
        case "pt":
          if (culture.Name == "pt-BR")
          {
            this.LoadPortugueseBrazilian();
            break;
          }
          this.LoadPortuguese();
          break;
        case "es":
          this.LoadSpanish();
          break;
        case "zh":
          this.LoadChinese();
          break;
        case "sv":
          this.LoadSwedish();
          break;
        case "sk":
          this.LoadSlovak();
          break;
        case "ro":
          this.LoadRomanian();
          break;
        case "it":
          this.LoadItalian();
          break;
        case "ar":
          this.LoadArabic();
          break;
        case "da":
          this.LoadDanish();
          break;
        case "az":
          this.LoadAzerbaijani();
          break;
        case "fi":
          this.LoadFinnish();
          break;
      }
      this.RaisePropertyChanged("BackstageButtonText");
      this.RaisePropertyChanged("BackstageButtonKeyTip");
      this.RaisePropertyChanged("MinimizeButtonScreenTipTitle");
      this.RaisePropertyChanged("MinimizeButtonScreenTipText");
      this.RaisePropertyChanged("ExpandButtonScreenTipTitle");
      this.RaisePropertyChanged("ExpandButtonScreenTipText");
      this.RaisePropertyChanged("QuickAccessToolBarDropDownButtonTooltip");
      this.RaisePropertyChanged("QuickAccessToolBarMoreControlsButtonTooltip");
      this.RaisePropertyChanged("QuickAccessToolBarMenuHeader");
      this.RaisePropertyChanged("QuickAccessToolBarMenuShowAbove");
      this.RaisePropertyChanged("QuickAccessToolBarMenuShowBelow");
      this.RaisePropertyChanged("RibbonContextMenuAddItem");
      this.RaisePropertyChanged("RibbonContextMenuAddGroup");
      this.RaisePropertyChanged("RibbonContextMenuAddGallery");
      this.RaisePropertyChanged("RibbonContextMenuAddMenu");
      this.RaisePropertyChanged("RibbonContextMenuRemoveItem");
      this.RaisePropertyChanged("RibbonContextMenuCustomizeRibbon");
      this.RaisePropertyChanged("RibbonContextMenuCustomizeQuickAccessToolBar");
      this.RaisePropertyChanged("RibbonContextMenuShowAbove");
      this.RaisePropertyChanged("RibbonContextMenuShowBelow");
      this.RaisePropertyChanged("RibbonContextMenuMinimizeRibbon");
      this.RaisePropertyChanged("ScreenTipDisableReasonHeader");
      this.RaisePropertyChanged("CustomizeStatusBar");
    }

    private void LoadEnglish()
    {
      this.backstageButtonText = "File";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "Minimize the Ribbon (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Show or hide the Ribbon\n\nWhen the Ribbon is hidden, only\nthe tab names are shown";
      this.expandButtonScreenTipTitle = "Expand the Ribbon (Ctrl + F1)";
      this.expandButtonScreenTipText = "Show or hide the Ribbon\n\nWhen the Ribbon is hidden, only\nthe tab names are shown";
      this.quickAccessToolBarDropDownButtonTooltip = "Customize Quick Access Toolbar";
      this.quickAccessToolBarMoreControlsButtonTooltip = "More controls";
      this.quickAccessToolBarMenuHeader = "Customize Quick Access Toolbar";
      this.quickAccessToolBarMenuShowAbove = "Show Above the Ribbon";
      this.quickAccessToolBarMenuShowBelow = "Show Below the Ribbon";
      this.ribbonContextMenuAddItem = "Add to Quick Access Toolbar";
      this.ribbonContextMenuAddGroup = "Add Group to Quick Access Toolbar";
      this.ribbonContextMenuAddGallery = "Add Gallery to Quick Access Toolbar";
      this.ribbonContextMenuAddMenu = "Add Menu to Quick Access Toolbar";
      this.ribbonContextMenuRemoveItem = "Remove from Quick Access Toolbar";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Customize Quick Access Toolbar...";
      this.ribbonContextMenuShowBelow = "Show Quick Access Toolbar Below the Ribbon";
      this.ribbonContextMenuShowAbove = "Show Quick Access Toolbar Above the Ribbon";
      this.ribbonContextMenuCustomizeRibbon = "Customize the Ribbon...";
      this.ribbonContextMenuMinimizeRibbon = "Minimize the Ribbon";
      this.screenTipDisableReasonHeader = "This command is currently disabled.";
    }

    private void LoadRussian()
    {
      this.backstageButtonText = "Файл";
      this.backstageButtonKeyTip = "Ф";
      this.minimizeButtonScreenTipTitle = "Свернуть ленту (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Отображение или скрытие ленты\n\nКогда лента скрыта, отображаются только\nимена вкладок.";
      this.expandButtonScreenTipTitle = "Развернуть ленту (Ctrl + F1)";
      this.expandButtonScreenTipText = "Отображение или скрытие ленты\n\nКогда лента скрыта, отображаются только\nимена вкладок.";
      this.quickAccessToolBarDropDownButtonTooltip = "Настройка панели быстрого доступа";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Другие элементы";
      this.quickAccessToolBarMenuHeader = "Настройка панели быстрого доступа";
      this.quickAccessToolBarMenuShowAbove = "Разместить над лентой";
      this.quickAccessToolBarMenuShowBelow = "Разместить под лентой";
      this.ribbonContextMenuAddItem = "Добавить на панель быстрого доступа";
      this.ribbonContextMenuAddGroup = "Добавить группу на панель быстрого доступа";
      this.ribbonContextMenuAddGallery = "Добавить коллекцию на панель быстрого доступа";
      this.ribbonContextMenuAddMenu = "Добавить меню на панель быстрого доступа";
      this.ribbonContextMenuRemoveItem = "Удалить с панели быстрого доступа";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Настройка панели быстрого доступа...";
      this.ribbonContextMenuShowBelow = "Разместить панель быстрого доступа под лентой";
      this.ribbonContextMenuShowAbove = "Разместить панель быстрого доступа над лентой";
      this.ribbonContextMenuCustomizeRibbon = "Настройка ленты...";
      this.ribbonContextMenuMinimizeRibbon = "Свернуть ленту";
      this.screenTipDisableReasonHeader = "В настоящее время эта команда отключена.";
      this.customizeStatusBar = "Настройка строки состояния";
    }

    private void LoadUkrainian()
    {
      this.backstageButtonText = "Файл";
      this.backstageButtonKeyTip = "Ф";
      this.minimizeButtonScreenTipTitle = "Сховати Стрічку (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Показати або сховати Стрічку\n\nКоли стрічка схована, видно\nтільки назви вкладок";
      this.expandButtonScreenTipTitle = "Показати Стрічку (Ctrl + F1)";
      this.expandButtonScreenTipText = "Показати або сховати Стрічку\n\nКоли стрічка схована, видно\nтільки назви вкладок";
      this.quickAccessToolBarDropDownButtonTooltip = "Налаштувати Панель Інструментів Швидкого Доступу";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Більше елементів";
      this.quickAccessToolBarMenuHeader = "Налаштувати Панель Інструментів Швидкого Доступу";
      this.quickAccessToolBarMenuShowAbove = "Показати Поверх Стрічки";
      this.quickAccessToolBarMenuShowBelow = "Показати Знизу Стрічки";
      this.ribbonContextMenuAddItem = "Додати до Панелі Інструментів Швидкого Доступу";
      this.ribbonContextMenuAddGroup = "Додати Групу до Панелі Інструментів Швидкого Доступу";
      this.ribbonContextMenuAddGallery = "Додати Галерею до Панелі Інструментів Швидкого Доступу";
      this.ribbonContextMenuAddMenu = "Додати Меню до Панелі Інструментів Швидкого Доступу";
      this.ribbonContextMenuRemoveItem = "Видалити з Панелі Інструментів Швидкого Доступу";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Налаштувати Панель Інструментів Швидкого Доступу...";
      this.ribbonContextMenuShowBelow = "Показати Панель Інструментів Швидкого Доступу Знизу Стрічки";
      this.ribbonContextMenuShowAbove = "Показати Панель Інструментів Швидкого Доступу Поверх Стрічки";
      this.ribbonContextMenuCustomizeRibbon = "Налаштувати Стрічку...";
      this.ribbonContextMenuMinimizeRibbon = "Зменшити Стрічку";
      this.screenTipDisableReasonHeader = "Ця команда на даний момент недоступна.";
    }

    private void LoadPersian()
    {
      this.backstageButtonText = "فایل";
      this.backstageButtonKeyTip = "ف";
      this.minimizeButtonScreenTipTitle = "کوچک کردن نوار (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "نمایش یا مخفی کردن نوار\n\nهنگامی که نوار مخفی است، تنها\nنام زبانه ها نمایش داده می شود.";
      this.expandButtonScreenTipTitle = "بزرگ کردن نوار (Ctrl + F1)";
      this.expandButtonScreenTipText = "نمایش یا مخفی کردن نوار\n\nهنگامی که نوار مخفی است، تنها\nنام زبانه ها نمایش داده می شود.";
      this.quickAccessToolBarDropDownButtonTooltip = "دلخواه سازی میله ابزار دسترسی سریع";
      this.quickAccessToolBarMoreControlsButtonTooltip = "ابزارهای دیگر";
      this.quickAccessToolBarMenuHeader = "دلخواه سازی میله ابزار دسترسی سریع";
      this.quickAccessToolBarMenuShowAbove = "نمایش در بالای نوار";
      this.quickAccessToolBarMenuShowBelow = "نمایش در پایین نوار";
      this.ribbonContextMenuAddItem = "اضافه کردن به میله ابزار دسترسی سریع";
      this.ribbonContextMenuAddGroup = "اضافه کردن گروه به میله ابزار دسترسی سریع";
      this.ribbonContextMenuAddGallery = "اضافه کردن گالری به میله ابزار دسترسی سریع";
      this.ribbonContextMenuAddMenu = "اضاقه کردن منو به میله ابزار دسترسی سریع";
      this.ribbonContextMenuRemoveItem = "حذف از میله ابزار دسترسی سریع";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "دلخواه سازی میله ابزار دسترسی سریع...";
      this.ribbonContextMenuShowBelow = "نمایش میله ابزار دسترسی سریع در پایین نوار";
      this.ribbonContextMenuShowAbove = "نمایش میله ابزار دسترسی سریع در بالای نوار";
      this.ribbonContextMenuCustomizeRibbon = "دلخواه سازی نوار...";
      this.ribbonContextMenuMinimizeRibbon = "کوچک کردن نوار";
    }

    private void LoadGerman()
    {
      this.backstageButtonText = "Datei";
      this.backstageButtonKeyTip = "D";
      this.minimizeButtonScreenTipTitle = "Menüband minimieren (Strg + F1)";
      this.minimizeButtonScreenTipText = "Das Menüband anzeigen oder ausblenden.\n\nWenn das Menüband\nausgeblendet ist, werden nur die\nRegisterkartennamen angezeigt.";
      this.expandButtonScreenTipTitle = "Menüband erweitern (Strg + F1)";
      this.expandButtonScreenTipText = "Das Menüband anzeigen oder ausblenden.\n\nWenn das Menüband\nausgeblendet ist, werden nur die\nRegisterkartennamen angezeigt.";
      this.quickAccessToolBarDropDownButtonTooltip = "Symbolleiste für den Schnellzugriff anpassen";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Weitere Befehle…";
      this.quickAccessToolBarMenuHeader = "Symbolleiste für den Schnellzugriff anpassen";
      this.quickAccessToolBarMenuShowAbove = "Über dem Menüband anzeigen";
      this.quickAccessToolBarMenuShowBelow = "Unter dem Menüband anzeigen";
      this.ribbonContextMenuAddItem = "Zu Symbolleiste für den Schnellzugriff hinzufügen";
      this.ribbonContextMenuAddGroup = "Gruppe zur Symbolleiste für den Schnellzugriff hinzufügen";
      this.ribbonContextMenuAddGallery = "Katalog zur Symbolleiste für den Schnellzugriff hinzufügen";
      this.ribbonContextMenuAddMenu = "Zu Symbolleiste für den Schnellzugriff hinzufügen";
      this.ribbonContextMenuRemoveItem = "Aus Symbolleiste für den Schnellzugriff entfernen";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Symbolleiste für den Schnellzugriff anpassen...";
      this.ribbonContextMenuShowBelow = "Symbolleiste für den Schnellzugriff unter dem Menüband anzeigen";
      this.ribbonContextMenuShowAbove = "Symbolleiste für den Schnellzugriff über dem Menüband anzeigen";
      this.ribbonContextMenuCustomizeRibbon = "Menüband anpassen...";
      this.ribbonContextMenuMinimizeRibbon = "Menüband minimieren";
    }

    private void LoadHungarian()
    {
      this.backstageButtonText = "Fájl";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "A menüszalag összecsukása (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Csak a lapnevek megjelenítése a menüszalagon";
      this.expandButtonScreenTipTitle = "Menüszalag kibontása (Ctrl + F1)";
      this.expandButtonScreenTipText = "A menüszalag megjelenítése úgy, hogy egy parancsra kattintás után is látható maradjon";
      this.quickAccessToolBarDropDownButtonTooltip = "Gyorselérési eszköztár testreszabása";
      this.quickAccessToolBarMoreControlsButtonTooltip = "További vezérlők";
      this.quickAccessToolBarMenuHeader = "Gyorselérési eszköztár testreszabása";
      this.quickAccessToolBarMenuShowAbove = "Megjelenítés a menüszalag alatt";
      this.quickAccessToolBarMenuShowBelow = "Megjelenítés a menüszalag felett";
      this.ribbonContextMenuAddItem = "Felvétel a gyorselérési eszköztárra";
      this.ribbonContextMenuAddGroup = "Felvétel a gyorselérési eszköztárra";
      this.ribbonContextMenuAddGallery = "Gyűjtemény felvétele a gyorselérési eszköztárra";
      this.ribbonContextMenuAddMenu = "Felvétel a gyorselérési eszköztárra";
      this.ribbonContextMenuRemoveItem = "Eltávolítás a gyorselérési eszköztárról";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Gyorselérési eszköztár testreszabása...";
      this.ribbonContextMenuShowBelow = "A gyorselérési eszköztár megjelenítése a menüszalag alatt";
      this.ribbonContextMenuShowAbove = "A gyorselérési eszköztár megjelenítése a menüszalag felett";
      this.ribbonContextMenuCustomizeRibbon = "Menüszalag testreszabása...";
      this.ribbonContextMenuMinimizeRibbon = " A menüszalag összecsukása";
      this.screenTipDisableReasonHeader = "Ez a parancs jelenleg nem használható.";
    }

    private void LoadCzech()
    {
      this.backstageButtonText = "Soubor";
      this.backstageButtonKeyTip = "S";
      this.minimizeButtonScreenTipTitle = "Skrýt pás karet (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Zobrazit nebo skrýt pás karet\n\nJe-li pás karet skrytý, jsou\nzobrazeny pouze názvy karet";
      this.expandButtonScreenTipTitle = "Zobrazit pás karet (Ctrl + F1)";
      this.expandButtonScreenTipText = "Zobrazit nebo skrýt pás karet\n\nJe-li pás karet skrytý, jsou\nzobrazeny pouze názvy karet";
      this.quickAccessToolBarDropDownButtonTooltip = "Přizpůsobit panel nástrojů Rychlý přístup";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Další příkazy";
      this.quickAccessToolBarMenuHeader = "Přizpůsobit panel nástrojů Rychlý přístup";
      this.quickAccessToolBarMenuShowAbove = "Zobrazit nad pásem karet";
      this.quickAccessToolBarMenuShowBelow = "Zobrazit pod pásem karet";
      this.ribbonContextMenuAddItem = "Přidat na panel nástrojů Rychlý přístup";
      this.ribbonContextMenuAddGroup = "Přidat na panel nástrojů Rychlý přístup";
      this.ribbonContextMenuAddGallery = "Přidat galerii na panel nástrojů Rychlý přístup";
      this.ribbonContextMenuAddMenu = "Přidat na panel nástrojů Rychlý přístup";
      this.ribbonContextMenuRemoveItem = "Odebrat z panelu nástrojů Rychlý přístup";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Přizpůsobit panel nástrojů Rychlý přístup...";
      this.ribbonContextMenuShowBelow = "Zobrazit panel nástrojů Rychlý přístup pod pásem karet";
      this.ribbonContextMenuShowAbove = "Zobrazit panel nástrojů Rychlý přístup nad pásem karet";
      this.ribbonContextMenuCustomizeRibbon = "Přizpůsobit pás karet...";
      this.ribbonContextMenuMinimizeRibbon = "Skrýt pás karet";
      this.screenTipDisableReasonHeader = "Tento příkaz je aktuálně zakázán.";
    }

    private void LoadFrench()
    {
      this.backstageButtonText = "Fichier";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "Minimiser le Ruban (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Afficher ou masquer le Ruban \n\nQuand le Ruban est masqué, seul\nles noms sont affichés";
      this.expandButtonScreenTipTitle = "Agrandir le Ruban (Ctrl + F1)";
      this.expandButtonScreenTipText = "Afficher ou masquer le Ruban \n\nQuand le Ruban est masqué, seul\nles noms sont affichés";
      this.quickAccessToolBarDropDownButtonTooltip = "Personnaliser la barre d'outils Accès Rapide";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Plus de contrôles";
      this.quickAccessToolBarMenuHeader = "Personnaliser la barre d'outil Accès Rapide";
      this.quickAccessToolBarMenuShowAbove = "Afficher au dessus du Ruban";
      this.quickAccessToolBarMenuShowBelow = "Afficher en dessous du Ruban";
      this.ribbonContextMenuAddItem = "Ajouter un élément à la barre d'outils Accès Rapide";
      this.ribbonContextMenuAddGroup = "Ajouter un groupe à la barre d'outils Accès Rapide";
      this.ribbonContextMenuAddGallery = "Ajouter une galerie à la barre d'outils Accès Rapide";
      this.ribbonContextMenuAddMenu = "Ajouter un menu à la barre d'outils Accès Rapide";
      this.ribbonContextMenuRemoveItem = "Supprimer de la barre d'outils Accès Rapide";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Personnaliser la barre d'outils Accès Rapide...";
      this.ribbonContextMenuShowBelow = "Afficher la barre d'outils Accès Rapide en dessous du Ruban";
      this.ribbonContextMenuShowAbove = "Afficher la barre d'outils Accès Rapide au dessus du Ruban";
      this.ribbonContextMenuCustomizeRibbon = "Personnaliser le Ruban...";
      this.ribbonContextMenuMinimizeRibbon = "Minimiser le Ruban";
      this.screenTipDisableReasonHeader = "Cette commande est actuellement désactivée.";
    }

    private void LoadPolish()
    {
      this.backstageButtonText = "Plik";
      this.backstageButtonKeyTip = "P";
      this.minimizeButtonScreenTipTitle = "Minimalizuj Wstążkę (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Pokazuje lub ukrywa Wstążkę\n\nGdy Wstążka jest ukryta, tylko\nnazwy zakładek są widoczne";
      this.expandButtonScreenTipTitle = "Rozwiń Wstążkę (Ctrl + F1)";
      this.expandButtonScreenTipText = "Pokazuje lub ukrywa Wstążkę\n\nGdy Wstążka jest ukryta, tylko\nnazwy zakładek są widoczne";
      this.quickAccessToolBarDropDownButtonTooltip = "Dostosuj pasek narzędzi Szybki dostęp";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Więcej poleceń...";
      this.quickAccessToolBarMenuHeader = "Dostosuj pasek narzędzi Szybki dostęp";
      this.quickAccessToolBarMenuShowAbove = "Pokaż powyżej Wstążki";
      this.quickAccessToolBarMenuShowBelow = "Pokaż poniżej Wstążki";
      this.ribbonContextMenuAddItem = "Dodaj do paska narzędzi Szybki dostęp";
      this.ribbonContextMenuAddGroup = "Dodaj Grupę do paska narzędzi Szybki dostęp";
      this.ribbonContextMenuAddGallery = "Dodaj Galerię do paska narzędzi Szybki dostęp";
      this.ribbonContextMenuAddMenu = "Dodaj do paska narzędzi Szybki dostęp";
      this.ribbonContextMenuRemoveItem = "Usuń z paska narzędzi Szybki dostęp";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Dostosuj pasek narzędzi Szybki dostęp...";
      this.ribbonContextMenuShowBelow = "Pokaż pasek Szybki dostęp poniżej Wstążki";
      this.ribbonContextMenuShowAbove = "Pokaż pasek Szybki dostęp powyżej Wstążki";
      this.ribbonContextMenuCustomizeRibbon = "Dostosuj Wstążkę...";
      this.ribbonContextMenuMinimizeRibbon = "Minimalizuj Wstążkę";
    }

    private void LoadJapanese()
    {
      this.backstageButtonText = "ファイル";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "リボンの最小化 (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "リボンの表示/非表示を切り替えます。\n\nリボンを非表示にすると、タブ名のみが表示されます。";
      this.expandButtonScreenTipTitle = "リボンの展開 (Ctrl + F1)";
      this.expandButtonScreenTipText = "リボンの表示/非表示を切り替えます。\n\nリボンを非表示にすると、タブ名のみが表示されます。";
      this.quickAccessToolBarDropDownButtonTooltip = "クイック アクセス ツール バーのユーザー設定";
      this.quickAccessToolBarMoreControlsButtonTooltip = "その他のボタン";
      this.quickAccessToolBarMenuHeader = "クイック アクセス ツール バーのユーザー設定";
      this.quickAccessToolBarMenuShowAbove = "リボンの上に表示";
      this.quickAccessToolBarMenuShowBelow = "リボンの下に表示";
      this.ribbonContextMenuAddItem = "クイック アクセス ツール バーに追加";
      this.ribbonContextMenuAddGroup = "グループをクイック アクセス ツール バーに追加";
      this.ribbonContextMenuAddGallery = "ギャラリーをクイック アクセス ツール バーに追加";
      this.ribbonContextMenuAddMenu = "メニューをクイック アクセス ツール バーに追加";
      this.ribbonContextMenuRemoveItem = "クイック アクセス ツール バーから削除";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "クイック アクセス ツール バーのユーザー設定...";
      this.ribbonContextMenuShowBelow = "クイック アクセス ツール バーをリボンの下に表示";
      this.ribbonContextMenuShowAbove = "クイック アクセス ツール バーをリボンの上に表示";
      this.ribbonContextMenuCustomizeRibbon = "リボンのユーザー設定...";
      this.ribbonContextMenuMinimizeRibbon = "リボンの最小化";
    }

    private void LoadDutch()
    {
      this.backstageButtonText = "Bestand";
      this.backstageButtonKeyTip = "B";
      this.minimizeButtonScreenTipTitle = "Het lint minimaliseren (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Verberg of laat het lint zien\n\nWanneer het lint verborgen is, \nzijn alleen de tabulatie namen zichtbaar";
      this.expandButtonScreenTipTitle = "Het lint Maximaliseren (Ctrl + F1)";
      this.expandButtonScreenTipText = "Verberg of laat het lint zien\n\nWanneer het lint verborgen is,\nzijn alleen de tabulatie namen zichtbaar";
      this.quickAccessToolBarDropDownButtonTooltip = "Werkbalk snelle toegang aanpassen";
      this.quickAccessToolBarMoreControlsButtonTooltip = "meer opdrachten";
      this.quickAccessToolBarMenuHeader = " Werkbalk snelle toegang aanpassen ";
      this.quickAccessToolBarMenuShowAbove = "Boven het lint weergeven";
      this.quickAccessToolBarMenuShowBelow = "beneden het lint weergeven";
      this.ribbonContextMenuAddItem = "Menu toevoegen aan werkbalk snelle toegang";
      this.ribbonContextMenuAddGroup = "Groep toevoegen aan werkbalk snelle toegang";
      this.ribbonContextMenuAddGallery = "Galerij toevoegen aan werkbalk snelle toegang";
      this.ribbonContextMenuAddMenu = " Menu toevoegen aan werkbalk snelle toegang ";
      this.ribbonContextMenuRemoveItem = " Verwijder uit werkbalk snelle toegang ";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Customize Quick Access Toolbar...";
      this.ribbonContextMenuShowBelow = " Werkbalk snelle toegang onder het lint weergeven";
      this.ribbonContextMenuShowAbove = " Werkbalk snelle toegang boven het lint weergeven ";
      this.ribbonContextMenuCustomizeRibbon = "Lint aanpassen...";
      this.ribbonContextMenuMinimizeRibbon = " Het lint minimaliseren";
    }

    private void LoadPortugueseBrazilian()
    {
      this.backstageButtonText = "Arquivo";
      this.backstageButtonKeyTip = "A";
      this.minimizeButtonScreenTipTitle = "Minimizar o Ribbon (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Mostrar ou esconder o  Ribbon\n\nQuando o Ribbon estiver escondido, somente\no nome das abas serão mostrados";
      this.expandButtonScreenTipTitle = "Expandir o Ribbon (Ctrl + F1)";
      this.expandButtonScreenTipText = "Mostrar ou esconder o  Ribbon\n\nQuando o Ribbon estiver escondido, somente\no nome das abas serão mostrados";
      this.quickAccessToolBarDropDownButtonTooltip = "Customizar Barra de acesso rápido";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Mais controles";
      this.quickAccessToolBarMenuHeader = " Customizar Barra de acesso rápido";
      this.quickAccessToolBarMenuShowAbove = "Mostrar acima do Ribbon";
      this.quickAccessToolBarMenuShowBelow = "Mostrar abaixo do Ribbon";
      this.ribbonContextMenuAddItem = "Adicionar para Barra de acesso rápido";
      this.ribbonContextMenuAddGroup = " Adicionar o grupo para Barra de acesso rápido";
      this.ribbonContextMenuAddGallery = "Adicionar a galeria para Barra de acesso rápido";
      this.ribbonContextMenuAddMenu = " Adicionar o menu para Barra de acesso rápido";
      this.ribbonContextMenuRemoveItem = "Remover da Barra de acesso rápido";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Customizar Barra de acesso rápido...";
      this.ribbonContextMenuShowBelow = "Mostrar Barra de acesso rápido abaixo do Ribbon";
      this.ribbonContextMenuShowAbove = "Mostrar Barra de acesso rápido acima do Ribbon";
      this.ribbonContextMenuCustomizeRibbon = "Customizar o Ribbon...";
      this.ribbonContextMenuMinimizeRibbon = "Minimizar o Ribbon";
      this.screenTipDisableReasonHeader = "Este comando está desativado.";
    }

    private void LoadSpanish()
    {
      this.backstageButtonText = "Archivo";
      this.backstageButtonKeyTip = "A";
      this.minimizeButtonScreenTipTitle = "Minimizar la cinta (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Muestra u oculta la cinta\n\nCuando la cinta está oculta, sólo\nse muestran los nombres de las pestañas";
      this.expandButtonScreenTipTitle = "Expandir la cinta (Ctrl + F1)";
      this.expandButtonScreenTipText = "Muestra u oculta la cinta\n\nCuando la cinta está oculta, sólo\nse muestran los nombres de las pestañas";
      this.quickAccessToolBarDropDownButtonTooltip = "Personalizar barra de herramientas de acceso rápido";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Más controles";
      this.quickAccessToolBarMenuHeader = "Personalizar barra de herramientas de acceso rápido";
      this.quickAccessToolBarMenuShowAbove = "Mostrar sobre la cinta";
      this.quickAccessToolBarMenuShowBelow = "Mostrar bajo la cinta";
      this.ribbonContextMenuAddItem = "Agregar a la barra de herramientas de acceso rápido";
      this.ribbonContextMenuAddGroup = "Agregar grupo a la barra de herramientas de acceso rápido";
      this.ribbonContextMenuAddGallery = "Agregar galería a la barra de herramientas de acceso rápido";
      this.ribbonContextMenuAddMenu = "Agregar menú a la barra de herramientas de acceso rápido";
      this.ribbonContextMenuRemoveItem = "Quitar de la barra de herramientas de acceso rápido";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Personalizar la barra de herramientas de acceso rápido...";
      this.ribbonContextMenuShowBelow = "Mostrar barra de herramientas de acceso rápido bajo la cinta";
      this.ribbonContextMenuShowAbove = "Mostrar barra de herramientas de acceso rápido sobre la cinta";
      this.ribbonContextMenuCustomizeRibbon = "Personalizar la cinta...";
      this.ribbonContextMenuMinimizeRibbon = "Minimizar la cinta";
    }

    private void LoadChinese()
    {
      this.backstageButtonText = "文件";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "功能区最小化 (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "隐藏功能区时，仅显示选项卡名称";
      this.expandButtonScreenTipTitle = "展开功能区 (Ctrl + F1)";
      this.expandButtonScreenTipText = "隐藏功能区时，仅显示选项卡名称";
      this.quickAccessToolBarDropDownButtonTooltip = "自定义快速访问具栏";
      this.quickAccessToolBarMoreControlsButtonTooltip = "其他命令";
      this.quickAccessToolBarMenuHeader = "自定义快速访问工具栏";
      this.quickAccessToolBarMenuShowAbove = "在功能区上方显示";
      this.quickAccessToolBarMenuShowBelow = "在功能区下方显示";
      this.ribbonContextMenuAddItem = "添加到快速访问工具栏";
      this.ribbonContextMenuAddGroup = "在快速访问工具栏中添加组";
      this.ribbonContextMenuAddGallery = "在快速访问工具栏中添加样式";
      this.ribbonContextMenuAddMenu = "在快速访问工具栏中添加菜单";
      this.ribbonContextMenuRemoveItem = "在快速访问工具栏中移除";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "自定义快速访问工具栏...";
      this.ribbonContextMenuShowBelow = "在功能区下方显示快速访问工具栏";
      this.ribbonContextMenuShowAbove = "在功能区上方显示快速访问工具栏";
      this.ribbonContextMenuCustomizeRibbon = "自定义功能区...";
      this.ribbonContextMenuMinimizeRibbon = "功能区最小化";
    }

    private void LoadSwedish()
    {
      this.backstageButtonText = "Arkiv";
      this.backstageButtonKeyTip = "A";
      this.minimizeButtonScreenTipTitle = "Minimera menyfliksområdet (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Visa eller göm menyfliksområdet \n\nNär menyfliksområdet är dolt, visas\nendast flikarna";
      this.expandButtonScreenTipTitle = "Expandera menyfliksområdet (Ctrl + F1)";
      this.expandButtonScreenTipText = "Visa eller göm menyfliksområdet \n\nNär menyfliksområdet är dolt, visas\nendast flikarna";
      this.quickAccessToolBarDropDownButtonTooltip = "Anpassa verktygsfältet Snabbåtkomst ";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Fler kommandon";
      this.quickAccessToolBarMenuHeader = " Anpassa verktygsfältet Snabbåtkomst";
      this.quickAccessToolBarMenuShowAbove = "Visa ovanför menyfliksområdet";
      this.quickAccessToolBarMenuShowBelow = "Visa under menyfliksområdet";
      this.ribbonContextMenuAddItem = "Lägg till i verktygsfältet Snabbåtkomst";
      this.ribbonContextMenuAddGroup = "Lägg till i verktygsfältet Snabbåtkomst";
      this.ribbonContextMenuAddGallery = "Lägg till galleriet i verktygsfältet Snabbåtkomst";
      this.ribbonContextMenuAddMenu = " Lägg till menyn i verktygsfältet Snabbåtkomst ";
      this.ribbonContextMenuRemoveItem = "Ta bort från verktygsfältet Snabbåtkomst";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Anpassa verktygsfältet Snabbåtkomst...";
      this.ribbonContextMenuShowBelow = " Visa verktygsfältet Snabbåtkomst under menyfliksområdet";
      this.ribbonContextMenuShowAbove = " Visa verktygsfältet Snabbåtkomst ovanför menyfliksområdet ";
      this.ribbonContextMenuCustomizeRibbon = "Anpassa menyfliksområdet...";
      this.ribbonContextMenuMinimizeRibbon = "Minimera menyfliksområdet";
    }

    private void LoadSlovak()
    {
      this.backstageButtonText = "Súbor";
      this.backstageButtonKeyTip = "S";
      this.minimizeButtonScreenTipTitle = "Skryť pás s nástrojmi (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Zobraziť alebo skryť pás s nástrojmi\n\nKeď je pás s nástrojmi skrytý, sú zobrazené iba názvy kariet";
      this.expandButtonScreenTipTitle = "Zobraziť pás s nástrojmi (Ctrl + F1)";
      this.expandButtonScreenTipText = " Zobraziť alebo skryť pás s nástrojmi\n\nKeď je pás s nástrojmi skrytý, sú zobrazené iba názvy kariet ";
      this.quickAccessToolBarDropDownButtonTooltip = "Prispôsobenie panela s nástrojmi Rýchly prístup";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Ďalšie príkazy";
      this.quickAccessToolBarMenuHeader = "Prispôsobenie panela s nástrojmi Rýchly prístup";
      this.quickAccessToolBarMenuShowAbove = " Zobraziť nad pásom s nástrojmi ";
      this.quickAccessToolBarMenuShowBelow = "Zobraziť pod pásom s nástrojmi";
      this.ribbonContextMenuAddItem = "Pridať na panel s nástrojmi Rýchly prístup";
      this.ribbonContextMenuAddGroup = " Pridať na panel s nástrojmi Rýchly prístup ";
      this.ribbonContextMenuAddGallery = " Pridať galériu do panela s nástrojmi Rýchly prístup ";
      this.ribbonContextMenuAddMenu = "Pridať na panel s nástrojmi Rýchly prístup";
      this.ribbonContextMenuRemoveItem = "Odstrániť z panela s nástrojmi Rýchly prístup ";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = " Prispôsobenie panela s nástrojmi Rýchly prístup...";
      this.ribbonContextMenuShowBelow = "Panel s nástrojmi Rýchly prístup zobraziť pod panelom s nástrojmi";
      this.ribbonContextMenuShowAbove = "Panel s nástrojmi Rýchly prístup zobraziť nad panelom s nástrojmi ";
      this.ribbonContextMenuCustomizeRibbon = "Prispôsobenie panela s nástrojmi Rýchly prístup...";
      this.ribbonContextMenuMinimizeRibbon = "Minimalizovať pás s nástrojmi";
    }

    private void LoadRomanian()
    {
      this.backstageButtonText = "Fișier";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "Minimizează Ribbon-ul (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Afișează sau ascunde Ribbon-ul\nCând Ribbon-ul este ascuns, sunt\nafișate doar numele taburilor";
      this.expandButtonScreenTipTitle = "Expandează Ribbon-ul (Ctrl + F1)";
      this.expandButtonScreenTipText = "Afișează sau ascunde Ribbon-ul\nCând Ribbon-ul este ascuns, sunt\nafișate doar numele taburilor";
      this.quickAccessToolBarDropDownButtonTooltip = "Personalizează Bara de Acces Rapid";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Mai multe controale";
      this.quickAccessToolBarMenuHeader = "Personalizează Bara de Acces Rapid";
      this.quickAccessToolBarMenuShowAbove = "Afișează peste Ribbon";
      this.quickAccessToolBarMenuShowBelow = "Afișează sub Ribbon";
      this.ribbonContextMenuAddItem = "Adaugă la Bara de Acess Rapid";
      this.ribbonContextMenuAddGroup = "Adaugă Grupul la Bara de Acess Rapid";
      this.ribbonContextMenuAddGallery = "Adaugă Galeria la Bara de Acess Rapid";
      this.ribbonContextMenuAddMenu = "Adaugă Meniul la Bara de Acess Rapid";
      this.ribbonContextMenuRemoveItem = "Eimină din Bara de Acess Rapid";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Personalizează Bara de Acces Rapid...";
      this.ribbonContextMenuShowBelow = "Afișează Bara de Acces Rapid sub Ribbon";
      this.ribbonContextMenuShowAbove = "Afișează Bara de Acces Rapid peste Ribbon";
      this.ribbonContextMenuCustomizeRibbon = "Personalizează Ribbon-ul...";
      this.ribbonContextMenuMinimizeRibbon = "Minimizează Ribbon-ul...";
      this.screenTipDisableReasonHeader = "Această comandă nu este disponibilă momentan.";
    }

    private void LoadItalian()
    {
      this.backstageButtonText = "File";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "Riduci a icona barra multifunzione (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Consente di visualizzare solo i nomi \ndelle schede nella barra multifunzione.";
      this.expandButtonScreenTipTitle = "Espandi la barra multifunzione (Ctrl + F1)";
      this.expandButtonScreenTipText = "Visualizza la barra multifunzione in modo\n che rimanga sempre espansa, anche se l’utente \nha fatto click su un comando.";
      this.quickAccessToolBarDropDownButtonTooltip = "Personalizza barra di accesso rapido";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Altri comandi…";
      this.quickAccessToolBarMenuHeader = "Personalizza barra di accesso rapido";
      this.quickAccessToolBarMenuShowAbove = "Mostra sopra la barra multifunzione";
      this.quickAccessToolBarMenuShowBelow = "Mostra sotto la barra multifunzione";
      this.ribbonContextMenuAddItem = "Aggiungi alla barra di accesso rapido";
      this.ribbonContextMenuAddGroup = "Aggiungi gruppo alla barra di accesso rapido";
      this.ribbonContextMenuAddGallery = "Aggiungi raccolta alla barra di accesso rapido";
      this.ribbonContextMenuAddMenu = "Aggiungi menu alla barra di accesso rapido";
      this.ribbonContextMenuRemoveItem = "Rimuovi dalla barra di accesso rapido";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Personalizza barra di accesso rapido...";
      this.ribbonContextMenuShowBelow = "Mostra la barra di accesso rapido sotto la barra multifunzione";
      this.ribbonContextMenuShowAbove = "Mostra la barra di accesso rapido sopra la barra multifunzione";
      this.ribbonContextMenuCustomizeRibbon = "Personalizza barra multifunzione...";
      this.ribbonContextMenuMinimizeRibbon = "Riduci a icona barra multifunzione";
      this.screenTipDisableReasonHeader = "Questo commando è disattivato.";
    }

    private void LoadArabic()
    {
      this.backstageButtonText = "ملف    ";
      this.backstageButtonKeyTip = "م    ";
      this.minimizeButtonScreenTipTitle = "(Ctrl + F1)تصغير الشريط ";
      this.minimizeButtonScreenTipText = "إظهار أسماء علامات التبويب فقط على الشريط.";
      this.expandButtonScreenTipTitle = "(Ctrl + F1)توسيع الشريط ";
      this.expandButtonScreenTipText = "إظهار الشريط بحيث يكون موسعاً دائماً حتى بعد النقر فوق أمر.";
      this.quickAccessToolBarDropDownButtonTooltip = "تخصيص شريط أدوات الوصول السريع";
      this.quickAccessToolBarMoreControlsButtonTooltip = "أوامر إضافية";
      this.quickAccessToolBarMenuHeader = "تخصيص شريط أدوات الوصول السريع";
      this.quickAccessToolBarMenuShowAbove = "إظهار أعلى الشريط";
      this.quickAccessToolBarMenuShowBelow = "إظهار أسفل الشريط";
      this.ribbonContextMenuAddItem = "إضافة إلى شريط أدوات الوصول السريع";
      this.ribbonContextMenuAddGroup = "إضافة إلى شريط أدوات الوصول السريع";
      this.ribbonContextMenuAddGallery = "إضافة إلى شريط أدوات الوصول السريع";
      this.ribbonContextMenuAddMenu = "إضافة إلى شريط أدوات الوصول السريع";
      this.ribbonContextMenuRemoveItem = "إزالة إلى شريط أدوات الوصول السريع";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "تخصيص شريط أدوات الوصول السريع...";
      this.ribbonContextMenuShowBelow = "إظهار شريط أدوات الوصول السريع أسفل الشريط";
      this.ribbonContextMenuShowAbove = "إظهار شريط أدوات الوصول السريع أعلى الشريط";
      this.ribbonContextMenuCustomizeRibbon = "تخصيص الشريط...";
      this.ribbonContextMenuMinimizeRibbon = "تصغير الشريط";
      this.screenTipDisableReasonHeader = "تم حالياً تعطيل هذا الأمر.";
    }

    private void LoadDanish()
    {
      this.backstageButtonText = "Filer";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "Minimer båndet (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Vis kun fanenavnene på båndet.";
      this.expandButtonScreenTipTitle = "Udvid båndet (Ctrl + F1)";
      this.expandButtonScreenTipText = "Vis båndet, så det altid er udvidet, selv\nnår du klikker på en kommando.";
      this.quickAccessToolBarDropDownButtonTooltip = "Tilpas værktøjslinjen Hurtig adgang";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Flere kontrolelementer";
      this.quickAccessToolBarMenuHeader = " Tilpas værktøjslinjen Hurtig adgang";
      this.quickAccessToolBarMenuShowAbove = "Vis ovenover båndet";
      this.quickAccessToolBarMenuShowBelow = "Vis under båndet";
      this.ribbonContextMenuAddItem = "Føj til værktøjslinjen Hurtig adgang";
      this.ribbonContextMenuAddGroup = "Føj til værktøjslinjen Hurtig adgang";
      this.ribbonContextMenuAddGallery = "Tilføj Galleri til værktøjslinjen Hurtig adgang";
      this.ribbonContextMenuAddMenu = "Føj til værktøjslinjen Hurtig adgang";
      this.ribbonContextMenuRemoveItem = "Fjern fra værktøjslinjen Hurtig adgang";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Tilpas værktøjslinjen Hurtig adgang...";
      this.ribbonContextMenuShowBelow = "Vis værktøjslinjen Hurtig adgang under båndet";
      this.ribbonContextMenuShowAbove = "Vis værktøjslinjen Hurtig adgang ovenover båndet";
      this.ribbonContextMenuCustomizeRibbon = "Tilpas båndet...";
      this.ribbonContextMenuMinimizeRibbon = "Minimer båndet";
      this.screenTipDisableReasonHeader = "Denne kommando er aktuelt deaktiveret.";
    }

    private void LoadPortuguese()
    {
      this.backstageButtonText = "Ficheiro";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "Minimizar o Friso (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Mostrar apenas os nomes dos separadores\n no Frisos.";
      this.expandButtonScreenTipTitle = "Expandir o Friso (Ctrl + F1)";
      this.expandButtonScreenTipText = "Mostrar o Friso de modo a aparecer sempre\nexpandido mesmo depois de clicar num\ncomando.";
      this.quickAccessToolBarDropDownButtonTooltip = "Personalizar Barra de Ferramentas de Acesso Rápido";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Mais Comandos...";
      this.quickAccessToolBarMenuHeader = "Personalizar Barra de Ferramentas de Acesso Rápido";
      this.quickAccessToolBarMenuShowAbove = "Mostrar Acima do Friso";
      this.quickAccessToolBarMenuShowBelow = "Mostrar Abaixo do Friso";
      this.ribbonContextMenuAddItem = "Adicionar à Barra de Ferramentas de Acesso Rápido";
      this.ribbonContextMenuAddGroup = "Adicionar Grupo à Barra de Ferramentas de Acesso Rápido";
      this.ribbonContextMenuAddGallery = "Adicionar Galeria à Barra de Ferramentas de Acesso Rápido";
      this.ribbonContextMenuAddMenu = "Adicionar Menu à Barra de Ferramentas de Acesso Rápido";
      this.ribbonContextMenuRemoveItem = "Remover da Barra de Ferramentas de Acesso Rápido";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Personalizar Barra de Ferramentas de Acesso Rápido...";
      this.ribbonContextMenuShowBelow = "Mostrar Barra de Ferramentas de Acesso Rápido Abaixo do Friso";
      this.ribbonContextMenuShowAbove = "Mostrar Barra de Ferramentas de Acesso Rápido Acima do Friso";
      this.ribbonContextMenuCustomizeRibbon = "Personalizar o Friso...";
      this.ribbonContextMenuMinimizeRibbon = "Minimizar o Friso";
      this.screenTipDisableReasonHeader = "Este comando está desactivado actualmente.";
    }

    private void LoadAzerbaijani()
    {
      this.backstageButtonText = "Fayl";
      this.backstageButtonKeyTip = "F";
      this.minimizeButtonScreenTipTitle = "Menyu lentini kiçilt (Ctrl + F1)";
      this.minimizeButtonScreenTipText = " Menyu lentini göstər və ya gizlət\n\n Menyu lentini kiçiləndə, yalnız\n tabların adları göstərilir";
      this.expandButtonScreenTipTitle = " Menyu lentini böyüt(Ctrl + F1)";
      this.expandButtonScreenTipText = " Menyu lentini göstər və ya gizlət\n\n Menyu lentini gizldəndə, yalnız, \n tabların adları göstərilir";
      this.quickAccessToolBarDropDownButtonTooltip = "Sürətli Keçidin Alətlərini fərdiləşdir";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Digər nəzarət vasitələri";
      this.quickAccessToolBarMenuHeader = " Sürətli Keçidin Alətlərini fərdiləşdir ";
      this.quickAccessToolBarMenuShowAbove = "Menyu lentinin üstündə göstər";
      this.quickAccessToolBarMenuShowBelow = " Menyu lentinin altında göstər ";
      this.ribbonContextMenuAddItem = "Sürətli Keçidin Alətlərinə əlavə et";
      this.ribbonContextMenuAddGroup = " Sürətli Keçidin Alətlərinə Qrup əlavə et ";
      this.ribbonContextMenuAddGallery = " Sürətli Keçidin Alətlərinə Qalereya əlavə et";
      this.ribbonContextMenuAddMenu = " Sürətli Keçidin Alətlərinə Menyu əlavə et";
      this.ribbonContextMenuRemoveItem = " Sürətli Keçidin Alətlərindən sil";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = " Sürətli Keçidin Alətlərini fərdiləşdir...";
      this.ribbonContextMenuShowBelow = " Sürətli Keçidin Alətlərini Menyu lentinin altında göstər ";
      this.ribbonContextMenuShowAbove = " Sürətli Keçidin Alətlərini Menyu lentinin üstündə göstər ";
      this.ribbonContextMenuCustomizeRibbon = "Menyu lentini fərdiləşdir...";
      this.ribbonContextMenuMinimizeRibbon = " Menyu lentini kiçilt";
    }

    private void LoadFinnish()
    {
      this.backstageButtonText = "Tiedosto";
      this.backstageButtonKeyTip = "T";
      this.minimizeButtonScreenTipTitle = "Pienennä valintanauha (Ctrl + F1)";
      this.minimizeButtonScreenTipText = "Näytä valintanauhassa vain\nvälilehtien nimet";
      this.expandButtonScreenTipTitle = "Laajenna valintanauha (Ctrl + F1)";
      this.expandButtonScreenTipText = "Näytä valintanauha aina\nlaajennettuna silloinkin, kun\nvalitset komennon";
      this.quickAccessToolBarDropDownButtonTooltip = "Mukauta pikatyökaluriviä";
      this.quickAccessToolBarMoreControlsButtonTooltip = "Lisää valintoja";
      this.quickAccessToolBarMenuHeader = "Mukauta pikatyökaluriviä";
      this.quickAccessToolBarMenuShowAbove = "Näytä valintanauhan yläpuolella";
      this.quickAccessToolBarMenuShowBelow = "Näytä valintanauhan alapuolella";
      this.ribbonContextMenuAddItem = "Lisää pikatyökaluriville";
      this.ribbonContextMenuAddGroup = "Lisää ryhmä pikatyökaluriviin";
      this.ribbonContextMenuAddGallery = "Lisää valikoima pikatyökaluriviin";
      this.ribbonContextMenuAddMenu = "Lisää valikko pikatyökaluriviin";
      this.ribbonContextMenuRemoveItem = "Poista pikatyökaluriviltä";
      this.ribbonContextMenuCustomizeQuickAccessToolbar = "Mukauta pikatyökaluriviä...";
      this.ribbonContextMenuShowBelow = "Näytä pikatyökalurivi valintanauhan alapuolella";
      this.ribbonContextMenuShowAbove = "Näytä pikatyökalurivi valintanauhan yläpuolella";
      this.ribbonContextMenuCustomizeRibbon = "Mukauta valintanauhaa...";
      this.ribbonContextMenuMinimizeRibbon = "Pienennä valintanauha";
      this.screenTipDisableReasonHeader = "Tämä komento on tällä hetkellä poissa käytöstä";
    }
  }
}
