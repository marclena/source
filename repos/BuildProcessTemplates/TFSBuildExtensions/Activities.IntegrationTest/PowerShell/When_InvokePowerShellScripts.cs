using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Management.Automation;

namespace Activities.IntegrationTest.PowerShell
{
    [TestClass]
    public class When_InvokePowerShellScripts
    {
        [TestMethod]
        public void Then_MultipleScriptsRunningInParallel()
        {
            var target = new TFSBuildExtensions.PowerShell.InvokePowerShellCommand{
                Script = @"Start-Sleep -s 5"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();
        }

        [TestMethod]
        public void Then_RunningSleepPowerShellCommandAsync_IsRunned()
        {
            var target = new TFSBuildExtensions.PowerShell.InvokePowershellCommandAsync
            {
                Script = @"Start-Sleep -s 5"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();
        }

        [TestMethod]
        public void Then_RunningWrite_HostPowerShellCommandAsync_IsRunned()
        {
            var target = new TFSBuildExtensions.PowerShell.InvokePowershellCommandAsync
            {
                Script = @"Write-Host foo"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();
        }

        [TestMethod]
        public void Then_RunningGetAppPoolStatusPowerShellCommandAsync_IsRunned()
        {
            var target = new TFSBuildExtensions.PowerShell.InvokePowershellCommandAsync
            {
                Script = @"C:\workspaces\tfs\Vueling\BuildProcessTemplate\PsTools\GetAppPoolState.ps1",
                Arguments = " wbcnvueintback1 tfsbuild **** Vueling.XXX.WCF.REST.WebService"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();
        }

        [TestMethod]
        public void Then_RunningRestartAppPoolPowershellCommandAsync_IsRunned()
        {
            var target = new TFSBuildExtensions.PowerShell.InvokePowershellCommandAsync
            {
                Script = @"C:\workspaces\tfs\Vueling\Vueling.Build\INT\Vueling.Build.Scripts.ConsoleUI\powershell\RestartAppPool.ps1",
                Arguments = " wbcnvueintwww01 tfsbuild ******* SkySales"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            PSObject[] psResults = (PSObject[])output["Result"];
        }
    }
}
