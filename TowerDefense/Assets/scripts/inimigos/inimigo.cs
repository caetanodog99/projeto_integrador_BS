using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class inimigo : MonoBehaviour
{
    [SerializeField] public int vida = 2020;
    [SerializeField] private float movespeed = 2f;
    [SerializeField] private int valor = 10;

    private Rigidbody2D rb;

    private Transform checkpoint;

    [NonSerialized] public int index = 0;
    [NonSerialized] public float distancia = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        checkpoint = inimigoManager.main.checkpoints[index];
    }

    void Update()
    {
        checkpoint = inimigoManager.main.checkpoints[index];
        distancia = Vector2.Distance(transform.position, inimigoManager.main.checkpoints[index].transform.position);
        if (Vector2.Distance(checkpoint.transform.position, transform.position) <= 0.1f)
        {
            index++;
            if (index >= inimigoManager.main.checkpoints.Length)
            {
                jogador.main.ReceberDano(vida);
                Destroy(gameObject);

            }
        }

        
    }


    void FixedUpdate()
    {
        Vector2 direction = (checkpoint.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0f, 0f, angle);

        float rotationSpeed = movespeed * 3f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);

        rb.velocity = direction * movespeed;
    }

    public void ReceberDano(int dano)
    {

        vida = vida-dano;

        if (vida <= 0)
        {
            jogador.main.creditos += valor;
            Destroy(gameObject);
        }
    }
}
