using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VecinoBuildingMangement.Models;
using VecinoBuildingMangement.ViewModels;

namespace VecinoBuildingMangementWebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        RepositoryUOW repositoryUOW;

        public GuestController()
        {
            this.repositoryUOW = new RepositoryUOW();
        }

        [HttpGet]
        public BuildingCatalouge GetBuildingCatalogue(int page=1) // Reminder To Fix - Also Fix Website
        {
            
            BuildingCatalouge buildingCatalouge = new BuildingCatalouge();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                buildingCatalouge.NumberOfBuildings = this.repositoryUOW.BuildingRepository.GetBuildingStats();
                buildingCatalouge.Buildings = this.repositoryUOW.BuildingRepository.GetBuildingsPerPage(page, 10);
               
               
                buildingCatalouge.PageCount = buildingCatalouge.NumberOfBuildings / 10;
                if (buildingCatalouge.NumberOfBuildings % 14 > 0)
                    buildingCatalouge.PageCount+=1;
                buildingCatalouge.Page = page;
                return buildingCatalouge;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally 
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
           
        }

        [HttpPost]
        public Resident Register([FromBody] Resident resident)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                if (this.repositoryUOW.ResidentRepository.Create(resident))
                {
                    string residentId =  this.repositoryUOW.ResidentRepository.GetLastId();
                    resident.ResidentId = residentId;
                    return resident;

                }
                else
                {
                  
                    return null;
                }
                    

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
            
        }

        [HttpGet]
        public IActionResult GetBuildingPhoto(string buildingId)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                string photo = this.repositoryUOW.BuildingRepository.GetBuildingPhotoById(buildingId);
                string extension = Path.GetExtension(photo).TrimStart('.');
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images","BuildingImages", photo);


                FileStream stream = System.IO.File.OpenRead(path);

                return File(stream, $"image/{extension}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Image Failed To Load");
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }
        [HttpGet]
        public List<Building> GetBuildingsMap()
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.BuildingRepository.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
        }

    }
}
