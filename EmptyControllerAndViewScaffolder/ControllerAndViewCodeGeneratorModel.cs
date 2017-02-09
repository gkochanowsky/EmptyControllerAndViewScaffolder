using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;

namespace EmptyControllerAndViewScaffolder
{
    public class ControllerAndViewCodeGeneratorModel
    {
        [Option(Name = "controllerName", ShortName = "cName", Description="Name for the controller")]
        public string ControllerName { get; set; }

        [Option(Name = "controllerNamespace", ShortName = "cNamespace", Description = "Namespace for the controller")]
        public string ControllerNamespace { get; set; }

        [Option(Name = "controllerRelativePath", ShortName = "cPath", Description = "Relative path for the controller")]
        public string ControllerRelativePath { get; set; }

        [Option(Name = "viewName", ShortName = "vName", Description = "Name for the View")]
        public string ViewName { get; set; }

        [Option(Name = "viewRelativePath", ShortName = "vPath", Description = "Relative Path for the view.")]
        public string ViewRelativePath { get; set; }

        [Option(Name = "layout", ShortName = "l", Description = "Custom layout page to use.")]
        public string LayoutPageFile { get; set; }

        [Option(Name = "partialView", ShortName = "partial", Description = "Specifies if the view should be created as a partial view.")]
        public bool IsPartialView { get; set; }

        [Option(Name = "UseDefaultLayout", ShortName = "udl", Description = "Specifies whether to use Default layout page.")]
        public bool UseDefaultLayout { get; set; }

        [Option(Name = "force", ShortName="f", Description = "Specifies whether to force overwrite existing files.")]
        public bool IsForce { get; set; }
    }
}
