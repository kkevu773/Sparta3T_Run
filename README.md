# 프로젝트 소개 
- **프로젝트명 :** AilenRun
- **프로젝트 설명 :** 쿠키런과 같은 2D 횡스크롤 러닝 게임


# 역할 분담
- **김상혁 :** Player 움직임
- **조아라 :** 게임 시스템 및 git관리
- **박영재 :** 시청각 매니저
- **정성재 :** 배경움직임
- **함승효 :** 몬스터 리스폰


# 와이어 프레임
<img width="3664" height="1784" alt="W5Team3WireFrame" src="https://github.com/user-attachments/assets/d0265251-1230-46ba-ad1e-214a5dd88ce6" />

# 구현 기능
- **캐릭터 자동 이동** 
    - 캐릭터가 일정한 속도로 자동 전진
    - 속도는 난이도에 따라 점진적으로 증가
- **장애물과 아이템**
    - 점프(스페이스)와 슬라이드(Shift)로 회피 가능한 장애물 추가
    - 아이템 종류
        - 점수 아이템(코인 등)
        - 속도 증가/감소 아이템
        - 체력 회복 아이템
- **속도 증가와 난이도 조절**
    - 일정 시간마다 장애물 빈도와 속도 증가
    - 난이도 조정에 따라 플레이어 반응 속도 요구
- **UI 시스템**
    - 화면 상단에 현재 점수, 최고 점수, 남은 체력 표시
    - 게임 오버 화면과 재시작 버튼 추가
- **게임 오버와 재시작**
    - 장애물에 부딪히거나 체력이 소진되면 게임 종료
    - 플레이어가 점수를 확인하고 다시 시작 가능

 # 구현 목록
## [김상혁]

## [조아라]
- `GameManager.cs`
  - 게임 흐름 관리
    <img width="1421" height="351" alt="W5 팀플 클래스 구조도 drawio (4)" src="https://github.com/user-attachments/assets/64c7ea52-eafb-4d0c-a400-211072b89f92" />

  - 난이도 시스템 : `GameManager` 내에서 `enum` 으로 관리
    ```cs
    public enum Difficulty
    {
        Easy,       // 쉬움 (0.8배속)
        Normal,     // 보통 (1배속)
        Hard        // 어려움 (1.2배속)
    }
    ```
    
  - 각각의 오브젝트 속도 제어
    ```cs
    
    ```
    
  - 플레이 기록 저장
    ```cs
    
    ```
