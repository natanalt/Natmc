using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Ui.Text
{
    public abstract class TextComponent
    {
        // https://wiki.vg/Chat#Current_system_.28JSON_Chat.29

        public TextComponentType Type { get; protected set; }
        public TextComponent Parent { get; set; }
        public List<TextComponent> Siblings { get; protected set; }

        public string RawText
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append(Resolve());
                foreach (var sibling in Siblings)
                {
                    builder.Append(sibling.RawText);
                }
                return builder.ToString();
            }
        }

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

        public ClickEvent m_ClickEvent;
        public ClickEvent ClickEvent
        {
            get => m_ClickEvent ?? Parent?.ClickEvent;
            set => m_ClickEvent = value;
        }

        public HoverEvent m_HoverEvent;
        public HoverEvent HoverEvent
        {
            get => m_HoverEvent ?? Parent?.HoverEvent;
            set => m_HoverEvent = value;
        }

        public TextComponent(TextComponent parent = null)
        {
            Parent = parent;
            Siblings = new List<TextComponent>();
        }

        public static TextComponent FromJsonToken(JToken token, out string error)
        {
            error = null;
            if (token is JObject o)
                return FromJsonObject(o, out error);
            else if (token is JArray a)
                return FromJsonArray(a, out error);
            else if (token is JValue v)
                return FromJsonValue(v);
            else
                throw new ArgumentException();
        }

        public static TextComponent FromJsonObject(JObject o, out string error)
        {
            TextComponentType type;
            if (o.ContainsKey("text"))
                type = TextComponentType.Text;
            else if (o.ContainsKey("keybind"))
                type = TextComponentType.Keybind;
            else if (o.ContainsKey("translate"))
                type = TextComponentType.TranslatedText;
            else if (o.ContainsKey("score"))
                type = TextComponentType.Score;
            else
            {
                error = "Unknown component type";
                return null;
            }

            TextComponent root = null;

            if (type == TextComponentType.Text)
            {
                var textEntry = o["text"];
                if (textEntry is JObject || textEntry is JArray)
                {
                    error = "text property can't be an object or array";
                    return null;
                }
                root = new StringComponent(textEntry.ToString());
            }
            else if (type == TextComponentType.Keybind)
            {
                var keybindEntry = o["keybind"];
                if (keybindEntry is JObject || keybindEntry is JArray)
                {
                    error = "keybind property can't be an object or array";
                    return null;
                }
                root = new KeybindComponent(keybindEntry.ToString());
            }
            else if (type == TextComponentType.TranslatedText)
            {
                var translateEntry = o["translate"];
                if (translateEntry is JObject || translateEntry is JArray)
                {
                    error = "translate property can't be an object or array";
                    return null;
                }

                var withToken = o["with"];
                if (!(withToken is JArray))
                {
                    error = "with property can only be an array";
                    return null;
                }
                var withArray = (JArray)withToken;

                var with = new List<TextComponent>();
                for (int i = 0; i < withArray.Count; i += 1)
                {
                    var component = FromJsonToken(withArray[i], out string parseError);
                    if (component == null)
                    {
                        error = $"Parse error for with in translate, index {i}: {parseError}";
                        return null;
                    }
                    with.Add(component);
                }

                root = new TranslationComponent(translateEntry.ToString(), with);
            }
            else if (type == TextComponentType.Score)
            {
                // TODO: Score component type
                throw new NotImplementedException("Score component type not implemented");
            }

            if (o.ContainsKey("bold"))
                root.Bold = o["bold"].ToString() == "true";
            if (o.ContainsKey("italic"))
                root.Italic = o["italic"].ToString() == "true";
            if (o.ContainsKey("underlined"))
                root.Underlined = o["underlined"].ToString() == "true";
            if (o.ContainsKey("obfuscated"))
                root.Obfuscated = o["obfuscated"].ToString() == "true";
            if (o.ContainsKey("insertion"))
                root.Insertion = o["insertion"].ToString();

            if (o.ContainsKey("color"))
            {
                var color = ColorValues.NameToColor(o["color"].ToString());
                if (color == null)
                {
                    error = $"Invalid color name {o["color"]}";
                    return null;
                }
                root.Color = color.Value;
            }

            if (o.ContainsKey("clickEvent"))
            {
                var clickEventObject = o["clickEvent"];
                var action = clickEventObject["action"].ToString();
                var value = clickEventObject["value"];

                switch (action)
                {
                    case "open_url":
                    case "open_file":
                        try
                        {
                            root.ClickEvent = new OpenUrlClickEvent(new Uri(value.ToString()));
                        }
                        catch (Exception e)
                        {
                            error = $"Couldn't parse URI: {e.Message}";
                            return null;
                        }
                        break;
                    case "run_command":
                        root.ClickEvent = new RunCommandClickEvent(value.ToString());
                        break;
                    case "suggest_command":
                        root.ClickEvent = new SuggestCommandClickEvent(value.ToString());
                        break;
                    case "change_page":
                        try
                        {
                            root.ClickEvent = new ChangePageClickEvent(int.Parse(value.ToString()));
                        }
                        catch (Exception)
                        {
                            error = $"Couldn't parse integer: {value}";
                            return null;
                        }
                        break;
                    default:
                        error = $"Invalid click event action {action}";
                        return null;
                }
            }

            if (o.ContainsKey("hoverEvent"))
            {
                // TODO: hover events
                throw new NotImplementedException("hoverEvent");
            }

            error = null;
            return root;
        }

        public static TextComponent FromJsonArray(JArray a, out string error)
        {
            // With components encoded as arrays, the first entry is the root,
            // and all consecutive entries are sibling components. The array
            // must not be empty.

            if (a.Count == 0)
            {
                error = "The array can't be empty";
                return null;
            }

            List<TextComponent> components = new List<TextComponent>();
            for (int i = 0; i < a.Count; i++)
            {
                var entry = a[i];

                if (entry.Type == JTokenType.Array)
                {
                    var sibling = FromJsonArray((JArray)entry, out string parseError);
                    if (sibling == null)
                    {
                        error = $"Parse error for array component, index {i}: {parseError}";
                        return null;
                    }
                    components.Add(sibling);
                }
                else if (entry.Type == JTokenType.Object)
                {
                    var sibling = FromJsonObject((JObject)entry, out string parseError);
                    if (sibling == null)
                    {
                        error = $"Parse error for object component, index {i}: {parseError}";
                        return null;
                    }
                    components.Add(sibling);
                }
                else
                {
                    error = $"Parse error for non array/object component, index {i}";
                    return null;
                }
            }

            var root = components[0];
            var siblings = components.GetRange(1, components.Count - 1);
            foreach (var sibling in siblings)
            {
                sibling.Parent = root;
                root.Siblings.Add(sibling);
            }

            error = null;
            return root;
        }

        public static TextComponent FromJsonValue(JValue value)
        {
            return new StringComponent(value.ToString());
        }

        public abstract string Resolve();
    }
}
