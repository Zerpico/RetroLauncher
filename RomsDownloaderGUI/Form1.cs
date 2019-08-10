using RomsDownloaderGUI.BaseParser;
using RomsDownloaderGUI.UrlParse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using AngleSharp.Html.Parser;

namespace RomsDownloaderGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateBase();
            Start();

        }

        SQLiteConnection dbConnection;
        private void CreateBase()
        {
            dbConnection = new SQLiteConnection();
            if (System.IO.File.Exists("library_base.sqlite"))
                System.IO.File.Delete("library_base.sqlite");

            SQLiteConnection.CreateFile("library_base.sqlite");
            dbConnection = new SQLiteConnection("Data Source=library_base.sqlite;Version=3;");
            dbConnection.Open();

            string sql = "CREATE TABLE [rl_games] (\n" +
                           "[game_id] integer NOT NULL PRIMARY KEY AUTOINCREMENT,\n" +
                           "[Name] nvarchar(255) NOT NULL,\n" +
                           "[NameSecond] nvarchar(500) ,\n" +
                           "[NameOther] nvarchar(500), \n" +
                           "[Platform] nvarchar(254) NOT NULL,\n" +
                           "[Year] int, \n" +
                           "[Developer] nvarchar(254), \n" +
                           "[Genre] nvarchar(254), \n" +
                           "[Players] int, \n" +
                           "[Annotation] nvarchar(1000), \n" +
                           "[ImgUrl] nvarchar(1000), \n" +
                           "[Url] nvarchar(1000) );";

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE [rl_game_link] (\n" +
                  "  [link_id] integer NOT NULL PRIMARY KEY AUTOINCREMENT,\n" +
                  "  [game_id] integer NOT NULL, \n" +
                  "  [url] nvarchar(2000) NOT NULL,\n" +
                  "  [type_url] integer NOT NULL, \n" +
                  "  FOREIGN KEY([game_id])\n" +
                  "      REFERENCES[rl_games] ([game_id])\n" +
                  "      ON UPDATE NO ACTION ON DELETE CASCADE);";

            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();
        }


        private void CreateBaseExpLoad()
        {
            dbConnection = new SQLiteConnection();

            dbConnection = new SQLiteConnection("Data Source=library_base.sqlite;Version=3;");
            dbConnection.Open();

            string sql = "CREATE TABLE [rl_genres] (\n" +
                            "[genre_id] integer NOT NULL PRIMARY KEY, \n" +
                            "[genre_name] nvarchar(254) NOT NULL);";

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE [rl_platforms] (\n" +
                    "[platform_id] integer NOT NULL PRIMARY KEY, \n" +
                    "[platform_name] nvarchar(254) NOT NULL,\n" +
                    "[platform_alias] nvarchar(254));";

            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            sql = "INSERT INTO rl_genres (genre_id, genre_name)\n" +
                    "select ROW_NUMBER() OVER(ORDER BY genre) RowNum, genre\n" +
                    "FROM(select distinct genre from rl_games) g ";

            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();


            sql = "INSERT INTO rl_platforms (platform_id, platform_name, platform_alias)\n" +
                   " select ROW_NUMBER() OVER(ORDER BY platform) RowNum, platform, alias\n" +
                   " FROM(\n" +
                   " select DISTINCT platform,\n" +
                   " case WHEN  platform = 'Dendy/Денди (NES)' then 'nes'\n" +
                   "      WHEN  platform = 'Super Nintendo Entertainment System (SNES)' then 'snes'\n" +
                   "      WHEN  platform = 'Sega Master System (SMS)' then 'sms'\n" +
                   "      WHEN  platform = 'Sega MegaDrive/Сега (GEN)' then 'gen'\n" +
                   "      WHEN  platform = 'TurboGrafx-16 (TG16)' then 'tg16'\n" +
                   "      WHEN  platform = 'GameBoy Color (GBC)' then 'gbc'\n" +
                   " end as alias\n" +
                   " FROM rl_games\n" +
                   " )";

            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();


            sql = "CREATE TABLE [rl_games_base] (\n" +
                   " [game_id] integer NOT NULL PRIMARY KEY, \n" +
                   " [game_name] nvarchar(254) NOT NULL,\n" +
                   " [name_second] nvarchar(254), \n" +
                   " [name_other] nvarchar(254), \n" +
                   " [platform_id] integer NOT NULL, \n" +
                   " [genre_id] integer NOT NULL, \n" +
                   " [year] integer, \n" +
                   " [developer] nvarchar(254), \n" +
                   " [annotation] nvarchar(2000), \n" +
                   "\n" +
                   " FOREIGN KEY([genre_id])\n" +
                   "     REFERENCES[rl_genres] ([genre_id])\n" +
                   "    ON UPDATE NO ACTION ON DELETE CASCADE,\n" +
                   "\n" +
                   "FOREIGN KEY([platform_id])\n" +
                   "     REFERENCES[rl_platforms] ([platform_id])\n" +
                   "    ON UPDATE NO ACTION ON DELETE CASCADE\n" +
                   " );";

            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();


            sql = "INSERT INTO rl_games_base (game_id,game_name,name_second,name_other,platform_id,genre_id,year,developer,annotation)\n" +
                    "SELECT game_id,Name,NameSecond,NameOther,pl.platform_id,gnr.genre_id,Year,Developer,Annotation\n" +
                    "FROM rl_games r\n" +
                    "JOIN rl_genres gnr ON r.genre = gnr.genre_name\n" +
                    "JOIN rl_platforms pl ON r.platform = pl.platform_name";

            command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();


            dbConnection.Close();
        }

        public void Start()
        {
            games = new List<IGame>();
            Worker();
        }

        string textLog;
        public string TextLog
        {
            get { return textLog; }
            set { textLog = value; richTextBox1.Invoke((Action)(() => { richTextBox1.Text = textLog; })); }
        }


        int progressMax;
        public int ProgressMax
        {
            get { return progressMax; }
            set { progressMax = value; progressBar1.Invoke((Action)(() => { progressBar1.Maximum = progressMax; })); }
        }

        int progressValue;
        public int ProgressValue
        {
            get { return progressValue; }
            set { progressValue = value; progressBar1.Invoke((Action)(() => { progressBar1.Value = progressValue; })); }
        }

        List<IGame> games { get; set; }
        BackgroundWorker wrk;
        //Начинаю всё парсить и скачивать
        public async void Worker()
        {
            //await Task.Factory.StartNew(() => { Storage.Load(); System.Threading.Thread.Sleep(100); Executer.OnUIThread(() => { TextLog = "Создан settings.xml"; }); });
            var emupars = new BaseSettings();
            ProgressMax = (emupars.Prefix.Count() * 2) + 3;
            ProgressValue = progressValue + 1;

            wrk = new BackgroundWorker();
            wrk.DoWork += Wrk_DoWork;
            // wrk.ProgressChanged += Wrk_ProgressChanged;
            wrk.RunWorkerCompleted += Wrk_RunWorkerCompleted;
            wrk.RunWorkerAsync();

        }

        private void Wrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TextLog = textLog + "\nБаза Скачана";
        }



        private void Wrk_DoWork(object sender, DoWorkEventArgs e)
        {
            BaseParse baseParse = new BaseParse();
            BaseSettings bset = new BaseSettings();
            HtmlLoader loader = new HtmlLoader(bset);
            //в цикле скачиваем и парсим сайт по каждому префиксу к основному url
            int i = 0;
            foreach (var prefix in bset.Prefix)
            {
                // если не вызвали Abort то продолжаем работу

                string source = loader.GetSource(prefix).Result;
                if (loader.ErrorMessage == null)
                {
                    //начинаем парсить 
                    var domParser = new HtmlParser();
                    //  if (!isActive) { OnComplete?.Invoke(this); return; }
                    var document = domParser.ParseDocument(source);
                    var result = baseParse.Parse(document, bset.BaseUrl);
                    i++;
                    InsertGames(result);
                    // wrk.ReportProgress(i);

                    result = null;
                }
                //else OnError?.Invoke(loader.ErrorMessage);
                System.Threading.Thread.Sleep(100);  // чтобы сайт не подумал что мы его ддосим


            }
            CreateBaseExpLoad();
        }

        string sql_ins = "INSERT INTO [rl_games] ([Name],[NameSecond],[NameOther],[Platform],[Year],[Developer],[Genre],[Players],[Annotation],[ImgUrl],[Url])\n" +
            "VALUES (@Name,@NameSecond,@NameOther,@Platform,@Year,@Developer,@Genre,@Players,@Annotation,@ImgUrl,@Url)";

        //При добавлении новых данных об игры и в базу
        private void InsertGames(IGame[] res)
        {
            var countGame = res.Count();
            TextLog = textLog + "\nОбновлено " + countGame + " игр  для  " + res[0].Platform;
            ProgressValue = progressValue + 1;

            dbConnection.Open();

            try
            {
                SQLiteCommand cmd = new SQLiteCommand(sql_ins, dbConnection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add("@Name", DbType.String);
                cmd.Parameters.Add("@NameSecond", DbType.String);
                cmd.Parameters.Add("@NameOther", DbType.String);
                cmd.Parameters.Add("@Platform", DbType.String);
                cmd.Parameters.Add("@Year", DbType.Int32);
                cmd.Parameters.Add("@Developer", DbType.String);
                cmd.Parameters.Add("@Genre", DbType.String);
                cmd.Parameters.Add("@Players", DbType.Int32);
                cmd.Parameters.Add("@Annotation", DbType.String);
                cmd.Parameters.Add("@ImgUrl", DbType.String);
                cmd.Parameters.Add("@Url", DbType.String);

                foreach (var item in res)
                {

                    cmd.Parameters["@Platform"].Value = item.Platform;
                    cmd.Parameters["@Name"].Value = item.Name.Replace("'", "").Replace("\"", "");

                    if (item.SecondName != null)
                        item.SecondName = item.SecondName.Replace("'", "").Replace("\"", "");
                    cmd.Parameters["@NameSecond"].Value = item.SecondName;

                    if (item.OtherName != null)
                        item.OtherName = item.OtherName.Replace("'", "").Replace("\"", "");
                    cmd.Parameters["@NameOther"].Value = item.OtherName;

                    if (item.Year.HasValue) cmd.Parameters["@Year"].Value = item.Year.Value;
                    else cmd.Parameters["@Year"].Value = DBNull.Value;

                    if (item.Developer != null)
                        item.Developer = item.Developer.Replace("'", "").Replace("\"", "");
                    cmd.Parameters["@Developer"].Value = item.Developer;

                    if (item.Players.HasValue) cmd.Parameters["@Players"].Value = item.Players.Value;
                    else cmd.Parameters["@Players"].Value = DBNull.Value;

                    //cmd.Parameters["Players"].Value = item.Players;
                    cmd.Parameters["@Genre"].Value = item.Genre;

                    if (item.Annotation != null)
                        item.Annotation = item.Annotation.Replace("'", "").Replace("\"", "");
                    cmd.Parameters["@Annotation"].Value = item.Annotation;
                    cmd.Parameters["@Url"].Value = item.Url;
                    cmd.Parameters["@ImgUrl"].Value = item.ImgUrl;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                TextLog = textLog + "\n" + ex.Message;
            }
            dbConnection.Close();

            //  games.AddRange(res);
            ProgressValue = progressValue + 1;
        }


        //При добавлении новых данных об игры и в базу
        private void InsertUrlGame(IGame res, string url, int type)
        {

            dbConnection.Open();

            try
            {
                SQLiteCommand cmd = new SQLiteCommand("INSERT INTO [rl_game_link] ([game_id],[url],[type_url]) VALUES (@game_id,@url,@type_url)", dbConnection);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add("@game_id", DbType.Int32);
                cmd.Parameters.Add("@url", DbType.String);
                cmd.Parameters.Add("@type_url", DbType.Int32);



                cmd.Parameters["@game_id"].Value = res.Id;
                cmd.Parameters["@url"].Value = url;
                cmd.Parameters["@type_url"].Value = type;

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                TextLog = textLog + "\n" + ex.Message;
            }
            dbConnection.Close();


        }

        //при обновлении всей базы игр
        private void Parser_OnComplete(object obj)
        {
            TextLog = textLog + "\nБаза полностью обновлена!\n\nВ базе " + games.Count.ToString() + " записи";
        }

        public void StartDownload()
        {
            if (!System.IO.Directory.Exists(PathGames))
                System.IO.Directory.CreateDirectory(PathGames);

            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            for (int i = 0; i < games.Count; i++)
            {
                string alias = "";
                switch (games[i].Platform)
                {
                    case @"Dendy/Денди (NES)":
                        alias = "nes";
                        break;
                    case @"Super Nintendo Entertainment System (SNES)":
                        alias = "snes";
                        break;
                    case @"Sega Master System (SMS)":
                        alias = "sms";
                        break;
                    case @"Sega MegaDrive/Сега (GEN)":
                        alias = "gen";
                        break;
                    case @"TurboGrafx-16 (TG16)":
                        alias = "tg16";
                        break;
                    case @"GameBoy Color (GBC)":
                        alias = "gbc";
                        break;
                }

                //создаем пути если их нет
                if (!System.IO.Directory.Exists(PathGames + alias))
                    System.IO.Directory.CreateDirectory(PathGames + alias);

                if (!System.IO.Directory.Exists(PathGames + alias + "\\roms\\"))
                    System.IO.Directory.CreateDirectory(PathGames + alias + "\\roms\\");

                var game_name = games[i].Name.Replace(' ', '_').Replace(':', ' ').Replace('\\', '_').Replace('/', '_').Replace('?', ' ').Replace('<', '_').Replace('>', '_');
                //настраиваем парсер для выдирания ссылки на архив с ромами игры с сайта
                var lastGamePath = PathGames + alias + "\\roms\\" + game_name + ".7z";
                /* var parseSet = new UrlParse.UrlSettings(games[i].Url);

                 var parser = new ParserWorker<string>(
                     new UrlParse.UrlParse(), parseSet);*/
                UrlParse.UrlParse urlParse = new UrlParse.UrlParse();
                UrlSettings bset = new UrlSettings(games[i].Url);

                ImgParse.ImgParse imgParse = new ImgParse.ImgParse();
                ImgParse.ImgSettings iset = new ImgParse.ImgSettings(games[i].Url);

                HtmlLoader loader = new HtmlLoader(bset);


                //в цикле скачиваем и парсим сайт по каждому префиксу к основному url
                int g = 0;
                foreach (var prefix in bset.Prefix)
                {
                    // если не вызвали Abort то продолжаем работу

                    var source = client.GetAsync(games[i].Url).Result.Content.ReadAsStringAsync().Result;
                    //начинаем парсить 
                    var domParser = new HtmlParser();
                    var document = domParser.ParseDocument(source);
                    var result = urlParse.Parse(document, bset.BaseUrl);

                    //System.IO.File.WriteAllBytes()
                    if (!string.IsNullOrEmpty(result))
                    {
                        var buffer = client.GetAsync(result).Result.Content.ReadAsByteArrayAsync().Result;
                        if (System.IO.File.Exists(lastGamePath))
                            System.IO.File.Delete(lastGamePath);
                        System.IO.File.WriteAllBytes(lastGamePath, buffer);
                        InsertUrlGame(games[i], lastGamePath, 1);
                    }
                    else
                    {
                        var buffer = client.GetAsync(games[i].Url).Result.Content.ReadAsByteArrayAsync().Result;
                        if (System.IO.File.Exists(lastGamePath))
                            System.IO.File.Delete(lastGamePath);
                        System.IO.File.WriteAllBytes(lastGamePath, buffer);
                        InsertUrlGame(games[i], lastGamePath, 1);
                    }


                    var imgresult = imgParse.Parse(document, iset.BaseUrl);



                    if (!System.IO.Directory.Exists(PathGames + alias + "\\pics\\"))
                        System.IO.Directory.CreateDirectory(PathGames + alias + "\\pics\\");
                    // var game_name = games[i].Name.Replace(' ', '_').Replace(':', ' ').Replace('\\', '_').Replace('/', '_').Replace('?', ' ').Replace('<', '_').Replace('>', '_');

                    //скачиваем пикчу обложки
                    var buffer_pic = client.GetAsync(games[i].ImgUrl).Result.Content.ReadAsByteArrayAsync().Result;
                    if (System.IO.File.Exists(PathGames + alias + "\\pics\\" + game_name + "0.jpg"))
                        System.IO.File.Delete(PathGames + alias + "\\pics\\" + game_name + "0.jpg");
                    System.IO.File.WriteAllBytes(PathGames + alias + "\\pics\\" + game_name + "0.jpg", buffer_pic);
                    InsertUrlGame(games[i], PathGames + alias + "\\pics\\" + game_name + "0.jpg", 2);


                    if (imgresult.Count() > 0)
                    {
                        int pi = 1;
                        //скачиваем остальные пикчи
                        foreach (var img in imgresult.Where(dd => dd != games[i].ImgUrl).ToArray())
                        {
                            buffer_pic = client.GetAsync(img).Result.Content.ReadAsByteArrayAsync().Result;
                            if (System.IO.File.Exists(PathGames + alias + "\\pics\\" + game_name + pi.ToString() + ".jpg"))
                                System.IO.File.Delete(PathGames + alias + "\\pics\\" + game_name + pi.ToString() + ".jpg");
                            System.IO.File.WriteAllBytes(PathGames + alias + "\\pics\\" + game_name + pi.ToString() + ".jpg", buffer_pic);
                            InsertUrlGame(games[i], PathGames + alias + "\\pics\\" + game_name + pi.ToString() + ".jpg", 3);
                            pi++;
                        }
                    }

                    System.Threading.Thread.Sleep(100);  // чтобы сайт не подумал что мы его ддосим
                }

                TextLog = games[i].Id.ToString();

            }
        }


        /* private void Parser_OnNewData(object arg1, string arg2)
         {
             //нашли ссылку, теперь скачиваем её в потоке с подписыванием на прогресс
             var downloader = new LoaderService();    
             downloader.DownloadFile(arg2, lastGamePath);
         }*/






        /// <summary>
        /// Путь до папки с ромами игр
        /// </summary>
        public static string PathGames
        {
            get { return System.IO.Directory.GetCurrentDirectory() + "\\RetroLauncher\\Games\\"; }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            games = GetBaseDownloaded().ToList();
            StartDownload();
        }




        public IEnumerable<IGame> GetBaseDownloaded()
        {
            dbConnection = new SQLiteConnection("Data Source=library_base.sqlite;Version=3;");
            dbConnection.Open();
            List<Game> result = new List<Game>();
            string sql = "SELECT * FROM rl_games";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;


            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Game newGame = new Game();
                newGame.Id = int.Parse(reader["game_id"].ToString());
                newGame.Platform = reader["Platform"].ToString();
                newGame.Name = reader["Name"].ToString();
                newGame.SecondName = reader["NameSecond"].ToString();
                newGame.OtherName = reader["NameOther"].ToString();
                newGame.Year = reader["Year"] == DBNull.Value ? new Nullable<int>() : int.Parse(reader["Year"].ToString());
                newGame.OtherName = reader["Developer"] == DBNull.Value ? string.Empty : reader["Developer"].ToString();
                newGame.Players = reader["Players"] == DBNull.Value ? new Nullable<int>() : int.Parse(reader["Players"].ToString());
                newGame.Genre = reader["Genre"] == DBNull.Value ? string.Empty : reader["Genre"].ToString();
                newGame.Annotation = reader["Annotation"] == DBNull.Value ? string.Empty : reader["Annotation"].ToString();
                newGame.Url = reader["Url"] == DBNull.Value ? string.Empty : reader["Url"].ToString();
                newGame.ImgUrl = reader["ImgUrl"] == DBNull.Value ? string.Empty : reader["ImgUrl"].ToString();


                result.Add(newGame);
            }

            dbConnection.Close();

            return result;
        }

        List<GameLink> gamesLinks;
        private void button3_Click(object sender, EventArgs e)
        {
            gamesLinks = GetBaseLinkDownloaded();

            ProgressValue = 0;
            ProgressMax = gamesLinks.Count+2;
            ProgressValue = progressValue + 1;

            wrk = new BackgroundWorker();
            wrk.DoWork += Wrk_DoWork1;
            // wrk.ProgressChanged += Wrk_ProgressChanged;
            wrk.RunWorkerCompleted += Wrk_RunWorkerCompleted;
            wrk.RunWorkerAsync();
        }

        private void Wrk_DoWork1(object sender, DoWorkEventArgs e)
        {
            RestYaDiskSample.YandexDiskClient ya_client = new RestYaDiskSample.YandexDiskClient("AgAAAAAGq9GCAAXRAb30qVKOiEknksyK2vlHa2E");
            for (int i = 0; i < gamesLinks.Count(); i++)
            {
                string directoryFile;
                if (gamesLinks[i].type_url == 1)
                    directoryFile = gamesLinks[i].platform_alias+"/roms/";
                else directoryFile = gamesLinks[i].platform_alias + "/pics/";

                
                System.IO.FileInfo uploadFile = new System.IO.FileInfo(gamesLinks[i].url);
                if (!ya_client.PUT(directoryFile + uploadFile.Name.Replace('&','_'), System.IO.File.ReadAllBytes(uploadFile.FullName)))
                    TextLog = textLog + "\n" + gamesLinks[i].game_id + ".  " + gamesLinks[i].url;

                gamesLinks[i].url_alter = directoryFile + uploadFile.Name.Replace('&', '_');

                if (!UpdateLink(gamesLinks[i]))
                    TextLog = textLog + "\n" + gamesLinks[i].game_id + ".  " + gamesLinks[i].url;

                ProgressValue = progressValue + 1;
            }
        }


        public bool UpdateLink(GameLink gameLink)
        {
            try
            {
                dbConnection = new SQLiteConnection("Data Source=library_base.sqlite;Version=3;");
                dbConnection.Open();

                string sql = "UPDATE rl_game_link SET url_alter = '" + gameLink.url_alter + "' where link_id = " + gameLink.link_id.ToString();

                SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                dbConnection.Close();

                return true;
            }
            catch (Exception ex) { return false; }
        }

        public List<GameLink> GetBaseLinkDownloaded()
        {
            dbConnection = new SQLiteConnection("Data Source=library_base.sqlite;Version=3;");
            dbConnection.Open();
            List<GameLink> result = new List<GameLink>();
            string sql = "SELECT lnk.*,pl.platform_alias FROM rl_game_link lnk\n"+
                         "JOIN rl_games_base gb ON lnk.game_id = gb.game_id\n"+
                         "JOIN rl_platforms pl ON gb.platform_id = pl.platform_id\n"+
                         "WHERE url_alter is null";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;


            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                GameLink newGame = new GameLink();
                newGame.link_id = int.Parse(reader["link_id"].ToString());
                newGame.game_id = int.Parse(reader["game_id"].ToString());
                newGame.url = reader["url"].ToString();
                newGame.url_alter = reader["url_alter"] == DBNull.Value ? string.Empty : reader["url_alter"].ToString();
                newGame.platform_alias = reader["platform_alias"].ToString();
                newGame.type_url = int.Parse(reader["type_url"].ToString());

                result.Add(newGame);
            }

            dbConnection.Close();

            return result;
        }
    }

    public class GameLink 
    {
        public int link_id;
        public int game_id;
        public string url;
        public string url_alter;
        public int type_url;
        public string platform_alias;
    }
}
