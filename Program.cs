using codenames_solver;
using Word2vec.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<CardsState>();
builder.Services.AddScoped<ControlState>();
builder.Services.AddScoped<DataState>();
builder.Services.AddHttpClient<SimilarityClient>("LocalApi", client => client.BaseAddress = new Uri("http://localhost:5073/"));
builder.Services.AddSingleton<Vocabulary>(sp =>
{
    var path = @"C:\Users\rorym\OneDrive\Desktop\Word2vec\nlpl\model_short.bin";
    var vocabulary = new Word2VecBinaryReader().Read(path);
    return vocabulary;
});
builder.Services.AddSingleton<ValidWords>();
builder.Services.AddSingleton<FuzzySort>();
builder.Services.AddScoped<DTOBuilder>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllers();

app.Run();
