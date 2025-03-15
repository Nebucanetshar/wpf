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

    public double _wu = 0;
    public double _wv = 0;

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

    private bool _animate;
    public bool Animate
    {
        get => _animate;
        set
        {
            _animate = value;
            OnPropertyChanged(nameof(Animate));
        }
    }

    public Mouvement()
    {
        Orbite = new Projection
        {
            Position = new Vector3D(0, 1, 0),
            Longitude = new Vector3D(0, -1, 0),
            Latitude = new Vector3D(0, -1, 0)
        };
    }

    /// <summary>
    /// motorise le produit vectoriel selon la variation de phi et theta 
    /// pour avoir une distance de deplacement sphérique (orbital) U^V = Wu * Wv
    /// </summary>
    private void OnRendering(object sender, EventArgs e)
    {
        while (_wu < 6.28 && _wv < 3.14)
        {
            _wu += 0.01;
            _wv += 0.01;

            Orbite = new Projection
            {
                Position = new Vector3D(0, 1, 0),
                Longitude = new Vector3D(0, -1, _wv),
                Latitude = new Vector3D(_wu, -1, 0)
            };
        }
    }

    public void StartAnimation(bool args)
    {
        if (args == true)
        {
            CompositionTarget.Rendering += OnRendering;
        }
            _animate = args;
        
        Trace.TraceInformation($"animation trigger: {args}");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}