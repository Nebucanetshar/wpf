using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using WPF_PROJ;
using WPF_MOVE;


namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property car on intéragit avec le comportement de la camera dans la vue grâce à "http://schemas.microsoft.com/xaml/behaviors"
/// </summary>
public class BindingBehavior : Behavior<HelixViewport3D>
{
    public static readonly DependencyProperty CameraProperty =
        DependencyProperty.Register(
            nameof(Camera),
            typeof(ProjectionCamera),
            typeof(BindingBehavior),
            new PropertyMetadata(new PerspectiveCamera(),OnCameraChanged));//valeur par défaut de CameraProperty
    
    public readonly DependencyPropertyChangedEventArgs e;
    
    public ProjectionCamera Camera
    {
        get => (ProjectionCamera)GetValue(CameraProperty);

        set => SetValue(CameraProperty, value);
    }

    /// <summary>
    ///dès qu'une nouvelle projection est assignée à Orbite, l'événement de changement propulse cette nouvelle valeur vers la méthode 
    ///Association() qui adapte dynamiquement l'état de la caméra liée.
    ///Problème: Orbite est du même type que e.NewValue donc d'ou provient le NullReference ? 
    /// </summary>
    /// <param name="e"></param>
    public void Association(DependencyPropertyChangedEventArgs e)
    {
        Trace.TraceInformation($"type e.NewValue: {e.NewValue.GetType()}");

        if (e.NewValue is Projection newCam)
        {
            AssociatedObject.Camera.Position = (Point3D)newCam.Position;
            AssociatedObject.CameraRotationMode = CameraRotationMode.Turnball;
        }
    }

    /// <summary>
    /// illustration du CallBack
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnCameraChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
    {
        if (d is BindingBehavior behavior)
        {
            behavior.Association(e);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached(); //attache Orbite à Camera de HelixViewport3D 
        
        if (Camera != null)
        {
            Association(e);
        }
    }
}
