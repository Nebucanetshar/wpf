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
    /// <summary>
    /// séparé la propriété du type d'instanciation pour implémenté une méthode d'héritage
    /// </summary>
    private readonly RelayCommand<bool> _marche;
    public ICommand Marche => _marche;
    
    #endregion

    /// <summary>
    /// relayCommand à un argument spécifié de type booléen défini à false par défault, illustre un garde-fou 
    /// évitant des comportement indésirable d'instanciation au demarrage de l'application,
    /// cela compromet l'éxécution de ma methode soumis à une condition true, comment faire switcher ce boolean ? 
    /// </summary>
    public Control()
    {
        _marche = new RelayCommand<bool>(param =>
        {
            if (param is bool value)
            {
                StartAnimation(true);
                _marche.RaiseCanExecuteChanged(); // switch du boolean !!
            }
        });
    }
}
