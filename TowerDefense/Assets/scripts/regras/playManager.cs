using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TerrainTools;

public class playManager : MonoBehaviour
{
    [SerializeField] public GameObject painelPause;
    [SerializeField] public GameObject painelLoja;

    [SerializeField] private List<GameObject> botoesLista = new List<GameObject>();
    [SerializeField] private GameObject menuSala;
    void Start()
    {
        if (menuSala.activeSelf == true)
        {
            foreach (GameObject botoes in botoesLista)
            {
                if (botoes != null)
                {
                   botoes.SetActive(false);
                }
            }
        }
       
    }

    public void BotaoPause()
    {
        painelPause.SetActive(true);
    }

    public void BotaoVoltar()
    {
        painelPause.SetActive(false);
    }

    public void AbrirLoja()
    {
        painelLoja.SetActive(true);
    }

    public void FecharLoja()
    {
        painelLoja.SetActive(false);
    }

    public void VoltarMenu()
    {
        int cena = SceneManager.GetActiveScene().buildIndex;
        SceneManager.UnloadSceneAsync(cena);
        SceneManager.LoadSceneAsync(0);
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            bool pauseAtivo = !painelPause.activeSelf;

            painelPause.SetActive(pauseAtivo);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            bool lojaAtiva = !painelLoja.activeSelf;

            painelLoja.SetActive(lojaAtiva);
        }

        if (menuSala.activeSelf == false)
        {
            foreach (GameObject botoes in botoesLista)
            {
                if (botoes != null)
                {
                    botoes.SetActive(true);
                }
            }
        }
    }
}
