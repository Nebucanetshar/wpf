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
        double t = -1.0;
        
       while(t <= 1.0)
       {
            //conversion de t dans l'indice de tableau avec la formule ((t+1)/2) * (n-1) pour mapper un intervale [-1;1] vers un intervalle [0;n-1]
            int i = (int)(((t + 1.0) / 2.0) * (10));
            int j = (int)(((t + 1.0) / 2.0) * (10));

            //variation du point selon l'axe x et y 
            double dx = p2[i].X - p1[i].X;
            double dy = p2[j].Y - p1[j].Y;

            t += 0.1;

            //configuration de l'angle phi 
            double azimuth = Math.Atan2(dx, dy);

            //projection de l'angle sur l'axe x et y 
            double r = 1.0;
            double x = r * Math.Cos(azimuth);
            double y = r * Math.Sin(azimuth);

            _longitude = new Vector3D(x, y, 0);

            Trace.TraceInformation($" i: {i}, t: {t}, dx: {dx}, dy: {dy}");
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
        double t = -1.0;

        while (t <= 1.0)
        {
            //conversion de t dans l'indice de tableau avec la formule ((t+1)/2) * (n-1) pour mapper un intervale [-1;1] vers un intervalle [0;n-1]
            int k = (int)(((t + 1.0) / 2.0) * (10));
            
            //variation du point selon l'axe z, r est la magnitude unitaire 
            double dz = p2[k].Z - p1[k].Z;
            double r = 1.0;

            t += 0.1;

            //configuration de l'angle polaire
            double polaire = Math.Acos(dz / r);

            //projection de l'angle sur l'axe z et y 
            double z = r * Math.Cos(polaire);
            double y = r * Math.Sin(polaire);

            _latitude = new Vector3D(0, y, z);

            Trace.TraceInformation($" k: {k}, t: {t}, dz: {dz}");
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