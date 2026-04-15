using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class listaPlayers : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textoTela;

    void Update()
    {
        AtualizarPlacar();
    }

    void AtualizarPlacar()
    {
        string placar = "=== Jogadores ===\n";
        NetworkObject[] todosObjetos = FindObjectsOfType<NetworkObject>();
        int numeroPlayer = 1;

        foreach (NetworkObject networkObj in todosObjetos)
        {
            Grid player = networkObj.GetComponent<Grid>();

            if (player != null)
            {
                string marcador = networkObj.HasInputAuthority ? " (VOCÊ)" : "";
                placar += $"Player {numeroPlayer}{marcador}\n";
                numeroPlayer++;
            }
        }

        textoTela.text = placar;
    }
}
