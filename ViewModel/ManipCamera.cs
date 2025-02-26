using HelixToolkit.Wpf;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
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

    private Point _lastMousePosition;

    
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
        _viewport.CameraController.IsRotationEnabled = false;
    }
    /// <summary>
    /// translation de la camera de gauche a droite et de haut en bas 
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    private void PanCamera(double dx, double dy)
    {
        if(_viewport.Camera is PerspectiveCamera camera)
        {
           
           //vecteur de deplacement en x et y dans le repère orthonormé de la camera
            Vector3D x = Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);
            x.Normalize();
            Vector3D y = camera.UpDirection;
            y.Normalize();
           
            //calcul du deplacement final
            Vector3D translation = (-dx * x) + (dy * y);
            
            //appliqué le deplacement à la position de la camera
            camera.Position += translation;
        }
    }
    /// <summary>
    /// on enregistre la position de la souris
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PanCameraDown(object sender, MouseButtonEventArgs e)
    {
        if(e.ChangedButton == MouseButton.Left)
        {
            _lastMousePosition = e.GetPosition(_viewport);
            _viewport.CaptureMouse();
            e.Handled = true;
        }
    }
    /// <summary>
    /// on applique un deplacement proportionnel a la position
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PanCameraMove(double dx,double dy,object sender, MouseEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed)
        {
            Point currentPosition = e.GetPosition(_viewport);
            dx = (currentPosition.X - _lastMousePosition.X) * 0.01;
            dy = (currentPosition.Y - _lastMousePosition.Y) * 0.01;

            PanCamera(dx, dy);
            _lastMousePosition=currentPosition;
            e.Handled = true;
        }
    }

    /// <summary>
    /// au relachement on arrête le mouvement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PanCameraUp(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            _viewport.ReleaseMouseCapture();
            e.Handled = true;
        }
    }


    /// <summary>
    /// mouvement du zoom d'avant en arrière avec la roulette de la souris 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ZoomCamera(object sender, MouseWheelEventArgs e)
    {
        var camera = _viewport.Camera as PerspectiveCamera;
        if (camera == null)
            return;

        Vector3D direction = camera.LookDirection;
        direction.Normalize();

        double z = e.Delta > 0 ? -2 : 2;
        camera.Position += direction * z;

    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
