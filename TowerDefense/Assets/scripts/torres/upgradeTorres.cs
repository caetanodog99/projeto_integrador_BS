using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeTorres : NetworkBehaviour
{
    [System.Serializable]
    class Nivel
    {
        public float area = 6f;
        public int dano = 10;
        public float cadencia = 1.1f;
        public int valor = 100;
    }

    [SerializeField] private Nivel[] niveis = new Nivel[3];
    public int nivelAtual = 0;
    [NonSerialized] public string valorAtual;
    private Torre torre;
    [SerializeField] private areaTorre areaTorre;

    void Awake()
    {
        torre = GetComponent<Torre>();
        valorAtual = "C$ " + niveis[0].valor.ToString();
    }

    public void Upgrade()
    {
        if (nivelAtual < niveis.Length)
        {
            if (niveis[nivelAtual].valor < jogador.main.creditos)
            {
                RPC_SolicitarUpgradeServidor();
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SolicitarUpgradeServidor()
    {
        if (nivelAtual < niveis.Length)
        {
            if (niveis[nivelAtual].valor < jogador.main.creditos)
            {
                jogador.main.creditos = jogador.main.creditos - niveis[nivelAtual].valor;

                RPC_AplicarUpgradeTodosOsPlayers();
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_AplicarUpgradeTodosOsPlayers()
    {
        torre.area = niveis[nivelAtual].area;
        torre.dano = niveis[nivelAtual].dano;
        torre.cadencia = niveis[nivelAtual].cadencia;
        areaTorre.AtualizarArea();

        nivelAtual++;

        if (nivelAtual >= niveis.Length)
        {
            valorAtual = "MAX";
        }
        else
        {
            valorAtual = "C$" + niveis[nivelAtual].valor.ToString();
        }

  
    }
}