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

    /* 난이도, 아이템별 속도 조절용 */
    [Header("Speed Settings")]
    [SerializeField] private float difficultySpeedMultiplier = 1.0f;
    [SerializeField] private float itemSpeedMultiplier = 1.0f;

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

        /* 최종 속도 = 기본 속도 * 난이도 배율 * 아이템 배율 */
        float appliedSpeed = speed * difficultySpeedMultiplier * itemSpeedMultiplier;

        float move = appliedSpeed * Time.deltaTime;

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
        currentPrefabsIndex = 0;
        totalMovedDistance = 0f;
        SpriteRenderer restartScene = backgroundPrefabs[0].GetComponent<SpriteRenderer>();
        foreach (var bg in activeBackgrounds)
        {
            SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
            sr.sprite = restartScene.sprite;
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

    /* 난이도에 따른 기본 속도 배율 설정 (게임 시작 시, 한 번만) */
    public void SetDifficultySpeedMultiplier(float multiplier)
    {
        difficultySpeedMultiplier = multiplier;
        Debug.Log($"{gameObject.name} 난이도 속도 배율: {multiplier}배속");
    }

    /* 아이템에 의한 일시적 속도 배율 설정 */
    public void SetItemSpeedMultiplier(float multiplier)
    {
        itemSpeedMultiplier = multiplier;
    }
}
