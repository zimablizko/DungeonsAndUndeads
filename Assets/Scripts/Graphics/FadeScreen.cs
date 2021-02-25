﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public Image fade;
    void Start()
    {
        fade.gameObject.SetActive(true);
        //FadeOut();
    }
    public void FadeOut()
    {
        fade.CrossFadeAlpha(0,1.5f,false);
    }
    
    public void FadeIn()
    {
        fade.CrossFadeAlpha(1f,1.5f,false);
    }
    
    
}
