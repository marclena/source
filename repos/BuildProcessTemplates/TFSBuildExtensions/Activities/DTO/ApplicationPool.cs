using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.DTO
{
    [Serializable]
    public class ApplicationPool
    {
        private int id;
        private int idSite;
        private string name;
        private string applicationPoolType;
        private string netCLRVersion = "v4.0";
        private bool enable32BitApplications = false;
        private string managedPipelineMode = "Integrated";
        private int queueLength = 1000;
        private int cpuLimit = 0;
        private string cpuLimitAction = "NoAction";
        private int cpuLimitInterval = 5;
        private string identity = "ApplicationPoolIdentity";
        private int idleTimeOut = 20;
        private bool loadUserProfile = true;
        private int maximumWorkerProcesses = 1;
        private int recyclingRegularTimeInterval = 1440;
        private int privateMemoryLimit = 0;
        private int virtualMemoryLimit = 0;
        private bool generateRecycleEventLogEntryManualRecycle = true;
        private bool generateRecycleEventLogEntryPrivateMemoryLimitExceeded = true;
        private bool generateRecycleEventLogEntryVirtualMemoryLimitExceeded = true;


        public int Id {
            get { return id; }
            set { id = value; }
        }

        public int IdSite {
            get { return idSite; }
            set { idSite = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public string ApplicationPoolType {
            get { return applicationPoolType; }
            set { applicationPoolType = value; }
        }

        public string NETCLRVersion {
            get { return netCLRVersion; }
            set { netCLRVersion = value; }
        }

        public bool Enable32BitApplications {
            get { return enable32BitApplications; }
            set { enable32BitApplications = value; }
        }

        public string ManagedPipelineMode {
            get { return managedPipelineMode; }
            set { managedPipelineMode = value; }
        }

        public int QueueLength {
            get { return queueLength; }
            set { queueLength = value; }
        }

        public int CPULimit {
            get { return cpuLimit; }
            set { cpuLimit = value; }
        }

        public string CPULimitAction {
            get { return cpuLimitAction; }
            set { cpuLimitAction = value; }
        }

        public int CPULimitInterval {
            get { return cpuLimitInterval; }
            set { cpuLimitInterval = value; }
        }

        public string Identity {
            get { return identity; }
            set { identity = value; }
        }

        public ProcessModelIdentityType GetIdentityType()
        {
            ProcessModelIdentityType processModelIdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;

            switch (Identity)
            {
                case "ApplicationPoolIdentity":
                    processModelIdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                    break;
                case "LocalService":
                    processModelIdentityType = ProcessModelIdentityType.LocalService;
                    break;
                case "LocalSystem":
                    processModelIdentityType = ProcessModelIdentityType.LocalSystem;
                    break;
                case "NetworkService":
                    processModelIdentityType = ProcessModelIdentityType.NetworkService;
                    break;
                case "SpecificUser":
                    processModelIdentityType = ProcessModelIdentityType.SpecificUser;
                    break;
                       
            }

            return processModelIdentityType;
        }

        public int IdleTimeout {
            get { return idleTimeOut; }
            set { idleTimeOut = value; }
        }

        public bool LoadUserProfile {
            get { return loadUserProfile; }
            set { loadUserProfile = value; }
        }

        public int MaximumWorkerProcesses {
            get { return maximumWorkerProcesses; }
            set { maximumWorkerProcesses = value; }
        }

        public int RecyclingRegularTimeInterval {
            get { return recyclingRegularTimeInterval; }
            set { recyclingRegularTimeInterval = value; }
        }

        public int PrivateMemoryLimit {
            get { return privateMemoryLimit; }
            set { privateMemoryLimit = value; }
        }

        public int VirtualMemoryLimit {
            get { return virtualMemoryLimit; }
            set { virtualMemoryLimit = value; }
        }

        public bool GenerateRecycleEventLogEntryManualRecycle {
            get { return generateRecycleEventLogEntryManualRecycle; }
            set { generateRecycleEventLogEntryManualRecycle = value; }
        }

        public bool GenerateRecycleEventLogEntryPrivateMemoryLimitExceeded {
            get { return generateRecycleEventLogEntryPrivateMemoryLimitExceeded; }
            set { generateRecycleEventLogEntryPrivateMemoryLimitExceeded = value; }
        }

        public bool GenerateRecycleEventLogEntryVirtualMemoryLimitExceeded {
            get { return generateRecycleEventLogEntryVirtualMemoryLimitExceeded; }
            set { generateRecycleEventLogEntryVirtualMemoryLimitExceeded = value; }
        }
    }
}
