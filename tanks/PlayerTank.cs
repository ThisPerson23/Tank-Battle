using Godot;
using System;

public class PlayerTank : Tank
{ 
    // To Control the Player Tank
    public override void Control(float delta)
    {
        // Rotation.
        GetNode<Sprite>("Turret").LookAt(GetGlobalMousePosition());
        var rotationDir = 0f;

        if (Input.IsActionPressed("turn_right"))
            rotationDir += 1;
        if (Input.IsActionPressed("turn_left"))
            rotationDir -= 1;

        this.Rotation += this.RotationSpeed * rotationDir * delta;

        // Velocity.
        this.Velocity = new Vector2();

        if (Input.IsActionPressed("forward"))
            this.Velocity = new Vector2(this.MaxSpeed, 0).Rotated(this.Rotation);
        if (Input.IsActionPressed("back"))
            this.Velocity = new Vector2(-this.MaxSpeed / 2, 0).Rotated(this.Rotation);

        // Firing.
        if (Input.IsActionPressed("left_click"))
            Fire(this.GunShots, this.GunSpread);
    }
}
