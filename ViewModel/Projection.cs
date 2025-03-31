using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Media.Media3D;

namespace WPF_PROJ;

/// <summary>
/// il faut incrémenté les points de -1 à 1 
/// </summary>
public class Projection
{
    public double Xm;
    public double Ym;
    public double Zm;
    public double Om;

    private double polaire;
    private double azimuth;

    public readonly List<Vector3D> theta = [];
    public readonly List<Vector3D> phi = [];

    private double angleStep = 0.01;
    private double a = 5.0;
    private double b = 3.0;
    private int frame = 0;

   
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

    public Projection()
    {
        //Longitude = Phi(Xm, Ym);
        //Latitude = Theta(Zm, Om);
        //M();

        UpdatePosition();
        GenerateOrbit();
    }

    /// <summary>
    /// vecteur directionnel autour de l'axe z (horizontal de la scène)
    /// </summary>
    /// <param name = "p1" > point de départ</param>
    /// <param name = "p2" > point d'arriver </param>
    //public Vector3D Phi(double p1, double p2)
    //{
    //    while (p1 <= 6.28 && p2 <= 6.28)
    //    {
    //        p1 += 0.01;
    //        p2 += 0.01;

    //        //calcul de projection
    //        double x = Math.Cos(p1) * Math.Sin(p2);
    //        double y = Math.Sin(p1) * Math.Sin(p2);
    //        double z = Math.Cos(p2);

    //        //configuration de l'angle
    //        double r = Math.Sqrt((x * x + y * y + z * z));
    //        azimuth = Math.Acos(z / r) * 180 / Math.PI;

    //        _longitude = new Vector3D(0, -1, azimuth);

    //        //stocke les valeurs de la longitude
    //        phi.Add(_longitude);
    //    }

    //    return _longitude;
    //}

    /// <summary>
    /// vecteur directionnel autour de l'axe x (vertical de la scène)
    /// </summary>
    /// <param name = "p1" > point de départ</param>
    /// <param name = "p2" > point d'arriver </param>
    //public Vector3D Theta(double p1, double p2)
    //{
    //    while (p1 <= 3.14 && p2 <= 3.14)
    //    {
    //        p1 += 0.01;
    //        p2 += 0.01;

    //        //calcul de projection
    //        double z = Math.Cos(p1) * Math.Sin(p2);
    //        double y = Math.Sin(p1) * Math.Sin(p2);

    //        //configuration de l'angle polaire
    //        polaire = Math.Atan2(z, y) * 180 / Math.PI;

    //        _latitude = new Vector3D(polaire, -1, 0);

    //        //stocke les valeurs de la latitude
    //        theta.Add(_latitude);
    //    }

    //    return _latitude;
    //}

    /// <summary>
    /// le déplacement de la position est illustré par le produit vectoriel itéré 
    /// </summary>
    //public Vector3D M()
    //{
    //    for (int i = 0; i < Math.Min(phi.Count, theta.Count); i++)
    //    {
    //        _position = Vector3D.CrossProduct(phi[i], theta[i]);

    //        if (_position.Length > 1)
    //        {
    //            _position.Normalize();
    //        }

    //        Trace.TraceInformation($"position: {_position}");
    //    }

    //    return _position;
    //}

    private void GenerateOrbit()
    {
        phi.Clear();
        theta.Clear();

        for (double t = 0; t < 2 * Math.PI; t += angleStep)
        {
            double anglePhi = t; // longitude
            double angleTheta = 0.5 * Math.Sin(2 * t); // variation en latitude

            double x = a * Math.Cos(anglePhi) * Math.Cos(angleTheta);
            double y = b * Math.Sin(anglePhi) * Math.Cos(angleTheta);
            double z = Math.Sin(angleTheta); // inclinaison 

            _position = new Vector3D(x, y, z);
            _position.Normalize();

            // definition du vecteur tangeante pour la vitesse
            double dx = -a * Math.Sin(anglePhi) * Math.Cos(angleTheta);
            double dy = b * Math.Cos(anglePhi) * Math.Cos(angleTheta);
            double dz = Math.Cos(angleTheta);

            Vector3D vitesse = new Vector3D(dx, dy, dz);
            vitesse.Normalize();

            phi.Add(_position);
            theta.Add(vitesse);
        } 
    }

    public Vector3D UpdatePosition()
    {
        if (phi.Count == 0 || theta.Count == 0) return new Vector3D(0, 0, 0);

        //selection de la position courante 
        int index = frame % phi.Count;
        
        var Newposition = phi[index];
        //Vector3D newPosition = Vector3D.CrossProduct(phi[index], theta[index]);

        //Trace.TraceInformation($"CrossProduct: {newPosition}");

        //newPosition.Normalize();

        frame++;

        Trace.TraceInformation($"frame: {frame}");
        
        return Newposition;
    }
}