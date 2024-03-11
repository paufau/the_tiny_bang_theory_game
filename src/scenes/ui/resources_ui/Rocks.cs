using Game.State;
using Godot;
using System;

public partial class Rocks : HBoxContainer
{
    private Label countLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        countLabel = GetNode<Label>("Count");
        StatesProvider.Rocks.Update += UpdateCount;
        UpdateCount(StatesProvider.Rocks.storedRocks);
    }

    public override void _ExitTree()
    {
        StatesProvider.Rocks.Update -= UpdateCount;
    }

    public void UpdateCount(int next)
    {
        countLabel.Text = next.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
