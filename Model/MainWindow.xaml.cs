using WPF_MOVE;
using WPF_PROXY;
using WPF_PROJ;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Diagnostics;
using System.Windows.Threading;

namespace WPF3D_MVVM;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = new Mouvement(); //mise à jour du constructeur Mouvement pour chaque instance 

        var proxy = (BindingProxy)FindResource("proxy");

        if (proxy?.Data is Mouvement dr)
        {
            Helix.Camera = ConvertToCamera(dr.Orbite);

            Helix.Camera.AnimateTo(
                new Point3D(dr.Orbite.Position.X, 1, dr.Orbite.Position.Z),
                new Vector3D(0, -1, 0),
                new Vector3D(0, 0, 1),
                150);
        }
    }
    /// <summary>
    ///permet la conversion de ma projection avec celui d'Helix.ProjectionCamera 
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public static ProjectionCamera ConvertToCamera(Projection r)
    {
        var camera = new PerspectiveCamera
        {
            Position = new Point3D(0, 1, 0),
            LookDirection = new Vector3D(r.Longitude.X, -1, 0),
            UpDirection = new Vector3D(0, 0, r.Latitude.Z),
            FieldOfView = 150,
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