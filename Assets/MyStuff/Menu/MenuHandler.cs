using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public List<GameObject> menuList;
    public void ToggleMainMenu()
    {
        if(menuList.Count > 0) ToggleSpecificMenu(0);
    }
    private void ToggleSpecificMenu(int index)
    {
        DisableAllMenus();
        menuList[index].SetActive(true);
    }
    private void DisableAllMenus()
    {
        foreach (var menu in menuList)
        {
            menu.SetActive(false);
        }
    }
}
