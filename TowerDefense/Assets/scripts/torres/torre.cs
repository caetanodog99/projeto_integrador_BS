using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Torre : MonoBehaviour
{
    [Header("Especificações da Torre:")]
    public float area = 8f;
    public int dano = 10;
    public float cadencia = 1f;

    [Header("Mirar no alvo:")]
    public bool primeiro = true;
    public bool ultimo = false;
    public bool forte = false;
    
    public GameObject alvo;
    private float recarga = 0f;
    void Start()
    {

    }

    void Update()
    {

        if (alvo)
        {
            if (recarga >= cadencia)
            {
                Vector2 direcao = (alvo.transform.position - transform.position).normalized;
                float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
                var rotacao = Quaternion.Euler(0f, 0f, angulo);
                transform.rotation = rotacao;

                //transform.right = alvo.transform.position - transform.position;

                alvo.GetComponent<inimigo>().ReceberDano(dano);
                recarga = 0f;
            }
            else
            {
                recarga += 1 * Time.deltaTime;
            }
        }
    }
}