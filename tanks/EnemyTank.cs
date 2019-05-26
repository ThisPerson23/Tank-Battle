using Godot;
using System;

public class EnemyTank : Tank
{   
    // Export Variables/Properties
    [Export] public float TurretSpeed { get; set; }
    [Export] public int DetectRadius { get; set; }

    // Global Variables
    private float _speed = 0f;
    private PathFollow2D _parent; 
    private PhysicsBody2D _target;

    // Ready
    public override void _Ready()
    {   
        // Call base class _Ready first
        base._Ready();

        _parent = GetParent() as PathFollow2D;

        // Set the detect radius
        CircleShape2D circle = new CircleShape2D();
        circle.Radius = this.DetectRadius;
        GetNode<CollisionShape2D>("DetectRadius/CollisionShape2D").Shape = circle;
    }

    // Process
    // Update Object
    public override void _Process(float delta)
    {
        if (_target != null)
        {
            Vector2 targetDir = (_target.GlobalPosition - this.GlobalPosition).Normalized();
            Vector2 currentDir = new Vector2(1, 0).Rotated(GetNode<Sprite>("Turret").GlobalRotation);

            GetNode<Sprite>("Turret").GlobalRotation = currentDir.LinearInterpolate(targetDir, this.TurretSpeed * delta).Angle();

            if (targetDir.Dot(currentDir) > 0.9f)
                Fire(this.GunShots, this.GunSpread, _target);
        }
    }

    // Control Enemy Tank Automatically
    public override void Control(float delta)
    {
        if (GetNode<RayCast2D>("LookAhead").IsColliding() || GetNode<RayCast2D>("LookAhead2").IsColliding() || GetNode<RayCast2D>("LookAhead3").IsColliding())
            _speed = Mathf.Lerp(_speed, 0f, 0.1f);
        else
            _speed = Mathf.Lerp(_speed, this.MaxSpeed, 0.05f);

        // Make Enemy Tank follow its path
        if (_parent is PathFollow2D)
        {
            _parent.SetOffset(_parent.GetOffset() + _speed * delta);
            this.Position = new Vector2();
        }
        else
        {
            // Other Movement Code.
            return;
        }
    }

    // Set the target
    private void OnDetectRadiusBodyEntered(PhysicsBody2D body)
    {
        _target = body;
    }

    // Drop the target
    private void OnDetectRadiusBodyExited(PhysicsBody2D body)
    {
        if (body == _target)
            _target = null;
    }
}
