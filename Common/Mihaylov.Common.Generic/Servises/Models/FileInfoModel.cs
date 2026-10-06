using System;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides information about a file, including full path, name, extension, subdirectories, existence, size in bytes, and last write time.
    /// </summary>
    public class FileInfoModel
    {
        /// <summary> Gets or sets the full name. </summary>
        public string FullName { get; set; }

        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; }

        /// <summary> File extension, for example ".txt". </summary>
        public string Extension { get; set; }

        /// <summary> Gets or sets the subdirectory paths. </summary>
        public string SubDirectories { get; set; }

        /// <summary> Gets or sets a value indicating whether the entity exists. </summary>
        public bool Exists { get; set; }

        /// <summary> Gets or sets the length, in bytes. </summary>
        public long Length { get; set; }

        /// <summary> Gets or sets the date and time when the resource was last written. </summary>
        public DateTime LastWriteTime { get; set; }
    }
}
