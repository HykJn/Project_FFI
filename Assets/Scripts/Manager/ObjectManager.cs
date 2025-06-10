using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObjectManager : MonoBehaviour
{
    #region =========Singleton==========
    private static ObjectManager instance;
    public static ObjectManager Instance => instance;
    #endregion

    #region ==========Prefabs==========
    [Header("Prefabs")]
    //Tools
    [SerializeField] private GameObject prefab_Tool_Normal_Axe;
    [SerializeField] private GameObject prefab_Tool_Normal_Hoe;
    [SerializeField] private GameObject prefab_Tool_Normal_WateringCan;
    //Seeds
    [SerializeField] private GameObject prefab_Seed_Carrot;
    [SerializeField] private GameObject prefab_Seed_EggPlant;
    [SerializeField] private GameObject prefab_Seed_Corn;
    [SerializeField] private GameObject prefab_Seed_Pumpkin;
    [SerializeField] private GameObject prefab_Seed_Tomato;
    //Plants
    [SerializeField] private GameObject prefab_Plant_Carrot;
    [SerializeField] private GameObject prefab_Plant_EggPlant;
    [SerializeField] private GameObject prefab_Plant_Corn;
    [SerializeField] private GameObject prefab_Plant_Pumpkin;
    [SerializeField] private GameObject prefab_Plant_Tomato;
    //Crops
    [SerializeField] private GameObject prefab_Crop_Carrot;
    [SerializeField] private GameObject prefab_Crop_EggPlant;
    [SerializeField] private GameObject prefab_Crop_Corn;
    [SerializeField] private GameObject prefab_Crop_Pumpkin;
    [SerializeField] private GameObject prefab_Crop_Tomato;
    //Fruit
    [SerializeField] private GameObject prefab_Fruit_Apple;
    [SerializeField] private GameObject prefab_Fruit_Orange;
    [SerializeField] private GameObject prefab_Fruit_Peach;
    [SerializeField] private GameObject prefab_Fruit_Pear;
    //Spawn
    [SerializeField] private GameObject prefab_Spawn_Egg_Chicken;
    [SerializeField] private GameObject prefab_Spawn_Egg_Cow;
    //Eggs
    [SerializeField] private GameObject prefab_Egg_Normal;
    //Milk
    [SerializeField] private GameObject prefab_Milk_Normal;
    //Props
    [SerializeField] private GameObject prefab_Store_Item;
    [SerializeField] private GameObject prefab_Store_Obj;
    //Animals
    [SerializeField] private GameObject prefab_Animal_Basic_Chicken;
    [SerializeField] private GameObject prefab_Animal_Basic_Cow;
    [SerializeField] private GameObject prefab_Animal_Basic_Mole;
    //Lands
    [SerializeField] private GameObject prefab_Land_Light_Grass;
    //Tree
    [SerializeField] private GameObject prefab_Tree_Default;
    [SerializeField] private GameObject prefab_Tree_Apple;
    [SerializeField] private GameObject prefab_Tree_Orange;
    [SerializeField] private GameObject prefab_Tree_Peach;
    [SerializeField] private GameObject prefab_Tree_Pear;
    //Wood
    [SerializeField] private GameObject prefab_Wood_Log;
    [SerializeField] private GameObject prefab_Wood_Branch;
    [SerializeField] private GameObject prefab_Wood_Stick;
    [SerializeField] private GameObject prefab_Wood_Plank;

    private Transform activated;
    //Tools
    private Transform pool_Tool_Normal_Axe;
    private Transform pool_Tool_Normal_Hoe;
    private Transform pool_Tool_Normal_WateringCan;
    //Seeds
    private Transform pool_Seed_Carrot;
    private Transform pool_Seed_EggPlant;
    private Transform pool_Seed_Corn;
    private Transform pool_Seed_Pumpkin;
    private Transform pool_Seed_Tomato;
    //Plants
    private Transform pool_Plant_Carrot;
    private Transform pool_Plant_EggPlant;
    private Transform pool_Plant_Corn;
    private Transform pool_Plant_Pumpkin;
    private Transform pool_Plant_Tomato;
    //Crops
    private Transform pool_Crop_Carrot;
    private Transform pool_Crop_EggPlant;
    private Transform pool_Crop_Corn;
    private Transform pool_Crop_Pumpkin;
    private Transform pool_Crop_Tomato;
    //Fruit
    private Transform pool_Fruit_Apple;
    private Transform pool_Fruit_Orange;
    private Transform pool_Fruit_Peach;
    private Transform pool_Fruit_Pear;
    //Spawn Eggs
    private Transform pool_Spawn_Egg_Chicken;
    private Transform pool_Spawn_Egg_Cow;
    //Eggs
    private Transform pool_Egg_Normal;
    //Milk
    private Transform pool_Milk_Normal;
    //Props
    private Transform pool_Store_Item;
    private Transform pool_Store_Obj;
    //Animals
    private Transform pool_Animal_Basic_Chicken;
    private Transform pool_Animal_Basic_Cow;
    private Transform pool_Animal_Basic_Mole;
    //Lands
    private Transform pool_Land_Light_Grass;
    //Tree
    private Transform pool_Tree_Default;
    private Transform pool_Tree_Apple;
    private Transform pool_Tree_Orange;
    private Transform pool_Tree_Peach;
    private Transform pool_Tree_Pear;

    //Wood
    private Transform pool_Wood_Log;
    private Transform pool_Wood_Branch;
    private Transform pool_Wood_Stick;
    private Transform pool_Wood_Plank;
    #endregion

    #region ==========Pools==========
    //Tools
    private const ushort cnt_Tool_Normal_Axe = 128;
    private const ushort cnt_Tool_Normal_Hoe = 128;
    private const ushort cnt_Tool_Normal_WateringCan = 128;
    //Seeds
    private const ushort cnt_Seed_Carrot = 1024;
    private const ushort cnt_Seed_EggPlant = 1024;
    private const ushort cnt_Seed_Corn = 1024;
    private const ushort cnt_Seed_Pumpkin = 1024;
    private const ushort cnt_Seed_Tomato = 1024;
    //Plants
    private const ushort cnt_Plant_Carrot = 10;
    private const ushort cnt_Plant_EggPlant = 10;
    private const ushort cnt_Plant_Corn = 10;
    private const ushort cnt_Plant_Pumpkin = 10;
    private const ushort cnt_Plant_Tomato = 10;
    //Crops
    private const ushort cnt_Crop_Carrot = 1024;
    private const ushort cnt_Crop_EggPlant = 1024;
    private const ushort cnt_Crop_Corn = 1024;
    private const ushort cnt_Crop_Pumpkin = 1024;
    private const ushort cnt_Crop_Tomato = 1024;
    //Fruit
    private const ushort cnt_Fruit_Apple = 1024;
    private const ushort cnt_Fruit_Orange = 1024;
    private const ushort cnt_Fruit_Peach = 1024;
    private const ushort cnt_Fruit_Pear = 1024;
    //Spawn Eggs
    private const ushort cnt_Spawn_Egg_Chicken = 1024;
    private const ushort cnt_Spawn_Egg_Cow = 1024;
    //Eggs
    private const ushort cnt_Egg_Normal = 1024;
    //Milk
    private const ushort cnt_Milk_Normal = 1024;
    //Props
    private const ushort cnt_Store_Item = 32;
    private const ushort cnt_Store_Obj = 32;
    //Animals
    private const ushort cnt_Animal_Basic_Chicken = 64;
    private const ushort cnt_Animal_Basic_Cow = 64;
    private const ushort cnt_Animal_Basic_Mole = 32;
    //Lands
    private const ushort cnt_Land_Light_Grass = 1024;
    //Tree
    private const ushort cnt_Tree_Default = 10;
    private const ushort cnt_Tree_Apple = 10;
    private const ushort cnt_Tree_Orange = 10;
    private const ushort cnt_Tree_Peach = 10;
    private const ushort cnt_Tree_Pear = 10;
    //Wood
    private const ushort cnt_Wood_Log = 1024;
    private const ushort cnt_Wood_Branch = 1024;
    private const ushort cnt_Wood_Stick = 1024;
    private const ushort cnt_Wood_Plank = 1024;
    #endregion

    #region ==========Unity Methods==========
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        //}

        //private void Start()
        //{
        activated = new GameObject("Activated").transform;
        DontDestroyOnLoad(activated.gameObject);
        //Initialize pools
        Transform parent;
        {
            //Tools
            Transform p_Tool = new GameObject("Tool").transform;
            {
                //Axe
                Transform p_Axe = new GameObject("Axe").transform;
                p_Axe.SetParent(p_Tool);
                {
                    //Normal
                    pool_Tool_Normal_Axe = new GameObject("Normal").transform;
                    pool_Tool_Normal_Axe.SetParent(p_Axe);
                    {
                        parent = pool_Tool_Normal_Axe;
                        for (int i = 0; i < cnt_Tool_Normal_Axe; i++)
                        {
                            GameObject obj = Instantiate(prefab_Tool_Normal_Axe, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                //Hoe
                Transform p_Hoe = new GameObject("Hoe").transform;
                p_Hoe.SetParent(p_Tool);
                {
                    //Normal
                    pool_Tool_Normal_Hoe = new GameObject("Normal").transform;
                    pool_Tool_Normal_Hoe.SetParent(p_Hoe);
                    {
                        parent = pool_Tool_Normal_Hoe;
                        for (int i = 0; i < cnt_Tool_Normal_Hoe; i++)
                        {
                            GameObject obj = Instantiate(prefab_Tool_Normal_Hoe, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                //WateringCan
                Transform p_WateringCan = new GameObject("WateringCan").transform;
                p_WateringCan.SetParent(p_Tool);
                {
                    //Normal
                    pool_Tool_Normal_WateringCan = new GameObject("Normal").transform;
                    pool_Tool_Normal_WateringCan.SetParent(p_WateringCan);
                    {
                        parent = pool_Tool_Normal_WateringCan;
                        for (int i = 0; i < cnt_Tool_Normal_WateringCan; i++)
                        {
                            GameObject obj = Instantiate(prefab_Tool_Normal_WateringCan, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                DontDestroyOnLoad(p_Tool.gameObject);
            }
            //Seeds
            Transform p_Seed = new GameObject("Seed").transform;
            {
                //Carrot
                pool_Seed_Carrot = new GameObject("Carrot").transform;
                pool_Seed_Carrot.SetParent(p_Seed);
                {
                    parent = pool_Seed_Carrot;
                    for (int i = 0; i < cnt_Seed_Carrot; i++)
                    {
                        GameObject obj = Instantiate(prefab_Seed_Carrot, parent);
                        obj.SetActive(false);
                    }
                }
                //EggPlant
                pool_Seed_EggPlant = new GameObject("EggPlant").transform;
                pool_Seed_EggPlant.SetParent(p_Seed);
                {
                    parent = pool_Seed_EggPlant;
                    for (int i = 0; i < cnt_Seed_EggPlant; i++)
                    {
                        GameObject obj = Instantiate(prefab_Seed_EggPlant, parent);
                        obj.SetActive(false);
                    }
                }
                //Corn
                pool_Seed_Corn = new GameObject("Corn").transform;
                pool_Seed_Corn.SetParent(p_Seed);
                {
                    parent = pool_Seed_Corn;
                    for (int i = 0; i < cnt_Seed_Corn; i++)
                    {
                        GameObject obj = Instantiate(prefab_Seed_Corn, parent);
                        obj.SetActive(false);
                    }
                }
                //Pumpkin
                pool_Seed_Pumpkin = new GameObject("Pumpkin").transform;
                pool_Seed_Pumpkin.SetParent(p_Seed);
                {
                    parent = pool_Seed_Pumpkin;
                    for (int i = 0; i < cnt_Seed_Pumpkin; i++)
                    {
                        GameObject obj = Instantiate(prefab_Seed_Pumpkin, parent);
                        obj.SetActive(false);
                    }
                }
                //Tomato
                pool_Seed_Tomato = new GameObject("Tomato").transform;
                pool_Seed_Tomato.SetParent(p_Seed);
                {
                    parent = pool_Seed_Tomato;
                    for (int i = 0; i < cnt_Seed_Tomato; i++)
                    {
                        GameObject obj = Instantiate(prefab_Seed_Tomato, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Seed.gameObject);
            }
            //Plants
            Transform p_Plant = new GameObject("Plant").transform;
            {
                //Carrot
                pool_Plant_Carrot = new GameObject("Carrot").transform;
                pool_Plant_Carrot.SetParent(p_Plant);
                {
                    parent = pool_Plant_Carrot;
                    for (int i = 0; i < cnt_Plant_Carrot; i++)
                    {
                        GameObject obj = Instantiate(prefab_Plant_Carrot, parent);
                        obj.SetActive(false);
                    }
                }
                //EggPlant
                pool_Plant_EggPlant = new GameObject("EggPlant").transform;
                pool_Plant_EggPlant.SetParent(p_Plant);
                {
                    parent = pool_Plant_EggPlant;
                    for (int i = 0; i < cnt_Plant_EggPlant; i++)
                    {
                        GameObject obj = Instantiate(prefab_Plant_EggPlant, parent);
                        obj.SetActive(false);
                    }
                }
                //Corn
                pool_Plant_Corn = new GameObject("Corn").transform;
                pool_Plant_Corn.SetParent(p_Plant);
                {
                    parent = pool_Plant_Corn;
                    for (int i = 0; i < cnt_Plant_Corn; i++)
                    {
                        GameObject obj = Instantiate(prefab_Plant_Corn, parent);
                        obj.SetActive(false);
                    }
                }
                //Pumpkin
                pool_Plant_Pumpkin = new GameObject("Pumpkin").transform;
                pool_Plant_Pumpkin.SetParent(p_Plant);
                {
                    parent = pool_Plant_Pumpkin;
                    for (int i = 0; i < cnt_Plant_Pumpkin; i++)
                    {
                        GameObject obj = Instantiate(prefab_Plant_Pumpkin, parent);
                        obj.SetActive(false);
                    }
                }
                //Tomato
                pool_Plant_Tomato = new GameObject("Tomato").transform;
                pool_Plant_Tomato.SetParent(p_Plant);
                {
                    parent = pool_Plant_Tomato;
                    for (int i = 0; i < cnt_Plant_Tomato; i++)
                    {
                        GameObject obj = Instantiate(prefab_Plant_Tomato, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Plant.gameObject);
            }
            //Crops
            Transform p_Crop = new GameObject("Crop").transform;
            {
                //Carrot
                pool_Crop_Carrot = new GameObject("Carrot").transform;
                pool_Crop_Carrot.SetParent(p_Crop);
                {
                    parent = pool_Crop_Carrot;
                    for (int i = 0; i < cnt_Crop_Carrot; i++)
                    {
                        GameObject obj = Instantiate(prefab_Crop_Carrot, parent);
                        obj.SetActive(false);
                    }
                }
                //EggPlant
                pool_Crop_EggPlant = new GameObject("EggPlant").transform;
                pool_Crop_EggPlant.SetParent(p_Crop);
                {
                    parent = pool_Crop_EggPlant;
                    for (int i = 0; i < cnt_Crop_EggPlant; i++)
                    {
                        GameObject obj = Instantiate(prefab_Crop_EggPlant, parent);
                        obj.SetActive(false);
                    }
                }
                //Corn
                pool_Crop_Corn = new GameObject("Corn").transform;
                pool_Crop_Corn.SetParent(p_Crop);
                {
                    parent = pool_Crop_Corn;
                    for (int i = 0; i < cnt_Crop_Corn; i++)
                    {
                        GameObject obj = Instantiate(prefab_Crop_Corn, parent);
                        obj.SetActive(false);
                    }
                }
                //Pumpkin
                pool_Crop_Pumpkin = new GameObject("Pumpkin").transform;
                pool_Crop_Pumpkin.SetParent(p_Crop);
                {
                    parent = pool_Crop_Pumpkin;
                    for (int i = 0; i < cnt_Crop_Pumpkin; i++)
                    {
                        GameObject obj = Instantiate(prefab_Crop_Pumpkin, parent);
                        obj.SetActive(false);
                    }
                }
                //Tomato
                pool_Crop_Tomato = new GameObject("Tomato").transform;
                pool_Crop_Tomato.SetParent(p_Crop);
                {
                    parent = pool_Crop_Tomato;
                    for (int i = 0; i < cnt_Crop_Tomato; i++)
                    {
                        GameObject obj = Instantiate(prefab_Crop_Tomato, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Crop.gameObject);
            }
            //Fruit
            Transform t_Fruit = new GameObject("Fruit").transform;
            {
                //Apple
                pool_Fruit_Apple = new GameObject("Apple").transform;
                pool_Fruit_Apple.SetParent(t_Fruit);
                {
                    parent = pool_Fruit_Apple;
                    for (int i = 0; i < cnt_Fruit_Apple; i++)
                    {
                        GameObject obj = Instantiate(prefab_Fruit_Apple, parent);
                        obj.SetActive(false);
                    }
                }
                //Orange
                pool_Fruit_Orange = new GameObject("Orange").transform;
                pool_Fruit_Orange.SetParent(t_Fruit);
                {
                    parent = pool_Fruit_Orange;
                    for (int i = 0; i < cnt_Fruit_Orange; i++)
                    {
                        GameObject obj = Instantiate(prefab_Fruit_Orange, parent);
                        obj.SetActive(false);
                    }
                }
                //Peach
                pool_Fruit_Peach = new GameObject("Peach").transform;
                pool_Fruit_Peach.SetParent(t_Fruit);
                {
                    parent = pool_Fruit_Peach;
                    for (int i = 0; i < cnt_Fruit_Peach; i++)
                    {
                        {
                            GameObject obj = Instantiate(prefab_Fruit_Peach, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                //Pear
                pool_Fruit_Pear = new GameObject("Pear").transform;
                pool_Fruit_Pear.SetParent(t_Fruit);
                {
                    parent = pool_Fruit_Pear;
                    for (int i = 0; i < cnt_Fruit_Pear; i++)
                    {
                        GameObject obj = Instantiate(prefab_Fruit_Pear, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(t_Fruit.gameObject);
            }
            //Spawn Eggs
            Transform t_Spawn_Egg = new GameObject("Spawn_Egg").transform;
            {
                //Chicken
                pool_Spawn_Egg_Chicken = new GameObject("Chicken").transform;
                pool_Spawn_Egg_Chicken.SetParent(t_Spawn_Egg);
                {
                    parent = pool_Spawn_Egg_Chicken;
                    for (int i = 0; i < cnt_Spawn_Egg_Chicken; i++)
                    {
                        GameObject obj = Instantiate(prefab_Spawn_Egg_Chicken, parent);
                        obj.SetActive(false);
                    }
                }
                //Cow
                pool_Spawn_Egg_Cow = new GameObject("Cow").transform;
                pool_Spawn_Egg_Cow.SetParent(t_Spawn_Egg);
                {
                    parent = pool_Spawn_Egg_Cow;
                    for (int i = 0; i < cnt_Spawn_Egg_Cow; i++)
                    {
                        GameObject obj = Instantiate(prefab_Spawn_Egg_Cow, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(t_Spawn_Egg.gameObject);
            }
            //Eggs
            Transform p_Egg = new GameObject("Egg").transform;
            {
                //Normal
                pool_Egg_Normal = new GameObject("Normal").transform;
                pool_Egg_Normal.SetParent(p_Egg);
                {
                    parent = pool_Egg_Normal;
                    for (int i = 0; i < cnt_Egg_Normal; i++)
                    {
                        GameObject obj = Instantiate(prefab_Egg_Normal, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Egg.gameObject);
            }
            //Milk
            Transform p_Milk = new GameObject("Milk").transform;
            {
                pool_Milk_Normal = new GameObject("Normal").transform;
                pool_Milk_Normal.SetParent(p_Milk);
                {
                    parent = pool_Milk_Normal;
                    for (int i = 0; i < cnt_Milk_Normal; i++)
                    {
                        GameObject obj = Instantiate(prefab_Milk_Normal, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Milk.gameObject);
            }
            //Props
            Transform p_Prop = new GameObject("Prop").transform;
            {
                //Store Item
                pool_Store_Item = new GameObject("Store_Item").transform;
                pool_Store_Item.SetParent(p_Prop);
                {
                    parent = pool_Store_Item;
                    for (int i = 0; i < cnt_Store_Item; i++)
                    {
                        GameObject obj = Instantiate(prefab_Store_Item, parent);
                        obj.SetActive(false);
                    }
                }
                //Store Object
                pool_Store_Obj = new GameObject("Store_Obj").transform;
                pool_Store_Obj.SetParent(p_Prop);
                {
                    parent = pool_Store_Obj;
                    for (int i = 0; i < cnt_Store_Obj; i++)
                    {
                        GameObject obj = Instantiate(prefab_Store_Obj, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Prop.gameObject);
            }
            //Animals
            Transform p_Animal = new GameObject("Animal").transform;
            {
                //Chicken
                Transform p_Chicken = new GameObject("Chicken").transform;
                p_Chicken.SetParent(p_Animal);
                {
                    //Basic
                    pool_Animal_Basic_Chicken = new GameObject("Basic").transform;
                    pool_Animal_Basic_Chicken.SetParent(p_Chicken);
                    {
                        parent = pool_Animal_Basic_Chicken;
                        for (int i = 0; i < cnt_Animal_Basic_Chicken; i++)
                        {
                            GameObject obj = Instantiate(prefab_Animal_Basic_Chicken, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                //Cow
                Transform p_Cow = new GameObject("Cow").transform;
                p_Cow.SetParent(p_Animal);
                {
                    //Basic
                    pool_Animal_Basic_Cow = new GameObject("Basic").transform;
                    pool_Animal_Basic_Cow.SetParent(p_Cow);
                    {
                        parent = pool_Animal_Basic_Cow;
                        for (int i = 0; i < cnt_Animal_Basic_Cow; i++)
                        {
                            GameObject obj = Instantiate(prefab_Animal_Basic_Cow, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                //Mole
                Transform p_Mole = new GameObject("Mole").transform;
                p_Mole.SetParent(p_Animal);
                {
                    //Basic
                    pool_Animal_Basic_Mole = new GameObject("Basic").transform;
                    pool_Animal_Basic_Mole.SetParent(p_Mole);
                    {
                        parent = pool_Animal_Basic_Mole;
                        for (int i = 0; i < cnt_Animal_Basic_Mole; i++)
                        {
                            GameObject obj = Instantiate(prefab_Animal_Basic_Mole, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                DontDestroyOnLoad(p_Animal.gameObject);
            }
            //Lands
            Transform p_Land = new GameObject("Land").transform;
            {
                //Light Grass
                pool_Land_Light_Grass = new GameObject("Light_Grass").transform;
                pool_Land_Light_Grass.SetParent(p_Land);
                {
                    parent = pool_Land_Light_Grass;
                    for (int i = 0; i < cnt_Land_Light_Grass; i++)
                    {
                        GameObject obj = Instantiate(prefab_Land_Light_Grass, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Land.gameObject);
            }
            //Trees
            Transform p_Tree = new GameObject("Tree").transform;
            {
                //Tree Default
                pool_Tree_Default = new GameObject("Default").transform;
                pool_Tree_Default.SetParent(p_Tree);
                {
                    parent = pool_Tree_Default;
                    for (int i = 0; i < cnt_Tree_Default; i++)
                    {
                        GameObject obj = Instantiate(prefab_Tree_Default, parent);
                        obj.SetActive(false);
                    }
                }
                //Tree Apple
                pool_Tree_Apple = new GameObject("Apple").transform;
                pool_Tree_Apple.SetParent(p_Tree);
                {
                    parent = pool_Tree_Apple;
                    for (int i = 0; i < cnt_Tree_Apple; i++)
                    {
                        GameObject obj = Instantiate(prefab_Tree_Apple, parent);
                        obj.SetActive(false);
                    }
                }
                //Tree Orange
                pool_Tree_Orange = new GameObject("Orange").transform;
                pool_Tree_Orange.SetParent(p_Tree);
                {
                    parent = pool_Tree_Orange;
                    for (int i = 0; i < cnt_Tree_Orange; i++)
                    {
                        GameObject obj = Instantiate(prefab_Tree_Orange, parent);
                        obj.SetActive(false);
                    }
                }
                //Tree Peach
                pool_Tree_Peach = new GameObject("Peach").transform;
                pool_Tree_Peach.SetParent(p_Tree);
                {
                    parent = pool_Tree_Peach;
                    for (int i = 0; i < cnt_Tree_Peach; i++)
                    {
                        GameObject obj = Instantiate(prefab_Tree_Peach, parent);
                        obj.SetActive(false);
                    }
                }
                //Tree Pear
                pool_Tree_Pear = new GameObject("Pear").transform;
                pool_Tree_Pear.SetParent(p_Tree);
                {
                    parent = pool_Tree_Pear;
                    for (int i = 0; i < cnt_Tree_Pear; i++)
                    {
                        GameObject obj = Instantiate(prefab_Tree_Pear, parent);
                        obj.SetActive(false);
                    }
                }
                DontDestroyOnLoad(p_Tree.gameObject);
            }
            //Ingredients
            Transform p_Ingredient = new GameObject("Ingredient").transform;
            {
                //Wood
                Transform p_Wood = new GameObject("Wood").transform;
                p_Wood.SetParent(p_Ingredient);
                {
                    //Log
                    pool_Wood_Log = new GameObject("Log").transform;
                    pool_Wood_Log.SetParent(p_Wood);
                    {
                        parent = pool_Wood_Log;
                        for (int i = 0; i < cnt_Wood_Log; i++)
                        {
                            GameObject obj = Instantiate(prefab_Wood_Log, parent);
                            obj.SetActive(false);
                        }
                    }
                    //Branch
                    pool_Wood_Branch = new GameObject("Branch").transform;
                    pool_Wood_Branch.SetParent(p_Wood);
                    {
                        parent = pool_Wood_Branch;
                        for (int i = 0; i < cnt_Wood_Branch; i++)
                        {
                            GameObject obj = Instantiate(prefab_Wood_Branch, parent);
                            obj.SetActive(false);
                        }
                    }
                    //Stick
                    pool_Wood_Stick = new GameObject("Stick").transform;
                    pool_Wood_Stick.SetParent(p_Wood);
                    {
                        parent = pool_Wood_Stick;
                        for (int i = 0; i < cnt_Wood_Stick; i++)
                        {
                            GameObject obj = Instantiate(prefab_Wood_Stick, parent);
                            obj.SetActive(false);
                        }
                    }
                    //Plank
                    pool_Wood_Plank = new GameObject("Plank").transform;
                    pool_Wood_Plank.SetParent(p_Wood);
                    {
                        parent = pool_Wood_Plank;
                        for (int i = 0; i < cnt_Wood_Plank; i++)
                        {
                            GameObject obj = Instantiate(prefab_Wood_Plank, parent);
                            obj.SetActive(false);
                        }
                    }
                }
                DontDestroyOnLoad(p_Ingredient.gameObject);
            }
        }
    }
    #endregion

    #region ==========Methods==========
    public GameObject GetInstance(ItemID id, bool active = true)
    {
        Transform pool = id switch
        {
            //Tools
            ItemID.TOOL_NORMAL_AXE => pool_Tool_Normal_Axe,
            ItemID.TOOL_NORMAL_HOE => pool_Tool_Normal_Hoe,
            ItemID.TOOL_NORMAL_WATERINGCAN => pool_Tool_Normal_WateringCan,

            //Seeds
            ItemID.SEED_CARROT => pool_Seed_Carrot,
            ItemID.SEED_EGGPLANT => pool_Seed_EggPlant,
            ItemID.SEED_CORN => pool_Seed_Corn,
            ItemID.SEED_PUMPKIN => pool_Seed_Pumpkin,
            ItemID.SEED_TOMATO => pool_Seed_Tomato,

            //Plants
            ItemID.PLANT_CARROT => pool_Plant_Carrot,
            ItemID.PLANT_EGGPLANT => pool_Plant_EggPlant,
            ItemID.PLANT_CORN => pool_Plant_Corn,
            ItemID.PLANT_PUMPKIN => pool_Plant_Pumpkin,
            ItemID.PLANT_TOMATO => pool_Plant_Tomato,

            //Crops
            ItemID.CROP_CARROT => pool_Crop_Carrot,
            ItemID.CROP_EGGPLANT => pool_Crop_EggPlant,
            ItemID.CROP_CORN => pool_Crop_Corn,
            ItemID.CROP_PUMPKIN => pool_Crop_Pumpkin,
            ItemID.CROP_TOMATO => pool_Crop_Tomato,

            //Fruits
            ItemID.FRUIT_APPLE => pool_Fruit_Apple,
            ItemID.FRUIT_ORANGE => pool_Fruit_Orange,
            ItemID.FRUIT_PEACH => pool_Fruit_Peach,
            ItemID.FRUIT_PEAR => pool_Fruit_Pear,

            //Spawn Eggs
            ItemID.SPAWN_EGG_CHICKEN => pool_Spawn_Egg_Chicken,
            ItemID.SPAWN_EGG_COW => pool_Spawn_Egg_Cow,

            //Eggs
            ItemID.EGG_NORMAL => pool_Egg_Normal,

            //Milk
            ItemID.MILK_NORMAL => pool_Milk_Normal,

            //Props
            ItemID.PROP_STORE_ITEM => pool_Store_Item,
            ItemID.PROP_STORE_OBJ => pool_Store_Obj,

            //Animals
            ItemID.ANIMAL_CHICKEN_BASIC => pool_Animal_Basic_Chicken,
            ItemID.ANIMAL_COW_BASIC => pool_Animal_Basic_Cow,
            ItemID.ANIMAL_MOLE => pool_Animal_Basic_Mole,

            //Lands
            ItemID.LAND_LIGHT_GRASS => pool_Land_Light_Grass,

            //Tree
            ItemID.TREE_DEFAULT => pool_Tree_Default,
            ItemID.TREE_APPLE => pool_Tree_Apple,
            ItemID.TREE_ORANGE => pool_Tree_Orange,
            ItemID.TREE_PEACH => pool_Tree_Peach,
            ItemID.TREE_PEAR => pool_Tree_Pear,

            //Wood
            ItemID.WOOD_LOG => pool_Wood_Log,
            ItemID.WOOD_BRANCH => pool_Wood_Branch,
            ItemID.WOOD_STICK => pool_Wood_Stick,
            ItemID.WOOD_PLANK => pool_Wood_Plank,

            //Default
            _ => null,
        };

        if (pool == null)
        {
            return null;
        }

        for (int i = 0; i < pool.childCount; i++)
        {
            if (!pool.GetChild(i).gameObject.activeSelf)
            {
                GameObject obj = pool.GetChild(i).gameObject;
                obj.SetActive(active);
                obj.transform.SetParent(activated);
                return obj;
            }
        }

        return null;
    }

    public GameObject GetPrefab(ItemID id)
    {
        return id switch
        {
            //Tools
            ItemID.TOOL_NORMAL_AXE => prefab_Tool_Normal_Axe,
            ItemID.TOOL_NORMAL_HOE => prefab_Tool_Normal_Hoe,
            ItemID.TOOL_NORMAL_WATERINGCAN => prefab_Tool_Normal_WateringCan,

            //Seeds
            ItemID.SEED_CARROT => prefab_Seed_Carrot,
            ItemID.SEED_EGGPLANT => prefab_Seed_EggPlant,
            ItemID.SEED_CORN => prefab_Seed_Corn,
            ItemID.SEED_PUMPKIN => prefab_Seed_Pumpkin,
            ItemID.SEED_TOMATO => prefab_Seed_Tomato,

            //Plants
            ItemID.PLANT_CARROT => prefab_Plant_Carrot,
            ItemID.PLANT_EGGPLANT => prefab_Plant_EggPlant,
            ItemID.PLANT_CORN => prefab_Plant_Corn,
            ItemID.PLANT_PUMPKIN => prefab_Plant_Pumpkin,
            ItemID.PLANT_TOMATO => prefab_Plant_Tomato,

            //Fruits
            ItemID.FRUIT_APPLE => prefab_Fruit_Apple,
            ItemID.FRUIT_ORANGE => prefab_Fruit_Orange,
            ItemID.FRUIT_PEACH => prefab_Fruit_Peach,
            ItemID.FRUIT_PEAR => prefab_Fruit_Pear,

            //Crops
            ItemID.CROP_CARROT => prefab_Crop_Carrot,
            ItemID.CROP_EGGPLANT => prefab_Crop_EggPlant,
            ItemID.CROP_CORN => prefab_Crop_Corn,
            ItemID.CROP_PUMPKIN => prefab_Crop_Pumpkin,
            ItemID.CROP_TOMATO => prefab_Crop_Tomato,

            //Spawn Eggs
            ItemID.SPAWN_EGG_CHICKEN => prefab_Spawn_Egg_Chicken,
            ItemID.SPAWN_EGG_COW => prefab_Spawn_Egg_Cow,

            //Eggs
            ItemID.EGG_NORMAL => prefab_Egg_Normal,

            //Milk
            ItemID.MILK_NORMAL => prefab_Milk_Normal,

            //Props
            ItemID.PROP_STORE_ITEM => prefab_Store_Item,
            ItemID.PROP_STORE_OBJ => prefab_Store_Obj,

            //Animals
            ItemID.ANIMAL_CHICKEN_BASIC => prefab_Animal_Basic_Chicken,
            ItemID.ANIMAL_COW_BASIC => prefab_Animal_Basic_Cow,
            ItemID.ANIMAL_MOLE => prefab_Animal_Basic_Mole,

            //Lands
            ItemID.LAND_LIGHT_GRASS => prefab_Land_Light_Grass,

            //Tree
            ItemID.TREE_DEFAULT => prefab_Tree_Default,
            ItemID.TREE_APPLE => prefab_Tree_Apple,
            ItemID.TREE_ORANGE => prefab_Tree_Orange,
            ItemID.TREE_PEACH => prefab_Tree_Peach,
            ItemID.TREE_PEAR => prefab_Tree_Pear,

            //Wood
            ItemID.WOOD_LOG => prefab_Wood_Log,
            ItemID.WOOD_BRANCH => prefab_Wood_Branch,
            ItemID.WOOD_STICK => prefab_Wood_Stick,
            ItemID.WOOD_PLANK => prefab_Wood_Plank,

            //Default
            _ => null,
        };
    }

    public void ReturnHierarchy(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        if (obj.TryGetComponent<Item>(out Item item))
        {
            obj.transform.parent = item.ID switch
            {
                //Tool
                ItemID.TOOL_NORMAL_AXE => pool_Tool_Normal_Axe,
                ItemID.TOOL_NORMAL_HOE => pool_Tool_Normal_Hoe,
                ItemID.TOOL_NORMAL_WATERINGCAN => pool_Tool_Normal_WateringCan,

                //Seed
                ItemID.SEED_CARROT => pool_Seed_Carrot,
                ItemID.SEED_EGGPLANT => pool_Seed_EggPlant,
                ItemID.SEED_CORN => pool_Seed_Corn,
                ItemID.SEED_PUMPKIN => pool_Seed_Pumpkin,
                ItemID.SEED_TOMATO => pool_Seed_Tomato,

                //Plant
                ItemID.PLANT_CARROT => pool_Plant_Carrot,
                ItemID.PLANT_EGGPLANT => pool_Plant_EggPlant,
                ItemID.PLANT_CORN => pool_Plant_Corn,
                ItemID.PLANT_PUMPKIN => pool_Plant_Pumpkin,
                ItemID.PLANT_TOMATO => pool_Plant_Tomato,

                //Crop
                ItemID.CROP_CARROT => pool_Crop_Carrot,
                ItemID.CROP_EGGPLANT => pool_Crop_EggPlant,
                ItemID.CROP_CORN => pool_Crop_Corn,
                ItemID.CROP_PUMPKIN => pool_Crop_Pumpkin,
                ItemID.CROP_TOMATO => pool_Crop_Tomato,
                
                //Fruit
                ItemID.FRUIT_APPLE => pool_Fruit_Apple,
                ItemID.FRUIT_ORANGE => pool_Fruit_Orange,
                ItemID.FRUIT_PEACH => pool_Fruit_Peach,
                ItemID.FRUIT_PEAR => pool_Fruit_Pear,

                //Spawn Egg
                ItemID.SPAWN_EGG_CHICKEN => pool_Spawn_Egg_Chicken,
                ItemID.SPAWN_EGG_COW => pool_Spawn_Egg_Cow,

                //Egg
                ItemID.EGG_NORMAL => pool_Egg_Normal,

                //Milk
                ItemID.MILK_NORMAL => pool_Milk_Normal,

                //Prop
                ItemID.PROP_STORE_ITEM => pool_Store_Item,
                ItemID.PROP_STORE_OBJ => pool_Store_Obj,

                //Land
                ItemID.LAND_LIGHT_GRASS => pool_Land_Light_Grass,

                //Tree
                ItemID.TREE_DEFAULT => pool_Tree_Default,
                ItemID.TREE_APPLE => pool_Tree_Apple,
                ItemID.TREE_ORANGE => pool_Tree_Orange,
                ItemID.TREE_PEACH => pool_Tree_Peach,
                ItemID.TREE_PEAR => pool_Tree_Pear,

                //Wood
                ItemID.WOOD_LOG => pool_Wood_Log,
                ItemID.WOOD_BRANCH => pool_Wood_Branch,
                ItemID.WOOD_STICK => pool_Wood_Stick,
                ItemID.WOOD_PLANK => pool_Wood_Plank,

                //Default
                _ => null
            };
            item.Init();
        }
        else if (obj.TryGetComponent<Animal>(out Animal animal))
        {
            obj.transform.parent = animal.ID switch
            {
                //Animal
                ItemID.ANIMAL_CHICKEN_BASIC => pool_Animal_Basic_Chicken,
                ItemID.ANIMAL_COW_BASIC => pool_Animal_Basic_Cow,
                ItemID.ANIMAL_MOLE => pool_Animal_Basic_Mole,
                //Default
                _ => null
            };
        }
        else
        {
            obj.transform.SetParent(null);
        }

        obj.SetActive(false);
    }
    #endregion
}