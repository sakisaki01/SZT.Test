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
                Title = nameof(ChartView),
                Route = nameof(ChartView),
                Items =
                {
                    new ShellContent{
                    ContentTemplate = new DataTemplate(typeof(ChartView))}
                }
            });

            Items.Add(new FlyoutItem
            {
                Title = nameof(DataSelectView),
                Route = nameof(DataSelectView),
                Items =
                {
                    new ShellContent{
                    ContentTemplate = new DataTemplate(typeof(DataSelectView))}
                }
            });

            Routing.RegisterRoute(nameof(MainView),typeof(MainView));

            Routing.RegisterRoute(nameof(DataSelectView),typeof(DataSelectView));

            Routing.RegisterRoute(nameof(ChartView),typeof(ChartView));


        }
    }
}
