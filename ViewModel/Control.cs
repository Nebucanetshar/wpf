using HelixToolkit.Wpf;
using System.Diagnostics;
using System.Windows.Media;
using WPF_MOVE;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.ComponentModel;


namespace WPF_CONTROL;

public class Control 
{
    private Point _lastMousePosition;
    private double _dx = 0.01;
    private double _dy = 0.01;
    private double _sensitivity = 0.1;

    public HelixViewport3D _viewport = new HelixViewport3D
    {
        Background = Brushes.LightCoral
    };


    #region Command Orbital
    public ICommand ClicDownLeft { get; }
    public ICommand OrbitalMove { get; }
    public ICommand ClicUpLeft { get; }
    #endregion

    public Control()
    {
        ClicDownLeft = new RelayCommand<Point>(OrbitalCameraDown);
        OrbitalMove = new RelayCommand<Point>(OrbitalCameraMove);
        ClicUpLeft = new RelayCommand(OrbitalCameraUp);
    }

    #region Control Orbital
    /// <summary>
    /// methode pour tourner la camera selon l'axe x et y 
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    /// <param name="speed"></param>
    public void OrbitalCamera(double dx, double dy, double speed = 0.01)
    {

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

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
