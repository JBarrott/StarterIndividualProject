using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public AudioClip keyClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        MinoController controller = other.GetComponent<MinoController>();

            if (controller != null)
            {
                controller.ChangeKey(1);
                Destroy(gameObject);

                controller.PlaySound(keyClip);
            }
    }
}
