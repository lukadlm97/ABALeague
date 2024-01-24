
/*
IConfiguration configuration = Configuration.Default.WithDefaultLoader();
IBrowsingContext context=BrowsingContext.New(configuration);
IDocument document = await context.OpenAsync(@"https://www.aba-liga.com/calendar/22/1/");


var rounds = document.QuerySelectorAll("div.panel.panel-default");
//var rounds = document.All.Where(m=>m.LocalName=="div" && m.ClassList.Equals("panel panel-default"));

foreach (var element in rounds)
{

    Console.WriteLine(element.OuterHtml);
}

//Console.WriteLine(document.DocumentElement.OuterHtml);
foreach (var element in rounds)
{
    var rawRound = element.QuerySelector("a").InnerHtml;
    var indexOfLeftBracket = rawRound.IndexOf("<");
    var roundName = rawRound.Substring(0, indexOfLeftBracket).Trim();
    Console.WriteLine(roundName);
    var rawRow = element.QuerySelectorAll("tr");
    bool initCall = true;
    foreach (var singleRow in rawRow)
    {
        if (initCall)
        {
            initCall = false;
            continue;
        }

        var locationTable = singleRow.QuerySelector("td.locationtable");
        Console.WriteLine(locationTable.InnerHtml.Trim());
        // Console.WriteLine(singleRow.InnerHtml);
        var cells = singleRow.QuerySelectorAll("td");
        var counter = 0;
        foreach (var cell in cells)
        {
            if (counter > 2)
            {
                break;
            }
            var items = cell.QuerySelectorAll("a");
            foreach (var item in items)
            {
                var content = item.InnerHtml.Trim();
                if (content.Contains("span"))
                {
                    var firstPart = content.Substring(0, content.IndexOf("<"));
                    var secondPart = content.Substring(content.LastIndexOf(">")+1,content.Length- content.LastIndexOf(">")-1);
                    content = firstPart.Trim()+" - "+secondPart.Trim();
                }
                Console.WriteLine(content);
                counter++;
            }
        }
        
    }
}

Console.ReadLine();
*/
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.ConsoleApp.Helpers;
using System.Runtime;
using System.Text;

var builder = Host.CreateDefaultBuilder();


builder.ConfigureServices((hostContext, services) =>
{ 
    services.ConfigurePersistenceServices(hostContext.Configuration);

    services.AddHostedService<AddReversNameToAnotherNameBackgroundService>();
});

builder.UseConsoleLifetime();
await builder.RunConsoleAsync();
