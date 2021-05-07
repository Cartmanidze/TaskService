using System;
using System.Collections.Generic;
using GenericRepository.EFCore.Models;

namespace TaskService.Models
{
    public class TextTask : BaseModel
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string SearchWords { get; set; }

        public ICollection<TextTaskResult> TextTaskResults { get; } = new HashSet<TextTaskResult>();
    }
}