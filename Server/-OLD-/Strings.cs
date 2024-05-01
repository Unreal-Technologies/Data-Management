namespace Server
{
    public abstract class Strings : Shared.Strings
    {
        #region Members
        private static Dictionary<string, string>? text;
        #endregion //Members

        #region Properties
        public static string String_Loaded { get { return Strings.GetValue("S!Loaded"); } }
        #endregion //Properties

        #region Public Methods
        public new static string GetKey(string value)
        {
            Strings.Load();
            if (Strings.text == null)
            {
                return value;
            }

            string? key = text.Where(x => x.Value == value).Select(x => x.Key).FirstOrDefault();
            if(key == null)
            {
                return value;
            }

            string result = Shared.Strings.GetKey(value);
            if (result != value)
            {
                return result;
            }

            return key;
        }

        public new static string GetValue(string key)
        {
            Strings.Load();
            if (Strings.text == null)
            {
                return key;
            }

            if (text.TryGetValue(key, out string? value))
            {
                return value;
            }

            string result = Shared.Strings.GetValue(key);
            if (result != key)
            {
                return result;
            }

            return key;
        }
        #endregion //Public Methods

        #region Private Methods
        private static void Load()
        {
            Languages? language = Strings.Language;
            if (Strings.LanguageOverride != null)
            {
                language = Strings.LanguageOverride;
            }

            if ((Strings.Current == null || Strings.text == null) && language != null)
            {
                Strings.Load(language.Value);
            }
        }

        private static void Load(Languages language)
        {
            string? content = Resources.ResourceManager.GetString(language.ToString());
            if(content == null)
            {
                return;
            }

            Strings.text = [];
            foreach(string line in content.Split("\r\n"))
            {
                string[] components = line.Split(":");
                string left = string.Join(":", components.Take(1));
                string right = string.Join(":", components.Skip(1));

                Strings.text.Add(left, right);
            }
            Strings.Current = language;
        }
        #endregion //Private Methods
    }
}
