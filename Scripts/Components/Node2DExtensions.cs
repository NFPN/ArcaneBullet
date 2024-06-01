using Godot;
using System;

namespace Arcanum.Utils
{
    public static class Node2DExtensions
    {
        public static async void DelayedSignalEmission(this Node2D node, string signal, Variant[] args, int delayMs)
        {
            var timer = new Timer();
            node.AddChild(timer);
            timer.OneShot = true;
            timer.WaitTime = delayMs / 1000.0f;
            timer.Start();

            await node.ToSignal(timer, "timeout");

            node.EmitSignal(signal, args);

            timer.QueueFree();
            GD.Print($"Delayed signal finished: {signal}");
        }
    }
}