using Arcanum.Components;
using Godot;
using System;
using System.Text;

namespace Arcanum
{
    public partial class DebugText : TextEdit
    {
        [Export] public Node2D Player { get; set; }
        private HealthComponent playerHealth;
        private int currentPlayerHealth = 0;

        [Export] public Camera Camera { get; set; }

        private DashComponent dashComponent;
        private StringBuilder debugText = new();

        public override void _Ready()
        {
            StartDebugger();
        }

        public void StartDebugger()
        {
            playerHealth = Player.GetNode<HealthComponent>("HealthComponent");
            playerHealth.Connect(nameof(playerHealth.HealthChanged), new Callable(this, nameof(HealthChanged)));
            dashComponent = Player.GetNode<DashComponent>("%DashComponent");
        }

        public override void _Process(double delta)
        {
            debugText.AppendLine($"DEBUG - {DateTime.UtcNow}")
            .AppendLine($"HP: {currentPlayerHealth.ToString()}")
            .AppendLine($"Cam: {Camera.Zoom.ToString()}")
            .AppendLine($"Dash: {dashComponent.IsDashing.ToString()}");

            Text = debugText.ToString();

            debugText.Clear();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey inputEventKey && inputEventKey.Keycode == Key.F && inputEventKey.IsPressed())
                playerHealth.TakeDamage(1);
        }

        private void HealthChanged(int currentHealth) => currentPlayerHealth = currentHealth;
    }
}
