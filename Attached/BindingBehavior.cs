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
        typeof(Vector3D),
        typeof(BindingBehavior),
        new PropertyMetadata(new Vector3D(), Behavior));

    private DependencyPropertyChangedEventArgs e;
    private readonly Mouvement _current = new();
    
    public Vector3D Orbite
    {
        get => (Vector3D)GetValue(OrbiteBind);
        set => SetValue(OrbiteBind, value);
    }

    /// <summary>
    /// CallBack, WPF appelle d'abord OnOrbiteChanged avec OldValue = unset et NewValue = null resulte d'une difficulté d'introspection 
    /// à cause d'un Binding actif mais comprenant une valeur non résolue (UnsetValue != null) dans le CLR 
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

    /// <summary>
    /// le binding n'est pas encore actif ?? si il l'est !
    /// REMARQUE Guillaume: pourquoi ne pas paramétré le type Orbite directement ? pas besoin grâce à (T)Base.AssociatedObject 
    /// </summary>
    /// <param name="e"></param>
    private void Association(DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is Vector3D)
        {
            AssociatedObject.Camera.Position = new Point3D(
                _current.Position.X,
                _current.Position.Y,
                _current.Position.Z);
        }
    }

    /// <summary>
    /// un struct est toujours true par defaut alors on s'assure de la validité du status du binde dans le OnAttached() car le dependencyObject
    /// correspond la propriété CLR Position dans le fichier Mouvement.cs
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        var binding = BindingOperations.GetBindingExpression(this, OrbiteBind);
        var value = GetValue(OrbiteBind);

        if (binding != null)
        {
            Trace.TraceInformation($"Binding Status: {binding?.Status}");
            Trace.TraceInformation($"Current Orbite value: {value.GetType().Name}");

            Association(e);
        }
    }
}