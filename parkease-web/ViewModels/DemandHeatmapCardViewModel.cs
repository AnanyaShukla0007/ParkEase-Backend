namespace ParkEase.Web.ViewModels;

public class DemandHeatmapCardViewModel
{
    public string City { get; set; } = string.Empty;
    public int EventCount { get; set; }
    public int NoResultCount { get; set; }
    public int FullLotCount { get; set; }
    public int AbandonedSearchCount { get; set; }
}