> [GameManager.cs](https://github.com/kkevu773/Sparta3T_Run/blob/main/Sparta_Run3T/Assets/Scripts/Managers/GameManager.cs)

## [함승효]

## [정성재]
- 계속되는 루프형식의 배경
<img width="162" height="123" alt="스크린샷 2025-11-04 164316" src="https://github.com/user-attachments/assets/504a6f4a-6523-485e-85a0-ab33ecf71558" />
- 배경 전환 시 FadeON / Out 활용 

## [박영재]

- 시청각을 관장하는 3개의 매니저 구상
> <img width="441" height="162" alt="매니저들 차이 drawio" src="https://github.com/user-attachments/assets/a1a2ed79-02be-401a-98b4-4bbe209d0cfe" />
- 중앙 관리자인 GameManager의 구조를 유지하면서 설계
> <img width="611" height="267" alt="단방향 싱글톤 drawio" src="https://github.com/user-attachments/assets/939d22e0-232e-4ca6-93c2-dc94d6ecbbac" />

- ## 오디오 매니저 

- ###  개요
`AudioManager`는 **배경음(BGM)**과 **효과음(SFX)**을 모두 통합 관리하는 중앙 사운드 매니저입니다.  
모든 오디오 재생은 이 스크립트를 거치며, 다른 스크립트에서는 직접 `AudioSource`를 제어하지 않습니다.

---

- ###  구조
| 구성 요소 | 설명 |
|------------|------|
| `SoundPair` | `SoundKey`와 `AudioSource`를 묶은 구조체. Inspector에서 연결함 |
| `soundDic` | Enum 키 기반으로 빠르게 사운드를 찾아 재생하기 위한 Dictionary |
| `Play()` | 효과음(SFX) 재생용. 내부적으로 `PlayOneShot` 사용 |
| `PlayBGM()` | 배경음(BGM) 전용. 루프 재생 및 중복 방지 기능 포함 |
| `SetBGMVolume()` / `SetSFXVolume()` | 슬라이더 연동으로 실시간 볼륨 조절 |
| `StopAllBGM()` | 모든 배경음 정지 (씬 전환 시 사용) |

---

- ###  핵심 코드
```csharp
if (soundDic.TryGetValue(key, out AudioSource src))
{
    src.PlayOneShot(src.clip, src.volume);
}
```
BGM은 루프, SFX는 PlayOneShot으로 단발 재생하되, SFX에도 실시간으로 볼륨 조절이 반영되도록 직접 전달.

> [AudioManager.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Audio/AudioManager.cs)  
> [SoundInstace.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Audio/SoundInstace.cs)  
> [SoundKey.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Audio/SoundKey.cs)

- ##  이펙트 매니저

- ###  개요
`EffectManager`는 **이펙트(파티클, 시각 효과)**를 통합 관리하는 매니저입니다.  
플레이어 동작, 아이템 획득, 충돌 이벤트 등에서 발생하는 비주얼 효과를  
Enum 기반으로 요청받아 자동으로 생성·제거합니다.

---

- ###  구조
| 구성 요소 | 설명 |
|------------|------|
| `EffectPair` | `EffectKey`와 프리팹을 연결한 구조체. Inspector에서 설정 |
| `effectDic` | Enum 키를 기준으로 프리팹을 빠르게 탐색하는 Dictionary |
| `Play()` | 일반 이펙트 재생. 위치 지정 후 자동 파괴(`EffectInstance` 내부 타이머) |
| `PlayUI()` | UI 전용 이펙트 (Canvas 자식으로 붙음) |
| `PlayLoop()` | 루프형 이펙트. 외부에서 수동으로 Stop 호출 필요 |
| `Stop()` | 루프 이펙트를 비활성화하거나 제거할 때 사용 |

---

- ###  핵심 코드
```csharp
if (effectDic.TryGetValue(key, out GameObject prefab))
{
    GameObject effect = Instantiate(prefab, pos, Quaternion.identity);
}
```
`EffectManager`는 `Enum` 키로 프리펩을 찾아 자동 생성 후 `EffectInstance`로 자동 파괴한다.
> [EffectInstance.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectInstance.cs)  
> [EffectKey.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectKey.cs)  
> [EffectManager.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectManager.cs)  
> [EffectPair.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectPair.cs)

- ## UI매니저

- ###  개요
`UIManager`는 게임 내 모든 **UI 요소(HUD, 설정창, 결과창, 난이도 선택창)**를 관리하는 핵심 매니저입니다.  
버튼, 슬라이더, 텍스트를 한 곳에서 제어하며,  
오디오와 게임 상태(GameManager) 흐름을 연결하는 **시청각 인터페이스의 허브 역할**을 합니다.

---

- ###  구조
| 구성 요소 | 설명 |
|------------|------|
| `UIPair` | `UIKey`와 GameObject를 묶은 구조체. Inspector에서 직접 등록 |
| `uiDic` | UIKey 기준으로 GameObject를 빠르게 찾는 Dictionary |
| `ShowUI()` | 개별 UI 표시/숨김 제어 |
| `ToggleSet()` / `CloseSet()` | 설정창 열기/닫기 및 일시정지 관리 (`Time.timeScale` 제어 포함) |
| `ShowGameOver()` | 점수 표시, 최고 기록 비교 및 결과창 활성화 |
| `UpdateScore()` / `UpdateBestScore()` / `UpdateHP()` | HUD 실시간 갱신 |
| `sliderBGM`, `sliderSFX` | 오디오 볼륨 제어 슬라이더 (AudioManager와 직접 연동) |
| `ShowCountdown()` / `HideCountdown()` | 카운트다운 텍스트 표시 및 숨김 |
| `ShowDifficultyPanel()` / `SelectDifficulty()` | 난이도 선택 메뉴 관리 (GameManager 연동) |

---

- ###  핵심 코드
```csharp
sliderBGM.onValueChanged.AddListener(BGMChange);
sliderSFX.onValueChanged.AddListener(SFXChange);

private void BGMChange(float value)
{
    AudioManager.Instance.SetBGMVolume(value);
}

private void SFXChange(float value)
{
    AudioManager.Instance.SetSFXVolume(value);
}
```
오디오매니저와 게임매니저와 직접적으로 연결이 되어 있어서 HUD, 게임 오버 UI등을 제어한다.

> [UIKey.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/UI/UIKey.cs)  
> [UIManager.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/UI/UIManager.cs)  
> [UIPair.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/UI/UIPair.cs)

- ## 목적지

- ###  개요
`Goal` 스크립트는 플레이어가 **도착 지점(Goal 오브젝트)**에 닿았을 때  
게임 클리어 상태를 GameManager로 전달하는 역할을 합니다.  
즉, **게임 오버(실패)**와 반대되는 **게임 클리어(성공)** 트리거입니다.

---

- ###  구조
| 구성 요소 | 설명 |
|------------|------|
| `OnTriggerEnter2D()` | 플레이어가 Goal 영역에 진입했을 때 호출 |
| `AudioManager.Instance.Play()` | 도착 시 효과음 재생 |
| `GameManager.Instance.GameClear()` | GameManager에 클리어 상태 보고 |

---

- ###  핵심 코드
```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        Debug.Log("Goal");
        AudioManager.Instance.Play(SoundKey.SFX_UI_GAMEOVER);
        GameManager.Instance.GameClear();
    }
}
```
플레이어의 충돌 이벤트를 감지하여 `GameManager`에게 게임 클리어 상태를 전달한다.
> [Goal.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Goal.cs)


 # 예시 이미지 / 플레이 이미지
  https://kenney.nl/assets 에셋스토어 활용
  <img width="918" height="515" alt="Sample A" src="https://github.com/user-attachments/assets/75a77e6f-9d54-497b-ba42-8fedebc066a6" />

  
> <img width="1920" height="1080" alt="게임 시연 mp4_20251104_151933 709" src="https://github.com/user-attachments/assets/bec5bbae-fbf1-4e16-bd1a-57a8847da2fc" />
> <img width="1920" height="1080" alt="게임 시연 mp4_20251104_151920 549" src="https://github.com/user-attachments/assets/6fe6b06a-2fa8-4de4-874f-e7a3a28ef573" />
> <img width="1920" height="1080" alt="게임 시연 mp4_20251104_151911 900" src="https://github.com/user-attachments/assets/b2d030b3-0a85-439a-b0a6-fcd50239efb1" />
> <img width="1920" height="1080" alt="게임 시연 mp4_20251104_151904 995" src="https://github.com/user-attachments/assets/ede20f11-e8e0-4677-94c5-3a8136b112d4" />



  

