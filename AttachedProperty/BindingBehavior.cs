using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using WPF_MOVE;


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
            new PropertyMetadata(OnCameraChanged));
    
    public readonly DependencyPropertyChangedEventArgs e;
    
    public ProjectionCamera Camera
    {
        get => (ProjectionCamera)GetValue(CameraProperty);

        set => SetValue(CameraProperty, value);
    }

    public void Association(DependencyPropertyChangedEventArgs e)
    {
        var newCam = e.NewValue as ProjectionCamera;

        AssociatedObject.Camera.Position = newCam.Position;
        AssociatedObject.Camera.LookDirection = newCam.LookDirection;
        AssociatedObject.Camera.UpDirection = newCam.UpDirection;
    }

    private static void OnCameraChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
    {
        if (d is BindingBehavior behavior)
        {
            behavior.Association(e);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        
        if (Camera != null)
        {
            Association(e);
        }
    }
}
