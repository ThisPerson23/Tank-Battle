using Godot;
using System;

public class Missile : Bullet
{
    // Export Variables/Properties
    [Export] public float SteerForce { get; set; }

    // Global Variables
    private Vector2 _acceleration = new Vector2();
    private PhysicsBody2D _target = null;

    // Initialize the missile
    public override void Start(Vector2 position, Vector2 direction, PhysicsBody2D target = null)
    {
        base.Start(position, direction);

        _target = target;
    }

    // Move missile towards target
    public override void _Process(float delta)
    {
        if (_target != null)
        {
            _acceleration += Seek();
            this.Velocity += _acceleration * delta;
            this.Velocity = this.Velocity.Clamped(this.Speed);
            this.Rotation = this.Velocity.Angle();
        }

        base._Process(delta);
    }

    // Find the target
    private Vector2 Seek()
    {
        Vector2 desired = (_target.Position - this.Position).Normalized() * this.Speed;
        Vector2 steer = (desired - this.Velocity).Normalized() * this.SteerForce;

        return steer;
    }
}
