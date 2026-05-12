using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class torreManager : NetworkBehaviour
{
    public static torreManager main;

    [Header("Torres disponíveis:")]
    [SerializeField] private GameObject torreSimples;
    [SerializeField] private GameObject torreSniper;
    [SerializeField] private GameObject torreMelee;
    [SerializeField] private GameObject torreRapida;

    [SerializeField] private LayerMask torreLayer;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI nomeTorre;
    [SerializeField] private TextMeshProUGUI valorUpgrade;
    [SerializeField] private TextMeshProUGUI alvoTorre;
    [SerializeField] private TextMeshProUGUI nivelTorre;

    [SerializeField] private GameObject painelDinheiro;

    private GameObject torreSelecionada;
    private GameObject colocandoTorre;
    private GameObject prefabOriginalParaSpawn;

    private void Awake()
    {
        if (main == null) main = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LimparSelecao();
        }

        if (colocandoTorre != null) return;

        HandleSelection();
    }

    private void HandleSelection()
    {
        bool clicou = Input.GetMouseButtonDown(0);
        bool tocou = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if (clicou || tocou)
        {
            Vector3 posicaoInput = clicou ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;

            if (EventSystem.current.IsPointerOverGameObject() ||
               (tocou && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                return;
            }

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(posicaoInput);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 100f, torreLayer);

            if (hit.collider != null)
            {
                SelectTower(hit.collider.gameObject);
            }
            else if (torreSelecionada)
            {
                DeselectTower();
            }
        }
    }

    private void SelectTower(GameObject tower)
    {
        if (torreSelecionada) DeselectTower();

        torreSelecionada = tower;
        if (torreSelecionada.transform.childCount > 1)
            torreSelecionada.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;

        panel.SetActive(true);

        upgradeTorres scriptUpgrade = torreSelecionada.GetComponent<upgradeTorres>();
        nomeTorre.text = torreSelecionada.name.Replace("(Clone)", "");
        nivelTorre.text = "Nível: " + scriptUpgrade.nivelAtual;
        valorUpgrade.text = scriptUpgrade.valorAtual;
    }

    private void DeselectTower()
    {
        if (torreSelecionada && torreSelecionada.transform.childCount > 1)
            torreSelecionada.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;

        panel.SetActive(false);
        torreSelecionada = null;
    }

    private void LimparSelecao()
    {
        if (colocandoTorre)
        {
            Destroy(colocandoTorre);
            colocandoTorre = null;
        }
    }

    public void setTorre(GameObject torre)
    {
        int custo = torre.GetComponent<Torre>().valor;
        if (jogador.main.creditos >= custo)
        {
            LimparSelecao();
            prefabOriginalParaSpawn = torre;
            colocandoTorre = Instantiate(torre);
            colocandoTorre.GetComponent<colocarTorre>().IniciaComoFantasma(this);
        }
        else
        {
            StartCoroutine(SemDinheiro());
        }
    }

    public void FinalizarPosicionamento(Vector3 posicaoFinal)
    {
        posicaoFinal.z = 0;
        Runner.Spawn(prefabOriginalParaSpawn, posicaoFinal, Quaternion.identity, Runner.LocalPlayer, (runner, obj) => {
            obj.transform.position = posicaoFinal;
        });

        jogador.main.creditos -= prefabOriginalParaSpawn.GetComponent<Torre>().valor;

        if (colocandoTorre != null)
        {
            Destroy(colocandoTorre);
            colocandoTorre = null;
        }
    }

    IEnumerator SemDinheiro()
    {
        painelDinheiro.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        painelDinheiro.SetActive(false);
    }

    public void EvoluirSelecionada()
    {
        if (torreSelecionada)
        {
            torreSelecionada.GetComponent<upgradeTorres>().Upgrade();
            nivelTorre.text = "Nível: " + torreSelecionada.GetComponent<upgradeTorres>().nivelAtual.ToString();
            valorUpgrade.text = torreSelecionada.GetComponent<upgradeTorres>().valorAtual;
        }
    }

    public void TrocarAlvo()
    {
        if (!torreSelecionada) return;
        Torre torre = torreSelecionada.GetComponent<Torre>();

        if (torre.primeiro)
        {
            torre.primeiro = false;
            torre.ultimo = true;
            torre.forte = false;
            alvoTorre.text = " Último";
        }
        else if (torre.ultimo)
        {
            torre.primeiro = false;
            torre.ultimo = false;
            torre.forte = true;
            alvoTorre.text = " Forte";
        }
        else if (torre.forte)
        {
            torre.primeiro = true;
            torre.ultimo = false;
            torre.forte = false;
            alvoTorre.text = " Primeiro";
        }
    }
}