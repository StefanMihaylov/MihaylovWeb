namespace Mihaylov.Common
{
    /// <summary>
    /// Represents basic information about a directory, including its full path, name, and file count.
    /// </summary>
    public class DirInfoModel
    {
        /// <summary>
        /// Gets or sets the absolute filesystem path of the file or directory represented by the instance.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of files.
        /// </summary>
        public int FilesCount { get; set; }
    }
}
