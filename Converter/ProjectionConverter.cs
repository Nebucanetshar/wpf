using System.Diagnostics;
using System.Windows.Media.Media3D;
using WPF_CONTROL;
using HelixToolkit.Wpf;
using WPF_PROJ;


namespace WPF_MOVE;

//public static class ProjectionConverter
//{
//    /// <summary>
//    /// centralisé la logique d'affichage liée à une projection
//    /// </summary>
//    /// <param name="ur"></param>
//    /// <returns></returns>
//    public static ProjectionCamera ConvertToCamera(Projection ur)
//    {
//        var camera = new PerspectiveCamera
//        {
//            Position = new Point3D(ur.Position.X, ur.Position.Y, ur.Position.Z),
//            LookDirection = new Vector3D(0, -1, 0),
//            UpDirection = new Vector3D(0, 0, 1),
//            FieldOfView = 150,
//        };

//        return camera;
//    }


//}