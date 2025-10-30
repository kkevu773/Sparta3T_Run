using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.UpdateScore(124);
        UIManager.Instance.UpdateBestScore(12512);
        UIManager.Instance.UpdateHP(30, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
