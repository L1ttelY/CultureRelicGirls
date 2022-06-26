using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YueWangSkill : MonoBehaviour
{
    public int skillStates;
    public Sprite[] skillScripts;
    float animaTime = 0;
    bool enterChange1 = false;
    bool enterChange2 = false;
    bool hasChange1 = false;
    bool hasChange2 = false;

    void Start()
    {
   
    }

    private void FixedUpdate()
    {
        if(enterChange1||enterChange2)
        animaTime -= Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        switch (skillStates)
        {
            case 0: GetComponent<SpriteRenderer>().sprite = null;
                    animaTime = 0;
                    enterChange2 = false;
                    enterChange1 = false;
                    hasChange1 = false;
                    hasChange2 = false;
                    break;
            
            case 1:
                if(!enterChange1&&!hasChange1)
                {
                    GetComponent<SpriteRenderer>().sprite = skillScripts[0];
                    animaTime = 0.5f;
                    enterChange1 = true;
                }
                break;
            
            case 2: if (!enterChange2&&!hasChange2)
                {
                    GetComponent<SpriteRenderer>().sprite = skillScripts[0];
                    animaTime = 0.5f;
                    enterChange2 = true;
                }
                break;

        }

        if (animaTime < 0 && enterChange1)
        {
            GetComponent<SpriteRenderer>().sprite = skillScripts[1];
            hasChange1 = true;
            enterChange1 = false;
        }
        
        if (animaTime < 0 && enterChange2)
        {
            GetComponent<SpriteRenderer>().sprite = skillScripts[2];
            hasChange2 = true;
            enterChange2 = false;
        }

    }
}
