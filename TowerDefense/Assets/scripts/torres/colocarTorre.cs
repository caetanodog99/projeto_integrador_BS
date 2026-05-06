using Fusion;
using UnityEngine;

public class colocarTorre : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteArea;
    [SerializeField] private CircleCollider2D colliderArea;
    [SerializeField] private Color cinza;
    [SerializeField] private Color vermelho;

    [SerializeField] public bool colocando { get; set; } = true;
    private bool restrito = false;
    private bool ehFantasma = false;
    private torreManager manager;

    private Torre torre;
    private float tempoUltimoToque = 0f;
    private float limiteTempoToqueDuplo = 0.25f;

    void Awake()
    {
        torre = GetComponent<Torre>();
        colliderArea.enabled = false;
        spriteArea.color = cinza;
    }

    
    public override void Spawned()
    {
        if (!ehFantasma)
        {
            colocando = false;          
            colliderArea.enabled = true;  
            spriteArea.enabled = false;     


            if (torre != null) torre.enabled = true;


            this.enabled = false;
        }
    }

    public void IniciaComoFantasma(torreManager m)
    {
        manager = m;
        ehFantasma = true;
        colocando = true;

        if (TryGetComponent<NetworkObject>(out var netObj)) netObj.enabled = false;
        if (torre != null) torre.enabled = false;
    }

    void Update()
    {

        if (!ehFantasma || !colocando) return;

        Vector2 posicaoInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.touchCount > 0)
        {
            posicaoInput = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        transform.position = posicaoInput;

        spriteArea.color = restrito ? vermelho : cinza;
        spriteArea.enabled = true;

        VerificarCliqueConfirmacao();
    }

    private void VerificarCliqueConfirmacao()
    {
        bool toqueDuploDetectado = false;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Time.time - tempoUltimoToque <= limiteTempoToqueDuplo) toqueDuploDetectado = true;
            tempoUltimoToque = Time.time;
        }


        if (Input.GetMouseButtonDown(1) || toqueDuploDetectado)
        {
            if (!restrito && torre.valor <= jogador.main.creditos)
            {
                manager.FinalizarPosicionamento(transform.position);

            }
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ehFantasma) return;
        if (collision.gameObject.CompareTag("caminho") || collision.gameObject.CompareTag("torre"))
        {
            restrito = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!ehFantasma) return;
        if (collision.gameObject.CompareTag("caminho") || collision.gameObject.CompareTag("torre"))
        {
            restrito = false;
        }
    }
}