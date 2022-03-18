using Assets.Scripts.Persistance.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Persistance.Repositories
{
    public class Repository<T> : MonoBehaviour where T : Base
    {
        [HideInInspector] public DataContext Context;

        private List<T> Entities => Context.Set<T>();

        public List<T> GetAll()
        {
            Context.Load();
            return Entities;
        }

        public T GetById(string id)
        {
            return Entities.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(T entity)
        {
            Entities.Remove(entity);
        }

        public void Add(T entity)
        {
            Entities.Add(entity);
        }

        public void Save()
        {
            Context.Save();
        }
    }
}