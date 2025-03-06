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

    private Point _lastMousePosition;

    private double _dx = 0.01;
    private double _dy = 0.01;
    private double _sensitivity = 0.1;

    #region Command Orbital
    public ICommand ClicDownLeft { get; }
    public ICommand OrbitalMove { get; }
    public ICommand ClicUpLeft { get; }
    #endregion

    #region binding orbital
    private ProjectionCamera? _orbite;
    public ProjectionCamera Orbite
    {
        get => _orbite;
        set
        {
            _orbite = value;
            OnPropertyChanged(nameof(Orbite));
        }
    }

    private Point3D _target;
    public Point3D Target
    {
        get => _target;
        set
        {
            _target = value;
            Orbite.LookDirection = _target - Orbite.Position;
            OnPropertyChanged(nameof(Target));
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
            Position = new Point3D(0, 1, 0),
            LookDirection = new Vector3D(0, -1, 0),
            UpDirection = new Vector3D(0, 1, 0),
            FieldOfView = 150
        };

        ClicDownLeft = new RelayCommand<Point>(OrbitalCameraDown);
        OrbitalMove = new RelayCommand<Point>(OrbitalCameraMove);
        ClicUpLeft = new RelayCommand(OrbitalCameraUp);
    }

    #region orbital
    /// <summary>
    /// methode pour tourner la camera selon l'axe x et y 
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="speed"></param>
    public void OrbitalCamera(double dx, double dy, double speed = 0.01)
    {
        try
        {
            if (_orbite != null) return;

            //centre pour permettre le point de pivot
            _target = new Point3D(0, 0, 0);

            //rotation autour de l'axe y (horizontal de la scène)
            Vector3D x = new Vector3D(0, 1, 0);
            

            //rotation autour de l'axe x (vertical de la scène) calculer avec le produit croisé
            //entre deux vecteurs (look et up direction)
            Vector3D y = Vector3D.CrossProduct(_orbite.LookDirection, _orbite.UpDirection);
            y.Normalize();
            Trace.TraceInformation($"Produit croisé selon y: {y}");

            //création des quaternions
            Quaternion wx = new Quaternion(x, - _dx * speed *Math.PI/180);
            Quaternion wy = new Quaternion(y, - _dy * speed* Math.PI/180);
            Trace.TraceInformation($"wx:{wx},wy:{wy}");

            //applique la rotation aux directions
            Quaternion rotation = wx * wy;
            Matrix3D matrice = Matrix3D.Identity;
            matrice.Rotate(rotation);

            //MAJ la direction de la camera
            Vector3D qz = matrice.Transform(_orbite.LookDirection);
            qz.Normalize();
            
            Trace.TraceInformation($"Orthonormé de qz:{qz.X},{qz.Y},{qz.Z}, Magnitude: {Math.Sqrt(qz.X * qz.X + qz.Y * qz.Y + qz.Z * qz.Z)}");
           
            Vector3D qy = matrice.Transform(_orbite.UpDirection);
            Point3D qx = _target - qy * _orbite.Position.DistanceTo(_target);
            

            //limite d'inclinaison verticale et MAJ de la position de la camera
            if (Vector3D.AngleBetween(qz, new Vector3D(0, 1, 0)) > 85 && (Vector3D.AngleBetween(qz, new Vector3D(0, -1, 0)) > 85))
            {
                Vector3D ny = matrice.Transform(_orbite.UpDirection);
                Point3D nx = _target - ny * _orbite.Position.DistanceTo(_target);

                _orbite.LookDirection = qz;
                _orbite.UpDirection = ny;
                _orbite.Position = nx;
                
                Trace.TraceInformation($"Nouvelle position de la caméra: {qx}");
                OnPropertyChanged(nameof(Orbite));
            }
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"ERREUR OrbitalCamera: {ex.Message}, StackTrace:{ex.StackTrace}");
        }
    }

    /// <summary>
    /// capture le click gauche et stocke la position
    /// </summary>
    /// <param name="e"></param>
    public void OrbitalCameraDown(Point e)
    {
        try
        {
            Trace.TraceInformation("clic detecté !");

            if (_viewport != null)
            {
                _lastMousePosition = e;
                _viewport.CaptureMouse();

                OrbitalCameraMove(e);
                _lastMousePosition.X = e.X;
                _lastMousePosition.Y = e.Y;
               

                Trace.TraceInformation($"Position de la souris: {_lastMousePosition.X}, {_lastMousePosition.Y}");
            }
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"ERREUR dans OrbitalCameraDow: {ex.Message}");
        }
        
    }

    /// <summary>
    ///  on applique un deplacement proportionnel a la position
    /// </summary>
    /// <param name="position"></param>
    public void OrbitalCameraMove(Point position)
    {
        Trace.TraceInformation("interception du cameraMove");

        if (_viewport != null)
        {
            _dx = (position.X - _lastMousePosition.X) * _sensitivity;
            _dy = (position.Y - _lastMousePosition.Y) * _sensitivity;

            OrbitalCamera(_dx, _dy);
            _lastMousePosition = position; // MAJ position de la souris 
        } 
    }

    /// <summary>
    /// au relachement on arrête le mouvement
    /// </summary>
    public void OrbitalCameraUp()
    {
        _viewport.ReleaseMouseCapture();
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

            //translation autour de l'axe x (vertical de la scène) calculer avec le produit croisé
            //entre deux vecteurs (look et up direction)
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
        if (_lastMousePosition != null)
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
