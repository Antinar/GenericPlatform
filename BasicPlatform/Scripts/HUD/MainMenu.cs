using Godot;
using System;
using System.Collections.Generic;

public partial class MainMenu : Control
{
    [Export] private Control _mainButtons;
    [Export] private Control _settingsPanel;
    [Export] private Control _BackButton ; 

    public override void _Ready()
    {
       //When we first execute we put visible the mainButtons and hidde the buttons of the setting
        _mainButtons.Visible = true;
        _settingsPanel.Visible = false;
        _BackButton.Visible = false;
    }

    private void OnSettingsButtonPressed()
    {
        _mainButtons.Visible = false;
        _settingsPanel.Visible = true;
        _BackButton.Visible = true;
    }

    private void OnBackButtonPressed()
    {
        _mainButtons.Visible = true;
        _settingsPanel.Visible = false;
        _BackButton.Visible = false;
    }

    //We use a Dictionary for the resolution of our proyect
    private readonly Dictionary<long, Vector2I> _resolutions = new()
    {
        { 0, new Vector2I(1280, 720) },
        { 1, new Vector2I(1600, 900) },
        { 2, new Vector2I(1920, 1080) }
    };

    //We will connect the signal toggled(bool) of the CheckButton Node
    private void OnFullscreenToggled(bool toggledOn)
    {
        if (toggledOn)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            //This function will center the window when we exit the fullscreen mode
            CenterWindow();
        }
    }
    //We will connect the signal item_selected(int) of the OptionButton Node
    private void OnResolutionSelected(int index)
    {
        if (_resolutions.ContainsKey(index))
        {
            Vector2I newSize = _resolutions[index];
            DisplayServer.WindowSetSize(newSize);
            CenterWindow();
        }
    }

    private void CenterWindow()
    {
        Vector2I screenSize = DisplayServer.ScreenGetSize();
        Vector2I windowSize = DisplayServer.WindowGetSize();
        DisplayServer.WindowSetPosition(screenSize / 2 - windowSize / 2);
    }

    
}