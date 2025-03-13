using HelixToolkit.Wpf;
using System.Diagnostics;
using System.Windows.Media;
using WPF_MOVE;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.ComponentModel;
using WPF_ENGINE;
using System.Windows.Media.Media3D;


namespace WPF_CONTROL;

public class Control : Moteur
{
    #region singleton
    public static Control _instance;
    public static Control Instance => _instance ??= new Control();
    #endregion 

    #region Command Orbital
    public ICommand Marche { get; }
    #endregion

    public Control()
    {
        Marche = new RelayCommand<bool>(OrbitalAnimation);
    }

}
