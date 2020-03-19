using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RetroLauncher.ServiceTools.Emuplace
{
    public static class Parser
    {

        static Dictionary<string, string[]> items;

        public static string[] GetValue(string name)
        {
            if (items == null)
                Load();
            if (items.ContainsKey(name))
                return items[name];
            return null;
        }

        public static void SetValue(string name, string[] value)
        {
            if (items == null)
                Load();
            if (items.ContainsKey(name))
                items[name] = value;
            else
                items.Add(name, value);
        }

        public static void Load()
        {
            if (!File.Exists(Storage.Source.PathEmulatorConfig))
            {
                //запускаем эмулятор чтобы он создал файл настроек
                System.Diagnostics.Process emulator = new System.Diagnostics.Process();
                emulator.StartInfo.FileName = Storage.Source.PathEmulator + "mednafen.exe";
                emulator.StartInfo.CreateNoWindow = false;
                emulator.Start();


            }

            items = ParseConfig(System.IO.File.ReadAllText(Storage.Source.PathEmulatorConfig));
        }

        static Dictionary<string, string[]> ParseConfig(string configText)
        {
            var reader = new StringReader(configText);
            var dict = new Dictionary<string, string[]>();

            var line = reader.ReadLine();
            while (line != null)
            {
                line = line.TrimStart();

                if (line.Length != 0 &&
                    !line.StartsWith(";"))
                {
                    var firstSpaceIndex = line.IndexOf(' ');
                    var key = line.Substring(0, firstSpaceIndex);
                    var values = line
                        .Substring(firstSpaceIndex)
                        .Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToArray();

                    dict[key] = values;
                }

                line = reader.ReadLine();
            }

            return dict;
        }


        public static void SaveConfig()
        {
            List<string> savedDict = new List<string>();
            foreach (var item in items)
            {
                string res = item.Key + " ";
                if (item.Value != null)
                    for (int i = 0; i < item.Value.Length; i++)
                    {
                        if (i == 0 || i == items.Values.Count - 1)
                            res += item.Value[i];
                        else res += " || " + item.Value[i];
                    }

                savedDict.Add(res);
            }

            var result = "";

            foreach (var di in savedDict)
                result += di + System.Environment.NewLine;

            using (StreamWriter sw = new StreamWriter(Storage.Source.PathEmulatorConfig, false))
            {
                foreach (var di in savedDict)
                    sw.WriteLine(di);
            }

        }
    }

}
