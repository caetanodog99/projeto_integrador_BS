using Fusion;
using UnityEngine;

public class colocarTorre : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteArea;
    [SerializeField] private CircleCollider2D colliderArea;
    [SerializeField] private Color cinza;
    [SerializeField] private Color vermelho;

    [Networked] public NetworkBool colocando { get; set; } = true;
    [Networked] private NetworkBool restrito { get; set; } = false;

    private Torre torre;
    private float tempoUltimoToque = 0f;
    private float limiteTempoToqueDuplo = 0.25f;

    void Awake()
    {
        torre = GetComponent<Torre>();
        colliderArea.enabled = false;
        spriteArea.color = cinza;
    }


    public override void Render()
    {

        if (Object.HasInputAuthority && colocando)
        {
            MoverLocalmente();
        }
    }

    public override void FixedUpdateNetwork()
    {

        if (!Object.HasInputAuthority) return;

        bool toqueDuploDetectado = false;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Time.time - tempoUltimoToque <= limiteTempoToqueDuplo)
            {
                toqueDuploDetectado = true;
            }
            tempoUltimoToque = Time.time;
        }


        if ((Input.GetMouseButtonDown(1) || toqueDuploDetectado) && colocando)
        {

            RPC_SolicitarPosicionamento(transform.position);
        }


        spriteArea.color = restrito ? vermelho : cinza;
    }

    private void MoverLocalmente()
    {
        Vector2 posicaoInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.touchCount > 0)
        {
            posicaoInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

        transform.position = posicaoInput;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SolicitarPosicionamento(Vector3 posicaoFinal)
    {

        if (!restrito && jogador.main.creditos >= torre.valor)
        {
            transform.position = posicaoFinal;
            colocando = false;
            restrito = false;
            colliderArea.enabled = true;
            spriteArea.enabled = false;


            jogador.main.creditos -= torre.valor;


            this.enabled = false;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Object.HasInputAuthority) return;
        if (collision.CompareTag("caminho") || collision.CompareTag("torre"))
        {
            restrito = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!Object.HasInputAuthority) return;
        if (collision.CompareTag("caminho") || collision.CompareTag("torre"))
        {
            restrito = false;
        }
    }
}