using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using EmptyControllerAndViewScaffolder.TemplateModels;
using Microsoft.Extensions.ProjectModel;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;

namespace EmptyControllerAndViewScaffolder
{
    [Alias("controllerandview")]
    public class ControllerAndViewCodeGenerator : ICodeGenerator
    {
        private const string ControllerTemplateName = "EmptyController.cshtml";
        private const string ViewTemplateName = "EmptyView.cshtml";

        private IProjectContext _projectContext;
        private IApplicationInfo _applicationInfo;
        private ICodeGeneratorActionsService _codeGeneratorActionsService;
        private IServiceProvider _serviceProvider;
        private ILogger _logger;

        public ControllerAndViewCodeGenerator(IProjectContext projectContext,
            IApplicationInfo applicationInfo,
            ICodeGeneratorActionsService codeGeneratorActionsService,
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            _projectContext = projectContext ?? throw new ArgumentNullException(nameof(projectContext));
            _applicationInfo = applicationInfo ?? throw new ArgumentNullException(nameof(applicationInfo));
            _codeGeneratorActionsService = codeGeneratorActionsService ?? throw new ArgumentNullException(nameof(codeGeneratorActionsService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task GenerateCode(ControllerAndViewCodeGeneratorModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            ValidateModel(model);
            

            var namespaceName = string.IsNullOrEmpty(model.ControllerNamespace)
                ? GetDefaultControllerNamespace(model.ControllerRelativePath)
                : model.ControllerNamespace;

            var controllerTemplateModel = new ControllerTemplateModel()
            {
                ControllerName = model.ControllerName,
                ControllerNamespace = namespaceName
            };

            bool isLayoutSelected = !model.IsPartialView &&
                (model.UseDefaultLayout || !String.IsNullOrEmpty(model.LayoutPageFile));

            var viewTemplateModel = new ViewTemplateModel()
            {
                ViewName = model.ViewName,
                LayoutPageFile = model.LayoutPageFile,
                IsPartialView = model.IsPartialView,
                IsLayoutPageSelected = isLayoutSelected,
            };

            var outputPaths = ValidateAndGetOutputPath(model);

            await _codeGeneratorActionsService.AddFileFromTemplateAsync(outputPaths.ControllerPath, ControllerTemplateName, TemplateFolders, controllerTemplateModel);
            _logger.LogMessage("Added Controller : " + outputPaths.ControllerPath.Substring(_applicationInfo.ApplicationBasePath.Length));
            await _codeGeneratorActionsService.AddFileFromTemplateAsync(outputPaths.ViewPath, ViewTemplateName, TemplateFolders, viewTemplateModel);
            _logger.LogMessage("Added View : " + outputPaths.ViewPath.Substring(_applicationInfo.ApplicationBasePath.Length));
        }

        protected (string ControllerPath, string ViewPath) ValidateAndGetOutputPath(ControllerAndViewCodeGeneratorModel commandLineModel)
        {

            var controllerPath = ValidateAndGetOutputPath(commandLineModel.ControllerRelativePath, commandLineModel.ControllerName + ".cs", commandLineModel.IsForce);
            var viewPath = ValidateAndGetOutputPath(commandLineModel.ViewRelativePath, commandLineModel.ViewName + ".cshtml", commandLineModel.IsForce);
            return (ControllerPath: controllerPath, ViewPath: viewPath);
        }

        private string ValidateAndGetOutputPath(string relativeFolderPath, string outputFileName, bool isForceOverwrite)
        {
            string outputFolder = String.IsNullOrEmpty(relativeFolderPath)
                ? _applicationInfo.ApplicationBasePath
                : Path.Combine(_applicationInfo.ApplicationBasePath, relativeFolderPath);

            var outputPath = Path.Combine(outputFolder, outputFileName);

            if (File.Exists(outputPath) && !isForceOverwrite)
            {
                throw new InvalidOperationException($"File already exists '{outputPath}' use -f to force over write.");
            }

            return outputPath;
        }

        private void ValidateModel(ControllerAndViewCodeGeneratorModel model)
        {
            if (string.IsNullOrEmpty(model.ControllerName))
            {
                throw new ArgumentNullException(nameof(model.ControllerName));
            }

            if (string.IsNullOrEmpty(model.ViewName))
            {
                throw new ArgumentNullException(nameof(model.ViewName));
            }

            ValidateNameSpaceName(model);
        }

        protected void ValidateNameSpaceName(ControllerAndViewCodeGeneratorModel generatorModel)
        {
            if (!string.IsNullOrEmpty(generatorModel.ControllerNamespace) &&
                !RoslynUtilities.IsValidNamespace(generatorModel.ControllerNamespace))
            {
                throw new InvalidOperationException($"The namespace name '{generatorModel.ControllerNamespace}' is invalid.");
            }
        }

        protected string GetDefaultControllerNamespace(string relativeFolderPath)
        {
            return NameSpaceUtilities.GetSafeNameSpaceFromPath(relativeFolderPath, _applicationInfo.ApplicationName);
        }

        protected IEnumerable<string> TemplateFolders
        {
            get
            {
                return TemplateFoldersUtilities.GetTemplateFolders(
                    containingProject: this.GetType().GetTypeInfo().Assembly.GetName().Name,
                    applicationBasePath: _applicationInfo.ApplicationBasePath,
                    baseFolders: new[] { "ControllerGenerator", "ViewGenerator" },
                    projectContext: _projectContext);
            }
        }
    }
}
