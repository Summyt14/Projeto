using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public RectTransform playerInMap;
    public RectTransform map2dEnd;
    public Transform map3dParent;
    public Transform map3dEnd;

    public GameObject minimap;
    private bool isShowing;

    private Vector3 normalized, mapped;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isShowing)
            {
                isShowing = true;
                minimap.SetActive(true);
            }
            else
            {
                isShowing = false;
                minimap.SetActive(false); 
            }
        }

        if (isShowing)
        {
            normalized = Divide(
                map3dParent.InverseTransformPoint(transform.position), // calcular a posição do jogador relativa ao centro do mapa
                map3dEnd.position - map3dParent.position // vetor de escala
            );
            normalized.y = normalized.z; //ao mapear de 3d para 2d a coordenada z deixa de ser relevante
            mapped = Vector3.Scale(normalized, map2dEnd.localPosition); //aplica a escala ao indicador do jogador no mapa
            playerInMap.localPosition = mapped; 
        }
        
    }

    private static Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }


    public void resetAfterLoad()
    {
        isShowing = false;
        minimap.SetActive(false); 
    }
}
