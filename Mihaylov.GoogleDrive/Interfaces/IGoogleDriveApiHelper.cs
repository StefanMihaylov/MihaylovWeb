using System.Collections.Generic;
using Mihaylov.GoogleDrive.Models;

namespace Mihaylov.GoogleDrive.Interfaces
{
    public interface IGoogleDriveApiHelper
    {
        IEnumerable<PhotoInfo> AllFiles(string folderId);

        IEnumerable<PhotoInfo> AllFolders();

        IEnumerable<PhotoInfo> AllFolders(string rootFolderId);

        string CreateFolder(string folderName);

        PhotoInfo GetPictureInfo(string fileId);

        ICollection<string> UploadAll(string uploadDirectory);

        string UploadFile(string uploadDirectory, string fileName);

        string GetUrl(string folderName, string fileName);
    }
}
