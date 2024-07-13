using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BoxController : MonoBehaviour
{
    public bool isGood;
    public bool isBad;

    private void Start()
    {
        if(isGood || isBad)
        {
            StartCoroutine("StartDelete");
        }
    }

    private void Update()
    {
        if(transform.parent != null)
        {
            transform.localPosition = Vector2.zero;
        }

        if(transform.parent.GetComponent<PlayerController>())
        {
            StopCoroutine("StartDelete");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent != null)
        { return; }

        if (collision.CompareTag("Player"))
        {
          
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (isBad || isGood)
            {
                if(isBad)
                {
                    GameManager.Instance.badBoxTrashCan.SetActive(true);
                }
                if (playerController.curBox > 0)
                {
                    return;
                }
                else
                {
                    int i = playerController.curBox;
                    transform.SetParent(playerController.BoxParents[i].transform, false);
                    transform.localPosition = Vector3.zero;
                    Transform parent = transform.parent;

                    transform.parent = null;
                    transform.localScale = new Vector3(3f, 3f, 3f);
                    transform.parent = parent;
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.GetBox);

                    playerController.GetBox();
                    playerController.haveSpecialBox = true;
                }
            }
            else
            {
                if(playerController.haveSpecialBox)
                {
                    return;
                }
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
    IEnumerator StartDelete()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
        if(isBad)
        GameManager.Instance.score -= GameManager.Instance.score / 100 * 20;
    }
}
