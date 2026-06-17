using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject connectCanvas;
    [SerializeField] private GameObject lobbyCanvas;

    [Header("Botones")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button playButton;

    [Header("UI")]
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text ipText;
    [SerializeField] private TMP_Text playersText;

    private void Start()
    {
        mainMenuCanvas.SetActive(true);
        connectCanvas.SetActive(false);
        lobbyCanvas.SetActive(false);

        playButton.gameObject.SetActive(false);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    // ================= HOST =================

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();

        hostButton.interactable = false;

        mainMenuCanvas.SetActive(false);
        connectCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);

        statusText.text = "Esperando jugadores...";
        ipText.text = "IP: " + GetLocalIPAddress();

        UpdatePlayerCount();
    }

    // ================= CLIENT =================

    public void OpenConnectMenu()
    {
        mainMenuCanvas.SetActive(false);
        connectCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
    }

    public void ConnectToHost()
    {
        string ipAddress = ipInputField.text.Trim();

        if (string.IsNullOrEmpty(ipAddress))
        {
            statusText.text = "Ingrese una IP válida";
            return;
        }

        UnityTransport transport =
            NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.ConnectionData.Address = ipAddress;

        bool connected = NetworkManager.Singleton.StartClient();

        if (!connected)
        {
            statusText.text = "No se pudo iniciar el cliente";
            return;
        }

        connectCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);

        playButton.gameObject.SetActive(false);

        statusText.text = "Conectando...";
        ipText.text = "";
        playersText.text = "";
    }

    // ================= CANCELAR =================

    public void CancelConnection()
    {
        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
        }

        mainMenuCanvas.SetActive(true);
        connectCanvas.SetActive(false);
        lobbyCanvas.SetActive(false);

        hostButton.interactable = true;

        statusText.text = "";
        ipText.text = "";
        playersText.text = "";

        playButton.gameObject.SetActive(false);
    }

    // ================= JUGAR =================

    public void StartGame()
    {
        if (!NetworkManager.Singleton.IsHost)
            return;

        if (NetworkManager.Singleton.ConnectedClients.Count < 2)
            return;

        NetworkManager.Singleton.SceneManager.LoadScene(
            "GameplayScene",
            LoadSceneMode.Single
        );
    }

    // ================= CONEXIONES =================

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            UpdatePlayerCount();
        }

        if (NetworkManager.Singleton.IsClient &&
            !NetworkManager.Singleton.IsHost)
        {
            statusText.text =
                "Esperando que el host inicie la partida...";
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton == null)
            return;

        if (NetworkManager.Singleton.IsHost)
        {
            UpdatePlayerCount();
        }
        else
        {
            statusText.text =
                "No se pudo conectar al host";
        }
    }

    // ================= JUGADORES =================

    private void UpdatePlayerCount()
    {
        int playerCount =
            NetworkManager.Singleton.ConnectedClients.Count;

        playersText.text =
            "Jugadores conectados: " + playerCount;

        playButton.gameObject.SetActive(playerCount >= 2);
    }

    // ================= IP LOCAL =================

    private string GetLocalIPAddress()
    {
        string localIP = "No encontrada";

        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily ==
                AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }

        return localIP;
    }

    public void QuitGame()
    {
        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
        }

        Application.Quit();
    }

}