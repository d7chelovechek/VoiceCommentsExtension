using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceCommentsExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.VoiceCommentsExtensionPackageString)]
    public sealed class VoiceCommentsExtensionPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(
            CancellationToken cancellationToken, 
            IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }
    }
}