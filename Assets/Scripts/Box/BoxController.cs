using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            transform.SetParent(player.transform, false);
            int i = player.GetComponent<PlayerController>().curBox;
            transform.localPosition = new Vector3(0, 1.2f + i, 0);
            player.GetComponent<PlayerController>().GetBox();
          
        }
    }
}
