using Microsoft.AspNetCore.Mvc;
using CodingChallenge.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodingChallengeApi.Controllers;

[ApiController]
[Route("api/[action]")]
public class NotificationApiController : ControllerBase
{
    private readonly ILogger<NotificationApiController> _logger;

    public NotificationApiController(ILogger<NotificationApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Supervisors()
    {
        var supervisors = new List<String>();
        try
        {
            supervisors = PopulateSupervisors().Result;
        }
        catch (HttpRequestException)
        {
            // Error on amazonaws call
            return BadRequest("Failed to fetch supervisors");
        }
        return Ok(supervisors);

    }

    [HttpPost]
    public IActionResult Submit([FromForm] NotificationApiModel notificationApiModel)
    {
        if (ModelState.IsValid)
        {
            Console.Write(notificationApiModel.ToString());
            return Ok("Request successful.");
        }
        return BadRequest("Failed to submit.");
    }

    private async Task<List<String>> PopulateSupervisors()
    {
        var returnList = new List<String>();

        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://o3m5qixdng.execute-api.us-east-1.amazonaws.com/api/managers");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var supervisors = JsonSerializer.Deserialize<List<SupervisorJson>>(responseBody, new JsonSerializerOptions{AllowTrailingCommas = true}) ?? new List<SupervisorJson>();

            //Removing numeric jurisdictions.
            supervisors.RemoveAll(o => int.TryParse(o.jurisdiction, out int tempInt)); 

            //Sorting alphabetically by jurisdiction, last name, then first name.
            supervisors = supervisors.OrderBy(o => o.jurisdiction).ThenBy(o => o.lastName).ThenBy(o => o.firstName).ToList();

            //FormatException with "jurisdiction - lastName, FirstName"
            supervisors.ForEach(o => 
            {
                var supervisorString = String.Format("{0} - {1}, {2}", o.jurisdiction, o.lastName, o.firstName);
                returnList.Add(supervisorString);
            });

            return returnList;
        }
    }
}

