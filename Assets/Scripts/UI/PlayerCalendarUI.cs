using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.TimeSystem;
using TMPro;
using UnityEngine;

namespace FarmGame.UI {
    public class PlayerCalendarUI : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _seasonText, _dayText, _timeTxt;
        private TimeManager _timeManager;

        private void OnEnable() {
            _timeManager = FindObjectOfType<TimeManager>(true);
            Debug.Assert(_timeManager != null, "TimeManager not found");
            _timeManager.OnClockProgress += UpdateAllUIElement;
        }

        private void UpdateAllUIElement(object sender, TimeManager.TimeEventArgs timeArgs) {
            UpdateTime(timeArgs.CurrentTime);
            UpdateDay(timeArgs.CurrentDay, timeArgs.WeekDay);
            UpdateSeason(timeArgs.CurrentSeason);
        }

        private void UpdateSeason(int currentSeason) {
            _seasonText.text = CalendarNamesHelper.GetSeasonName(currentSeason);
        }

        private void UpdateDay(int currentDay, int weekDay) {
            _dayText.text = $"{currentDay} ({CalendarNamesHelper.GetWeekDayName(weekDay)})";
        }

        private void UpdateTime(TimeSpan currentTime) {
            _timeTxt.text = currentTime.ToString(@"hh\:mm");
        }


        private void OnDisable() {
            _timeManager.OnClockProgress -= UpdateAllUIElement;
        }
    }
}
