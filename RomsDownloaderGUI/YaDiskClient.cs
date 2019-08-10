using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RestYaDiskSample
{
    /// <summary>
    /// Реализация API для сервиса "Яндекс диск"
    /// Протокол: WevDav
    /// Автор:  Зангиев Владимир
    /// </summary>
    public class YandexDiskClient
    {
        string token;
        string APP_PATH = "https://cloud-api.yandex.net";

        public string error;

        private void myinit(string token, string error)
        {
            this.error = error;
            this.token = token;
        }

        /// <summary>
        /// Создает экзепляр класса
        /// </summary>
        public YandexDiskClient()
        {
            myinit("", "no errors");
        }

        /// <summary>
        /// Создает экзепляр класса
        /// </summary>
        /// <param name="token">токен доступа</param>
        public YandexDiskClient(string token)
        {
            myinit(token, "no errors");
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))
            {               

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", token);
                //    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }
        /// <summary>
        /// Запрос возвращает информацию о папке dir и всех папках и файлах в нем
        /// </summary>
        /// <param name="dir">Папка</param>
        /// <returns>Список файлов и папок в папке dir или null если произошла ошибка</returns>
        public string PROPPATCH(string dir)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync(APP_PATH + "/v1/disk/resources?path=" + dir).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        

        /// <summary>
        /// Запрос создает папку dir на сервере
        /// </summary>
        /// <param name="dir">Название папки</param>
        /// <returns>true если запрос выполнен успешно</returns>
        public bool MKCOL(string dir)
        {
            HttpWebRequest myweb = (HttpWebRequest)WebRequest.Create("https://webdav.yandex.ru/" + dir);
            myweb.Accept = "*/*";
            myweb.Headers.Add("Depth: 1");
            myweb.Headers.Add("Authorization: OAuth " + token);
            myweb.Method = "MKCOL";

            try
            {
                HttpWebResponse resp = (HttpWebResponse)myweb.GetResponse();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Запрос отправляет файл на сервер
        /// </summary>
        /// <param name="dir">Название файла на сервере (путь к файлу)</param>
        /// <param name="myfile">Загружаемый файл</param>
        /// <returns>true если запрос выполнен успешно</returns>
        public bool PUT(string namefile, byte[] paramFileBytes)
        {
            using (var client = CreateClient())
            {

                var response = client.GetAsync(APP_PATH + "/v1/disk/resources/upload?path=RetroLauncherFiles/" + namefile+ "&overwrite=true").Result;
                var urlForUpload = response.Content.ReadAsStringAsync().Result;
                var loadjson =  JsonConvert.DeserializeObject<Dictionary<string,string>>(urlForUpload);

                HttpContent content = new ByteArrayContent(paramFileBytes);
                var fileAnswer = client.PutAsync(loadjson["href"], content).Result;
              
            }
            return true;
        }

        /// <summary>
        /// Запрос скачивает файл с сервера
        /// </summary>
        /// <param name="dir">Путь к файлу на сервере</param>
        /// <param name="myfile">Название файла</param>
        /// <returns>true если запрос выполнен успешно</returns>
        public string GET(string myfile)
        {
            using (var client = CreateClient())
            {
                var response = client.GetAsync(APP_PATH + "/v1/disk/resources/download?path=RetroLauncherFiles/" + myfile).Result;
                var urlResult = response.Content.ReadAsStringAsync().Result;
                var loadjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(urlResult);
                return loadjson["href"];
            }
        }

        private DirInfo ParseDirInfo(string xml)
        {
            DirInfo rez = new DirInfo();
            rez.creationDate = DirInfo.getData(xml, "creationdate");
            rez.displayName = DirInfo.getData(xml, "displayname");
            rez.contentLenght = Convert.ToInt32(DirInfo.getData(xml, "getcontentlength"));
            rez.lastModified = DirInfo.getData(xml, "getlastmodified");
            return rez;
        }
    }

    public class DirInfo
    {
        public DirInfo() { }
        public string displayName;
        public int contentLenght;
        public string creationDate;
        public string lastModified;

        public static string getData(string xml, string tag)
        {
            int p1, p2;
            p1 = xml.IndexOf("<d:" + tag + ">");
            p2 = xml.IndexOf("</d:" + tag + ">");
            return xml.Substring(p1 + tag.Length + 4, p2 - 4 - (p1 + tag.Length));
        }
    }
}
