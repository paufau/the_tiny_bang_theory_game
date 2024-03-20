using Game.InputSystem;
using Game.State;
using Godot;

public partial class SelectionHelper : Node2D
{
    public delegate void SelectionResultHandler(Rect2 rectGlobalCoords);

    private SelectionResultHandler? selectionResultHandler;
    private Vector2? initialDraggingPosition = null;
    private TextureRect? selectionTexture;

    private bool isSelectionActive = false;

    public override void _Ready()
    {
        selectionTexture = GetNode<TextureRect>("SelectionTexture");
        StatesProvider.selectionHelper = this;
    }

    public override void _Process(double delta)
    {
        if (!isSelectionActive) return;

        var rect = GetSelectedRect();
        selectionTexture.SetPosition(rect.Position);
        selectionTexture.SetSize(rect.Size);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!isSelectionActive) return;

        if (@event.IsPressed() && initialDraggingPosition == null)
        {
            InputProcessor.OnGameClick(@event, (InputEventMouseButton mouseButton) =>
            {
                if (mouseButton.ButtonIndex == MouseButton.Left)
                {
                    initialDraggingPosition = GetGlobalMousePosition();
                }
            });
            return;
        };

        if (@event.IsPressed()) return;

        InputProcessor.OnGameClick(@event, (InputEventMouseButton mouseButton) =>
        {
            if (mouseButton.ButtonIndex == MouseButton.Right)
            {
                StopSelection();
            }
            else if (mouseButton.ButtonIndex == MouseButton.Left)
            {
                selectionResultHandler?.Invoke(GetSelectedRect());
                initialDraggingPosition = null;
            }
        });
    }

    private Rect2 GetSelectedRect()
    {
        var nav = StatesProvider.NavigatorState;

        Vector2 mousePosition = GetGlobalMousePosition();
        Vector2 initialPosition = mousePosition;

        Vector2 tileSizeX = new Vector2(nav.tileMap.TileSet.TileSize.X, 0);
        Vector2 tileSizeY = new Vector2(0, nav.tileMap.TileSet.TileSize.Y);

        if (initialDraggingPosition != null)
        {
            initialPosition = (Vector2)initialDraggingPosition;
        }

        initialPosition -= tileSizeY;

        Vector2 fixedMousePosition = mousePosition - tileSizeY;

        if (mousePosition.X > initialPosition.X)
        {
            fixedMousePosition += tileSizeX;
        }

        if (mousePosition.Y > initialPosition.Y)
        {
            fixedMousePosition += tileSizeY;
        }

        Rect2 rect = new(nav.SnapToNearestTileOrigin(initialPosition), nav.tileMap.TileSet.TileSize);
        rect = rect.Expand(nav.SnapToNearestTileOrigin(fixedMousePosition));

        return rect;
    }

    public void StartSelection(SelectionResultHandler selectionResultHandler)
    {
        isSelectionActive = true;
        this.selectionResultHandler = selectionResultHandler;
        selectionTexture.Show();
    }

    public void StopSelection()
    {
        selectionTexture.Hide();
        isSelectionActive = false;
        selectionResultHandler = null;
    }
}
