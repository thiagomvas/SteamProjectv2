using SharpTables;
using SteamAchieves.Wrapper;
using SteamAchieves.Wrapper.Data;

HttpClient http = new();
string apiKey = File.ReadAllText(@"C:\Users\Thiago\source\steamapikey.txt");
string userId = "76561198379450830";
int drg = 548430;

var api = new SteamApiService(apiKey);

var game = await api.GetGameDetailsForUserAsync(drg, userId);

List<Row> rows = new List<Row>();
int maxLength = 50;
foreach(var prop in game.GetType().GetProperties())
{
	var result = prop.GetValue(game);
	string val = result?.ToString() ?? "N/A";
	val = val.Length > maxLength ? val[..maxLength] + "..." : val;
	rows.Add(new Row([prop.Name, val]));
}

Table table = new Table();

rows.ForEach(table.AddRow);

table.Print();

Table ach  = Table.FromDataSet(game.Achievements, a =>
{
	string desc = a.Description.Length > maxLength ? a.Description[..maxLength] + "..." : a.Description;
	return new Row([a.IsUnlocked ? "Yes" : "No", a.Name, desc, a.ApiName]);
});

ach.SetHeader(new("Unlocked", "Name", "Description", "API Name"));

ach.Print();