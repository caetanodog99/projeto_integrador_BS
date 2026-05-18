using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TerrainTools;

public class playManager : MonoBehaviour
{
    [SerializeField] public GameObject painelPause;
    [SerializeField] public GameObject painelLoja;

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

    private void Update()
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
    }
}
