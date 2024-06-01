using Godot;
using System;

namespace Arcanum.Components
{
    // [Tool]
    public partial class DashComponent : Node2D
    {
        [Export] public int DashSpeed { get; set; } = 1500;
        [Export] public double DashDuration { get; set; } = 0.1;
        [Export] public double DashRefreshTime { get; set; } = 0.5;

        public bool IsDashing { get { return dashTime < DashDuration; } }

        private int dashDirection;
        private double dashTime;
        private bool canDash;

        //TODO: not working for some reason
        public float Dash(int direction)
        {
            if (!canDash) return 0;

            dashTime = 0;
            canDash = false;
            dashDirection = direction;
            return dashDirection * DashSpeed;
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!canDash && dashTime > DashRefreshTime)
                canDash = true;

            if (dashTime < DashRefreshTime)
                dashTime += delta;
        }
    }
}