using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.SaveSystem;
using UnityEngine;

namespace FarmGame.TimeSystem {
    public class TimeManager : MonoBehaviour, ISavable {
        public event EventHandler<TimeEventArgs> OnClockProgress, OnDayProgress;

        [SerializeField] private TimeSpan _currentTime;
        [SerializeField] private float _passedTime = 0;
        [SerializeField] private int _wakeUpTime = 6;

        private GameCalendar _calendar;

        //1 second = 10 in-game minutes
        private static int REAL_TO_GAME_TIME_RATIO = 1;

        public int SaveID => SaveIDRepository.TIME_MANAGER_ID;

        private void Start() {
            if (_calendar == null) {
                _calendar = new GameCalendar();
                _calendar.OnSeasonChange += (seasonIndex) => SendDayUpdateEvent(true);
                _currentTime = new TimeSpan(_currentTime.Days, 6, 0, 0); //start new day at 6:00
            }

            SendTimeUpdateEvent();
        }

        private void SendTimeUpdateEvent() {
            OnClockProgress?.Invoke(this, new TimeEventArgs(false, _calendar, _currentTime, true));
        }

        private void SendDayUpdateEvent(bool seasonChanged, bool theSameDay = false) {
            OnDayProgress?.Invoke(this, new TimeEventArgs(seasonChanged, _calendar, _currentTime, theSameDay));
        }

        public void GoToNextDay() {
            bool theSameDay = false;
            int oldSeasonIndex = _calendar.Season;
            int tempWakeUpHour = _wakeUpTime;
            if (_currentTime.Hours >= 0 && _currentTime.Hours < _wakeUpTime) {
                tempWakeUpHour += _currentTime.Hours;
                theSameDay = true;
            } else {
                _calendar.ProgressTime();
            }

            _currentTime = new TimeSpan(_calendar.Day, tempWakeUpHour, 0, 0);
            _passedTime = 0;
            SendDayUpdateEvent(oldSeasonIndex != _calendar.Season, theSameDay);
            SendTimeUpdateEvent();
            Debug.Log(ToString());
        }

        public override string ToString() {
            return $"Season: {CalendarNamesHelper.GetSeasonName(_calendar.Season)}, Day: {_calendar.Day}, WeekDay: {CalendarNamesHelper.GetWeekDayName(_calendar.WeekDay)}, Year: {_calendar.Year}";
        }

        private void Update() {
            if (_passedTime < REAL_TO_GAME_TIME_RATIO) {
                _passedTime += Time.deltaTime;
            } else {
                _passedTime = _passedTime - REAL_TO_GAME_TIME_RATIO;
                _currentTime = _currentTime.Add(new TimeSpan(0, 10, 0));
                if (_currentTime.Hours == 0 && _currentTime.Minutes == 0) {
                    int oldSeasonIndex = _calendar.Season;
                    _calendar.ProgressTime();
                    SendDayUpdateEvent(oldSeasonIndex != _calendar.Season, true);
                }
                SendTimeUpdateEvent();
            }
        }

        public string GetData() {
            return $"{_calendar.GetSaveData()},{_currentTime.Hours},{_currentTime.Minutes},{Mathf.RoundToInt(_passedTime)}";
        }

        public void RestoreData(string data) {
            if (string.IsNullOrEmpty(data)) {
                _currentTime = new TimeSpan(_currentTime.Days, 6, 0, 0); //start new day at 6:00
                _calendar = new();
                _calendar.OnSeasonChange += (seasonIndex) => SendDayUpdateEvent(true);
                return;
            }

            string[] loadedData = data.Split(',');
            if (loadedData.Length > 0) {
                _calendar = new GameCalendar(int.Parse(loadedData[0]), int.Parse(loadedData[1]), int.Parse(loadedData[2]));
                _currentTime = new TimeSpan(_currentTime.Days, int.Parse(loadedData[3]), int.Parse(loadedData[4]), 0);
                _passedTime = int.Parse(loadedData[5]);
                _calendar.OnSeasonChange += (seasonIndex) => SendDayUpdateEvent(true);

                SendTimeUpdateEvent();
            } else {

                _currentTime = new TimeSpan(_currentTime.Days, 6, 0, 0); //start new day at 6:00
                _calendar = new();
                _calendar.OnSeasonChange += (seasonIndex) => SendDayUpdateEvent(true);
                return;
            }
        }

        public class TimeEventArgs : EventArgs {

            public bool SeasonChanged { get; private set; }
            public int CurrentDay { get; private set; }
            public int WeekDay { get; private set; }
            public int CurrentSeason { get; private set; }
            public TimeSpan CurrentTime { get; private set; }
            public bool TheSameDay { get; private set; }
            public TimeEventArgs(bool seasonChanged, int currentDay, int weekDay, int currentSeason, TimeSpan currentTime, bool theSameDay) {
                SeasonChanged = seasonChanged;
                CurrentDay = currentDay;
                WeekDay = weekDay;
                CurrentSeason = currentSeason;
                CurrentTime = currentTime;
                TheSameDay = theSameDay;
            }

            public TimeEventArgs(bool seasonChanged, GameCalendar gameCalendar, TimeSpan currentTime, bool theSameDay) {
                SeasonChanged = seasonChanged;
                CurrentDay = gameCalendar.Day;
                WeekDay = gameCalendar.WeekDay;
                CurrentSeason = gameCalendar.Season;
                CurrentTime = currentTime;
                TheSameDay = theSameDay;
            }
        }
    }
}
