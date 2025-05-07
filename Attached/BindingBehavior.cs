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
        new PropertyMetadata(new Vector3D(),Trigger));

    public Vector3D Orbite
    {
        get => (Vector3D)GetValue(OrbiteBind);
        set => SetValue(OrbiteBind, value); //c'est lui qui déclenche le callBack mais à quel moment ? 
        
    }

    /// <summary>
    /// methode provenant de l'abstract Behavior basé sur l'implémentation (T)base.AssociatedObject de l'abstract Behavior<T> quand T est hérité d'une DependencyObject, le biniding proxy 
    /// héritant de Freezable lui est définie une CreateInstanceCore() appeler "Position" (ref: .xaml l.24)
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        var binding = BindingOperations.GetBindingExpression(this, OrbiteBind);
        var value = GetValue(OrbiteBind);
        
        var change = new DependencyPropertyChangedEventArgs(OrbiteBind, null, value);//n'est pas détecté par le système WPF 

        Trace.TraceInformation($"binding status: {binding?.Status}");
        Trace.TraceInformation($"binding value: {value.GetType().Name}");
        Trace.TraceInformation($"binding value est différent de null ?: {value != null}");

        if (value is Vector3D _old)
        {
            AssociatedObject.Camera.Position = new Point3D(
                _old.X,
                _old.Y,
                _old.Z );

            Association(change);
            
        }
    }

    /// <summary>
    /// Association des anciennes valeur de la position avec les nouvelles 
    /// </summary>
    /// <param name="e"></param>
    private void Association(DependencyPropertyChangedEventArgs e)
    {
        Trace.TraceInformation($"l'association de valeur peut-il ce faire ?: {e.NewValue != null}");

        if (e.NewValue is Vector3D _new)
        {
            AssociatedObject.Camera.Position = new Point3D(
                _new.X,
                _new.Y,
                _new.Z);
        }
    }

    /// <summary>
    /// rappel de changement de valeur (CallBack) le SetValue demande au système WPF de mettre à jour la propriété Orbite, 
    /// WPF ensuite déclenche beahvior si la valeur change
    /// </summary>
    /// <param name="d"></param>
    /// <param name="move"></param>
    private static void Trigger(DependencyObject d, DependencyPropertyChangedEventArgs move)
    {
        if (d is BindingBehavior behavior && behavior.AssociatedObject != null)
        {
            behavior.Association(move);
        }
    }
}