using Godot;
using System;

public class Map : Node2D
{
    // Signals
    [Signal] public delegate void NextLevel();
    [Signal] public delegate void Restart();

    // Global Variables
    public int CurrentLevel = 1;

    // Ready
    // Set camera limit
    public override void _Ready()
    {
        SetCameraLimits();

        // Set custom cursor
        Input.SetCustomMouseCursor(ResourceLoader.Load("res://assets/UI/crossair_blackOutline.png"), Input.CursorShape.Arrow, new Vector2(16, 16));

        GetNode<Tank>("PlayerTank")._map = GetNode<TileMap>("Ground");
    }

    // Set the Player Camera's Limit to the Map Size
    private void SetCameraLimits()
    {
        Rect2 mapLimits = GetNode<TileMap>("Ground").GetUsedRect();
        Vector2 mapCellSize = GetNode<TileMap>("Ground").CellSize;

        GetNode<Camera2D>("PlayerTank/Camera").LimitLeft = (int)(mapLimits.Position.x * mapCellSize.x);
        GetNode<Camera2D>("PlayerTank/Camera").LimitRight = (int)(mapLimits.End.x * mapCellSize.x);
        GetNode<Camera2D>("PlayerTank/Camera").LimitTop = (int)(mapLimits.Position.y * mapCellSize.y);
        GetNode<Camera2D>("PlayerTank/Camera").LimitBottom = (int)(mapLimits.End.y * mapCellSize.y);
    }

    // Spawn a new bullet
    private void OnTankShoot(PackedScene bullet, Vector2 position, Vector2 direction, PhysicsBody2D target = null)
    {
        var b = bullet.Instance() as Bullet;
        AddChild(b);
        b.Start(position, direction, target);
    }
    
    private void OnPlayerDead()
    {
        EmitSignal("Restart");
    }

    private void OnRestart()
    {
        GetTree().ChangeScene(Globals.Levels[0]);
    }

    private void OnNextLevel()
    {
        GetTree().ChangeScene(Globals.Levels[--CurrentLevel]);
    }
}
