using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Media.Media3D;

namespace WPF_PROJ;

/// <summary>
/// il faut incrémenté les points de -1 à 1 
/// </summary>
public class Projection
{
    Point3D[] Xm = new Point3D[10];
    Point3D[] Ym = new Point3D[10];
    Point3D[] Zm = new Point3D[10];
    Point3D[] Om = new Point3D[10];

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
    public  Vector3D Latitude
    {
        get => _latitude;
        set => _latitude = value;
    }

    public Projection()
    {
        Longitude = Phi(Xm, Ym);
        Latitude = Theta(Zm, Om);
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe z (horizontal de la scène)
    /// </summary>
    /// <param name="p1"> point de départ </param>
    /// <param name="p2"> point d'arriver </param>
    public Vector3D Phi(Point3D[] p1, Point3D[] p2)
    {
        double t = 0;
        
       while(t <= 6.28)
       {
            t += 0.01;

            //projection de l'angle sur l'axe x et y 
            double r = 1.0;
            double dx = r * Math.Cos(t);
            double dy = r * Math.Sin(t);

            //configuration de l'angle phi 
            double azimuth = Math.Atan2(dx, dy) * 180 / Math.PI;

            _longitude = new Vector3D(0, -1, azimuth);

            Trace.TraceInformation($"longitude: {_longitude}");
       }

        return _longitude;
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe x (vertical de la scène)
    /// </summary>
    /// <param name="p1"> point de départ </param>
    /// <param name="p2"> point d'arriver </param>
    public Vector3D Theta(Point3D[] p1, Point3D[] p2)
    {
        double t = 0;

        while (t <= 6.28)
        {
            t += 0.1;

            //projection de l'angle sur l'axe z et y 
            double r = 1.0;
            double dz = r * Math.Cos(t);

            //configuration de l'angle polaire
            double polaire = Math.Acos(dz / r) * 180/Math.PI;

            _latitude = new Vector3D(polaire, -1, 0);

            Trace.TraceInformation($"latitude: {_latitude}");
        }

        return _latitude;
    }

    /// <summary>
    /// la position illustre le produit vectoriel de U^V = Wu * Wv
    /// </summary>
    public Vector3D M()
    {
        _position = Vector3D.CrossProduct(_latitude, _longitude);

        return _position;
    }
}