using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{

    bool got=false;



    private void Update()
    {
        if(transform.parent != null)
        {
            transform.localPosition = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (got) { return; }

        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GetBox);
            got = true;
            PlayerController playerController = collision.GetComponent<PlayerController>();
            int i = playerController.curBox;
            transform.SetParent(playerController.BoxParents[i].transform, false);      
            transform.localPosition = Vector3.zero;
            Transform parent = transform.parent;

            transform.parent = null;
            transform.localScale = new Vector3(3f, 3f, 3f);
            transform.parent = parent;

            Debug.Log(1);
            playerController.GetBox();
        }
    }
}
