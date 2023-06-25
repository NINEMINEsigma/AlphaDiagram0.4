using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using UnityEngine;

namespace AD.Entry
{
    public class EntrySystem : IADSystem
    {
        public IADArchitecture ADinstance()
        {
            return EntryApp.ADinstance;
        }
    }
}