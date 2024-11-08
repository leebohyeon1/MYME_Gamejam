using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BoxController : MonoBehaviour
{
    public bool isGood;
    public bool isBad;
    public bool isPlayer = false;
    private void Start()
    {
        isPlayer = false;
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

        if(transform.parent == null)
        {
            return;
        }
        if(transform.parent.GetComponent<PlayerController>())
        {
            StopCoroutine("StartDelete");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isPlayer)
        {
            return;
        }
        
        if (collision.CompareTag("Player"))
        {
            isPlayer = true;
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
            playerController.haveSpecialBox = true;
               
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
