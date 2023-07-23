using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AD.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AD.BASE;
using AD.Utility;

namespace AD.Entry
{
    //Main Controller
    public class EntryManager : AD.SceneBaseController
    {
        private void Start()
        {
            EntryApp.instance.RegisterController(this);
        }

        ~EntryManager()
        {
                
        }
    }
}