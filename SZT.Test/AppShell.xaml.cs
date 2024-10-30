using SZT.Test.View;

namespace SZT.Test
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Items.Add(new FlyoutItem
            {
                Title = nameof(MainView),
                Route = nameof(MainView),
                Items =
                {
                    new ShellContent{
                    ContentTemplate = new DataTemplate(typeof(MainView))}
                }
            });

            Routing.RegisterRoute(nameof(MainView),typeof(MainView));

            Items.Add(new FlyoutItem
            {
                Title = nameof(ChartView),
                Route = nameof(ChartView),
                Items =
                {
                    new ShellContent{
                    ContentTemplate = new DataTemplate(typeof(ChartView))}
                }
            });
        }
    }
}
