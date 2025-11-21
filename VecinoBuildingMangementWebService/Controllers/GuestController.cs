using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        
        public BuildingCatalouge GetBuildingCatalogue(string CityId=null,int page=0)
        {
            
            BuildingCatalouge buildingCatalouge = new BuildingCatalouge();
            try
            {
                this.repositoryUOW.DbHelperOleDb.OpenConnection();
                if (CityId == null && page == 0)
                {
                    buildingCatalouge.Buildings = this.repositoryUOW.BuildingRepository.GetAll();

                }
                else if (CityId != null && page == 0)
                {
                    buildingCatalouge.Buildings = this.repositoryUOW.BuildingRepository.GetByCityId(CityId);
                }
               
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


    }
}
