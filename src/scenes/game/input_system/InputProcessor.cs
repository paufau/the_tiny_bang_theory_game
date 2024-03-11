using System;
using Godot;

namespace Game.InputSystem
{
	public class InputProcessor
	{
		public delegate void MouseClickDelegate(InputEventMouseButton click);

		public static bool isUIHovered = false;
		public static void OnGameClick(InputEvent @event, MouseClickDelegate mouseClickDelegate)
		{
			if (isUIHovered) return;
			if (@event is InputEventMouseButton click)
			{
				mouseClickDelegate.Invoke(click);
			}
		}

		private Action onPress;
		private Button button;

		public InputProcessor(Button button, Action onPress)
		{
			this.button = button;
			this.onPress = onPress;

			button.Pressed += onPress;
			button.MouseEntered += HandleHover;
			button.MouseExited += HandleUnhover;
		}

        public void Dispose()
        {
			button.Pressed -= onPress;
			button.MouseEntered -= HandleHover;
			button.MouseExited -= HandleUnhover;
        }

		public void HandleHover()
		{
            isUIHovered = true;
		}

		public void HandleUnhover()
		{
            isUIHovered = false;
		}
    }
}

