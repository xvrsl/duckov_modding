# 值得注意的API和实现细节

## 一、物品

### 物品生成

使用 ItemAssetsCollection 类里的函数来生成物品。

<b>接口</b>
```
//notable functions
public static async UniTask<Item> InstantiateAsync(int typeID)
public static Item InstantiateSync(int typeID) 
```

<b>示例</b>
```
using ItemStatsSystem;

...
// 生成一个 glick (Item #254)
Item glick = ItemAssetsCollection.InstantiateAsync(254);

// 操作生成的物品，比如把它送给玩家
ItemUtilities.SendToPlayer(glick);
...
```

### 物品工具类

使用 ItemUtilities 类里的一些函数来操作物品。比如送到玩家身上，以及其他一些操作。

<b>接口</b>
```
//notable functions

// 发送给玩家并存储
public static void SendToPlayer(Item item, bool dontMerge = false, bool sendToStorage = true)
public static bool SendToPlayerCharacter(Item item, bool dontMerge = false)
public static bool SendToPlayerCharacterInventory(Item item, bool dontMerge = false)

// 检查物品之间的关系
public static bool IsInPlayerCharacter(this Item item)
public static bool IsInPlayerStorage(this Item item)

// 试着把一个东西插到另一个插槽上
public static bool TryPlug(this Item main, Item part, bool emptyOnly = false,
                          Inventory backupInventory = null, int preferredFirstIndex = 0)
```

### 物品类

物品类定义在 ItemStatsSystem 命名空间下。

<b>接口</b>
```
//notable function definitions

// 使item脱离当前的东西，比如从槽位中移除、从 Inventory 中移除等。
public void Detach()
```

## 二、角色

### 角色控制
CharacterMainControl 是所有角色的核心组件。

<b>接口</b>
```
//notable function definitions

// 设置角色的阵营
public void SetTeam(Teams _team)
```

### 敌人生成

（待写）

## 三、对话

### 大对话

DoSubtitle 这个函数在1.0.29之前的版本是私有的，但是现在已经变为了公有。最新版本的本游戏中，可以调用它来展示对话框。
同时这是一个异步函数，所以调用时需要小心一些，多次调用会影响彼此。

<b>接口</b>
```
// 类名为 DialogueUI
public async UniTask DoSubtitle(SubtitlesRequestInfo info)
```

<b>示例</b>
```
using Dialogues;

...
NodeCanvas.DialogueTrees.SubtitlesRequestInfo content = new(...);
...
DialogueUI.instance.DoSubtitle(content);
...
```

### 气泡对话
调用本接口，可以生成一个汽包对话框。同样是异步函数。

<b>接口</b>
```
// 类名 Duckov.UI.DialogueBubbles.DialogueBubblesManager

//function definition
public static async UniTask Show(string text, Transform target, float yOffset = -1, bool needInteraction = false,
                                bool skippable = false,float speed=-1, float duration = 2f)
```

<b>示例</b>
```
using Duckov.UI.DialogueBubbles;

...
DialogueBubblesManager.Show("Hello world!", someGameObject.transform);
...
```
