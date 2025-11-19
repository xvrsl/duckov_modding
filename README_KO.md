# Duckov 모딩 예제

_Escape From Duckov 모딩을 위한 예제 프로젝트입니다._

[中文](README.md) | [English](README_EN.md) | 한국어

[주요 API 문서](Documents/NotableAPIs.md)

## About Harmony

The game don't have Harmony library integrated. We noticed that different versions of Harmony library conflicts each other when used in different Mods. Please consider using the most used version of Harmony library in the community, probably the newest 2.4.1 release.

## 개요

Escape From Duckov의 모딩 시스템은 Duckov_Data/Mods 폴더의 하위 폴더와 Steam 창작마당에서 구독한 아이템 폴더를 스캔하고 읽습니다. 모드는 이러한 폴더에 포함된 `dll` 파일, `info.ini`, `preview.png`를 통해 게임에서 표시되고 로드됩니다.

Escape From Duckov는 info.ini의 name 매개변수를 읽어 네임스페이스로 사용하여 ModBehaviour라는 클래스를 로드합니다. 예를 들어, info.ini에 `name=MyMod`가 포함되어 있으면 `MyMod.dll` 파일에서 `MyMod.ModBehaviour`를 로드합니다.

ModBehaviour는 `Duckov.Modding.ModBehaviour`를 상속해야 합니다. `Duckov.Modding.ModBehaviour`는 MonoBehaviour를 상속하는 클래스이며 모드 시스템에 필요한 몇 가지 추가 기능을 포함합니다. 모드를 로드할 때 Escape From Duckov는 해당 모드에 대한 GameObject를 생성하고 GameObject.AddComponent(Type)을 호출하여 ModBehaviour의 인스턴스를 생성합니다. 모드 제작자는 ModBehaviour에서 Start\Update와 같은 Unity 이벤트를 작성하거나 Escape From Duckov의 다른 이벤트를 등록하여 기능을 구현할 수 있습니다.

## 모드 파일 구조

준비된 모드 폴더를 Escape From Duckov 설치 디렉토리 내의 `Duckov_Data/Mods`에 배치하면 게임 메인 메뉴의 모드 인터페이스에서 모드를 로드할 수 있습니다.
모드의 이름이 "MyMod"라고 가정하면 배포 파일 구조는 다음과 같아야 합니다:

- MyMod (폴더)
  - MyMod.dll
  - info.ini
  - preview.png (정사각형 미리보기 이미지, 권장 해상도 `256*256`)

[모드 폴더 예제](DisplayItemValue/ReleaseExample/DisplayItemValue/)

### info.ini

info.ini는 다음 매개변수를 포함해야 합니다:

- name (모드 이름, 주로 dll 파일 로드에 사용됨)
- displayName (표시 이름)
- description (표시 설명)

info.ini는 다음 매개변수를 포함할 수도 있습니다:

- publishedFileId (Steam 창작마당에서 이 모드의 ID를 기록)
- tags (steam workshop tags, separate with comma)

**참고: Steam 창작마당에 업로드할 때 info.ini가 덮어쓰여집니다. 그 결과 info.ini의 원래 정보가 손실될 수 있습니다. 따라서 위의 항목 이외의 정보를 info.ini에 저장하는 것은 권장하지 않습니다.**

#### Possible Tags
- Weapon
- Equipment & Gear
- Loot & Economy
- Quality of Life
- Cheats & Exploits
- Visual Enhancements
- Sound
- Quest & Progression
- Companion & NPC
- Collectibles
- Gameplay
- Multiplayer & Co-op
- Utility
- Medical & Survival

## C# 프로젝트 구성

1. 컴퓨터에 Escape From Duckov가 설치되어 있어야 합니다.
2. .NET 클래스 라이브러리 프로젝트를 생성합니다.
3. 프로젝트 매개변수를 구성합니다.

   1. 대상 프레임워크
      - **TargetFramework를 netstandard2.1로 설정하는 것을 권장합니다.**
      - 참고: `<ImplicitUsings>`와 같이 TargetFramework에서 지원하지 않는 기능을 제거하세요
   2. Reference Include

      - Escape From Duckov의 `\Duckov_Data\Managed\*.dll`을 참조에 추가합니다.
      - 예제:

      ```
      <ItemGroup>
        <Reference Include="$(DuckovPath)\Duckov_Data\Managed\TeamSoda.*" />
        <Reference Include="$(DuckovPath)\Duckov_Data\Managed\ItemStatsSystem.dll" />
        <Reference Include="$(DuckovPath)\Duckov_Data\Managed\Unity*" />
      </ItemGroup>
      ```

4. 완료! 이제 모드의 네임스페이스에 ModBehaviour 클래스를 작성하세요. 프로젝트를 빌드하면 모드의 메인 dll을 얻을 수 있습니다.

csproj 파일 예제: [DisplayItemValue.csproj](DisplayItemValue/DisplayItemValue.csproj)

## 기타

### Unity 패키지

