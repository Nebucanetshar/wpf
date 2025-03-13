using HelixToolkit.Wpf;
using System.ComponentModel;
using WPF_PROJ;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.Command;
using System.Collections.Specialized;

namespace WPF_MOVE;

public class Mouvement : Projection, INotifyPropertyChanged
{
    private Projection _orbite;
    
    public Projection Orbite
    {
        get => _orbite;
        set
        {
            _orbite = value;
            OnPropertyChanged(nameof(Orbite));
        }
    }

    /// <summary>
    /// conversion de la cible en point 
    /// </summary>
    public Point3D Point => new Point3D(Target.X, Target.Y, Target.Z);
    private Vector3D _target;
    public Vector3D Target
    {
        get => _target;
        set
        {
            _target = value;
            Orbite.Longitude = _target - Orbite.Position;
            
            OnPropertyChanged(nameof(Point));
        }
    }

    public Mouvement() { }

    public Mouvement(double wu, double wv)
    {
        Orbite = new Projection
        {
            Position = new Vector3D(0, 1, 0),
            Longitude = new Vector3D(0, -1, wv),
            Latitude = new Vector3D(wu, -1, 0)
            
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
