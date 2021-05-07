using System;

namespace TaskService.Dto
{
    public class TextTaskDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string SearchWords { get; set; }

        public string Duration { get; set; }
    }
}
