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
## 김상혁

## 조아라
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

## 함승효

## 정성재

## 박영재

시청각을 관장하는 3개의 매니저 구상
<img width="441" height="162" alt="매니저들 차이 drawio" src="https://github.com/user-attachments/assets/a1a2ed79-02be-401a-98b4-4bbe209d0cfe" />
중앙 관리자인 GameManager의 구조를 유지하면서 설계

### 오디오 매니저 

## 🎧 AudioManager

> [AudioManager.cs](https://github.com/kkevu773/Sparta3T_Run/blob/main/Sparta_Run3T/Assets/Scripts/Managers/Audio/AudioManager.cs)

###  개요
`AudioManager`는 **배경음(BGM)**과 **효과음(SFX)**을 모두 통합 관리하는 중앙 사운드 매니저입니다.  
모든 오디오 재생은 이 스크립트를 거치며, 다른 스크립트에서는 직접 `AudioSource`를 제어하지 않습니다.

---

###  구조
| 구성 요소 | 설명 |
|------------|------|
| `SoundPair` | `SoundKey`와 `AudioSource`를 묶은 구조체. Inspector에서 연결함 |
| `soundDic` | Enum 키 기반으로 빠르게 사운드를 찾아 재생하기 위한 Dictionary |
| `Play()` | 효과음(SFX) 재생용. 내부적으로 `PlayOneShot` 사용 |
| `PlayBGM()` | 배경음(BGM) 전용. 루프 재생 및 중복 방지 기능 포함 |
| `SetBGMVolume()` / `SetSFXVolume()` | 슬라이더 연동으로 실시간 볼륨 조절 |
| `StopAllBGM()` | 모든 배경음 정지 (씬 전환 시 사용) |

---

###  핵심 코드
```csharp
if (soundDic.TryGetValue(key, out AudioSource src))
{
    src.PlayOneShot(src.clip, src.volume);  // 실시간 볼륨 반영
}
```


> [AudioManager.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Audio/AudioManager.cs)  
> [SoundInstace.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Audio/SoundInstace.cs)  
> [SoundKey.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Audio/SoundKey.cs)
> 
> [EffectInstance.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectInstance.cs)  
> [EffectKey.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectKey.cs)  
> [EffectManager.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectManager.cs)  
> [EffectPair.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Effects/EffectPair.cs)
> 
> [UIKey.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/UI/UIKey.cs)  
> [UIManager.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/UI/UIManager.cs)  
> [UIPair.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/UI/UIPair.cs)
> 
> [Goal.cs](https://raw.githubusercontent.com/kkevu773/Sparta3T_Run/refs/heads/main/Sparta_Run3T/Assets/Scripts/Managers/Goal.cs)


 # 예시 이미지 / 영상
  https://kenney.nl/assets 에셋스토어 활용
  <img width="918" height="515" alt="Sample A" src="https://github.com/user-attachments/assets/75a77e6f-9d54-497b-ba42-8fedebc066a6" />



  

