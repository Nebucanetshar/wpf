﻿using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;

namespace WPF_PROXY;

/// <summary>
/// création d'une attached Property permet de lier la position de la souris et de l'utilisé dans le binding
/// </summary>
public static class MouseHelper
{
    public static readonly DependencyProperty MousePositionProperty =
        DependencyProperty.RegisterAttached(
            "MousePosition", // nom de la propriété 
            typeof(Point), // type de la propriété 
            typeof(MouseHelper), //type propriétaire
            new PropertyMetadata(new Point(0, 0))); //valeur par default 

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
    /// definie la nouvelle position
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetMousePosition(UIElement element,Point value)
    {
        element.SetValue(MousePositionProperty, value);
    }

    /// <summary>
    /// capture la position de la souris et MAJ l'attached Property
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void UpdateMousePosition(object sender, MouseEventArgs e)
    {
        if (sender is UIElement element)
        {
            Point position = e.GetPosition(element);
            SetMousePosition(element, position);
        }
    }

    /// <summary>
    /// Configuration du routage pour la vue
    /// </summary>
    /// <param name="element"></param>
    public static void AttachMouseTracking(UIElement element)
    {
        if (element != null)
        {
            element.MouseMove -= UpdateMousePosition;
            element.MouseMove += UpdateMousePosition;

        }
    }

   
}
