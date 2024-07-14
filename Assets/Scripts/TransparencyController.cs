using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player;
    public Camera mainCamera;
    public LayerMask layerMask; // Raycast�� Ž���� ���̾� ����
    private List<Renderer> previousRenderers = new List<Renderer>();

    void Update()
    {
        Vector3 direction = (player.position - mainCamera.transform.position).normalized;
        float distance = Vector3.Distance(mainCamera.transform.position, player.position);

        // ī�޶�� �÷��̾� ������ ��� ������Ʈ�� Ž��
        RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, direction, distance, layerMask);

        // ���� �������� ������Ʈ���� ���� ���·� ����
        foreach (Renderer rend in previousRenderers)
        {
            if (rend != null)
            {
                SetTransparency(rend, 1f); // ���� ������ ����
            }
        }

        previousRenderers.Clear();

        // ���� �������� ������Ʈ���� �����ϰ� ����
        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                SetTransparency(rend, 0.3f); // ������ ����
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
