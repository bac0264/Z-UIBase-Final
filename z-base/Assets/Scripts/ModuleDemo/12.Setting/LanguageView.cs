using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class LanguageView : MonoBehaviour, IEnhancedScrollerDelegate
{
    private LanguageSubView prefab = null;
    
    public EnhancedScroller scroller;
    
    private List<int> _data = new List<int>();
    
    [SerializeField] private Button languageBtn;
    [SerializeField] private Text currentLanguageTxt;

    private void Awake()
    {
        scroller.Delegate = this;
        prefab = LoadResourceController.GetLanguageSubView();
        languageBtn.onClick.AddListener(OnClickPickLanguage);
    }

    public void InitOrUpdateView()
    {
        OnClickLanguage();
        LoadData();
    }

    private void SetCurrentLanguage()
    {
        currentLanguageTxt.text = ((SystemLanguage) DataPlayer.GetModule<PlayerSetting>().GetCurrentLanguage()).ToString();
    }
    private void LoadData()
    {
        int count = Enum.GetNames(typeof(SystemLanguage)).Length;

        for (int i = 0; i < count; i++)
        {
            _data.Add(i);
        }
        scroller.ReloadData();
    }

    private void OnClickPickLanguage()
    {
        scroller.gameObject.SetActive(true);
    }

    public void OnClickLanguage()
    {
        scroller.gameObject.SetActive(false);

        SetCurrentLanguage();
    }
    
    #region enhance
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        // in this example, we just pass the number of our data elements
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        // in this example, even numbered cells are 30 pixels tall, odd numbered cells are 100 pixels tall
        return 65;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        LanguageSubView cellView = scroller.GetCellView(prefab) as LanguageSubView;

        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
        cellView.name = "Cell " + dataIndex.ToString();

        // in this example, we just pass the data to our cell's view which will update its UI
        cellView.SetData(_data[dataIndex], OnClickLanguage);

        // return the cell to the scroller
        return cellView;
    }
    #endregion
}
