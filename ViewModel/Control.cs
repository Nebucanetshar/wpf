using HelixToolkit.Wpf;
using System.Diagnostics;
using System.Windows.Media;
using WPF_MOVE;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.ComponentModel;

using System.Windows.Media.Media3D;


namespace WPF_CONTROL;

public class Control : Mouvement
{
    #region singleton
    public static Control? _instance;
    public static Control Instance => _instance ??= new Control();
    #endregion 

    #region Command Orbital
    public ICommand Marche { get; }
    
    #endregion

    /// <summary>
    /// relayCommand à un argument spécifié de type booléen défini à false par défault, illustre un garde-fou 
    /// évitant des comportement indésirable d'instanciation au demarrage de l'application,
    /// cela compromet l'éxécution de ma methode soumis à une condition true, comment faire switcher ce boolean ? 
    /// </summary>
    public Control()
    {
        Marche = new RelayCommand<bool>(element =>
        {
            if (element is bool type)
            {
                StartAnimation(true);
            }
        });
    }
}
