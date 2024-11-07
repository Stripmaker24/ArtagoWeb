using Domain.Helpers;
using Microsoft.AspNetCore.Hosting.Server;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;


namespace Website.Server.Utils
{
    public static class ClientGenerator
    {
        public static void GenerateClients(this WebApplication application, string swaggerLocation, string typescriptClientTarget, string csharpClientTarget)
        {
            var server = application.Services.GetService<IServer>();

            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            string documentationUrl = server.GetServerUrl() + swaggerLocation;
            _ = GenerateTypescript(documentationUrl, typescriptClientTarget);
            _ = GenerateCSharp(documentationUrl, csharpClientTarget);
        }

        public static async Task GenerateTypescript(string documentationUrl, string clientTarget)
        {
            var document = await OpenApiDocument.FromUrlAsync(documentationUrl);
            var settings = new TypeScriptClientGeneratorSettings
            {
                ClassName = "{controller}Client",
                UseTransformOptionsMethod = true,
                ClientBaseClass = "BaseClass"
            };
            var generator = new TypeScriptClientGenerator(document, settings);
            var code = "import BaseClass from \"./BaseClient\"; \n" + generator.GenerateFile();
            File.WriteAllText(clientTarget, code);
        }

        public static async Task GenerateCSharp(string documentationUrl, string clientTarget)
        {
            try
            {
                var document = await OpenApiDocument.FromUrlAsync(documentationUrl);
                var settings = new CSharpClientGeneratorSettings
                {
                    ClassName = "ApiClient",
                    GenerateBaseUrlProperty = false,
                    CSharpGeneratorSettings =
                    {
                        Namespace = "Artago"
                    }
                };
                var generator = new CSharpClientGenerator(document, settings);
                var code = generator.GenerateFile();

                File.WriteAllText(clientTarget, code);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
