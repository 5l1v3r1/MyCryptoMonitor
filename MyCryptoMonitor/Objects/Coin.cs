﻿namespace MyCryptoMonitor.Objects
{
    public class Coin
    {
        #region Public Properties

        public decimal Change1HourPercent { get; set; }
        public decimal Change24HourPercent { get; set; }
        public decimal Change7DayPercent { get; set; }
        public int Index { get; set; }
        public string LongName { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Price { get; set; }
        public string ShortName { get; set; }
        public decimal Supply { get; set; }

        #endregion Public Properties
    }
}