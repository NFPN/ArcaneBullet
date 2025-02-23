using Arcanum.Components;
using Godot;
using System;

namespace Arcanum
{
    public partial class Player : CharacterBody2D
    {
        [Export]
        public int Gravity { get; set; } = 2800;

        [Export]
        public float JumpSpeed { get; set; } = 700;

        [Export]
        public float Speed { get; set; } = 200;

        private bool usedDoubleJump;
        private double tapTimer;
        private bool isFalling;
        private bool isSprinting;
        private bool isCrouching;
        private float curDirection;
        private float lastDirection = 1;

        private DashComponent dashComponent;
        private HitboxComponent hitboxComponent;
        private DoubleTapDetection doubleTapDetection = new();
        private AnimatedSprite2D playerAnimationSprite;
        private Vector2 curVelocity;
        private readonly float pushForce = 0.02f;

        public override void _Ready()
        {
            playerAnimationSprite = GetNode<AnimatedSprite2D>("PlayerAnimationSprite");
            dashComponent = GetNode<DashComponent>("DashComponent");
            hitboxComponent = GetNode<HitboxComponent>("HitboxComponent");

            Speed = 200;
        }

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

            if (curVelocity.X == 0)
            {
                playerAnimationSprite.FlipH = GetGlobalMousePosition().X < Position.X;
                lastDirection = playerAnimationSprite.FlipH ? -1 : 1;
            }

            if (curDirection != 0)
            {
                // hitboxComponent.Position = new Vector2(hitboxComponent.Position.Y, curDirection < 0 ? -1 : 1);
                playerAnimationSprite.FlipH = curDirection < 0;
                lastDirection = curDirection;
            }

            if (curVelocity.Y < 650)
                curVelocity.Y += Gravity * (float)delta;

            if (!dashComponent.IsDashing)
                curVelocity.X = curDirection * Speed;

            isSprinting = Input.IsActionPressed("sprint");
            isCrouching = Input.IsActionPressed("crouch");

            if (isSprinting && !dashComponent.IsDashing)
                curVelocity.X = curDirection * Speed * 1.5f;

            if (Input.IsActionJustPressed("jump"))
                curVelocity.Y = Jump(curVelocity.Y);
            if (Input.IsActionJustPressed("dash"))
                curVelocity.X += dashComponent.Dash((int)Math.Round(lastDirection));

            Velocity = curVelocity;

            MoveAndSlide();
            UpdateAnimationState();

            var collision = GetLastSlideCollision();

            if (collision != null && collision.GetCollider() is RigidBody2D rb)
            {
                rb.ApplyCentralImpulse(-collision.GetNormal() * pushForce);
            }
        }

        private void UpdateAnimationState()
        {
            if (dashComponent.IsDashing)
                playerAnimationSprite.Play("dash");
            if (isFalling)
                playerAnimationSprite.Play("falling");
            else if (Velocity.Abs().X != 0 && !isFalling)
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
}
