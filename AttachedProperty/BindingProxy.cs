using System.Windows;
using WPF_MOVE;

namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property avec animation car la propriété Camera de "http://helix-toolkit.org/wpf" effectue que des BindingProxy
/// </summary>
public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(Mouvement), //animation de ma projection pour la propriété Orbite dans le fichier Mouvement.cs
            typeof(BindingProxy),
            new UIPropertyMetadata(null));

    protected override Freezable CreateInstanceCore() => new BindingProxy(); 

    public Mouvement Data 
    {
        get => (Mouvement)GetValue(DataProperty);

        set => SetValue(DataProperty, value); 
    }
}
