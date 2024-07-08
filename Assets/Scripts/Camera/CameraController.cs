using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // �÷��̾��� Transform�� ������ ����
    public Vector3 offset = new Vector3(0,0,-10f);  // �÷��̾�� ī�޶� ������ ������

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
