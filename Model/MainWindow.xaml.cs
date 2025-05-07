using WPF_MOVE;
using WPF_PROXY;
using WPF_PROJ;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Diagnostics;
using System.Windows.Threading;
using System.Security.Cryptography.X509Certificates;

namespace WPF3D_MVVM;

/// <summary>
/// Interaction logic for MainWindow.xaml (il faut configurer une camera d'initialisation avec animateTo)
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var newData = new Mouvement();

        ForceUpdate(this,"proxy", newData);

        Helix.Camera = OriginOrdered();
        
    }

    /// <summary>
    /// on cherche a forcer la mise a jour du bindingProxy a la mano ou a utiliser un OneWayToSource flemme de configurer 
    /// un héritage DependencyObject pour Projection.cs
    /// </summary>
    /// <param name="context"></param>
    /// <param name="proxy"></param>
    /// <param name="newData"></param>
    public static void ForceUpdate(FrameworkElement context, string proxy, Mouvement newData)
    {
        Trace.TraceInformation($"est ce que l'élément du framework est null ?: {context != null}");
        Trace.TraceInformation($"quel est le type de l'élément du framework ?:{context?.GetType().Name}");

        Trace.TraceInformation($"est que proxy est null: {proxy != null}");
        Trace.TraceInformation($"est que Application.Current est null: {Application.Current != null}");

        var resource = Application.Current.Resources[proxy];
        Trace.TraceInformation($"aucune resource trouver: {proxy}");

        if (resource is not BindingProxy e)
        {
            Trace.TraceInformation($"existe mais n'est pas un binding proxy: type:{resource.GetType().Name}");
        }

        
        // ---> forçage ici 
        try
        {
            if (Application.Current.Resources[proxy] is BindingProxy d)
            {
                d.Data = newData;
            }
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"update ne s'effectue pas: {ex.Message}");
        }
        
    }

    public static ProjectionCamera OriginOrdered()
    {
        var camera = new PerspectiveCamera
        {
            Position = new Point3D(1, 0, 0),
            LookDirection = new Vector3D(-1, 0, 0),
            UpDirection = new Vector3D(0, 0, 1),
        };

        return camera;
    }

    /// <summary>
    /// Routage vers <i:Interaction.Triggers>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void OnViewportLoaded(object sender, RoutedEventArgs e)
    {
        MouseHelper.AttachMouseTracking(Helix);
    }
}