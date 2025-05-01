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

    public Vector3D _position;
    public Vector3D Position
    {
        get => _position;
        set
        {
            _position = value;

            Trace.TraceInformation($"[OnPropertyChanged] Position update to {_position}");
            
            OnPropertyChanged(nameof(Position));
        }
    }

    private bool _animate;
    public bool Animate
    {
        get => _animate;
        set
        {
            _animate = value;
        }
    }
    /// <summary>
    /// La classe Mouvement appelle le constructeur Projection en base 
    /// </summary>
    public Mouvement() : base()
    {
        Position = new Vector3D(Xm, Ym, Zm);
    }

    ///<summary>
    ///le déplacement de la position est illustré par le produit vectoriel itéré créant l'animation du mouvement orbital 
    ///dans un espace sphérique avec MAJ camera pour chaque frame
    ///</summary>
    public void OnRendering(object? sender, EventArgs e) 
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

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}