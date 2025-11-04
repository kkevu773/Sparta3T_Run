# Sparta3T_Run
쿠키런 모방 게임

<역할 분담>
김상혁: Player 움직임
조아라: 게임시스템 및 git관리
박영재: 시청각 매니저
정성재: 배경움직임
함승효: 몬스터 리스폰

<와이어 프레임>
<img width="3664" height="1784" alt="W5Team3WireFrame" src="https://github.com/user-attachments/assets/d0265251-1230-46ba-ad1e-214a5dd88ce6" />

<구현 기능>
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
 
  https://kenney.nl/assets 에셋스토어 활용
  <img width="918" height="515" alt="Sample A" src="https://github.com/user-attachments/assets/75a77e6f-9d54-497b-ba42-8fedebc066a6" />

