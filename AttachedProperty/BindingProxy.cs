using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;

namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property car la propriété camera du type HelixViewport3D ne peut pas utilisé {Binding} directement
/// </summary>
public class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(object),  
            typeof(BindingProxy), 
            new UIPropertyMetadata(null));

    protected override Freezable CreateInstanceCore() => new BindingProxy();

    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value); 
    }
}
