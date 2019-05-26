using Godot;
using System;

public class TitleScreen : Control
{
    private int _nextLevel = 1;

    // Check spacebar press
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_select"))
            GetTree().ChangeScene(Globals.Levels[_nextLevel]);;
    }
}
