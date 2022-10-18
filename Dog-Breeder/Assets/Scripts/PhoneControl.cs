﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneControl : MonoBehaviour
{
    public GameObject DogListScreen,ShopScreen,MainScreen,PairScreen;
    public GameObject DogListImgs;
    public Text DogCountText,PairListText;
    // Start is called before the first frame update

    public static PhoneControl Instance;
    private void Awake()
    {
        if (!Instance) Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCheckDogListButton()
    {
        DogCountText.text = "You have " + GameManager.Instance.DogList.Count + "/" + GameManager.Instance.DogCapMax + "Dogs. ";
        for( int i =0;i < GameManager.Instance.DogList.Count; i++)
        {
            DogListImgs.transform.GetChild(i).GetComponentInChildren<Text>().text = GameManager.Instance.DogList[i].Name;
            DogListImgs.transform.GetChild(i).GetComponent<ButtonControl>().DogIndex = i;
        }
        

        MainScreen.SetActive(false);
        DogListScreen.SetActive(true);

    }

    public void GoBackMainScreenButton()
    {
        ShopScreen.SetActive(false);
        DogListScreen.SetActive(false);
        PairScreen.SetActive(false);
        MainScreen.SetActive(true);

    }
    
    public void PairScreenButton()
    {


        List<Dropdown.OptionData> ListOptions = new List<Dropdown.OptionData>();
        
        for (int i = 0; i < GameManager.Instance.DogList.Count; i++)
            ListOptions.Add(new Dropdown.OptionData(GameManager.Instance.DogList[i].Name + " " + GameManager.Instance.DogList[i].DogID));

        for (int i = 0; i < 2; i++)
        {
            PairScreen.transform.GetChild(i).GetComponent<Dropdown>().ClearOptions();
            PairScreen.transform.GetChild(i).GetComponent<Dropdown>().AddOptions(ListOptions);
        }


        MainScreen.SetActive(false);
        PairScreen.SetActive(true);
    }

    public void PairDogButton()
    {
        int _firstDropDownValue = PairScreen.transform.GetChild(0).GetComponent<Dropdown>().value;
        int _secondDropDownValue = PairScreen.transform.GetChild(1).GetComponent<Dropdown>().value;

        int _firstDogID = int.Parse(PairScreen.transform.GetChild(0).GetComponent<Dropdown>().options[_firstDropDownValue].text.Split(' ')[1]);
        int _secondDogID = int.Parse(PairScreen.transform.GetChild(1).GetComponent<Dropdown>().options[_secondDropDownValue].text.Split(' ')[1]);

        if (_firstDogID == _secondDogID)
            Debug.Log("Cant let same dog pair");
        else
        {
            Debug.Log("Pair Succeed");
            GameManager.Instance.DogPaired(_firstDogID, _secondDogID);
        }

    }

    public void ShowPairList()
    {
        string _pairDogList = "";
        foreach (var key in GameManager.Instance.DogPairDic.Keys)
        {
            string _pairDogName1 = GameManager.Instance.DogList[GameManager.Instance.FindDogIndex(key)].name;
            string _pairDogName2 = GameManager.Instance.DogList[GameManager.Instance.FindDogIndex(GameManager.Instance.DogPairDic[key])].name;
            string _pairDogs = _pairDogName1 + " pair with " + _pairDogName2 + "\n";
            _pairDogList += _pairDogs;
        }
        PairListText.text = _pairDogList;
    }
     

}