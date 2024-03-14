using Game.InputSystem;
using Game.State;
using Godot;
using System;
using System.Collections.Generic;

public partial class Alert : CanvasLayer
{
    [Export]
    private Label label;

    private Queue<string> texts = new();

    public void ShowAlert(string text)
    {
        Pause();
        StatesProvider.buildingUI.Hide();
        label.Text = text;
        Visible = true;
    }

    public void ShowAlert(List<string> multipleStrings)
    {
        texts = new(multipleStrings);

        if (texts.Count == 0) return;
        ShowAlert(texts.Dequeue());
    }

    public void HideAlert()
    {
        Visible = false;
        label.Text = "";
        GetTree().Paused = false;
        StatesProvider.buildingUI.Show();
    }

    public void Pause()
    {
        GetTree().Paused = true;
    }

    private void Next()
    {
        if (texts.Count == 0)
        {
            HideAlert();
            return;
        }

        label.Text = texts.Dequeue();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed()) return;

        if (@event is InputEventKey keyEvent)
        {
            if (keyEvent.Keycode == Key.Space)
            {
                Next();
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StatesProvider.alert = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
