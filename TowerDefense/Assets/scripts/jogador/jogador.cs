using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jogador : NetworkBehaviour
{
    public static jogador main;

    [Networked, OnChangedRender(nameof(AtualizarInterfaceDeVida))]
    public int vida { get; set; } = 100;

    [Networked, OnChangedRender(nameof(AtualizarInterfaceDeCreditos))]
    public int creditos { get; set; } = 100;

    [SerializeField] private TextMeshProUGUI vidaTexto;
    [SerializeField] private TextMeshProUGUI creditosTexto;

    [SerializeField] private GameObject painelDerrota;

    void Awake()
    {
        main = this;
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            vida = 100;
            creditos = 100;
        }
        AtualizarInterfaceDeVida();
        AtualizarInterfaceDeCreditos();
    }

    private void AtualizarInterfaceDeVida()
    {
        if (vidaTexto != null)
        {
            vidaTexto.text = "Vida: " + vida.ToString();
        }

        if (vida <= 0 && painelDerrota != null)
        {
            painelDerrota.SetActive(true);
        }
    }

    private void AtualizarInterfaceDeCreditos()
    {
        if (creditosTexto != null)
        {
            creditosTexto.text = "Créditos: " + creditos.ToString();
        }
    }

    public void ReceberDano(int dano)
    {
        if (Object != null && !Object.HasStateAuthority) return;
        vida = vida - dano;
    }

    public void TentarNovamente()
    {
        if (Object != null && Object.HasStateAuthority)
        {
            string faseAtual = SceneManager.GetActiveScene().name; 
            SceneManager.LoadScene(faseAtual);
        }
    }

    public void MenuInicial()
    {
        if (Runner != null)
        {
            Runner.Shutdown();
        }
        SceneManager.LoadScene(0);
    }
}