﻿namespace Mihaylov.Common.Host.Abstract.AssemblyVersion
{
    public interface IModuleInfo
    {        
        string ModuleName { get; }

        string Version { get; }

        string Framework { get; }

        string BuildDate { get; }

        string GitCommit { get; }

        string JenkinsBuildNumber { get; }
    }
}