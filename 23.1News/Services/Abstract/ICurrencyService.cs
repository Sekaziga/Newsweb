﻿using _23._1News.Models.Db;

namespace _23._1News.Services.Abstract
{
    public interface ICurrencyService
    {
        Task<string> GetSpotRateAsync();

        //void SaveData(TodaysSpotRate todaysSpotRate);

        Task<SpotRate> GetRate();
    }
}