using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInstance : MonoBehaviour
{
    [Header("기본 설정")]
    public float lifeTime = 1.0f;
    public bool useUnscaledTime = false;

    [Header("위치 조정")]
    public Vector3 positionOffset;

    private float timer = 0f;

    private void OnEnable()
    {
        timer = 0;
        transform.position += positionOffset;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
