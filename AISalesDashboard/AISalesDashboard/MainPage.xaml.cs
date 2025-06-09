namespace AISalesDashboard
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Content = new SplashScreenView();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(1500);
                ShowSampleView();
            });
        }

        public void ShowSampleView()
        {
#if ANDROID || IOS
        this.Content = new AndroidUI();

#elif WINDOWS || MACCATALYST
            this.Content = new DesktopUI();

#endif
        }
    }

}
