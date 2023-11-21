using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float floatSpeed = 1f;  // Velocidade do movimento de flutuação
    public float floatHeight = 10f;  // Altura máxima de flutuação
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Movimento senoidal para simular flutuação
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}
