using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using AL_Effect;
using UnityEngine;
[CustomEditor(typeof(NewMapEditor))]
public class MapEditor_GUI : Editor
{

    NewMapEditor editor;
    private GameObject ThisMap;
    private GameObject thisCube;
    

    private float showBaseCube = 0;
    private bool showBase_boolean;

    private float showMoveCubeEditpr = 0;  //是否显示可移动地块编辑面板，默认为0:不显示
    private float showCactus = 0;          //是否显示陷阱编辑面板，默认为0:不显示
    private float showGrass = 0;           //是否显示草丛编辑面板，默认为0:不显示
    private float showConveyor = 0;           //是否显示传送带编辑面板，默认为0:不显示

    private float showAdjust_pos=0;
    private float showAdjust_rot=0;

    private int conveyor_count = 0;
    

    private GameObject CubePool;


    void OnEnable()
    {
        editor = target as NewMapEditor;
        CubePool = GameObject.Find("EnvironmentCubePool");
        editor.MoveDirections_List = new List<Vector3>();

    }
//    public override void OnInspectorGUI()
//    {
//
//        base.OnInspectorGUI();
//
//        if (editor.New_MapConfig == null)
//        {
//            editor.New_MapConfig = Resources.Load<HBMapConfig>("MapConfig/HBMapConfig");
//            UpdateCubes();
//        }
//
//        /////
//        //新增基础体时，此处需要添加代码
//        //////
//        #region 选择基础单元
//
//        if (GUILayout.Button("更新基本体", GUILayout.Height(40)))
//            UpdateCubes();
//
//        showBase_boolean = EditorGUILayout.BeginToggleGroup("是否显示基础物体", showBase_boolean);
//        if (showBase_boolean)
//            showBaseCube = 1;
//        else
//            showBaseCube = 0;
//
//      
//        //得到基本体
//        if (EditorGUILayout.BeginFadeGroup(showBaseCube))
//        {
//            for (int i = 0; i < editor.baseCubes.Count; i++)
//            {
//                editor.baseCubes[i] = EditorGUILayout.ObjectField(editor.baseCubes[i].name, editor.baseCubes[i], typeof(GameObject), true) as GameObject;
//            }
//            
//        }
//        #endregion
//        EditorGUILayout.EndToggleGroup();
//        #region  地图风格
//        EditorGUILayout.BeginHorizontal();
//        editor.mapType = EditorGUILayout.TextField("地图风格", editor.mapType);
//        editor._maptype = (NewMapEditor.MapType)EditorGUILayout.EnumPopup(editor._maptype);
//        switch (editor._maptype)
//        {
//            case (NewMapEditor.MapType.GrassLand):
//                editor.mapType = "草地";
//                break;
//            case (NewMapEditor.MapType.Factory):
//                editor.mapType = "工厂";
//                break;
//            case (NewMapEditor.MapType.Ice):
//                editor.mapType = "冰面";
//                break;
//            default:
//                break;
//        }
//        EditorGUILayout.EndHorizontal();
//
//        #endregion
//
//        #region 地图尺寸
//        //  EditorGUILayout.HelpBox("地图尺寸", MessageType.Info);
//        GUILayout.Space(10);
//
//        //锁定地图尺寸，调整时需更改此处代码
//        //   GUI.enabled = false;
//        if (editor.MapSize == Vector3.zero)
//        {
//            editor.MapSize = new Vector3(20, 6, 20);
//        }
//        editor.MapSize = EditorGUILayout.Vector3Field("地图尺寸", editor.MapSize);
//        //   GUI.enabled = true;
//        #endregion
//        
//
//        #region 生成物体名
//        //  EditorGUILayout.HelpBox("本次生成物体名", MessageType.Info);
//        GUILayout.Space(10);
//        editor.CreateName = EditorGUILayout.TextField("生成名", editor.CreateName);
//      //  editor.ItemID= EditorGUILayout.IntField("本次ID", editor.ItemID);
//        #endregion
//
//
//        #region 生成何种基本体
//        //    EditorGUILayout.HelpBox("生成何种基本体", MessageType.Info);
//        GUILayout.Space(10);
//        //设定布局
//        EditorGUILayout.BeginHorizontal();
//        GUILayout.Label("基本体种类",new GUIStyle() { alignment=TextAnchor.LowerLeft,fontSize=15});
//        editor._cubeType = (NewMapEditor.Cube_Type)EditorGUILayout.EnumPopup(editor._cubeType);
//        EditorGUILayout.EndHorizontal();
//
//        GUILayout.Space(10);
//
//        EditorGUILayout.BeginHorizontal();
//        GUILayout.Label("哪个基本体", new GUIStyle() { alignment = TextAnchor.LowerLeft, fontSize = 15 });
//        //花园的选择逻辑
//        if (editor._maptype == NewMapEditor.MapType.GrassLand)
//        {
//            switch (editor._cubeType)
//            {
//                #region 地板与基本方块
//                case (NewMapEditor.Cube_Type.Floor):
//                    editor._grasslandFloor = (NewMapEditor.GrassLand_Floor)EditorGUILayout.EnumPopup(editor._grasslandFloor);
//                    switch (editor._grasslandFloor)
//                    {
//                        #region 生成地板
//                        case (NewMapEditor.GrassLand_Floor.Ground):
//                            editor._HBCubeType = CubeType.GrassLand_Cube;
//                            editor.BaseName = "地板";
//                            editor.thisCube = editor.baseCubes[0];
//                            editor.CreateName = "Floor_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 0f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.CubeCount = new Vector3(editor.MapSize.x, 1, editor.MapSize.z);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成方块
//                        case (NewMapEditor.GrassLand_Floor.GrassLand_Cube):
//                            editor._HBCubeType = CubeType.GrassLand_Cube;
//                            editor.BaseName = "方块";
//                            editor.thisCube = editor.baseCubes[0];
//                            editor.CreateName = "Cube_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        default:
//                            break;
//                        #endregion
//
//
//                    }
//                    break;
//                #endregion
//                #region 斜坡
//                case (NewMapEditor.Cube_Type.Slope):
//                    editor._HBCubeType = CubeType.GrassLand_Slope;
//
//
//
//                    editor.BaseName = "斜坡";
//                    editor.thisCube = editor.baseCubes[7];
//
//                    editor.thisCube = EditorGUILayout.ObjectField(editor.thisCube, typeof(GameObject), true) as GameObject;
//                    editor.CreateName = "Slope_";
//
//                    editor.isMoveCube = false;
//                    editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//
//                    showGrass = 0;
//                    showCactus = 0;
//                    showMoveCubeEditpr = 0;
//                    showConveyor = 0;
//
//                    break;
//                #endregion
//                #region 有安全等级的墙
//                case (NewMapEditor.Cube_Type.Wall):
//                    editor._grasslandWalls = (NewMapEditor.GrassLand_Walls)EditorGUILayout.EnumPopup(editor._grasslandWalls);
//
//                    switch (editor._grasslandWalls)
//                    {
//                        #region 墙
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Wall):
//                            editor._HBCubeType = CubeType.GrassLand_Wall;
//                            editor.BaseName = "墙";
//                            editor.thisCube = editor.baseCubes[1];
//                            editor.CreateName = "Wall_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 盒子
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Box):
//                            editor._HBCubeType = CubeType.GrassLand_Box;
//                            editor.BaseName = "盒子";
//                            editor.thisCube = editor.baseCubes[3];
//                            editor.CreateName = "Box_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 栅栏001
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Palisade_001):
//                            editor._HBCubeType = CubeType.GrassLand_Palisade_001;
//                            editor.BaseName = "栅栏001";
//                            editor.thisCube = editor.baseCubes[4];
//                            editor.CreateName = "Palisade_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 栅栏002
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Palisade_002):
//                            editor._HBCubeType = CubeType.GrassLand_Palisade_002;
//                            editor.BaseName = "栅栏002";
//                            editor.thisCube = editor.baseCubes[5];
//                            editor.CreateName = "Palisade_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 木桶
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Bucket):
//                            editor._HBCubeType = CubeType.GrassLand_Bucket;
//                            editor.BaseName = "木桶";
//                            editor.thisCube = editor.baseCubes[6];
//                            editor.CreateName = "Bucket_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 鸡蛋001
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Egg_001):
//                            editor._HBCubeType = CubeType.GrassLand_Egg_001;
//                            editor.BaseName = "鸡蛋001";
//                            editor.thisCube = editor.baseCubes[8];
//                            editor.CreateName = "Egg_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 鸡蛋002
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Egg_002):
//                            editor._HBCubeType = CubeType.GrassLand_Egg_002;
//                            editor.BaseName = "鸡蛋002";
//                            editor.thisCube = editor.baseCubes[9];
//                            editor.CreateName = "Egg_002_";
//                            
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 石头
//                        case (NewMapEditor.GrassLand_Walls.GrassLand_Stone):
//                            editor._HBCubeType = CubeType.GrassLand_Stone;
//                            editor.BaseName = "石头";
//                            editor.thisCube = editor.baseCubes[10];
//                            editor.CreateName = "Stone_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                            #endregion
//
//                    }
//                    break;
//
//                #endregion
//                case (NewMapEditor.Cube_Type.Gizmos):
//                    editor._grasslandGizmos = (NewMapEditor.GrassLand_Gizmos)EditorGUILayout.EnumPopup(editor._grasslandGizmos);
//                    switch (editor._grasslandGizmos)
//                    {
//                        #region 花
//                        case (NewMapEditor.GrassLand_Gizmos.GrassLand_Flower):
//                            editor._HBCubeType = CubeType.GrassLand_Flower;
//                            editor.BaseName = "花";
//                            editor.thisCube = editor.baseCubes[2];
//                            editor.CreateName = "Flower_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x / 2, 1.5f, -editor.MapSize.z / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 1;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 草
//                        case (NewMapEditor.GrassLand_Gizmos.GrassLand_Grass):
//                            editor._HBCubeType = CubeType.GrassLand_Grass;
//                            editor.BaseName = "草";
//                            editor.thisCube = editor.baseCubes[11];
//                            editor.CreateName = "Grass_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x / 2, 1f, -editor.MapSize.z / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 1;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                            #endregion
//                    }
//                    break;
//                case (NewMapEditor.Cube_Type.MoveFloor):
//                    editor._HBCubeType = CubeType.MoveCube;
//                    editor.BaseName = "可移动地块";
//                    //此处可能需要更改
//                    editor.thisCube = editor.baseCubes[0];
//                    editor.StartPos = new Vector3(-editor.MapSize.x / 2, 1.5f, -editor.MapSize.z / 2);
//                    editor.isMoveCube = true;
//                    editor.CreateName = "MoveFloor_";
//
//                    showGrass = 0;
//                    showCactus = 0;
//                    showMoveCubeEditpr = 1;
//                    showConveyor = 0;
//                    break;
//                case (NewMapEditor.Cube_Type.Conveyor):
//                    editor._grasslandFloor = (NewMapEditor.GrassLand_Floor)EditorGUILayout.EnumPopup(editor._grasslandFloor);
//                    editor._HBCubeType = CubeType.Conveyor;
//                    editor.BaseName = "传送带";
//                    editor.thisCube = editor.baseCubes[16];
//                    editor.isMoveCube = false;
//                    editor.CreateName = "Conveyor_";
//
//                    showGrass = 0;
//                    showCactus = 0;
//                    showMoveCubeEditpr = 0;
//                    showConveyor = 1;
//
//                    break;
//                default:
//                    break;
//            }
//        }
//        ///在此处添加其他风格地图的逻辑
//        else if (editor._maptype == NewMapEditor.MapType.Factory)
//        {
//            switch(editor._cubeType)
//            {
//                case (NewMapEditor.Cube_Type.Floor):
//                    editor._factoryFloor = (NewMapEditor.Factory_Floor)EditorGUILayout.EnumPopup(editor._factoryFloor);
//                    switch (editor._factoryFloor)
//                    {
//                        #region 生成地板
//                        case (NewMapEditor.Factory_Floor.Ground):
//                            editor._HBCubeType = CubeType.Factory_Cube;
//                            editor.BaseName = "地板";
//                            editor.thisCube = editor.baseCubes[18];
//                            editor.CreateName = "Floor_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x*editor.Scale.x / 2, 0f, -editor.MapSize.z * editor.Scale.x / 2 );
//                            editor.CubeCount = new Vector3(editor.MapSize.x, 1, editor.MapSize.z);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成方块
//                        case (NewMapEditor.Factory_Floor.Factory_Cube):
//                            editor._HBCubeType = CubeType.Factory_Cube;
//                            editor.BaseName = "方块";
//                            editor.thisCube = editor.baseCubes[18];
//                            editor.CreateName = "Cube_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        default:
//                            break;
//                        #endregion
//                    }
//
//                    break;
//                case (NewMapEditor.Cube_Type.Wall):
//                    editor._factoryWall = (NewMapEditor.Factory_Walls)EditorGUILayout.EnumPopup(editor._factoryWall);
//                    switch (editor._factoryWall)
//                    {
//                        #region 生成墙
//                        case (NewMapEditor.Factory_Walls.Factory_Wall):
//                            editor._HBCubeType = CubeType.Factory_Wall;
//                            editor.thisCube = editor.baseCubes[19];
//                            editor.CreateName = "Wall_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成警报器
//                        case (NewMapEditor.Factory_Walls.Factory_Alarm):
//                            editor._HBCubeType = CubeType.Factory_Alarm;
//                            editor.thisCube = editor.baseCubes[22];
//                            editor.CreateName = "Alarm_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成盒子
//                        case (NewMapEditor.Factory_Walls.Factory_Box):
//                            editor._HBCubeType = CubeType.Factory_Box;
//                            editor.thisCube = editor.baseCubes[20];
//                            editor.CreateName = "Box_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成斜坡
//                        case (NewMapEditor.Factory_Walls.Factory_Slope):
//                            editor._HBCubeType = CubeType.Factory_Slope;
//                            editor.thisCube = editor.baseCubes[24];
//                            editor.CreateName = "Slope_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成桶
//                        case (NewMapEditor.Factory_Walls.Factory_Bucket):
//                            editor._HBCubeType = CubeType.Factory_Bucket;
//                            editor.thisCube = editor.baseCubes[21];
//                            editor.CreateName = "Bucket_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成螺丝
//                        case (NewMapEditor.Factory_Walls.Factory_Nut):
//                            editor._HBCubeType = CubeType.Factory_Nut;
//                            editor.thisCube = editor.baseCubes[23];
//                            editor.CreateName = "Nut_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成齿轮1
//                        case (NewMapEditor.Factory_Walls.Factory_Gear_001):
//                            editor._HBCubeType = CubeType.Factory_Gear_001;
//                            editor.thisCube = editor.baseCubes[25];
//                            editor.CreateName = "Gear_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成齿轮2
//                        case (NewMapEditor.Factory_Walls.Factory_Gear_002):
//                            editor._HBCubeType = CubeType.Factory_Gear_002;
//                            editor.thisCube = editor.baseCubes[26];
//                            editor.CreateName = "Gear_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成齿轮2
//                        case (NewMapEditor.Factory_Walls.Factory_Gear_003):
//                            editor._HBCubeType = CubeType.Factory_Gear_003;
//                            editor.thisCube = editor.baseCubes[27];
//                            editor.CreateName = "Gear_003_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成栅栏
//                        case (NewMapEditor.Factory_Walls.Factory_Palisade):
//                            editor._HBCubeType = CubeType.Factory_Palisade;
//                            editor.thisCube = editor.baseCubes[28];
//                            editor.CreateName = "Palisade_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成边界001
//                        case (NewMapEditor.Factory_Walls.Factory_Side_001):
//                            editor._HBCubeType = CubeType.Factory_Side_001;
//                            editor.thisCube = editor.baseCubes[29];
//                            editor.CreateName = "Side_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成边界002
//                        case (NewMapEditor.Factory_Walls.Factory_Side_002):
//                            editor._HBCubeType = CubeType.Factory_Side_002;
//                            editor.thisCube = editor.baseCubes[30];
//                            editor.CreateName = "Side_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//
//                        #region 生成石头001
//                        case (NewMapEditor.Factory_Walls.Factory_Stone_001):
//                            editor._HBCubeType = CubeType.Factory_Stone_001;
//                            editor.thisCube = editor.baseCubes[31];
//                            editor.CreateName = "Stone_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成石头002
//                        case (NewMapEditor.Factory_Walls.Factory_Stone_002):
//                            editor._HBCubeType = CubeType.Factory_Stone_002;
//                            editor.thisCube = editor.baseCubes[32];
//                            editor.CreateName = "Stone_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, - editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成引爆器
//                        case (NewMapEditor.Factory_Walls.Factory_Trigger):
//                            editor._HBCubeType = CubeType.Factory_Trigger;
//                            editor.thisCube = editor.baseCubes[33];
//                            editor.CreateName = "Trigger";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成传送带边
//                        case (NewMapEditor.Factory_Walls.Factory_ConveyorSide):
//                            editor._HBCubeType = CubeType.Factory_ConveyorSide;
//                            editor.thisCube = editor.baseCubes[35];
//                            editor.CreateName = "ConveyorSide";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成传送带
//                        case (NewMapEditor.Factory_Walls.Factory_Conveyor):
//                            editor._HBCubeType = CubeType.Factory_Conveyor;
//                            editor.thisCube = editor.baseCubes[34];
//                            editor.CreateName = "Conveyor";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        default:
//                            break;
//                    }
//                    break;
//                #region 可移动地块
//                case (NewMapEditor.Cube_Type.MoveFloor):
//                    editor._HBCubeType = CubeType.MoveCube;
//                    //此处可能需要更改
//                    editor.thisCube = editor.baseCubes[18];
//                    editor.StartPos = new Vector3(-editor.MapSize.x / 2, 1.5f, -editor.MapSize.z / 2);
//                    editor.isMoveCube = true;
//                    editor.CreateName = "MoveFloor_";
//
//                    showGrass = 0;
//                    showCactus = 0;
//                    showMoveCubeEditpr = 1;
//                    showConveyor = 0;
//                    break;
//                    #endregion
//            }
//
//        }
//
//        else if (editor._maptype==NewMapEditor.MapType.Ice)
//        {
//            switch (editor._cubeType)
//            {
//                case (NewMapEditor.Cube_Type.Floor):
//                    editor._iceFloor = (NewMapEditor.Ice_Floor)EditorGUILayout.EnumPopup(editor._iceFloor);
//                    switch (editor._iceFloor)
//                    {
//                        #region 生成地板
//                        case (NewMapEditor.Ice_Floor.Ground):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Cube;
//                            editor.thisCube = editor.baseCubes[36];
//                            //
//                            editor.CreateName = "Floor_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 0f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.CubeCount = new Vector3(editor.MapSize.x, 1, editor.MapSize.z);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成方块
//                        case (NewMapEditor.Ice_Floor.Ice_Cube):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Cube;
//                            editor.thisCube = editor.baseCubes[36];
//                            //
//                            editor.CreateName = "Cube_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成雪糕001
//                        case (NewMapEditor.Ice_Floor.Ice_IceCream_001):
//                            //
//                            editor._HBCubeType = CubeType.Ice_IceCream_001;
//                            editor.thisCube = editor.baseCubes[42];
//                            //
//                            editor.CreateName = "IceCream_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        default:
//                            break;
//                    }
//                    break;
//             
//                case (NewMapEditor.Cube_Type.Wall):
//
//                    editor._iceWall = (NewMapEditor.Ice_Walls)EditorGUILayout.EnumPopup(editor._iceWall);
//                    switch (editor._iceWall)
//                    {
//                        #region 生成墙
//                        case (NewMapEditor.Ice_Walls.Ice_Wall):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Wall;
//                            editor.thisCube = editor.baseCubes[37];
//                            //
//                            editor.CreateName = "Wall_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 0f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                   
//                        #region 生成花
//                        case (NewMapEditor.Ice_Walls.Ice_Flower):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Flower;
//                            editor.thisCube = editor.baseCubes[39];
//                            //
//                            editor.CreateName = "Flower_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成冰淇淋2
//                        case (NewMapEditor.Ice_Walls.Ice_IceCream_002):
//                            //
//                            editor._HBCubeType = CubeType.Ice_IceCream_002;
//                            editor.thisCube = editor.baseCubes[43];
//                            //
//                            editor.CreateName = "IceCream_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成冰淇淋3
//                        case (NewMapEditor.Ice_Walls.Ice_IceCream_003):
//                            //
//                            editor._HBCubeType = CubeType.Ice_IceCream_003;
//                            editor.thisCube = editor.baseCubes[44];
//                            //
//                            editor.CreateName = "IceCream_003_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成冰柱1
//                        case (NewMapEditor.Ice_Walls.Ice_Pillar_001):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Pillar_001;
//                            editor.thisCube = editor.baseCubes[45];
//                            //
//                            editor.CreateName = "Ice_Pillar_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成冰柱2
//                        case (NewMapEditor.Ice_Walls.Ice_Pillar_002):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Pillar_002;
//                            editor.thisCube = editor.baseCubes[46];
//                            //
//                            editor.CreateName = "Ice_Pillar_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成冰柱3
//                        case (NewMapEditor.Ice_Walls.Ice_Pillar_003):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Pillar_003;
//                            editor.thisCube = editor.baseCubes[47];
//                            //
//                            editor.CreateName = "Ice_Pillar_003_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成冰栅栏1
//                        case (NewMapEditor.Ice_Walls.Ice_Palisade_001):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Palisade_001;
//                            editor.thisCube = editor.baseCubes[48];
//                            //
//                            editor.CreateName = "Ice_Palisade_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成冰栅栏2
//                        case (NewMapEditor.Ice_Walls.Ice_Palisade_002):
//                            //
//                            editor._HBCubeType = CubeType.Ice_Palisade_002;
//                            editor.thisCube = editor.baseCubes[49];
//                            //
//                            editor.CreateName = "Ice_Palisade_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//
//                        #region 生成雪1
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_001):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_001;
//                            editor.thisCube = editor.baseCubes[50];
//                            //
//                            editor.CreateName = "Ice_SnowBase_001_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//
//                        #region 生成雪2
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_002):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_002;
//                            editor.thisCube = editor.baseCubes[51];
//                            //
//                            editor.CreateName = "Ice_SnowBase_002_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//
//                        #region 生成雪3
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_003):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_003;
//                            editor.thisCube = editor.baseCubes[52];
//                            //
//                            editor.CreateName = "Ice_SnowBase_003_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成雪4
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_004):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_004;
//                            editor.thisCube = editor.baseCubes[53];
//                            //
//                            editor.CreateName = "Ice_SnowBase_004_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成雪5
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_005):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_005;
//                            editor.thisCube = editor.baseCubes[54];
//                            //
//                            editor.CreateName = "Ice_SnowBase_005_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成雪6
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_006):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_006;
//                            editor.thisCube = editor.baseCubes[55];
//                            //
//                            editor.CreateName = "Ice_SnowBase_006_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成雪7
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_007):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_007;
//                            editor.thisCube = editor.baseCubes[56];
//                            //
//                            editor.CreateName = "Ice_SnowBase_007_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//                        #region 生成雪8
//                        case (NewMapEditor.Ice_Walls.Ice_SnowBase_008):
//                            //
//                            editor._HBCubeType = CubeType.Ice_SnowBase_008;
//                            editor.thisCube = editor.baseCubes[57];
//                            //
//                            editor.CreateName = "Ice_SnowBase_008_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x * editor.Scale.x / 2, 1.5f, -editor.MapSize.z * editor.Scale.x / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 0;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                        #endregion
//
//
//                        default:
//                            break;
//                    }
//                    break;
//                case (NewMapEditor.Cube_Type.Gizmos):
//                    editor._ice_Gizmos= (NewMapEditor.Ice_Gizmos)EditorGUILayout.EnumPopup(editor._ice_Gizmos);
//                    switch (editor._ice_Gizmos)
//                    {
//                        #region 草
//                        case (NewMapEditor.Ice_Gizmos.Ice_Grass):
//                            editor._HBCubeType = CubeType.Ice_Grass;
//                            editor.BaseName = "草";
//                            editor.thisCube = editor.baseCubes[40];
//                            editor.CreateName = "Grass_";
//
//                            editor.StartPos = new Vector3(-editor.MapSize.x / 2, 1f, -editor.MapSize.z / 2);
//                            editor.isMoveCube = false;
//
//                            showGrass = 1;
//                            showCactus = 0;
//                            showMoveCubeEditpr = 0;
//                            showConveyor = 0;
//                            break;
//                            #endregion
//                    }
//                    break;
//                #region 可移动地块
//                case (NewMapEditor.Cube_Type.MoveFloor):
//                    editor._HBCubeType = CubeType.MoveCube;
//                    //此处可能需要更改
//                    editor.thisCube = editor.baseCubes[18];
//                    editor.StartPos = new Vector3(-editor.MapSize.x / 2, 1.5f, -editor.MapSize.z / 2);
//                    editor.isMoveCube = true;
//                    editor.CreateName = "MoveFloor_";
//
//                    showGrass = 0;
//                    showCactus = 0;
//                    showMoveCubeEditpr = 1;
//                    showConveyor = 0;
//                    break;
//                #endregion
//                default:
//                    break;
//            }
//        }
//
//        #endregion
//        //  editor.BaseName = EditorGUILayout.TextField("基础单元名", editor.BaseName);
//        // editor.whichCube = (NewMapEditor.Cubes_GrassLand)EditorGUILayout.EnumPopup(editor.whichCube);
//        EditorGUILayout.EndHorizontal();
//
//        GUILayout.Space(10);
//
//        #region 可移动地块生成逻辑
//        //
//        if (EditorGUILayout.BeginFadeGroup(showMoveCubeEditpr))
//        {
//            EditorGUILayout.HelpBox("数目一定要是偶数", MessageType.Warning);
//            editor.Size = EditorGUILayout.IntField("路径点数目", editor.Size);
//
//            for (int i = 0; i < editor.Size; i++)
//            {
//                editor.MoveDirections_List.Add(new Vector3(0, 0, 0));
//                editor.MoveDirections_List[i] = EditorGUILayout.Vector3Field("路径点", editor.MoveDirections_List[i]);
//            }
//            editor.MoveDirections = new Vector3[20];
//            for (int j = 0; j < editor.MoveDirections_List.Count; j++)
//            {
//                if (j >= editor.Size)
//                {
//                    editor.MoveDirections_List.Remove(editor.MoveDirections_List[j]);
//                    break;
//                }
//                editor.MoveDirections[j] = editor.MoveDirections_List[j];
//
//            }
//
//
//            //速度
//            editor.speed = EditorGUILayout.FloatField("移动速度", editor.speed);
//
//            //停留时间
//            editor.stayTime = EditorGUILayout.FloatField("停留时间", editor.stayTime);
//        }
//        #endregion
//
//        #region 陷阱生成逻辑
//        if (EditorGUILayout.BeginFadeGroup(showCactus))
//        {
//            editor.damagePercent = EditorGUILayout.Slider("伤害百分比", editor.damagePercent, 0f, 100.0f);
//            editor.hitForce = EditorGUILayout.Slider("击飞力度", editor.hitForce, 0f, 100.0f);
//            editor.hitHeight = EditorGUILayout.Slider("击飞高度", editor.hitHeight, 0f, 6.0f);
//            editor.immyneInterval = EditorGUILayout.Slider("免疫时长", editor.immyneInterval, 0f, 5.0f);
//            editor.effectID = "50100";
//        }
//
//        #endregion
//
//        #region 草丛生成逻辑
//        if (EditorGUILayout.BeginFadeGroup(showGrass))
//        {
//            editor.visiableDistance = EditorGUILayout.Slider("可视距离", editor.visiableDistance, 0f, 10.0f);
//        }
//
//        #endregion
//        #region 传送带生成逻辑
//        if (EditorGUILayout.BeginFadeGroup(showConveyor))
//        {
//        //    EditorGUILayout.HelpBox("传送带无法调整位置或方向！请认真填写坐标！", MessageType.Error);
//         //   editor.Convetor_startPos = EditorGUILayout.Vector3Field("初始位置", editor.Convetor_startPos);
//        //    editor.Convetor_endPos = EditorGUILayout.Vector3Field("结束位置", editor.Convetor_endPos);
//            editor.conveyor_Speed = EditorGUILayout.FloatField("速度", editor.conveyor_Speed);
//
//         //   if (editor.Convetor_startPos.x - editor.Convetor_endPos.x != 0)
//        //    {
//         //       conveyor_count = (int)Mathf.Abs(editor.Convetor_startPos.x - editor.Convetor_endPos.x) + 1;
//         //   }
//         //   else if (editor.Convetor_startPos.z - editor.Convetor_endPos.z != 0)
//          //  {
//          //      conveyor_count = (int)Mathf.Abs(editor.Convetor_startPos.z - editor.Convetor_endPos.z) + 1;
//         //   }
//
//        }
//
//        #endregion
//        editor.thisCube = EditorGUILayout.ObjectField("本次将生成的基本体", editor.thisCube, typeof(GameObject), true) as GameObject;
//
//        #region 生成大小
//        if (showConveyor == 0)
//        {
//            //    EditorGUILayout.HelpBox("生成大小", MessageType.Info);
//            GUILayout.Space(10);
//            editor.CubeCount = EditorGUILayout.Vector3Field("各方向上的生成数量", editor.CubeCount);
//
//
//
//            if (editor.CubeCount.x == 0)
//                editor.CubeCount.x = 1;
//            if (editor.CubeCount.y == 0)
//                editor.CubeCount.y = 1;
//            if (editor.CubeCount.z == 0)
//                editor.CubeCount.z = 1;
//        }
//        #endregion
//
//        #region 生成比例
//        //   EditorGUILayout.HelpBox("方块比例", MessageType.Info);
//        GUILayout.Space(10);
//        editor.Scale = EditorGUILayout.Vector3Field("生成物块的大小比例",editor.Scale);
//        if (editor.Scale == Vector3.zero)
//            editor.Scale = new Vector3(1.5f,1.5f,1.5f);
//
//        #endregion
//
//        #region 生成或调整位置
//        //   EditorGUILayout.HelpBox("生成或调整位置", MessageType.Info);
//
//        //定死初始生成位置，为地图左下角
//        GUILayout.Space(10);
//        GUI.enabled = false;
//        editor.StartPos = EditorGUILayout.Vector3Field("初始生成位置", editor.StartPos);
//        GUI.enabled = true;
//        GUILayout.Space(10);
//
//        editor.isPos = EditorGUILayout.BeginToggleGroup("是否有需要调整位置的物体", editor.isPos);
//
//        if (editor.isPos)
//        {
//            showAdjust_pos = 1;
//        }
//        else
//            showAdjust_pos = 0;
//
//        EditorGUILayout.EndToggleGroup();
//
//        if (EditorGUILayout.BeginFadeGroup(showAdjust_pos))
//        {
//            editor.AdjustObject_Pos = EditorGUILayout.ObjectField("需要调整位置的物体", editor.AdjustObject_Pos, typeof(GameObject), true) as GameObject;
//
//            //三方向的调整值，初始默认为地图左下角原点
//            editor.Ad_Pos_X = EditorGUILayout.Slider("X坐标", editor.Ad_Pos_X, editor.StartPos.x, editor.MapSize.x / 2 - 1);
//            editor.Ad_Pos_Y = EditorGUILayout.Slider("Y坐标", editor.Ad_Pos_Y, 0, editor.MapSize.y);
//            editor.Ad_Pos_Z = EditorGUILayout.Slider("Z坐标", editor.Ad_Pos_Z, editor.StartPos.z, editor.MapSize.z / 2 - 1);
//
//
//            //清零原点
//         //   EditorGUILayout.HelpBox("更换调整物体时注意清零位置", MessageType.Info);
//            if (GUILayout.Button("清零", GUILayout.Height(35)))
//            {
//                editor.AdjustObject_Pos = null;
//                editor.Ad_Pos_X = editor.StartPos.x;
//                editor.Ad_Pos_Y = editor.StartPos.y;
//                editor.Ad_Pos_Z = editor.StartPos.z;
//            }
//            #region 调整位置
//
//            Vector3 newPos = new Vector3(editor.Ad_Pos_X, editor.Ad_Pos_Y, editor.Ad_Pos_Z);
//
//            newPos.x = editor.StartPos.x + ((int)newPos.x - editor.StartPos.x)* editor.Scale.x;
//            newPos.y = (int)newPos.y * editor.Scale.y;
//            newPos.z = editor.StartPos.z + ((int)newPos.z - editor.StartPos.z) * editor.Scale.z;
//            if (editor.AdjustObject_Pos == null)
//            {
//                editor.AdjustObject_Pos = GameObject.Find("ControllGameObject").gameObject;
//                newPos.x = 0;
//                newPos.y = 0;
//                newPos.z = 0;
//            }
//
//
//            editor.AdjustObject_Pos.transform.position = newPos;
//        }
//        #endregion
//        #endregion
//
//
//        #region 生成或调整方向
//     //   EditorGUILayout.HelpBox("生成或调整方向", MessageType.Info);
//      //  editor.CubeDirection = EditorGUILayout.Vector3Field("生成方向", editor.CubeDirection);
//    //    GUILayout.Space(10);
//
//        editor.isDir = EditorGUILayout.BeginToggleGroup("是否有需要调整方向的物体", editor.isDir);
//
//
//        if (editor.isDir)
//        {
//            showAdjust_rot = 1;
//        }
//        else
//            showAdjust_rot = 0;
//        EditorGUILayout.EndToggleGroup();
//        if (EditorGUILayout.BeginFadeGroup(showAdjust_rot))
//        {
//            editor.AdjustOnject_Dir = EditorGUILayout.ObjectField("需要调整方向的物体", editor.AdjustOnject_Dir, typeof(GameObject), true) as GameObject;
//
//            #region 调整方向
//            EditorGUILayout.BeginHorizontal();
//            EditorGUILayout.TextField("X轴");
//            if (GUILayout.Button("顺时针转90度", GUILayout.Height(30)))
//            {
//                Space rotateSpace = Space.Self;
//                Vector3 rot;
//                rot.x = (int)editor.AdjustOnject_Dir.transform.rotation.x;
//                rot.y = (int)editor.AdjustOnject_Dir.transform.rotation.y;
//                rot.z = (int)editor.AdjustOnject_Dir.transform.rotation.z;
//                rot.x += 90.0f;
//                rot.x = (int)rot.x;
//                if (rot.x >= 359.0f)
//                    rot.x = 0;
//
//                editor.AdjustOnject_Dir.transform.Rotate(rot, rotateSpace);
//
//            }
//            EditorGUILayout.TextField("Y轴");
//            if (GUILayout.Button("顺时针转90度", GUILayout.Height(30)))
//            {
//                Space rotateSpace = Space.Self;
//                Vector3 rot;
//                rot.x = (int)editor.AdjustOnject_Dir.transform.rotation.x;
//                rot.y = (int)editor.AdjustOnject_Dir.transform.rotation.y;
//                rot.z = (int)editor.AdjustOnject_Dir.transform.rotation.z;
//                rot.y += 90.0f;
//                rot.y = (int)rot.y;
//                if (rot.y >= 359.0f)
//                    rot.y = 0;
//                editor.AdjustOnject_Dir.transform.Rotate(rot, rotateSpace);
//            }
//            EditorGUILayout.TextField("Z轴");
//            if (GUILayout.Button("顺时针转90度", GUILayout.Height(30)))
//            {
//                Space rotateSpace = Space.Self;
//                Vector3 rot;
//                rot.x = (int)editor.AdjustOnject_Dir.transform.rotation.x;
//                rot.y = (int)editor.AdjustOnject_Dir.transform.rotation.y;
//                rot.z = (int)editor.AdjustOnject_Dir.transform.rotation.z;
//                rot.z += 90.0f;
//                rot.z = (int)rot.z;
//                if (rot.z >= 359.0f)
//                    rot.z = 0;
//                editor.AdjustOnject_Dir.transform.Rotate(rot, rotateSpace);
//            }
//            EditorGUILayout.EndHorizontal();
//        }
//        #endregion
//        #endregion
//
//
//        if (GUILayout.Button("生成方块", GUILayout.Height(40)))
//        {
//            switch (editor._HBCubeType)
//            {
//                case (CubeType.MoveCube):
//                    CreateMoveCube(editor.CreateName, editor.CubeCount, editor.thisCube, editor._cubeType, editor._HBCubeType, editor.StartPos, editor.MoveDirections, editor.speed, editor.stayTime);
//                    break;
//                case (CubeType.Conveyor):
//                    CreateConveyor(editor.CreateName, editor.Convetor_startPos, editor.Convetor_endPos,editor.conveyor_Speed, editor.thisCube, editor._cubeType,editor._HBCubeType);
//                    break;
//                default:
//                    CreateCubes(editor.CreateName, editor.CubeCount, editor.thisCube, editor._cubeType, editor._HBCubeType, editor.StartPos);
//                    break;
//            }
//        }
//
//        #region 扩展功能
//        if (GUILayout.Button("删除重复方块", GUILayout.Height(40)))
//            DeleteCubes();
//
//      
//        if (GUILayout.Button("是否可随机旋转", GUILayout.Height(40)))
//            RandomRotCubes();
//        /*
//        EditorGUILayout.HelpBox("重置地图ID", MessageType.Info);
//        if (GUILayout.Button("重置地图id", GUILayout.Height(40)))
//            DeleteRepeatID();
//            */
//            
//        editor.MapId = EditorGUILayout.TextField("地图id", editor.MapId);
//        if (GUILayout.Button("存储地图信息", GUILayout.Height(40)))
//            New_SaveMapConmfig(editor.MapId);
//
//
//        
//        if (GUILayout.Button("加载地图信息", GUILayout.Height(40)))
//            New_LoadMapConfig(editor.MapId);
//        
//        if (GUILayout.Button("删除地图信息", GUILayout.Height(40)))
//            DeleteMap(editor.MapId);
//        #endregion
//
//
//    }
//
//    /// <summary>
//    /// 生成方快们    /// </summary>
//    /// <param name="name">生成的组名</param>
//    /// <param name="count">生成大小</param>
//    /// <param name="which">生成哪种基本体</param>
//    /// <param name="thisType">生成基本体破坏种类</param>
//    /// <param name="pos">生成位置</param>
//    /// <param name="dir">生成方向</param>
//    public void CreateCubes(string name, Vector3 count, GameObject which, NewMapEditor.Cube_Type thisType, CubeType baseType, Vector3 StartPos)
//    {
//        //本次将生成的方块总数
//        int allCubeCount = 0;
//
//        #region  生成父物体,确定生成名
//      //  GameObject cube_Father = new GameObject(string.Format("{0}{1}{2}{3}", name, "_", editor.thisCreateID.ToString(), "_father"));
//
//
//        editor.thisCreateID += 1;
//        #endregion
//        //角度
//        float whichRot = 0;
//        GameObject go = null;
//
//        switch (thisType)
//        {
//            #region 生成地板的逻辑
//            case (NewMapEditor.Cube_Type.Floor):
//                    editor.thisCreateID = 0;
//                allCubeCount = Convert.ToInt32(count.x * count.y * count.z);
//       //         cube_Father.transform.position = editor.StartPos;
//                for (int x = 0; x < count.x; x++)
//                    for (int y = 0; y < count.y; y++)
//                        for (int z = 0; z < count.z; z++)
//                        {
//                            whichRot = (int)GetRanodomRotation(count);
//
//                            go = GameObject.Instantiate(which, new Vector3(StartPos.x + x * editor.Scale.x, StartPos.y + y * editor.Scale.x, StartPos.z + z * editor.Scale.x), Quaternion.Euler(which.transform.localRotation.x, whichRot, which.transform.localRotation.z));
//                            go.transform.localScale = editor.Scale;
//                            go.tag = "Ground";
//                            HBCubeController thisController = go.GetOrAddComponent<HBCubeController>();
//                            thisController.Type = baseType;
//                            thisController.ifDepend = true;
//                            thisController.BasicCube = true;
//                            thisController._dependLenght = 1.5f;
//                            //为-1时，表示该物体没有父物体
//                            thisController.ParentID = -1;
//
//                            go.name = name + go.GetComponent<HBCubeController>().ItemID.ToString();
//                            go.transform.parent = CubePool.transform;
//                        }
//             //   cube_Father.transform.parent = CubePool.transform;
//                break;
//            #endregion
//            #region 生成斜坡的逻辑
//            case (NewMapEditor.Cube_Type.Slope):
//            //    cube_Father.transform.position = editor.StartPos;
//                int cubeCount = (int)count.z; //此层该生成多少方块
//                for (int x = 0; x < count.x; x++)
//                {
//                    cubeCount = (int)count.z;
//                    for (int z = 0; z < count.z; z++)
//                        for (int thisCount = 0; thisCount <= 2 * (cubeCount - 1); thisCount++)
//                        {
//                            if (thisCount == 2 * (cubeCount - 1))//需要生成斜坡时
//                            {
//                                go = GameObject.Instantiate(which, new Vector3(StartPos.x + x*editor.Scale.x, StartPos.y + z * editor.Scale.x, StartPos.z + thisCount * editor.Scale.x), Quaternion.Euler(which.transform.localRotation.x, which.transform.localRotation.y - 90.0f, editor.baseCubes[0].transform.localRotation.z));
//                                go.tag = "Ground";
//                                go.transform.localScale = editor.Scale;
//                                HBCubeController thisController = go.GetOrAddComponent<HBCubeController>();
//                                thisController.Type = baseType;
//                                thisController.BasicCube = true;
//                                thisController._dependLenght = 1.5f;
//                                thisController.ifDepend = true;
//                                //为-1时，表示该物体没有父物体
//                                thisController.ParentID = -1;
//                            //    editor.ItemID += 1;
//                          //      thisController.ItemID = editor.ItemID;
//                         //       go.transform.parent = cube_Father.transform;
//                                go.name = name + thisController.ItemID.ToString();
//                                cubeCount -= 1;    //下一层生成方块数-2
//                                go.transform.parent = CubePool.transform;
//                            }
//                            else     //生成方块
//                            {
//                                whichRot = (int)GetRanodomRotation(count);
//                                go = GameObject.Instantiate(editor.baseCubes[0], new Vector3(StartPos.x + x * editor.Scale.x, StartPos.y + z * editor.Scale.x, StartPos.z + thisCount * editor.Scale.x), Quaternion.Euler(editor.baseCubes[0].transform.localRotation.x, whichRot, editor.baseCubes[0].transform.localRotation.z));
//                                go.tag = "Ground";
//                                go.transform.localScale = editor.Scale;
//                                HBCubeController thisController = go.GetOrAddComponent<HBCubeController>();
//                                thisController.Type = CubeType.GrassLand_Cube;
//                                thisController.ifDepend = true;
//                                thisController.BasicCube = true;
//                                thisController._dependLenght = 1.5f;
//                                //为-1时，表示该物体没有父物体
//                                thisController.ParentID = -1;
//                            //    editor.ItemID += 1;
//                             //   thisController.ItemID = editor.ItemID;
//                            //    go.transform.parent = cube_Father.transform;
//                                go.name = name + thisController.ItemID.ToString();
//                                go.transform.parent = CubePool.transform;
//                            }
//                        }
//                }
//             //   cube_Father.transform.parent = CubePool.transform;
//                break;
//            #endregion
//            #region 生成安全等级墙的逻辑
//            case (NewMapEditor.Cube_Type.Wall):
//                allCubeCount = Convert.ToInt32(count.x * count.y * count.z);
//             //   cube_Father.transform.position = editor.StartPos;
//                for (int x = 0; x < count.x; x++)
//                    for (int y = 0; y < count.y; y++)
//                        for (int z = 0; z < count.z; z++)
//                        {
//                            //随机角度
//                            if (baseType == CubeType.GrassLand_Wall||baseType==CubeType.Factory_Wall)
//                                whichRot = GetRanodomRotation(count);
//                            else
//                                whichRot = which.transform.localRotation.y;
//                            go = GameObject.Instantiate(which, new Vector3(StartPos.x + x * editor.Scale.x, StartPos.y + y * editor.Scale.x, StartPos.z + z *editor.Scale.x), Quaternion.Euler(which.transform.localRotation.x, whichRot, which.transform.localRotation.z));
//
//                            go.transform.localScale = editor.Scale;
//                            
//
//                            HBCubeController thisController = go.GetOrAddComponent<HBCubeController>();
//                            thisController.Type = baseType;
//                            thisController.BasicCube = true;
//                            thisController._dependLenght = 1.5f;
//                            thisController.ifDepend = true;
//                            //为-1时，表示该物体没有父物体
//                            thisController.ParentID = -1;
//                         //   editor.ItemID += 1;
//                         //   thisController.ItemID = editor.ItemID;
//                         //   if (baseType != CubeType.GrassLand_Box&&baseType!=CubeType.GrassLand_Wall)
//                         //   {
//                         //       DestroyImmediate(cube_Father);
//                              //  cube_Father.transform.parent = CubePool.transform;
//                        //    }
//                        //    else
//                           // {
//                        //        go.transform.parent = cube_Father.transform;
//                        ////        cube_Father.transform.parent = CubePool.transform;
//                               
//                         //   }
//                            go.name = name + thisController.ItemID.ToString();
//                            go.transform.parent = CubePool.transform;
//                        }
//                break;
//            #endregion
//            #region 生成装饰物的逻辑
//            case (NewMapEditor.Cube_Type.Gizmos):
//
//                allCubeCount = Convert.ToInt32(count.x * count.y * count.z);
//              //  cube_Father.transform.position = editor.StartPos;
//                for (int x = 0; x < count.x; x++)
//                    for (int y = 0; y < count.y; y++)
//                        for (int z = 0; z < count.z; z++)
//                        {
//                            //随机角度
//                            whichRot = GetRanodomRotation(count);
//
//                            go = GameObject.Instantiate(which, new Vector3(StartPos.x + x * editor.Scale.x, StartPos.y + y * editor.Scale.x, StartPos.z + z * editor.Scale.x), Quaternion.Euler(which.transform.localRotation.x, whichRot, which.transform.localRotation.z));
//
//                            go.transform.localScale = editor.Scale;
//                            HBCubeController thisController = go.GetOrAddComponent<HBCubeController>();
//                            thisController.Type = baseType;
//                            thisController.BasicCube = false;
//                            thisController.ifDepend = true;
//                            thisController._dependLenght = 1.5f;
//
//                            //为-1时，表示该物体没有父物体
//                            thisController.ParentID = -1;
//                           // editor.ItemID += 1;
//                          //  thisController.ItemID = editor.ItemID;
//
//                            //添加草丛参数
//                            thisController.Init(baseType);
//                         //   DestroyImmediate(cube_Father);
//                            go.transform.parent = CubePool.transform;
//                            go.GetComponent<AlEffectThickGrowthOfGrass>().visibleDistance = editor.visiableDistance;
//                            
//                            go.name = name + thisController.ItemID.ToString();
//                            go.transform.parent = CubePool.transform;
//                        }
//                
//                break;
//                #endregion
//        }
//        
//    }
//    /// <summary>
//    /// 创建可移动地块
//    /// </summary>
//    public void CreateMoveCube(string name, Vector3 count, GameObject which, NewMapEditor.Cube_Type thisType, CubeType baseType, Vector3 StartPos, Vector3[] dir, float speed, float stayTime)
//    {
//        int allCubeCount = 0;
//        #region  生成父物体,确定生成名
//        GameObject cube_Father = new GameObject(string.Format("{0}{1}{2}{3}", name, "_", editor.thisCreateID.ToString(), "_father"));
//
//        editor.thisCreateID += 1;
//        #endregion
//        //角度
//        float whichRot = 0;
//        GameObject go = null;
//
//        allCubeCount = Convert.ToInt32(count.x * count.y * count.z);
//        cube_Father.transform.position = editor.StartPos;
//
//        HBCubeController father_controller = cube_Father.GetOrAddComponent<HBCubeController>();
//
//        father_controller.Type = CubeType.Move_ParentItem;
//        editor.ItemID += 1;
//        editor.FatherID = editor.ItemID;
//        // cube_Father.GetComponent<HBCubeController>().ParentID = editor.FatherID;
//        father_controller.ItemID = editor.ItemID;
//        father_controller.ParentID = -1;
//        //  father_controller.Init(CubeType.MoveCube);
//        AlEffectMove this_Move = cube_Father.GetOrAddComponent<AlEffectMove>();
//
//        List<Vector3> _moveDirections = new List<Vector3>();
//        for (int i = 0; i < dir.Length; i++)
//        {
//            if (dir[i] != new Vector3(0, 0, 0))
//                _moveDirections.Add(dir[i]);
//        }
//
//        this_Move.moveDirection = new Vector3[_moveDirections.Count];
//
//
//        for (int j = 0; j < _moveDirections.Count; j++)
//        {
//            this_Move.moveDirection[j] = _moveDirections[j];
//        }
//
//        this_Move.speed = speed;
//        this_Move.stayTime = stayTime;
//
//        for (int x = 0; x < count.x; x++)
//            for (int y = 0; y < count.y; y++)
//                for (int z = 0; z < count.z; z++)
//                {
//                    //随机角度
//                    whichRot = GetRanodomRotation(count);
//
//                    go = GameObject.Instantiate(editor.thisCube, new Vector3(StartPos.x + x * editor.Scale.x, StartPos.y + y * editor.Scale.x, StartPos.z + z * editor.Scale.x), Quaternion.Euler(which.transform.localRotation.x, whichRot, which.transform.localRotation.z));
//                    //    go.AddComponent<HBCubeController>();
//                    go.transform.localScale = editor.Scale;
//                    go.tag = "Ground";
//                    go.GetOrAddComponent<HBCubeController>();
//                    HBCubeController thisController = go.GetComponent<HBCubeController>();
//                    thisController.Type = baseType;
//                    thisController._dependLenght = 1.5f;
//
//                    thisController.BasicCube = true;
//
//                    //   go.GetComponent<HBCubeController>().cubePath = HBItemData.ItemPath[10];
//                    //   go.GetComponent<HBCubeController>().piecesPath.Add(HBItemData.PiecePath[10]);
//
//                    //为-1时，表示该物体没有父物体
//                    thisController.ParentID = editor.FatherID;
//                    editor.ItemID += 1;
//                    thisController.ItemID = editor.ItemID;
//
//                    go.transform.parent = cube_Father.transform;
//                    go.name = name + thisController.ItemID.ToString();
//
//                }
//        cube_Father.transform.parent = CubePool.transform;
//
//    }
//
//    /// <summary>
//    /// 创建传送带
//    /// </summary>
//    public void CreateConveyor(string name, Vector3 startPos, Vector3 endPos, float speed, GameObject which, NewMapEditor.Cube_Type thisType, CubeType baseType)
//    {
//        //本次将生成的方块总数
//        int allCubeCount_X = 0;
//        int allCubeCount_Z = 0;
//        if (startPos.x - endPos.x != 0)
//            allCubeCount_X = (int)Mathf.Abs(startPos.x - endPos.x);
//        else
//            allCubeCount_Z = (int)Mathf.Abs(startPos.z - endPos.z);
//
//
//        #region  生成父物体,确定生成名
//        GameObject cube_Father = new GameObject(string.Format("{0}{1}{2}{3}", name, "_", editor.thisCreateID.ToString(), "_father"));
//
//        #endregion
//        //角度
//        GameObject go = null;
//        cube_Father.transform.position = editor.StartPos;
//        if (allCubeCount_X != 0)
//        {
//            for (int i = 0; i < allCubeCount_X; i++)
//            {
//                go = GameObject.Instantiate(which, new Vector3(startPos.x + i, startPos.y, startPos.z), which.transform.rotation);
//
//                HBCubeController thisController = go.GetOrAddComponent<HBCubeController>();
//                thisController.Type = baseType;
//                thisController.BasicCube = false;
//
//                //为-1时，表示该物体没有父物体
//             //   thisController.ParentID = -1;
//             //   editor.ItemID += 1;
//                thisController.ItemID = editor.ItemID;
//
//                //添加传送带参数
//                thisController.Init(baseType);
//                go.transform.parent = cube_Father.transform;
////                go.GetComponent<LXEffectConveyor>()._startPos= cube_Father.transform.position;
////                go.GetComponent<LXEffectConveyor>()._endPos= endPos;
//                go.GetComponent<LXEffectConveyor>().Speed = editor.conveyor_Speed;
//
//                go.name = name + thisController.ItemID.ToString();
//            }
//        }
//        if (allCubeCount_Z != 0)
//        {
//            for (int i = 0; i < allCubeCount_Z; i++)
//            {
//                go = GameObject.Instantiate(which, new Vector3(startPos.x , startPos.y, startPos.z+i), which.transform.rotation);
//
//                HBCubeController thisController = go.GetOrAddComponent<HBCubeController>();
//                go.GetOrAddComponent<LXEffectConveyor>();
//                thisController.Type = baseType;
//                thisController.BasicCube = false;
//
//                //为-1时，表示该物体没有父物体
//             //   thisController.ParentID = -1;
//            //    editor.ItemID += 1;
//                thisController.ItemID = editor.ItemID;
//
//                //添加传送带参数
//                thisController.Init(baseType);
//                go.transform.parent = cube_Father.transform;
////                go.GetComponent<LXEffectConveyor>()._startPos = startPos;
////                go.GetComponent<LXEffectConveyor>()._endPos = endPos;
//                go.GetComponent<LXEffectConveyor>().Speed = editor.conveyor_Speed;
//
//                go.name = name + thisController.ItemID.ToString();
//            }
//        }
//
//    }
//    
//
//  /// <summary>
//  /// 给可进行随机旋转的方块随机角度
//  /// </summary>
//    public void RandomRotCubes()
//    {
//
//        List<GameObject> cubes = new List<GameObject>();
//        foreach (HBCubeController cubeController in CubePool.transform.GetComponentsInChildren<HBCubeController>())
//        {
//            cubes.Add(cubeController.gameObject);
//        }
//        for (int i = 0; i < cubes.Count; i++)
//        {
//            if (cubes[i].GetComponent<HBCubeController>().Type == CubeType.GrassLand_Box ||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.GrassLand_Cube||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.GrassLand_Wall||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.GrassLand_Bucket||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.GrassLand_Egg_001||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.GrassLand_Egg_002 ||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.Factory_Alarm ||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.Factory_Box ||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.Factory_Bucket ||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.Factory_Cube ||
//                cubes[i].GetComponent<HBCubeController>().Type == CubeType.Factory_Wall)
//            {
//                cubes[i].GetComponent<HBCubeController>().RandomRotation = true;
//            }
//        }
//    }
//    /// <summary>
//    /// 得到随机旋转角度
//    /// </summary>
//    public float GetRanodomRotation(Vector3 count)
//    {
//        float whichRot = 0.0f;
//        for (int x = 0; x < count.x; x++)
//            for (int y = 0; y < count.y; y++)
//                for (int z = 0; z < count.z; z++)
//                {
//                    //随机角度
//                    whichRot = UnityEngine.Random.Range(0, 4);
//                    switch ((int)UnityEngine.Random.Range(0, 4))
//                    {
//                        case 0:
//                            whichRot = 0.0f;
//                            break;
//                        case 1:
//                            whichRot = 90.0f;
//                            break;
//                        case 2:
//                            whichRot = 180.0f;
//                            break;
//                        case 3:
//                            whichRot = 270.0f;
//                            break;
//                        default:
//                            break;
//                    }
//                }
//        return whichRot;
//    }
//    /// <summary>
//    /// 更新基本体
//    /// </summary>
//    public void UpdateCubes()
//    {
//        editor.baseCubes.Clear();
//        editor.cubes = new GameObject[HBItemData.ItemPath.Count];
//            editor.baseConveyor = Resources.Load<GameObject>("HappyBombPrefabs/NewBasePrefabs/Conveyor");
//        
//            for (int i = 0; i < editor.cubes.Length; i++)
//            {
//            Debugerr.LogLXZ("path is "+HBItemData.ItemPath[i]);
//                editor.cubes[i] = Resources.Load<GameObject>(HBItemData.ItemPath[i]);
//                editor.baseCubes.Add(editor.cubes[i]);
//            }
//
//    }
//    /// <summary>
//    /// 删除重复方块
//    /// </summary>
//    public void DeleteCubes()
//    {
//        List<GameObject> cubes = new List<GameObject>();
//        foreach (HBCubeController cubeController in CubePool.transform.GetComponentsInChildren<HBCubeController>())
//        {
//            cubes.Add(cubeController.gameObject);
//        }
//        //   cubes = GameObject.FindGameObjectsWithTag("Ground");
//        for (int i = 0; i < cubes.Count; i++)
//            for (int j = i + 1; j < cubes.Count; j++)
//            {
//                if (cubes[i] != null && cubes[i].GetComponent<HBCubeController>().Type != CubeType.Move_ParentItem && cubes[j] != null && Math.Abs(cubes[i].transform.position.x - cubes[j].transform.position.x) < 0.1f && Math.Abs(cubes[i].transform.position.y - cubes[j].transform.position.y) < 0.1f && Math.Abs(cubes[i].transform.position.z - cubes[j].transform.position.z) < 0.1f)
//                {
//                    //添加判断条件，优先删除普通地块
//                    if (cubes[i].GetComponent<HBCubeController>().Type < cubes[j].GetComponent<HBCubeController>().Type)// == CubeType.Break)
//                    {
//                        DestroyImmediate(cubes[i].gameObject);
//                    }
//                    else
//                    {
//                        DestroyImmediate(cubes[j].gameObject);
//                    }
//                }
//            }
//    }
//
//    /// <summary>
//    /// 保存地图信息
//    /// </summary>
//    /// <param name="id"></param>
//    public void New_SaveMapConmfig(string id)
//    {
//        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
//        List<HBCubeController> cubeControllers = new List<HBCubeController>();
//
//        foreach (HBCubeController cubeController in cubePool.transform.GetComponentsInChildren<HBCubeController>())
//        {
//            cubeControllers.Add(cubeController);
//        }
//
//        HBMapConfig.MapData mapData = new HBMapConfig.MapData();
//        mapData.MapID = int.Parse(editor.MapId);
//
//        Dictionary<int, Transform> childDic = new Dictionary<int, Transform>();
//        for (int i = 0; i < cubeControllers.Count; i++)
//        {
//            GameObject item = cubeControllers[i].gameObject;
//            HBCubeController hbCubeController = cubeControllers[i];
//            int parentID = -1;
//            if (item.transform.parent.GetComponent<HBCubeController>())
//            {
//                parentID = -100;
//                childDic.Add(item.GetInstanceID(), item.transform);
//            }
//
//            string effectParams = "";
//            AlEffectBase effectBase = item.GetComponent<AlEffectBase>();
//            if (effectBase != null)
//            {
//                effectParams = effectBase.CreatParams();
//            }
//            if (hbCubeController.Type == CubeType.Factory_Box || hbCubeController.Type == CubeType.Factory_Bucket ||
//             hbCubeController.Type == CubeType.Factory_Cube || hbCubeController.Type == CubeType.Factory_Wall ||
//             hbCubeController.Type == CubeType.GrassLand_Box || hbCubeController.Type == CubeType.GrassLand_Cube ||
//             hbCubeController.Type == CubeType.GrassLand_MiddleCube || hbCubeController.Type == CubeType.GrassLand_Wall ||
//             hbCubeController.Type == CubeType.GrassLand_Cube_002 || hbCubeController.Type == CubeType.Ice_Box ||
//             hbCubeController.Type == CubeType.Ice_Cube || hbCubeController.Type == CubeType.Ice_SnowCube ||
//             hbCubeController.Type == CubeType.Ice_Wall)
//            {
//                hbCubeController.RandomRotation = true;
//            }
//            HBMapConfig.CubeData cubeData = new HBMapConfig.CubeData()
//            {
//                ItemID = item.GetInstanceID(),
//                Type = hbCubeController.Type,
//                ParentItemID = parentID,
//                Position = item.transform.localPosition,
//                RandomRotation = hbCubeController.RandomRotation,
//                Rotation = item.transform.rotation,
//                Scale = item.transform.localScale,
//                
//               
//                EffectParams = effectParams,
//            };
//            mapData.CubeDataList.Add(cubeData);
//        }
//
//        //保存特殊父物体ID
//        for (int i = 0; i < mapData.CubeDataList.Count; i++)
//        {
//            if (mapData.CubeDataList[i].ParentItemID == -100)
//            {
//                mapData.CubeDataList[i].ParentItemID = childDic[mapData.CubeDataList[i].ItemID].parent.gameObject.GetInstanceID();
//            }
//        }
//        
//        HBMapConfig mapConfig = Resources.Load<HBMapConfig>("MapConfig/HBMapConfig");
//
//        for (int i = 0; i < mapConfig.MapDataList.Count; i++)
//        {
//            if (mapConfig.MapDataList[i].MapID == int.Parse(editor.MapId))
//            {
//                mapConfig.MapDataList[i] = mapData;
//                EditorUtility.SetDirty(mapConfig);
//                Debug.Log("覆盖场景：" + editor.MapId);
//                return;
//            }
//        }
//
//        mapConfig.MapDataList.Add(mapData);
//        EditorUtility.SetDirty(mapConfig);
//        Debug.Log("储存新场景：" + editor.MapId + "成功");
//
//
//    }
//
//
//    /// <summary>
//    /// 加载地图信息
//    /// </summary>
//    /// <param name="id"></param>
//    public void New_LoadMapConfig(string id)
//    {
//        GameObject cubePool = GameObject.Find("EnvironmentCubePool");
//        HBMapConfig mapConfig = Resources.Load<HBMapConfig>("MapConfig/HBMapConfig");
//        Dictionary<int, GameObject> EnvironmentCubeDic = new Dictionary<int, GameObject>();
//        List<HBMapConfig.CubeData> cubeDatas = new List<HBMapConfig.CubeData>();
//        for (int i = 0; i < mapConfig.MapDataList.Count; i++)
//        {
//            if (mapConfig.MapDataList[i].MapID == int.Parse(editor.MapId))
//            {
//                cubeDatas = mapConfig.MapDataList[i].CubeDataList;
//            }
//        }
//        for (int i = 0; i < cubeDatas.Count; i++)
//        {
//            HBMapConfig.CubeData data = cubeDatas[i];
//            CubeType type = data.Type;
//            GameObject obj = Instantiate(Resources.Load<GameObject>(HBItemData.ItemPath[(int)type]), data.Position, data.Rotation);
//            obj.transform.localScale = data.Scale;
//            HBCubeController cubeCtrl = obj.GetOrAddComponent<HBCubeController>();
//
//            cubeCtrl.ItemID = data.ItemID;
//            cubeCtrl.ParentID = data.ParentItemID;
//            cubeCtrl.RandomRotation = data.RandomRotation;
//            cubeCtrl.Type = data.Type;
//            cubeCtrl._dependLenght = 1.5F;
//            cubeCtrl.ifDepend = false;
//
//            AlEffectBase effect = cubeCtrl.Init(type);
//            if (effect != null)
//            {
//                effect.Init(data.EffectParams);
//            }
//
//            if (data.ParentItemID == -1)
//            {
//                obj.transform.SetParent(CubePool.transform);
//            }
//
//            EnvironmentCubeDic.Add(data.ItemID, obj);
//        }
//
//        //所有方块生成完毕后,根据配置表设置对应的父物体
//        for (int i = 0; i < cubeDatas.Count; i++)
//        {
//            HBMapConfig.CubeData data = cubeDatas[i];
//            if (data.ParentItemID != -1)
//            {
//                GameObject item = EnvironmentCubeDic[data.ItemID];
//                item.transform.SetParent(EnvironmentCubeDic[data.ParentItemID].transform);
//                item.transform.localPosition = data.Position;
//                item.transform.localRotation = data.Rotation;
//                item.transform.localScale = data.Scale;
//            }
//        }
//    }
//    /// <summary>
//    /// 删除地图
//    /// </summary>
//    public void DeleteMap(string id)
//    {
//        List<HBMapConfig.CubeData> cubeDatas = new List<HBMapConfig.CubeData>();
//        bool findID = false;
//        foreach (HBMapConfig.MapData a in editor.New_MapConfig.MapDataList)
//        {
//            // Debug.Log(a.MapID);
//            if (a.MapID == int.Parse(editor.MapId))
//            {
//                editor.New_MapConfig.MapDataList.Remove(a);
//                findID = true;
//                return;
//            }
//        }
//        if (!findID)
//        {
//            Debug.LogError("此ID不存在");
//        }
//    }
}

