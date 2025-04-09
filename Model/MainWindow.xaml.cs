using WPF_MOVE;
using WPF_PROXY;
using WPF_PROJ;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WPF3D_MVVM;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = new Mouvement(); //perd la mise à jour du constructeur Mouvement pour chaque instance 

        // permet la conversion de ma projection avec celui d'Helix.ProjectionCamera 
        var proxy = (BindingProxy)FindResource("proxy");

        if (proxy?.Data is Mouvement du)
        {
            Helix.Camera = ConvertToCamera(du.Orbite);

            Helix.Camera.AnimateTo(
                new Point3D(du.Orbite.Position.X, 1, du.Orbite.Position.Z),
                new Vector3D(0, -1, 0),
                new Vector3D(0, 0, 1),
                150);
        }
    }

    public static ProjectionCamera ConvertToCamera(Projection u)
    {
        var camera = new PerspectiveCamera
        {
            Position = new Point3D(0, 1, 0),
            LookDirection = new Vector3D(0, -1, u.Longitude.Z),
            UpDirection = new Vector3D(u.Latitude.X, 0, 1),
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