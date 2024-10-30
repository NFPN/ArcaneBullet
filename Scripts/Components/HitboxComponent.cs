using Godot;
using System;

namespace Arcanum.Components
{
    public partial class HitboxComponent : CollisionShape2D
    {
        [Export] public HealthComponent healthComponent;
        [Export] public int Damage { get; set; } = 10;

        private Callable thisCallable;

        public override void _Ready()
        {
            thisCallable = new Callable(this, nameof(OnBodyCollision));
            //need to connect to something that makes sense, otherwise this wont work
            //Connect("hitbox_collision", thisCallable);
        }

        private void OnBodyCollision(Node body)
        {
            if (body is HitboxComponent damageHitbox)
            {
                GD.Print(body.Name);
                healthComponent.TakeDamage(damageHitbox.Damage);
            }
        }
    }
}