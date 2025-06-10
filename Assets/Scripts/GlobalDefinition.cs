using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region ==========Enums==========
public enum ItemID
{
    //Item
    ITEM_NULL = 0x0000,

    //Tool
    TOOL_NULL = 0x1000,
    TOOL_NORMAL_AXE = 0x1010,
    TOOL_NORMAL_HOE = 0x1020,
    TOOL_NORMAL_WATERINGCAN = 0x1030,

    //Seed
    SEED_NULL = 0x2000,
    SEED_CARROT = 0x2001,
    SEED_EGGPLANT = 0x2002,
    SEED_TOMATO = 0x2003,
    SEED_CORN = 0x2004,
    SEED_PUMPKIN = 0x2005,

    //Plant
    PLANT_NULL = 0x3000,
    PLANT_CARROT = 0x3001,
    PLANT_EGGPLANT = 0x3002,
    PLANT_TOMATO = 0x3003,
    PLANT_CORN = 0x3004,
    PLANT_PUMPKIN = 0x3005,

    //Crop
    CROP_NULL = 0x4000,
    CROP_CARROT = 0x4001,
    CROP_EGGPLANT = 0x4002,
    CROP_TOMATO = 0x4003,
    CROP_CORN = 0x4004,
    CROP_PUMPKIN = 0x4005,

    //Fruits
    FRUIT_NULL = 0x5000,
    FRUIT_APPLE = 0x5001,
    FRUIT_ORANGE = 0x5002,
    FRUIT_PEACH = 0x5003,
    FRUIT_PEAR = 0x5004,

    //Spawn Egg
    SPAWN_EGG_NULL = 0x6000,
    SPAWN_EGG_CHICKEN = 0x6001,
    SPAWN_EGG_COW = 0x6002,

    //Egg
    EGG_NULL = 0x7000,
    EGG_NORMAL = 0x7001,

    //Milk
    MILK_NULL = 0x7100,
    MILK_NORMAL = 0x7101,

    //Prop
    PROP_NULL = 0x8000,
    PROP_STORE_ITEM = 0x8010,
    PROP_STORE_OBJ = 0x8011,

    //Animal
    ANIMAL_NULL = 0x9000,
    ANIMAL_CHICKEN_BASIC = 0x9010,
    ANIMAL_COW_BASIC = 0x9020,
    ANIMAL_MOLE = 0x9030,

    //Land
    LAND_NULL = 0xA000,
    LAND_LIGHT_GRASS = 0xA001,

    //Tree
    TREE_NULL = 0xB000,
    TREE_DEFAULT = 0xB001,
    TREE_APPLE = 0xB002,
    TREE_ORANGE = 0xB003,
    TREE_PEACH = 0xB004,
    TREE_PEAR = 0xB005,

    //Wood
    WOOD_NULL = 0xC000,
    WOOD_LOG = 0xC001,
    WOOD_BRANCH = 0xC002,
    WOOD_STICK = 0xC003,
    WOOD_PLANK = 0xC004,
}

public enum BGMID
{
    Title, 
    Day0,
    Day1,
    Day2,
    Noon0,
    Noon1,
    Noon2,
    Night0,
    Night1,
    Night2,
    Store,
    MISC
}

public enum SFXID
{
    //Tool
    Axe,
    Hoe,
    WateringCan,

    //Item
    GetItem,
    PlantSeed,
    EatCrop,

    //Props
    PlaceProp,
    RemoveProp,

    //Animal
    Chicken0, Chicken1, Chicken2,
    Cow0, Cow1, Cow2,

    //UI
    ButtonClick,
    OnSliderMove,
    Purchase,
    SellItem,
}

public enum AchievementID
{
    //Golds
    EarnedTotalGolds,
    EarnedDailyGolds,
    EarnedFarmingGolds,
    EarnedAnimalGolds,
    EarnedMiscGolds,
    CurrentGolds,
    SpentGolds,
    //Crop
    HarvestedTotalCrop,
    HarvestedCarrot,
    HarvestedEggplant,
    //Tool
    UsedTool,
    UsedAxe,
    UsedHoe,
    UsedWateringCan,
    FilledWateringCan,
}

public enum ActionID
{
    //Tool
    UseTool = 0x0100,
    CutTree = 0x0101,
    Plow = 0x0102,
    Watering = 0x0103,

    //Plant
    Plant = 0x0200,
    PlantCarrot = 0x0201,
    PlantEggplant = 0x0202,
    PlantTomato = 0x0203,
    PlantCorn = 0x0204,
    PlantPumpkin = 0x0205,

    //Harvest
    Harvest = 0x0300,
    HarvestCarrot = 0x0301,
    HarvestEggplant = 0x0302,
    HarvestTomato = 0x0303,
    HarvestCorn = 0x0304,
    HarvestPumpkin = 0x0305,

    //Animal
    FeedAnimal = 0x0400,
    FeedChicken = 0x0401,
    FeedCow = 0x0402,

    GetAnimalProduct = 0x0500,
    GetEgg = 0x0501,
    GetMilk = 0x0502,
}

public enum QuestState
{
    NotRead,
    Read,
    Clear
}

