using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoLibrary.DataAccess.Services.Interfaces;
using VideoLibrary.DataAccess.Stored_Procedures;
using VideoLibrary.Models.WebApp.Video;

namespace VideoLibrary.DataAccess.Services
{
    public class VideoService : IVideoService
    {
        #region Constructor
        private readonly VideoLibraryDatabase _videoLibraryDatabase;
        private readonly IConfiguration _configuration;

        public VideoService(VideoLibraryDatabase videoLibraryDatabase, IConfiguration configuration)
        {
            _videoLibraryDatabase = videoLibraryDatabase;
            _configuration = configuration;
        }
        #endregion

        #region Get Video Data 
        public async Task<List<sp_GetVideoDataModel>> GetVideoDataAsync()
        {
            var dataList = await _videoLibraryDatabase.sp_GetVideoData
                                       .FromSqlInterpolated($"sp_GetVideoData")
                                       .ToListAsync();

            List<sp_GetVideoDataModel> Model = new List<sp_GetVideoDataModel>();
            foreach (sp_GetVideoData data in dataList)
            {
                sp_GetVideoDataModel newDataList = new sp_GetVideoDataModel()
                {
                    VideoId = data.VideoId,
                    Title = data.Title,
                    Url = data.Url,
                    Source = data.Source,
                    Actors = data.Actors
                };

                Model.Add(newDataList);
            }

            return Model;
        }
        #endregion
    }
}
