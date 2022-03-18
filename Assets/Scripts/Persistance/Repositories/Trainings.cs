using Assets.Scripts.Persistance.Models;
using System;

namespace Assets.Scripts.Persistance.Repositories
{
    [Serializable]
    public class Trainings : Repository<Training>
    {
        private void Awake()
        {
            var context = new JsonDataContext() { FileName = "trainings" };
            Context = context;
        }
    }
}
