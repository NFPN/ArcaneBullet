using Godot;
using System;

public partial class Camera : Camera2D
{
	[Export] public float PlayerWeight = 1;
	[Export] public float MouseWeight = 0.1f;
	[Export] public float offsetX = 60;

	private Node2D playerNode;
	private Node2D chrosshairNode;


	private float minZoom = 1.1f;
	private float maxZoom = 1.5f;
	private float zoomDuration = 0.2f;
	private bool shouldZoom;

	public override void _Ready()
	{
		playerNode = GetNode<Node2D>("%Player");
		chrosshairNode = GetNode<Node2D>("%Chrosshair/Sprite2D");
	}

	public override void _Process(double delta)
	{
		if (playerNode == null || chrosshairNode == null)
			return;

		var targetPos = (playerNode.GlobalPosition * PlayerWeight)
					  + (chrosshairNode.GlobalPosition * MouseWeight);

		// Apply smoothing to the camera movement
		var currentPos = GlobalPosition;
		var newPos = currentPos.Lerp(targetPos, 1);
		newPos.X = Mathf.Round(newPos.X - offsetX);
		newPos.Y = Mathf.Round(newPos.Y);

		GlobalPosition = newPos;

		shouldZoom = Input.IsActionPressed("sprint");

		if (shouldZoom && Zoom.X < maxZoom)
			Zoom = Zoom.Lerp(new Vector2(minZoom, minZoom), 0.1f);
		else
			Zoom = Zoom.Lerp(new Vector2(maxZoom, maxZoom), 0.01f);

	}
}
