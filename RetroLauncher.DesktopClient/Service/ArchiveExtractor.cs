using System.IO;

namespace RetroLauncher.DesktopClient.Service
{
    public class ArchiveExtractor
    {
        public static void ExtractAll(string pathArchive, string pathToExtract)
        {

            if (!Directory.Exists(pathToExtract))
                Directory.CreateDirectory(pathToExtract);

           // using (Stream stream = File.OpenRead(pathArchive))
            //{
                using (SevenZipExtractor.ArchiveFile archive = new SevenZipExtractor.ArchiveFile(pathArchive))
                    archive.Extract(pathToExtract, true);




           // }


        }
    }
}