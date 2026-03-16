using Godot;
using System;

public partial class ExitButton : Button
{
    // This function will be vinculated with the signal "pressed" of the button
    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}