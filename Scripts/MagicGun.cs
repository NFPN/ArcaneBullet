using Godot;

namespace Arcanum
{
    public partial class MagicGun : Node2D
    {
        [Export] public AnimatedSprite2D GunSprite { get; set; }

        private Vector2 mousePos = new();
        private float currentRot;

        public override void _Ready()
        {
            GunSprite ??= GetNode<AnimatedSprite2D>("GunSprite");
        }

        public override void _Process(double delta)
        {
            mousePos = GetGlobalMousePosition().Round();

            LookAt(mousePos);

            currentRot = Mathf.Abs(Mathf.RadToDeg(Rotation) % 360);

            if (currentRot > 90 && currentRot < 270 && !GunSprite.FlipV)
                GunSprite.FlipV = true;
            else if ((currentRot < 90 || currentRot > 270) && GunSprite.FlipV)
                GunSprite.FlipV = false;
        }
    }
}
