using StackUnderFlow.Domains.Model;
using StackUnderFlow.Domains.Repository;
using StackUnderFlow.Infrastructure.Kubernetes;

namespace StackUnderFlow.Domains.Services
{
    public class RunnerService(
        IScriptRepository scriptRepository,
        IScriptVersionRepository scriptVersionRepository,
        INotificationService notificationService,
        KubernetesService kubernetesService)
        : IRunnerService
    {
        public async Task<string> ExecuteScript(int scriptId)
        {
            var script = await scriptRepository.GetScriptById(scriptId) ?? throw new InvalidOperationException($"Script with ID {scriptId} not found.");
            var scriptBinary = (await scriptVersionRepository.GetLatestScriptVersionByScriptId(script.ScriptId))?.SourceScriptBinary ?? throw new InvalidOperationException($"Script binary for script ID {scriptId} not found.");

            using var reader = new StreamReader(new MemoryStream(scriptBinary));
            var scriptContent = await reader.ReadToEndAsync();
            string output;

            try
            {
                output = script.ProgrammingLanguage.Equals("Python", StringComparison.OrdinalIgnoreCase)
                    ? await kubernetesService.ExecutePythonScript("default", scriptContent)
                    : script.ProgrammingLanguage.Equals("Csharp", StringComparison.OrdinalIgnoreCase)
                    ? await kubernetesService.ExecuteCsharpScript("default", scriptContent)
                    : throw new NotSupportedException($"Script file extension for script ID {scriptId} is not supported.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to execute script ID {scriptId}: {ex.Message}", ex);
            }

            return output;
        }

        public async Task<string> ExecuteScriptFile(IFormFile? script)
        {
            if (script == null || script.Length == 0)
                return "Script file is null or empty.";

            string output;
            try
            {
                var fileExtension = Path.GetExtension(script.FileName).ToLowerInvariant();
                using var reader = new StreamReader(script.OpenReadStream());
                var scriptContent = await reader.ReadToEndAsync();

                output = fileExtension switch
                {
                    ".py" => await kubernetesService.ExecutePythonScript("default", scriptContent),
                    ".csx" => await kubernetesService.ExecuteCsharpScript("default", scriptContent),
                    _ => $"Unsupported script file extension: {fileExtension}"
                };
            }
            catch (Exception ex)
            {
                return $"Failed to execute script file {script.FileName}: {ex.Message}";
            }

            return output;
        }

        private async Task<string> ExecuteScriptWithInput(Script script, string inputFileBinary,
            string pipelineRequestPipelineId)
        {
            var scriptBinary = (await scriptVersionRepository.GetLatestScriptVersionByScriptId(script.ScriptId))?.SourceScriptBinary ?? throw new InvalidOperationException($"Script binary for script ID {script.ScriptId} not found.");

            using var reader = new StreamReader(new MemoryStream(scriptBinary));
            var scriptContent = await reader.ReadToEndAsync();
            string output;
            try
            {
                output = script.ProgrammingLanguage.Equals("Python", StringComparison.OrdinalIgnoreCase)
                    ? await kubernetesService.ExecutePythonScriptWithInput("default", scriptContent, inputFileBinary, script.InputScriptType, script.OutputScriptType, pipelineRequestPipelineId)
                    : script.ProgrammingLanguage.Equals("Csharp", StringComparison.OrdinalIgnoreCase)
                    ? await kubernetesService.ExecuteCsharpScriptWithInput("default", scriptContent, inputFileBinary, script.InputScriptType, script.OutputScriptType, pipelineRequestPipelineId)
                    : throw new NotSupportedException($"Script file extension for script ID {script.ScriptId} is not supported.");
            }
            catch (Exception ex)
            {
                await notificationService.SendMessageAsync(pipelineRequestPipelineId, $"Failed to execute script ID {script.ScriptId}: {ex.Message}");
                throw new InvalidOperationException($"Failed to execute script ID {script.ScriptId}: {ex.Message}", ex);
            }

            return output;
        }
        
        public async Task<List<IFormFile>> ExecutePipelineWithIds(List<int> scriptIds, IFormFile input,
            string pipelineRequestPipelineId)
        {
            await notificationService.SendMessageAsync(pipelineRequestPipelineId, "Making sure scripts are compatible...");
            if (GetFileType(Path.GetExtension(input.FileName).ToLowerInvariant()) == SupportedType.Unsupported)
                throw new InvalidOperationException($"Unsupported input file type: {Path.GetExtension(input.FileName).ToLowerInvariant()}");
            
            var scripts = new List<Script>();
            foreach (var scriptId in scriptIds)
            {
                var script = await scriptRepository.GetScriptById(scriptId) ?? throw new InvalidOperationException($"Script with ID {scriptId} not found.");
                scripts.Add(script);
            }
            
            if (GetFileType(scripts[0].InputScriptType) != GetFileType(Path.GetExtension(input?.FileName)?.ToLowerInvariant()))
                throw new InvalidOperationException($"The input type of script ID {scripts[0].ScriptId} does not match the input file type.");
            
            for (var i = 0; i < scripts.Count - 1; i++)
            {
                if (scripts[i].OutputScriptType != scripts[i + 1].InputScriptType)
                    throw new InvalidOperationException($"The output type of script ID {scripts[i].ScriptId} does not match the input type of script ID {scripts[i + 1].ScriptId}.");
            }

            var inputFileBase64 = ConvertFileToBase64(input!);
            var output = new List<string>();
            foreach (var script in scripts)
            {
                await notificationService.SendMessageAsync(pipelineRequestPipelineId, $"Executing script ID {script.ScriptId}...");
                var result = await ExecuteScriptWithInput(script, inputFileBase64, pipelineRequestPipelineId);
                await notificationService.SendMessageAsync(pipelineRequestPipelineId, $"Script ID {script.ScriptId} executed successfully.");
                output.Add(result);
                inputFileBase64 = result;
            }
            var finalOutputs = output.Select((base64String, i) => ConvertBase64ToFile(base64String, $"output-{i + 1}{scripts[i].OutputScriptType}")).ToList();
            return finalOutputs;
        }
        
        private static string ConvertFileToBase64(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();
            return Convert.ToBase64String(fileBytes);
        }

        private static IFormFile ConvertBase64ToFile(string base64String, string fileName)
        {
            var fileBytes = Convert.FromBase64String(base64String);
            return new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, null!, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream",
                ContentDisposition = $"form-data; name=\"file\"; filename=\"{fileName}\""
            };
        }

        private static SupportedType GetFileType(string? input) => input switch
        {
            ".txt" => SupportedType.Text,
            ".jpeg" => SupportedType.JPEG,
            ".png" => SupportedType.PNG,
            ".pdf" => SupportedType.PDF,
            ".xlsx" => SupportedType.XLSX,
            _ => SupportedType.Unsupported
        };
    }
}