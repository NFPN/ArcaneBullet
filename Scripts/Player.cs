using Godot;
using System;

public partial class Player : CharacterBody2D
{

	[Export]
	public int Gravity { get; internal set; } = 3000;
	[Export]
	public int Speed { get; internal set; } = 350;
	[Export]
	public float JumpSpeed { get; internal set; } = 800;
	[Export]
	public float DashSpeed { get; internal set; } = 1500;

	private bool usedDoubleJump;
	private bool canDash;
	private bool isDashing;
	private double dashTimer;
	private double tapTimer;
	private bool isFalling;
	private bool isSprinting;
	private bool isCrouching;
	private float curDirection;
	private float lastDirection = 1;

	private DoubleTapDetection doubleTapDetection = new();
	private AnimatedSprite2D playerAnimationSprite;
	private Vector2 curVelocity;


	public override void _Ready()
		=> playerAnimationSprite = GetNode<AnimatedSprite2D>("PlayerAnimationSprite");

	public override void _Process(double delta)
	{
		if (IsOnFloor() && usedDoubleJump)
			usedDoubleJump = false;

		isFalling = Velocity.Y > 0;
	}

	public override void _PhysicsProcess(double delta)
	{
		curDirection = Input.GetAxis("left", "right");
		curVelocity = Velocity;

		dashTimer += delta;

		//hardcoded dash, could it be changed it with animation instead?
		if (dashTimer > .1)
			isDashing = false;

		if (dashTimer > .5)
			canDash = true;

		if (curVelocity.X == 0)
			playerAnimationSprite.FlipH = GetGlobalMousePosition().X < Position.X;


		if (curDirection != 0)
		{
			playerAnimationSprite.FlipH = curDirection < 0;
			lastDirection = curDirection;
		}

		if (curVelocity.Y < 650)
			curVelocity.Y += Gravity * (float)delta;

		if (!isDashing)
			curVelocity.X = curDirection * Speed;


		isSprinting = Input.IsActionPressed("sprint");
		isCrouching = Input.IsActionPressed("crouch");

		if (isSprinting && !isDashing)
			curVelocity.X = curDirection * Speed * 1.5f;

		if (Input.IsActionJustPressed("jump"))
			curVelocity.Y = Jump(curVelocity.Y);
		if (Input.IsActionJustPressed("dash"))
			curVelocity.X = Dash(curVelocity.X, curDirection);

		//TODO: This should be toggled in settings (off by default)
		// if (doubleTapDetection.HasDoubleTapped(Input.IsActionJustPressed("left") || Input.IsActionJustPressed("right")))
		// 	curVelocity.X = Dash(curVelocity.X, direction);

		Velocity = curVelocity;

		UpdateAnimationState();
		MoveAndSlide();
	}

	private void UpdateAnimationState()
	{
		if (isDashing)
			playerAnimationSprite.Play("dash");
		else if (isFalling)
			playerAnimationSprite.Play("falling");
		else if (Velocity.X != 0 && !isFalling)
		{
			if (isSprinting)
				playerAnimationSprite.Play("sprint");
			else
				playerAnimationSprite.Play("run");
		}
		else if (playerAnimationSprite.Animation != "crouch" && isCrouching)
			playerAnimationSprite.Play("crouch");
		else if (playerAnimationSprite.Animation != "idle" && !isFalling && !isCrouching)
			playerAnimationSprite.Play("idle");
	}

	private float Dash(float curVelocity, float direction)
	{
		if (canDash)
		{
			isDashing = true;
			canDash = false;
			//UpdateAnimationState();
			dashTimer = 0;
			return lastDirection * DashSpeed;
		}
		return curVelocity;
	}

	private float Jump(float curVelocity)
	{
		if (IsOnFloor())
			return -JumpSpeed;

		if (!usedDoubleJump && !IsOnFloor())
		{
			usedDoubleJump = true;
			return -JumpSpeed;
		}
		return curVelocity;
	}
}
