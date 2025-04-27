using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using WPF_PROJ;
using WPF_MOVE;
using System.Windows.Data;
using System.ComponentModel;


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
        new PropertyMetadata(new Projection(), OrbiteChanged));

    private DependencyPropertyChangedEventArgs e;

    public Projection Orbite
    {
        get => (Projection)GetValue(OrbiteBind);
        set => SetValue(OrbiteBind, value);
    }

    /// <summary>
    /// CallBack, WPF appelle d'abord OnOrbiteChanged avec OldValue = unset et NewValue = null resulte d'une difficulté d'introspection 
    /// à cause d'un Binding actif mais comprenant une valeur non résolue (UnsetValue != null) dans le CLR 
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OrbiteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindingBehavior behavior && behavior.AssociatedObject != null)
        {
            behavior.Association(e);
        }
    }

    /// <summary>
    /// le binding n'est pas encore actif ?? si il l'est !
    /// REMARQUE Guillaume: pourquoi ne pas paramétré le type Orbite directement ? 
    /// </summary>
    /// <param name="e"></param>
    private void Association(DependencyPropertyChangedEventArgs e)
    {
        Trace.TraceInformation($"e.NewValue différent de null ?: {e.NewValue != null}");
        Trace.TraceInformation($"Type e.NewValue: {e.NewValue?.GetType().Name}");
        Trace.TraceInformation($"Assemblie e.NewValue: {e.NewValue?.GetType().AssemblyQualifiedName}");


        if (e.NewValue is Projection newCam)
        {
            AssociatedObject.Camera.Position = (Point3D)newCam.Position;
        }
    }

    /// <summary>
    /// le binding renvoie un type object en mémoire qui n'est pas forcément le même type attendu par e.NewValue
    /// le DependecyProperty est bindé mais la valeur n'est pas encore injecté 
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        if (Orbite != null) 
        {
            var binding = BindingOperations.GetBindingExpression(this, OrbiteBind);
            var value = GetValue(OrbiteBind);

            Trace.TraceInformation($"Binding Status: {binding?.Status}");
            Trace.TraceInformation($"Current Orbite value: {value.GetType().Name ?? "Null"}");

            Association(e);
        }
    }
}