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

        DataContext = new Mouvement();

        Helix.Camera = OriginOrdered();
    }

    public static ProjectionCamera OriginOrdered()
    {
        var camera = new PerspectiveCamera
        {
            Position = new Point3D(7, 0, 0),
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