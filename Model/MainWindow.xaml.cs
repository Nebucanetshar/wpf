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
        
        DataContext = new Mouvement();

        // permet la conversion de ma projection avec celui d'Helix.ProjectionCamera 
        var proxy = (BindingProxy)FindResource("proxy");

        if (proxy?.Data is Mouvement ur)
        {
            //Helix.Camera = ProjectionConverter.ConvertToCamera(ur.Orbite);

            //rotation libre (par défault)
            Helix.CameraRotationMode = CameraRotationMode.Trackball;
 
            Helix.Camera.AnimateTo(
                new Point3D(ur.Orbite.Position.X, ur.Orbite.Position.Y, ur.Orbite.Position.Z),
                new Vector3D(0, -1, 0),
                new Vector3D(0, 0, 1),
                150);
        }
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