#  Unity Roguelike Action RPG

메이플스토리와 Skull에서 영감을 받은 2D 로그라이크 액션 RPG입니다.  
정밀한 히트박스 전투, 스탯 성장, 실시간 스킬 시스템을 직접 Unity로 구현한 프로젝트입니다.

![Game Demo](https://your-image-link-or-gif.gif)

---

## 포트폴리오 첨부
[아래의 링크에 접속하셔서 레포트를 확인해주세요 !]
https://www.notion.so/UNITY-RPG-REPORT-1f9d26a2e08b805b9b4de1440db706f4

##  주요 기능

###  플레이어 시스템
- 방향키 이동, 점프, 대시 (잔상 포함)
- 스킬 발동 시스템 (마나 및 쿨타임)
- 3연타 기본 공격 콤보

###  전투 시스템
- 히트박스 기반 피해 판정
- 크리티컬 및 데미지 출력
- 적 추적 AI 및 공격 판정

###  성장 시스템
- 레벨 업 → 스탯 증가 (STR, DEX, CRIT)
- 경험치 획득 및 EXP 게이지 표시

###  UI 시스템
- HP / MP / EXP 슬라이더
- 스탯 패널 토글 (S 키)
- 플레이어 따라다니는 UI (월드-스크린 변환)

###  몬스터 시스템
- 추적, 피격, 사망 → 골드 및 EXP 드랍
- 퀘스트 진행 연동

---

## 🖼 스크린샷

| 전투 시스템 | 스탯 창 UI | 스킬 연출 |
|-------------|------------|------------|
| ![](images/combat.png) | ![](images/stat.png) | ![](images/skill.gif) |

---

##  실행 방법

```bash
git clone https://github.com/privateeye/projectT.git
cd roguelike-rpg
# Unity에서 프로젝트 열기
```
- Unity 2021 이상 권장
- DOTween, TextMeshPro 사용

#  디렉토리 구조
```
Assets/
├── Script/
│   ├── Player/
│   ├── Enemy/
│   ├── UI/
│   └── Common/
├── Prefabs/
├── Animations/
├── Sprites/
└── Scenes/
```

# 향후 개발 계획
- 기본 전투 시스템 구현(완료)

- 스킬 및 잔상 연출(스킬완료, 완료)

 - 레벨 & 스탯 시스템(완료)

 - 장비 시스템(완료)

 - 던전 보스 전투(완료)

 - 세이브 / 로드 기능(완료)

# 코드
코드확인은과 정확한 디렉토리 구조 확인은 github에서 확인 부탁드립니다.

# 작업방식
1. 제작자가 선 코드를 짜며, 최적화 및 리펙토링 작업을 AI에게 시켰습니다.
2. AI가 리펙토링 작업을 한 후 난 오류에 대해서는 제작자가 직접 수정하였습니다.
3. 하나의 구조를 구현 후 나오는 모든 버그를 수정 후, 다음 작업을 이행 하였습니다.
4. 
    제작자
김범수 (Unity,Unreal 개발자 / 게임 클라이언트 / C++,C#)

문의: makeurselfff@gmail.com
