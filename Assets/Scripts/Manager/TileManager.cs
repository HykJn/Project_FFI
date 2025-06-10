using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    #region Sturcture
    struct TileData
    {
        public bool IsPlowable { get; set; }
        public bool IsPlowed { get; set; }
        public bool IsWet { get; set; }
        public bool IsPlanted { get; set; }

        public TileData(bool isPlowable = false, bool isPlowed = false, bool isWet = false, bool isPlanted = false)
        {
            this.IsPlowable = isPlowable;
            this.IsPlowed = isPlowed;
            this.IsWet = isWet;
            this.IsPlanted = isPlanted;
        }
    }
    #endregion

    #region Properties
    public static TileManager Instance => instance;

    public Tilemap Layer_Lands => layer_Lands;
    public Tilemap Layer_FarmLands => layer_FarmLands;
    public Tilemap Layer_Plants => layer_Plants;
    public Tilemap[] Layer_Props => layer_Props;

    public bool MoleSpawned { get; set; }
    public int TreeCount { get; set; } = 0;
    private int MaxTreeCount
    {
        get
        {
            return (layer_Lands.size.x * layer_Lands.size.y) / 20;
        }
    }
    #endregion

    #region PrivateFields
    //Singleton instance
    private static TileManager instance = null;

    //Tilemap layers
    [Header("Layers")]
    [SerializeField] private Tilemap layer_Lands;
    [SerializeField] private Tilemap layer_FarmLands;
    [SerializeField] private Tilemap layer_Plants;
    [SerializeField] private Tilemap[] layer_Props;

    //RuleTiles
    [Header("Tiles")]
    [SerializeField] private RuleTile rule_FarmLand;
    [SerializeField] private RuleTile rule_Land;
    [SerializeField] private RuleTile rule_GameObject;

    //Color preset
    private readonly Color COLOR_FARMLAND = new Color(232, 207, 166, 255) / 255f;
    private readonly Color COLOR_WETLAND = new Color(174, 110, 40, 255) / 255f;

    //Data of tiles
    private Dictionary<Vector2Int, TileData> tileData = new();

    //Misc
    private float moleSpawnTick = 0f;
    private int spawnTreeDays = 0;
    #endregion

    #region Unity
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
    }

    private void Start()
    {
        Init();

        for (int i = 0; i < layer_Props.Length; i++)
        {
            InitPropLayer(i);
        }

        spawnTreeDays = 1;
        TreeSpawner();
        GameManager.Instance.OnEndDay += TreeSpawner;
    }

    private void Update()
    {
        MoleSpawner();
    }
    #endregion

    #region Methods
    public void Init()
    {
        UpdateFarmLandLayer();
        UpdateLandLayer();
    }

    void UpdateLandLayer()
    {
        layer_Lands.CompressBounds();

        foreach (Vector2Int pos in layer_Lands.cellBounds.allPositionsWithin)
        {
            if (!layer_Lands.HasTile((Vector3Int)pos)) continue;
            bool flag = false;   //flag for exit for loop
            //Check 8 direction of current tile
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3Int tempPos = (Vector3Int)pos + new Vector3Int(x, y);
                    //If any tile doesn't exist at near -> Not Plowable
                    if (!layer_Lands.HasTile(tempPos))
                    {
                        if (!tileData.ContainsKey(pos))
                        {
                            tileData.Add(pos, new TileData());
                        }
                        TileData temp = tileData[pos];
                        temp.IsPlowable = false;
                        tileData[pos] = temp;
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }
            if (flag) continue;
            else
            {
                //If all tile exsit at near -> Plowable
                if (!tileData.ContainsKey(pos))
                {
                    tileData.Add(pos, new TileData());
                }
                TileData temp = tileData[pos];
                temp.IsPlowable = true;
                tileData[pos] = temp;
            }
        }
    }

    void UpdateFarmLandLayer()
    {
        layer_FarmLands.CompressBounds();

        foreach (Vector2Int pos in layer_FarmLands.cellBounds.allPositionsWithin)
        {
            if (!layer_FarmLands.HasTile((Vector3Int)pos)) continue;

            //Check tile's data
            if (!tileData.ContainsKey(pos))
            {
                tileData.Add(pos, new TileData(true));
            }
            //Set dry
            if (tileData[pos].IsPlowed && tileData[pos].IsWet)
            {
                layer_FarmLands.SetColor((Vector3Int)pos, COLOR_FARMLAND);

                //Set tile's data
                TileData prev = tileData[pos];
                prev.IsWet = false;
                tileData[pos] = prev;
            }
            //If plowed but not wet
            else if (tileData[pos].IsPlowed && !tileData[pos].IsWet)
            {
                //Remove farmland
                layer_FarmLands.SetTile((Vector3Int)pos, null);

                //Set tile's data
                TileData prev = tileData[pos];
                prev.IsPlowed = false;
                prev.IsPlanted = false;
                tileData[pos] = prev;
            }
        }
    }

    void InitPropLayer(int layer)
    {
        layer_Props[layer].CompressBounds();

        if (layer_Props[layer].transform.childCount > 0)
        {
            int childCount = layer_Props[layer].transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform prop = layer_Props[layer].transform.GetChild(i);
                rule_GameObject.m_DefaultGameObject = prop.gameObject;
                layer_Props[layer].SetTile(Vector3Int.RoundToInt(prop.position), rule_GameObject);
                rule_GameObject.m_DefaultGameObject = null;
                Destroy(prop.gameObject);
            }
        }
    }

    public bool Plow(Vector2Int pos)
    {
        if (!tileData.ContainsKey(pos)) return false;

        for (int i = 0; i < layer_Props.Length; i++)
        {
            if (TryGetItem<Item>(pos, layer_Props[i], out Item item))
            {
                return false;
            }
        }


        //Check mole exist in pos
        RaycastHit2D hit = Physics2D.BoxCast(pos, Vector2.one, 0, Vector3.forward);
        if (hit && hit.collider.CompareTag("Mole"))
        {
            hit.collider.gameObject.GetComponent<Mole>().Hide();
            hit.collider.gameObject.SetActive(false);

            return true;
        }


        //Check tile's data
        TileData tile = tileData[pos];
        if (tile.IsPlowable && !tile.IsPlowed)
        {
            //Set tile
            layer_FarmLands.SetTile((Vector3Int)pos, rule_FarmLand);
            layer_FarmLands.SetColor((Vector3Int)pos, COLOR_FARMLAND);

            //Set tile's data
            tile.IsPlowed = true;
            tileData[pos] = tile;

            QuestManager.OnQuestProceed?.Invoke(ActionID.Plow, 1);
            return true;
        }

        return false;
    }

    public bool Harvest(Vector2Int pos)
    {
        if (!tileData.ContainsKey(pos)) return false;

        if (TryGetPlant(pos, out Plant plant))
        {
            if (plant.Growth == plant.MaxGrowth)
            {
                plant.DropCrops();
                ActionID action = plant.CropID switch
                {
                    ItemID.CROP_CARROT => ActionID.HarvestCarrot,
                    ItemID.CROP_EGGPLANT => ActionID.HarvestEggplant,
                    ItemID.CROP_TOMATO => ActionID.HarvestTomato,
                    ItemID.CROP_CORN => ActionID.HarvestCorn,
                    ItemID.CROP_PUMPKIN => ActionID.HarvestPumpkin,
                };
                QuestManager.OnQuestProceed?.Invoke(action, 1);
                return true;
            }
            else
            {
                plant.Wither();
            }
        }
        return false;
    }

    public bool Axing(Vector2Int pos)
    {
        //TODO: Will Added later
        if (!tileData.ContainsKey(pos)) return false;

        if (TryGetPropObj(pos, out PropObj prop))
        {
            if (prop is IRemovable iRemovable)
            {
                iRemovable.Remove();
                return true;
            }
        }
        else
        {
            for (int i = 0; i < layer_Props.Length; i++)
            {
                if (TryGetItem<Tree>(pos, layer_Props[i], out Tree tree))
                {
                    tree.Chop();
                    QuestManager.OnQuestProceed?.Invoke(ActionID.CutTree, 1);
                    return true;
                }
            }
        }
        return false;
    }

    public bool Watering(Vector2Int pos)
    {
        if (!tileData.ContainsKey(pos)) return false;

        //Check tile's info
        TileData tile = tileData[pos];
        if (tile.IsPlowed && !tile.IsWet)
        {
            //Set tile
            layer_FarmLands.SetColor((Vector3Int)pos, COLOR_WETLAND);

            //Set tile's info
            tile.IsWet = true;
            tileData[pos] = tile;

            QuestManager.OnQuestProceed?.Invoke(ActionID.Watering, 1);
            return true;
        }
        return false;
    }

    public bool FillWater(Vector2Int pos)
    {
        if (tileData.ContainsKey(pos) || layer_Lands.HasTile((Vector3Int)pos)) return false;
        return true;
    }

    public bool Plant(Vector2Int pos, GameObject plant)
    {
        if (!tileData.ContainsKey(pos)) return false;

        //Check tile's data
        TileData prev = tileData[pos];
        if (prev.IsPlowed && !prev.IsPlanted)
        {
            //Set tile
            rule_GameObject.m_DefaultGameObject = plant;
            layer_Plants.SetTile((Vector3Int)pos, rule_GameObject);

            //Set tile's data
            prev.IsPlanted = true;
            tileData[pos] = prev;

            //Test
            rule_GameObject.m_DefaultGameObject = null;
            return true;
        }
        return false;
    }

    public bool Growable(Vector2Int pos)
    {
        if (!tileData.ContainsKey(pos)) return false;

        return tileData[pos].IsPlowed && tileData[pos].IsWet && tileData[pos].IsPlanted;
    }

    public void Wither(Vector2Int pos)
    {
        if (!tileData.ContainsKey(pos)) return;

        layer_Plants.SetTile((Vector3Int)pos, null);
        TileData tile = tileData[pos];
        tile.IsPlanted = false;
        tileData[pos] = tile;
    }

    public bool TryGetPlant(Vector2Int pos, out Plant plant)
    {
        if (!tileData.ContainsKey(pos))
        {
            plant = null;
            return false;
        }

        GameObject plantObj = layer_Plants.GetInstantiatedObject((Vector3Int)pos);
        if (plantObj != null) return plantObj.TryGetComponent<Plant>(out plant);
        plant = null; return false;
    }

    public bool PlaceProp(Vector2Int pos, GameObject propObj, int layer)
    {
        if (!tileData.ContainsKey(pos) || layer_Props[layer].HasTile((Vector3Int)pos))
        {
            propObj.SetActive(false);
            return false;
        }

        propObj.SetActive(true);
        rule_GameObject.m_DefaultGameObject = propObj;
        layer_Props[layer].SetTile((Vector3Int)pos, rule_GameObject);
        rule_GameObject.m_DefaultGameObject = null;

        if (layer_Props[layer].GetInstantiatedObject((Vector3Int)pos).TryGetComponent<Item>(out Item item))
        {
            item.Init();
        }

        tileData[pos] = new(false, false, false, false);

        return true;
    }

    public bool PlaceProp(Vector2Int pos, GameObject propObj)
    {
        for (int i = 0; i < layer_Props.Length; i++)
        {
            if (PlaceProp(pos, propObj, i))
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveProp(Vector2Int pos, int layer)
    {
        if (!tileData.ContainsKey(pos) || !layer_Props[layer].HasTile((Vector3Int)pos)) return;

        layer_Props[layer].SetTile((Vector3Int)pos, null);
        tileData[pos] = new(true, false, false, false);
    }

    public bool TryGetPropObj(Vector2Int pos, int layer, out PropObj prop)
    {
        if (!tileData.ContainsKey(pos))
        {
            prop = null;
            return false;
        }

        GameObject propObj = layer_Props[layer].GetInstantiatedObject((Vector3Int)pos);
        if (propObj != null) return propObj.TryGetComponent<PropObj>(out prop);
        prop = null; return false;
    }

    public bool TryGetPropObj(Vector2Int pos, out PropObj prop)
    {
        for (int i = 0; i < layer_Props.Length; i++)
        {
            if (TryGetPropObj(pos, i, out prop))
            {
                return true;
            }
        }
        prop = null;
        return false;
    }

    public bool CreateLand(Vector2Int pos, RuleTile tile)
    {
        if (tileData.ContainsKey(pos) || layer_Lands.HasTile((Vector3Int)pos) || tile == null) return false;

        layer_Lands.SetTile((Vector3Int)pos, tile);
        TileData dat = new(true);
        tileData.Add(pos, dat);

        return true;
    }

    public bool RemoveLand(Vector2Int pos)
    {
        if (!tileData.ContainsKey(pos) || !layer_Lands.HasTile((Vector3Int)pos)) return false;

        for (int i = 0; i < layer_Props.Length; i++)
        {
            if (TryGetPropObj(pos, i, out PropObj prop))
            {
                return false;
            }
        }

        if (layer_FarmLands.HasTile((Vector3Int)pos)) layer_FarmLands.SetTile((Vector3Int)pos, null);
        if (layer_Plants.HasTile((Vector3Int)pos))
        {
            if (TryGetPlant(pos, out Plant plant))
            {
                if (!Harvest(pos)) plant.Wither();
            }
        }
        layer_Lands.SetTile((Vector3Int)pos, null);
        tileData.Remove(pos);

        return true;
    }

    public bool HasLand(Vector2Int position)
    {
        return tileData.ContainsKey(position) && layer_Lands.HasTile((Vector3Int)position);
    }

    public bool TryGetItem<T>(Vector2Int position, Tilemap layer, out T item) where T : Item
    {
        if (!tileData.ContainsKey(position))
        {
            item = null;
            return false;
        }

        GameObject itemObj = layer.GetInstantiatedObject((Vector3Int)position);
        if (itemObj != null) return itemObj.TryGetComponent<T>(out item);
        item = null; return false;
    }

    public Vector2 GetRandomPos(Vector2Int origin)
    {
        float randX = UnityEngine.Random.Range(-1f, 1f);
        float randY = UnityEngine.Random.Range(-1f, 1f);
        Vector2Int temp = Vector2Int.RoundToInt(new Vector2(origin.x + randX, origin.y + randY));
        if (!HasLand(temp))
        {
            return GetRandomPos(origin);
        }
        return new Vector2(randX + origin.x, randY + origin.y);
    }

    public IEnumerator DropItemSpread(GameObject item, Vector2 destination)
    {
        if (item == null) yield break;

        Vector2 start = item.transform.position;
        float t = 0;
        do
        {
            t += Time.deltaTime * 3f;
            item.transform.position = Vector2.Lerp(start, destination, t);
            yield return new WaitForEndOfFrame();
        } while (Vector2.Distance(item.transform.position, destination) > 0.1f);
        item.transform.position = destination;
    }

    public bool SpawnAnimal(Vector2Int position, GameObject animal)
    {
        if (animal == null) return false;

        animal.SetActive(false);
        if (!HasLand(position)) return false;

        if (layer_FarmLands.HasTile((Vector3Int)position)) return false;
        for (int i = 0; i < Layer_Props.Length; i++)
        {
            if (layer_Props[i].HasTile((Vector3Int)position)) return false;
        }

        animal.SetActive(true);
        animal.transform.position = (Vector3Int)position;
        return true;
    }

    public void MoleSpawner()
    {
        if (GameManager.Instance.IsPlaying && !MoleSpawned)
        {
            moleSpawnTick += Time.deltaTime;
            if (moleSpawnTick >= 45f)
            {
                moleSpawnTick = 0f;
                for (int i = 0; i < 5; i++)
                {
                    Vector2Int min = (Vector2Int)layer_Lands.cellBounds.min;
                    Vector2Int max = (Vector2Int)layer_Lands.cellBounds.max;

                    Vector2Int randPos = new Vector2Int(UnityEngine.Random.Range(min.x, max.x),
                        UnityEngine.Random.Range(min.y, max.y)
                    );

                    if (SpawnAnimal(randPos, ObjectManager.Instance.GetInstance(ItemID.ANIMAL_MOLE, true)))
                    {
                        MoleSpawned = true;
                        return;
                    }
                }
                MoleSpawner();
            }
        }
    }

    public void TreeSpawner()
    {
        if (TreeCount >= MaxTreeCount)
        {
            spawnTreeDays = 0;
            return;
        }

        if (spawnTreeDays == 1)
        {
            spawnTreeDays = 0;

            while (TreeCount < MaxTreeCount)
            {
                Vector2Int min = (Vector2Int)layer_Lands.cellBounds.min + Vector2Int.one;
                Vector2Int max = (Vector2Int)layer_Lands.cellBounds.max - Vector2Int.one;
                Vector2Int randPos = new Vector2Int(UnityEngine.Random.Range(min.x, max.x),
                        UnityEngine.Random.Range(min.y, max.y)
                    );

                if (HasLand(randPos))
                {
                    bool hasFarmLand = layer_FarmLands.HasTile((Vector3Int)randPos);
                    bool hasProp = TryGetPropObj(randPos, out PropObj temp);
                    if (!hasFarmLand && !hasProp)
                    {
                        int randNum = UnityEngine.Random.Range(0, 10);
                        ItemID treeID = ItemID.TREE_DEFAULT;
                        if(randNum >= 1 && randNum <= 4)
                        {
                            treeID += randNum;
                        }
                        PlaceProp(randPos, ObjectManager.Instance.GetPrefab(treeID), 0);
                        TreeCount++;
                    }
                }
            }
        }
        else
        {
            spawnTreeDays++;
        }
    }
    #endregion
}
