using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Pinchos : Area2D
{
	[Export] public int Damage = 1;
	private AnimatedSprite2D _animatedSprite2D;
	private Timer _damageTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		_damageTimer = GetNode<Timer>("DamageTimer");
		//Esto hace que se llame a la función cuando un cuerpo entra en 
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
		_damageTimer.Timeout += OnDamageTimerTimeout;
		//Activamos las animaciones
		ActivarTrampa();
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body is IDamageble )
		{
			//Llamamos a la función para aplicarle el daño
			ApplyDamage();
			//Activamos el timer si está parado
			if (_damageTimer.IsStopped())
			{
				_damageTimer.Start();
			}
		}
		
	}

	private void OnBodyExited(Node2D body)
	{
		//Si no quedan algo a lo que hacer daño dentro del area , paramos el timer 
		if(GetOverlappingBodies().Count == 0)
		{
			_damageTimer.Stop();
		}
	}

	private void OnDamageTimerTimeout()
    {
        //Cada vez que se reinicie el timer hacemos daño
        ApplyDamage();
    }

	private void ApplyDamage()
	{
		var bodies = GetOverlappingBodies();
		foreach (Node2D body in bodies)
		{
			if (body is IDamageble target)
			{
				target.TakeDamage(Damage);	
			}
		}
	}
	public void ActivarTrampa()
    {
        // 1. Animamos la capa de adelante
        Node2D capaAlante = GetNode<Node2D>("CapaDelantera");
        foreach (Node hijo in capaAlante.GetChildren())
        {
            if (hijo is AnimatedSprite2D sprite)
            {
                sprite.Play("Layer1");
            }
        }

        // 2. Animamos la capa de atrás
        Node2D capaAtras = GetNode<Node2D>("CapaTrasera");
        foreach (Node hijo in capaAtras.GetChildren())
        {
            if (hijo is AnimatedSprite2D sprite)
            {
                sprite.Play("Layer2");
            }
        }
    }

}