public enum ToolType
{
    Hoe, Axe, WateringCan,
}

public enum GameState
{
    Title, Playing, Pause, Exit
}

public enum Timezone
{
    Morning, Noon, Night
}

public enum Weather
{
    Clear, Rain
}

public enum InGamePanelType
{
    Option, Store, Inventory, DailyResult, Quest
}
#endregion

#region ==========Interfaces==========
public interface ISellable
{
    public int Price { get; }
    public void Sell();
}

public interface IUsableItem
{
    public void UseItem(Vector2Int position, KeyCode key);
}

public interface IAmount
{
    public int Amount { get; set; }
}

public interface IDurability
{
    public int MaxDurability { get; }
    public int Durability { get; }
}

public interface IGrade
{
    public int Grade { get; }
}

public interface IIneteractable
{
    public void Interact();
}

public interface IRemovable
{
    public void Remove();
}

public interface IPanel
{
    public bool Enabled { get; set; }
    public void OpenPanel();
    public void ClosePanel();
}

public interface IHover
{
    public void OnHover();
}

#endregion

#region ==========Structures==========
[Serializable]
public struct ItemData
{
    public ItemID itemID;
    public int value1;
    public int value2;

    public ItemData(ItemID itemID, int value1, int value2 = 0)
    {
        this.itemID = itemID;
        this.value1 = value1;
        this.value2 = value2;
    }
}

[Serializable]
public struct PlayerData
{
    //Player's data
    public Vector3 position;
    public ItemData[] quickslot;
    public ItemData[] inventory;
    public int health;
    public int stamina;
    public int golds;

    public PlayerData(Player player)
    {
        position = player.transform.position;
        quickslot = new ItemData[9];
        for (int i = 0; i < 9; i++)
        {
            quickslot[i] = player.Quickslot[i] == null ?
                new ItemData(ItemID.ITEM_NULL, 0) : player.Quickslot[i].Data;
        }
        inventory = new ItemData[36];
        for(int i = 0; i < 36; i++)
        {
            inventory[i] = player.Inventory[i / 9, i % 9] == null ?
                new ItemData(ItemID.ITEM_NULL, 0) : player.Inventory[i / 9, i % 9].Data;
        }
        health = player.Health;
        stamina = player.Stamina;
        golds = player.Golds;
    }
}

[Serializable]
public struct ItemPositionData
{
    public ItemData itemData;
    public Vector3 position;

    public ItemPositionData(GameObject obj)
    {
        if (obj.TryGetComponent<Item>(out Item item))
        {
            itemData = item.Data;
        }
        else itemData = new(ItemID.ITEM_NULL, 0);
        position = obj.transform.position;
    }
}

[Serializable]
public struct SceneData
{
    public ItemPositionData[] objects;
    //TODO: Add tile's data
    public SceneData(GameObject[] items)
    {
        objects = new ItemPositionData[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            objects[i] = new(items[i]);
        }
    }
}

[Serializable]
public struct SystemData
{
    public int curDay;

    public SystemData(int curDay)
    {
        this.curDay = curDay;
    }
}

[Serializable]
public struct SaveData
{
    public PlayerData playerData;
    public SceneData sceneData;
    public SystemData systemData;

    public SaveData(PlayerData playerData, SceneData sceneData, SystemData systemData)
    {
        this.playerData = playerData;
        this.sceneData = sceneData;
        this.systemData = systemData;
    }
}

[Serializable]
public struct KVPair<TKey, TValue>
{
    public TKey Key { get => key; set => key = value; }
    public TValue Value { get => this.value; set => this.value = value; }

    [SerializeField] private TKey key;
    [SerializeField] private TValue value;

    public KVPair(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}

[Serializable]
public struct Dict<TKey, TValue> : IEnumerable
{
    public TKey[] Keys => keys.ToArray();
    public TValue[] Values => values.ToArray();
    public int Count => list.Count;
    public TValue this[TKey key] => GetValue(key);


    [SerializeField] private List<KVPair<TKey, TValue>> list;
    private List<TKey> keys;
    private List<TValue> values;

    public void Add(TKey key, TValue value)
    {
        if (list == null) list = new List<KVPair<TKey, TValue>>();
        list.Add(new KVPair<TKey, TValue>(key, value));
        keys.Add(key);
        values.Add(value);
    }

    public bool Remove(TKey key)
    {
        if (list == null || list.Count == 0) return false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Key.Equals(key))
            {
                list.RemoveAt(i);
                keys.RemoveAt(i);
                values.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    private TValue GetValue(TKey key)
    {
        if (list == null || list.Count == 0) throw new NullReferenceException();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Key.Equals(key))
            {
                return list[i].Value;
            }
        }
        throw new KeyNotFoundException($"Key '{key}' not found in the dictionary.");
    }

    public IEnumerator GetEnumerator()
    {
        if (list == null || list.Count == 0) yield break;
        foreach (KVPair<TKey, TValue> kvp in list)
        {
            yield return kvp;
        }
    }

    public Dict(int capacity = 20)
    {
        list = new(capacity);
        keys = new(capacity);
        values = new(capacity);
    }
}
#endregion