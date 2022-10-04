using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBala : MonoBehaviour
{
    public GameObject prefab;
    public float shootCooldown;
    float nextShoot;

    void LateUpdate()
    {
        disparoAutomatico();

        /* El cooldown de antes era 0.18
        if (Time.time > nextShoot && Input.GetMouseButton(0))
        {
            GameObject bala = Instantiate(prefab, transform.position, transform.rotation);
            nextShoot = Time.time + shootCooldown;
            Destroy(bala, 3f);

            if (AudioManager.instance != null)
            {
                AudioManager.PlaySound(AudioManager.instance.disparoBala);
            }
        }
        */
    }

    void disparoAutomatico()
    {
        if (Time.time > nextShoot)
        {
            GameObject bala = Instantiate(prefab, transform.position, transform.rotation);
            nextShoot = Time.time + shootCooldown;
            Destroy(bala, 3f);

            if (AudioManager.instance != null)
            {
                AudioManager.PlaySound(AudioManager.instance.disparoBala);
            }
        }
    }
}
