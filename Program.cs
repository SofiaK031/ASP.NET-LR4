using LR4;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("CustomConfigurations/books.json");
builder.Configuration.AddJsonFile("CustomConfigurations/libraryUsers.json");
builder.Services.Configure<List<Book>>(builder.Configuration.GetSection("Books"));
builder.Services.Configure<List<LibraryUser>>(builder.Configuration.GetSection("Users"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// HTML generation function for menu
string GenerateNavbar()
{
	return @"
		<style>
			body {
				margin: 0;
				padding: 0;
				font-family: Georgia;
			}
			.navbar {
				overflow: hidden;
				background-color: #333;
				position: fixed;
				display: flex;
				justify-content: center;
				top: 0;
				width: 100%;
				z-index: 1000;
				font-family: Georgia;
			}
			.navbar a {
				float: left;
				display: block;
				color: #f2f2f2;
				text-align: center;
				padding: 15px 20px;
				text-decoration: none;
				font-size: 18px;
			}
			.navbar a:hover {
				background-color: #ddd;
				color: black;
			}
			.content { padding: 50px 20px 20px; }
			p {	text-align: center;	}
		</style>
		<div class='navbar'>
			<a href='/'>Home</a>
			<a href='/library/'>Library</a>
			<a href='/library/books'>Books</a>
			<a href='/library/profile/'>Profile</a>
		</div>
	";
}

// Home page
app.MapGet("/", (HttpContext httpContext) =>
{
	var body = GenerateNavbar() + @"
		<div class='content'>
			<p>Welcome! Choose a section from the menu above.</p>
		</div>
	";

	httpContext.Response.ContentType = "text/html";
	httpContext.Response.WriteAsync(body);
});

// Library page
app.MapGet("/library", (HttpContext httpContext) =>
{
	var body = GenerateNavbar() + @"
		<div class='content'>
			<p>Greeting from library! Welcome to the books' world!</p>
		</div>
	";

	httpContext.Response.ContentType = "text/html";
	httpContext.Response.WriteAsync(body);
});

// Page with books
app.MapGet("/library/books", (IOptions<List<Book>> books, HttpContext httpContext) =>
{
	var body = GenerateNavbar() + "<div class='content'><p style='font-weight: bold;'>Books</p>";

	foreach (var book in books.Value)
	{
		body += $"{book.ToString()}<br><br>";
	}

	body += "<a href='/'>Back to the Home page</a></div>";
	httpContext.Response.ContentType = "text/html";
	httpContext.Response.WriteAsync(body);
});

// User profile page
app.MapGet("/library/profile/{id:int:min(0)?}", (IOptions<List<LibraryUser>> users, int? id, HttpContext httpContext) =>
{
	int maxId = users.Value.Count - 1;

	var body = GenerateNavbar();
	body += "<div class='content' style='margin-top: 10px;'>";

	if (id == null || id < 0 || id > maxId)
	{
		body += users.Value[0].ToString();
	}
	else
	{
		body += users.Value[(int)id].ToString();
	}

	body += "</div>";
	httpContext.Response.ContentType = "text/html";
	httpContext.Response.WriteAsync(body);
});

app.Run();
