using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace WPF_PROJ;

/// <summary>
/// 
/// </summary>
public class Projection 
{
    public Point3D Zm;
    public Point3D Om;
    public Point3D Xm;
    public Point3D Ym;

    private Vector3D _position;
    public Vector3D Position
    {
        get => _position;
        set => _position = value;
    }

    private Vector3D _longitude;
    public Vector3D Longitude
    {
        get => _longitude;
        set => _longitude = value;
    }

    private Vector3D _latitude;
    public Vector3D Latitude
    {
        get => _latitude;
        set => _latitude = value;
    }

    public Projection() { }
   
    /// <summary>
    /// vecteur directionnel de p1 vers p2 autour de z (horizontal de la scène)
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public Vector3D Phi(Point3D p1, Point3D p2)
    {
       double dx = p2.X - p1.X;
       double dy = p2.Y - p1.Y;

       double azimuth = Math.Atan2(dx, dy);
        
        return _longitude = new Vector3D(0, 0, azimuth);
    }

    /// <summary>
    /// vecteur directionnel de p2 vers p4 autour de x (vertical de la scène)
    /// </summary>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    public Vector3D Theta(Point3D p3, Point3D p4)
    {
       double z = p4.Z - p3.Z;
       double r = p4.Y - p3.Y;
        
       double polaire = Math.Acos(z/r);
        
        return _latitude = new Vector3D(polaire, 0, 0);
    }

    /// <summary>
    /// la position illustre la variation du produit vectoriel de U^V = Wu * Wv
    /// </summary>
    public Vector3D M()
    {
        _position = Vector3D.CrossProduct(_latitude, _longitude);
        return _position;
    }
}
