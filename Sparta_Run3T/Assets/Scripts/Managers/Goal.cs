using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMove>();

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Goal");

            AudioManager.Instance.Play(SoundKey.SFX_UI_GAMEOVER);

            GameManager.Instance.GameClear();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
