using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor ;


[CustomEditor(typeof(PerLevelDataManager))]
public class PerLevelDataManagerScript : Editor
{



    public override void OnInspectorGUI()
    {   
        var manager = (PerLevelDataManager)target ;
        base.OnInspectorGUI();
        
        GUILayout.Label("Button Below add these values for all levels ");
        GUILayout.Label("Game Speed => 2");
        GUILayout.Label("Score to win => 10");

        if(GUILayout.Button("Fill Default values"))
                {
                    foreach(var item in manager.level)
                    {
                        item.gameSpeed = 2 ;
                        item.scoreToWin = 10;
                    }
                }


        GUILayout.Label("You can edit this scrupt from the Asset/Editor/PerLevelDataManagerScript");
    }
}
