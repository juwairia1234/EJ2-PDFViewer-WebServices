using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "MyPolicy";
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    // Use the default property (Pascal) casing
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder.WithOrigins(
                                "https://polite-ocean-0b1256410.1.azurestaticapps.net/",
                                "http://localhost:3000",
                                "https://localhost:3000"
                            )
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
});

// Configure Kestrel options
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // 50 MB
});

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
builder.Services.AddResponseCompression();

var app = builder.Build();

//Register Syncfusion license
string licenseKey = "ORg4AjUWIQA/Gnt2XFhhQlJHfVhdXnxLflFzVWJYdV52flFPcC0sT3RfQFhjT3xTd0FhWn9eeXJQRWteWA==";
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.UseResponseCompression();
app.MapControllers();

app.Run();