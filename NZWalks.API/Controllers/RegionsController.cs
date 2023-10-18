using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this._context = dbContext;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomainModel = _context.Regions.ToList(); 

            var regionsDTO = new List<RegionDTO>();

            foreach (var regionDomainModel in regionsDomainModel)
            {
                regionsDTO.Add(new RegionDTO() { 
                    Id = regionDomainModel.Id,
                    Name = regionDomainModel.Name,
                    Code = regionDomainModel.Code,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                });
            }

            return Ok(regionsDTO);
        }
        
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            var regionDomainModel = _context.Regions.Find(id);
            
            if(regionDomainModel == null)
                return NotFound();

            var regionDTO = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDTO);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };

            _context.Regions.Add(regionDomainModel);
            _context.SaveChanges();

            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDTO.Id}, regionDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var regionDomainModel =  _context.Regions.FirstOrDefault(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            _context.SaveChanges();

            var regionDTO = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = _context.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            _context.Remove(regionDomainModel);
            _context.SaveChanges();

            return Ok();
        }
    }
}
