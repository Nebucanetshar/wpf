using System.Diagnostics;
using System.Windows.Media.Media3D;
using WPF_CONTROL;

namespace WPF_PROJ;

public static class ProjectionConverter
{
    public static ProjectionCamera ConvertToCamera(Projection ur)
    {
        var camera = new PerspectiveCamera
        {
            Position = new Point3D(ur.Position.X, ur.Position.Y, ur.Position.Z),
            LookDirection = new Vector3D(0, -1, 0), 
            UpDirection = new Vector3D(0, 0, 1), 
            FieldOfView = 150,
        };

        return camera;
    }
}