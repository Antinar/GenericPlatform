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
		//Esto hace que se llame a la función cuando un cuerpo entra en 
		BodyEntered += OnBodyEntered;
		//Activamos las animaciones
		ActivarTrampa();
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body is IDamageble damagebleTarget)
		{
			damagebleTarget.TakeDamage(Damage);
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
