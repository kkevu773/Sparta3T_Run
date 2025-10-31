using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundPrefabs;
    [SerializeField] private int backgroundCount = 3;
    [SerializeField][Range(1f, 20f)] private float speed = 4f;
    [SerializeField] private float changeDistance;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] public SpriteRenderer FadeObj;

    private List<GameObject> activeBackgrounds = new List<GameObject>();
    private float totalMovedDistance = 0f;
    private int currentPrefabsIndex = 0;
    private float backgroundWidth;
    private Camera mainCam;

    /* 배경 스크롤 시작/정지 구분 */
    private bool isScrolling = true;

    // Start is called before the first frame update
    void Start()
    {

        mainCam = Camera.main;
        SpriteRenderer sr = backgroundPrefabs[0].GetComponent<SpriteRenderer>();
        backgroundWidth = sr.bounds.size.x;

        for (int i = 0; i < backgroundCount; i++)
        {
            GameObject bg = Instantiate(backgroundPrefabs[0],
                new Vector3(i * backgroundWidth, 0, 0),
                Quaternion.identity,
                transform);

            activeBackgrounds.Add(bg);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* isScrolling == false => 바로 return */
        if (!isScrolling) return;

        float move = speed * Time.deltaTime;

        foreach (var bg in activeBackgrounds)
            bg.transform.Translate(Vector2.left * move);

        totalMovedDistance += move;
       
        float cameraLeftEdge = mainCam.transform.position.x - (mainCam.orthographicSize * mainCam.aspect);
        GameObject first = activeBackgrounds[0];
        float leftX = first.transform.position.x + backgroundWidth / 2;


        if (leftX < cameraLeftEdge)
        {
            GameObject last = activeBackgrounds[activeBackgrounds.Count - 1];

            first.transform.position = new Vector3(
              last.transform.position.x + backgroundWidth,
              first.transform.position.y,
              first.transform.position.z
              );

            activeBackgrounds.RemoveAt(0);
            activeBackgrounds.Add(first);
        }

  
        if (totalMovedDistance >= changeDistance)
        {
            totalMovedDistance = 0;
            StartCoroutine(OnFade());
        }
    }

    private void ChangeScene()
    {
        currentPrefabsIndex++;
        if(currentPrefabsIndex >= backgroundPrefabs.Length )
            currentPrefabsIndex = 0;

        SpriteRenderer nextScene = backgroundPrefabs[currentPrefabsIndex].GetComponent<SpriteRenderer>();
        foreach (var bg in activeBackgrounds)
        {
            SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
            sr.sprite = nextScene.sprite;

        }
    }

    private IEnumerator OnFade()
    {
      Color color = FadeObj.color;
        

        float t = 0f;
        while(t < fadeDuration)
        {
           t += Time.deltaTime;
            float value = Mathf.Lerp(0f, 1f, t / fadeDuration);
            color.a = value;
            FadeObj.color = color;
            yield return null;  
        }

        ChangeScene();
        yield return new WaitForSeconds(0.2f);

        t = 0f;
        while(t < fadeDuration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(1f, 0f, t / fadeDuration);
            color.a = value;
            FadeObj.color = color;
            yield return null;
        }

    }

    public void Restart()
    {
        /* Time.timeScale = 0f; */
        currentPrefabsIndex = 0;
        totalMovedDistance = 0f;
        SpriteRenderer restartScene = backgroundPrefabs[0].GetComponent<SpriteRenderer>();
        foreach (var bg in activeBackgrounds)
        {
            SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
            sr.sprite = restartScene.sprite;
        }
        /* Time.timeScale = 1f; */
    }



    /* 배경 스크롤 시작 */
    public void StartScroll()
    {
        isScrolling = true;
    }

    /* 배경 스크롤 정지 */
    public void StopScroll()
    {
        isScrolling = false;
    }

    /* 게임 재시작 시 배경 초기화 */
    public void ResetBackground()
    {
        /* 첫 번째 배경으로 리셋 */
        currentPrefabsIndex = 0;
        totalMovedDistance = 0f;

        SpriteRenderer firstSprite = backgroundPrefabs[0].GetComponent<SpriteRenderer>();

        /* 모든 배경을 첫 번째 스프라이트로 변경 */
        foreach (var bg in activeBackgrounds)
        {
            SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
            sr.sprite = firstSprite.sprite;
        }

        /* 배경 위치 초기화 */
        for (int i = 0; i < activeBackgrounds.Count; i++)
        {
            activeBackgrounds[i].transform.position = new Vector3(
                i * backgroundWidth,
                0,
                0
            );
        }

        /* Fade 오브젝트 투명하게 */
        if (FadeObj != null)
        {
            Color color = FadeObj.color;
            color.a = 0f;
            FadeObj.color = color;
        }

        /* 스크롤 시작 */
        isScrolling = true;
    }
}
