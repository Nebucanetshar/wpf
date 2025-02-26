using HelixToolkit.Wpf;
using System.ComponentModel;
using System.Diagnostics;
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

    public ManipCamera() { }

    #region orbital
    /// <summary>
    /// methode pour tourner la camera selon l'axe x et y 
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="speed"></param>
    public void OrbitalCamera(double dx, double dy, double speed = 0.5)
    {
        try
        {
            var camera = _viewport.Camera as PerspectiveCamera;

            if (camera == null) return;

            //rotation autour de l'axe y (horizontal de la scène)
            Vector3D x = new Vector3D(0, 1, 0);

            //rotation autour de l'axe x (vertical de la scène) calculer avec le produit croisé
            //entre deux vecteurs (look et up direction)
            Vector3D y = Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);

            //création des quaternions
            Quaternion qx = new Quaternion(x, -dx * speed);
            Quaternion qy = new Quaternion(y, -dy * speed);

            //applique la rotation aux directions
            Quaternion rotation = qx * qy;
            Matrix3D matrice = Matrix3D.Identity;
            matrice.Rotate(rotation);

            //MAJ la direction de la camera
            camera.LookDirection = matrice.Transform(camera.LookDirection);
            camera.UpDirection = matrice.Transform(camera.UpDirection);
        }
        catch (Exception ex)
        {
           Trace.TraceInformation($"ERREUR orbitalCamera: {ex.Message} ");
        }
    }

    /// <summary>
    /// capture le click gauche et stocke la position
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OrbitalCameraDown(object sender, MouseButtonEventArgs e)
    {
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
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OrbitalCameraMove(double dx,double dy,object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(_viewport);
                dx = position.X - _lastMousePosition.X;
                dy = position.Y - _lastMousePosition.Y;

                OrbitalCamera(dx, dy);
                _lastMousePosition = position; // MAJ position de la souris
            }
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"ERREUR OrbitalCameraMove {ex.Message}");
        }
    }

    /// <summary>
    /// au relachement on arrête le mouvement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OrbitalCameraUp(object sender, MouseButtonEventArgs e)
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
        if(e.ChangedButton == MouseButton.Right)
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
    private void PanCameraMove(double dx,double dy,object sender, MouseEventArgs e)
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
