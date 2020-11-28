using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public class TranslationComponent : TextComponent
    {
        public string Translation;
        public List<TextComponent> With;

        public TranslationComponent(string translation, List<TextComponent> with)
        {
            Type = TextComponentType.TranslatedText;
            Translation = translation;
            With = with;
        }

        public override string Resolve()
        {
            throw new NotImplementedException();
        }
    }
}
