using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : NetworkBehaviour
{
    [Networked] public int vida { get; set; } = 20;
    [SerializeField] private float velocidade = 2f;
    [SerializeField] private int valor = 10;

    private Rigidbody2D rb;
    private Transform checkpoint;

    [Networked]
    public int index { get; set; } = 0;
    [NonSerialized] public float distancia = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Spawned()
    {
        if (inimigoManager.main != null && inimigoManager.main.spawnpoint != null)
        {
            transform.position = inimigoManager.main.spawnpoint.position;
        }
        AtualizarCheckpoint();
    }

    void Update()
    {
        if (inimigoManager.main == null) return;

        AtualizarCheckpoint();

        if (checkpoint == null) return;

        distancia = Vector2.Distance(transform.position, checkpoint.position);

        if (Object != null && Object.HasStateAuthority)
        {
            if (distancia <= 0.1f)
            {
                index++;
                if (index >= inimigoManager.main.checkpoints.Length)
                {
                    if (jogador.main != null) jogador.main.ReceberDano(vida);
                    Runner.Despawn(Object);
                    return;
                }
            }
        }
    }

    void FixedUpdate()
    {
        AtualizarCheckpoint();

        if (checkpoint != null)
        {
            Vector2 direction = (checkpoint.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0f, 0f, angle);

            float rotationSpeed = velocidade * 3f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);

            if (Object != null)
            {
                rb.velocity = direction * velocidade;
            }
        }
    }

    private void AtualizarCheckpoint()
    {
        if (inimigoManager.main != null && index < inimigoManager.main.checkpoints.Length)
        {
            checkpoint = inimigoManager.main.checkpoints[index];
        }
    }

    public void ReceberDano(int dano)
    {
        if (Object == null || !Object.HasStateAuthority) return;

        vida = vida - dano;

        if (vida <= 0)
        {

            if (jogador.main != null) jogador.main.creditos += valor;
            Runner.Despawn(Object);
        }
    }
}