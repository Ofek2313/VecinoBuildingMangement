using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public BuildingCatalouge GetBuildingCatalogue(string cityId=null,int page=0) // Reminder To Fix - Also Fix Website
        {
            
            BuildingCatalouge buildingCatalouge = new BuildingCatalouge();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                if (cityId == null && page == 0)
                {
                    buildingCatalouge.Buildings = this.repositoryUOW.BuildingRepository.GetAll();

                }
                else if (cityId != null && page == 0)
                {
                    buildingCatalouge.Buildings = this.repositoryUOW.BuildingRepository.GetByCityId(cityId);
                }
                else if(cityId == null && page != 0)
                {
                    buildingCatalouge.Buildings = this.repositoryUOW.BuildingRepository.GetBuildingByPage(page);
                }
                else if(cityId != null && page != 0)
                {
                    int buildingPerPage = 5;
                    buildingCatalouge.Buildings = this.repositoryUOW.BuildingRepository.GetByCityId(cityId);
                    buildingCatalouge.Buildings.Skip(buildingPerPage * (page - 1)).Take(buildingPerPage).ToList();

                }
                int buildingCount = this.repositoryUOW.BuildingRepository.GetBuildingCount();
                buildingCatalouge.PageCount = buildingCount / 5;
                if (buildingCount % 5 > 0)
                    buildingCatalouge.PageCount++;
                return buildingCatalouge;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
           
        }

        [HttpPost]
        public bool Register([FromBody] Resident resident)
        {
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                return this.repositoryUOW.ResidentRepository.Create(resident);

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                this.repositoryUOW.DbHelperOleDb.CloseConnection();
            }
            
        }

    }
}
