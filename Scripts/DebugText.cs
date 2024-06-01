using Arcanum.Components;
using Godot;
using System;
using System.Linq;
using System.Text;

public partial class DebugText : TextEdit
{
	[Export] Node2D Player;
	private HealthComponent playerHealth;
	private int currentPlayerHealth = 0;


	private StringBuilder debugText = new();

	public override void _Ready()
	{
		StartDebugger();
	}

	public void StartDebugger()
	{
		playerHealth = Player.GetNode<HealthComponent>("HealthComponent");
		playerHealth.Connect(nameof(playerHealth.HealthChanged), new Callable(this, nameof(HealthChanged)));
	}

	public override void _Process(double delta)
	{
		debugText.AppendLine($"DEBUG - {DateTime.UtcNow}")
		.AppendLine(currentPlayerHealth.ToString());

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
