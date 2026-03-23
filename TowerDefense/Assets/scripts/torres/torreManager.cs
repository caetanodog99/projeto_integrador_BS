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

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,100f,torreLayer);

            if(hit.collider != null)
            {
                if (torreSelecionada)
                {
                    GameObject area1 = torreSelecionada.transform.GetChild(1).gameObject;
          
                    area1.GetComponent<SpriteRenderer>().enabled = false;
                }
                torreSelecionada = hit.collider.gameObject;

                GameObject area2 = torreSelecionada.transform.GetChild(1).gameObject;

                area2.GetComponent<SpriteRenderer>().enabled = true;

                panel.SetActive(true);
                nomeTorre.text = torreSelecionada.name.Replace("(Clone)","");
                nivelTorre.text = "Nível: " + torreSelecionada.GetComponent<upgradeTorres>().nivelAtual.ToString();
                valorUpgrade.text = torreSelecionada.GetComponent<upgradeTorres>().valorAtual;

                Torre torre = torreSelecionada.GetComponent<Torre>();

                if (torre.primeiro)
                {
                    alvoTorre.text = " Primeiro";
                }
                else if (torre.ultimo)
                {
                    alvoTorre.text = " Último";
                }
                else if (torre.forte)
                {
                    alvoTorre.text = " Forte";
                }
                else
                {
                    alvoTorre.text = " Primeiro";
                }
            }
            else if(!EventSystem.current.IsPointerOverGameObject()&& torreSelecionada)
            {
                panel.SetActive(false);

                GameObject area1 = torreSelecionada.transform.GetChild(1).gameObject;

                area1.GetComponent<SpriteRenderer>().enabled = false;

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
