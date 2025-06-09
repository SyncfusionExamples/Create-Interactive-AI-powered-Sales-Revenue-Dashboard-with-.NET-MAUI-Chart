using Syncfusion.Maui.DataGrid.Exporting;
using Syncfusion.Maui.Toolkit.Charts;
using Syncfusion.Pdf;
using System.Globalization;

namespace AISalesDashboard;

public partial class ProductDetailsAndroid : ContentView
{
	public ProductDetailsAndroid()
	{
		InitializeComponent();
	}

    private void Export_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        //if (exportSelection.SelectedIndex == 1)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    DataGridPdfExportingController pdfExport = new DataGridPdfExportingController();
        //    DataGridPdfExportingOption option = new DataGridPdfExportingOption();
        //    var pdfDoc = new PdfDocument();
        //    pdfDoc = pdfExport.ExportToPdf(this.dataGrid, option);
        //    pdfDoc.Save(stream);
        //    pdfDoc.Close(true);
        //    SaveService saveService = new();
        //    saveService.SaveAndView("ExportFeature.pdf", "application/pdf", stream);
        //}
        //else if (exportSelection.SelectedIndex == 0)
        //{
        //    DataGridExcelExportingController excelExport = new DataGridExcelExportingController();
        //    DataGridExcelExportingOption option = new DataGridExcelExportingOption();
        //    var excelEngine = excelExport.ExportToExcel(this.dataGrid, option);
        //    var workbook = excelEngine.Excel.Workbooks[0];
        //    MemoryStream stream = new MemoryStream();
        //    workbook.SaveAs(stream);
        //    workbook.Close();
        //    excelEngine.Dispose();
        //    string OutputFilename = "ExportFeature.xlsx";
        //    SaveService saveService = new();
        //    saveService.SaveAndView(OutputFilename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);
        //}
    }
}