﻿namespace EXE201.Controllers.DTO.Review
{
    public class ReviewDTOCreate
    {
        //public long Id { get; set; }
        public long AccountId { get; set; }
        public long PackageId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
