using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D _animatedSprite2D;

	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Lógica de gravedad
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Lógica de Salto
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Obtenemos la dirección (float entre -1 y 1)
		float direction = Input.GetAxis("ui_left", "ui_right");

		if (direction != 0)
		{
			velocity.X = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		// Lógica de animación
		ActualizarAnimaciones(direction);
	}

	private void ActualizarAnimaciones(float direction)
	{
		if (!IsOnFloor())
		{
			_animatedSprite2D.Play("jump");
		}
		else if (direction != 0)
		{
			_animatedSprite2D.Play("moving");
		}
		else
		{
			_animatedSprite2D.Play("idle");
		}
		
		// Voltear el sprite
		if (direction != 0)
		{
			_animatedSprite2D.FlipH = direction < 0;
		}
	}
}
