using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrossInputController : MonoBehaviour
{
    public GameObject document;
    public const float Step = 0.1f;
    
    private bool isUpHold;
    private bool isRightHold;
    private bool isLeftHold;
    private float horizontalVal;
    
    private bool isMobile;


    public float GetHorizontal()
    {
        return horizontalVal;
    }

    public bool GetJump()
    {
        return isUpHold;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        isMobile = Input.touchSupported;
        // isMobile = true;
        isUpHold = false;
        horizontalVal = 0f;
        if (isMobile) document.SetActive(true);
        else return;

        var root = document.GetComponent<UIDocument>().rootVisualElement;
        var bLeft = root.Q<Button>("Left");
        var bRight = root.Q<Button>("Right");
        var bUp = root.Q<Button>("Up");
        bLeft.clickable = null;
        bRight.clickable = null;
        bUp.clickable = null;
        
        bLeft.RegisterCallback<PointerDownEvent>(evt =>
        {
            isLeftHold = true;
        });
        bRight.RegisterCallback<PointerDownEvent>(evt =>
        {
            isRightHold = true;
        });
        bUp.RegisterCallback<PointerDownEvent>(evt =>
        {
            isUpHold = true;
        });
        bUp.RegisterCallback<PointerUpEvent>(evt =>
        {
            isUpHold = false;
        });

        bLeft.RegisterCallback<PointerUpEvent>(evt =>
        {
            isLeftHold = false;
        });
        bRight.RegisterCallback<PointerUpEvent>(evt =>
        {
            isRightHold = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMobile)
        {
            horizontalVal = Input.GetAxisRaw("Horizontal");
            isUpHold = Input.GetKey(KeyCode.Space);
        }
        else
        {
            if (isLeftHold)
                horizontalVal = Mathf.Clamp(horizontalVal - Step, -1, 1);
            else if (isRightHold)
                horizontalVal = Mathf.Clamp(horizontalVal + Step, -1, 1);
            else
                horizontalVal = 0;
        }
    }
}
