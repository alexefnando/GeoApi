using Microsoft.AspNetCore.Mvc;

namespace GeoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapaController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _wmsUrl = "http://localhost:8090/geoserver/geoapi/wms";
        private readonly string _wfsUrl = "http://localhost:8090/geoserver/geoapi/wfs";

        public MapaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("geoserver");
        }

        [HttpGet("wms")]
        public async Task<IActionResult> GetWms()
        {
            var queryString = Request.QueryString.Value;
            var url = $"{_wmsUrl}{queryString}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/png";
            return File(content, contentType);
        }

        [HttpGet("wfs/provincias")]
        public Task<IActionResult> GetProvincias() => GetWfsLayer("geoapi:provincias", 30);

        [HttpGet("wfs/geologia")]
        public Task<IActionResult> GetGeologia() => GetWfsLayer("geoapi:geologia", 100);

        [HttpGet("wfs/muestras")]
        public Task<IActionResult> GetMuestras() => GetWfsLayer("geoapi:muestras_rocas", 1500);

        [HttpGet("wfs/ecuador")]
        public Task<IActionResult> GetEcuador() => GetWfsLayer("geoapi:ecuador", null);

        private async Task<IActionResult> GetWfsLayer(string typeName, int? maxFeatures)
        {
            var url = $"{_wfsUrl}?service=WFS&version=1.0.0&request=GetFeature" +
                      $"&typeName={typeName}&outputFormat=application/json";
            if (maxFeatures.HasValue) url += $"&maxFeatures={maxFeatures.Value}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet("wfs/muestras/cercana")]
        public async Task<IActionResult> GetMuestraCercana(
            [FromQuery] double lat,
            [FromQuery] double lon,
            [FromQuery] double radio = 100)
        {
            var latStr = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var lonStr = lon.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var radioStr = radio.ToString(System.Globalization.CultureInfo.InvariantCulture);

            var cql = $"DWITHIN(geom,POINT({lonStr} {latStr}),{radioStr},meters)";
            var url = $"{_wfsUrl}?service=WFS&version=1.0.0&request=GetFeature" +
                      $"&typeName=geoapi:muestras_rocas&outputFormat=application/json" +
                      $"&CQL_FILTER={Uri.EscapeDataString(cql)}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}