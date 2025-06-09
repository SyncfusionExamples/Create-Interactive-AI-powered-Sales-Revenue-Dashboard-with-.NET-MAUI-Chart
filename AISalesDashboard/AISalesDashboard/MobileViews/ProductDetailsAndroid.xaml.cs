
namespace AISalesDashboard;

public partial class ProductDetailsAndroid : ContentView
{
	public ProductDetailsAndroid()
	{
		InitializeComponent();
	}

    private void Export_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        //if (exportselection.selectedindex == 1)
        //{
        //    memorystream stream = new memorystream();
        //    datagridpdfexportingcontroller pdfexport = new datagridpdfexportingcontroller();
        //    datagridpdfexportingoption option = new datagridpdfexportingoption();
        //    var pdfdoc = new pdfdocument();
        //    pdfdoc = pdfexport.exporttopdf(this.datagrid, option);
        //    pdfdoc.save(stream);
        //    pdfdoc.close(true);
        //    saveservice saveservice = new();
        //    saveservice.saveandview("exportfeature.pdf", "application/pdf", stream);
        //}
        //else if (exportselection.selectedindex == 0)
        //{
        //    datagridexcelexportingcontroller excelexport = new datagridexcelexportingcontroller();
        //    datagridexcelexportingoption option = new datagridexcelexportingoption();
        //    var excelengine = excelexport.exporttoexcel(this.datagrid, option);
        //    var workbook = excelengine.excel.workbooks[0];
        //    memorystream stream = new memorystream();
        //    workbook.saveas(stream);
        //    workbook.close();
        //    excelengine.dispose();
        //    string outputfilename = "exportfeature.xlsx";
        //    saveservice saveservice = new();
        //    saveservice.saveandview(outputfilename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);
        //}
    }
}