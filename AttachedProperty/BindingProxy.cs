using System.Windows;
using System.Windows.Media.Media3D;
using WPF_MOVE;
using WPF_PROJ;


namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property à Camera de "http://helix-toolkit.org/wpf" 
/// </summary>
public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(Mouvement),
            typeof(BindingProxy),
            new UIPropertyMetadata(null)); //valeur de la propriété Orbite dans le fichier Mouvement.cs 

    protected override Freezable CreateInstanceCore() => new BindingProxy();

    public Mouvement Data
    {
        get => (Mouvement)GetValue(DataProperty);

        set => SetValue(DataProperty, value);
    }
}
