using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string flaskKey = "flask";
    private string sleepingBagKey = "sleepingBag";
    private string matchBoxKey = "matchBox";

    [SerializeField] private Backpack backPack;

    [SerializeField] private DraggableItem flaskItem;
    [SerializeField] private DraggableItem sleepingBagItem;
    [SerializeField] private DraggableItem matchBoxItem;

    private void Start()
    {
        if (GetFlaskStatus())
        {
            Transform plaement;
            plaement = backPack.AddItemInstant(flaskItem);
            flaskItem.MoveToBackpackInstant(plaement);
        }

        if (GetSleepingBagStatus())
        {
            Transform plaement;
            plaement = backPack.AddItemInstant(sleepingBagItem);
            sleepingBagItem.MoveToBackpackInstant(plaement);
        }

        if (GetMatchBoxStatus())
        {
            Transform plaement;
            plaement = backPack.AddItemInstant(matchBoxItem);
            matchBoxItem.MoveToBackpackInstant(plaement);
        }
    }

    public void ResetSaveFiles()
    {
        SetFlaskStatus(false);
        SetSleepingBagStatus(false);
        SetMatchBoxStatus(false);
    }

    public bool GetFlaskStatus()
    {
        return GetItemSavedStatus(flaskKey);
    }
    public bool GetSleepingBagStatus()
    {
        return GetItemSavedStatus(sleepingBagKey);
    }
    public bool GetMatchBoxStatus()
    {
        return GetItemSavedStatus(matchBoxKey);
    }

    public bool GetItemSavedStatus(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }


    public void SetFlaskStatus(bool isInside)
    {
        SetItemSavedStatus(flaskKey, isInside);
    }
    public void SetSleepingBagStatus(bool isInside)
    {
        SetItemSavedStatus(sleepingBagKey, isInside);
    }
    public void SetMatchBoxStatus(bool isInside)
    {
        SetItemSavedStatus(matchBoxKey, isInside);
    }

    public void SetItemSavedStatus(string key, bool isInside)
    {
        PlayerPrefs.SetInt(key, isInside ? 1 : 0);
        PlayerPrefs.Save();
    }

}
