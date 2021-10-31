using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] GameObject connectButtonsPanel;
    [SerializeField] InputField ipInput;
    [SerializeField] string hostIP = "127.0.0.1";
    [SerializeField] Camera lobbyCamera;

    UNetTransport transport;

    public void Host()
    {
        connectButtonsPanel.SetActive(false);
        lobbyCamera.gameObject.SetActive(false);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(GetRandomSpawn(), Quaternion.identity);
    }

    void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "Password123";
        callback(true, null, approve, GetRandomSpawn(), Quaternion.identity);
    }

    public void Join()
    {
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = hostIP;
        connectButtonsPanel.SetActive(false);
        lobbyCamera.gameObject.SetActive(false);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("Password123");
        NetworkManager.Singleton.StartClient();
    }

    Vector3 GetRandomSpawn()
    {
        return new Vector3(UnityEngine.Random.Range(-10f, 10f), 2f, UnityEngine.Random.Range(-10f, 10f));
    }

    public void IPAddressChanged()
    {
        hostIP = ipInput.text;
    }
}