using Game.State;
using Godot;

public partial class ResourceUI : HBoxContainer
{
    public void Configure(AbstractStore<int> store)
    {
        this.store = store;
    }

    private AbstractStore<int> GetStore()
    {
        return store;
    }

    private Label titleLabel;
    private Label countLabel;
    private AbstractStore<int> store;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        countLabel = GetNode<Label>("Count");
        titleLabel = GetNode<Label>("Title");

        titleLabel.Text = store.GetTitle() + ": ";

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
