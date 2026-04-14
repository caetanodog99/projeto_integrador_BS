using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using TMPro;

public class inimigoManager : MonoBehaviour
{
    public static inimigoManager main;
    public Transform spawnpoint;
    public Transform[] checkpoints;

    [SerializeField] private GameObject inimigoRapido;
    [SerializeField] private GameObject inimigoTank;
    [SerializeField] private GameObject inimigoNormal;

    [SerializeField] private int onda = 1;
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

    void Start()
    {
        
    }

    void Update()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("inimigo");

        if (!ondaInterrompida && ondaConcluida && inimigos.Length == 0)
        {
            jogador.main.creditos += 15 + (5 * onda);
            ondaInterrompida = true;
            painelOndas.SetActive(true);
        }

        if(onda == 11)
        {
            Time.timeScale = 0f;
            painelVitoria.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && botaoPlay.activeSelf)
        {
            SetOndas();

            botaoPlay.SetActive(false);

            OndasTXT.text = "Onda: 1";
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProximaOnda();
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
        SetOndas();
        
        botaoPlay.SetActive(false);

        OndasTXT.text = "Onda: 1";
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

    public void ProximaOnda()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("inimigo");

        painelOndas.SetActive(false);

        if (ondaConcluida && inimigos.Length == 0)
        {
            Debug.Log("onda "+ onda +" concluida!");
            onda++;
            ondaConcluida = false;
            ondaInterrompida = false;
            inimigosTotal += Mathf.RoundToInt(inimigosTotal * inimigosTotalSpawn);
            SetOndas();
        }

        OndasTXT.text = "Onda: " + onda;
    }
    IEnumerator spawn()
    {
        for (int i = 0; i < ondas.Count; i++)
        {
            Instantiate(ondas[i], spawnpoint.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(SpawnDelayMin, SpawnDelayMax));
        }
        ondaConcluida = true;
    }
}