using Game.State;
using Godot;

public partial class Fuel : HBoxContainer
{
    private AbstractStore<int> GetStore()
    {
        return StatesProvider.Fuel;
    }

    private Label countLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        countLabel = GetNode<Label>("Count");
        GetStore().OnUpdate += UpdateCount;
        UpdateCount(GetStore().value);
    }

    public override void _ExitTree()
    {
        GetStore().OnUpdate -= UpdateCount;
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
