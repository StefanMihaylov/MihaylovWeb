using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Mihaylov.GoogleDrive.Interfaces;
using Mihaylov.GoogleDrive.Models;
using Ninject.Extensions.Logging;

namespace Mihaylov.GoogleDrive
{
    public class GoogleDriveApiHelper : IGoogleDriveApiHelper
    {
        private const string ClientId = "";
        private const string ClientSecret = "";
        private const string FolderId = ""; // Panorami

        private static readonly HashSet<string> MimeTypes = new HashSet<string>() { "image/jpeg" };

        private static Dictionary<string, Dictionary<string, string>> panoramas;

        private readonly DriveService service;
        private readonly string rootFolderId;
        private readonly ILogger logger;

        public GoogleDriveApiHelper(ILogger logger, string dataStoreFolder)
            : this(logger, ClientId, ClientSecret, FolderId, dataStoreFolder)
        {
        }

        public GoogleDriveApiHelper(ILogger logger, string clientId, string password, string rootFolderId, string dataStoreFolder)
        {
            this.ValidateString(clientId, nameof(clientId));
            this.ValidateString(password, nameof(password));
            this.ValidateString(rootFolderId, nameof(rootFolderId));
            this.ValidateString(dataStoreFolder, nameof(dataStoreFolder));

            var initialiser = this.GoogleApiSettings(clientId, password, dataStoreFolder);

            this.rootFolderId = rootFolderId;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.service = new DriveService(initialiser);

            panoramas = new Dictionary<string, Dictionary<string, string>>();

            this.logger.Info("Google Api initialized");
            this.Initialize();
        }

        public string CreateFolder(string folderName)
        {
            var fileMetadata = new File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
            };

            var request = this.service.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();

            return file.Id;
        }

        public string UploadFile(string uploadDirectory, string fileName)
        {
            File fileMetadata = new File()
            {
                Name = fileName,
                MimeType = "image/jpg"
            };

            var fileinfo = System.IO.Path.Combine(uploadDirectory, fileName);
            byte[] byteArray = System.IO.File.ReadAllBytes(fileinfo);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            FilesResource.CreateMediaUpload request = this.service.Files.Create(fileMetadata, stream, fileMetadata.FileExtension);
            request.Fields = "id";
            request.Upload();
            File file = request.ResponseBody;

            return file.Id;
        }

        public ICollection<string> UploadAll(string uploadDirectory)
        {
            var result = new List<string>();

            var directory = new System.IO.DirectoryInfo(uploadDirectory);
            foreach (var file in directory.GetFiles())
            {
                if (file.Extension == ".jpg")
                {
                    result.Add(this.UploadFile(uploadDirectory, file.Name));
                }
            }

            return result;
        }

        public PhotoInfo GetPictureInfo(string fileId)
        {
            try
            {
                File file = service.Files.Get(fileId).Execute();
                return file;
            }
            catch
            {
                return new PhotoInfo();
            }
        }

        public void Initialize()
        {
            var folders = this.AllFolders(this.rootFolderId);
            foreach (var folder in folders)
            {
                var files = this.AllFiles(folder.Id).ToDictionary(f => f.Name, f => f.Url);
                panoramas.Add(folder.Name, files);
            }
        }

        public string GetUrl(string folderName, string fileName)
        {
            return panoramas[folderName][fileName];
        }

        public IEnumerable<PhotoInfo> AllFolders()
        {
            return this.AllFolders(this.rootFolderId);
        }

        public IEnumerable<PhotoInfo> AllFolders(string rootFolderId)
        {
            var folders = this.GetAllFilesInformation(rootFolderId, "application/vnd.google-apps.folder");
            return folders;
        }

        public IEnumerable<PhotoInfo> AllFiles(string folderId)
        {
            var files = this.GetAllFilesInformation(folderId, "image/jpeg");
            return files;
        }

        private List<PhotoInfo> GetAllFilesInformation(string rootFolderId, string mimeType)
        {
            var resultFiles = new List<PhotoInfo>();
            string nextPageToken = null;

            do
            {
                FilesResource.ListRequest request = this.service.Files.List();
                request.Q = "mimeType='" + mimeType + "' and '" + rootFolderId + "' in parents";
                request.Fields = "nextPageToken, files(id, name, webContentLink, size, mimeType)";
                request.Spaces = "drive";
                request.PageSize = 1000; // max value
                request.PageToken = nextPageToken;
                FileList fileList = request.Execute();

                var files = fileList.Files.Select(f => (PhotoInfo)f);
                resultFiles.AddRange(files);

                nextPageToken = fileList.NextPageToken;
            }
            while (nextPageToken != null);

            return resultFiles;
        }

        private BaseClientService.Initializer GoogleApiSettings(string clientId, string password, string dataStoreFolder)
        {
            var secrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = password
            };

            var scopes = new[] { DriveService.Scope.Drive };
            var store = new FileDataStore(dataStoreFolder);
            var token = CancellationToken.None;

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, "user", token, store).Result;

            var initialiser = new BaseClientService.Initializer
            {
                ApplicationName = "MihaylovWeb",
                HttpClientInitializer = credential,
            };

            return initialiser;
        }

        private void ValidateString(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
