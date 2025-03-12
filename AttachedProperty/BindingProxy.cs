using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using WPF_PROJ;

namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property car la propriété Camera de "http://helix-toolkit.org/wpf" effectue que des binding proxy
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
