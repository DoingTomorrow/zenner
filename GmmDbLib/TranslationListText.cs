// Decompiled with JetBrains decompiler
// Type: GmmDbLib.TranslationListText
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;

#nullable disable
namespace GmmDbLib
{
  public class TranslationListText : IComparable
  {
    public bool IsMessage = false;
    public TranslatorTwoLetterISOLanguageNames UsedLanguages;
    public string TranslationReferenceText = string.Empty;
    public string TranslationText = string.Empty;
    public int? MessageNumber;

    public Tg TranslationGroup { get; private set; }

    public string KeyText { get; private set; }

    public string DefaultText { get; private set; }

    public TranslationListText(string listText)
    {
      int length = listText.IndexOf(':');
      string str = listText.Substring(0, length);
      Tg result;
      if (!Enum.TryParse<Tg>(str, out result))
        throw new Exception("Unknown TranslationGroup : " + str + " -> Use newer software version for translation !!!");
      this.TranslationGroup = result;
      this.KeyText = listText.Substring(length + 1);
    }

    public TranslationListText(Tg translationGroup, string keyText)
    {
      this.TranslationGroup = translationGroup;
      this.KeyText = keyText;
    }

    public TranslationListText(Tg translationGroup, string keyText, string defaultText)
    {
      this.TranslationGroup = translationGroup;
      this.KeyText = keyText;
      this.DefaultText = defaultText;
    }

    public TranslationListText(Tg translationGroup, string keyText, bool isMessage)
    {
      this.TranslationGroup = translationGroup;
      this.KeyText = keyText;
      this.IsMessage = isMessage;
    }

    public TranslationListText(
      Tg translationGroup,
      string keyText,
      string defaultText,
      bool isMessage)
    {
      this.TranslationGroup = translationGroup;
      this.KeyText = keyText;
      this.DefaultText = defaultText;
      this.IsMessage = isMessage;
    }

    public void SetText(string languageCode, string languageText)
    {
      if (this.UsedLanguages == null)
        throw new Exception("UsedLanguages not defined");
      if (languageCode == this.UsedLanguages.ReferenceLanguage)
      {
        this.TranslationReferenceText = languageText;
      }
      else
      {
        if (!(languageCode == this.UsedLanguages.TranslationLanguage))
          throw new Exception("language code not part of UsedLanguages");
        this.TranslationText = languageText;
      }
    }

    public override string ToString() => this.TranslationGroup.ToString() + ":" + this.KeyText;

    public int CompareTo(object obj)
    {
      return obj == null || !(obj is TranslationListText) ? 1 : (obj as TranslationListText).ToString().CompareTo(this.ToString());
    }
  }
}
