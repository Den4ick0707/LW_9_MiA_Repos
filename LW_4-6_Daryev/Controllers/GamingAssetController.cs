using LW_4_3_5_Daryev_PI231.Services;
using LW_4_3_5_Daryev_PI231.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LW_4_3_5_Daryev_PI231.Controllers
{
    /// <summary>
    ///     Gamming Assets API Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GamingAssetsController : ControllerBase
    {
        private readonly IGamingAssetService _service;
        public GamingAssetsController(IGamingAssetService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all assets asynchronously.
        /// </summary>
        /// <returns>This method returns an HTTP 200 OK response containing a list of all assets..</returns>

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _service.GetAllAssetsAsync();
            return Ok(assets);
        }

        /// <summary>
        ///     Retrieves an asset by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This method returns an HTTP 200 OK response contains element by ID else return HTTPS 404 </returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var asset = await _service.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            return Ok(asset);
        }
        /// <summary>
        ///     Creates a new asset asynchronously. 
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns>This method return HTTP 201 CREATED</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssetDTO createDto)
        {
            var newAsset = await _service.CreateAssetAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newAsset.Id }, newAsset);
        }
        /// <summary>
        ///  Update asset by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateDto"></param>
        /// <returns>This method return HTTP 204 if asset updated else return 404</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateAssetDTO updateDto)
        {
            var success = await _service.UpdateAssetAsync(id, updateDto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        /// <summary>
        ///     Delete asset by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This method return HTTP 204 if asset deleted else return 404</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _service.DeleteAssetAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}