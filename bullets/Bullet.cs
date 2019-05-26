using Godot;
using System;

public class Bullet : Area2D
{
    // Export Variables/Properties
    [Export] public int Speed { get; set; }
    [Export] public int Damage { get; set; }
    [Export] public float Lifetime { get; set; }

    // Properties
    public Vector2 Velocity { get; set; }

    // Start Function
    // Initialize Object
    public virtual void Start(Vector2 position, Vector2 direction, PhysicsBody2D target = null)
    {
        this.Position = position;
        this.Rotation = direction.Angle();

        GetNode<Timer>("Lifetime").WaitTime = this.Lifetime;
        this.Velocity = direction * this.Speed;

        GetNode<Timer>("Lifetime").Start();
    }

    // Process Function
    // Update Object
    public override void _Process(float delta)
    {
        this.Position += this.Velocity * delta;
    }

    // Destroy the bullet
    private void Explode()
    {
        // Disable _Process - Stop Moving
        SetProcess(false);

        GetNode<Sprite>("Sprite").Hide();
        GetNode<AnimatedSprite>("Explosion").Show();
        GetNode<AnimatedSprite>("Explosion").Play("smoke");
    }

    private void OnExplosionAnimationFinished()
    {
        QueueFree();
    }

    private void OnBulletBodyEntered(PhysicsBody2D body)
    {
        Explode();

        if (body.HasMethod("TakeDamage"))
        {
            var tank = body as Tank;
            tank.TakeDamage(this.Damage);
        }
    }

    // Explode bullet when timer runs out
    private void OnLifetimeTimeout()
    {
        Explode();
    }
}
