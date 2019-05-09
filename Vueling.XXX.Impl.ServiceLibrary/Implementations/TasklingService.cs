using ATC.Taskling.Client.Contracts.ServiceLibrary;
using ATC.Taskling.Client.Contracts.ServiceLibrary.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.Implementations
{
    [RegisterService]
    public class TasklingService : ITasklingService
    {
        #region ..: Fields :..

        private readonly ITasklingClient _tasklingClient;

        #endregion

        #region ..: Constructor :..

        public TasklingService(ITasklingClient tasklingClient)
        {
            _tasklingClient = tasklingClient;
        }

        #endregion

        #region ..: Public Methods :..

        public void CreationBlocksExample(ITaskExecutionContext executionContext)
        {
            // NUMERIC RANGE BLOCK EXAMPLE

            var lastNumericValue = GetLastNumericValue(executionContext);

            CreateAndProcessNumericRangeBlock(executionContext, lastNumericValue, lastNumericValue + 100);

            // DATE RANGE BLOCK EXAMPLE

            var lastDateProcessed = GetLastDateValue(executionContext);

            CreateAndProcessDateRangeBlock(executionContext, lastDateProcessed, DateTime.Now);

            // OBJECT BLOCK EXAMPLE

            var flightObject = new FlightDTO("1000", DateTime.Now);

            CreateAndProcessObjectBlock(executionContext, flightObject);

            // LIST BLOCK EXAMPLE

            var flightObjects = new List<FlightDTO>
            {
                new FlightDTO("1000", DateTime.Now),
                new FlightDTO("1001", DateTime.Now),
                new FlightDTO("1002", DateTime.Now)
            };

            CreateAndProcessListBlocks(executionContext, flightObjects);
        }

        #endregion

        #region ..: Private Methods :..

        private long GetLastNumericValue(ITaskExecutionContext executionContext)
        {
            var tasklingQueryClient = 
                _tasklingClient.CreateTasklingQueryClient(
                    executionContext.ApplicationName, executionContext.TaskName);

            var lastNumericRangeBlock = tasklingQueryClient.GetLastNumericRangeBlock(LastBlockOrder.LastCreated, BlockExecutionStatus.Completed);

            return lastNumericRangeBlock != null ? lastNumericRangeBlock.EndNumber : 0;
        }

        private DateTime GetLastDateValue(ITaskExecutionContext executionContext)
        {
            var tasklingQueryClient = 
                _tasklingClient.CreateTasklingQueryClient(
                    executionContext.ApplicationName, executionContext.TaskName);

            var lastDateRangeBlock = tasklingQueryClient.GetLastDateRangeBlock(LastBlockOrder.LastCreated, BlockExecutionStatus.Completed);

            return lastDateRangeBlock != null ? lastDateRangeBlock.EndDate : DateTime.Now.AddDays(-1);
        }

        public void CreateAndProcessNumericRangeBlock(ITaskExecutionContext executionContext,
            long startValue, long endValue)
        {
            var interval = 10;
            var numericRangeBlockContexts = executionContext.GetNumericRangeBlocks(x => x.WithRange(startValue, endValue, interval));

            foreach(var numericRangeBlockContext in numericRangeBlockContexts)
            {
                try
                {
                    numericRangeBlockContext.Start();

                    var numericRangeBlock = numericRangeBlockContext.NumericRangeBlock;

                    // DO SOMETHING WITH THE numericRangeBlock VALUES:
                    //    numericRangeBlock.StartNumber
                    //    numericRangeBlock.EndNumber

                    numericRangeBlockContext.Complete();
                }
                catch(Exception ex)
                {
                    numericRangeBlockContext.Failed(ex.ToString());
                }
            }
        }

        public void CreateAndProcessDateRangeBlock(ITaskExecutionContext executionContext,
            DateTime startDate, DateTime endDate)
        {
            var intervalTime = new TimeSpan(1,0,0);
            var dateRangeBlockContexts = executionContext.GetDateRangeBlocks(x => x.WithRange(startDate, endDate, intervalTime));

            foreach(var dateRangeBlockContext in dateRangeBlockContexts)
            {
                try
                {
                    dateRangeBlockContext.Start();

                    var dateRangeBlock = dateRangeBlockContext.DateRangeBlock;

                    // DO SOMETHING WITH THE dateRangeBlock VALUES:
                    //    dateRangeBlock.StartDate
                    //    dateRangeBlock.EndDate

                    dateRangeBlockContext.Complete();
                }
                catch(Exception ex)
                {
                    dateRangeBlockContext.Failed(ex.ToString());
                }
            }
        }

        public void CreateAndProcessObjectBlock<T>(ITaskExecutionContext executionContext,
            T objectValue)
        {
            var objectBlockContext = executionContext.GetObjectBlocks<T>(x => x.WithObject(objectValue)).FirstOrDefault();

            try
            {
                objectBlockContext.Start();

                var objectBlock = objectBlockContext.Block;

                // DO SOMETHING WITH THE objectBlock VALUE:
                //    objectBlock.Object

                objectBlockContext.Complete();
            }
            catch(Exception ex)
            {
                objectBlockContext.Failed(ex.ToString());
            }
        }

        public void CreateAndProcessListBlocks<T>(ITaskExecutionContext executionContext,
            List<T> objectValues)
        {
            short maxBlockValues = 100;
            var listBlockContexts = executionContext.GetListBlocks<T>(x => x.WithPeriodicCommit(objectValues, maxBlockValues, BatchSize.Ten));

            foreach(var listBlockContext in listBlockContexts)
            {
                try
                {
                    listBlockContext.Start();

                    var itemContexts = listBlockContext.GetItems();

                    foreach(var itemContext in itemContexts)
                    {
                        try
                        {
                            // DO SOMETHING WITH THE itemContext VALUE:
                            //    itemContext.Value

                            var itemValue = itemContext.Value;

                            // IF THE OBJECT VALUE ARE DISCARDED:

                            itemContext.Discarded("Discard message");

                            // ELSE IF THE OBJECT VALUE HAS BEEN PROCESSED CORRECTLY

                            itemContext.Completed();
                        }
                        catch(Exception ex)
                        {
                            itemContext.Failed(ex.ToString());
                        }
                    }

                    listBlockContext.Complete();
                }
                catch(Exception ex)
                {
                    listBlockContext.Failed(ex.ToString());
                }
            }
        }

        #endregion
    }
}
