using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using VideoLibrary.DataAccess;
using VideoLibrary.DataAccess.Services.Interfaces;
using VideoLibrary.DataAccess.Tables;
using VideoLibrary.IdentityServer.Database.Tables;
using VideoLibrary.Interfaces;

namespace VideoLibrary.WebApp.Controllers
{
    public class VideoController : Controller
    {
        private readonly UserManager<FedExCCUser> _userManager;
        private readonly RoleManager<FedExCCRole> _roleManager;
        private readonly IVideoService _videoService;
        private readonly VideoLibraryDatabase _videoLibraryDatabase;

        public VideoController(UserManager<FedExCCUser> userManager, RoleManager<FedExCCRole> roleManager, IVideoService videoService, VideoLibraryDatabase videoLibraryDatabase)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _videoService = videoService;
            _videoLibraryDatabase = videoLibraryDatabase;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region Manage Video Data

        #region Get Video Data
        public async Task<object> GetVideoData(DataSourceLoadOptions loadOptions)
        {
            //var userId = _userManager.GetUserId(User);
            var data = await _videoService.GetVideoDataAsync();
            return DataSourceLoader.Load(data, loadOptions);
        }
        #endregion

        #region Insert Video Data
        [HttpPost]
        public async Task<IActionResult> InsertVideoData(string values)
        {
            tbl_VideoData data = new tbl_VideoData();
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(ModelState.GetFullErrorMessage());

            //tbl_VideoData existing = await _videoLibraryDatabase.tbl_VideoData.Where(x => x.stationName.ToLower() == data.stationName.ToLower()).FirstOrDefaultAsync();
            //if (existing != null)
            //{
            //    ModelState.AddModelError("error", "Station Exists");
            //    return BadRequest(ModelState.GetFullErrorMessage());
            //}

            //var userId = _userManager.GetUserId(User);
            //data.createdBy = Guid.Parse(userId);
            //data.createdDT = DateTime.UtcNow;

            _videoLibraryDatabase.tbl_VideoData.Add(data);
            await _videoLibraryDatabase.SaveChangesAsync();


            return Ok();
        }
        #endregion

        #region Update Video Data
        [HttpPut]
        public async Task<IActionResult> UpdateVideoData(int key, string values)
        {
            var data = _videoLibraryDatabase.tbl_VideoData.First(f => f.VideoId == key);
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(ModelState.GetFullErrorMessage());

            //tbl_VideoData currentData = _videoLibraryDatabase.tbl_VideoData.Where(x => x.stationName.ToLower() == data.stationName.ToLower()).FirstOrDefault();
            //if (currentData != null && currentData.stationName != data.stationName)
            //{
            //    ModelState.AddModelError("error", "Station Exists");
            //    return BadRequest(ModelState.GetFullErrorMessage());
            //}

            //var userId = _userManager.GetUserId(User);
            //data.updatedBy = Guid.Parse(userId);
            //data.updatedDT = DateTime.UtcNow;

            await _videoLibraryDatabase.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Delete Video Data
        [HttpDelete]
        public void DeleteVideoData(int key)
        {
            var data = _videoLibraryDatabase.tbl_VideoData.First(o => o.VideoId == key);
            _videoLibraryDatabase.tbl_VideoData.Remove(data);
            _videoLibraryDatabase.SaveChanges();
        }
        #endregion

        #endregion

    }
}
