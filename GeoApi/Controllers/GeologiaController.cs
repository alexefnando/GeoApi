using Microsoft.AspNetCore.Mvc;

namespace GeoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeologiaController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _wfsUrl = "http://localhost:8090/geoserver/geoapi/wfs";
        private const string TypeName = "geoapi:geologia";

        public GeologiaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("geoserver");
        }

        private async Task<IActionResult> QueryWfs(string? cqlFilter = null)
        {
            var url = $"{_wfsUrl}?service=WFS&version=1.0.0&request=GetFeature" +
                      $"&typeName={TypeName}&outputFormat=application/json";
            if (!string.IsNullOrEmpty(cqlFilter))
                url += $"&CQL_FILTER={Uri.EscapeDataString(cqlFilter)}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet]
        public Task<IActionResult> GetAll() => QueryWfs();

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var url = $"{_wfsUrl}?service=WFS&version=1.0.0&request=GetFeature" +
                    $"&typeName={TypeName}&outputFormat=application/json" +
                    $"&featureID=geologia.{id}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet("edad/{edad}")]
        public Task<IActionResult> GetByEdad(string edad)
            => QueryWfs($"edad ILIKE '%{edad}%'");
    }
}