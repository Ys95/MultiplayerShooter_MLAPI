using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour, ICanTakeDamage
{
    [SerializeField] NetworkVariableInt health = new NetworkVariableInt(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, 100);
    [SerializeField] GameObject UI;
    [SerializeField] Slider HealthBar;
    [SerializeField] Text HealthText;

    PlayerSpawner playerSpawner;

    void Awake()
    {
        playerSpawner = GetComponent<PlayerSpawner>();
        if (!IsLocalPlayer)
        {
            UI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            UpdateHealth();
        }
        if (health.Value <= 0 && IsLocalPlayer)
        {
            health.Value = 100;
            playerSpawner.Respawn();
        }
    }

    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }

    void UpdateHealth()
    {
        Debug.Log("Health update" + health.Value);
        HealthBar.value = health.Value;
        HealthText.text = health.Value.ToString();
    }
}