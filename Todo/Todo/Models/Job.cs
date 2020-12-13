using System;

namespace Todo.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public bool IsEnded { get; set; }
    }
}
