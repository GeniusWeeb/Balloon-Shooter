
using System;
using BallLine;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelChooseUIManager : MonoBehaviour
{

    [SerializeField] private GameObject levelSelectPrefab;
    [SerializeField] private Transform levelPanelContent; 

    private void OnEnable()
    {
        Init();
    }
    
    private void Init()
    {

        foreach (var item in LevelManager.Instance.Levels)
        {
          GameObject levelPrefab =  Instantiate(levelSelectPrefab, transform.position, quaternion.identity, levelPanelContent);
          var levelName = levelPrefab.GetComponentInChildren<TextMeshProUGUI>().text =   item.GetComponent<Level>().levelName;
          levelPrefab.GetComponent<Button>().interactable = item.GetComponent<Level>().IsUnlocked;
          
          levelPrefab.GetComponent<Button>().onClick.AddListener(() =>
          {
              
           UIManager.Instance.PlaySelectedLevel(levelName);
              
          });
        }
    }

   

    public void CloseThis()
    {
        this.gameObject.SetActive(false);
        UIManager.Instance.LevelChooseBtn.SetActive(true);
    }

    void PerformCleanUp()
    {
        for (int i = 0; i < levelPanelContent.childCount; i++)
        {
            Destroy(levelPanelContent.GetChild(i).gameObject);
        }
    }

    private void OnDisable()
    {
        PerformCleanUp();
    }
}
