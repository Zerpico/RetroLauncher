using System.IO;

namespace RetroLauncher.Common
{
    public static class ArchiveExtractor
    {
        public static void ExtractAll(string pathArchive, string pathToExtract)
        {

            if (!Directory.Exists(pathToExtract))
                Directory.CreateDirectory(pathToExtract);

            using var archive = new SevenZipExtractor.ArchiveFile(pathArchive);
            archive.Extract(pathToExtract, true);
        }
    }
}