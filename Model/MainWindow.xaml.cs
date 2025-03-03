using Sphère_MVVM;
using WPF_MAN;
using WPF_PROXY;


using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        DataContext = new ManipCamera();
       
       // acces a une instance de ManipCamera depuis BindingProxy pour lier Helix.Camera 
        var proxy = (BindingProxy)FindResource("proxy");
        if(proxy?.Data is ManipCamera manipCamera)
        {
            Helix.Camera = manipCamera.Orbite;
        }
    }

    /// <summary>
    /// ajout de la méthode AttachMouseTracking pour attacher correctement MouseMouve
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnViewportLoaded(object sender, RoutedEventArgs e)
    {
        MouseHelper.AttachMouseTracking(Helix);
    }
}