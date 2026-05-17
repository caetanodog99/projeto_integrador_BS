using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Torre : NetworkBehaviour
{
    [Header("Especificações da Torre:")]
    public float area = 8f;
    public int dano = 10;
    public float cadencia = 1f;
    public int valor = 60;

    [Header("Mirar no alvo:")]
    public bool primeiro = true;
    public bool ultimo = false;
    public bool forte = false;

    [Header("Efeitos:")]
    [SerializeField] GameObject efeitoDisparo;

    public GameObject alvo;
    private float recarga = 0f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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

                inimigo scriptInimigo = alvo.GetComponent<inimigo>();
                if (scriptInimigo != null && Object != null)
                {
                    scriptInimigo.ReceberDano(dano, Object.StateAuthority);
                }

                recarga = 0f;
                StartCoroutine(DisparoEfeito());
            }
            else
            {
                recarga += 1 * Time.deltaTime;
            }
        }
    }

    IEnumerator DisparoEfeito()
    {
        animator.SetBool("atacando", true);
        efeitoDisparo.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        efeitoDisparo.SetActive(false);
        animator.SetBool("atacando", false);
    }
}