using Godot;

namespace Arcanum
{
    public partial class Chrosshair : Sprite2D
    {
        private Camera2D camera;
        private Vector2 camSize;
        private Vector2 mousePos;
        private Vector2I boundary;
        private bool isMouseWithinWindow;

        public override void _Ready()
        {
            boundary = GetViewport().GetWindow().Size;
            GD.Print(boundary);
        }

        public override void _Process(double delta)
        {
            mousePos = GetGlobalMousePosition().Round();
            isMouseWithinWindow = mousePos.X >= 0 && mousePos.X <= boundary.X &&
                                  mousePos.Y >= 0 && mousePos.Y <= boundary.Y;

            if (isMouseWithinWindow)
            {
                GlobalPosition = mousePos;
                HandleMouseMode(Input.MouseModeEnum.Hidden);
                return;
            }

            HandleMouseMode(Input.MouseModeEnum.Visible);
        }

        private void HandleMouseMode(Input.MouseModeEnum mouseMode)
        {
            if (Input.MouseMode != mouseMode)
            {
                Visible = isMouseWithinWindow;
                Input.MouseMode = mouseMode;
            }
        }
    }
}
