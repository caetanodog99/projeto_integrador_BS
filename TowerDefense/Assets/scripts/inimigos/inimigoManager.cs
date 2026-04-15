using Fusion;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class inimigoManager : NetworkBehaviour
{
    public static inimigoManager main;
    public Transform spawnpoint;
    public Transform[] checkpoints;

    [SerializeField] private GameObject inimigoRapido;
    [SerializeField] private GameObject inimigoTank;
    [SerializeField] private GameObject inimigoNormal;

    [Networked][SerializeField] private int onda { get; set; } = 1;
    [Networked][SerializeField] private int inimigosTotal { get; set; } = 6;
    [Networked][SerializeField] private float inimigosTotalSpawn { get; set; } = 0.2f;
    [Networked][SerializeField] private float SpawnDelayMax { get; set; } = 1f;
    [Networked][SerializeField] private float SpawnDelayMin { get; set; } = 0.75f;

    [Networked][SerializeField] private float normalSpawn { get; set; } = 0.5f;
    [Networked][SerializeField] private float rapidoSpawn { get; set; } = 0.3f;
    [Networked][SerializeField] private float tankSpawn { get; set; } = 0.2f;

    [SerializeField] private GameObject painelOndas;
    [SerializeField] public GameObject painelVitoria;
    [SerializeField] private TextMeshProUGUI OndasTXT;
    [SerializeField] public GameObject botaoPlay;

    [Networked] public NetworkBool ondaConcluida { get; set; }
    [Networked] public NetworkBool jogoIniciado { get; set; }
    [Networked] private TickTimer spawnTimer { get; set; }

    private int inimigosSpawnadosNestaOnda = 0;
    private List<GameObject> listaOrdemSpawn = new List<GameObject>();

    void Awake()
    {
        main = this;
    }

    public override void FixedUpdateNetwork()
    {
        if (OndasTXT != null) OndasTXT.text = "Onda: " + onda;

        if (Object.HasStateAuthority)
        {
            LogicaDeOndasServidor();
        }

        if (painelVitoria != null) painelVitoria.SetActive(onda == 11);
        if (botaoPlay != null) botaoPlay.SetActive(!jogoIniciado);
    }

    private void LogicaDeOndasServidor()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("inimigo");

        if (jogoIniciado && ondaConcluida && inimigos.Length == 0 && onda < 11)
        {
            if (painelOndas != null && !painelOndas.activeSelf)
            {
                if (jogador.main != null) jogador.main.creditos += 15 + (5 * onda);
                RPC_AtivarPainelOndas(true);
            }
        }

        if (jogoIniciado && !ondaConcluida && spawnTimer.ExpiredOrNotRunning(Runner))
        {
            if (listaOrdemSpawn.Count == 0) PrepararOnda();

            if (inimigosSpawnadosNestaOnda < listaOrdemSpawn.Count)
            {
                Runner.Spawn(listaOrdemSpawn[inimigosSpawnadosNestaOnda], spawnpoint.position, Quaternion.identity);
                inimigosSpawnadosNestaOnda++;

                float delay = UnityEngine.Random.Range(SpawnDelayMin, SpawnDelayMax);
                spawnTimer = TickTimer.CreateFromSeconds(Runner, delay);
            }
            else
            {
                ondaConcluida = true;
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SolicitarInicio()
    {
        if (!jogoIniciado)
        {
            jogoIniciado = true;
            PrepararOnda();
            RPC_AtivarPainelOndas(false);
        }
        else if (ondaConcluida)
        {
            ProximaOnda();
        }
    }

    public void BotaoPlay()
    {
        RPC_SolicitarInicio();
    }

    private void PrepararOnda()
    {
        int tankTotal = (onda >= 1) ? Mathf.RoundToInt(inimigosTotal * tankSpawn) : 0;
        int normalTotal = Mathf.RoundToInt(inimigosTotal * normalSpawn);
        int rapidoTotal = Mathf.RoundToInt(inimigosTotal * rapidoSpawn);

        listaOrdemSpawn.Clear();
        for (int i = 0; i < normalTotal; i++) listaOrdemSpawn.Add(inimigoNormal);
        for (int i = 0; i < rapidoTotal; i++) listaOrdemSpawn.Add(inimigoRapido);
        for (int i = 0; i < tankTotal; i++) listaOrdemSpawn.Add(inimigoTank);

        listaOrdemSpawn = Embaralhar(listaOrdemSpawn);

        inimigosSpawnadosNestaOnda = 0;
        ondaConcluida = false;
        spawnTimer = TickTimer.CreateFromSeconds(Runner, 1f);
    }

    public void ProximaOnda()
    {
        if (Object.HasStateAuthority && ondaConcluida)
        {
            onda++;
            inimigosTotal += Mathf.RoundToInt(inimigosTotal * inimigosTotalSpawn);
            PrepararOnda();
            RPC_AtivarPainelOndas(false);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_AtivarPainelOndas(bool ativo)
    {
        if (painelOndas != null) painelOndas.SetActive(ativo);
    }

    public List<GameObject> Embaralhar(List<GameObject> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            GameObject temp = lista[i];
            int randomIndex = UnityEngine.Random.Range(i, lista.Count);
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
        return lista;
    }
}