namespace Mihaylov.Api.Site.Contracts.Hubs
{
    public class UpdateProgressBarModel
    {
        public int Total { get; set; }

        public int Single { get; set; }

        public int Count { get; set; }

        public UpdateProgressBarModel()
        {
        }

        public UpdateProgressBarModel(int single, int total, int count)
        {
            Single = single;
            Total = total;
            Count = count;
        }
    }
}
