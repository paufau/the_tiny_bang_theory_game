using Godot;
using System;

public partial class BuildingUI : CanvasLayer
{
    [Export]
    private Button buildLadderButton;

    [Export]
    private BuildingProcessor buildingProcessor;

    private enum Actions
    {
        NONE,
        BUILD_LADDER,
    }

    private Actions selectedAction = Actions.NONE;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed()) return;

        if (@event is InputEventMouseButton mouse)
        {
            if (mouse.ButtonIndex == MouseButton.Right)
            {
                selectedAction = Actions.NONE;
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        buildLadderButton.Pressed += HandlePressBuildLadder;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void HandlePressBuildLadder()
    {
        selectedAction = Actions.BUILD_LADDER;
        buildingProcessor.SetBuildingIntent(new BuildingProcessor.B_Ladder());
    }
}
