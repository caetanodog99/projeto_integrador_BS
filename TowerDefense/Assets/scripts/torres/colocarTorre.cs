using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class colocarTorre : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteArea;
    [SerializeField] private CircleCollider2D colliderArea;
    [SerializeField] private Color cinza;
    [SerializeField] private Color vermelho;


    [SerializeField] public bool colocando = true;
    private bool restrito;

    private float tempoUltimoToque = 0f;
    private float limiteTempoToqueDuplo = 0.25f;

    void Awake()
    {
        colliderArea.enabled = false;
    }

    void Update()
    {
       
        bool toqueDuploDetectado = false;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Time.time - tempoUltimoToque <= limiteTempoToqueDuplo)
            {
                toqueDuploDetectado = true;
            }
            tempoUltimoToque = Time.time;
        }

        if (colocando)
        {
            Vector2 posicaoInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.touchCount > 0)
            {
                posicaoInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }

            transform.position = posicaoInput;
        }

       
        if (Input.GetMouseButtonDown(1) || toqueDuploDetectado && !restrito)
        {
            colliderArea.enabled = true;
            colocando = false;
            GetComponent<colocarTorre>().enabled = false;
        }

        if (restrito)
        {
            spriteArea.color = vermelho;
        }
        else
        {
            spriteArea.color = cinza;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "caminho" || collision.gameObject.tag == "torre" && colocando)
        {
            restrito = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "caminho" || collision.gameObject.tag == "torre" && colocando)
        {
            restrito = false;
        }
    }
}