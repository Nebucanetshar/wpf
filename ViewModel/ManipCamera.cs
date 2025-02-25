using HelixToolkit.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPF_MAN;

public class ManipCamera : INotifyPropertyChanged
{
    public HelixViewport3D _viewport = new HelixViewport3D
    {
        Background = Brushes.LightCoral
    };
    
    //private HelixViewport3D _orbite;
    //public HelixViewport3D Orbite
    //{
    //    get => _orbite;
    //    set
    //    {
    //        _orbite = value;
    //        OnPropertyChanged(nameof(Orbite));
    //    }

    //}

    //private HelixViewport3D _zoom;
    //public HelixViewport3D Zoom
    //{
    //    get => _zoom;
    //    set
    //    {
    //        _zoom = value;
    //        OnPropertyChanged(nameof(Zoom));
    //    }
    //}

    //private HelixViewport3D _pan;
    //public HelixViewport3D Pan
    //{
    //    get => _pan;
    //    set
    //    {
    //        _pan = value;
    //        OnPropertyChanged(nameof(Pan));
    //    }
    //}

    public ManipCamera()
    {

    }
    
    public void OrbitalCamera()
    {
        
    }

    public void PanCamera()
    {

    }

    private void ZoomCamera(object sender, MouseWheelEventArgs e)
    {
        var camera = _viewport.Camera as PerspectiveCamera;
        if (camera == null)
            return;

        Vector3D direction = camera.LookDirection;
        direction.Normalize();

        double forward = e.Delta > 0 ? -2 : 2;
        camera.Position += direction * forward;

    }
    

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
