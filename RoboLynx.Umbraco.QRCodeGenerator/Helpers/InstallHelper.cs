using System.Web.Configuration;

namespace RoboLynx.Umbraco.QRCodeGenerator.Helpers
{
    public class InstallHelpers
    {
        string AppSettingKey = Constants.PluginName;
        public TranslationHelper TranslationHelper => new TranslationHelper();

        /// <summary>
        /// Add & merge in tranlsations from our lang files into Umbraco lang files
        /// </summary>
        public void AddTranslations()
        {
            TranslationHelper.AddTranslations();
        }

        private string GetAssemblyVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        public bool IsInstalled()
        {
            //Check to see if appSetting AnalyticsStartupInstalled is true or even present
            var installAppSetting = WebConfigurationManager.AppSettings[AppSettingKey];
            return !string.IsNullOrEmpty(installAppSetting) && installAppSetting == GetAssemblyVersion();
        }

        public void Install()
        {
            //Check to see if language keys for section needs to be added
            AddTranslations();

            //All done installing our custom stuff
            //As we only want this to run once - not every startup of Umbraco
            var webConfig = WebConfigurationManager.OpenWebConfiguration("/");
            webConfig.AppSettings.Settings.Remove(AppSettingKey);
            webConfig.AppSettings.Settings.Add(AppSettingKey, GetAssemblyVersion());
            webConfig.Save();
        }
    }
}
