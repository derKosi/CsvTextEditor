﻿namespace CsvTextEditor
{
    using System.Diagnostics;
    using Catel;
    using Catel.Configuration;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Orc.ProjectManagement;
    using Services;

    public class FileOpenInTextEditorCommandContainer : FileOpenInExternalToolCommandContainerBase
    {
        private readonly IFileExtensionService _fileExtensionService;
        private readonly IProcessService _processService;
        private readonly IConfigurationService _configurationService;

        public FileOpenInTextEditorCommandContainer(ICommandManager commandManager, IProjectManager projectManager, IFileExtensionService fileExtensionService,
            IFileService fileService, IProcessService processService, IConfigurationService configurationService)
            : base(Commands.File.OpenInTextEditor, "txt", commandManager, projectManager, fileExtensionService, fileService, processService)
        {
            Argument.IsNotNull(() => fileExtensionService);
            Argument.IsNotNull(() => configurationService);
            Argument.IsNotNull(() => processService);

            _fileExtensionService = fileExtensionService;
            _processService = processService;
            _configurationService = configurationService;
        }

        protected override void Execute(object parameter)
        {
            var externalToolPath = _configurationService.GetRoamingValue<string>(Configuration.CustomEditor, null);

            if(string.Equals(externalToolPath, "null"))
            {
                externalToolPath = null;
            }

            var toolPath = !string.IsNullOrEmpty(externalToolPath) ? externalToolPath : _fileExtensionService.GetRegisteredTool("txt");

            if(!string.IsNullOrEmpty(toolPath))
            {
                _processService.StartProcess(toolPath, _projectManager.ActiveProject.Location);
            }
            else
            {
                Process.Start("notepad.exe", _projectManager.ActiveProject.Location);
            }
        }
    }
}
