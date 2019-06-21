using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using AL_Effect;
using behaviac;
using DG.DemiEditor;
using Google.Protobuf.WellKnownTypes;
using LitJson;
//using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class HBItemWindow : EditorWindow
{
#if MAPEDITOR
    [MenuItem("Custom/HBMapEditor", priority = 0)]
    static void OpenMapEditor()
    {
        GetWindow(typeof(HBItemWindow));
    }
#endif
    private CubeType _cubeType;
    private MapType _mapType;

    private CubeType_GrassLand _grassCube;
    private CubeType_Factory _factoryCube;
    private CubeType_Ice _iceCube;
    private Cubetype_Castle _castleCube;
    private CubeType_Effect _effectCube;

    private GameObject item;
    private GameObject item_Show;
    private Vector3 MapSize;
    private int cubeKey;

    private Vector3 _localScale = Vector3.one * 1.5f;
    private float _dependLenght = 1.5f;
    private bool _randomRotation;
    private bool _castShadows = true;
    private bool _receiveShadows = true;

    private string _sceneID = "";
    private string _effect = "";

    private string description = "";
    GameObject buggyGameObject;

    HBItemWindow()
    {
        titleContent = new GUIContent("Creat HBItem");
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUILayout.Label("Map Editor", new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 20,
            normal = new GUIStyleState()
            {
                textColor = Color.yellow
            }
        });

        //绘制文本
        _sceneID = EditorGUILayout.TextField("SceneID", _sceneID);

        //小标题
        GUILayout.Space(10);
        GUILayout.Label("选择方块种类", new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
            normal = new GUIStyleState()
            {
                textColor = Color.cyan
            }
        });

        MapSize = EditorGUILayout.Vector3Field("地图尺寸:", MapSize);
        if (MapSize.x == 0 || MapSize.y == 0 || MapSize.z == 0)
        {
            MapSize = new Vector3(20, 6, 24);
        }

        GUILayout.Space(5);

        //绘制枚举菜单
        _mapType = (MapType) EditorGUILayout.EnumPopup("MapType", _mapType);
        GUILayout.Space(5);
        string[] id;
        int[] _id;
        switch (_mapType)
        {
            #region 草地

            case MapType.GrassLand:
                _factoryCube = CubeType_Factory.Factory_Nut;
                _iceCube = CubeType_Ice.Ice_Grass;
                _castleCube = Cubetype_Castle.Tower_Red;
                _effectCube = CubeType_Effect.PropBox;
                //绘制枚举菜单
                _grassCube = (CubeType_GrassLand) EditorGUILayout.EnumPopup("CubeType", _grassCube);


                id = HBItemData.ItemOfMap[0].Split(';');
                _id = new int[id.Length];
                for (int i = 0; i < id.Length; i++)
                {
                    _id[i] = Convert.ToInt32(id[i]);
                    //   Debug.Log(string.Format("'{0}' is {1}",i, (CubeType) _id[i]));
                }

                cubeKey = (int) _grassCube;

                _cubeType = (CubeType) _id[cubeKey];


                _dependLenght = 1.5f;
                if (_grassCube.ToString().Contains("HardCube"))
                {
                    _dependLenght = 0.75f;
                }


                item_Show = Resources.Load<GameObject>(HBItemData.ItemPath[(int) _cubeType]);
                break;

            #endregion

            #region 工厂

            case MapType.Factory:
                _grassCube = CubeType_GrassLand.GrassLand_Flower;
                _iceCube = CubeType_Ice.Ice_Grass;
                _castleCube = Cubetype_Castle.Tower_Red;
                _effectCube = CubeType_Effect.PropBox;
                //绘制枚举菜单
                _factoryCube = (CubeType_Factory) EditorGUILayout.EnumPopup("CubeType", _factoryCube);

                id = HBItemData.ItemOfMap[1].Split(';');
                _id = new int[id.Length];
                for (int i = 0; i < id.Length; i++)
                {
                    _id[i] = Convert.ToInt32(id[i]);
                }

                cubeKey = (int) _factoryCube;
                _cubeType = (CubeType) _id[cubeKey];


                _dependLenght = 1.5f;
                if (_factoryCube.ToString().Contains("HardCube"))
                {
                    _dependLenght = 0.75f;
                }

                item_Show = Resources.Load<GameObject>(HBItemData.ItemPath[(int) _cubeType]);
                break;

            #endregion

            #region 冰面

            case MapType.Ice:
                _grassCube = CubeType_GrassLand.GrassLand_Flower;
                _factoryCube = CubeType_Factory.Factory_Nut;
                _castleCube = Cubetype_Castle.Tower_Red;
                _effectCube = CubeType_Effect.PropBox;
                //绘制枚举菜单
                _iceCube = (CubeType_Ice) EditorGUILayout.EnumPopup("CubeType", _iceCube);

                cubeKey = (int) _iceCube;
                //    Debug.Log("cubekey is "+cubeKey);

                id = HBItemData.ItemOfMap[2].Split(';');
                _id = new int[id.Length];
                for (int i = 0; i < id.Length; i++)
                {
                    _id[i] = Convert.ToInt32(id[i]);
                }
                
                _cubeType = (CubeType) _id[cubeKey];
                

                _dependLenght = 1.5f;
                if (_iceCube.ToString().Contains("HardCube"))
                {
                    _dependLenght = 0.75f;
                }

              


                item_Show = Resources.Load<GameObject>(HBItemData.ItemPath[(int) _cubeType]);
                break;

            #endregion

            case MapType.Effect:
                _grassCube = CubeType_GrassLand.GrassLand_Flower;
                _factoryCube = CubeType_Factory.Factory_Nut;
                _iceCube = CubeType_Ice.Ice_Grass;
                _castleCube = Cubetype_Castle.Tower_Red;
                //绘制枚举菜单
                _effectCube = (CubeType_Effect) EditorGUILayout.EnumPopup("CubeType", _effectCube);

                cubeKey = (int) _effectCube;

                id = HBItemData.ItemOfMap[3].Split(';');
                _id = new int[id.Length];
                for (int i = 0; i < id.Length; i++)
                {
                    _id[i] = Convert.ToInt32(id[i]);
                }
                
                _cubeType = (CubeType) _id[cubeKey];
                item_Show = Resources.Load<GameObject>(HBItemData.ItemPath[(int) _cubeType]);
                
//                if (_effectCube.ToString().Contains("HardCube"))
//                {
//                    _cubeType = CubeType.HardCube_Parent;
//                    item_Show = Resources.Load<GameObject>("HappyBombPrefabs/NewBasePrefabs/HardCube_ParentPrefab");
//                    //  _dependLenght = 0.75f;
//                }
//                else if (_effectCube.ToString().Contains("Move"))
//                {
//                    _cubeType = CubeType.Move_ParentItem;
//                    item_Show = Resources.Load<GameObject>("HappyBombPrefabs/NewBasePrefabs/Move_ParentPrefab");
//                }
//                else if (_effectCube.ToString().Contains("Prop"))
//                {
//                    _cubeType = CubeType.PropBox;
//                    item_Show = Resources.Load<GameObject>("HappyBombPrefabs/NewBasePrefabs/PropBox");
//                }

                break;

            #region 城堡

            case MapType.Castle:
                _grassCube = CubeType_GrassLand.GrassLand_Flower;
                _factoryCube = CubeType_Factory.Factory_Nut;
                _iceCube = CubeType_Ice.Ice_Flower;
                _effectCube = CubeType_Effect.PropBox;

                //绘制枚举菜单
                _castleCube = (Cubetype_Castle) EditorGUILayout.EnumPopup("CubeType", _castleCube);

                cubeKey = (int) _castleCube;
                //    Debug.Log("cubekey is "+cubeKey);
                id = HBItemData.ItemOfMap[4].Split(';');
                _id = new int[id.Length];
                for (int i = 0; i < id.Length; i++)
                {
                    _id[i] = Convert.ToInt32(id[i]);
                }
                
                _cubeType = (CubeType) _id[cubeKey];
                

//                if (cubeKey == 0)
//                {
//                    _cubeType = (CubeType) (73);
//                }
//                else if (cubeKey == 1)
//                {
//                    _cubeType = (CubeType) (70);
//                }
//                else if (cubeKey == 2)
//                {
//                    _cubeType = (CubeType) (86);
//                }

                item_Show = Resources.Load<GameObject>(HBItemData.ItemPath[(int) _cubeType]);
                break;

            #endregion

            default:
                //绘制枚举菜单
                _cubeType = (CubeType) EditorGUILayout.EnumPopup("AllCubeType", _cubeType);
                item_Show = Resources.Load<GameObject>(HBItemData.ItemPath[(int) _cubeType]);
                break;
        }

        item_Show = EditorGUILayout.ObjectField("本次将生成的基本体", item_Show, typeof(GameObject), true) as GameObject;
        //根据选择的方块种类返回CubeType


        GUILayout.Space(5);


        //小标题
        GUILayout.Space(10);
        GUILayout.Label("填写缩放 位置吸附的单位长度 生成时四方向随机旋转", new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 15,
            normal = new GUIStyleState()
            {
                textColor = Color.cyan
            }
        });


        _localScale = EditorGUILayout.Vector3Field("LocalScale", _localScale);
        _dependLenght = EditorGUILayout.FloatField("DependLenght", _dependLenght);
        _randomRotation = EditorGUILayout.Toggle("随机旋转", _randomRotation);
        _castShadows = EditorGUILayout.Toggle("产生阴影", _castShadows);
        _receiveShadows = EditorGUILayout.Toggle("接收阴影", _receiveShadows);

        if (GUILayout.Button("实例化地形", GUILayout.Height(30)))
        {
            Creat();
            _localScale = Vector3.one * 1.5f;
        }

        //绘制当前正在编辑的场景
