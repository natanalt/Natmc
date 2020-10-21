using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public class TextComponent
    {
        public TextComponent Parent { get; set; }

        private bool? m_Bold;
        public bool? Bold
        {
            get => m_Bold == null ? Parent == null ? false : Parent.Bold : m_Bold.Value;
            set => m_Bold = value;
        }

        private bool? m_Italic;
        public bool? Italic
        {
            get => m_Italic == null ? Parent == null ? false : Parent.Italic : m_Italic.Value;
            set => m_Italic = value;
        }

        private bool? m_Underlined;
        public bool? Underlined
        {
            get => m_Underlined == null ? Parent == null ? false : Parent.Underlined : m_Underlined.Value;
            set => m_Underlined = value;
        }

        private bool? m_Strikethrough;
        public bool? Strikethrough
        {
            get => m_Strikethrough == null ? Parent == null ? false : Parent.Strikethrough : m_Strikethrough.Value;
            set => m_Strikethrough = value;
        }

        private bool? m_Obfuscated;
        public bool? Obfuscated
        {
            get => m_Obfuscated == null ? Parent == null ? false : Parent.Obfuscated : m_Obfuscated.Value;
            set => m_Obfuscated = value;
        }

        private McColor? m_Color;
        public McColor? Color
        {
            get => m_Color == null ? Parent == null ? McColor.White : Parent.Color : m_Color.Value;
            set => m_Color = value;
        }

        private string m_Insertion;
        public string Insertion
        {
            get => m_Insertion ?? Parent?.Insertion;
            set => m_Insertion = value;
        }
    }
}
