using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VideoLibrary.DataAccess.Stored_Procedures;
using VideoLibrary.DataAccess.Tables;

namespace VideoLibrary.DataAccess
{
    public class VideoLibraryDatabase : DbContext
    {
        public VideoLibraryDatabase(DbContextOptions<VideoLibraryDatabase> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<sp_Orders_GetNewOrderId>(entity => entity.HasNoKey());
        }

        #region Tables
        public DbSet<tbl_VideoData> tbl_VideoData { get; set; }
        #endregion

        #region Views
        //public DbSet<vw_DL_UOMWeight> vw_DL_UOMWeight { get; set; }
        #endregion

        #region Stored Procedures
        public DbSet<sp_GetVideoData> sp_GetVideoData { get; set; }
        #endregion
    }
}
