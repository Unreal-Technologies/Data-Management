namespace Shared
{
    public abstract class Strings : UT.Data.Strings
    {
        #region Members
        private static Dictionary<string, string>? text;
        #endregion //Members

        #region Properties
        public static string Word_Add { get { return Strings.GetValue("W!Add"); } }
        public static string Word_Edit { get { return Strings.GetValue("W!Edit"); } }
        public static string Word_Remove { get { return Strings.GetValue("W!Remove"); } }
        public static string Word_Roles { get { return Strings.GetValue("W!Roles"); } }
        public static string Word_Administrator { get { return Strings.GetValue("W!Administrator"); } }
        public static string Word_Description { get { return Strings.GetValue("W!Description"); } }
        public static string Word_Access { get { return Strings.GetValue("W!Access"); } }
        public static string Word_User { get { return Strings.GetValue("W!User"); } }
        public static string Word_Count { get { return Strings.GetValue("W!Count"); } }
        public static string Word_Launching { get { return Strings.GetValue("W!Launching"); } }
        public static string Word_Account { get { return Strings.GetValue("W!Account"); } }
        public static string Word_Logout { get { return Strings.GetValue("W!Logout"); } }
        public static string String_UserCount { get { return Strings.GetValue("S!UserCount"); } }
        public static string String_WrongUsernameOrPassword { get { return Strings.GetValue("S!WrongUsernameOrPassword"); } }
        public static string Word_Information { get { return Strings.GetValue("W!Information"); } }
        public static string Word_Authenticate { get { return Strings.GetValue("W!Authenticate"); } }
        public static string Word_Username { get { return Strings.GetValue("W!Username"); } }
        public static string Word_Password { get { return Strings.GetValue("W!Password"); } }
        public static string Word_Login { get { return Strings.GetValue("W!Login"); } }
        public static string String_StartingServerCommunication { get { return Strings.GetValue("S!StartingServerCommunication"); } }
        public static string String_LoadingModules { get { return Strings.GetValue("S!LoadingModules"); } }
        public static string Word_Starting { get { return Strings.GetValue("W!Starting"); } }
        public static string String_LoadedXModules { get { return Strings.GetValue("S!LoadedXModules"); } }
        public static string String_Copyright { get { return Strings.GetValue("S!Copyright"); } }
        public static string String_Version { get { return Strings.GetValue("S!Version"); } }
        public static string Word_Id { get { return Strings.GetValue("W!Id"); } }
        public static string Word_Save { get { return Strings.GetValue("W!Save"); } }
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

            string result = UT.Data.Strings.GetKey(value);
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

            string result = UT.Data.Strings.GetValue(key);
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
