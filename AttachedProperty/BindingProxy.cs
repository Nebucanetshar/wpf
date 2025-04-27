using System.Windows;
using System.Windows.Media.Media3D;
using WPF_MOVE;
using WPF_PROJ;


namespace WPF_PROXY;

/// <summary>
/// Instanciation de Orbite via un Freezable "http://helix-toolkit.org/wpf" 
/// </summary>
public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(Mouvement),
            typeof(BindingProxy),
            new UIPropertyMetadata(new Mouvement())); //valeur de la propriété Orbite dans le fichier Mouvement.cs 

    protected override Freezable CreateInstanceCore() => new BindingProxy();

    public Mouvement Data
    {
        get => (Mouvement)GetValue(DataProperty);

        set => SetValue(DataProperty, value);
    }
}
