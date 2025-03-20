using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Media.Media3D;

namespace WPF_PROJ;

/// <summary>
/// il faut incrémenté les points de -1 à 1 
/// </summary>
public class Projection
{
    Point3D[] Xm;
    Point3D[] Ym;
    Point3D[] Zm;
    Point3D[] Om;

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
        Phi(Xm,Ym);
        Theta(Zm,Om);
        M();
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe z (horizontal de la scène)
    /// </summary>
    /// <param name="p1"> point de départ </param>
    /// <param name="p2"> point d'arriver </param>
    public Vector3D Phi(Point3D[] p1, Point3D[] p2)
    {
        int i = -1;
        int j = 1;

        while ( i > 1 && j < -1)
        {
            //variation du point selon l'axe x et y 
            double dx = p2[i].X - p1[j].X;
            double dy = p2[i].Y - p1[j].Y;

            i += 1 / 100;
            j -= 1 / 100;

            //configuration de l'angle phi 
            double azimuth = Math.Atan2(dx, dy);

            //projection de l'angle sur l'axe x et y 
            double r = 1.0;
            double x = r * Math.Cos(azimuth);
            double y = r * Math.Sin(azimuth);

            _longitude = new Vector3D(x, y, 0);
            
            Trace.TraceInformation($"points de phi: {p2[i].X},{p1[i].X}");
        }

        return _longitude;
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe x (vertical de la scène)
    /// </summary>
    /// <param name="p3"> point de départ </param>
    /// <param name="p4"> point d'arriver </param>
    public Vector3D Theta(Point3D[] p3, Point3D[] p4)
    {
        int i = -1;
        int j = 1;

        while ( i < 1 && j < -1)
        {
            //variation du point selon l'axe z, r est la magnitude unitaire 
            double dz = p4[i].Z - p3[j].Z;

            i += 1 / 100;
            j -= 1 / 100;

            double r = 1.0;

            //configuration de l'angle polaire
            double polaire = Math.Acos(dz / r);

            //projection de l'angle sur l'axe z et y 
            double z = r * Math.Cos(polaire);
            double y = r * Math.Sin(polaire);

            _latitude = new Vector3D(0, y, z);

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