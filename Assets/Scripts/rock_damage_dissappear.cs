using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock_damage_dissappear : MonoBehaviour
{
    public LayerMask killLayers;

    public bool alsoKillOnPlayer = true;

    bool dying;

    void OnTriggerEnter2D(Collider2D other) { Handle(other.gameObject); }
    void OnCollisionEnter2D(Collision2D col) { Handle(col.collider.gameObject); }

    void Handle(GameObject other)
    {
        if (dying) return;

        if (alsoKillOnPlayer && other.CompareTag("Player"))
        {
            // 让 player_damage 还能在本帧触发，稍微延迟销毁
            DestroySoon();
            return;
        }

        int layerBit = 1 << other.layer;
        if ((killLayers.value & layerBit) != 0)
        {
            DestroySoon();
        }
    }

    void DestroySoon()
    {
        dying = true;
        Destroy(gameObject, 0.01f);
    }
}
