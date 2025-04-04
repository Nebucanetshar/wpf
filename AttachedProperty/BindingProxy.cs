using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using WPF_MOVE;
using WPF_PROJ;

namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property avec animation car la propriété Camera de "http://helix-toolkit.org/wpf" effectue que des BindingProxy
/// </summary>
public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(Projection), //animation de ma projection pour la propriété Orbite dans le fichier Mouvement.cs
            typeof(BindingProxy),
            new UIPropertyMetadata(null));

    protected override Freezable CreateInstanceCore() => new BindingProxy();

    public Projection Data 
    {
        get => (Projection)GetValue(DataProperty);
        set => SetValue(DataProperty, value); 
    }
}
