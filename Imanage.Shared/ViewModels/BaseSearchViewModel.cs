namespace Imanage.Shared.ViewModels
{
    public class BaseSearchViewModel
    {
        public string Keyword { get; set; }
        public string Filter { get; set; }
        public int? PageIndex { get; set; }
        public int? PageTotal { get; set; }
        public int? PageSize { get; set; }
    }
}