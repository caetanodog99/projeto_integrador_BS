using Fusion;
using System;
using UnityEngine;

public class inimigo : NetworkBehaviour
{
    [Networked] public int vida { get; set; } = 20;
    [Networked] private float movespeed { get; set; } = 2f;
    [Networked] private int valor { get; set; } = 10;


    [Networked] public int index { get; set; } = 0;

    private Rigidbody2D rb;
    private Transform checkpoint;

    [NonSerialized] public float distancia = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public override void Spawned()
    {
        if (inimigoManager.main != null && inimigoManager.main.checkpoints.Length > 0)
        {
            checkpoint = inimigoManager.main.checkpoints[index];
        }
    }

    public override void FixedUpdateNetwork()
    {

        if (Object.HasStateAuthority)
        {
            MoverInimigo();
        }
    }

    private void MoverInimigo()
    {
        if (index >= inimigoManager.main.checkpoints.Length) return;

        checkpoint = inimigoManager.main.checkpoints[index];
        distancia = Vector2.Distance(transform.position, checkpoint.position);


        if (distancia <= 0.2f)
        {
            index++;


            if (index >= inimigoManager.main.checkpoints.Length)
            {

                if (jogador.main != null) jogador.main.ReceberDano(vida);


                Runner.Despawn(Object);
                return;
            }

            checkpoint = inimigoManager.main.checkpoints[index];
        }


        Vector2 direction = (checkpoint.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, angle), movespeed * 5f);


        transform.position = Vector2.MoveTowards(transform.position, checkpoint.position, movespeed * Runner.DeltaTime);
    }


    public void ReceberDano(int dano)
    {
        if (Object.HasStateAuthority)
        {
            AplicarDano(dano);
        }
        else
        {
            RPC_SolicitarDano(dano);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SolicitarDano(int dano)
    {
        AplicarDano(dano);
    }

    private void AplicarDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            if (jogador.main != null) jogador.main.creditos += valor;
            Runner.Despawn(Object);
        }
    }
}