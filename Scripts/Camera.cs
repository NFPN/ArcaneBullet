using Arcanum.Components;
using Godot;

namespace Arcanum
{
    public partial class Camera : Camera2D
    {
        [Export] public float PlayerWeight { get; set; } = 1;
        [Export] public float MouseWeight { get; set; } = 0.1f;
        [Export] public float OffsetX { get; set; } = 60;

        private Node2D playerNode;
        private Node2D chrosshairNode;
        private DashComponent dashComponent;

        private const float zoomOut = 2.3f;
        private const float zoomIn = 2.5f;
        private const float zoomSpeed = 0.01f;
        private bool shouldZoomOut;

        public override void _Ready()
        {
            playerNode = Owner.GetNode<Node2D>("%Player");
            chrosshairNode = Owner.GetNode<Node2D>("%Chrosshair/Sprite2D");
            dashComponent = playerNode.GetNode<DashComponent>("%DashComponent");
        }

        public override void _Process(double delta)
        {
            if (playerNode == null || chrosshairNode == null || dashComponent == null)
            {
                return;
            }

            var targetPos = (playerNode.GlobalPosition * PlayerWeight)
                          + (chrosshairNode.GlobalPosition * MouseWeight);

            // Apply smoothing to the camera movement
            var currentPos = GlobalPosition;
            var newPos = currentPos.Lerp(targetPos, 1);
            newPos.X = Mathf.Round(newPos.X - OffsetX);
            newPos.Y = Mathf.Round(newPos.Y);

            GlobalPosition = newPos;

            shouldZoomOut = dashComponent.IsDashing;

            if (shouldZoomOut && Zoom.X > zoomOut)
                Zoom = Zoom.Lerp(new Vector2(zoomOut, zoomOut), zoomSpeed);
            else
                Zoom = Zoom.Lerp(new Vector2(zoomIn, zoomIn), 0.01f);
        }
    }
}
