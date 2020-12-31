using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIModuleDailyQuest : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;

    private DailyQuestData[] _data;
    private DailyQuestView dailyQuestPrefab;

    private DailyQuestCollection dailyQuestCollection = null;
    private PlayerDailyQuest playerQuest;

    private void Awake()
    {
        PublisherService.Register();
        dailyQuestPrefab = LoadResourceController.GetDailyQuestView();
        playerQuest = DataPlayer.GetModule<PlayerDailyQuest>();
        dailyQuestCollection = LoadResourceController.GetDailyQuestCollection();
    }

    private void Start()
    {
        UpdateView();
        PublisherService.DebugLog();
    }

    public void UpdateView()
    {
        LoadData();
    }

    private void LoadData()
    {
        _data = dailyQuestCollection.dataGroups;
        scroller.Delegate = this;
        ReloadData();
    }

    private void ReloadData(int index = 0)
    {
        scroller.ReloadData();
        scroller.JumpToDataIndex(index);
    }
    
    public InputField progress;
    public InputField subjectType;

    public void AddProgress()
    {
        PublisherService.NotifyListener(new BaseListenerData((SubjectType) int.Parse(subjectType.text),
            int.Parse(progress.text)));
         SceneManager.LoadScene("6.DailyQuest");
    }

    #region setup enhance

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        // in this example, we just pass the number of our data elements
        return _data.Length;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        // in this example, even numbered cells are 30 pixels tall, odd numbered cells are 100 pixels tall
        return 300;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        var cellView = scroller.GetCellView(dailyQuestPrefab) as DailyQuestView;

        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
        cellView.name = "Cell " + dataIndex.ToString();

        // in this example, we just pass the data to our cell's view which will update its UI
        cellView.SetData(_data[dataIndex], ReloadData, dataIndex);

        // return the cell to the scroller
        return cellView;
    }

    #endregion
}