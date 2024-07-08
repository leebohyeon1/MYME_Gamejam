using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform을 지정할 변수
    public Vector3 offset = new Vector3(0,0,-10f);  // 플레이어와 카메라 사이의 오프셋

    private void Start()
    {
        if(player == null)  
        player = GameObject.Find("Player").GetComponent<Transform>();
    }
    void Update()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1f);
        transform.position = smoothedPosition;
    }
}
