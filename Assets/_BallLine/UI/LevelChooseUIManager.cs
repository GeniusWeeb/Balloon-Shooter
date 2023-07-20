
using System;
using BallLine;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
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
          levelPrefab.GetComponentInChildren<TextMeshProUGUI>().text =   item.GetComponent<Level>().levelName;
          levelPrefab.GetComponent<Button>().interactable = item.GetComponent<Level>().IsUnlocked;
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
