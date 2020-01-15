using System;
using System.IO;


namespace RetroLauncher.Service
{
    public class ArchiveExtractor
    {
        public static void ExtractAll(string pathArchive, string pathToExtract)
        {

            if (!Directory.Exists(pathToExtract))
                Directory.CreateDirectory(pathToExtract);

            using (Stream stream = File.OpenRead(pathArchive))
            {
                using (SevenZipExtractor.ArchiveFile archive = new SevenZipExtractor.ArchiveFile(stream))
                    archive.Extract(pathToExtract, true);
            }


        }
    }
}