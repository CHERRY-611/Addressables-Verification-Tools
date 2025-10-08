# Addressables 성능 검증 및 에디터 도구 개발
</br>

## 1. 기술 개요
Unity 프로젝트를 진행하며 **Addressables 시스템**을 활용할 때,  
씬 전환이나 빌드 변경 시 **성능과 데이터 일관성**을 직접 검증할 수 있는 도구가 필요하다고 느꼈습니다.  

이에 따라 **Addressables의 메모리 사용량**과 **로드 시간**을 정량적으로 분석할 수 있는 테스트 도구를 제작했습니다.  
이 프로젝트는 **Addressables의 성능 검증 및 빌드 신뢰성 향상**을 목표로 하며,  
다음 두 가지 테스트를 진행했습니다.

- **Addressable 기능 테스트:** `HashChecker` — 빌드 해시 비교 및 라벨별 로드 테스트  
- **Editor Tool 개발:** `AddressableProfilerWindow` — 씬 로딩 및 메모리 사용량 분석  

</br>


## 2. 주요 기능

### (1) Addressable 기능 테스트
Addressables 빌드 결과(`buildLayout.json`)를 비교하여 **번들 해시 변경 여부를 탐지**하고,  
**UI 버튼으로 라벨별 에셋 로딩 테스트**를 수행하는 실험입니다.

**기능 요약**
- `BuildLayout.Open()`을 통해 `buildLayoutA/B.json` 로드  
- 동일 번들의 해시값 차이 탐지 시 **경고 로그 출력**  
- `"Essential"`, `"Extra"` 라벨 단위로 Addressable 자산 **스폰 테스트 가능**  
- 에디터 환경에서만 해시 비교 기능 노출 (`#if UNITY_EDITOR`)  
<p align="center">
<img width="940" height="113" alt="image" src="https://github.com/user-attachments/assets/2cd0c1a3-ad89-4f24-8483-a971341573ce" />
</p>

</br>

### (2) Editor Tool 개발
Addressables **씬 로딩 및 언로딩 시의 메모리 변화량과 로드 시간**을 확인하는 **에디터 도구**입니다.

**기능 요약**
- 현재 씬과 다음 씬 이름을 입력하여 **전환 테스트 수행**  
- 전환 과정에서 **GC / Unity 메모리 측정**  
- **로드 전 → 언로드 후 → 로드 후** 단계별 메모리 로그 출력  
- `Addressables.LoadSceneAsync` / `UnloadSceneAsync` 사용  

**활용 예시**
- Addressable **Scene 단위의 메모리 누수 탐지**  
- **최적화 전후의 씬 로드 속도 비교**  
- **GC 발생량 및 Unity Allocated 메모리 추적**  
<p align="center">
<img width="408" height="255" alt="image" src="https://github.com/user-attachments/assets/b685011b-c087-49f3-bbe0-79eb3b0ebd73" />
<img width="431" height="255" alt="image" src="https://github.com/user-attachments/assets/911a13ca-85ff-4ae0-afc4-a6c5dd8ceb2f" />
</p>

</br>

## 4. 활용 가능성
- **팀 단위 Addressables 관리 툴**로 확장 가능  
- **경량화된 Addressables 프로파일러**로서 Unity 기본 Profiler 보완  

</br>

## 5. 추가 아이디어
- `AddressableProfilerWindow` 결과를 **CSV로 내보내어 메모리 변화 기록**  
- `HashChecker`에 **번들 크기 비교 기능 추가**  
