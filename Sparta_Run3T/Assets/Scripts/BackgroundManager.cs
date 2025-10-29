using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    //[SerializeField] private GameObject background;
    [SerializeField] private int backgroundCount = 3;
    [SerializeField][Range(1f, 20f)] private float Speed = 4f;
    [SerializeField] private float changeDistance= 300f;


   

    private float BackgroundWidth;
    private Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        BackgroundWidth = sr.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);

        float cameraLeftEdge = camera.transform.position.x - (camera.orthographicSize * camera.aspect) - (BackgroundWidth / 2);

        if (transform.position.x < cameraLeftEdge)
        {
            transform.position = new Vector3(
                transform.position.x + BackgroundWidth * 2f,
                transform.position.y,
                transform.position.z
                );
        }
    }
}