//        GUILayout.Space(10);
//        GUI.skin.label.fontSize = 12;
//        GUI.skin.label.alignment = TextAnchor.UpperLeft;
//        GUILayout.Label("Currently Scene:" + EditorSceneManager.GetActiveScene().name);

        //绘制对象
//        GUILayout.Space(10);
//        buggyGameObject = (GameObject) EditorGUILayout.ObjectField("Buggy Game Object", buggyGameObject, typeof(GameObject), true);

        //绘制描述文本区域
//        GUILayout.Space(10);
//        GUILayout.BeginHorizontal();
//        GUILayout.Label("Description", GUILayout.MaxWidth(80));
//        description = EditorGUILayout.TextArea(description, GUILayout.MaxHeight(75));
//        GUILayout.EndHorizontal();

//        EditorGUILayout.Space();
        GUILayout.Space(10);

        if (GUILayout.Button("加载场景", GUILayout.Height(30)))
        {
            Load();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("储存场景", GUILayout.Height(30)))
        {
            Save();
        }

        GUILayout.Space(10);


        if (GUILayout.Button("清空场景", GUILayout.Height(30)))
        {
            Clear();
        }

//        GUILayout.Space(10);
//
//
//        /*  if (GUILayout.Button("删除场景", GUILayout.Height(30)))
//          {
//              Delete();
//          }
//          GUILayout.Space(5);*/
//        if (GUILayout.Button("固定草丛的旋转值为0", GUILayout.Height(30)))
//        {
//            RotGrass();
//        }

        GUILayout.Space(10);


        /*  if (GUILayout.Button("删除场景", GUILayout.Height(30)))
          {
              Delete();
          }
          GUILayout.Space(5);*/
        if (GUILayout.Button("删除重复方块（慎用）", GUILayout.Height(30)))
        {
            DeleteRepeatCube();
        }
        
        GUILayout.Space(10);


        if (GUILayout.Button("查询所有地图的重复方块", GUILayout.Height(30)))
        {
         CheckAllMap();
        }

        GUILayout.EndVertical();
    }


    void Creat()
    {
        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
        //  GameObject cubePool = GameObject.Find("EnvironmentCubePool");
        //  GameObject item = null;
        // _dependLenght = 1.5f;

        if (_grassCube == CubeType_GrassLand.GrassLand_Ground || _factoryCube == CubeType_Factory.Factory_Ground ||
            _iceCube == CubeType_Ice.Ice_Ground)
        {
            for (int x = 0; x < MapSize.x; x++)
            for (int z = 0; z < MapSize.z; z++)
            {
                item = Instantiate(item_Show,
                    new Vector3(-MapSize.x * _localScale.x / 2 + x * _localScale.x, 0,
                        -MapSize.z * _localScale.z / 2 + z * _localScale.z), new Quaternion(0, 0, 0, 0),
                    cubePool.transform);
                item.transform.localScale = _localScale;
                HBCubeController cubeCtrl = item.AddComponent<HBCubeController>();
                cubeCtrl.ifDepend = true;
                _randomRotation = false;
                _castShadows = true;
                _receiveShadows = true;

                cubeCtrl.Init(_cubeType, _dependLenght, _randomRotation);
            }
        }

        else if (_castleCube == Cubetype_Castle.AllCastle)
        {
            GameObject father = new GameObject();
            father.name = "Castle_Father";
            father.transform.parent = cubePool.transform;
            father.AddComponent<HBCubeController>()._dependLenght = 0.75f;
            father.GetComponent<HBCubeController>().ifDepend = true;

            List<GameObject> itemArray = new List<GameObject>();
            item = Instantiate(item_Show, new Vector3(0, _localScale.y, 0), new Quaternion(0, 0, 0, 0),
                cubePool.transform);

            for (int i = 0; i < item.GetComponentsInChildren<Transform>().Length; i++)
            {
                itemArray.Add(item.GetComponentsInChildren<Transform>()[i].gameObject);
            }

            foreach (GameObject obj in itemArray)
            {
                HBCubeController cubeCtrl = obj.AddComponent<HBCubeController>();
                cubeCtrl.ifDepend = true;
                CubeType thisType = CubeType.Castle_C10;
                if (obj.name.Contains("C10"))
                {
                    thisType = CubeType.Castle_C10;
                }
                else if (obj.name.Contains("C11"))
                {
                    thisType = CubeType.Castle_C11;
                }
                else if (obj.name.Contains("CB0"))
                {
                    thisType = CubeType.Castle_CB0;
                }
                else if (obj.name.Contains("CB1"))
                {
                    thisType = CubeType.Castle_CB1;
                }
                else if (obj.name.Contains("CB2"))
                {
                    thisType = CubeType.Castle_CB2;
                }
                else if (obj.name.Contains("CB3"))
                {
                    thisType = CubeType.Castle_CB3;
                }
                else if (obj.name.Contains("CB4"))
                {
                    thisType = CubeType.Castle_CB4;
                }
                else if (obj.name.Contains("CB5"))
                {
                    thisType = CubeType.Castle_CB5;
                }
                else if (obj.name.Contains("CB6"))
                {
                    thisType = CubeType.Castle_CB6;
                }
                else if (obj.name.Contains("CB7"))
                {
                    thisType = CubeType.Castle_CB7;
                }
                else if (obj.name.Contains("CB8"))
                {
                    thisType = CubeType.Castle_CB8;
                }
                else if (obj.name.Contains("CB9"))
                {
                    thisType = CubeType.Castle_CB9;
                }

                _dependLenght = 0.75f;


                _randomRotation = false;
                _castShadows = true;
                _receiveShadows = true;
                cubeCtrl.Init(thisType, _dependLenght, _randomRotation);
                obj.transform.DetachChildren();
                obj.transform.parent = father.transform;
            }

            DestroyImmediate(item);
        }
        else
        {
            item = Instantiate(item_Show, new Vector3(0, _localScale.y, 0), new Quaternion(0, 0, 0, 0),
                cubePool.transform);
            //  GameObject item = Instantiate(Resources.Load<GameObject>(HBItemData.ItemPath[(int) _cubeType]), Vector3.zero, new Quaternion(0, 0, 0, 0),
            //   cubePool.transform);
            if (_cubeType == CubeType.Factory_Slope)
                item.transform.localScale = new Vector3(1.1f, 0.75f, 1.5f);
            else if (_cubeType == CubeType.Factory_Palisade || _cubeType == CubeType.Move_ParentItem ||
                     _cubeType == CubeType.HardCube_Parent || _cubeType == CubeType.PropBox ||
                     _cubeType == CubeType.Tower_Red || _cubeType == CubeType.Castle)
                item.transform.localScale = new Vector3(1, 1, 1);
            else
                item.transform.localScale = _localScale;

            HBCubeController cubeCtrl = item.AddComponent<HBCubeController>();
            cubeCtrl.ifDepend = true;

            _randomRotation = false;
            _castShadows = true;
            _receiveShadows = true;

            cubeCtrl.Init(_cubeType, _dependLenght, _randomRotation);
        }
    }

    void Save()
    {
        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
        List<HBCubeController> cubeControllers = new List<HBCubeController>();

        foreach (HBCubeController cubeController in cubePool.transform.GetComponentsInChildren<HBCubeController>())
        {
            cubeControllers.Add(cubeController);
        }

        if (cubeControllers.Count <= 5)
        {
            Debug.LogError("当前地图存储数据为空，请注意！");
            return;
        }
        
        HBMapConfig.MapData mapData = new HBMapConfig.MapData();
        mapData.MapID = int.Parse(_sceneID);

        Dictionary<int, Transform> childDic = new Dictionary<int, Transform>();
        for (int i = 0; i < cubeControllers.Count; i++)
        {
            GameObject item = cubeControllers[i].gameObject;

            HBCubeController hbCubeController = cubeControllers[i];


            if (hbCubeController.transform.name.Contains("CB") || hbCubeController.transform.name.Contains("C1"))
            {
                hbCubeController.transform.DetachChildren();
                hbCubeController.transform.parent = cubePool.transform;
            }

            if (hbCubeController.transform.name.Contains("Castle_Father"))
            {
                //  GameObject father = hbCubeController.transform.gameObject;
                DestroyImmediate(hbCubeController);
                //      DestroyImmediate(father);
                continue;
            }

            int parentID = -1;
            if (item.transform.parent.name != "EnvironmentCubePool")
            {
                parentID = -100;
                childDic.Add(item.GetInstanceID(), item.transform);
            }

            string effectParams = "";
            AlEffectBase effectBase = item.GetComponent<AlEffectBase>();
            if (effectBase != null)
            {
                effectParams = effectBase.CreatParams();
            }


            Vector3 fwd = Vector3.down;
            RaycastHit hit = new RaycastHit();
            if (!Physics.Raycast(hbCubeController.transform.position, fwd, out hit, 5f))
            {
                hbCubeController.CastShadows = false;
                hbCubeController.ReceiveShadows = true;
                //  Debug.Log("ray out name is " + hbCubeController.transform.name );
            }
            else
            {
                hbCubeController.CastShadows = true;
                hbCubeController.ReceiveShadows = true;

//                Debug.Log("ray out name is " + hbCubeController.transform.name + " hit name is " +
//                                hit.transform.name + " CastShadows is " + hbCubeController.CastShadows +
//                                " ReceiveShadows is " + hbCubeController.ReceiveShadows);
            }

            if (hbCubeController.Type == CubeType.HardCube_Parent)
            {
                //   Debug.Log("is hard parent");
                hbCubeController.CastShadows = false;
                hbCubeController.ReceiveShadows = true;
            }

            if (hbCubeController.transform.name.Contains("Tower") || hbCubeController.transform.name.Contains("CB") ||
                hbCubeController.transform.name.Contains("C1"))
            {
                hbCubeController.CastShadows = true;
                hbCubeController.ReceiveShadows = false;
            }


            HBMapConfig.CubeData cubeData = new HBMapConfig.CubeData()
            {
                ItemID = item.GetInstanceID(),
                Type = hbCubeController.Type,
                ParentItemID = parentID,
                RandomRotation = hbCubeController.RandomRotation,
                CastShadows = hbCubeController.CastShadows,
                ReceiveShadows = hbCubeController.ReceiveShadows,
                Position = item.transform.localPosition,
                Rotation = item.transform.localRotation,
                Scale = item.transform.localScale,
                EffectParams = effectParams,
            };
            mapData.CubeDataList.Add(cubeData);
        }

        //保存特殊父物体ID
        for (int i = 0; i < mapData.CubeDataList.Count; i++)
        {
            if (mapData.CubeDataList[i].ParentItemID == -100)
            {
                mapData.CubeDataList[i].ParentItemID =
                    childDic[mapData.CubeDataList[i].ItemID].parent.gameObject.GetInstanceID();
            }
        }

        Debug.Log("Save:" + JsonMapper.ToJson(mapData));

        HBMapConfig mapConfig = Resources.Load<HBMapConfig>("MapConfig/HBMapConfig");

        for (int i = 0; i < mapConfig.MapDataList.Count; i++)
        {
            if (mapConfig.MapDataList[i].MapID == int.Parse(_sceneID))
            {
                mapConfig.MapDataList[i] = mapData;
                EditorUtility.SetDirty(mapConfig);
                Debug.Log("覆盖场景：" + _sceneID);
                return;
            }
        }

        mapConfig.MapDataList.Add(mapData);
        EditorUtility.SetDirty(mapConfig);
        Debug.Log("储存新场景：" + _sceneID + "成功");
    }

    private void StartCoroutine(IEnumerator isHaveCubeUnder)
    {
        throw new NotImplementedException();
    }


    IEnumerator IsHaveCubeUnder(HBCubeController hbCubeController)
    {
        yield return null;

        Vector3 fwd = Vector3.down;
        if (!Physics.Raycast(hbCubeController.transform.position, fwd, 5f))
        {
            hbCubeController.CastShadows = false;
            hbCubeController.ReceiveShadows = true;
        }
    }

    void Load()
    {
        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
        HBMapConfig mapConfig = Resources.Load<HBMapConfig>("MapConfig/HBMapConfig");
        Dictionary<int, GameObject> EnvironmentCubeDic = new Dictionary<int, GameObject>();
        List<HBMapConfig.CubeData> cubeDatas = new List<HBMapConfig.CubeData>();
        for (int i = 0; i < mapConfig.MapDataList.Count; i++)
        {
            if (mapConfig.MapDataList[i].MapID == int.Parse(_sceneID))
            {
                cubeDatas = mapConfig.MapDataList[i].CubeDataList;
            }
        }

        if (cubeDatas.Count == 0)
        {
            Debug.LogError("场景" + _sceneID + "不存在或者没有储存数据");
            return;
        }

        for (int i = 0; i < cubeDatas.Count; i++)
        {
            HBMapConfig.CubeData data = cubeDatas[i];
            //     Debug.Log("type is "+data.Type);
            CubeType type = data.Type;
            GameObject obj = Instantiate(Resources.Load<GameObject>(HBItemData.ItemPath[(int) type]), data.Position,
                data.Rotation);

            HBCubeController cubeCtrl = obj.GetOrAddComponent<HBCubeController>();

            cubeCtrl.ItemID = data.ItemID;
            cubeCtrl.ParentID = data.ParentItemID;
            cubeCtrl.RandomRotation = data.RandomRotation;
            cubeCtrl.Type = data.Type;
            cubeCtrl.ifDepend = false;
            cubeCtrl.CastShadows = data.CastShadows;
            cubeCtrl.ReceiveShadows = data.ReceiveShadows;


            AlEffectBase effect = cubeCtrl.Init(type, _dependLenght, cubeCtrl.RandomRotation, cubeCtrl.CastShadows,
                cubeCtrl.ReceiveShadows);
            if (effect != null)
            {
                effect.Init(data.EffectParams);
            }

            if (data.ParentItemID == -1)
            {
                obj.transform.SetParent(cubePool.transform);
            }

            EnvironmentCubeDic.Add(data.ItemID, obj);
        }

        //所有方块生成完毕后,根据配置表设置对应的父物体
        for (int i = 0; i < cubeDatas.Count; i++)
        {
            HBMapConfig.CubeData data = cubeDatas[i];
            if (data.ParentItemID != -1)
            {
                GameObject item = EnvironmentCubeDic[data.ItemID];
                item.transform.SetParent(EnvironmentCubeDic[data.ParentItemID].transform);
                item.transform.localPosition = data.Position;
                item.transform.localRotation = data.Rotation;
                item.transform.localScale = data.Scale;
            }
        }

        //所有父物体设置完毕后，根据配置加载局部缩放
        for (int i = 0; i < cubeDatas.Count; i++)
        {
            HBMapConfig.CubeData data = cubeDatas[i];
            if (data.Scale != Vector3.one)
            {
                EnvironmentCubeDic[data.ItemID].transform.localScale = data.Scale;
            }
        }

        Debug.Log("Load");
    }

    void Clear()
    {
        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
        foreach (HBCubeController cubeCtrl in cubePool.GetComponentsInChildren<HBCubeController>(false))
        {
            if (cubeCtrl != null)
            {
                DestroyImmediate(cubeCtrl.gameObject);
            }
        }
    }

    /*   void Delete()
       {
           HBMapConfig mapConfig = Resources.Load<HBMapConfig>("MapConfig/HBMapConfig");
   
           for (int i = 0; i < mapConfig.MapDataList.Count; i++)
           {
               if (mapConfig.MapDataList[i].MapID == int.Parse(_sceneID))
               {
                   mapConfig.MapDataList.Remove(mapConfig.MapDataList[i]);
                   Debug.Log("删除场景：" + _sceneID);
                   return;
               }
           }
   
           Debug.Log("场景：" + _sceneID + "不存在");
       }*/

    void RotGrass()
    {
        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
        foreach (HBCubeController cubeCtrl in cubePool.GetComponentsInChildren<HBCubeController>(false))
        {
            if (cubeCtrl != null && cubeCtrl.Type.Equals(CubeType.GrassLand_Grass) ||
                cubeCtrl.Type.Equals(CubeType.Ice_Grass))
            {
                cubeCtrl.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    void DeleteRepeatCube()
    {
        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
        List<GameObject> cubes = new List<GameObject>();
        foreach (HBCubeController cubeController in cubePool.GetComponentsInChildren<HBCubeController>())
        {
            cubes.Add(cubeController.gameObject);
        }
        Debug.Log("场景id is "+_sceneID+" 方块数量  "+cubes.Count);
        //   cubes = GameObject.FindGameObjectsWithTag("Ground");
        for (int i = 0; i < cubes.Count; i++)
        for (int j = i + 1; j < cubes.Count; j++)
        {
            if (cubes[i] != null && cubes[i].GetComponent<HBCubeController>().Type != CubeType.Move_ParentItem &&
                cubes[j] != null && Math.Abs(cubes[i].transform.position.x - cubes[j].transform.position.x) < 0.05f &&
                Math.Abs(cubes[i].transform.position.y - cubes[j].transform.position.y) < 0.05f &&
                Math.Abs(cubes[i].transform.position.z - cubes[j].transform.position.z) < 0.05f&&
                cubes[i].GetComponent<HBCubeController>().ParentID==-1&&  cubes[j].GetComponent<HBCubeController>().ParentID==-1&&
                cubes[i].transform.localRotation==cubes[j].transform.localRotation)
            {
                if (cubes[i].transform.name.Contains("Tower") || cubes[i].transform.name.Contains("CB") ||
                    cubes[i].transform.name.Contains("C1"))
                    continue;
                else if (cubes[i].GetComponent<HBCubeController>().Type!=cubes[j].GetComponent<HBCubeController>().Type)
                    continue;
                else if (cubes[i].transform.parent.name != "EnvironmentCubePool")
                {
                    Debug.Log("本次删除的方块名为： "+cubes[i].name+" 删除方块位置为 "+cubes[i].transform.localPosition);
                    DestroyImmediate(cubes[i].gameObject);
                }
                else
                {    
                    Debug.Log("本次删除的方块名为： "+cubes[i].name+" 删除方块位置为 "+cubes[i].transform.localPosition);
                    DestroyImmediate(cubes[j].gameObject);
                }
            }
        }
    }

    void CheckAllMap()
    {
        List<HBMapConfig.MapData> thisMapData=new  List<HBMapConfig.MapData>();
        int i = 0;
        int thisID = 0;
        int thisErrorID=0;
        float Progress = 0;
        bool Find = false;
        HBMapConfig mapConfig = Resources.Load<HBMapConfig>("MapConfig/HBMapConfig");
        try
        {
            for (int p = 0; p < mapConfig.MapDataList.Count; p++)
            {
                thisMapData.Add(mapConfig.MapDataList[p]); 
            }
            
           
            Debug.Log("共有 "+thisMapData.Count+" 张地图");
        }
        catch (Exception e)
        {
            Debug.LogError("出现问题"+e);
            throw;
        }
      
        for (int m = 0; m < thisMapData.Count; m++)
        {
            Find = false;
            thisID = thisMapData[m].MapID;
            Progress += 1.0f;
            EditorUtility.DisplayProgressBar("进度","查询进度:"+Progress+"/"+(float)thisMapData.Count,Progress/(float)thisMapData.Count);
            Debug.Log("本次地图id为 "+thisID);
            for (int n =0; n < thisMapData[m].CubeDataList.Count; n++)
            {
                for (int k = n + 1; k < thisMapData[m].CubeDataList.Count; k++)
                {
                    if (Math.Abs(thisMapData[m].CubeDataList[n].Position.x-thisMapData[m].CubeDataList[k].Position.x)<=0.4f &&
                        Math.Abs(thisMapData[m].CubeDataList[n].Position.y-thisMapData[m].CubeDataList[k].Position.y)<=0.4f&&
                        Math.Abs(thisMapData[m].CubeDataList[n].Position.z-thisMapData[m].CubeDataList[k].Position.z)<=0.4f&&
                        thisMapData[m].CubeDataList[n].Type == thisMapData[m].CubeDataList[k].Type&&
                        thisMapData[m].CubeDataList[n].Rotation == thisMapData[m].CubeDataList[k].Rotation&&
                        thisMapData[m].CubeDataList[k].ParentItemID==-1&&thisMapData[m].CubeDataList[n].ParentItemID==-1&&
                        !thisMapData[m].CubeDataList[k].Type.ToString().Contains("Castle"))
                    {
                        thisErrorID = thisID;
                        Debug.LogError("发现有同种类重复方块的地图！ 地图ID为 "+thisErrorID+" \n 方块1的Type为： "+thisMapData[m].CubeDataList[k].Type
                                       +" 方块1的位置为 "+thisMapData[m].CubeDataList[k].Position+" \n 方块2的Type为： "+thisMapData[m].CubeDataList[n].Type
                                       +" 方块2的位置为 "+thisMapData[m].CubeDataList[n].Position);
                        //     Find = true;
                    }
                    else if (Math.Abs(thisMapData[m].CubeDataList[n].Position.x-thisMapData[m].CubeDataList[k].Position.x)<=0.5f &&
                        Math.Abs(thisMapData[m].CubeDataList[n].Position.y-thisMapData[m].CubeDataList[k].Position.y)<=0.5f&&
                        Math.Abs(thisMapData[m].CubeDataList[n].Position.z-thisMapData[m].CubeDataList[k].Position.z)<=0.5f&&
                        thisMapData[m].CubeDataList[k].ParentItemID==-1&&thisMapData[m].CubeDataList[n].ParentItemID==-1&&
                        !thisMapData[m].CubeDataList[k].Type.ToString().Contains("Castle")&&
                        !thisMapData[m].CubeDataList[k].Type.ToString().Contains("SnowBase")&&
                        !thisMapData[m].CubeDataList[n].Type.ToString().Contains("SnowBase")&&
                        !thisMapData[m].CubeDataList[n].Type.ToString().Contains("Factory_Side")&&
                        !thisMapData[m].CubeDataList[k].Type.ToString().Contains("Factory_Side")&&
                        !thisMapData[m].CubeDataList[n].Type.ToString().Contains("Tower")&&
                        !thisMapData[m].CubeDataList[k].Type.ToString().Contains("Tower")&&
                        !thisMapData[m].CubeDataList[k].Type.ToString().Contains("Castle")&&
                        !thisMapData[m].CubeDataList[k].Type.ToString().Contains("Castle"))
                    {
                        thisErrorID = thisID;
                        Debug.LogError("发现有需要人工排查的重复方块的地图！ 地图ID为 "+thisErrorID+" \n 方块1的Type为： "+thisMapData[m].CubeDataList[k].Type
                                       +" 方块1的位置为 "+thisMapData[m].CubeDataList[k].Position+" \n 方块2的Type为： "+thisMapData[m].CubeDataList[n].Type
                                       +" 方块2的位置为 "+thisMapData[m].CubeDataList[n].Position);
                        //     Find = true;
                    }
                }

//                if (Find)
//                    break;
            }
        }

        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("消息", "遍历完成！请在Console面板中擦汗看信息", "OJBK");
    }
}