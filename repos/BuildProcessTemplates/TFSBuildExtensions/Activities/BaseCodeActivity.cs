﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.TeamFoundation.Build.Client;
using System.Activities;
using Microsoft.TeamFoundation.Build.Workflow.Services;
using Microsoft.TeamFoundation.Build.Workflow.Activities;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSBuildExtensions
{
    /// <summary>
    /// Provides a base class to all Activities which support remoting
    /// </summary>
    public abstract class BaseCodeActivity : CodeActivity
    {
        private bool failingbuild;
        private InArgument<bool> logExceptionStack = true;

        private List<string> informationMessageList;

        private List<string> warningMessageList;

        private List<string> errorMessageList;

        /// <summary>
        /// Set to true to fail the build if the activity logs any errors. Default is false
        /// </summary>
        [Description("Set to true to fail the build if errors are logged")]
        public InArgument<bool> FailBuildOnError { get; set; }

        /// <summary>
        /// Set to true to fail the build if the activity logs any errors. Default is false
        /// </summary>
        [Description("Set to true to make all warnings errors")]
        public InArgument<bool> TreatWarningsAsErrors { get; set; }

        /// <summary>
        /// Set to true to ignore any unhandled exceptions thrown by activities. Default is false
        /// </summary>
        [Description("Set to true to ignore unhandled exceptions")]
        public InArgument<bool> IgnoreExceptions { get; set; }

        /// <summary>
        /// Set to true to log the entire stack in the event of an exception. Default is true
        /// </summary>
        [Description("Set to true to log the entire stack in the event of an exception")]
        public InArgument<bool> LogExceptionStack
        {
            get { return this.logExceptionStack; }
            set { this.logExceptionStack = value; }
        }

        /// <summary>
        /// Variable to hold CodeActivityContext
        /// </summary>
        protected CodeActivityContext ActivityContext { get; set; }

        protected IBuildDetail BuildDetail { get; set; }

        public List<string> InformationMessageList
        {
            get { return informationMessageList; }
            set { informationMessageList = value; }
        }

        public List<string> WarningMessageList
        {
            get { return warningMessageList; }
            set { warningMessageList = value; }
        }

        public List<string> ErrorMessageList
        {
            get { return errorMessageList; }
            set { errorMessageList = value; }
        }


        /// <summary>
        /// Entry point to the Activity. It sets the context and executes InternalExecute which is implemented by derived activities
        /// </summary>
        /// <param name="context">CodeActivityContext</param>
        protected override void Execute(CodeActivityContext context)
        {
            this.ActivityContext = context;
            try
            {
                this.BuildDetail = this.ActivityContext.GetExtension<IBuildDetail>();

                this.InternalExecute();
            }
            catch (Exception ex)
            {
                if (!this.failingbuild)
                {
                    if (this.LogExceptionStack.Get(context))
                    {
                        string innerException = string.Empty;
                        if (ex.InnerException != null)
                        {
                            innerException = string.Format("Inner Exception: {0}", ex.InnerException.Message);
                        }

                        this.LogBuildError(string.Format("Error: {0}. Stack Trace: {1}. {2}", ex.Message, ex.StackTrace, innerException));
                    }
                }

                if (this.IgnoreExceptions.Get(context) != true)
                {
                    throw;
                }      
}
            finally
            {
                this.AddMessagesToLogDetail();
            }
        }

        /// <summary>
        /// InternalExecute method which activities should implement
        /// </summary>
        protected abstract void InternalExecute();

        /// <summary>
        /// Logs a message as a build error
        /// Also can fail the build if the FailBuildOnError flag is set
        /// </summary>
        /// <param name="errorMessage">Message to save</param>
        protected void LogBuildError(string errorMessage)
        {
            Debug.WriteLine(string.Format("BuildError: {0}", errorMessage));
            if (this.FailBuildOnError.Get(this.ActivityContext))
            {
                this.failingbuild = true;
                var buildDetail = this.ActivityContext.GetExtension<IBuildDetail>();
                buildDetail.Status = BuildStatus.Failed;
                buildDetail.Save();
                throw new Exception(errorMessage);
            }

            this.ActivityContext.TrackBuildError(errorMessage);
        }

        /// <summary>
        /// Logs a message as a build warning
        /// </summary>
        /// <param name="warningMessage">Message to save</param>
        protected void LogBuildWarning(string warningMessage)
        {
            if (this.TreatWarningsAsErrors.Get(this.ActivityContext))
            {
                this.LogBuildError(warningMessage);
            }
            else
            {
                this.ActivityContext.TrackBuildWarning(warningMessage);
                Debug.WriteLine(string.Format("BuildWarning: {0}", warningMessage));
            }
        }

        /// <summary>
        /// Logs a generical build message
        /// </summary>
        /// <param name="message">The message to save</param>
        /// <param name="importance">The verbosity importance of the message</param>
        protected void LogBuildMessage(string message, BuildMessageImportance importance = BuildMessageImportance.Normal)
        {
            this.ActivityContext.TrackBuildMessage(message, importance);
            Debug.WriteLine(string.Format("BuildMessage: {0}", message));
        }

        /// <summary>
        /// Logs a link to the build log
        /// </summary>
        /// <param name="message">Message to save as link name</param>
        /// <param name="uri">Uri for link</param>
        protected void LogBuildLink(string message, Uri uri)
        {
            IActivityTracking currentTracking = this.ActivityContext.GetExtension<IBuildLoggingExtension>().GetActivityTracking(this.ActivityContext);
            var link = currentTracking.Node.Children.AddExternalLink(message, uri);
            link.Save();
            Debug.WriteLine(string.Format("BuildLink: {0}, Uri: {1}", message, uri));
        }

        /// <summary>
        /// Add a text node to the build log
        /// </summary>
        /// <param name="text">Display text</param>
        /// <param name="parent">Parent node in the build log</param>
        /// <returns>The new node containing the supplied text</returns>
        protected static IBuildInformationNode AddTextNode(string text, IBuildInformationNode parent)
        {
            IBuildInformationNode childNode = parent.Children.CreateNode();
            childNode.Type = parent.Type;
            childNode.Fields.Add("DisplayText", text);
            childNode.Save();
            return childNode;
        }

        /// <summary>
        /// Add a hyperlink to the
        /// </summary>
        /// <param name="text">Display text of the hyperlink</param>
        /// <param name="uri">Uri of the hyperlink</param>
        /// <param name="parent">Parent node in the build log</param>
        /// <returns>The new external link containing the supplied hyperlink</returns>
        protected static IExternalLink AddLinkNode(string text, Uri uri, IBuildInformationNode parent)
        {
            var link = parent.Children.AddExternalLink(text, uri);
            link.Save();
            return link;
        }

        internal void AddMessagesToLogDetail()
        {
            if (informationMessageList != null)
            {
                foreach (var message in informationMessageList)
                {
                    this.ActivityContext.TrackBuildMessage(message, BuildMessageImportance.Normal);
                }
            }

            if (warningMessageList != null)
            {
                foreach (var message in warningMessageList)
                {
                    this.ActivityContext.TrackBuildWarning(message);
                }
            }

            if (errorMessageList != null)
            {
                foreach (var message in errorMessageList)
                {
                    this.ActivityContext.TrackBuildError(message);
                }
            }
        }
    }
}
