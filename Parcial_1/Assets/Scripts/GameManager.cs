using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PlayerScoreData :
    INetworkSerializable,
    IEquatable<PlayerScoreData>
{
    public ulong ClientId;
    public int Score;


    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref Score);
    }

    public bool Equals(PlayerScoreData other)
    {
        return ClientId == other.ClientId &&
               Score == other.Score;
    }

    public override bool Equals(object obj)
    {
        return obj is PlayerScoreData other &&
               Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ClientId, Score);
    }
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public NetworkList<PlayerScoreData> PlayerScores = new();

    [SerializeField]
    private float gameDuration = 180f;

    private NetworkVariable<float> remainingTime =
        new NetworkVariable<float>();
   
    private NetworkVariable<ulong> winnerClientId =
    new NetworkVariable<ulong>();

    private NetworkVariable<int> winnerScore =
        new NetworkVariable<int>();

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        remainingTime.Value = gameDuration;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            AddPlayer(client.ClientId);
        }

        NetworkManager.Singleton.OnClientConnectedCallback += AddPlayer;
    }

    public override void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= AddPlayer;
        }

        base.OnDestroy();
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (remainingTime.Value <= 0)
            return;

        remainingTime.Value -= Time.deltaTime;

        if (remainingTime.Value <= 0)
        {
            EndGame();
        }
    }

    private void AddPlayer(ulong clientId)
    {
        for (int i = 0; i < PlayerScores.Count; i++)
        {
            if (PlayerScores[i].ClientId == clientId)
                return;
        }

        PlayerScores.Add(
            new PlayerScoreData
            {
                ClientId = clientId,
                Score = 0
            });
    }

    public void AddPoint(ulong clientId)
    {
        for (int i = 0; i < PlayerScores.Count; i++)
        {
            if (PlayerScores[i].ClientId == clientId)
            {
                PlayerScoreData data = PlayerScores[i];
                data.Score++;

                PlayerScores[i] = data;
                return;
            }
        }
    }

    public float GetRemainingTime()
    {
        return remainingTime.Value;
    }

    private void EndGame()
    {
        int highestScore = -1;
        ulong winnerId = 0;

        foreach (var player in PlayerScores)
        {
            if (player.Score > highestScore)
            {
                highestScore = player.Score;
                winnerId = player.ClientId;
            }
        }

        winnerClientId.Value = winnerId;
        winnerScore.Value = highestScore;

        SetWinnerClientRpc(
            winnerId,
            highestScore);

        NetworkManager.SceneManager.LoadScene(
            "VictoryScene",
            LoadSceneMode.Single);
    }

    [ClientRpc]
    private void SetWinnerClientRpc(
    ulong winnerId,
    int highestScore)
    {
        GameResults.WinnerClientId = winnerId;
        GameResults.WinnerScore = highestScore;
    }
}