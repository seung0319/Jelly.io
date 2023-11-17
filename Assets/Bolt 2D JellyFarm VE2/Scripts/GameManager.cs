using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int jelly_point;
    public int gold;

    public TextMeshProUGUI jelly_text;
    public TextMeshProUGUI gold_text;

    public AnimatorController[] acs;
    public int[] Sell_List;
    public Sprite[] Sprite_List;
    public string[] Name_List;
    public int[] JellyPoint_List;

    public TextMeshProUGUI Jelly_ID_text;
    public Image Jelly_Sprite;
    public TextMeshProUGUI Jelly_Name;
    public TextMeshProUGUI Jelly_Price;

    int jelly_idx = 0;

    public bool isSell;

    public Image jelly_panel;
    Animator jelly_Animator;

    private void Awake()
    {
        isSell = false;
        jelly_Animator = jelly_panel.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        // A 지점에서 B 지점 , 거리 비율 T
        jelly_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelly_text.text), jelly_point , 0.5f));
        gold_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(gold_text.text), gold , 0.5f));
        
    }

    
    public void CheckSell()
    {
        isSell = isSell == false;
    }

    public void GetGold(int id, int level)
    {
        gold += Sell_List[id] * level;
    }
    public void ChangeAnimatorController(Animator animator, int level)
    {
        animator.runtimeAnimatorController = acs[level - 1];
    }


    public bool isJellyPanelClicked;
    public void OnClickPanelEnter()
    {
        SetPage();
        if (isJellyPanelClicked)
            jelly_Animator.SetTrigger("Hide");
        else
            jelly_Animator.SetTrigger("Show");
        isJellyPanelClicked = !isJellyPanelClicked;
    }

    public void PageUp()
    {
        if(jelly_idx + 1 < Sprite_List.Length)
        {
            jelly_idx++;
            SetPage();
        }
        else
        {
            
        }
    }

    public void PageDown()
    {
        if (jelly_idx - 1 >= 0)
        {
            jelly_idx--;
            SetPage();
        }
        else
        {

        }
    }

    public void SetPage()
    {
        Jelly_ID_text.text = $"NO. {jelly_idx + 1}";
        Jelly_Sprite.sprite = Sprite_List[jelly_idx];
        Jelly_Name.text = Name_List[jelly_idx];
        Jelly_Price.text = string.Format("{0:N0}", JellyPoint_List[jelly_idx]);
    }
}
