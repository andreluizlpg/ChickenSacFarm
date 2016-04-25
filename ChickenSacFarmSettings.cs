using System.ComponentModel;
using System;
using Loki.Common;
using Loki;

namespace ChickenSacFarm
{
    /// <summary>Settings for the ExamplePlugin. </summary>
    public class ChickenSacFarmSettings : JsonSettings
    {
        private static ChickenSacFarmSettings _instance;

        private string _fragDusk;
        private string _fragDawn;
        private string _fragNoon;
        private string _fragMidnight;
        private string _vaalGems;

        /// <summary>The current instance for this class. </summary>
        public static ChickenSacFarmSettings Instance
        {
            get { return _instance ?? (_instance = new ChickenSacFarmSettings()); }
        }

        /// <summary>The default ctor. Will use the settings path "ExamplePlugin".</summary>
        public ChickenSacFarmSettings()
            : base(GetSettingsFilePath(Configuration.Instance.Name, string.Format("{0}.json", "ChickenSacFarm")))
        {
            // TODO: Setup defaults here if needed for properties that don't support DefaultValue.
            _fragDusk = "0";
            _fragDawn = "0";
            _fragNoon = "0";
            _fragMidnight = "0";
            _vaalGems = "0";
        }
        public void IncreaseDusk()
        {
            int num = Convert.ToInt32(fragDusk);
            num += 1;
            this.fragDusk = num.ToString();
        }
        public void IncreaseDawn()
        {
            int num = Convert.ToInt32(fragDawn);
            num += 1;
            this.fragDawn = num.ToString();
        }
        public void IncreaseNoon()
        {
            int num = Convert.ToInt32(fragNoon);
            num += 1;
            this.fragNoon = num.ToString();
        }
        public void IncreaseMidnight()
        {
            int num = Convert.ToInt32(fragMidnight);
            num += 1;
            this.fragMidnight = num.ToString();
        }
        public void IncreaseGems()
        {
            int num = Convert.ToInt32(vaalGems);
            num += 1;
            this.vaalGems = num.ToString();
        }
        

        [DefaultValue("0")]
        public string fragDusk
        {
            get { return _fragDusk; }
            set
            {
                if (value.Equals(_fragDusk))
                {
                    return;
                }
                _fragDusk = value;
                NotifyPropertyChanged(() => fragDusk);
            }
        }
        [DefaultValue("0")]
        public string fragDawn
        {
            get { return _fragDawn; }
            set
            {
                if (value.Equals(_fragDawn))
                {
                    return;
                }
                _fragDawn = value;
                NotifyPropertyChanged(() => fragDawn);
            }
        }
        [DefaultValue("0")]
        public string fragNoon
        {
            get { return _fragNoon; }
            set
            {
                if (value.Equals(_fragNoon))
                {
                    return;
                }
                _fragNoon = value;
                NotifyPropertyChanged(() => fragNoon);
            }
        }
        [DefaultValue("0")]
        public string fragMidnight
        {
            get { return _fragMidnight; }
            set
            {
                if (value.Equals(_fragMidnight))
                {
                    return;
                }
                _fragMidnight = value;
                NotifyPropertyChanged(() => fragMidnight);
            }
        }
        [DefaultValue("0")]
        public string vaalGems
        {
            get { return _vaalGems; }
            set
            {
                if (value.Equals(_vaalGems))
                {
                    return;
                }
                _vaalGems = value;
                NotifyPropertyChanged(() => vaalGems);
            }
        }
    }
}