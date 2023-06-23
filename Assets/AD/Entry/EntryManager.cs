using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AD.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AD.ADbase;
using AD.Utility;

namespace AD.Entry
{
    //Main Controller
    public class EntryManager : AD.SceneBaseController
    {
        private void Start()
        {
            EntryApp.ADinstance.RegisterController(this);
        }

        ~EntryManager()
        {
                
        }


        public override IADArchitecture ADinstance()
        {
            return EntryApp.ADinstance;
        } 
    }
}