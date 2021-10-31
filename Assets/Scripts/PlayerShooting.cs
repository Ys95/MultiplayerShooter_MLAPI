using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] LineRenderer bulletLine;
    [SerializeField] Transform gunBarell;
    [SerializeField] float timeBetweenBullets = 0.25f;
    [SerializeField] float range = 100f;

    float timer;
    int shootableMask;
    float effectsDisplayTime = 0.9f;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        Cursor.visible = false;
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            timer += Time.deltaTime;
            if (Input.GetButtonDown("Fire1") && timer >= timeBetweenBullets)
            {
                timer = 0f;
                ShootServerRpc();
            }
        }
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        if (Physics.Raycast(gunBarell.position, gunBarell.forward, out RaycastHit hit, 200f, shootableMask))
        {
            var enemyHealth = hit.transform.GetComponent<ICanTakeDamage>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }

            var rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForceAtPosition(gunBarell.forward.normalized, hit.point, ForceMode.Impulse);
            }

            ShootHitClientRpc(hit.point);
        }
        else
        {
            ShootMissClientRpc();
        }
    }

    [ClientRpc]
    void ShootClientRpc()
    {
        bulletLine.enabled = true;
        bulletLine.SetPosition(0, gunBarell.position);
        if (Physics.Raycast(gunBarell.position, gunBarell.forward, out RaycastHit hit, 200f, shootableMask))
        {
            bulletLine.SetPosition(1, hit.point);
        }
        else
        {
            bulletLine.SetPosition(1, gunBarell.position + (gunBarell.forward * 200f));
        }
        Invoke("DisableEffects", timeBetweenBullets * effectsDisplayTime);
    }

    [ClientRpc]
    void ShootHitClientRpc(Vector3 hitPoint)
    {
        bulletLine.enabled = true;
        bulletLine.SetPosition(0, gunBarell.position);
        bulletLine.SetPosition(1, hitPoint);
        Invoke("DisableEffects", timeBetweenBullets * effectsDisplayTime);
    }

    [ClientRpc]
    void ShootMissClientRpc()
    {
        Debug.Log("Shoot");
        bulletLine.enabled = true;
        bulletLine.SetPosition(0, gunBarell.position);
        bulletLine.SetPosition(1, gunBarell.position + (gunBarell.forward * 200f));
        Invoke("DisableEffects", timeBetweenBullets * effectsDisplayTime);
    }

    void DisableEffects()
    {
        bulletLine.enabled = false;
    }
}