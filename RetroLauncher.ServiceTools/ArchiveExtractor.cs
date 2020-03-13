using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace RetroLauncher.ServiceTools
{
    public static class ArchiveExtractor
    {
        public static void ExtractAll(string pathArchive, string pathToExtract, bool deleteArchive = false)
        {
            //если конечноо пути нет, то создадим его
            if (!Directory.Exists(pathToExtract))
                Directory.CreateDirectory(pathToExtract);

            //распаковываем файлы в папку
            using (var archive = new SevenZipExtractor.ArchiveFile(pathArchive))
            {
                archive.Extract(pathToExtract, true);
            }

            //удаляем архив после распаковки, если надо
            if (deleteArchive)
                File.Delete(pathArchive);
        }
    }
}
