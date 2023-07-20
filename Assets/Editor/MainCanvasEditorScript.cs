using System;
using System.Collections;
using System.Collections.Generic;
using BallLine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


//Custom Editor script to control the Ui Manager

[CustomEditor(typeof(UIManager))]
public class MainCanvasEditorScript : Editor
{
  

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        UIManager uiManager = (UIManager) target; //1
        
        
        if(GUILayout.Button("End game"))
            uiManager.EndGame();
        
      
            
    }
}
