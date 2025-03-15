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
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //Loaded += OnViewportLoaded;
        
        Mouvement move = new Mouvement();
        this.DataContext = move;

        // permet la conversion de Mouvement.Orbite avec celui d'Helix.Camera 
        var proxy = (BindingProxy)FindResource("proxy");
        if(proxy?.Data is Mouvement orbital)
        {
            Helix.Camera = ProjectionConverter.ConvertToCamera(orbital.Orbite);
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