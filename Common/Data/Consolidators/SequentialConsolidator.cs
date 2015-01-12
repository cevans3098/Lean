﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;

namespace QuantConnect.Data.Consolidators
{
    /// <summary>
    /// This consolidator wires up the events on its First and Second consolidators
    /// such that data flows from the First to Second consolidator. It's output comes
    /// from the Second.
    /// </summary>
    public class SequentialConsolidator : IDataConsolidator
    {
        /// <summary>
        /// Gets the first consolidator to receive data
        /// </summary>
        public IDataConsolidator First
        {
            get; private set;
        }

        /// <summary>
        /// Gets the second consolidator that ends up receiving data produced
        /// by the first
        /// </summary>
        public IDataConsolidator Second
        {
            get; private set;
        }

        /// <summary>
        /// Gets the most recently consolidated piece of data. This will be null if this consolidator
        /// has not produced any data yet.
        /// 
        /// For a SequentialConsolidator, this is the output from the 'Second' consolidator.
        /// </summary>
        public BaseData Consolidated
        {
            get { return Second.Consolidated; }
        }

        /// <summary>
        /// Gets the type consumed by this consolidator
        /// </summary>
        public Type InputType
        {
            get { return First.InputType; }
        }

        /// <summary>
        /// Gets the type produced by this consolidator
        /// </summary>
        public Type OutputType
        {
            get { return Second.OutputType; }
        }

        /// <summary>
        /// Updates this consolidator with the specified data
        /// </summary>
        /// <param name="data">The new data for the consolidator</param>
        public void Update(BaseData data)
        {
            First.Update(data);
        }

        /// <summary>
        /// Event handler that fires when a new piece of data is produced
        /// </summary>
        public event DataConsolidatedHandler DataConsolidated;

        /// <summary>
        /// Creates a new consolidator that will pump date through the first, and then the output
        /// of the first into the second. This enables 'wrapping' or 'composing' of consolidators
        /// </summary>
        /// <param name="first">The first consolidator to receive data</param>
        /// <param name="second">The consolidator to receive first's output</param>
        public SequentialConsolidator(IDataConsolidator first, IDataConsolidator second)
        {
            if (first.OutputType != second.InputType)
            {
                throw new ArgumentException("first.OutputType must equal second.OutputType!");
            }
            First = first;
            Second = second;

            // wire up the second one to get data from the first
            first.DataConsolidated += (sender, consolidated) => second.Update(consolidated);
        }
    }
}