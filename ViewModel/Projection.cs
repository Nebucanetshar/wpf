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
    
    double polaire;
    double azimuth;


    private Vector3D _position;
    public Vector3D Position
    {
        get => _position;
        set => _position = value;
    }

    private List<Vector3D> _longitude;
    public List<Vector3D> Longitude
    {
        get => _longitude;
        set => _longitude = value;
    }

    private List<Vector3D> _latitude;
    public  List<Vector3D> Latitude
    {
        get => _latitude;
        set => _latitude = value;
    }

    public Projection()
    {
        Longitude = Phi(Xm, Ym);
        Latitude = Theta(Zm, Om);
        M();
        
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe z (horizontal de la scène)
    /// </summary>
    /// <param name="p1"> point de départ </param>
    /// <param name="p2"> point d'arriver </param>
    public List<Vector3D> Phi(double p1, double p2)
    {
        List<Vector3D> result = [];

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
            azimuth = Math.Acos(z / r) * 180 / Math.PI;

            Vector3D longitude = new Vector3D(0, -1, azimuth);

            _longitude = result.Add(longitude);

            Trace.TraceInformation($"longitude: {longitude}");
        }

        return _longitude;
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe x (vertical de la scène)
    /// </summary>
    /// <param name="p1"> point de départ </param>
    /// <param name="p2"> point d'arriver </param>
    public List<Vector3D> Theta(double p1, double p2)
    {
        List<Vector3D> result = [];

        while (p1 <= 3.14 && p2 <= 3.14)
        {
            p1 += 0.01;
            p2 += 0.01;

            //projection de l'angle polaire
            double z = Math.Cos(p1) * Math.Sin(p2);
            double y = Math.Sin(p1) * Math.Sin(p2);

            //configuration de l'angle polaire
            polaire = Math.Atan2(z, y) * 180 / Math.PI;

             Vector3D latitude = new Vector3D(polaire, -1, 0);

            //stocke le vecteur dans la liste
            _latitude = result.Add(latitude);

            Trace.TraceInformation($"latitude: {_latitude}");
        }

        return _latitude;
    }

    /// <summary>
    /// la position illustre le produit vectoriel de U^V = Wu * Wv
    /// </summary>
    public Vector3D M()
    {
        List<Vector3D> longitude = Phi(Xm, Ym);
        List<Vector3D> latitude = Theta(Zm, Om);

        for (int i = 0; i < Math.Min(longitude.Count, latitude.Count); i++)
        {
            _position = Vector3D.CrossProduct(longitude[i], latitude[i]);

            Trace.TraceInformation($"position: {_position}");
        }

        return _position;
    }
}