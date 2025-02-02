﻿using Core.Entities.Common;
using Core.Entities.Users;

namespace Core.Entities.Files
{
    public class MyFile: AuditEntity
    {
        public string OriginalName { get; set; }
        public string? Description { get; set; }
        
        public long Size { get; set; }
        public string Extension { get; set; }
        public int Downloads { get; set; }

        public Guid OwnerId { get; set; }
        public AppUser Owner { get; set; }
        public MyFileData? Data { get; set; } 

    }
}
