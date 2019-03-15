using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 射线管理组件
/// </summary>
public class RayManager: MonoBehaviour {
    Ray ray;
    RaycastHit hit = new RaycastHit();

    private Transform  HitTransform;
    private bool isHit;
    private bool isHit_UI;
    private ScriptsManager RM_SM;
    private ShowAndMove RM_SAM;
    private RoleInfor RM_RI;

   public  void L_Start()
    {
        RM_SM = GameObject.Find("Main Camera").GetComponent<ScriptsManager>();
        RM_RI = RM_SM.RI;
      //  RM_SAM = GameObject.Find("Show").GetComponent<ShowAndMove>();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
        
            isHit_UI = true;
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject!=null)
            {
           //     Debug.Log("Point name is "+ EventSystem.current.name);
            }
            else
            {
                isHit_UI = false;
            }
            if (isHit_UI == false)
            {
              //  RM_SAM.CloseThis();
            }
        }
    }
    public bool IfHit()
    {
        return isHit;
    }
    public RaycastHit GiveRayHit()
    {
        return hit;
    }
}
