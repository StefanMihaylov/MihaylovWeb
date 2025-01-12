namespace Mihaylov.Api.Other.Contracts.Nexus.Models
{
    public class DockerImagesRequest
    {
        public string Repository { get; set; }

        public string ContinuationToken { get; set; }


        public DockerImagesRequest(string repository, string nextPage)
        {
            Repository = repository;
            ContinuationToken = nextPage;
        }
    }
}
