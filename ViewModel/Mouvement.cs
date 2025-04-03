using HelixToolkit.Wpf;
using System.ComponentModel;
using WPF_PROJ;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight.Command;
using System.Collections.Specialized;
using WPF3D_MVVM;
using System.Windows.Media.Animation;


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
    ///animation du mouvement orbital dans un espace sphérique avec MAJ camera pour chaque frame 
    /// </summary>
    private void OnRendering(object? sender, EventArgs e)
    {
        ////MAJ position de la camera pour chaque frame du produit vectoriel 
        //Orbite.Position = Orbite.M();

        //Trace.TraceInformation($"MAJ Camera: {Orbite.Position}");

        ////Trigger l'event MAJ de la camera 
        //OnPropertyChanged(nameof(Orbite));

        Orbite.Position = Orbite.UpdatePosition();

        Trace.TraceInformation($"Position : {Orbite.Position}");

        OnPropertyChanged(nameof(Orbite));// est ce que le changement de property et configuré pour l'animation ?  
    }

    public void StartAnimation(bool type)
    {
        if (type == true)
        {
            CompositionTarget.Rendering += OnRendering;
        }

        _animate = type;
        Trace.TraceInformation($"commandParameter EventArgs: {type}");
    }

    public event PropertyChangedEventHandler? PropertyChanged; 
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}