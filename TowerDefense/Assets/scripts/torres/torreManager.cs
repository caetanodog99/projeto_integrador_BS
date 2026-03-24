using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class torreManager : MonoBehaviour
{
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


    private GameObject torreSelecionada;
    private GameObject colocandoTorre;

    
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            LimparSelecao();
        }

        if (colocandoTorre)
        {
            if (!colocandoTorre.GetComponent<colocarTorre>().colocando)
            {
                colocandoTorre = null;
            }
        }

        bool clicou = Input.GetMouseButtonDown(0);
        bool tocou = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if (clicou || tocou)
        {
            
            Vector3 posicaoEntrada = clicou ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;

          
            if (EventSystem.current.IsPointerOverGameObject() ||
               (tocou && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                return;
            }

          
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(posicaoEntrada), Vector2.zero, 100f, torreLayer);

            if (hit.collider != null)
            {
              
                if (torreSelecionada)
                {
                    torreSelecionada.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }

                torreSelecionada = hit.collider.gameObject;
                torreSelecionada.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                panel.SetActive(true);

               
                upgradeTorres scriptUpgrade = torreSelecionada.GetComponent<upgradeTorres>();
                nomeTorre.text = torreSelecionada.name.Replace("(Clone)", "");
                nivelTorre.text = "Nível: " + scriptUpgrade.nivelAtual;
                valorUpgrade.text = scriptUpgrade.valorAtual;

                //ConfigurarTextoAlvo(torreSelecionada.GetComponent<Torre>());
            }
            else if (torreSelecionada)
            {
                
                panel.SetActive(false);
                torreSelecionada.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                torreSelecionada = null;
            }
        }
    }

    private void LimparSelecao()
    {
        if(colocandoTorre)
        {
            Destroy(colocandoTorre);
            colocandoTorre = null;
        }
    }

    public void setTorre(GameObject torre)
    {
        LimparSelecao();
        colocandoTorre = Instantiate(torre);
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