Unity로 개발할 때 이 저장소에 포함된 [manifest.json 파일](UnityFiles/manifest.json)을 참조하여 패키지를 선택할 수 있습니다.

### 커스텀 게임 아이템

- `ItemStatsSystem.ItemAssetsCollection.AddDynamicEntry(Item prefab)`를 호출하여 커스텀 아이템을 추가합니다
- `ItemStatsSystem.ItemAssetsCollection.RemoveDynamicEntry(Item prefab)`를 호출하여 모드 아이템을 제거합니다
- 커스텀 아이템 프리팹은 TypeID가 올바르게 구성되어 있어야 합니다. 기본 게임 및 다른 MOD와의 충돌을 피하세요.
- 게임에 진입할 때 해당 MOD가 로드되지 않으면 저장 파일의 커스텀 아이템은 직접 사라집니다.

### 로컬라이제이션

- `SodaCraft.Localizations.LocalizationManager.SetOverrideText(string key, string value)`를 호출하여 표시되는 로컬라이제이션 텍스트를 재정의합니다.
- 언어를 전환할 때 로직을 처리하려면 `SodaCraft.Localizations.LocalizationManager.OnSetLanguage:System.Action<SystemLanguage>` 이벤트를 사용하세요

## Duckov 커뮤니티 규칙

Duckov 커뮤니티의 장기적인 발전을 돕기 위해 모든 분들께 긍정적인 창작 환경에 기여해 주실 것을 요청합니다. 다음 규칙을 준수해 주세요:

1. 개발팀과 Steam 플랫폼이 운영되는 지역의 법률을 위반하는 콘텐츠는 엄격히 금지됩니다. 여기에는 정치, 아동 성 착취, 폭력이나 테러 조장과 관련된 콘텐츠가 포함되지만 이에 국한되지 않습니다.
2. 캐릭터를 심각하게 모욕하거나, 스토리를 왜곡하거나, 플레이어 커뮤니티에 불편함, 갈등 또는 논란을 야기하는 것을 목표로 하는 콘텐츠는 금지됩니다. 여기에는 실제 분쟁을 유발할 수 있는 시사 문제나 실제 인물과 관련된 콘텐츠도 포함됩니다.
3. 저작권이 있는 게임 자산이나 기타 제3자 자료를 무단으로 사용하는 것은 금지됩니다.
4. 모드는 플레이어를 광고, 모금, 결제 요청 또는 기타 상업적 또는 비공식 외부 링크로 유도하는 데 사용해서는 안 됩니다.
5. AI 생성 콘텐츠를 포함하는 모드는 명확하게 표시되어야 합니다.
   Steam 창작마당에 게시된 모드의 경우 위 규칙을 위반하면 사전 통보 없이 제거될 수 있으며 제작자의 권한이 정지될 수 있습니다.

## 커뮤니티 기여

> **면책 조항:** 이 부분은 공식적으로 제공되지 않습니다. 사용자는 자신의 책임 하에 이 콘텐츠를 참조하고 사용해야 합니다.

### Ducky.Sdk - 현대적인 모드 개발 프레임워크

커뮤니티 개발자들이 [Ducky.Sdk](https://www.nuget.org/packages/Ducky.Sdk)를 만들었습니다. 이는 개발 프로세스를 크게 단순화하는 포괄적인 모드 개발 SDK입니다:

**주요 기능:**
- 🎯 **간소화된 구성**: 자동화된 프로젝트 구성 및 종속성 관리
- 🌍 **로컬라이제이션 지원**: CSV 및 Markdown 번역 파일을 지원하는 내장 다국어 로컬라이제이션 시스템
- 📦 **종속성 패키징**: 타사 NuGet 패키지 종속성 자동 처리
- 🔄 **자동 배포**: 빌드 후 게임 디렉토리에 자동 배포
- 🛠️ **코드 분석**: 내장 분석기로 일반적인 실수 방지

**빠른 시작:**

초급부터 고급까지의 완전한 예제가 포함된 [Ducky.Sdk 샘플 프로젝트](https://github.com/ducky7go/Ducky.Samples)를 참조하세요:

- **초급**: [Ducky.SingleProject](https://github.com/ducky7go/Ducky.Samples/tree/main/Ducky.SingleProject) - 가장 간단한 모드 구조
- **중급**: [Ducky.Localization](https://github.com/ducky7go/Ducky.Samples/tree/main/Ducky.Localization) - 다국어 로컬라이제이션 시스템
- **고급**: [Ducky.TryHarmony](https://github.com/ducky7go/Ducky.Samples/tree/main/Ducky.TryHarmony) - 런타임 코드 인젝션

```bash
# Ducky.Sdk로 프로젝트를 만들려면 다음만 필요합니다:
<PropertyGroup>
  <TargetFramework>netstandard2.1</TargetFramework>
  <ModName>YourModName</ModName>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="Ducky.Sdk" Version="*" />
</ItemGroup>
```

자세한 문서는 [Ducky.Samples 저장소](https://github.com/ducky7go/Ducky.Samples)를 참조하세요.