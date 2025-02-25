using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Sphère_MVVM;

public class Sphère : INotifyPropertyChanged
{
    private Point3D _sphère;
    public Point3D BindSphère
    {
        get => _sphère;
        set
        {
            _sphère = value;
            OnPropertyChanged(nameof(BindSphère));
        }
    }

    private Brush _color;
    public Brush BindColor
    {
        get => _color;
        set
        {
            _color= value;
            OnPropertyChanged(nameof(BindColor));
        }
    }

    public Sphère()
    {
        BindSphère = new Point3D(0, 0, 1);
        BindColor = Brushes.Green;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
