﻿using Assets.Scripts.Persistance.Models;
using System;

namespace Assets.Scripts.Persistance.Repositories
{
    [Serializable]
    public class Networks : Repository<Network>
    {
        private void Awake()
        {
            var context = new JsonDataContext() { FileName = "networks"};
            Context = context;
        }
    }
}
