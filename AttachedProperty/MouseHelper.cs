using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;

namespace WPF_MOUSE;

/// <summary>
/// création d'une attached Property car MousePosition n'existe pas nativement en Xaml
/// </summary>
public static class MouseHelper
{
    public static readonly DependencyProperty MousePositionProperty =
        DependencyProperty.RegisterAttached(
            "MousePosition", // nom de la propriété 
            typeof(Point), // type de la propriété 
            typeof(MouseHelper), //type propriétaire
            new PropertyMetadata(default(Point))); //valeur par default 

    /// <summary>
    /// affecte une nouvelle position de la souris à un élément
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Point GetMousePosition(UIElement element)
    {
        return (Point)element.GetValue(MousePositionProperty);
    }
    /// <summary>
    /// récupère la position actuelle stocké
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetMousePosition(UIElement element, Point value)
    {
        element.SetValue(MousePositionProperty, value);
    }

    /// <summary>
    /// capture l'éveénement de la souris (MouseMove ect..) vérifie que sender est bien un élément UIElement
    /// (Grid, HelixViewport3D ect...) calcul la position actuel de la souris avec e.GetPosition et met a jour 
    /// la propriété attaché avec SetMousePosition
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void UpdateMousePosition(object sender, MouseEventArgs e)
    {
        if(sender is UIElement element)
        {
            SetMousePosition(element, e.GetPosition(element));
        }
    }
}
