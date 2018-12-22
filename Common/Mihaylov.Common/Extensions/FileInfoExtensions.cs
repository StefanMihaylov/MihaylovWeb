namespace Mihaylov.Common.Extensions
{
    using System.IO;

    public static class FileInfoExtensions
    {
        public static void Rename(this FileInfo fileInfo, string newFileName)
        {
            string newFilePath = fileInfo.Directory.FullName + @"\" + newFileName;
            fileInfo.MoveTo(newFilePath);
        }
    }
}
