using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media.Media3D;
using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using WPF_PROJ;
using WPF_MOVE;
using System.Windows.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WPF_PROXY;

/// <summary>
/// <HelixViewport3D>
/// création d'une propriété attachée (Orbite) typé Vector3D censé reflété la position de la caméra
/// </HelixViewport3D>
/// </summary>
public class BindingBehavior : Behavior<HelixViewport3D>
{
    public static readonly DependencyProperty OrbiteBind =
    DependencyProperty.Register(
        nameof(Orbite),
        typeof(Vector3D),
        typeof(BindingBehavior),
        new PropertyMetadata(new Vector3D(),Behavior)); // rappel de changement de valeur 

    public Vector3D Orbite
    {
        get => (Vector3D)GetValue(OrbiteBind);
        set => SetValue(OrbiteBind, value);
    }
    protected override void OnAttached()
    {
        base.OnAttached();

        var binding = BindingOperations.GetBindingExpression(this, OrbiteBind);
        var value = GetValue(OrbiteBind);
        var trigger = new DependencyPropertyChangedEventArgs(OrbiteBind, null, value);

        if (binding != null)
        {
            Trace.TraceInformation($"Binding Status: {binding?.Status}");
            Trace.TraceInformation($"Current Orbite value: {value.GetType().Name}");
            Trace.TraceInformation($"Value Orbite est différent de null ?: {value != null}");

            Association(trigger);
        }
    }

    private void Association(DependencyPropertyChangedEventArgs e)
    {
        Trace.TraceInformation($"NewValue est différent de null ?: {e.NewValue != null}");

        if (e.NewValue is Vector3D move)
        {
            AssociatedObject.Camera.Position = new Point3D(
                move.X,
                move.Y,
                move.Z);
        }
    }

    /// <summary>
    /// la vrai valeur bindée arrive via ce callback 
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void Behavior(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindingBehavior behavior && behavior.AssociatedObject != null)
        {
            behavior.Association(e);
        }
    }
}