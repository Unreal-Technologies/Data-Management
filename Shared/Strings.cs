namespace Shared
{
    public abstract class Strings
    {
        #region Members
        private static Dictionary<string, string>? text;
        private static Languages? current;
        #endregion //Members

        #region Enums
        public enum Languages
        {
            En, Nl
        }
        #endregion //Enums

        #region Properties
        public static string Word_Add { get { return Strings.Get("W!Add"); } }
        public static string Word_Edit { get { return Strings.Get("W!Edit"); } }
        public static string Word_Remove { get { return Strings.Get("W!Remove"); } }
        public static string Word_Roles { get { return Strings.Get("W!Roles"); } }
        public static string Word_Administrator { get { return Strings.Get("W!Administrator"); } }
        public static string Word_Description { get { return Strings.Get("W!Description"); } }
        public static string Word_Access { get { return Strings.Get("W!Access"); } }
        public static string Word_User { get { return Strings.Get("W!User"); } }
        public static string Word_Count { get { return Strings.Get("W!Count"); } }
        public static string Word_Launching { get { return Strings.Get("W!Launching"); } }
        public static string Word_Account { get { return Strings.Get("W!Account"); } }
        public static string Word_Logout { get { return Strings.Get("W!Logout"); } }
        public static string String_UserCount { get { return Strings.Get("S!UserCount"); } }
        public static string String_WrongUsernameOrPassword { get { return Strings.Get("S!WrongUsernameOrPassword"); } }
        public static string Word_Information { get { return Strings.Get("W!Information"); } }
        public static string Word_Authenticate { get { return Strings.Get("W!Authenticate"); } }
        public static string Word_Username { get { return Strings.Get("W!Username"); } }
        public static string Word_Password { get { return Strings.Get("W!Password"); } }
        public static string Word_Login { get { return Strings.Get("W!Login"); } }
        public static string String_StartingServerCommunication { get { return Strings.Get("S!StartingServerCommunication"); } }
        public static string String_LoadingModules { get { return Strings.Get("S!LoadingModules"); } }
        public static string Word_Starting { get { return Strings.Get("W!Starting"); } }
        public static string String_LoadedXModules { get { return Strings.Get("S!LoadedXModules"); } }
        public static string String_Copyright { get { return Strings.Get("S!Copyright"); } }
        public static string String_Version { get { return Strings.Get("S!Version"); } }
        public static Languages? LanguageOverride { get; set; }
        #endregion //Properties

        #region Public Methods
        public static string Get(string key)
        {
            Languages? language = ApplicationState.Language;
            if(Strings.LanguageOverride != null)
            {
                language = Strings.LanguageOverride;
            }

            if ((Strings.current == null || Strings.text == null) && language != null)
            {
                Strings.Load(language.Value);
            }
            if (Strings.text == null)
            {
                return key;
            }

            if (text.TryGetValue(key, out string? value))
            {
                return value;
            }

            return key;
        }
        #endregion //Public Methods

        #region Private Methods
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
            Strings.current = language;
        }
        #endregion //Private Methods
    }
}
