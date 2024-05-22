using Godot;
using System;

namespace Arcanum.Skills
{
    public partial class Dash : Skill
    {
        [Export] public float DashSpeed { get; internal set; } = 1500;
        [Export] public float DashDuration = 0.2f;

        private bool canDash;
        private bool isDashing;
        private double dashTimer;

        private Callable thisCallable;

        public override void Activate() { }

        //TODO: still wip, gotta bring some stuff form player.cs
        public override void Activate(SkillParameter[] parameters)
        {
            if (parameters.Length == 0 || parameters.Length > 1)
                return;

            var dashParam = parameters[0];

            if (!isDashing)
            {
                isDashing = true;
                OnActivate();
                var timer = new Timer
                {
                    OneShot = true,
                    WaitTime = DashDuration
                };

                thisCallable = new Callable(this, nameof(OnDashComplete));

                timer.Connect("timeout", thisCallable);
                AddChild(timer);
                timer.Start();
            }
        }

        private void OnDashComplete()
        {
            isDashing = false;
            OnDeactivate();
        }

        public override void _PhysicsProcess(double delta)
        {

            dashTimer += delta;

            //hardcoded dash time, could it be changed it with animation instead? or inherited
            if (dashTimer > .1)
                isDashing = false;

            if (dashTimer > .5)
                canDash = true;
        }
    }
}