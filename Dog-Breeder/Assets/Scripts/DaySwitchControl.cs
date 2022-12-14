using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaySwitchControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform startPoint;

    public GameObject TransportScene;
    public GameObject LostScene;
    public GameObject MoneyCal, DebuffCal;
    public GameObject AimPoint;
    public Text DayCountText;

    public Text DayText,SavingText,CostText,FinalValueText,DebText;

    public string TransCostText = "";
    public string DebuffText = "";

    public static DaySwitchControl Instance;
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

    public void GoNextDayButton()
    {
        AimPoint.SetActive(false);
        CameraMovement.Instance._moveMode = false;
        GameManager.Instance.StatusUI.SetActive(GameManager.Instance.IsStatusActive = false);
        GameManager.Instance.PhoneUI.SetActive(GameManager.Instance.IsPhoneActive = false);

       

        TransCostText = "";
        DebuffText = "";
        TransportScene.SetActive(true);
        GameManager.Instance.DayCount += 1;

        DayText.text = "DAY " + GameManager.Instance.DayCount;
        SavingText.text = "Savings                     " + GameManager.Instance.YesterdayMoney.ToString("c2") + "\n" ;
        SavingText.text += "Sales                        " + (GameManager.Instance.Money - GameManager.Instance.YesterdayMoney + GameManager.Instance.TodayUpdateCost).ToString("c2") + "\n";
        if(GameManager.Instance.TodayUpdateCost != 0)
            SavingText.text += "UpdateFee                    " +  GameManager.Instance.TodayUpdateCost.ToString("c2");
        CostCalculate();
        FinalValueText.text = "-----------------------------------------------\n                                       " + GameManager.Instance.Money.ToString("c2");


        for (int i = 0;i< GameManager.Instance.DogList.Count; i++)
            GameManager.Instance.DogList[i].NewDay();

        DemandController.Instance.ShuffleDemands();

        GameManager.Instance.NewDay();
        DebText.text = DebuffText;

        Time.timeScale = 0;
    }

    void CostCalculate()
    {
        int dogCost = GameManager.Instance.DogList.Count * GameManager.Instance.CostPerDog;    
        GameManager.Instance.Money -= dogCost;
        GameManager.Instance.newDogCount = 0;
        TransCostText += "Dog Cost                   -" + dogCost.ToString("c2") + "\n";
        GameManager.Instance.Money -= GameManager.Instance.CostPerDay;
        TransCostText += "Daily Cost                 -" + GameManager.Instance.CostPerDay.ToString("c2") + "\n";
        if (GameManager.Instance.DayCount % 6 == 0)
        {
            GameManager.Instance.WeekCount += 1;
            GameManager.Instance.Money -= GameManager.Instance.CostPerWeak;
            TransCostText += "Rent                       -" + GameManager.Instance.CostPerWeak.ToString("c2") + "\n";

            foreach(var dog in GameManager.Instance.DogList)
            {
                dog.HP -= 1;
            }
           
        }

        if (GameManager.Instance.WeekCount % 5 == 0)
        {
            GameManager.Instance.MonthCount += 1;
            GameManager.Instance.Money -= GameManager.Instance.CostPerMonth;
            TransCostText += "Rent/Month:           -" + GameManager.Instance.CostPerMonth.ToString("c2") + "\n";
        }

        CostText.text = TransCostText;
    }
    public void GoContinueButton()
    {

        GameManager.Instance.TodayUpdateCost = 0;

        if (MoneyCal.activeSelf)
        {
            MoneyCal.SetActive(false);
            DebuffCal.SetActive(true);
        }
        else
        {
            if (GameManager.Instance.Money < 0)
                LostSceneActicve();
            else
            {
                Time.timeScale = 1;
                TransportScene.SetActive(false);
                GameManager.Instance.YesterdayMoney = GameManager.Instance.Money;
                int _dayAll = GameManager.Instance.DayCount;

                int _weekDay = _dayAll % 5;
                switch (_weekDay)
                {
                    case 1:
                        DayCountText.text = "MONDAY";
                        break;

                    case 2:
                        DayCountText.text = "TUESDAY";
                        break;

                    case 3:
                        DayCountText.text = "WEDNESDAY";
                        break;

                    case 4:
                        DayCountText.text = "THURSDAY";
                        break;

                    case 0:
                        DayCountText.text = "FRIDAY";
                        break;

                }
                DayCountText.text += "\n<size=56>DAY " + _dayAll + "</size>";
                MoneyCal.SetActive(true);
                DebuffCal.SetActive(false);
                PhoneControl.Instance.PairScreenButton();

                Camera.main.transform.position = startPoint.position;
                Camera.main.transform.rotation = startPoint.rotation;
                AimPoint.SetActive(true);
                CameraMovement.Instance._moveMode = true;
                ReminderPopUp(GameManager.Instance.DayCount);
            }

        }



    }

    void ReminderPopUp(int Day)
    {
        if (GameManager.Instance.ReminderPopDaysList.Contains(Day))
            GameManager.Instance.OpenRentReminderUI();
    }
    public void LostSceneActicve()
    {
        MoneyCal.SetActive(false);
        DebuffCal.SetActive(false);
        LostScene.SetActive(true);
    }
}