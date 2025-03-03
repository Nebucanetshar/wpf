using HelixToolkit.Wpf;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.Command;

namespace WPF_MAN;

public class ManipCamera : INotifyPropertyChanged
{
    public HelixViewport3D _viewport = new HelixViewport3D
    {
        Background = Brushes.LightCoral
    };

    private Point? _lastMousePosition = null;

    private double _dx;
    private double _dy;
    private double _sensitivity = 5;

    public ICommand ClicDown { get; }
    public ICommand OrbitalMove { get; }
    public ICommand ClicUp { get; }

    #region binding orbital
    private ProjectionCamera _orbite;
    public ProjectionCamera Orbite
    {
        get => _orbite;
        set
        {
            _orbite = value;
            OnPropertyChanged(nameof(Orbite));
        }
    }
    #endregion
    #region binding pan
    private HelixViewport3D _pan;
    public HelixViewport3D Pan
    {
        get => _pan;
        set
        {
            _pan = value;
            OnPropertyChanged(nameof(Pan));
        }
    }
    #endregion

    #region binding zoom
    private HelixViewport3D _zoom;
    public HelixViewport3D Zoom
    {
        get => _zoom;
        set
        {
            _zoom = value;
            OnPropertyChanged(nameof(Zoom));
        }
    }
    #endregion

    public ManipCamera()
    {
        Orbite = new PerspectiveCamera
        {
            Position = new Point3D(6, 6, 5),
            LookDirection = new Vector3D(-1,-1,-1),
            UpDirection = new Vector3D(-1, -1, 0),
            FieldOfView = 90
        };

        ClicDown = new RelayCommand<MouseButtonEventArgs>(OrbitalCameraDown);
        OrbitalMove = new RelayCommand<Point>(OrbitalCameraMove);
        ClicUp = new RelayCommand(OrbitalCameraUp);
    }

    #region orbital
    /// <summary>
    /// methode pour tourner la camera selon l'axe x et y 
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="speed"></param>
    public void OrbitalCamera(double dx ,double dy, double speed = 0.5)
    {
        try
        {
            if (_orbite == null) return;

            //centre de rotation (point de pivot)
            Point3D target = new Point3D(0, 0, 0);

            //rotation autour de l'axe y (horizontal de la scène)
            Vector3D x = new Vector3D(0, 1, 0);

            //rotation autour de l'axe x (vertical de la scène) calculer avec le produit croisé
            //entre deux vecteurs (look et up direction)
            Vector3D y = Vector3D.CrossProduct(_orbite.LookDirection, _orbite.UpDirection);
            y.Normalize();

            //création des quaternions
            Quaternion wx = new Quaternion(x, - _dx * speed);
            Quaternion wy = new Quaternion(y, - _dy * speed);

            //applique la rotation aux directions
            Quaternion rotation = wx * wy;
            Matrix3D matrice = Matrix3D.Identity;
            matrice.Rotate(rotation);

            //MAJ la direction de la camera
            Vector3D qz = matrice.Transform(_orbite.LookDirection);
            Vector3D qy = matrice.Transform(_orbite.UpDirection);
            Point3D qx = target - qz * _orbite.Position.DistanceTo(target);
            
            //limite d'inclinaison verticale et MAJ de la position de la camera
            if (Vector3D.AngleBetween(qz, new Vector3D(0,1,0)) > 5 && (Vector3D.AngleBetween(qz,new Vector3D(0,-1,0)) > 5))
            {
                _orbite.LookDirection = qz;
                _orbite.UpDirection = qy;
                _orbite.Position = qx;

                OnPropertyChanged(nameof(Orbite));
            }
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"ERREUR OrbitalCamera: {ex.Message} ");
        }
    }

    /// <summary>
    /// capture le click gauche et stocke la position
    /// </summary>
    /// <param name="e"></param>
    public void OrbitalCameraDown(MouseButtonEventArgs e)
    {
        Trace.TraceInformation("interception du clicDown");
        try
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _lastMousePosition = e.GetPosition(_viewport);
                _viewport.CaptureMouse();
            }
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"ERREUR OrbitalCameraDown: {ex.Message}");
        }
    }

    /// <summary>
    /// deplace la camera en fonction du mouvement de la souris
    /// </summary>
    /// <param name="e"></param>
    public void OrbitalCameraMove(Point position)
    {
        Trace.TraceInformation("interception du cameraMove");

        if (_lastMousePosition != null)
        {
            _dx = position.X - _lastMousePosition.Value.X * _sensitivity;
            _dy = position.Y - _lastMousePosition.Value.Y * _sensitivity;

            OrbitalCamera(_dy, _dx);
            _lastMousePosition = position; // MAJ position de la souris 
        }
    }

    /// <summary>
    /// au relachement on arrête le mouvement
    /// </summary>
    public void OrbitalCameraUp()
    {
        try
        {
            //libère la souris
            _viewport.ReleaseMouseCapture();
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"ERREUR OrbitalCameraUp: {ex.Message}");
        }
    }
    #endregion

    #region panoramique
    /// <summary>
    /// translation de la camera de gauche a droite et de haut en bas 
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    private void PanCamera(double dx, double dy)
    {
        if (_viewport.Camera is PerspectiveCamera camera)
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
        if (e.ChangedButton == MouseButton.Right)
        {
            _lastMousePosition = e.GetPosition(_viewport);
            _viewport.CaptureMouse();
        }
    }
    /// <summary>
    /// on applique un deplacement proportionnel a la position
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PanCameraMove(double dx, double dy, object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            Point currentPosition = e.GetPosition(_viewport);
            dx = (currentPosition.X - _lastMousePosition.X) * 0.01;
            dy = (currentPosition.Y - _lastMousePosition.Y) * 0.01;

            PanCamera(dx, dy);
            _lastMousePosition = currentPosition;
        }
    }

    /// <summary>
    /// au relachement on arrête le mouvement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PanCameraUp(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _viewport.ReleaseMouseCapture();
        }
    }
    #endregion

    #region zoom
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
    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
