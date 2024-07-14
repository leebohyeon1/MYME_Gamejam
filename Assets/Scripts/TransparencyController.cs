using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player;
    public Camera mainCamera;
    public LayerMask layerMask; // Raycast가 탐지할 레이어 설정
    private List<Renderer> previousRenderers = new List<Renderer>();

    void Update()
    {
        Vector3 direction = (player.position - mainCamera.transform.position).normalized;
        float distance = Vector3.Distance(mainCamera.transform.position, player.position);

        // 카메라와 플레이어 사이의 모든 오브젝트를 탐지
        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, direction, distance, layerMask);

        // 이전 프레임의 오브젝트들을 원래 상태로 복원
        foreach (Renderer rend in previousRenderers)
        {
            if (rend != null)
            {
                SetTransparency(rend, 1f); // 원래 투명도로 설정
            }
        }

        previousRenderers.Clear();

        // 현재 프레임의 오브젝트들을 투명하게 설정
        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                SetTransparency(rend, 0.3f); // 투명도로 설정
                previousRenderers.Add(rend);
            }
        }
    }

    private void SetTransparency(Renderer renderer, float alpha)
    {
        foreach (Material mat in renderer.materials)
        {
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
        }
    }
}
