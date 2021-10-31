using MLAPI;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;

    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        menuPanel.SetActive(false);
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
        menuPanel.SetActive(false);
    }
}