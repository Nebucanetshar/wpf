using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using Microsoft.Xaml.Behaviors;


namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property car on interagit avec un comportement dans la vue grâce à "http://schemas.microsoft.com/xaml/behaviors"
/// </summary>
public class BindingBehavior : Behavior<HelixViewport3D>
{
    public static readonly DependencyProperty CameraProperty =
        DependencyProperty.Register(
            nameof(Camera),
            typeof(ProjectionCamera),
            typeof(BindingBehavior),
            new PropertyMetadata(null, OnCameraChanged));

    public ProjectionCamera Camera
    {
        get => (ProjectionCamera)GetValue(CameraProperty);
        set => SetValue(CameraProperty, value);
    }

    private static void OnCameraChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindingBehavior behavior && behavior.AssociatedObject != null)
        {
            behavior.AssociatedObject.Camera = e.NewValue as ProjectionCamera;
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (Camera != null)
        {
            AssociatedObject.Camera = Camera;
        }

    }
}
