using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] Transform GunBarrel;
    [SerializeField] Transform BulletSpawn;

    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootServerRpc();
            }
        }
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        if (Physics.Raycast(BulletSpawn.position, BulletSpawn.forward, out RaycastHit hit, 200f))
        {
            var enemyHealth = hit.transform.GetComponent<ICanTakeDamage>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }
        }
        ShootClientRpc();
    }

    [ClientRpc]
    void ShootClientRpc()
    {
        Debug.Log("Shoot");
        var bullet = Instantiate(bulletTrail, GunBarrel.position, Quaternion.identity);
        bullet.AddPosition(GunBarrel.position);
        if (Physics.Raycast(BulletSpawn.position, BulletSpawn.forward, out RaycastHit hit, 200f))
        {
            bullet.transform.position = hit.point;
        }
        else
        {
            bullet.transform.position = BulletSpawn.position + (BulletSpawn.forward * 200f);
        }
    }
}