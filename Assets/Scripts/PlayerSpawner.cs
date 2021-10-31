using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] Behaviour[] scripts;
    [SerializeField] ParticleSystem DeathParticle;

    CharacterController cc;
    Renderer[] renderers;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (IsLocalPlayer && Input.GetKeyDown(KeyCode.Y))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        RespawnServerRPC();
    }

    [ServerRpc]
    void RespawnServerRPC()
    {
        RespawnClientRPC(GetRandomSpawn());
    }

    [ClientRpc]
    void RespawnClientRPC(Vector3 spwnPos)
    {
        StartCoroutine(RespawnCoroutine(spwnPos));
    }

    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-5f, 5f);
        float y = 2f;
        float z = Random.Range(-5f, 5f);
        return new Vector3(x, y, z);
    }

    IEnumerator RespawnCoroutine(Vector3 spwnPos)
    {
        Instantiate(DeathParticle, transform.position, transform.rotation);
        cc.enabled = false;
        SetPlayerState(false);
        transform.position = spwnPos;
        yield return new WaitForSeconds(3f);
        cc.enabled = true;
        SetPlayerState(true);
    }

    void SetPlayerState(bool state)
    {
        foreach (var script in scripts)
        {
            script.enabled = state;
        }

        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
    }
}