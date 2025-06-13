using Syncfusion.Maui.DataGrid.Exporting;
using Syncfusion.Pdf;
using System.Globalization;

namespace AISalesDashboard;

public partial class ProductList : ContentView
{
	public ProductList()
	{
		InitializeComponent();
	}
    private void ExportAsPDF(object sender, TappedEventArgs e)
    {
        MemoryStream stream = new MemoryStream();
        DataGridPdfExportingController pdfExport = new DataGridPdfExportingController();
        DataGridPdfExportingOption option = new DataGridPdfExportingOption();
        var pdfDoc = new PdfDocument();
        pdfDoc = pdfExport.ExportToPdf(this.dataGrid, option);
        pdfDoc.Save(stream);
        pdfDoc.Close(true);
        SaveService saveService = new();
        saveService.SaveAndView("ExportFeature.pdf", "application/pdf", stream);
    }
}

public class ProductStockCountContentBackground : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double stockCount || parameter is not string param)
            return Colors.Transparent;

        return param switch
        {
            "Background" => stockCount < 10 ? Color.FromArgb("#FEF2F2") : Color.FromArgb("#F0FDF4"),
            "TextColor" => stockCount < 10 ? Color.FromArgb("#DC2626") : Color.FromArgb("#15803D"),
            _ => Colors.Transparent
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ProductIdFormatter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int id)
        {
            return $"P{id:D4}";
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str && str.StartsWith("P"))
        {
            if (int.TryParse(str.Substring(1), out int result))
                return result;
        }
        return value;
    }
}