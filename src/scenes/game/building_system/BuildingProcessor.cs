using Game.BuildingSystem;
using Godot;
using System;

public partial class BuildingProcessor : Node2D
{
    private TileMap tileMap;
    private int tileSize;

    [Export]
    private navigator nav;

    private int buildingLayer = 3;

    public Node2D? activeBuilding;

    public abstract class Building
    {
        public abstract PackedScene GetScene();
    }

    public class B_Ladder : Building
    {
        public override PackedScene GetScene()
        {
            return ResourceLoader.Load<PackedScene>("res://src/scenes/game/building_system/buildings/ladder.tscn");
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        tileMap = GetParent<TileMap>();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed()) return;

        if (@event is InputEventMouseButton mouseButton) {
            if (activeBuilding != null && mouseButton.ButtonIndex == MouseButton.Left)
            {
                BuildAt();
            } else if (activeBuilding != null && mouseButton.ButtonIndex == MouseButton.Right)
            {
                ClearBuildingIntent();
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (activeBuilding == null) return;

        var mousePosition = GetGlobalMousePosition();
        var snappedPosition = ToGlobal(
            tileMap.MapToLocal(
                tileMap.LocalToMap(
                    ToLocal(
                        mousePosition))));
        activeBuilding.GlobalPosition = snappedPosition;
    }

    public Node2D InstantiateBuilding(Building building)
    {
        var instance = (Node2D)building.GetScene().Instantiate();
        AddChild(instance);
        return instance;
    }

    public void BuildAt()
    {
        if (activeBuilding is IBuildHandler buildHandler)
        {
            buildHandler.OnBuild();
        }
        AddChild(activeBuilding.Duplicate());
    }

    public void SetBuildingIntent(Building building)
    {
        activeBuilding = InstantiateBuilding(building);
    }

    public void ClearBuildingIntent()
    {
        activeBuilding.QueueFree();
        activeBuilding = null;
    }
}
