using Game.InputSystem;
using Godot;
using System;

public partial class BuildingUI : CanvasLayer
{
    [Export]
    private Button buildLadderButton;
    private InputProcessor buildLadderProcessor;

    [Export]
    private Button buildBoxButton;
    private InputProcessor buildBoxProcessor;

    [Export]
    private Button digButton;
    private InputProcessor digInputProcessor;

    [Export]
    private Button buildBedButton;
    private InputProcessor buildBedProcessor;

    [Export]
    private BuildingProcessor buildingProcessor;

    [Export]
    private BreakProcessor diggingProcessor;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        digInputProcessor = new(digButton, () =>
        {
            diggingProcessor.EnableBreaking();
        });

        buildBoxProcessor = new(buildBoxButton, () =>
        {
            buildingProcessor.SetBuildingIntent(new BuildingProcessor.B_Box());
        });

        buildLadderProcessor = new(buildLadderButton, () =>
        {
            buildingProcessor.SetBuildingIntent(new BuildingProcessor.B_Ladder());
        });

        buildBedProcessor = new(buildBedButton, () =>
        {
            buildingProcessor.SetBuildingIntent(new BuildingProcessor.B_Bed());
        });
    }

    public override void _ExitTree()
    {
        digInputProcessor.Dispose();
        buildBoxProcessor.Dispose();
        buildLadderProcessor.Dispose();
        buildBedProcessor.Dispose();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
