using Parcial1.ViewModels;

namespace Parcial1;

public partial class ModelsPage : ContentPage
{
    public ModelsPage(ModelsViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}