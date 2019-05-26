using Godot;
using System;

public class HUD : CanvasLayer
{
    Texture barGreen = ResourceLoader.Load("res://assets/UI/barHorizontal_green_mid 200.png") as Texture;
    Texture barYellow = ResourceLoader.Load("res://assets/UI/barHorizontal_yellow_mid 200.png") as Texture;
    Texture barRed = ResourceLoader.Load("res://assets/UI/barHorizontal_red_mid 200.png") as Texture;

    private Texture _barTexture;

    public void UpdateHealthbar(int value)
    {
        _barTexture = barGreen;

        if (value <= 60)
            _barTexture = barYellow;
        if (value <= 25)
            _barTexture = barRed;

        GetNode<TextureProgress>("Margin/Container/HealthBar").TextureProgress_ = _barTexture;

        GetNode<Tween>("Margin/Container/HealthBar/Tween").InterpolateProperty(GetNode<TextureProgress>("Margin/Container/HealthBar"), "value",
                        GetNode<TextureProgress>("Margin/Container/HealthBar").Value, value, 0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        GetNode<Tween>("Margin/Container/HealthBar/Tween").Start();
        GetNode<AnimationPlayer>("Anim").Play("healthbar_flash");
    }

    public void UpdateAmmoGauge(int value)
    {
        GetNode<TextureProgress>("Margin/Container/AmmoGauge").Value = value;
    }

    private void OnAnimAnimationFinished(String animName)
    {
        if (animName == "healthbar_flash")
            GetNode<TextureProgress>("Margin/Container/HealthBar").TextureProgress_ = _barTexture;
    }
}
