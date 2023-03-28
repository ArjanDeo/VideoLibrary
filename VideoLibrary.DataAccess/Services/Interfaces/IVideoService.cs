using System.Collections.Generic;
using System.Threading.Tasks;
using VideoLibrary.Models.WebApp.Video;

namespace VideoLibrary.DataAccess.Services.Interfaces
{
    public interface IVideoService
    {
        Task<List<sp_GetVideoDataModel>> GetVideoDataAsync();
    }
}
