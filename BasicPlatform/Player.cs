using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D _animatedSprite2D;
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Lógica de gravedad
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "", "");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		Velocity = velocity;
		MoveAndSlide();
		
	//Logica de animación
	if (!IsOnFloor())
		{
			_animatedSprite2D.Play("jump");
		}else if (direction != Vector2.Zero){
			_animatedSprite2D.Play("moving");
		}else{
			_animatedSprite2D.Play("idle");
		}
		
		if(direction.X != 0){
			_animatedSprite2D.FlipH = direction.X < 0;
		}
		
		
	}
	public override void _Ready(){
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
}
