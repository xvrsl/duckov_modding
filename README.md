# Duckov Modding 示例

_《逃离鸭科夫》的 mod 示例说明。_

中文 | [English](README_EN.md) | [한국어](README_KO.md)

[值得注意的 API](Documents/NotableAPIs_CN.md)

## 关于Harmony

游戏目前没有内置 Harmony。观察到同时加载不同版本 Harmony 时会有冲突。可以参考社区中比较流行的 Harmony 来进行开发，比如撰写此文时最新的2.4.1版本。

## 工作原理概述

《逃离鸭科夫》的 Mod 系统会自动扫描并读取 Duckov_Data/Mods 文件夹，以及从 Steam 创意工坊中订阅的物品文件夹。当扫描发现这些文件夹中包含有特定的 dll 文件、info.ini 和 preview.png 文件，那么就能够在游戏的 Mods 菜单中管理并加载 mod。
注意：mac系统的相关文件夹位于 Duckov/Duckov.app/**Contents/Mods/**。

### 规则一
游戏会读取 mod 文件夹的 info.ini 中的 **name** 参数，并以此作为 namespace 尝试加载名为 ModBehaviour 的类。例如，info.ini 中存放有`name=MyMod`，则会加载`MyMod.dll`文件中的`MyMod.ModBehaviour`。

### 规则二
mod 的 ModBehaviour 类应继承自 **Duckov.Modding.ModBehaviour**，这是一个继承自 Unity 的 MonoBehaivour 的类，其中包含了一些 mod 系统中需要使用的额外功能。在加载 mod 时，《逃离鸭科夫》会创建一个该 mod 的 GameObject，并通过调用 GameObject.AddComponent(Type) 的方式创建一个 ModBehaviour 的实例。Mod 作者可以通过编写 ModBehaviour 的 Start\Update 等 Unity 事件实现自定义功能，也可以通过注册《逃离鸭科夫》中的其他事件实现自定义功能。

## Mod 文件夹结构

将准备好的 Mod 文件夹放到《逃离鸭科夫》本体的 Duckov_Data/Mods 中，即可在游戏主界面的 Mods 界面加载该 Mod。
以上面的 “MyMod” 为例，发布的文件结构应该如下：

- MyMod（mod 的文件夹）
  - MyMod.dll（mod的自定义的功能逻辑）
  - info.ini（重要，mod的配置文件）
  - preview.png（正方形的预览图，建议分辨率 `256*256`）

[Mod 文件夹示例](DisplayItemValue/ReleaseExample/DisplayItemValue/)

### info.ini 详情

本配置文件应包含以下参数（不建议存放其他的额外数据）：

- name（mod 名称，主要用于加载 dll 文件。详情见上面的 _规则一_）
- displayName（显示的名称）
- description（显示的描述）
- publishedFileId（_可能包含_。本 Mod 在 steam 创意工坊的 id）
- tags (在创意工坊中显示的Tag, 使用逗号分隔)

**注意：在上传 Steam Workshop 的时候，会复写 info.ini。配置文件中部分数据可能会因此丢失。所以不建议在 info.ini 中存储除以上项目之外的其他信息。**

#### Tags可以使用的参数

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
- 
## 配置 C# 工程

**注意：在上传 Steam Workshop 的时候，会复写 info.ini。info.ini 中原有的信息可能会因此丢失。所以不建议在 info.ini 中存储除以上项目之外的其他信息。**

## 配置 C# 工程 / Configuring C# Project

1. 在电脑上准备好《逃离鸭科夫》本体。
2. 通过 Visual Studio 软件创建一个 .Net 类库（Class Library）。
3. 配置工程参数。
   1) 框架（Target Framework）
      - **TargetFramework 建议设置为 .Net Standard 2.1**。
      - 注意删除 TargetFramework 不支持的功能，比如`<ImplicitUsings>`
   2) 添加引用（Reference Include）
      - 将《逃离鸭科夫》的`\Duckov_Data\Managed\*.dll`添加到引用中。
      - 示例：
      ```
        <ItemGroup>
          <Reference Include="$(DuckovPath)\Duckov_Data\Managed\TeamSoda.*" />
          <Reference Include="$(DuckovPath)\Duckov_Data\Managed\ItemStatsSystem.dll" />
          <Reference Include="$(DuckovPath)\Duckov_Data\Managed\Unity*" />
        </ItemGroup>
      ```

4. 工程配置完成！现在在你 Mod 工程的 Namespace 中编写一个 ModBehaivour 的类。
5. 构建工程，即可得到你的 mod 的主要 dll。然后按照上述规则说明整理好文件夹结构，即可开始本地测试。

[csproj 文件示例](DisplayItemValue/DisplayItemValue.csproj)

注意：如果碰到 .csproj 下的文件路径无法被识别问题时，可用第三方IDE（如 VS Code）打开文件，将文件编码从 UTF-8 with BOM 改为 UTF-8（无 BOM）并重新保存，再用 Visual Studio 打开就应该正常了。

## 其他

### Unity Package

使用 Unity 进行开发时，可以参考本仓库附带的 [manifest.json 文件](UnityFiles/manifest.json) 来选择 package。

### 自定义游戏物品

```
// 添加自定义物品
ItemStatsSystem.ItemAssetsCollection.AddDynamicEntry(Item prefab)

// 移除自定义物品
ItemStatsSystem.ItemAssetsCollection.RemoveDynamicEntry(Item prefab)
```

- 自定义物品的 prefab 上需要配置好 TypeID。该 ID 应避免与游戏本体、其他 MOD 冲突。
- 进入游戏时如果未加载对应 MOD，存档中的自定义物品会直接消失。

### 本地化

```
// 覆盖本地化文本
SodaCraft.Localizations.LocalizationManager.SetOverrideText(string key, string value)

// 处理语言切换时的逻辑
SodaCraft.Localizations.LocalizationManager.OnSetLanguage:System.Action<SystemLanguage>
```

## 鸭科夫社区准则

为了鸭科夫社区的长期健康与和谐发展，我们需要共同维护良好的创作环境。 因此，我们希望大家遵守以下规则：

1. 禁止违反开发组以及 Steam平台所在地区法律的内容，禁止涉及政治、散布淫秽色情、宣扬暴力恐怖的内容。
2. 禁止对玩家正常游戏流程、存档安全或社区秩序存在损害的行为，或其他形式的恶意代码。
3. 禁止严重侮辱角色或者扭曲剧情、意图在玩家社群内容引起反感和制造对立的内容，或者涉及到热门时事与现实人物等容易引发现实争议的内容。
4. 禁止未经授权，使用受版权保护的游戏资源或其他第三方素材的内容。
5. 禁止利用 Mod 引导至广告、募捐等商业或非官方性质的外部链接，或引导他人付费的行为。
6. 使用AI内容的 Mod 需要标注。

对于在 Steam创意工坊发布的 Mod，如果违反上述规则，我们可能会在不事先通知的情况下直接删除，并可能封禁相关创作者的权限。
