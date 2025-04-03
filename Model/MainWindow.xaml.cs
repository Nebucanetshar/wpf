using Sphère_MVVM;
using WPF_MOVE;
using WPF_PROXY;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using WPF_PROJ;
using WPF_CONTROL;



namespace WPF3D_MVVM;

/// <summary>
/// Interaction logic for MainWindow.xaml,Si Data="{Binding}" est défini trop tôt, 
/// WPF pourrait ne pas encore avoir attribué DataContext = new Mouvement();, donc proxy.Data reste null
/// Le Dispatcher.Invoke(..., DispatcherPriority.Loaded) permet de s'assurer que le binding 
/// est bien établi avant d'accéder à proxy.Data.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = new Mouvement();

        // permet la conversion de ma projection avec celui d'Helix.ProjectionCamera 
        var proxy = (BindingProxy)FindResource("proxy");

        if (proxy?.Data is Mouvement convert)
        {
            Helix.Camera = ProjectionConverter.ConvertToCamera(convert.Orbite);
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