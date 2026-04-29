using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jogador : NetworkBehaviour
{
    public static jogador main;
    [Networked][SerializeField] private int vida { get; set; } = 100;
    public int creditos = 100;
    [SerializeField] private TextMeshProUGUI vidaTexto;
    [SerializeField] private TextMeshProUGUI creditosTexto;

    [SerializeField] private GameObject painelDerrota;
    void Awake()
    {
        main = this;
    }

    
    void Update()
    {
        vidaTexto.text = "Vida: " + vida.ToString();
        creditosTexto.text = "Créditos: " + creditos.ToString();
        //Debug.Log("creditos:" + creditos);
    }

    public void ReceberDano(int dano)
    {
        vida = vida - dano;

        if (vida <= 0)
        {
            painelDerrota.SetActive(true);
        }
    }

    public void TentarNovamente()
    {
        string faseAtual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(faseAtual);
    }

    public void MenuInicial()
    {
        string faseAtual = SceneManager.GetActiveScene().name;
        SceneManager.UnloadScene(faseAtual);
        SceneManager.LoadSceneAsync(0);
    }
}
