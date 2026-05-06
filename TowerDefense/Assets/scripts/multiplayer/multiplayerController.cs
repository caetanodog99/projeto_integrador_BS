using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class multiplayerController : MonoBehaviour, INetworkRunnerCallbacks
{
    public TextMeshProUGUI nomeSala;
    public TextMeshProUGUI erro;
    public GameObject playerPrefab;
    public GameObject TelaEntrarSala;

    private NetworkRunner _runner;

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        if (string.IsNullOrEmpty(nomeSala.text))
        {

            erro.text = "O nome da sala não pode ser vazio!";
            return;
        }
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = nomeSala.text,
            Scene = SceneRef.FromIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        TelaEntrarSala.gameObject.SetActive(false);
    }


    public async void CriarSala()
    {
        StartGame(GameMode.Shared);
        Debug.Log(nomeSala.text);
    }

    public async void EntrarSala()
    {
        StartGame(GameMode.Shared);
        Debug.Log(nomeSala.text);
    }



    public void OnConnectedToServer(NetworkRunner _runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner _runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner _runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner _runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner _runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner _runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner _runner, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner _runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner _runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectExitAOI(NetworkRunner _runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner _runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner _runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner _runner, PlayerRef player, ReliableKey key, float progress)
    {
       throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner _runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner _runner)
    {

        if (_runner.LocalPlayer != PlayerRef.None && _runner.GetPlayerObject(_runner.LocalPlayer) == null)
        {
            var objetoDaRede = _runner.Spawn(playerPrefab,
                new Vector3(0, -1, 0),
                Quaternion.identity,
                _runner.LocalPlayer);
            _runner.SetPlayerObject(_runner.LocalPlayer, objetoDaRede);
        }

    }

    public void OnSceneLoadStart(NetworkRunner _runner)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner _runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner _runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner _runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }

    void Start()
    {

    }


    void Update()
    {

    }
}
