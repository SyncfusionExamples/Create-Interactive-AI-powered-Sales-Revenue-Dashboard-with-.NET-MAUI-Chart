using Syncfusion.Maui.Maps;

namespace SalesDashboard
{
    public partial class SaveService
    {
        public partial void SaveAndView(string filename, string contentType, MemoryStream stream);
    }

    public class CustomMarker : MapMarker
    {
        public string? Name { get; set; }
    }
}
