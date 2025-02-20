//using System.ComponentModel;
//using System.Windows.Media.Media3D;

//namespace WPF3D_MVVM;

//public class Spiral : INotifyPropertyChanged
//{
//    private Point3DCollection _point;
//    public int i = 0;
//    public double a = 0.2;
//    public double b = 0.1;
//    public Point3DCollection Courbe
//    {
//        get => _point;
//        set
//        {
//            _point = value;
//            OnPropertyChanged(nameof(Courbe));
//        }
//    }

//    public Spiral()
//    {
//        _point = new Point3DCollection();
//        GenerateSpiral();
//    }

//    private void GenerateSpiral()
//    {
//        for ( i =0; i<100; i++)
//        {
//            double t = i * 0.2;
//            double x = a * t * Math.Cos(t);
//            double y = a * t * Math.Sin(t);
//            double z = b * t;

//            _point.Add(new Point3D(x, y, z));
//        }

//        Courbe = _point;
//    }

//    public event PropertyChangedEventHandler? PropertyChanged;
//    protected void OnPropertyChanged(string propertyName)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
    
//}
