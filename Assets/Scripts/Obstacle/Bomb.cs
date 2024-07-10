using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(new Vector3(transform.localScale.x * 5, transform.localScale.y * 5,
            transform.localScale.z * 5), 2f).SetEase(Ease.InQuad).OnComplete(()=> Attack());
    }

    void Attack()
    {
        Instantiate(explosion,transform.position,Quaternion.identity);
        Destroy(gameObject);     
    }
}
