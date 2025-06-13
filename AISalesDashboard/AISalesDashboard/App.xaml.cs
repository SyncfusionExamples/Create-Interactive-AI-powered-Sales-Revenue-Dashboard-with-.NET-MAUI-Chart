namespace AISalesDashboard
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF1cWWhOYVJ0WmFZfVtgcV9DaFZRQGYuP1ZhSXxWdkNiW39bdXxVT2RdWU19XUs=");
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}