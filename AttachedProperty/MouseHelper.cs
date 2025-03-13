﻿using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace WPF_PROXY;

/// <summary>
/// création d'une attached Property permet de lier la position de la souris et de l'utilisé dans le binding
/// </summary>
public static class MouseHelper
{
    #region Command
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached(
            "MouseProperty", // nom de la propriété 
            typeof(ICommand), // type de la propriété 
            typeof(MouseHelper), //type propriétaire
            new PropertyMetadata(null)); //valeur par default 

    
    public static ICommand GetCommand(UIElement element)
    {
        return (ICommand)element.GetValue(CommandProperty);
    }
   
    public static void SetCommand(UIElement element,ICommand value)
    {
        try
        {
            element.SetValue(CommandProperty,value);
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"Command MouseProperty: {ex.Message}");
        }
        
    }
    #endregion

    #region CommandParameter
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty .RegisterAttached(
            "MouseParameter",
            typeof(object),
            typeof(MouseHelper),
            new PropertyMetadata(null));

    public static bool GetCommandParameter(UIElement element)
    {
        return (bool)element.GetValue(CommandParameterProperty);
    }

    public static void SetCommandParameter(UIElement element, object value)
    {
        try
        {
            element.SetValue(CommandParameterProperty,value);
        }
        catch (Exception ex)
        {
            Trace.TraceInformation($"CommandParameter MouseParameter:{ex.Message}");
        }
        
       
    }
    #endregion

    #region AttachTracking
    /// <summary>
    /// capture la position de la souris et MAJ l'attached Property
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void UpdateMousePosition(object sender, MouseEventArgs e)
    {
        if (sender is UIElement element)
        {
            ICommand command = GetCommand(element);
            bool commandParameter = GetCommandParameter(element);

            if( command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
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
    #endregion
}
