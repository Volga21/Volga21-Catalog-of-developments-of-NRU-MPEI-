using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropHandler : MonoBehaviour
{
    public Spawner spawner;

    List<string> m_DropOptions = new List<string>();

    //This is the Dropdown
    TMP_Dropdown m_Dropdown;

    private void Start()
    {
        //Fetch the Dropdown GameObject the script is attached to
        m_Dropdown = GetComponent<TMP_Dropdown>();
        //Clear the old options of the Dropdown menu
        m_Dropdown.ClearOptions();
        //Add the options created in the List above
        //foreach (DeviceModel item in spawner.ObjectModelsList[0])
        //{
        //    m_DropOptions.Add(item.modelName);
        //}
        m_Dropdown.AddOptions(m_DropOptions);
    }

    //public void CallSpawn(int val)
    //{
    //    spawner.SpawnObject(0,val);
    //}
}
