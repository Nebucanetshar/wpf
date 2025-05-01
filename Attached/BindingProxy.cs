using System.Windows;
using WPF_MOVE;

namespace WPF_PROXY;

/// <summary>
///Définition du fichier Mouvement.cs comme un DependecyObject avec le bias de Freeazable 
/// </summary>
public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(Mouvement),
            typeof(BindingProxy),
            new UIPropertyMetadata(new Mouvement()));

    protected override Freezable CreateInstanceCore() => new BindingProxy();

    public Mouvement Data
    {
        get => (Mouvement)GetValue(DataProperty);

        set => SetValue(DataProperty, value);
    }
}
