using Syncfusion.Maui.DataGrid.Exporting;
using Syncfusion.Pdf;

namespace AISalesDashboard;

public partial class OrderList : ContentView
{
	public OrderList()
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