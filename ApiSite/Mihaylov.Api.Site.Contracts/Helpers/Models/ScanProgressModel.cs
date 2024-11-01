namespace Mihaylov.Api.Site.Contracts.Helpers.Models
{
    public class ScanProgressModel
    {
        public int Files { get; set; }

        public int Processes { get; set; }

        public ScanProgressModel()
        {
        }

        public ScanProgressModel(int files, int processes)
        {
            Files = files;
            Processes = processes;
        }
    }
}
