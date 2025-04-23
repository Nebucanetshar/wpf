using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using WPF_PROJ;
using WPF_MOVE;
using System.Windows.Data;


namespace WPF_PROXY;

/// <summary>
/// création d'une Attached Property car on intéragit avec le comportement de la camera dans la vue grâce à "http://schemas.microsoft.com/xaml/behaviors"
/// </summary>
public class BindingBehavior : Behavior<HelixViewport3D>
{
    public static readonly DependencyProperty OrbiteBind =
    DependencyProperty.Register(
        nameof(Orbite),
        typeof(Projection),
        typeof(BindingBehavior),
        new PropertyMetadata(new Projection(), OnOrbiteChanged));

    private DependencyPropertyChangedEventArgs e;

    public Projection Orbite
    {
        get => (Projection)GetValue(OrbiteBind);
        set => SetValue(OrbiteBind, value);
    }

    /// <summary>
    /// CallBack
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnOrbiteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindingBehavior behavior && behavior.AssociatedObject != null)
        {
            behavior.Association(e);
        }
    }

    /// <summary>
    /// le binding n'est pas encore actif ??
    /// </summary>
    /// <param name="e"></param>
    private void Association(DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is WPF_PROJ.Projection newCam)
        {
            AssociatedObject.Camera.Position = (Point3D)newCam.Position;
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (Orbite != null)
        {
            var binding = BindingOperations.GetBindingExpression(this, OrbiteBind);
            var value = GetValue(OrbiteBind);

            Trace.TraceInformation($"Binding Status: {binding?.Status}");
            Trace.TraceInformation($"Current Orbite value: {value ?? "Null"}");
            
            Association(e);
        }

    }
}

