/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.DomainModels.OrderDomain;

namespace TradeHub.TradeManager.Server.Tests.Unit
{
    [TestFixture]
    public class TradeTestCases
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionBuySide_CreateTradeObject_TradeIsNotComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Buy, 40, 103, OrderExecutionProvider.Simulated, "0", new Security(){Symbol = "GOOG"}, new DateTime(2015,01,21, 18,20,57));

            Assert.AreEqual(40, trade.TradeSize, "Trade Size");
            Assert.IsTrue(!trade.IsComplete(), "Trade.IsComplete");
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionSellSide_CreateTradeObject_TradeIsNotComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Sell, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            Assert.AreEqual(-40, trade.TradeSize, "Trade Size");
            Assert.IsTrue(!trade.IsComplete(), "Trade.IsComplete");
        }

        [Test]
        [Category("Unit")]
        public void NewExecution_UpdateTradeBuySide_TradeIsNotComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Buy, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionId = "1";
            int executionSize = 13;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionId, executionSize, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            Assert.IsTrue(!trade.IsComplete(), "Trade.IsComplete");
            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionId), "Execution ID");
            Assert.AreEqual(-executionSize, trade.ExecutionDetails[executionId], "Execution Size");
            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionMultiple_UpdateTradeBuySide_TradeIsNotComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Buy, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionIdOne = "1";
            int executionSizeOne = 13;

            // Add new execution
            trade.Add(executionIdOne, executionSizeOne, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            string executionIdTwo = "2";
            int executionSizeTwo = 26;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionIdTwo, executionSizeTwo, 103.5M, new DateTime(2015, 01, 21, 18, 21, 09));

            Assert.IsTrue(!trade.IsComplete(), "Trade.IsComplete");
            
            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdOne), "Execution ID One");
            Assert.AreEqual(-executionSizeOne, trade.ExecutionDetails[executionIdOne], "Execution Size One");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdTwo), "Execution ID Two");
            Assert.AreEqual(-executionSizeTwo, trade.ExecutionDetails[executionIdTwo], "Execution Size Two");

            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(0001, 01, 01, 00, 00, 00), trade.CompletionTime, "Trade Completion Time");
            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecution_UpdateTradeBuySide_TradeIsComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Buy, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionId = "1";
            int executionSize = 40;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionId, executionSize, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");
            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionId), "Execution ID");
            Assert.AreEqual(-executionSize, trade.ExecutionDetails[executionId], "Execution Size");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 59), trade.CompletionTime, "Trade Completion Time");
            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionMultiple_UpdateTradeBuySide_TradeIsComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Buy, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionIdOne = "1";
            int executionSizeOne = 13;

            // Add new execution
            trade.Add(executionIdOne, executionSizeOne, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            string executionIdTwo = "2";
            int executionSizeTwo = 27;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionIdTwo, executionSizeTwo, 102.5M, new DateTime(2015, 01, 21, 18, 21, 09));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdOne), "Execution ID One");
            Assert.AreEqual(-executionSizeOne, trade.ExecutionDetails[executionIdOne], "Execution Size One");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdTwo), "Execution ID Two");
            Assert.AreEqual(-executionSizeTwo, trade.ExecutionDetails[executionIdTwo], "Execution Size Two");

            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 21, 09), trade.CompletionTime, "Trade Completion Time");

            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecution_UpdateTradeBuySide_TradeIsCompleteWithUnusedExecutionSize()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Buy, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionId = "1";
            int executionSize = 42;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionId, executionSize, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");
            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionId), "Execution ID");
            Assert.AreEqual(-40, trade.ExecutionDetails[executionId], "Execution Size");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 59), trade.CompletionTime, "Trade Completion Time");
            Assert.AreEqual(2, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionMultiple_UpdateTradeBuySide_TradeIsCompleteWithUnusedExecutionSize()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Buy, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionIdOne = "1";
            int executionSizeOne = 13;

            // Add new execution
            trade.Add(executionIdOne, executionSizeOne, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            string executionIdTwo = "2";
            int executionSizeTwo = 28;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionIdTwo, executionSizeTwo, 102.5M, new DateTime(2015, 01, 21, 18, 21, 09));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdOne), "Execution ID One");
            Assert.AreEqual(-executionSizeOne, trade.ExecutionDetails[executionIdOne], "Execution Size One");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdTwo), "Execution ID Two");
            Assert.AreEqual(-27, trade.ExecutionDetails[executionIdTwo], "Execution Size Two");

            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 21, 09), trade.CompletionTime, "Trade Completion Time");

            Assert.AreEqual(1, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecution_UpdateTradeSellSide_TradeIsNotComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Sell, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionId = "1";
            int executionSize = 13;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionId, executionSize, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            Assert.IsTrue(!trade.IsComplete(), "Trade.IsComplete");
            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionId), "Execution ID");
            Assert.AreEqual(executionSize, trade.ExecutionDetails[executionId], "Execution Size");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(0001, 01, 01, 00, 00, 00), trade.CompletionTime, "Trade Completion Time");
            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionMultiple_UpdateTradeSellSide_TradeIsNotComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Sell, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionIdOne = "1";
            int executionSizeOne = 13;

            // Add new execution
            trade.Add(executionIdOne, executionSizeOne, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            string executionIdTwo = "2";
            int executionSizeTwo = 26;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionIdTwo, executionSizeTwo, 102.5M, new DateTime(2015, 01, 21, 18, 21, 09));

            Assert.IsTrue(!trade.IsComplete(), "Trade.IsComplete");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdOne), "Execution ID One");
            Assert.AreEqual(executionSizeOne, trade.ExecutionDetails[executionIdOne], "Execution Size One");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdTwo), "Execution ID Two");
            Assert.AreEqual(executionSizeTwo, trade.ExecutionDetails[executionIdTwo], "Execution Size Two");

            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");

            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecution_UpdateTradeSellSide_TradeIsComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Sell, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionId = "1";
            int executionSize = 40;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionId, executionSize, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");
            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionId), "Execution ID");
            Assert.AreEqual(executionSize, trade.ExecutionDetails[executionId], "Execution Size");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 59), trade.CompletionTime, "Trade Completion Time");
            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionMultiple_UpdateTradeSellSide_TradeIsComplete()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Sell, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionIdOne = "1";
            int executionSizeOne = 13;

            // Add new execution
            trade.Add(executionIdOne, executionSizeOne, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            string executionIdTwo = "2";
            int executionSizeTwo = 27;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionIdTwo, executionSizeTwo, 102.4M, new DateTime(2015, 01, 21, 18, 21, 09));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdOne), "Execution ID One");
            Assert.AreEqual(executionSizeOne, trade.ExecutionDetails[executionIdOne], "Execution Size One");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdTwo), "Execution ID Two");
            Assert.AreEqual(executionSizeTwo, trade.ExecutionDetails[executionIdTwo], "Execution Size Two");

            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 21, 09), trade.CompletionTime, "Trade Completion Time");

            Assert.AreEqual(0, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecution_UpdateTradeSellSide_TradeIsCompleteWithUnusedExecutionSize()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Sell, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionId = "1";
            int executionSize = 44;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionId, executionSize, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");
            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionId), "Execution ID");
            Assert.AreEqual(40, trade.ExecutionDetails[executionId], "Execution Size");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 59), trade.CompletionTime, "Trade Completion Time");
            Assert.AreEqual(4, unusedExecutionSize, "UnusedExecution Size");
        }

        [Test]
        [Category("Unit")]
        public void NewExecutionMultiple_UpdateTradeSellSide_TradeIsCompleteWithUnusedExecutionSize()
        {
            // Create New Trade Object
            Trade trade = new Trade(TradeSide.Sell, 40, 103, OrderExecutionProvider.Simulated, "0", new Security() { Symbol = "GOOG" }, new DateTime(2015, 01, 21, 18, 20, 57));

            string executionIdOne = "1";
            int executionSizeOne = 13;

            // Add new execution
            trade.Add(executionIdOne, executionSizeOne, 102, new DateTime(2015, 01, 21, 18, 20, 59));

            string executionIdTwo = "2";
            int executionSizeTwo = 32;

            // Add new execution
            int unusedExecutionSize = trade.Add(executionIdTwo, executionSizeTwo, 102.5M, new DateTime(2015, 01, 21, 18, 21, 09));

            Assert.IsTrue(trade.IsComplete(), "Trade.IsComplete");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdOne), "Execution ID One");
            Assert.AreEqual(executionSizeOne, trade.ExecutionDetails[executionIdOne], "Execution Size One");

            Assert.IsTrue(trade.ExecutionDetails.ContainsKey(executionIdTwo), "Execution ID Two");
            Assert.AreEqual(27, trade.ExecutionDetails[executionIdTwo], "Execution Size Two");

            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 20, 57), trade.StartTime, "Trade Start Time");
            Assert.AreEqual(new DateTime(2015, 01, 21, 18, 21, 09), trade.CompletionTime, "Trade Completion Time");

            Assert.AreEqual(5, unusedExecutionSize, "UnusedExecution Size");
        }
    }
}
