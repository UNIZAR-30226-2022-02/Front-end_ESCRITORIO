using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FondoMovil : MonoBehaviour
{
    [SerializeField]
    RawImage image;
    [SerializeField]
    float speed;
    [SerializeField]
    Vector2 direction;

    Rect rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = image.uvRect;
    }

    // Update is called once per frame
    void Update()
    {
        rect.x += direction.x * speed * Time.deltaTime;
        rect.y += direction.y * speed * Time.deltaTime;
        image.uvRect = rect;
    }

}
