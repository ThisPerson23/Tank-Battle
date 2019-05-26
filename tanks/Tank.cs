using Godot;
using System;

public class Tank : KinematicBody2D
{
    // Signals
    [Signal] public delegate void Shoot();
    [Signal] public delegate void Dead();
    [Signal] public delegate void HealthChanged();
    [Signal] public delegate void AmmoChanged();

    // Export Variables/Properties
    [Export] public PackedScene Bullet { get; set; }
    [Export] public int MaxSpeed { get; set; }
    [Export] public float RotationSpeed { get; set; }
    [Export] public float GunCooldown { get; set; }
    [Export] public int MaxHealth { get; set; }
    [Export] public int GunShots { get; set; }
    [Export] public float GunSpread { get; set; } = 0.2f;
    [Export] public int MaxAmmo { get; set; } = 20;
    [Export] public int Ammo { get; set; } = -1;
    [Export] public float OffroadFriction { get; set; } = 0.75f;


    // Properties
    public Vector2 Velocity { get; set; } = new Vector2();

    // Global Variables
    private bool _canShoot = true;
    private bool _alive = true;
    private int _health;
    public TileMap _map;

    // Ready
    public override void _Ready()
    {
        _health = this.MaxHealth;
        GetNode<Particles2D>("Smoke").Emitting = false;
        EmitSignal("HealthChanged", _health * 100 / this.MaxHealth);
        SetAmmo(this.Ammo);
        GetNode<Timer>("GunTimer").WaitTime = this.GunCooldown;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        EmitSignal("HealthChanged", _health * 100 / this.MaxHealth);

        if (_health <= this.MaxHealth / 2)
            GetNode<Particles2D>("Smoke").Emitting = true;

        if (_health <= 0)
            Explode();
    }

    public void Heal(int amount)
    {
        _health += amount;
        _health = Mathf.Clamp(_health, 0, this.MaxHealth);
        EmitSignal("HealthChanged", _health * 100 / this.MaxHealth);

        if (_health > this.MaxHealth / 2)
            GetNode<Particles2D>("Smoke").Emitting = false;
    }

    public void SetAmmo(int value)
    {
        if (value > this.MaxAmmo)
            value = this.MaxAmmo;

        this.Ammo = value;
        EmitSignal("AmmoChanged", this.Ammo * 100 / this.MaxAmmo);
    }

    public void Explode()
    {
        GetNode<CollisionShape2D>("Hitbox").Disabled = true;
        _alive = false;

        GetNode<Sprite>("Turret").Hide();
        GetNode<Sprite>("Body").Hide();

        GetNode<AnimatedSprite>("Explosion").Show();
        GetNode<AnimatedSprite>("Explosion").Play();
    }
    
    // To Control the Tank
    public virtual void Control(float delta)
    {
        return;
    }
    
    // Physics Process
    public override void _PhysicsProcess(float delta)
    {
        if (!_alive)
            return;

        Control(delta);

        // Check if player is on a slow terrain tile
        // If so, slow them down
        if (_map != null)
        {
            int tile = _map.GetCellv(_map.WorldToMap(this.Position));

            foreach (int v in Globals.SlowTerrain)
            {
                if (tile == v)
                    Velocity  *= this.OffroadFriction;
            }
        }
         
        MoveAndSlide(this.Velocity);
    }

    // Called when a tank shoots
    // Emits the Shoot signal for the Map to listen to
    public void Fire(int num, float spread, PhysicsBody2D target = null)
    {
        if (_canShoot && this.Ammo != 0)
        {
            SetAmmo(this.Ammo - 1);
            _canShoot = false;
            GetNode<Timer>("GunTimer").Start();

            Vector2 dir = new Vector2(1, 0).Rotated(GetNode<Sprite>("Turret").GlobalRotation);

            if (num > 1)
            {
                for (int i = 0; i < num; i++)
                {
                    float angle = -spread * i * (2 * spread) / (num - 1);
                    EmitSignal("Shoot", this.Bullet, GetNode<Position2D>("Turret/Muzzle").GlobalPosition, dir.Rotated(angle), target);
                }
            }
            else
                EmitSignal("Shoot", this.Bullet, GetNode<Position2D>("Turret/Muzzle").GlobalPosition, dir, target);

            // Play muzzle flash
            GetNode<AnimationPlayer>("Anim").Play("muzzle_flash");
        }
    }

    // Reset _canShoot
    private void OnGunTimerTimeout()
    {
        _canShoot = true;
    }

    private void OnExplosionAnimationFinished()
    {
        QueueFree();
        EmitSignal("Dead");
    }
}
