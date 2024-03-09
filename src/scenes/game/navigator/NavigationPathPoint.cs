using Godot;

public class NavigationPathPoint
{
    public long id;
    public Vector2 position;
    public bool isFloating = false;

    public NavigationPathPoint(long id, Vector2 position)
    {
        this.id = id;
        this.position = position;
    }
}


