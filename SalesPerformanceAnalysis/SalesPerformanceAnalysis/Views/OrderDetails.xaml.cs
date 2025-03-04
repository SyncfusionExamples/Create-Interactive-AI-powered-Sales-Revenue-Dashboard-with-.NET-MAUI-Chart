using Syncfusion.Maui.DataGrid.Exporting;

namespace SalesPerformanceAnalysis;

public partial class OrderDetails : ContentView
{
	public OrderDetails()
	{
		InitializeComponent();
	}

    public void ExportPDF(object sender, TappedEventArgs e)
    {
        MemoryStream stream = new MemoryStream();
        DataGridPdfExportingController pdfExport = new DataGridPdfExportingController();
        DataGridPdfExportingOption option = new DataGridPdfExportingOption();
        var pdfDoc = pdfExport.ExportToPdf(this.dataGrid, option);
        pdfDoc.Save(stream);
        pdfDoc.Close(true);
        SaveService saveService = new();
        saveService.SaveAndView("ExportFeature.pdf", "application/pdf", stream);
    }

    public void ExportAsExcel(object sender, TappedEventArgs e)
    {
        DataGridExcelExportingController excelExport = new DataGridExcelExportingController();
        DataGridExcelExportingOption option = new DataGridExcelExportingOption();
        var excelEngine = excelExport.ExportToExcel(this.dataGrid, option);
        var workbook = excelEngine.Excel.Workbooks[0];
        MemoryStream stream = new MemoryStream();
        workbook.SaveAs(stream);
        workbook.Close();
        excelEngine.Dispose();
        string OutputFilename = "ExportFeature.xlsx";
        SaveService saveService = new();
        saveService.SaveAndView(OutputFilename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        MemoryStream stream = new MemoryStream();
        DataGridPdfExportingController pdfExport = new DataGridPdfExportingController();
        DataGridPdfExportingOption option = new DataGridPdfExportingOption();
        var pdfDoc = pdfExport.ExportToPdf(this.dataGrid, option);
        pdfDoc.Save(stream);
        pdfDoc.Close(true);
        SaveService saveService = new();
        saveService.SaveAndView("ExportFeature.pdf", "application/pdf", stream);
    }

    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        DataGridExcelExportingController excelExport = new DataGridExcelExportingController();
        DataGridExcelExportingOption option = new DataGridExcelExportingOption();
        var excelEngine = excelExport.ExportToExcel(this.dataGrid, option);
        var workbook = excelEngine.Excel.Workbooks[0];
        MemoryStream stream = new MemoryStream();
        workbook.SaveAs(stream);
        workbook.Close();
        excelEngine.Dispose();
        string OutputFilename = "ExportFeature.xlsx";
        SaveService saveService = new();
        saveService.SaveAndView(OutputFilename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);
    }
}

