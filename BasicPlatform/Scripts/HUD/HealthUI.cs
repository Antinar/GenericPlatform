using Godot;
using System;
using System.Collections.Generic;

public partial class HealthUI : HBoxContainer
{
    //We manage every heart like a diferent "scene" for their animations
    [Export] public PackedScene HeartScene;
    [Export] public int MaxHealth = 3;//In the future we will change this in case we want to put objects that can modify the maxhealth of the player

    private List<AnimatedSprite2D> _heartSprites = new List<AnimatedSprite2D>();

    public override void _Ready()
    {
        //With this we will conect with the object player so they can exchange signals
        var player = GetTree().Root.FindChild("Player", true, false) as Player;

        if (player != null) player.HealthChanged += OnPlayerHealthChange;

        for (int i = 0; i < MaxHealth; i++)
        {
            Control heartNode = (Control)HeartScene.Instantiate();
            AddChild(heartNode);
            //Use of a var so we don't have to seach the sprite every time and put it in the list
            var sprite = heartNode.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            _heartSprites.Add(sprite);

            sprite.Play("idleHeart");
        }
    }


    //When the player have a change in his life we will call this function
    public void OnPlayerHealthChange(int currentHealth)
    {

        GD.Print($"Señal recibida en UI. Vida actual: {currentHealth}");
        for (int i = 0; i < _heartSprites.Count; i++)
        {

            if (i < currentHealth)
            {
                _heartSprites[i].Play("idleHeart");
            }
            else
            {
                _heartSprites[i].Play("damageHeart");
            }
        }


    }
}
