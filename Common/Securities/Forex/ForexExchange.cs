/*
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

/**********************************************************
* USING NAMESPACES
**********************************************************/

using System;

namespace QuantConnect.Securities.Forex 
{
    /******************************************************** 
    * CLASS DEFINITIONS
    *********************************************************/
    /// <summary>
    /// Forex exchange class - information and helper tools for forex exchange properties
    /// </summary>
    /// <seealso cref="SecurityExchange"/>
    public class ForexExchange : SecurityExchange {

        /******************************************************** 
        * CLASS VARIABLES
        *********************************************************/
        private TimeSpan _marketOpen = TimeSpan.FromHours(0);
        private TimeSpan _marketClose = TimeSpan.FromHours(23.999999);

        /******************************************************** 
        * CLASS CONSTRUCTION
        *********************************************************/
        /// <summary>
        /// Initialise forex exchange exchange
        /// </summary>
        public ForexExchange() : 
            base() {
        }

        /******************************************************** 
        * CLASS PROPERTIES
        *********************************************************/
        /// <summary>
        /// Override the base ExchangeOpen property with FXCM Market Hours
        /// </summary>
        public override bool ExchangeOpen
        {
            get
            {
                return DateTimeIsOpen(Time);
            }
        }


        /// <summary>
        /// Number of trading days per year for this security, used for performance statistics.
        /// </summary>
        public override int TradingDaysPerYear
        {
            get
            {
                // 365 - Saturdays = 313;
                return 313;
            }
        }


        /// <summary>
        /// Check this date time is open for the forex market.
        /// </summary>
        /// <param name="dateToCheck">time of day</param>
        /// <returns>true if open</returns>
        public override bool DateTimeIsOpen(DateTime dateToCheck)
        {
            if (!DateIsOpen(dateToCheck))
                return false;

            if (dateToCheck.DayOfWeek == DayOfWeek.Friday && dateToCheck.TimeOfDay.TotalHours >= 16)
                return false;

            if (dateToCheck.DayOfWeek == DayOfWeek.Sunday && dateToCheck.TimeOfDay.TotalHours < 17)
                return false;

            return true;
        }


        /// <summary>
        /// Check if this datetime is open for the FXCM markets:
        /// </summary>
        /// <param name="dateToCheck">Datetime date to analyse</param>
        /// <returns>Boolean true if market is open</returns>
        public override bool DateIsOpen(DateTime dateToCheck)
        {
            //FXCM closed on Saturday
            if (dateToCheck.DayOfWeek == DayOfWeek.Saturday)
                return false;

            //Otherwise all other days at least partially open
            return true;
        }


        /// <summary>
        /// FOREX market opening time: midnight on week days, 5pm on Sunday
        /// </summary>
        public override TimeSpan MarketOpen
        {
            get { return _marketOpen; }
            set { _marketOpen = value; }
        }

        /// <summary>
        /// FOREX market closing time: officially no closing time the FX markets are open over midnight.
        /// </summary>
        public override TimeSpan MarketClose
        {
            get { return _marketClose; }
            set { _marketClose = value; }
        }

        /******************************************************** 
        * CLASS METHODS
        *********************************************************/

    } //End of ForexExchange

} //End Namespace