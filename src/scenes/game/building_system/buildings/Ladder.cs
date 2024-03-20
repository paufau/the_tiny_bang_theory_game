using Game.BuildingSystem;
using Game.State;
using Godot;

public partial class Ladder : Node2D, IBuildHandler
{
    public void OnBuild()
    {
        StatesProvider.NavigatorState.AddFloatingPoint(GlobalPosition);
    }
}
