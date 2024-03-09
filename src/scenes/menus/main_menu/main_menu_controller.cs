using Godot;
using System;

public partial class main_menu_controller : Control
{
    [Export]
    Button playButton;

    [Export]
    Button settingsButton;

    [Export]
    PackedScene nextScene;

    public override void _Ready()
    {
        playButton.Pressed += HandlePressPlayButton;
    }

    public override void _ExitTree()
    {
        playButton.Pressed -= HandlePressPlayButton;
    }

    private void HandlePressPlayButton()
    {
        GetTree().Root.AddChild(nextScene.Instantiate());
        QueueFree();
    }
}
