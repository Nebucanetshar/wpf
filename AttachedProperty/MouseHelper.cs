using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;

namespace WPF_PROXY;

/// <summary>
/// création d'une attached Property car MousePosition n'existe pas nativement en Xaml
/// </summary>
public static class MouseHelper
{
    public static readonly DependencyProperty MousePositionProperty =
        DependencyProperty.RegisterAttached(
            "MousePosition", // nom de la propriété 
            typeof(MouseEventArgs), // type de la propriété 
            typeof(MouseHelper), //type propriétaire
            new PropertyMetadata(null)); //valeur par default 

    /// <summary>
    /// affecte une nouvelle position de la souris à un élément
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static MouseEventArgs GetMousePosition(UIElement element)
    {
        return (MouseEventArgs)element.GetValue(MousePositionProperty);
    }
    /// <summary>
    /// récupère la position actuelle stocké
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetMousePosition(UIElement element, MouseEventArgs value)
    {
        element.SetValue(MousePositionProperty, value);
    }

    /// <summary>
    /// Pour s'assuré de la comptibilité de typage entre RelayCommand et InvokeCommandAction dans la vue 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void UpdateMousePosition(UIElement element)
    {
        element.MouseMove += (s, e) =>
        {
            SetMousePosition(element, e);
        };
    }

   
}
