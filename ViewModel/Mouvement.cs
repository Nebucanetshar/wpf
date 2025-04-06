using System.ComponentModel;
using WPF_PROJ;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Net;



namespace WPF_MOVE;

public class Mouvement : Projection, INotifyPropertyChanged
{
    private int frame = 0;

    private Vector3D _position;
    public Vector3D Position
    {
        get => _position;
        set => _position = value;
    }

    private Projection _orbite;
    public Projection Orbite
    {
        get => _orbite;
        set
        {
            _orbite = value;
            OnPropertyChanged(nameof(Orbite.Position));
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
    /// <summary>
    /// La classe Mouvement appelle le constructeur Projection en base 
    /// </summary>
    public Mouvement(): base() 
    {
        Orbite = new Projection()
        {
            Position = new Vector3D(Xm, Ym, Zm),
            Longitude = new Vector3D(0, -1, 0),
            Latitude = new Vector3D(0, 0, 1),
        };
    }

    ///<summary>
    ///le déplacement de la position est illustré par le produit vectoriel itéré créant l'animation du mouvement orbital 
    ///dans un espace sphérique avec MAJ camera pour chaque frame
    ///</summary>
    private void OnRendering(object? sender, EventArgs e)
    {
        int i = frame % Math.Min(phi.Count, theta.Count);

        _position = Vector3D.CrossProduct(phi[i], theta[i]);

        if (_position.Length > 1)
        {
            _position.Normalize();
        }

        Trace.TraceInformation($"position: {_position}");

        frame++;
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