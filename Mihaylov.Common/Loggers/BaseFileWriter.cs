namespace Mihaylov.Common.Loggers
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Mihaylov.Common.Extensions;

    public abstract class BaseFileWriter
    {
        private const string DateTimeFormat = "dd.MM.yyyy г. HH:mm:ss.fff";
        private const string DirPattern = "yyyy_MM";
        private const string ArchiveFileExtension = "_dd_HH_mm_ss_fff";
        private const string FileExtension = ".txt";
        private const int MaximumSymbolPerLine = 50;

        private const int DefaultMaxFileSizeInBytes = 5000 * 1024;

        public BaseFileWriter(string rootDirectory, string subDirectory, string fileName)
            : this(rootDirectory, subDirectory, fileName, DefaultMaxFileSizeInBytes)
        {
        }

        public BaseFileWriter(string rootDirectory, string subDirectory, string fileName, int maxFileSizeInBytes)
        {
            this.RootDirectory = rootDirectory;
            this.SubDirectory = subDirectory;
            this.FileName = fileName;
            this.MaxFileSizeInBytes = maxFileSizeInBytes;
        }

        public string FileDirectory
        {
            get
            {
                return Path.Combine(this.RootDirectory, this.SubDirectory);
            }
        }

        private string RootDirectory { get; set; }

        private string SubDirectory { get; set; }

        private string FileName { get; set; }

        private int MaxFileSizeInBytes { get; set; }

        private string FilePath
        {
            get
            {
                var dir = Path.Combine(this.FileDirectory, DateTime.Now.ToString(DirPattern));
                var isDirExisting = Directory.Exists(dir);
                if (!isDirExisting)
                {
                    Directory.CreateDirectory(dir);
                }

                return Path.Combine(dir, this.FileName + FileExtension);
            }
        }

        protected void WriteString(string messageText)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(DateTime.Now.ToString(DateTimeFormat));

            if (messageText != null &&
                (messageText.Length > MaximumSymbolPerLine || messageText.Contains(Environment.NewLine)))
            {
                stringBuilder.AppendLine();
            }
            else
            {
                stringBuilder.Append(" : ");
            }

            stringBuilder.AppendLine(messageText);
            var text = stringBuilder.ToString();

            try
            {
                this.WriteStringToFile(text);
                this.TryRenameFileAsync();
            }
            catch (Exception ex)
            {
                bool isFileLocked = this.CheckIfFileIsLocked(ex);
                if (isFileLocked)
                {
                    this.WriteStringAsync(text);
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void WriteStringAsync(string text)
        {
            Task.Factory.StartNew(() =>
            {
                bool isOperationFinished = false;
                while (!isOperationFinished)
                {
                    Thread.Sleep(20);

                    try
                    {
                        this.WriteStringToFile(text);
                        this.TryRenameFileAsync();
                        isOperationFinished = true;
                    }
                    catch (Exception ex)
                    {
                        bool isFileLocked = this.CheckIfFileIsLocked(ex);
                        if (!isFileLocked)
                        {
                            throw ex;
                        }
                    }
                }
            });
        }

        private void WriteStringToFile(string text)
        {
            using (var stream = new StreamWriter(this.FilePath, true))
            {
                stream.WriteLine(text);
            }
        }

        private void TryRenameFileAsync()
        {
            var file = new FileInfo(this.FilePath);
            if (this.MaxFileSizeInBytes != 0 && file.Length > this.MaxFileSizeInBytes)
            {
                Task.Factory.StartNew(() =>
                {
                    bool isOperationFinished = false;
                    while (!isOperationFinished)
                    {
                        Thread.Sleep(20);

                        try
                        {
                            var fileForRename = new FileInfo(this.FilePath);
                            var newFileName = this.FileName + DateTime.Now.ToString(ArchiveFileExtension) + FileExtension;
                            fileForRename.Rename(newFileName);
                            isOperationFinished = true;
                        }
                        catch (Exception ex)
                        {
                            bool isFileLocked = this.CheckIfFileIsLocked(ex);
                            if (!isFileLocked)
                            {
                                throw ex;
                            }
                        }
                    }
                });
            }
        }

        private bool CheckIfFileIsLocked(Exception ex)
        {
            int errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
            if (errorCode == 32 || errorCode == 33)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
