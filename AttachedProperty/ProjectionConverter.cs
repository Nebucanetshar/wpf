using System.Windows.Media.Media3D;
using WPF_CONTROL;

namespace WPF_PROJ;

public static class ProjectionConverter
{
    public static ProjectionCamera ConvertToCamera(Projection ur)
    {
        if (ur == null) return new PerspectiveCamera();

        //return new PerspectiveCamera
        //{
        //    Position = new Point3D(ur.Position.X, ur.Position.Y, ur.Position.Z),
        //    LookDirection = new Vector3D(ur.Longitude.X, ur.Longitude.Y,ur.Longitude.Z),
        //    UpDirection = new Vector3D(ur.Latitude.X, ur.Latitude.Y, ur.Latitude.Z),
        //    FieldOfView = 150,
        //};

        return new PerspectiveCamera
        {
            Position = new Point3D(ur.Position.X, ur.Position.Y, ur.Position.Z),
            LookDirection = new Vector3D(0, -1, 0), // regarde le centre 
            UpDirection = new Vector3D(0, 0, 1), // Correction de l'axe 
            FieldOfView = 150,
        };
    }
}