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
    private Vector3D _target;
    public Vector3D Target
    {
        get => _target;
        set
        {
            _target = value;
            Orbite.Longitude = _target - Orbite.Position;
            OnPropertyChanged(nameof(Target));
        }
    }

    public Mouvement(double wu, double wv)
    {
        Orbite = new Projection
        {
            Position = new Vector3D(0, 1, 0),
            Latitude = new Vector3D(0, 0, wv),
            Longitude = new Vector3D(wu, 0, 0)
        };
    }

    /// <summary>
    /// animation du produit vectoriel selon la variation de phi et theta 
    /// pour avoir une distance de deplacement sphérique (orbital) U^V = Wu * Wv
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    public void OrbitalAnimation(Point3D Zm, Point3D Om, Point3D Xm, Point3D Ym)
    {
       
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
