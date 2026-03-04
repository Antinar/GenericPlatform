using Godot;
using System;

public partial class Player : CharacterBody2D
{
	//Constantes
	//Constanstes de movimiento
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public const int NJumps = 2;
	//Variables privadas 
	//Variables de Animaciones
	private AnimatedSprite2D _animatedSprite2D;
	//Variables de movimiento
	private int _jumpsleft;
	//Variables de vida del jugador
	[Export] public int PlayerMaxHealth = 3;
	[Export] public float DamageCooldown = 1.0f;
	private int _playerHealth;
	private bool _canTakeDamage = true;

	public override void _Ready()
	{
		_playerHealth = PlayerMaxHealth;
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
		else
		{
			_jumpsleft = NJumps;
		}

		// Lógica de Salto
		if (Input.IsActionJustPressed("ui_accept") && (IsOnFloor() || _jumpsleft > 0))
		{

			velocity.Y = JumpVelocity;
			_jumpsleft--;

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
		AnimacionesMovimiento(direction);
	}

	private void AnimacionesMovimiento(float direction)
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

	private void AnimacionesVida(int opción)
	{
		//El jugador a sido dañado
		if (opción == 1)
		{
			_animatedSprite2D.Play("damaged");
		}//El jugador a muerto
		else if (opción == 2)
		{
			_animatedSprite2D.Play("Death");
		}
	}

	public void TakeDamage(int damage)
	{
		if (!_canTakeDamage) return;

		_playerHealth -= damage;
		_playerHealth = Mathf.Clamp(_playerHealth, 0, PlayerMaxHealth);

		if (_playerHealth <= 0)
		{
			Die();
		}
		else
		{
			Invulnerability();
		}
	}

	private async void Invulnerability()
	{
		_canTakeDamage = false;

		Modulate = new Color(1,1,1,0.5f);

		await ToSignal(GetTree().CreateTimer(DamageCooldown), SceneTreeTimer.SignalName.Timeout);;
		_canTakeDamage = true;
        Modulate = new Color(1, 1, 1, 1);
		AnimacionesVida(1);
	}
	private void Die()
	{	
		AnimacionesVida(2);
		//PlaceHolder (reinicia la escena)
		GetTree().ReloadCurrentScene();
	}
}
