# 🎮 Unity 로그라이크 액션 RPG 개발 리포트

## 📌 프로젝트 개요

- **장르**: 2D 로그라이크 액션 RPG  
- **엔진**: Unity (2D 기반)  
- **플랫폼**: PC  
- **참고 게임**: MapleStory, Skull  
- **특징**
  - 히트박스 기반 정밀 전투
  - 실시간 스킬/대시/잔상 시스템
  - 플레이어 성장(레벨, 스탯)
  - 직관적인 UI와 연동된 전투 흐름

---

## 🧍‍♂️ 플레이어 시스템

### ▶ 기본 이동 & 점프
- Rigidbody2D 기반 물리 이동
- 방향키 + 점프(Space)로 기본 이동
- `LeftShift` → 방향 대시 구현
- `AfterImage` 오브젝트로 잔상 효과 제공

### ▶ 스킬 시스템 (`Q`)
- 마나 소모 + 쿨타임 체크 → 스킬 발동
- 스킬 히트박스 프리팹 (`SkillHitbox`) 생성
- 캐릭터 위로 점프 + 잔상 지속 출력

---

## ⚔️ 전투 시스템

### ▶ 기본 공격 (`Z`)
- 3단 콤보 애니메이션 공격
- `Hitbox.cs`를 통해 적에게 데미지 전달
- 치명타 판정 및 데미지 배율 계산 포함

### ▶ 적의 공격
- `EnemyAttack.cs` → 일정 간격으로 `Player`에 데미지
- 쿨타임 체크, `IDamageable` 인터페이스 기반

### ▶ 데미지 출력
- `DamageTextSpawner` → 적 머리 위 숫자 텍스트
- 크리티컬 여부에 따라 스타일 분리 가능

---

## 🧠 스탯 및 레벨 시스템

### ▶ 스탯 관리
- `PlayerStat.cs`
  - **STR** → 공격력 증가
  - **DEX** → 공격력 + 명중치
  - **CRIT** → 크리티컬 확률

### ▶ 레벨업 (`EXP`)
- `PlayerLevel.cs`:
  - 몬스터 처치 시 경험치 획득
  - `레벨업` 시 스탯 자동 증가
  - `baseExp` * 성장계수 기반 곡선형 성장

```csharp
public int GetRequiredExp(int level)
{
    return Mathf.RoundToInt(baseExp * Mathf.Pow(expGrowthFactor, level - 1));
}
```

# 🖥️ UI 시스템
## ▶ Player 상태 UI
- PlayerUI.cs: HP/MP/EXP 슬라이더 실시간 업데이트

- FollowPlayerUI.cs: 화면 내 캐릭터 하단에 고정 UI 표시

## ▶ 스탯 패널 (S 키)
- StatPanelToggle.cs: S 키로 패널 ON/OFF

- StatUI.cs: 텍스트로 STR/DEX/CRIT 시각화

- 시각화

# 👹 몬스터 시스템
Enemy.cs:

- chaseRange 내 접근 시 추적

- 사망 시 애니메이션 → 골드 생성

- 경험치 및 퀘스트 진행 연동

```
PlayerLevel.instance?.AddExp(expReward);
QuestManager.instance?.UpdateQuestProgress(gameObject.name);
```
🧩 기타 시스템
▶ 스폰 시스템
- PlayerSpawnManager.cs

- `PlayerPrefs`에서 스폰 위치 불러오기

- `Rigidbody2D` 물리 제어 후 정확한 위치 이동

▶ 인터페이스 구조화
- ```IDamageable``` 인터페이스:

- 모든 피해 처리 객체에 TakeDamage() 통일 적용

# 🧱 향후 개발 계획
 ### 기본 전투 시스템 구현

 ### 스킬 및 대시 잔상 효과 연출

 ### 경험치/레벨/스탯 시스템 완성

 ### 장비 & 인벤토리 시스템 연동

 ### 던전 & 보스 전용 시스템 추가

 ### 세이브/로드 기능 개발(완료)

 ### 배경 맵 랜덤 생성기능

# 결론
이 프로젝트는 Unity를 활용하여 로그라이크 액션 RPG의 핵심 시스템을 대부분 구현하였다. 특히 전투, 성장, UI, 스킬 요소를 직접 설계 및 최적화하면서 구조적 설계와 모듈화에 중점을 두었다.

향후 개발 및 확장을 위한 기반이 충분히 마련된 상태이다.

