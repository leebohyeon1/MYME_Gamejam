using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Bomb);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void End()
    {
        Destroy(gameObject);
    }
}
