using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public float timeToLive = 0.5f;
    public float floatSpeed = 500;

    public Vector3 floatDirection = new Vector3(0, 1, 0);

    public TextMeshProUGUI textMesh;

    RectTransform rectTransform;

    Color startingPoint;

    float timeElapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //textMesh = GetComponent<TextMeshPro>();
        rectTransform = GetComponent<RectTransform>();
        startingPoint = textMesh.color;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        rectTransform.position += floatDirection * floatSpeed * Time.deltaTime;

        textMesh.color = new Color(startingPoint.r, startingPoint.g, startingPoint.b, 1 - (timeElapsed / timeToLive));

        if(timeElapsed > timeToLive)
        {
            Destroy(gameObject);
        }
    }
}
