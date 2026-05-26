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

    private AudioSource audioAtirando;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioAtirando = GetComponent<AudioSource>();
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

                inimigo scriptInimigo = alvo.GetComponent<inimigo>();
                if (scriptInimigo != null)
                {
                    scriptInimigo.ReceberDano(dano);
                }

                recarga = 0f;

                RPC_DispararEfeitoEAnimacao();
            }
            else
            {
                recarga += 1 * Time.deltaTime;
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_DispararEfeitoEAnimacao()
    {
        audioAtirando.Play();
        StopAllCoroutines();
        StartCoroutine(DisparoEfeito());
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