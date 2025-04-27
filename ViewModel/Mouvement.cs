using System.ComponentModel;
using WPF_PROJ;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Net;
using System.Windows;



namespace WPF_MOVE;

public class Mouvement : Projection, INotifyPropertyChanged
{
    private int frame = 0;

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
    public Point3D Point => new(Target.X, Target.Y, Target.Z);
    private Vector3D _target;
    public Vector3D Target
    {
        get => _target;
        set
        {
             _target = Orbite.Longitude;
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
    /// <summary>
    /// La classe Mouvement appelle le constructeur Projection en base 
    /// </summary>
    public Mouvement(): base() 
    {
        Orbite = new Projection() 
        {
            Position = new Vector3D(Xm, 1, Zm),
        };
    }

    ///<summary>
    ///le déplacement de la position est illustré par le produit vectoriel itéré créant l'animation du mouvement orbital 
    ///dans un espace sphérique avec MAJ camera pour chaque frame
    ///</summary>
    public void OnRendering(object? sender, EventArgs e) // utilisation d'une autre propriété animable ? 
    {
        int i = frame % Math.Min(phi.Count, theta.Count);

        _position = Vector3D.CrossProduct(phi[i], theta[i]);

        if (_position.Length > 0)
        {
            _position.Normalize();
        }

        frame++;

        Trace.TraceInformation($"frame: {frame}, position: {_position}");
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


    /// <summary>
    /// pourquoi est ce qu'il n'est pas configurer directement avec le freezable ou le dependecyObject ?
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged; 
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}