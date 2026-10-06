using System.Collections.Generic;
using System.IO;

namespace Mihaylov.Common
{
    /// <summary>
    /// Provides an abstraction for file and directory operations, including reading and writing files and streams,
    /// enumerating files and directories, and manipulating file-system entries.
    /// </summary>
    public interface IFileWrapper
    {
        /// <summary>
        /// Reads all bytes from the file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to read.</param>
        /// <returns>A byte array containing the file contents.</returns>
        byte[] ReadFile(string path);

        /// <summary>
        /// Opens a Stream for the file at the specified path.
        /// </summary>
        /// <param name="path">The file path to open.</param>
        /// <returns>A Stream for the file at the specified path.</returns>
        Stream GetStreamFile(string path);

        /// <summary>
        /// Saves the contents of the provided stream to the specified file path.
        /// </summary>
        /// <param name="stream">Stream that provides the data to write to the file.</param>
        /// <param name="path">File system path (absolute or relative) where the stream will be saved.</param>
        void SaveStreamFile(Stream stream, string path);

        /// <summary>
        /// Saves the specified content to a file at the given path.
        /// </summary>
        /// <param name="path">The file system path where the content will be written; existing files are overwritten.</param>
        /// <param name="content">The string content to write to the file.</param>
        void SaveFile(string path, string content);

        /// <summary>
        /// Enumerates files in the specified directory and returns FileInfoModel instances; can include files from
        /// subdirectories and produce paths relative to basePath when provided.
        /// </summary>
        /// <param name="directoryPath">Path of the directory to enumerate.</param>
        /// <param name="includeSubdirectories">True to include files from all subdirectories; false to enumerate only the top-level directory.</param>
        /// <param name="basePath">Optional base path used to compute relative file paths; when null, file paths are absolute.</param>
        /// <returns>An enumerable of FileInfoModel representing each discovered file.</returns>
        IEnumerable<FileInfoModel> GetAllFiles(string directoryPath, bool includeSubdirectories, string basePath = null);

        /// <summary>
        /// Gets information about the file at the specified path.
        /// </summary>
        /// <param name="path">The file system path of the file.</param>
        /// <returns>A FileInfoModel that describes the specified file.</returns>
        FileInfoModel GetFileInfo(string path);

        /// <summary>
        /// Deletes the file at the specified path.
        /// </summary>
        /// <param name="filePath">Path to the file to delete.</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Retrieves information for the subdirectories of the specified directory path.
        /// </summary>
        /// <param name="directoryPath">The path of the directory whose subdirectories are enumerated.</param>
        /// <returns>A sequence of DirInfoModel objects representing each subdirectory of the specified directory; empty if no
        /// subdirectories are present.</returns>
        IEnumerable<DirInfoModel> GetDirectories(string directoryPath);

        /// <summary>
        /// Creates a subdirectory with the specified name under the provided base path and returns its full path.
        /// </summary>
        /// <param name="basePath">Base directory path under which the new directory will be created.</param>
        /// <param name="name">Name of the directory to create; may contain relative path segments but must not be an absolute path.</param>
        /// <returns>Full path of the created directory.</returns>
        string CreateDirectory(string basePath, string name);

        /// <summary>
        /// Moves the specified files into the destination directory.
        /// </summary>
        /// <param name="dir">The path of the destination directory.</param>
        /// <param name="files">A sequence of file paths to move.</param>
        void MoveFiles(string dir, IEnumerable<string> files);
    }
}
