using System;

namespace Assets.Scripts.Persistance.Models
{
    [Serializable]
    public class Base
    {
        public Base()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }

        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
