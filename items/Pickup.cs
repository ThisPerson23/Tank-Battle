using Godot;
using System;

public class Pickup : Area2D
{
    public enum PickupType
    {
        Health, 
        Ammo
    }

    // Export Variable/Properties
    [Export] public PickupType Type { get; set; } = PickupType.Health;
    [Export] public Vector2 Amount { get; set; } = new Vector2(10, 25);

    // Global Variables
    private Texture[] _iconTextures = {ResourceLoader.Load("res://assets/effects/wrench_white.png") as Texture,
                                       ResourceLoader.Load("res://assets/effects/ammo_machinegun.png") as Texture};

    public override void _Ready()
    {
        GetNode<Sprite>("Icon").Texture = _iconTextures[(int)this.Type];
    }

    private void OnPickupBodyEntered(PhysicsBody2D body)
    {
        var tank = body as Tank;

        switch (this.Type)
        {
            case PickupType.Health:

                if (tank.HasMethod("Heal"))
                    tank.Heal((int)GD.RandRange(Amount.x, Amount.y));

                break;
            case PickupType.Ammo:

                if (tank.HasMethod("SetAmmo"))
                    tank.SetAmmo((int)GD.RandRange(Amount.x, Amount.y));

                break;
            default:
                // Do nothing.
                break;
        }

        QueueFree();
    }
}
