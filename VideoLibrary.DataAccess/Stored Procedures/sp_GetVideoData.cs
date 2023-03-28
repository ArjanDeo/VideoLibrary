using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VideoLibrary.DataAccess.Stored_Procedures
{
    public class sp_GetVideoData
    {

        [Key]
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Actors { get; set; }
    }
}
