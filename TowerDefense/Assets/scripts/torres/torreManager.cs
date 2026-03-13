using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torreManager : MonoBehaviour
{
    [Header("Torres disponíveis:")]
    [SerializeField] private GameObject torreSimples;
    [SerializeField] private GameObject torreSniper;
    [SerializeField] private GameObject torreMelee;

    private GameObject colocandoTorre;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            setTorre(torreSimples);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            setTorre(torreSniper);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            setTorre(torreMelee);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    private void LimparSelecao()
    {
        if(colocandoTorre)
        {
            Destroy(colocandoTorre);
            colocandoTorre = null;
        }
    }

    private void setTorre(GameObject torre)
    {
        LimparSelecao();
        colocandoTorre = Instantiate(torre);
    }
}
