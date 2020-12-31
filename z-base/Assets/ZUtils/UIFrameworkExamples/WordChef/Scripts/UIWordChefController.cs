using System;
using UnityEngine;
using Zitga.ContextSystem;
using Zitga.Localization;
using Zitga.Sound;
using Zitga.Update;

namespace deVoid.UIFramework.Examples
{
    public class UIWordChefController : MonoBehaviour
    {
        private Context context;

        private GlobalUpdateSystem updateSystem;
        
        private void Awake()
        {
            context = Context.Current;
            
            InitUpdateSystem();
            
            InitLocalization();
            
            InitSoundManager();
            
            InitUIFrame();
        }

        private void Start() {
            UIFrame.Instance.OpenWindow(ScreenIds.HomeWindow);
        }

        private void InitUIFrame()
        {
            var result = Resources.Load<UIFrame>("UIFrame");
            
            if (result)
            { 
                var uiFrame = Instantiate(result);

                context.GetContainer().Register(uiFrame);
            }
            else
            {
                throw new Exception("UIFrame is not exist");
            }
        }

        private void InitUpdateSystem()
        {
            updateSystem = new GlobalUpdateSystem();
            
            context.GetContainer().Register(updateSystem);
        }

        private void InitLocalization()
        {
            var localization = Localization.Current;
            
            localization.localCultureInfo = Locale.GetCultureInfoByLanguage(SystemLanguage.English);
            
            context.GetContainer().Register(localization);
        }
        
        private void InitSoundManager()
        {
            var soundManager = new SoundManager();
            
            context.GetContainer().Register(soundManager);
        }

        private void Update()
        {
            updateSystem.OnUpdate(Time.deltaTime);
        }
    }
}