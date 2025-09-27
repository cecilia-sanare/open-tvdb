using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTVDB.API.Database;

namespace OpenTVDB.API.Tests;

public class WebApplicationFactoryTest : WebApplicationFactory<Program>
{
}
