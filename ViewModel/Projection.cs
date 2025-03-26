using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Media.Media3D;

namespace WPF_PROJ;

/// <summary>
/// il faut incrémenté les points de -1 à 1 
/// </summary>
public class Projection
{
    double Xm;
    double Ym;
    double Zm;
    double Om;

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
    public Vector3D Phi(double p1, double p2)
    {
        while (p1 <= 6.28 && p2 <= 6.28)
        {
            p1 += 0.01;
            p2 += 0.01;

            //projection de l'angle 
            double x = Math.Cos(p1) * Math.Sin(p2);
            double y = Math.Sin(p1) * Math.Sin(p2);
            double z = Math.Cos(p2);

            //configuration de l'angle
            double r = Math.Sqrt((x * x + y * y + z * z));
            double azimuth = Math.Acos(z / r) * 180 / Math.PI;

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
    public Vector3D Theta(double p1, double p2)
    {
        while (p1 <= 3.14 && p2 <= 3.14)
        {
            p1 += 0.01;
            p2 += 0.01;

            //projection de l'angle polaire
            double z = Math.Cos(p1) * Math.Sin(p2);
            double y = Math.Sin(p1) * Math.Sin(p2);

            //configuration de l'angle polaire
            double polaire = Math.Atan2(z, y) * 180 / Math.PI;

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