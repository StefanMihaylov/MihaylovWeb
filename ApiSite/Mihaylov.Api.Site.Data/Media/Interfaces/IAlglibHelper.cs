using System.Collections.Generic;
using Mihaylov.Api.Site.Contracts.Helpers.Models;

namespace Mihaylov.Api.Site.Data.Media.Interfaces;

public interface IAlglibHelper
{
    void GetDuplucatesHash(List<DuplicateModel> duplicateList, IEnumerable<MediaInfoModel> files, double minDistance);
}
