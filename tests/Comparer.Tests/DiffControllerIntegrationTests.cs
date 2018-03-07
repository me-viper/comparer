using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using ComparerService.App;
using ComparerService.App.Models;
using ComparerService.App.Services;
using ComparerService.App.Utility;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;

using Newtonsoft.Json;

using NUnit.Framework;

namespace Comparer.Tests
{
    [TestFixture]
    public class DiffControllerIntegrationTests
    {
        private TestServer _server;

        [SetUp]
        public void Init()
        {
            _server = new TestServer(
                new WebHostBuilder()
                    .UseStartup<TestStartup>()
                    .ConfigureServices(p => p.AddAutofac())
                );
        }

        [TearDown]
        public void Stop()
        {
            _server?.Dispose();
        }

        [Test]
        public async Task DiffEqualContentTest()
        {
            var client = _server.CreateClient();

            const string stringToCompare = "This is sample text";

            // Set left side.
            var postLeftResponse = await SetSideContent(client, "left", stringToCompare).ConfigureAwait(false);
            postLeftResponse.EnsureSuccessStatusCode();

            // Set right side.
            var postRightResponse = await SetSideContent(client, "right", stringToCompare).ConfigureAwait(false);
            postRightResponse.EnsureSuccessStatusCode();

            // Compare.
            var diffResposne = await client.GetAsync("/v1/diff/1").ConfigureAwait(false);
            diffResposne.EnsureSuccessStatusCode();

            // Assert.
            var resultString = await diffResposne.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<DiffResultDto>(resultString);

            Assert.That(result.DiffType, Is.EqualTo(DiffType.Equal));
            Assert.That(result.Diffs, Is.Empty);
            Assert.That(result.Id, Is.EqualTo("1"));
        }

        [Test]
        public async Task DiffNotEqualContentSizeTest()
        {
            var client = _server.CreateClient();

            const string stringToCompareLeft = "This is sample text";
            const string stringToCompareRight = "This is another sample text";

            // Set left side.
            var postLeftResponse = await SetSideContent(client, "left", stringToCompareLeft).ConfigureAwait(false);
            postLeftResponse.EnsureSuccessStatusCode();

            // Set right side.
            var postRightResponse = await SetSideContent(client, "right", stringToCompareRight).ConfigureAwait(false);
            postRightResponse.EnsureSuccessStatusCode();

            // Compare.
            var diffResposne = await client.GetAsync("/v1/diff/1").ConfigureAwait(false);
            diffResposne.EnsureSuccessStatusCode();

            // Assert.
            var resultString = await diffResposne.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<DiffResultDto>(resultString);

            Assert.That(result.DiffType, Is.EqualTo(DiffType.SizeDoesNotMatch));
            Assert.That(result.Diffs, Is.Empty);
            Assert.That(result.Id, Is.EqualTo("1"));
        }

        [Test]
        public async Task DiffContentTest()
        {
            var client = _server.CreateClient();

            const string stringToCompareLeft = "11 aa xx bb 00";
            const string stringToCompareRight = "22 aa zz bb 99";

            // Set left side.
            var postLeftResponse = await SetSideContent(client, "left", stringToCompareLeft).ConfigureAwait(false);
            postLeftResponse.EnsureSuccessStatusCode();

            // Set right side.
            var postRightResponse = await SetSideContent(client, "right", stringToCompareRight).ConfigureAwait(false);
            postRightResponse.EnsureSuccessStatusCode();

            // Compare.
            var diffResposne = await client.GetAsync("/v1/diff/1").ConfigureAwait(false);
            diffResposne.EnsureSuccessStatusCode();

            // Assert.
            var resultString = await diffResposne.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<DiffResultDto>(resultString);

            var resultDiffs = result.Diffs.ToArray();

            Assert.That(result.DiffType, Is.EqualTo(DiffType.Diff));
            Assert.That(result.Diffs, Has.Exactly(3).Items);
            Assert.That(result.Id, Is.EqualTo("1"));

            Assert.That(resultDiffs[0], Is.EqualTo(new DiffSpan(1, 2)));
            Assert.That(resultDiffs[1], Is.EqualTo(new DiffSpan(7, 2)));
            Assert.That(resultDiffs[2], Is.EqualTo(new DiffSpan(13, 2)));
        }

        [Test]
        public async Task DiffNoContentToCompareTest()
        {
            var client = _server.CreateClient();

            var diffResposne = await client.GetAsync("/v1/diff/1").ConfigureAwait(false);
            diffResposne.EnsureSuccessStatusCode();

            Assert.That(diffResposne.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(diffResposne.Content.AsString(), Is.Empty);
        }

        [Test]
        public async Task InvalidContentLeftSideTest()
        {
            var client = _server.CreateClient();

            var postContent = new StringContent(JsonConvert.SerializeObject("Not base64 string!"), Encoding.UTF8, "application/json");
            var postResposnse = await client.PostAsync("/v1/diff/1/left", postContent).ConfigureAwait(false);

            Assert.That(!postResposnse.IsSuccessStatusCode);
            Assert.That(postResposnse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(postResposnse.Content.AsString(), Does.StartWith("\"Invalid content format"));
        }

        [Test]
        public async Task InvalidContentRightSideTest()
        {
            var client = _server.CreateClient();

            var postContent = new StringContent(JsonConvert.SerializeObject("Not base64 string!"), Encoding.UTF8, "application/json");
            var postResposnse = await client.PostAsync("/v1/diff/1/right", postContent).ConfigureAwait(false);

            Assert.That(!postResposnse.IsSuccessStatusCode);
            Assert.That(postResposnse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(postResposnse.Content.AsString(), Does.StartWith("\"Invalid content format"));
        }

        private static async Task<HttpResponseMessage> SetSideContent(HttpClient client, string side, string content)
        {
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
            var postContent = new StringContent(JsonConvert.SerializeObject(encodedString), Encoding.UTF8, "application/json");
            return await client.PostAsync($"/v1/diff/1/{side}", postContent).ConfigureAwait(false);
        }

        public class TestStartup
        {
            public TestStartup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc();
            }

            public void ConfigureContainer(ContainerBuilder builder)
            {
                builder.RegisterType<DiffService>().As<IDiffService>();
                builder.RegisterType<InMemoryRepository>().As<IComparisonContentRepository>().SingleInstance();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                app.UseDeveloperExceptionPage();
                app.UseErrorLogging();

                app.UseMvc();
            }
        }
    }
}