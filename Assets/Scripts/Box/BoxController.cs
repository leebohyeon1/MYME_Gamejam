using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{

    private void Update()
    {
        if(transform.parent != null)
        {
            transform.localPosition = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent != null)
        { return; }

        if (collision.CompareTag("Player"))
        {
           
            PlayerController playerController = collision.GetComponent<PlayerController>();
            int i = playerController.curBox;
            transform.SetParent(playerController.BoxParents[i].transform, false);      
            transform.localPosition = Vector3.zero;
            Transform parent = transform.parent;

            transform.parent = null;
            transform.localScale = new Vector3(3f, 3f, 3f);
            transform.parent = parent;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GetBox);

            playerController.GetBox();
        }
    }
}
