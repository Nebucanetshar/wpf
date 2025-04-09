using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace WPF_PROJ;

/// <summary>
/// calcul de projection pour définir une animation d'un mouvement 
/// </summary>
public class Projection
{
    /// <summary>
    /// il faut get; set; c'est valeurs, dans l'instance du mouvement elle reste toujours à 0
    /// </summary>
    public double Xm { get; set; } = 0;
    public double Ym { get; set; } = 0;
    public double Zm { get; set; } = 0;
    public double Om { get; set; } = 0;

    public double polaire;
    public double azimuth;


    public readonly List<Vector3D> theta = [];
    public readonly List<Vector3D> phi = [];

    public Vector3D _position;
    public Vector3D Position
    {
        get => _position;
        set => _position = value;
    }

    public Vector3D _longitude;
    public Vector3D Longitude
    {
        get => _longitude;
        set => _longitude = value;
    }

    public Vector3D _latitude;
    public Vector3D Latitude
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
    /// <param name = "p1" > point de départ</param>
    /// <param name = "p2" > point d'arriver </param>
    public Vector3D Phi(double p1, double p2)
    {
        while (p1 <= 6.28 && p2 <= 6.28)
        {
            p1 += 0.01;
            p2 += 0.01;

            //calcul de projection
            double x = Math.Cos(p1) * Math.Sin(p2);
            double y = Math.Sin(p1) * Math.Sin(p2);
            double z = Math.Cos(p2);

            //configuration de l'angle
            double r = Math.Sqrt((x * x + y * y + z * z));
            azimuth = Math.Acos(z / r) * 180 / Math.PI;

            _longitude = new Vector3D(0, -1, azimuth);

            if (_longitude.Length > 0)
            {
                _longitude.Normalize();
            }

            //stocke les valeurs de la longitude
            phi.Add(_longitude);

            //Trace.TraceInformation($"longitude: {Longitude}, p1:{p1}, p2:{p2}");

        }
        return _longitude;
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe x (vertical de la scène)
    /// </summary>
    /// <param name = "p1" > point de départ</param>
    /// <param name = "p2" > point d'arriver </param>
    public Vector3D Theta(double p1, double p2)
    {
        while (p1 <= 3.14 && p2 <= 3.14)
        {
            p1 += 0.01;
            p2 += 0.01;

            //calcul de projection
            double z = Math.Cos(p1) * Math.Sin(p2);
            double y = Math.Sin(p1) * Math.Sin(p2);

            //configuration de l'angle polaire
            polaire = Math.Atan2(z, y) * 180 / Math.PI;

            _latitude = new Vector3D(polaire, -1, 0);

            if (_latitude.Length > 0)
            {
                _latitude.Normalize();
            }

            //stocke les valeurs de la latitude
            theta.Add(_latitude);

            //Trace.TraceInformation($"latitude: {Latitude}, p1:{p1}, p2:{p2}");
        }

        return _latitude;
    }
}