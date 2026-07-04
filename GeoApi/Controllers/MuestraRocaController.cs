using Microsoft.AspNetCore.Mvc;

namespace GeoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MuestraRocaController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _wfsUrl = "http://localhost:8090/geoserver/geoapi/wfs";
        private const string TypeName = "geoapi:muestras_rocas";

        public MuestraRocaController(IHttpClientFactory httpClientFactory)
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
                    $"&featureID=muestras_rocas.{id}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet("provincia/{provincia}")]
        public Task<IActionResult> GetByProvincia(string provincia)
            => QueryWfs($"provincia ILIKE '%{provincia}%'");

        [HttpGet("tipo/{tipo}")]
        public Task<IActionResult> GetByTipo(string tipo)
            => QueryWfs($"tipo ILIKE '%{tipo}%'");
    }
}