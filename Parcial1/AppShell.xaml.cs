namespace Parcial1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ModelsPage), typeof(ModelsPage));
    }
}