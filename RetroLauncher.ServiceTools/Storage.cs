using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RetroLauncher.ServiceTools
{
    public class Storage
    {
        private const string WorkFolder = "RetroLauncher";
        public readonly string AppDataPath; //%AppDATA%
        public readonly string PathApp; //%AppDATA%\RetroLauncher
        public readonly string PathSettingsApp; //%AppDATA%\RetroLauncher\settings.xml
        public readonly string PathEmulator; //%AppDATA%\RetroLauncher\emulator
        public readonly string PathEmulatorExe; //%AppDATA%\RetroLauncher\emulator\mednafen.exe
        public readonly string PathEmulatorConfig; //%AppDATA%\RetroLauncher\emulator\mednafen.cfg
        public readonly string PathGames; //%AppDATA%\RetroLauncher\Games
        public readonly string PathLocalDb; //%AppDATA%\RetroLauncher\localdb.db


        private Storage()
        {
            //инициализация путей
            AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            PathApp = Path.Combine(AppDataPath, WorkFolder);
            PathSettingsApp = Path.Combine(PathApp, "settings.xml");
            PathEmulator = Path.Combine(PathApp, "emulator");
            PathEmulatorExe = Path.Combine(PathEmulator, "mednafen.exe");
            PathEmulatorConfig = Path.Combine(PathEmulator, "mednafen.cfg");
            PathGames = Path.Combine(PathApp, "Games");
            PathLocalDb = Path.Combine(PathApp, "localdb.db");

            //инициализация справочника
            if (!IsCreateSettings()) Create();
            Load();
        }

        //экземпляр синглтона
        private static Storage source = null;

        //для потокобезопасности
        private static readonly object threadlock = new object();

        public static Storage Source
        {
            get
            {

                lock (threadlock)
                {
                    if (source == null)
                        source = new Storage();

                    return source;
                }
            }
        }

        //словарь параметров
        Dictionary<string, (Type type, Object value)> items;

        /// <summary>
        /// Получить значение параметра по имени
        /// </summary>
        /// <param name="name">название параметра</param>
        /// <returns></returns>
        public object GetValue(string name)
        {
            if (items.ContainsKey(name))
                return Convert.ChangeType(items[name].value, items[name].type);
            return null; //TODO: изменить такой фатал
        }

        /// <summary>
        /// Получить значение параметра по имени
        /// </summary>
        /// <typeparam name="T">тип параметра для получения</typeparam>
        /// <param name="name">название параметра</param>
        /// <returns></returns>
        public T GetValue<T>(string name)
        {

            if (items.ContainsKey(name))
            {
                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(TypeProxy), items[name].value.ToString());
                return (T)items[name].value;
            }

            return default(T); //TODO: изменить такой фатал
        }

        /// <summary>
        /// Указать значение параметра, если отсутствует создатся новый
        /// </summary>
        /// <param name="name">название параметра</param>
        /// <param name="value">значение параметра</param>
        public void SetValue(string name, object value, bool isSaveForce = false)
        {
            //указать значение параметра
            if (items.ContainsKey(name)) items[name] = (value.GetType(), value);
            else items.Add(name, (value.GetType(), value));
            //сохранить сразу ?
            if (isSaveForce) Save();
        }

        /*
        public void SetValue<T>(string name, T value, bool isSaveForce = false)
        {
            //указать значение параметра
            if (items.ContainsKey(name)) items[name] = (value.GetType(), value);
            else items.Add(name, value);
            //сохранить сразу ?
            if (isSaveForce) Save();
        }
        */
        
        bool IsCreateSettings()
        {
            if (Directory.Exists(PathApp)) return true;
            return false;
        }

        /// <summary>
        /// Загрузить словарь настроек
        /// </summary>
        private void Load()
        {
            if (!File.Exists(PathSettingsApp))
                if (!Create())
                    throw new IOException("Не удалось создать файл: " + PathSettingsApp);


            //загружаем элементы
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(PathSettingsApp);

            // получим корневой элемент
            XmlElement xRoot = xmlDoc.DocumentElement;

            items = new Dictionary<string, (Type type, Object value)>();
            foreach (XmlNode item in xRoot.ChildNodes)
            {
                double tryDouble;
                int tryInt;
                bool trybool;

                if (int.TryParse(item.InnerText, out tryInt))
                    items.Add(item.Name, (typeof(int), tryInt));
                else if (double.TryParse(item.InnerText, out tryDouble))
                    items.Add(item.Name, (typeof(double), tryDouble));
                else if (bool.TryParse(item.InnerText, out trybool))
                    items.Add(item.Name, (typeof(bool), trybool));
                else items.Add(item.Name, (typeof(string), item.InnerText));

            }
        }


        /// <summary>
        /// Создать словарь настроек
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            try
            {
                //загружаем файл
                XmlDocument doc = new XmlDocument();
                //создаем все необходимые директории если надо
                if (!Directory.Exists(PathApp))
                    Directory.CreateDirectory(PathApp);
                if (!File.Exists(PathSettingsApp))
                {
                    XmlTextWriter textWritter = new XmlTextWriter(PathSettingsApp, null);
                    textWritter.WriteStartDocument();
                    textWritter.WriteStartElement("settings");
                    textWritter.WriteEndElement();
                    textWritter.Close();
                }

                //Создаем элементы
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(PathSettingsApp);

                items = new Dictionary<string, (Type type, Object value)>();
                items.Add("ProxyType", (typeof(TypeProxy), TypeProxy.Default));
                items.Add("ProxyHost", (typeof(string), ""));
                items.Add("ProxyPort", (typeof(int), 0));
                items.Add("WindowWidth", (typeof(int), 0));
                items.Add("WindowHeight", (typeof(int), 0));
                items.Add("FirstLoad", (typeof(bool), true));

                foreach (var item in items)
                {
                    XmlElement subRoot = xmlDoc.CreateElement(item.Key);
                    subRoot.InnerText = item.Value.value.ToString();
                    subRoot.SetAttribute("type", item.Value.type.Name);
                    xmlDoc.DocumentElement.AppendChild(subRoot);
                }

                xmlDoc.Save(PathSettingsApp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Сохранить словарь настроек
        /// </summary>
        public void Save()
        {
            try
            {
                //загружаем файл
                XmlDocument doc = new XmlDocument();
                if (!Directory.Exists(PathApp))
                    Directory.CreateDirectory(PathApp);

                XmlTextWriter textWritter = new XmlTextWriter(PathSettingsApp, null);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("settings");
                textWritter.WriteEndElement();
                textWritter.Close();

                //Создаем элементы
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(PathSettingsApp);

                XmlElement xRoot = xmlDoc.CreateElement("settings");
                foreach (var item in items)
                {
                    XmlElement subRoot = xmlDoc.CreateElement(item.Key);
                    subRoot.InnerText = item.Value.value.ToString();
                    var f = item.Value.GetType();
                    //subRoot.SetAttribute("type", item.Value.GetType().IsEnum ? "Enum" : item.Value.GetType().Name);
                    subRoot.SetAttribute("type", item.Value.type.Name);
                    xmlDoc.DocumentElement.AppendChild(subRoot);
                }

                xmlDoc.Save(PathSettingsApp);

            }
            catch (Exception)
            {
                //TODO Return massage
                //System.Windows.MessageBox.Show(
                //    "Не удалось сохранить настройки приложения.\nНо вы не расстраивайтесь, это не критично!", "Ошибка!",
                //    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }
    }

    public enum TypeProxy
    {
        Default,
        Http
    }
}
