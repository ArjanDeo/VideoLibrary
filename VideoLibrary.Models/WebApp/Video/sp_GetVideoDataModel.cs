using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.Models.WebApp.Video
{
    public class sp_GetVideoDataModel
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Actors { get; set; }
    }
}
