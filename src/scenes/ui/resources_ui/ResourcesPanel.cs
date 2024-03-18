using Game.State;
using Godot;
using System;
using System.Collections.Generic;

public partial class ResourcesPanel : VBoxContainer
{
    private List<AbstractStore<int>> resources = new()
    {
        StatesProvider.Fuel,
        StatesProvider.Rocks,
        StatesProvider.Meat,
        StatesProvider.CookedMeat,
    };

    private PackedScene resourceUIScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        resourceUIScene = ResourceLoader.Load<PackedScene>("res://src/scenes/ui/resources_ui/ResourceUI.tscn");

        foreach(var res in resources)
        {
            var resInstance = resourceUIScene.Instantiate();
            var resUI = (ResourceUI)resInstance;
            resUI.Configure(res);
            AddChild(resInstance);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
