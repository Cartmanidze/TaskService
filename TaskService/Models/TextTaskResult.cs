using System;
using GenericRepository.EFCore.Models;

namespace TaskService.Models
{
    public class TextTaskResult : BaseModel
    {

        public string FoundedWords { get; set; }

        public Guid TextId { get; set; }

        public TextTask TextTask { get; set; }

        public Guid TextTaskOid { get; set; }
    }
}
