using Godot;
using System;
using Arcanum.Utils;

namespace Arcanum.Components
{
    public partial class HealthComponent : Node2D
    {
        [Signal]
        public delegate void HealthChangedEventHandler(int currentHealth);
        [Signal]
        public delegate void DiedEventHandler();


        [Export]
        public int MaxHealth { get; private set; } = 100;

        private int currentHealth;

        public override void _Ready()
        {
            currentHealth = MaxHealth;
            this.DelayedSignalEmission(SignalName.HealthChanged, new[] { Variant.From(currentHealth) }, 100);
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            EmitSignal(SignalName.HealthChanged, currentHealth);
            if (currentHealth <= 0)
            {
                GD.Print("Player is dead");
                EmitSignal(SignalName.Died);
            }
        }

        public int GetHealth() => currentHealth;

        public void Heal(int amount)
        {
            currentHealth = Math.Min(currentHealth + amount, MaxHealth);
        }
    }
}