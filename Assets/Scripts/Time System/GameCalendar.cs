using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.TimeSystem {
    public class GameCalendar {

        private static int DAY_IN_MONTH = 30;
        private static int SEASON_IN_YEAR = 4;
        public int Season { get; private set; }
        public int Day { get; private set; }
        public int WeekDay { get; private set; }
        public int Year { get; private set; }
        public event Action<int> OnSeasonChange;

        public GameCalendar() {
            Season = 0;
            Day = 1;
            WeekDay = GetWeekDay(Season, Day);
            Year = 1;
        }

        public GameCalendar(int year, int season, int day) {
            Season = season;
            Day = day;
            WeekDay = GetWeekDay(Season, Day);
            Year = year;
        }

        private int GetWeekDay(int season, int day) {
            int yearDay = (Year * SEASON_IN_YEAR + season) * DAY_IN_MONTH + day;
            return yearDay % 7;
        }

        public void ProgressTime() {
            Day++;
            if (Day > DAY_IN_MONTH) {
                Day = 1;
                Season++;
                if (Season >= SEASON_IN_YEAR) {
                    Season = 0;
                    Year++;
                }
                OnSeasonChange?.Invoke(Year);
            }
            WeekDay = GetWeekDay(Season, Day);
        }

        public string GetSaveData() {
            return $"{Year},{Season},{Day}";
        }
    }
}
