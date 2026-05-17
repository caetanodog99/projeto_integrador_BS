using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class inimigoManager : NetworkBehaviour
{
    public static inimigoManager main;
    public Transform spawnpoint;
    public Transform[] checkpoints;

    [SerializeField] private GameObject inimigoRapido;
    [SerializeField] private GameObject inimigoTank;
    [SerializeField] private GameObject inimigoNormal;

    [Networked, OnChangedRender(nameof(AtualizarInterfaceDeOnda))]
    private int onda { get; set; } = 1;

    [SerializeField] private int inimigosTotal = 6;
    [SerializeField] private float inimigosTotalSpawn = 0.2f;
    [SerializeField] private float SpawnDelayMax = 1f;
    [SerializeField] private float SpawnDelayMin = 0.75f;

    [SerializeField] private float normalSpawn = 0.5f;
    [SerializeField] private float rapidoSpawn = 0.3f;
    [SerializeField] private float tankSpawn = 0.2f;

    [SerializeField] private GameObject painelOndas;

    [SerializeField] public GameObject painelVitoria;

    [SerializeField] private TextMeshProUGUI OndasTXT;

    [SerializeField] public GameObject botaoPlay;

    private bool ondaConcluida = false;
    private bool ondaInterrompida = false;
    private List<GameObject> ondas = new List<GameObject>();
    private int inimigoFalta;

    private int normalTotal;
    private int rapidoTotal;
    private int tankTotal;

    void Awake()
    {
        main = this;
    }

    void Update()
    {
        if (Object != null)
        {
            if (!Object.HasStateAuthority && botaoPlay.activeSelf)
            {
                botaoPlay.SetActive(false);
            }
        }

        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("inimigo");

        if (!ondaInterrompida && ondaConcluida && inimigos.Length == 0)
        {
            jogador.main.creditos += 15 + (5 * onda);
            ondaInterrompida = true;

            if (Object != null && Object.HasStateAuthority)
            {
                painelOndas.SetActive(true);
            }
        }

        if (onda == 11)
        {
            painelVitoria.SetActive(true);
        }

        if (Object != null && Object.HasStateAuthority)
        {
            if (Input.GetKeyDown(KeyCode.Space) && botaoPlay.activeSelf)
            {
                RPC_IniciarJogo();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                RPC_ChamarProximaOnda();
            }
        }
    }

    private void SetOndas()
    {
        normalTotal = Mathf.RoundToInt(inimigosTotal * (normalSpawn + tankTotal));
        rapidoTotal = Mathf.RoundToInt(inimigosTotal * rapidoSpawn);
        tankTotal = 0;

        if (onda % 1 == 0)
        {
            tankTotal = Mathf.RoundToInt(inimigosTotal * tankSpawn);
            normalTotal = Mathf.RoundToInt(inimigosTotal * normalSpawn);
        }

        inimigoFalta = normalTotal + rapidoTotal + tankTotal;
        inimigosTotal = inimigoFalta;

        ondas = new List<GameObject>();

        for (int i = 0; i < normalTotal; i++)
        {
            ondas.Add(inimigoNormal);
        }
        for (int i = 0; i < rapidoTotal; i++)
        {
            ondas.Add(inimigoRapido);
        }
        for (int i = 0; i < tankTotal; i++)
        {
            ondas.Add(inimigoTank);
        }

        ondas = Embaralhar(ondas);

        StartCoroutine(spawn());
    }

    public void BotaoPlay()
    {
        if (!Object.HasStateAuthority) return;
        RPC_IniciarJogo();
    }

    public void ProximaOnda()
    {
        if (!Object.HasStateAuthority) return;
        RPC_ChamarProximaOnda();
    }

    private void AtualizarInterfaceDeOnda()
    {
        if (OndasTXT != null)
        {
            OndasTXT.text = "Onda: " + onda;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_IniciarJogo()
    {
        SetOndas();
        botaoPlay.SetActive(false);
        AtualizarInterfaceDeOnda();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ChamarProximaOnda()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("inimigo");

        painelOndas.SetActive(false);

        if (ondaConcluida && inimigos.Length == 0)
        {
            Debug.Log("onda " + onda + " concluida!");
            onda++;
            ondaConcluida = false;
            ondaInterrompida = false;
            inimigosTotal += Mathf.RoundToInt(inimigosTotal * inimigosTotalSpawn);
            SetOndas();
        }

        AtualizarInterfaceDeOnda();
    }

    public List<GameObject> Embaralhar(List<GameObject> ondas)
    {
        List<GameObject> tempo = new List<GameObject>();
        List<GameObject> resultado = new List<GameObject>();
        tempo.AddRange(ondas);

        for (int i = 0; i < ondas.Count; i++)
        {
            int index = Random.Range(0, tempo.Count - 1);
            resultado.Add(tempo[index]);
            tempo.RemoveAt(index);
        }

        return resultado;
    }

    IEnumerator spawn()
    {
        if (Object.HasStateAuthority)
        {
            for (int i = 0; i < ondas.Count; i++)
            {
                Runner.Spawn(ondas[i], spawnpoint.position, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(SpawnDelayMin, SpawnDelayMax));
            }
            ondaConcluida = true;
        }
    }
}