using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text description;
    public TMP_Text alert;
    public TMP_Dropdown houseDropdown;
    public TMP_Dropdown characterDropdown;
    public CharacterDataBaseScriptableObject characterDataBase;
    // Start is called before the first frame update
    void Start()
    {
        houseDropdown.ClearOptions();
        houseDropdown.AddOptions(characterDataBase.houseList);
        characterDropdown.ClearOptions();
        List<string> characterOptions = new List<string>();
        characterOptions.Add("All");
        characterOptions.AddRange(characterDataBase.characterNameList);
        characterDropdown.AddOptions(characterOptions);
        //Defaul selecte stark
        ResetAllSelection();
        AddSelectedHouse();
    }

    // Update is called once per frame
    void Update()
    {
        ShowDescription();
    }

    public void AddSelectedHouse()
    {
        string slectedHouse = houseDropdown.options[houseDropdown.value].text;
        foreach (CharacterInfo info in characterDataBase.characterInfoList)
        {
            if (info.houseName == slectedHouse)
            {
                info.show = true;
            }
        }
    }
    public void RemoveSelectedHouse()
    {
        string slectedHouse = houseDropdown.options[houseDropdown.value].text;
        foreach (CharacterInfo info in characterDataBase.characterInfoList)
        {
            if (info.houseName == slectedHouse)
            {
                info.show = false;
            }
        }
    }
    public void AddSelectedChar()
    {
        string slectedChar = characterDropdown.options[characterDropdown.value].text;
        foreach (CharacterInfo info in characterDataBase.characterInfoList)
        {
            if (info.characterName == slectedChar)
            {
                if (info.show)
                {
                    info.selected = true;
                    GameObject.Find(slectedChar).GetComponent<CharacterBase>().SetLineColor();
                }
                else
                {
                    info.show = true;
                }
            }
        }
    }
    public void RemoveSelectedChar()
    {
        string slectedChar = characterDropdown.options[characterDropdown.value].text;
        foreach (CharacterInfo info in characterDataBase.characterInfoList)
        {
            if (info.characterName == slectedChar)
            {
                info.show = false;
                info.selected = false;
                GameObject.Find(slectedChar).GetComponent<CharacterBase>().SetLineColor();
            }
        }
    }

    public void ResetAllSelection()
    {
        foreach (CharacterInfo info in characterDataBase.characterInfoList)
        {
            info.show = false;
            info.selected = false;
        }
    }

    public void ShowDescription()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform != null)
                {
                    if (hit.collider.gameObject.transform.parent != null)
                    {
                        if (hit.collider.gameObject.transform.parent.GetComponent<CharacterBase>())
                        {
                            if (hit.collider.gameObject.transform.parent.GetComponent<CharacterBase>().info.show)
                            {
                                description.text = hit.collider.gameObject.transform.parent.name;
                            }
                        }
                        else
                        {
                            description.text = hit.collider.gameObject.transform.parent.name;
                        }
                    }
                    else if (hit.collider.gameObject.GetComponent<LineRenderer>())
                    {
                        string tmp = hit.collider.gameObject.name;
                        string name = tmp.Substring(0, tmp.Length - 5);
                        int characterIndex = characterDataBase.characterNameList.IndexOf(name);
                        if (characterDataBase.characterInfoList[characterIndex].show)
                        {
                            description.text = name;
                            characterDataBase.characterInfoList[characterIndex].selected = !characterDataBase.characterInfoList[characterIndex].selected;
                            GameObject.Find(name).GetComponent<CharacterBase>().SetLineColor();
                        }

                    }

                }
            }
        }
    }
}
