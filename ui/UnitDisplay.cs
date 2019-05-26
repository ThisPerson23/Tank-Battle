using Godot;
using System;

public class UnitDisplay : Node2D
{
    Texture barGreen = ResourceLoader.Load("res://assets/UI/barHorizontal_green_mid 200.png") as Texture;
    Texture barYellow = ResourceLoader.Load("res://assets/UI/barHorizontal_yellow_mid 200.png") as Texture;
    Texture barRed = ResourceLoader.Load("res://assets/UI/barHorizontal_red_mid 200.png") as Texture;

    public override void _Ready()
    {
        // Hide initially
        Hide();
    }

    public override void _Process(float delta)
    {
        this.GlobalRotation = 0f;
    }

    public void UpdateHealthbar(int value)
    {
        GetNode<TextureProgress>("HealthBar").TextureProgress_ = barGreen;

        if (value <= 60)
            GetNode<TextureProgress>("HealthBar").TextureProgress_ = barYellow;
        if (value <= 25)
            GetNode<TextureProgress>("HealthBar").TextureProgress_ = barRed;
        if (value < 100)
            Show();

        GetNode<TextureProgress>("HealthBar").Value = value;
    }
}
